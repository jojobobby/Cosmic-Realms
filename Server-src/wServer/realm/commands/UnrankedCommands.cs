using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using common;
using common.resources;
using TagLib;
using wServer.networking;
using wServer.realm.entities;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using File = TagLib.File;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking;
using wServer.realm.entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using common;
using common.resources;
using log4net;
using wServer.logic.loot;
using wServer.networking;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.entities.vendors;
using wServer.realm.terrain;
using wServer.realm.worlds.logic;
using DiscordWebhook;

namespace wServer.realm.commands
{
    class JoinGuildCommand : Command
    {
        public JoinGuildCommand() : base("join") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.ProcessPacket(new JoinGuild()
            {
                GuildName = args
            });
            return true;
        }
    }

    class TutorialCommand : Command
    {
        public TutorialCommand() : base("tutorial") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Tutorial,
                Name = "Tutorial"
            });
            return true;
        }
    }

    class ServerCommand : Command
    {
        public ServerCommand() : base("world") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo($"[{player.Owner.Id}] {player.Owner.GetDisplayName()} ({player.Owner.Players.Count} players)");
            return true;
        }
    }

    class PauseCommand : Command
    {
        public PauseCommand() : base("pause") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var owner = player.Owner;
            if (player.SpectateTarget != null)
            {
                player.SendError("The use of pause is disabled while spectating.");
                return false;
            }

            if (!owner.SafeMapArea)
            {
                player.SendError("This action has been reported to the mods, one day suspension for pausing in a dungeon.");
                return false;
            }

            if (player.HasConditionEffect(ConditionEffects.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
                return true;
            }

           
            if (owner != null && (owner is Arena || owner is ArenaSolo || owner is DeathArena))
            {
                player.SendInfo("Can't pause in arena.");
                return false;
            }

            if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
            {
                player.SendError("Not safe to pause.");
                return false;
            }

            player.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Paused,
                DurationMS = -1
            });
            player.SendInfo("Game paused.");
            return true;
        }
    }

    /// <summary>
    /// This introduces a subtle bug, since the client UI is not notified when a /teleport is typed, it's cooldown does not reset.
    /// This leads to the unfortunate situation where the cooldown has been not been reached, but the UI doesn't know. The graphical TP will fail
    /// and cause it's timer to reset. NB: typing /teleport will workaround this timeout issue.
    /// 
    /// then just send the packet...
    /// </summary>
     class TeleportCommand : Command
    {
        public TeleportCommand() : base("tp", alias: "teleport") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var target = player.IsControlling ? player.SpectateTarget : player;
            foreach (var i in player.Owner.Players.Values)
            {
                var targetAccount = i.Manager.Database.GetAccount(i.Id);
                if (!i.Name.EqualsIgnoreCase(args))
                    continue;
                if (i.Id == player.Id) {
                    player.SendError($"Cannot teleport to self!");
                    return false;
                }
                if (targetAccount != null)
                {
                    if (targetAccount.Hidden)
                    {
                        player.SendError($"Unable to find player: {args}");
                        return false;
                    }
                }
                player.Teleport(time, i.Id, true);
                return true;
            }

            player.SendError($"Unable to find player: {args}");
            return false;
        }
    }

    class DungeonAccept : Command
    {
        public DungeonAccept() : base("daccept", alias: "da") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int id;
            try
            {
                id = int.Parse(args);
            }
            catch (Exception)
            {
                player.SendError("ID must be a number.");
                return false;
            }
            var world = player.Manager.GetWorld(id);
            if (world != null)
            {
                if (world.PlayerDungeon && world.Invites.Contains(player.Name.ToLower()))
                {
                    if (world.GetAge() > 90000)
                    {
                        player.SendError("The invite has expired.");
                        return false;
                    }
                    else
                    {
                        world.Invites.Remove(player.Name.ToLower());
                        player.Client.Reconnect(new Reconnect()
                        {
                            Host = "",
                            Port = 2050,
                            GameId = world.Id,
                            Name = world.SBName != null ? world.SBName : world.Name,
                        });
                        return true;
                    }
                }
                else if (world.PlayerDungeon && world.Invited.Contains(player.Name.ToLower()))
                {
                    player.SendError("You have already entered " + world.GetDisplayName() + ".");
                    return false;
                }
                else
                {
                    player.SendError("You were not invited to join " + world.GetDisplayName() + ".");
                    return false;
                }
            }
            else
            {
                player.SendError("The world was not found.");
                return false;
            }
        }
    }

    class DungeonInvite : Command
    {
        public DungeonInvite() : base("dinvite", alias: "di") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            if (!(player.Owner.PlayerDungeon && player.Owner.Opener.Equals(player.Name))) {
                player.SendError("This is not your dungeon!");
                return false;
            }
            else if (player.Owner.GetAge() > 90000)
            {
                player.SendError("It's too late to invite players!");
                return false;
            }

            HashSet<string> invited = new HashSet<string>();
            HashSet<string> missed = new HashSet<string>();
            HashSet<string> unable = new HashSet<string>();

            if (args.Contains("-g"))
            {
                foreach (var i in player.Manager.Clients.Keys
                    .Where(x => x.Player != null)
                    .Where(x => !x.Account.IgnoreList.Contains(player.AccountId))
                    .Where(x => x.Account.GuildId > 0)
                    .Where(x => x.Account.GuildId == player.Client.Account.GuildId)
                    .Select(x => x.Player))
                {
                    if (i.Name.EqualsIgnoreCase(player.Name)) continue;

                    // already in the dungeon
                    if (i.Owner.Id == player.Owner.Id)
                    {
                        unable.Add(i.Name);
                        player.Owner.Invited.Add(i.Name.ToLower());
                        continue;
                    }

                    if (player.Owner.Invited.Contains(i.Name.ToLower()))
                    {
                        unable.Add(i.Name);
                    }
                    else if (player.Manager.Chat.Invite(player, i.Name, player.Owner.GetDisplayName(), player.Owner.Id))
                    {
                        player.Owner.Invited.Add(i.Name.ToLower());
                        player.Owner.Invites.Add(i.Name.ToLower());
                        invited.Add(i.Name);
                    }
                    else
                    {
                        missed.Add(i.Name);
                    }
                }

                if (invited.Count > 0)
                {
                    player.SendInfo("Invited: " + string.Join(", ", invited));
                }
                if (unable.Count > 0)
                {
                    player.SendInfo("Already invited: " + string.Join(", ", unable));
                }
                if (missed.Count > 0)
                {
                    player.SendInfo("Not found: " + string.Join(", ", missed));
                }
                return true;
            }

            var players = args.Split(' ').Where(n => !n.Equals("")).ToArray();

            if (players.Length > 0)
            {
                foreach (string p in players)
                {
                    if (p.EqualsIgnoreCase(player.Name)) continue;

                    if (player.Owner.Invited.Contains(p.ToLower()))
                    {
                        unable.Add(p);
                    }
                    else if (player.Manager.Chat.Invite(player, p, player.Owner.GetDisplayName(), player.Owner.Id))
                    {
                        player.Owner.Invited.Add(p.ToLower());
                        player.Owner.Invites.Add(p.ToLower());
                        invited.Add(p);
                    }
                    else
                    {
                        missed.Add(p);
                    }
                }
                if (invited.Count > 0)
                {
                    player.SendInfo("Invited: " + string.Join(", ", invited));
                }
                if (unable.Count > 0)
                {
                    player.SendInfo("Already invited: " + string.Join(", ", unable));
                }
                if (missed.Count > 0)
                {
                    player.SendInfo("Not found: " + string.Join(", ", missed));
                }
                return true;
            }
            else 
            {
                player.SendError("Specify some players to invite!");
                return false;
            }
        }
    }

    class TellCommand : Command
    {
        public TellCommand() : base("tell", alias: "t") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not tell at this time.");
                return false;
            }

            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args.Substring(0, index);
            string msg = args.Substring(index + 1);

            if (player.Name.ToLower() == playername.ToLower())
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            if (!player.Manager.Chat.Tell(player, playername, msg))
            {
                player.SendError(string.Format("{0} not found.", playername));
                return false;
            }
            return true;
        }
    }
    internal class MarketCommand : Command
    {
        public MarketCommand() : base("market") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            return true;
        }
    }
    internal class FindCommand : Command
    {
        public FindCommand() : base("find") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
                var x = player;
            if (x.Client.Account.Name == args && x != null)

                if (x.Rank < 70)
                {
                    if (player.Rank >= 70)
                    {
                        player.SendInfo("Found " + x);
                        player.SendInfo("World: " + x.Owner.Name);
                        player.SendInfo("Level: " + x.Level);
                        player.SendInfo("Fame: " + x.Fame);
                        player.SendInfo("Gold: " + x.Credits);
                        player.SendInfo("AccountID: " + x.AccountId);
                        return true;
                    }
                    else
                    {
                        player.SendInfo("Found player!");
                        player.SendInfo("World: " + x.Owner.Name);
                        player.SendInfo("Level: " + x.Level);
                        return true;
                    }

                }
            player.SendInfo("Player not online!");
            return false;
        }
    }
    internal class MscaleCommand : Command
    {
        public MscaleCommand() : base("mscale") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            return true;
        }
    }

    class PublicChatCommand : Command
    {
        public PublicChatCommand() : base("PublicChat", alias: "pc") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not chat at this time.");
                return false;
            }

            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /pc <text>");
                return false;
            }

            return true;
        }
    }

    class GCommand : Command
    {
        public GCommand() : base("g", alias: "guild") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not guild chat at this time.");
                return false;
            }

            if (String.IsNullOrEmpty(player.Guild))
            {
                player.SendError("You need to be in a guild to guild chat.");
                return false;
            }

            return player.Manager.Chat.Guild(player, args);
        }
    }

    class LocalCommand : Command
    {
        public LocalCommand() : base("l") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }

            if (player.Muted)
            {
                player.SendError("Muted. You can not local chat at this time.");
                return false;
            }

            if (player.CompareAndCheckSpam(args, time.TotalElapsedMs))
            {
                return false;
            }

            var sent = player.Manager.Chat.Local(player, args);
            if (!sent)
            {
                player.SendError("Failed to send message. Use of extended ascii characters and ascii whitespace (other than space) is not allowed.");
            }
            else
            {
                player.Owner.ChatReceived(player, args);
            }

            

            return sent;
        }
    }

    class HelpCommand : Command
    {
        //actually the command is 'help', but /help is intercepted by client
        public HelpCommand() : base("commands") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            StringBuilder sb = new StringBuilder("Available commands: ");
            var cmds = player.Manager.Commands.Commands.Values.Distinct()
                .Where(x => x.HasPermission(player) && x.ListCommand)
                .ToArray();
            Array.Sort(cmds, (c1, c2) => c1.CommandName.CompareTo(c2.CommandName));
            for (int i = 0; i < cmds.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(cmds[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    class IgnoreCommand : Command
    {
        public IgnoreCommand() : base("ignore") { }

        protected override bool Process(Player player, RealmTime time, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /ignore <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("Can't ignore yourself!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.IgnoreAccount(srcAccount, targetAccount, true);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 1, // ignore list
                AccountIds = srcAccount.IgnoreList
                    .Select(i => i.ToString())
                    .ToArray()
            });

            player.SendInfo(playerName + " has been added to your ignore list.");
            return true;
        }
    }

    class UnignoreCommand : Command
    {
        public UnignoreCommand() : base("unignore") { }

        protected override bool Process(Player player, RealmTime time, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /unignore <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("You are no longer ignoring yourself. Good job.");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.IgnoreAccount(srcAccount, targetAccount, false);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 1, // ignore list
                AccountIds = srcAccount.IgnoreList
                    .Select(i => i.ToString())
                    .ToArray()
            });

            player.SendInfo(playerName + " no longer ignored.");
            return true;
        }
    }

    class LockCommand : Command
    {
        public LockCommand() : base("lock") { }

        protected override bool Process(Player player, RealmTime time, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /lock <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("Can't lock yourself!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.LockAccount(srcAccount, targetAccount, true);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 0, // locked list
                AccountIds = player.Client.Account.LockList
                    .Select(i => i.ToString())
                    .ToArray(),
                LockAction = 1
            });

            player.SendInfo(playerName + " has been locked.");
            return true;
        }
    }

    class UnlockCommand : Command
    {
        public UnlockCommand() : base("unlock") { }

        protected override bool Process(Player player, RealmTime time, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (String.IsNullOrEmpty(playerName))
            {
                player.SendError("Usage: /unlock <player name>");
                return false;
            }

            if (player.Name.ToLower() == playerName.ToLower())
            {
                player.SendInfo("You are no longer locking yourself. Nice!");
                return false;
            }

            var target = player.Manager.Database.ResolveId(playerName);
            var targetAccount = player.Manager.Database.GetAccount(target);
            var srcAccount = player.Client.Account;

            if (target == 0 || targetAccount == null || targetAccount.Hidden && player.Admin == 0)
            {
                player.SendError("Player not found.");
                return false;
            }

            player.Manager.Database.LockAccount(srcAccount, targetAccount, false);

            player.Client.SendPacket(new AccountList()
            {
                AccountListId = 0, // locked list
                AccountIds = player.Client.Account.LockList
                    .Select(i => i.ToString())
                    .ToArray(),
                LockAction = 0
            });

            player.SendInfo(playerName + " no longer locked.");
            return true;
        }
    }

    class UptimeCommand : Command
    {
        public UptimeCommand() : base("uptime") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(time.TotalElapsedMs);

            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);

            player.SendInfo("The server has been up for " + answer + ".");
            return true;
        }
    }

  /*  class NpeCommand : Command
    {
        public NpeCommand() : base("npe") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Stats[0] = 100;
            player.Stats[1] = 100;
            player.Stats[2] = 10;
            player.Stats[3] = 0;
            player.Stats[4] = 10;
            player.Stats[5] = 10;
            player.Stats[6] = 10;
            player.Stats[7] = 10;
            player.Level = 1;
            player.Experience = 0;
            
            player.SendInfo("You character stats, level, and experience has be npe'ified.");
            return true;
        }
    }
    */
    class PositionCommand : Command
    {
        public PositionCommand() : base("pos", alias: "position") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo("Current Position: " + (int)player.X + ", " + (int)player.Y);
            return true;
        }
    }

    class TradeCommand : Command
    {
        public TradeCommand() : base("trade") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (String.IsNullOrWhiteSpace(args))
            {
                player.SendError("Usage: /trade <player name>");
                return false;
            }

            player.RequestTrade(args);
            return true;
        }
    }

  
    class ArenaCommand : Command
    {
        public ArenaCommand() : base("arena") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Arena,
                Name = "Arena"
            });
            return true;
        }
    }

    /*class DeathArenaCommand : Command
    {
        public DeathArenaCommand() : base("oa") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.DeathArena,
                Name = "Oryx's Arena"
            });
            return true;
        }
    }*/

    /*
    class RealmCommand : Command
    {
        public RealmCommand() : base("realm") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Realm,
                Name = "Realm"
            });
            return true;
        }
    }
    */
    /*  class NexusCommand : Command
      {
          public NexusCommand() : base("nexus") { }

          protected override bool Process(Player player, RealmTime time, string args)
          {
              player.Client.Reconnect(new Reconnect()
              {
                  Host = "",
                  Port = 2050,
                  GameId = World.Nexus,
                  Name = "Nexus"
              });
              return true;
          }
      }
      */
    class DailyQuestCommand : Command
      {
          public DailyQuestCommand() : base("dailyquest", alias: "dq") { }

          protected override bool Process(Player player, RealmTime time, string args)
          {
              player.Client.Reconnect(new Reconnect()
              {
                  Host = "",
                  Port = 2050,
                  GameId = World.Tinker,
                  Name = "Daily Quest Room"
              });
              return true;
          }
      }

     /* class VaultCommand : Command
      {
          public VaultCommand() : base("vault") { }

          protected override bool Process(Player player, RealmTime time, string args)
          {
              player.Client.Reconnect(new Reconnect()
              {
                  Host = "",
                  Port = 2050,
                  GameId = World.Vault,
                  Name = "Vault"
              });
              return true;
          }
      }
  */
    class SoloArenaCommand : Command
    {
        public SoloArenaCommand() : base("sa") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.ArenaSolo,
                Name = "Arena Solo"
            });
            return true;
        }
    }

    class GhallCommand : Command
    {
        public GhallCommand() : base("ghall") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.GuildRank == -1)
            {
                player.SendError("You need to be in a guild.");
                return false;
            }
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.GuildHall,
                Name = "Guild Hall"
            });
            return true;
        }
    }

    class LefttoMaxCommand : Command
    {
        public LefttoMaxCommand() : base("lefttomax") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];

            player.SendInfo($"HP: {pd.Stats[0].MaxValue - player.Stats.Base[0]}");
            player.SendInfo($"MP: {pd.Stats[1].MaxValue - player.Stats.Base[1]}");
            player.SendInfo($"Attack: {pd.Stats[2].MaxValue - player.Stats.Base[2]}");
            player.SendInfo($"Defense: {pd.Stats[3].MaxValue - player.Stats.Base[3]}");
            player.SendInfo($"Speed: {pd.Stats[4].MaxValue - player.Stats.Base[4]}");
            player.SendInfo($"Dexterity: {pd.Stats[5].MaxValue - player.Stats.Base[5]}");
            player.SendInfo($"Vitality: {pd.Stats[6].MaxValue - player.Stats.Base[6]}");
            player.SendInfo($"Wisdom: {pd.Stats[7].MaxValue - player.Stats.Base[7]}");
            player.SendInfo($"LootBoost: {50 - player.Stats.Base[10]}");
            player.SendInfo($"Critical Dmg: {pd.Stats[11].MaxValue - player.Stats.Base[11]}");
            player.SendInfo($"Critical Hit: {pd.Stats[12].MaxValue - player.Stats.Base[12]}");
            return true;
        }
    }
   
    class GLandCommand : Command
    {
        public GLandCommand() : base("gland", alias: "glands") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var target = player.IsControlling ? player.SpectateTarget : player;
            if (!(player.Owner is Realm))
            {
                player.SendError("This command requires you to be in realm first.");
                return false;
            }
            //if (!player.done_checking)
            //{
            //    player.SendError("You can't teleport right now.");
            //    return false;
            //}
            player.TeleportPosition(time, 1000 + 0.5f, 850 + 0.5f);
            return true;
        }
    }

    class WhoCommand : Command
    {
        public WhoCommand() : base("who") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var owner = player.Owner;
            var players = owner.Players.Values
                .Where(p => p.Client != null && p.CanBeSeenBy(player))
                .ToArray();

            var sb = new StringBuilder($"Players in current area ({owner.Players.Count}): ");
            for (var i = 0; i < players.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append(players[i].Name);
            }
            
            player.SendInfo(sb.ToString());
            return true;
        }
    }

    class OnlineCommand : Command
    {
        public OnlineCommand() : base("online") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var servers = player.Manager.InterServer.GetServerList();
            if (player.Client.Account.Rank < 70)
            {
                var players = 0;
                foreach (var server in servers)
                {
                    players += server.playerList.Count();
                }
                player.SendInfo($"Players online: {players}");
            }
            else { 
                var players =
                    (from server in servers
                     from plr in server.playerList
                     where !plr.Hidden || player.Rank < 70
                     select plr.Name)
                    .ToArray();
                var sb = new StringBuilder($"Players online ({players.Length}): ");
                for (var i = 0; i < players.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");
                    sb.Append(players[i]);
                }
                player.SendInfo(sb.ToString());
            }
            return true;
        }
    }

    class WhereCommand : Command
    {
        public WhereCommand() : base("where") { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendInfo("Usage: /where <player name>");
                return true;
            }

            var servers = player.Manager.InterServer.GetServerList();

            foreach (var server in servers)
                foreach (PlayerInfo plr in server.playerList)
                {
                    if (!plr.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ||
                        plr.Hidden && !player.Client.Account.Admin || player.Rank <= 70)
                        continue;

                    player.SendInfo($"{plr.Name} is playing on {server.name} at [{plr.WorldInstance}]{plr.WorldName}.");
                    return true;
                }

            var pId = player.Manager.Database.ResolveId(name);
            if (pId == 0)
            {
                player.SendInfo($"No player with the name {name}.");
                return true;
            }

            var acc = player.Manager.Database.GetAccount(pId, "lastSeen");
            if (acc.LastSeen == 0)
            {
                player.SendInfo($"{name} not online. Has not been seen since the dawn of time.");
                return true;
            }

            var dt = Utils.FromUnixTimestamp(acc.LastSeen);
            player.SendInfo($"{name} not online. Player last seen {Utils.TimeAgo(dt)}.");
            return true;
        }
    }
    
    
    class RemoveAccountOverrideCommand : Command
    {
        public RemoveAccountOverrideCommand() : base("removeOverride", 0, listCommand: false) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Client.Account;
            if (acc.AccountIdOverrider == 0)
            {
                player.SendError("Account isn't overridden.");
                return false;
            }

            var overriderAcc = player.Manager.Database.GetAccount(acc.AccountIdOverrider);
            if (overriderAcc == null)
            {
                player.SendError("Account not found!");
                return false;
            }

            overriderAcc.AccountIdOverride = 0;
            overriderAcc.FlushAsync();
            player.SendInfo("Account override removed.");
            return true;
        }
    }

    class CurrentSongCommand : Command
    {
        public CurrentSongCommand() : base("currentsong", alias: "song") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var properName = player.Owner.Music;
            var file = File.Create(Environment.CurrentDirectory + $"/resources/web/music/{properName}.mp3");
            var artist = file.Tag.FirstPerformer ?? "Unknown";
            var title = file.Tag.Title ?? properName;
            var album = file.Tag.Album != null ? $" from {file.Tag.Album}" : "";
            var filename = $" ({properName}.mp3)";
            
            player.SendInfo($"Current Song: {title} by {artist}{album}{filename}.");
            return true;
        }
    }
    internal class ServerChatCommand : Command
    {
        public ServerChatCommand() : base("sc", permLevel: 1) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            int x = 0;
            if (player.Muted)
                return false;

            if (args == string.Empty)
            {
                player.SendInfo("Usage: /sc <Your text>");
                return false;
            }
            player.Manager.Chat.ServerChat(player, "" + args);
            
            x++;
            player.Owner.Timers.Add(new WorldTimer(2000, (world, RealmTime) => x--));
            return true;
        }
    }
    internal class CFameCommand : Command
    {
        public CFameCommand() : base("myfame") { }


        protected override bool Process(Player player, RealmTime time, string args)
        {
            int Fame = player.Client.Character.FinalFame;
            player.SendInfo("You currently have " + Fame + " Fame.");
            return true;
        }
    }
    class GuildKickCommand : Command
    {
        public GuildKickCommand() : base("gkick") { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (player.Owner is Test)
                return false;

            var manager = player.Client.Manager;

            // if resigning
            if (player.Name.Equals(name))
            {
                // chat needs to be done before removal so we can use
                // srcPlayer as a source for guild info
                manager.Chat.Guild(player, player.Name + " has left the guild.", true);

                if (!manager.Database.RemoveFromGuild(player.Client.Account))
                {
                    player.SendError("Guild not found.");
                    return false;
                }

                player.Guild = "";
                player.GuildRank = 0;

                return true;
            }

            // get target account id
            var targetAccId = manager.Database.ResolveId(name);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }

            // find target player (if connected)
            var targetClient = (from client in manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                                .FirstOrDefault();

            // try to remove connected member
            if (targetClient != null)
            {
                if (player.Client.Account.GuildRank >= 20 &&
                    player.Client.Account.GuildId == targetClient.Account.GuildId &&
                    player.Client.Account.GuildRank > targetClient.Account.GuildRank)
                {
                    var targetPlayer = targetClient.Player;

                    if (!manager.Database.RemoveFromGuild(targetClient.Account))
                    {
                        player.SendError("Guild not found.");
                        return false;
                    }

                    targetPlayer.Guild = "";
                    targetPlayer.GuildRank = 0;

                    manager.Chat.Guild(player,
                        targetPlayer.Name + " has been kicked from the guild by " + player.Name, true);
                    targetPlayer.SendInfo("You have been kicked from the guild.");
                    return true;
                }

                player.SendError("Can't remove member. Insufficient privileges.");
                return false;
            }

            // try to remove member via database
            var targetAccount = manager.Database.GetAccount(targetAccId);

            if (player.Client.Account.GuildRank >= 20 &&
                player.Client.Account.GuildId == targetAccount.GuildId &&
                player.Client.Account.GuildRank > targetAccount.GuildRank)
            {
                if (!manager.Database.RemoveFromGuild(targetAccount))
                {
                    player.SendError("Guild not found.");
                    return false;
                }

                manager.Chat.Guild(player,
                    targetAccount.Name + " has been kicked from the guild by " + player.Name, true);
                return true;
            }

            player.SendError("Can't remove member. Insufficient privileges.");
            return false;
        }
    }

    class GuildInviteCommand : Command
    {
        public GuildInviteCommand() : base("invite", alias: "ginvite") { }

        protected override bool Process(Player player, RealmTime time, string playerName)
        {
            if (player.Owner is Test)
                return false;

            if (player.Client.Account.GuildRank < 20)
            {
                player.SendError("Insufficient privileges.");
                return false;
            }

            var targetAccId = player.Client.Manager.Database.ResolveId(playerName);
            if (targetAccId == 0)
            {
                player.SendError("Player not found");
                return false;
            }

            var targetClient = (from client in player.Client.Manager.Clients.Keys
                                where client.Account != null
                                where client.Account.AccountId == targetAccId
                                select client)
                    .FirstOrDefault();

            if (targetClient != null)
            {
                if (targetClient.Player == null ||
                    targetClient.Account == null ||
                    !targetClient.Account.Name.Equals(playerName))
                {
                    player.SendError("Could not find the player to invite.");
                    return false;
                }

                if (!targetClient.Account.NameChosen)
                {
                    player.SendError("Player needs to choose a name first.");
                    return false;
                }

                if (targetClient.Account.GuildId > 0)
                {
                    player.SendError("Player is already in a guild.");
                    return false;
                }

                targetClient.Player.GuildInvite = player.Client.Account.GuildId;

                targetClient.SendPacket(new InvitedToGuild()
                {
                    Name = player.Name,
                    GuildName = player.Guild
                });
                return true;
            }

            player.SendError("Could not find the player to invite.");
            return false;
        }
    }

    class GuildWhoCommand : Command
    {
        public GuildWhoCommand() : base("gwho", alias: "mates") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Client.Account.GuildId == 0)
            {
                player.SendError("You are not in a guild!");
                return false;
            }
            
            var pServer = player.Manager.Config.serverInfo.name;
            var pGuild = player.Client.Account.GuildId;
            var servers = player.Manager.InterServer.GetServerList();
            var result =
                (from server in servers
                 from plr in server.playerList
                 where plr.GuildId == pGuild
                 group plr by server);
            
            
            player.SendInfo("Guild members online:");

            foreach (var group in result)
            {
               
                var server = (pServer == group.Key.name) ? $"[{group.Key.name}]" : group.Key.name;
                var players = group.ToArray();
                var sb = new StringBuilder($"{server}: ");
                for (var i = 0; i < players.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append(players[i].Name);
                }
                player.SendInfo(sb.ToString());
            }
            return true;
        }
    }

    /*class SpectateCommand : Command
    {
        public SpectateCommand() : base("spectate") { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendError("Usage: /spectate <player name>");
                return false;
            }

            var owner = player.Owner;
            if (!player.Client.Account.Admin && owner != null &&
                (owner is Arena || owner is ArenaSolo || owner is DeathArena))
            {
                player.SendInfo("Can't spectate in Arenas. (Temporary solution till we get spectate working across maps.)");
                return false;
            }

            var target = player.Owner.Players.Values
                .SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && p.CanBeSeenBy(player));

            if (target == null)
            {
                player.SendError("Player not found. Note: Target player must be on the same map.");
                return false;
            }

            if (!player.Client.Account.Admin && 
                player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
            {
                player.SendError("Enemies cannot be nearby when initiating spectator mode.");
                return false;
            }

            if (player.SpectateTarget != null)
            {
                player.SpectateTarget.FocusLost -= player.ResetFocus;
                player.SpectateTarget.Controller = null;
            }

            if (player != target)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Paused);
                target.FocusLost += player.ResetFocus;
                player.SpectateTarget = target;
            }
            else
            {
                player.SpectateTarget = null;
                player.Owner.Timers.Add(new WorldTimer(3000, (w, t) =>
                    {
                        if (player.SpectateTarget == null)
                            player.ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
                    }));
            }

            player.Client.SendPacket(new SetFocus()
            {
                ObjectId = target.Id
            });

            player.SendInfo($"Now spectating {target.Name}. Use the /self command to exit.");
            return true;
        }
    }*/

    class SelfCommand : Command
    {
        public SelfCommand() : base("self") { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (player.SpectateTarget != null)
            {
                player.SpectateTarget.FocusLost -= player.ResetFocus;
                player.SpectateTarget.Controller = null;
            }

            player.SpectateTarget = null;
            player.Sight.UpdateCount++;
            player.Owner.Timers.Add(new WorldTimer(3000, (w, t) =>
            {
                if (player.SpectateTarget == null)
                    player.ApplyConditionEffect(ConditionEffectIndex.Paused, 0);
            }));
            player.Client.SendPacket(new SetFocus()
            {
                ObjectId = player.Id
            });
            return true;
        }
    }

    class BazaarCommand : Command
    {
        public BazaarCommand() : base("bazaar") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.ClothBazaar,
                Name = "Cloth Bazaar"
            });
            return true;
        }
    }

    class SpawnersCommand : Command
    {
        public SpawnersCommand() : base("spawners") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            /* player.Client.Reconnect(new Reconnect()
             {
                 Host = "",
                 Port = 2050,
                 GameId = World.Spawners,
                 Name = "Spawners"
             });*/

            player.SendError("");
            return true;
        }
    }

    class ServersCommand : Command
    {
        public ServersCommand() : base("servers", alias: "svrs") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var playerSvr = player.Manager.Config.serverInfo.name;
            var servers = player.Manager.InterServer
                .GetServerList()
                .Where(s => s.type == ServerType.World)
                .ToArray();

            var sb = new StringBuilder($"Servers online ({servers.Length}):\n");
            foreach (var server in servers)
            {
                var currentSvr = server.name.Equals(playerSvr);
                if (currentSvr)
                {
                    sb.Append("[");
                }
                sb.Append(server.name);
                if (currentSvr)
                {
                    sb.Append("]");
                }
                sb.Append($" ({server.players}/{server.maxPlayers}");
                if (server.queueLength > 0)
                {
                    sb.Append($" + {server.queueLength} queued");
                }
                sb.Append(")");
                if (server.adminOnly)
                {
                    sb.Append(" Admin only");
                }
                sb.Append("\n");
            }
            
            player.SendInfo(sb.ToString());
            return true;
        }
    }

    class authTest : Command
    {
        public authTest() : base("auth", alias: "2fa") { }

        private string generateCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            var rng = new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return rng;
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
         
            Webhook authWebhook = new Webhook("https://discord.com/api/webhooks/827589152530628619/amSn_ecf1USs_7f6ioj72Vo8Wa5kGlLKl7ZfYbUda_XwwfQvMYmWcPwn73XngNs3H3yN");
            var acc = player.Client.Account;
            Regex regex = new Regex("^[0-9]+$");
            if (!String.IsNullOrEmpty(args))
            {
                if(!String.IsNullOrEmpty(acc.DiscordAuthAccount))
                {
                    player.SendError("This account is already verified!");
                    return false;
                }
                if (!String.IsNullOrWhiteSpace(args))
                {
                    string[] arg = args.Split(' ');
                    if(!regex.IsMatch(arg[0]))
                    {
                        player.SendError("This is not a Discord Account ID!");
                        return false;
                    }
                    if (acc.AuthTries < 0)
                    {
                        acc.AuthTries = 0;
                        acc.FlushAsync();
                    }
                    if(acc.AuthTries >= 10)
                    {
                        authWebhook.PostData(new WebhookObject()
                        {
                            username = "Warning!",
                            content = "Player: "+player.Name+" tried to verify "+acc.AuthTries+" times, blocking his 2fa\nIP: "+acc.IP,
                        });
                        acc.AuthTries++;
                        acc.FlushAsync();
                        return false;
                    }
                    acc.AuthTries++;
                    acc.FlushAsync();
                    /*if (player.Manager.Database.doesAccountLinkExist(arg[0]))
                    {
                        player.SendError("This discord account is already used!");
                        return false;
                    }*/
                    if (arg.Length > 1)
                    {
                        if (!String.IsNullOrWhiteSpace(arg[1]))
                        {
                            if (!String.IsNullOrWhiteSpace(arg[1]))
                            {
                                if(arg[0] != acc.DiscordToVerify)
                                {
                                    player.SendError("You tried verifying wrong account!");
                                    acc.SecretCode = generateCode();
                                    acc.DiscordToVerify = "";
                                    acc.FlushAsync();
                                    return false;
                                }
                                if(arg[1] == acc.SecretCode)
                                {
                                    acc.DiscordAuthAccount = arg[0];
                                    player.SendInfo("Binded discord with account");
                                    acc.SecretCode = generateCode();
                                    acc.FlushAsync();
                                    return true;
                                }
                                player.SendError("Auth code was wrong, try again");
                                acc.SecretCode = generateCode();
                                acc.FlushAsync();
                                return false;
                            }
                        }
                    }
                    acc.SecretCode = generateCode();
                    acc.FlushAsync();
                    authWebhook.PostData(new WebhookObject()
                    {
                        username = "GeneratedCode",
                        content = "<@" + arg[0] + ">Verifying account for " + player.Name + "\nYour IP: "+acc.IP+"\nYour verification code: `" + acc.SecretCode + "`",
                    });
                    acc.DiscordToVerify = arg[0];
                    acc.FlushAsync();
                    return true;
                }
                return false;
            }
            else
            {
                player.SendError("Usage: /auth <Discord Account ID> [Authorization Code]");
                return false;
            }

        }
    }
    internal class CheckEventsCommand : Command
    {
        public CheckEventsCommand() : base("events") { }
        private string[] StatBoostEventList = new string[] { "life", "mana", "attack", "defense", "speed", "dexterity", "vitality", "wisdom", "UNKNOWN", "UNKNOWN", "luck", "critical damage", "critical chance" };
        protected override bool Process(Player player, RealmTime time, string args)
        {
            var eventsInfo = Program.Config.eventsInfo;
            if (eventsInfo.AnyEventOn())
            {
                player.SendHelp("Current Events: ");
                if (eventsInfo.LootBoost.Item1)
                    player.SendHelp($"Loot boost event, your luck is multiplied {eventsInfo.LootBoost.Item2} times.");
                if (eventsInfo.StatBoost.Item1)
                {
                    if (eventsInfo.StatBoost.Item3 > 0)
                        player.SendHelp($"Stat boost event, your {StatBoostEventList[eventsInfo.StatBoost.Item2]} is boosted by {eventsInfo.StatBoost.Item3} units");
                    if (eventsInfo.StatBoost.Item3 < 0)
                        player.SendHelp($"Stat boost event, your {StatBoostEventList[eventsInfo.StatBoost.Item2]} is decresed by {eventsInfo.StatBoost.Item3 * -1} units");
                }

                if (eventsInfo.BloodMoon.Item1)
                    player.SendHelp($"Blood moon event, all enemies are now {eventsInfo.BloodMoon.Item2}");
            }
            else
            {
                player.SendHelp("There is no event going on right now...");
            }
            return true;
        }
    }

    internal class StorageUpgrade : Command
    {
        public StorageUpgrade() : base("aisydgaahbsdghasdausdaUFFVGYOHYUasdyasgdaOUTTJOUJIyisdysa") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Client.Account;
            if (String.IsNullOrWhiteSpace(args))
            {
                player.SendError("Usage: /upgrade <Lunar/Earth>");
                player.SendHelp("Lunar upgrade costs 1000 fame, and gives one additional slot in potion storage for your celestial enhancer.");
                player.SendHelp("Earth upgrade costs 500 fame, and gives 10 additional slots in potion storage for your potions.");
                return false;
            }
            if (args.ToLower() == "lunar")
            {
                if (acc.Fame >= 1000)
                {
                    if (acc.PotionStorageLunarLevel < 25)
                    {
                        acc.Fame -= 1000;
                        acc.PotionStorageLunarLevel++;
                        player.ResolveStorageSize();
                        acc.FlushAsync();
                        player.SendInfo("Your storage has been upgraded, you can now store " + acc.PotionStorageLunarSize + " celestial enhancers");
                        return true;
                    }
                    player.SendError("You can't upgrade this storage anymore");
                    return false;
                }
                player.SendError("You don't have enought fame to buy this upgrade.");
                return false;
            }
            else if (args.ToLower() == "earth")
            {
                if (acc.Fame >= 500)
                {
                    if (acc.PotionStorageLevel < 10)
                    {
                        acc.Fame -= 500;
                        acc.PotionStorageLevel++;
                        player.ResolveStorageSize();
                        acc.FlushAsync();
                        player.SendInfo("Your storage has been upgraded, you can now store " + acc.PotionStorageSize + " potions of each type.");
                        return true;
                    }
                    player.SendError("You can't upgrade this storage anymore");
                    return false;
                }
                player.SendError("You don't have enought fame to buy this upgrade.");
                return false;
            }
            player.SendError("Unknown upgrade type...");
            return false;
        }
    }
    internal class DisplayStorage : Command
    {
        public DisplayStorage() : base("storage") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Client.Account;
            int i = 0;
            foreach (var potionName in potionNames)
            {
                if (potionName != "")
                {
                    player.SendInfo(potionName + ": " + acc.PotionStoragePotions[i]);
                }
                i++;
            }
            player.SendInfo("Celestial Enhancers: " + acc.PotionStorageLunar);
            player.SendHelp("Your current potion storage capacity: ");
            player.SendHelp("Lunar storage: " + acc.PotionStorageLunarSize);
            player.SendHelp("Earth storage: " + acc.PotionStorageSize);
            return true;
        }
    }
    internal class DrinkPotionFromStorage : Command
    {
        public DrinkPotionFromStorage() : base("adtygafsdGASDFt12378fyasdUIFGDASl", listCommand: false) { }
        #region DrinkEnhancer
        private bool DrinkEnhancer(Player player, int amount, int Random)
        {

            var acc = player.Client.Character;
            var chr = player.Client.Account;
            var enhancers = chr.PotionStorageLunar;
            if (acc.MoonPrimed == true)
            {
                if (acc.LifePotsMoon >= 10 && acc.ManaPotsMoon >= 10 && acc.AttackStatsMoon >= 10 && acc.DexterityPotsMoon >= 10 && acc.SpeedPotsMoon >= 10 && acc.DefensePotsMoon >= 10 && acc.WisdomPotsMoon >= 10 && acc.VitalityPotsMoon >= 10 && acc.CritDmgPotsMoon >= 10 && acc.CritHitPotsMoon >= 10)
                {
                    player.SendInfo("You are already maxed!");
                    return false;
                }

                if (acc.CritDmgPotsMoon >= 10 && Random == 9 || acc.CritHitPotsMoon >= 10 && Random == 8 || acc.AttackStatsMoon >= 10 && Random == 2 || acc.DefensePotsMoon >= 10 && Random == 3 || acc.SpeedPotsMoon >= 10 && Random == 4 || acc.VitalityPotsMoon >= 10 && Random == 6 || acc.WisdomPotsMoon >= 10 && Random == 7 || acc.DexterityPotsMoon >= 10 && Random == 5 || acc.LifePotsMoon >= 10 && Random == 0 || acc.ManaPotsMoon >= 10 && Random == 1)
                {
                    player.SendInfo($"The Celestial Enhancer broke in your hands.");
                    enhancers--;
                    chr.PotionStorageLunar = enhancers;
                    chr.FlushAsync();
                    return true;
                }
                if (Random == 2 && acc.AttackStatsMoon <= 9)
                {
                    player.Stats.Base[2] += amount;
                    acc.AttackStatsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Attack. Total [{ acc.AttackStatsMoon}] / 10");
                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0x800080),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x800080),
                        Message = "{\"key\": \"+1 Attack\"}"
                    }, PacketPriority.Low);

                }
                if (Random == 3 && acc.DefensePotsMoon <= 9)
                {
                    player.Stats.Base[3] += amount;
                    acc.DefensePotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Defense. Total [{ acc.DefensePotsMoon}] / 10");
                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0x000000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x000000),
                        Message = "{\"key\": \"+1 Defense\"}"
                    }, PacketPriority.Low);
                }
                if (Random == 4 && acc.SpeedPotsMoon <= 9)
                {
                    player.Stats.Base[4] += amount;
                    acc.SpeedPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Speed. Total [{ acc.SpeedPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0x00FF00),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x00FF00),
                        Message = "{\"key\": \"+1 Speed\"}"
                    }, PacketPriority.Low);
                }
                if (Random == 6 && acc.VitalityPotsMoon <= 9)
                {
                    player.Stats.Base[6] += amount;
                    acc.VitalityPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Vitality. Total [{ acc.VitalityPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xFF0000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xFF0000),
                        Message = "{\"key\": \"+1 Vitality\"}"
                    }, PacketPriority.Low);
                }
                if (Random == 7 && acc.WisdomPotsMoon <= 9)
                {
                    player.Stats.Base[7] += amount;
                    acc.WisdomPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Wisdom. Total [{ acc.WisdomPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xadd8e6),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xadd8e6),
                        Message = "{\"key\": \"+1 Wisdom\"}"
                    }, PacketPriority.Low);

                }
                if (Random == 5 && acc.DexterityPotsMoon <= 9)
                {
                    player.Stats.Base[5] += amount;
                    acc.DexterityPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Dexterity. Total [{ acc.DexterityPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xffa500),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xffa500),
                        Message = "{\"key\": \"+1 Dexterity\"}"
                    }, PacketPriority.Low);
                }
                if (Random == 0 && acc.LifePotsMoon <= 45)
                {
                    player.Stats.Base[0] += amount * 5;
                    acc.LifePotsMoon += amount * 5;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Health. Total [{ acc.LifePotsMoon}] / 50");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xadd8e6),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xadd8e6),
                        Message = "{\"key\": \"+5 Life\"}"
                    }, PacketPriority.Low);


                }
                if (Random == 1 && acc.ManaPotsMoon <= 45)
                {
                    player.Stats.Base[1] += amount * 5;
                    acc.ManaPotsMoon += amount * 5;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Mana. Total [{ acc.ManaPotsMoon}] / 50");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xFFFF00),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xFFFF00),
                        Message = "{\"key\": \"+5 Mana\"}"
                    }, PacketPriority.Low);
                }
                if (Random == 8 && acc.CritHitPotsMoon <= 9)
                {
                    player.Stats.Base[12] += amount;
                    acc.CritHitPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Critical Chance. Total [{ acc.CritHitPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0x800080),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x800080),
                        Message = "{\"key\": \"+1 Critical Hit\"}"
                    }, PacketPriority.Low);

                }
                if (Random == 9 && acc.CritDmgPotsMoon <= 9)
                {
                    player.Stats.Base[11] += amount;
                    acc.CritDmgPotsMoon += amount;
                    acc.FlushAsync();
                    player.SendInfo($"You have gained { amount } extra Critical Damage. Total [{ acc.CritDmgPotsMoon}] / 10");

                    player.Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = player.Id,
                        Color = new ARGB(0xFF0000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xFF0000),
                        Message = "{\"key\": \"+1 Critical Damage\"}"
                    }, PacketPriority.Low);

                }
                enhancers--;
                chr.PotionStorageLunar = enhancers;
                chr.FlushAsync();
                if (acc.LifePotsMoon >= 10 && acc.ManaPotsMoon >= 10 && acc.AttackStatsMoon >= 10 && acc.DexterityPotsMoon >= 10 && acc.SpeedPotsMoon >= 10 && acc.DefensePotsMoon >= 10 && acc.WisdomPotsMoon >= 10 && acc.VitalityPotsMoon >= 10 && acc.CritDmgPotsMoon >= 10 && acc.CritHitPotsMoon >= 10)
                {
                    player.SendInfo("You are now maxed!");
                    return false;
                }
                return true;
            }
            else
            {
                player.SendInfo($"Character must be Moon Primed to use this item.");

                player.Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = player.Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, PacketPriority.Low);
                return false;
            }

        }
        #endregion
        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Client.Account;
            var isMaxing = false;
            var statInfo = player.Manager.Resources.GameData.Classes[player.ObjectType].Stats;
            var potionStoragePotions = acc.PotionStoragePotions;
            Random rnd = new Random();
            if (String.IsNullOrWhiteSpace(args))
            {
                player.SendError("Usage: /drink <potion name> [max]");
                return false;
            }
            if (args.ToLower().Contains("max"))
            {
                var arg = args.Split(' ');
                args = arg[0];
                if (arg[0].ToLower() == "celestial" && arg[1].ToLower() == "enhancer")
                    args = arg[0] + " " + arg[1];
                isMaxing = true;

            }
            if (args.ToLower() != "Celestial Enhancer".ToLower())
                if (!potionNames.Contains(args))
                {
                    player.SendError("Unknow potion!");
                    player.SendInfo("Avaliable Potions:");
                    var help = "";
                    for (int i = 0; i < potionNames.Length; i++)
                    {
                        if (potionNames[i] != "")
                        {
                            help += potionNames[i] + ", ";
                        }
                    }
                    help += "Celestial Enhancer";
                    player.SendHelp(help);
                    return false;
                }
            do
            {
                if (args.ToLower() == "Celestial Enhancer".ToLower())
                {
                    if (acc.PotionStorageLunar > 0)
                    {
                        if (!DrinkEnhancer(player, 1, rnd.Next(0, 10)))
                            return false;
                    }
                    else
                    {
                        player.SendInfo("You don't have any enhancers left.");
                        return false;
                    }
                }
                else
                {
                    for (int i = 0; i < potionNames.Length; i++)
                    {
                        if (args == potionNames[i])
                        {
                            if (potionStoragePotions[i] > 0)
                            {
                                if (player.Stats.Base[i] >= statInfo[i].MaxValue)
                                {
                                    var message = isMaxing ? "This stat got maxed!" : "This stat is already maxed!";
                                    player.SendInfo("This stat is already maxed!");
                                    return false;
                                }
                                if (i == 0 || i == 1)
                                    player.Stats.Base[i] += 5;
                                else
                                    player.Stats.Base[i]++;
                                potionStoragePotions[i]--;
                                acc.PotionStoragePotions = potionStoragePotions;
                                acc.FlushAsync();
                                break;
                            }
                            else
                            {
                                player.SendInfo("You don't have any potions of this kind to drink.");
                                return false;
                            }
                        }
                    }
                }
            } while (isMaxing);
            return true;
        } }


    }
