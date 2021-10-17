﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using common;
using common.resources;
using log4net;
using Newtonsoft.Json;
using wServer.networking;
using wServer.networking.packets.outgoing;
using wServer.realm.setpieces;
using wServer.realm.entities;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;


using wServer.networking.packets.outgoing.pets;
using System.Collections.Concurrent;
using wServer.logic;
using wServer.networking.packets;
using wServer.realm.terrain;

namespace wServer.realm.commands
{
    class DebugCommand2 : Command
    {
        public DebugCommand2() : base("debug2", permLevel: 100) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.CurrentDatetime = 200000;
            return true;
        }
    }

    class DayCommand : Command
    {
        public DayCommand() : base("Day", permLevel: 100) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.CurrentDatetime = 0;
            return true;
        }
    }

    class NightCommand : Command
    {
        public NightCommand() : base("Night", permLevel: 100) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.CurrentDatetime = 48000;
            return true;
        }
    }
    class SpawnCommand : Command
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SpawnCommand));
        private struct JsonSpawn
        {
            public string notif;
            public SpawnProperties[] spawns;
        }

        private readonly Client _client;
        public Client Client => _client;



        private struct SpawnProperties
        {
            public string name;
            public int? hp;
            public int? size;
            public int? count;
            public int[] x;
            public int[] y;
            public bool? target;
        }

        private const int Delay = 3; // in seconds

        public SpawnCommand() : base("spawn", permLevel: 200) { }


        protected override bool Process(Player player, RealmTime time, string args)
        {
            args = args.Trim();
            return args.StartsWith("{") ?
                SpawnJson(player, args) :
                SpawnBasic(player, args);
        }

        private bool SpawnJson(Player player, string json)
        {
            var gameData = player.Manager.Resources.GameData;

            JsonSpawn props;
            try
            {
                props = JsonConvert.DeserializeObject<JsonSpawn>(json);
            }
            catch (Exception)
            {
                player.SendError("JSON not formatted correctly!");
                return false;
            }

            if (props.spawns != null)
                foreach (var spawn in props.spawns)
                {
                    if (spawn.name == null)
                    {
                        player.SendError("No mob specified. Every entry needs a name property.");
                        return false;
                    }

                    var objType = GetSpawnObjectType(gameData, spawn.name);
                    if (objType == null)
                    {
                        player.SendError("Unknown entity!");
                        return false;
                    }

                    var desc = gameData.ObjectDescs[objType.Value];

                    if (player.Client.Account.Rank < 100 &&
                        player.Owner is DeathArena &&
                        desc.ObjectId.Contains("Fountain"))
                    {
                        player.SendError("Insufficient rank.");
                        return false;
                    }

                    var hp = desc.MaxHP;
                    if (spawn.hp > hp && spawn.hp < int.MaxValue)
                        hp = spawn.hp.Value;

                    var size = desc.MinSize;
                    if (spawn.size >= 25 && spawn.size <= 250)
                        size = spawn.size.Value;

                    var count = 1;
                    if (spawn.count > count && spawn.count <= 250)
                        count = spawn.count.Value;

                    int[] x = null;
                    int[] y = null;

                    if (spawn.x != null)
                        x = new int[spawn.x.Length];

                    if (spawn.y != null)
                        y = new int[spawn.y.Length];

                    if (x != null)
                    {
                        for (int i = 0; i < x.Length && i < count; i++)
                        {
                            if (spawn.x[i] > 0 && spawn.x[i] <= player.Owner.Map.Width)
                            {
                                x[i] = spawn.x[i];
                            }

                        }

                    }

                    if (y != null)
                    {
                        for (int i = 0; i < y.Length && i < count; i++)
                        {
                            if (spawn.y[i] > 0 && spawn.y[i] <= player.Owner.Map.Height)
                            {
                                y[i] = spawn.y[i];
                            }

                        }

                    }

                    bool target = false;
                    if (spawn.target != null)
                        target = spawn.target.Value;

                    QueueSpawnEvent(player, count, objType.Value, hp, size, x, y, target);
                }

            if (props.notif != null)
            {
                NotifySpawn(player, props.notif);
            }


            return true;
        }

        private bool SpawnBasic(Player player, string args)
        {
            var gameData = player.Manager.Resources.GameData;

            // split argument
            var index = args.IndexOf(' ');
            int num;
            var name = args;
            if (args.IndexOf(' ') > 0 && int.TryParse(args.Substring(0, args.IndexOf(' ')), out num)) //multi
                name = args.Substring(index + 1);
            else
                num = 1;

            var objType = GetSpawnObjectType(gameData, name);
            if (objType == null)
            {
                player.SendError("Unknown entity!");
                return false;
            }

            if (num <= 0)
            {
                player.SendInfo($"Really? {num} {name}? I'll get right on that...");
                return false;
            }

            var id = player.Manager.Resources.GameData.ObjectTypeToId[objType.Value];
            if (player.Client.Account.Rank < 100 &&
                player.Owner is DeathArena &&
                id.Contains("Fountain"))
            {
                player.SendError("Insufficient rank.");
                return false;
            }



            NotifySpawn(player, id, num);
            QueueSpawnEvent(player, num, objType.Value);
            return true;
        }



        private ushort? GetSpawnObjectType(XmlData gameData, string name)
        {
            ushort objType;
            if (!gameData.IdToObjectType.TryGetValue(name, out objType) ||
                !gameData.ObjectDescs.ContainsKey(objType))
            {
                // no match found, try to get partial match
                var mobs = gameData.IdToObjectType
                    .Where(m => m.Key.ContainsIgnoreCase(name) && gameData.ObjectDescs.ContainsKey(m.Value))
                    .Select(m => gameData.ObjectDescs[m.Value]);

                if (!mobs.Any())
                    return null;

                var maxHp = mobs.Max(e => e.MaxHP);
                objType = mobs.First(e => e.MaxHP == maxHp).ObjectType;
            }

            return objType;
        }

        private void NotifySpawn(Player player, string mob, int? num = null)
        {
            var w = player.Owner;

            var notif = mob;
            if (num != null)
                notif = "Spawning " + ((num > 1) ? num + " " : "") + mob + "...";

            w.BroadcastPacket(new Notification
            {
                Color = new ARGB(0xffff0000),
                ObjectId = (player.IsControlling) ? player.SpectateTarget.Id : player.Id,
                Message = notif
            }, null, PacketPriority.Low);

            if (player.IsControlling)
                w.BroadcastPacket(new Text
                {
                    Name = $"#{player.SpectateTarget.ObjectDesc.DisplayId}",
                    NumStars = -1,
                    BubbleTime = 0,
                    Txt = notif
                }, null, PacketPriority.Low);
            else
                w.BroadcastPacket(new Text
                {
                    Name = $"#{player.Name}",
                    NumStars = player.Stars,
                    Admin = player.Admin,
                    BubbleTime = 0,
                    Txt = notif
                }, null, PacketPriority.Low);
        }

        public void QueueSpawnEvent(
            Player player,
            int num,
            ushort mobObjectType, int? hp = null, int? size = null,
            int[] x = null, int[] y = null,
            bool? target = false)
        {
            var pX = player.X;
            var pY = player.Y;

            player.Owner.Timers.Add(new WorldTimer(Delay * 1000, (world, t) => // spawn mob in delay seconds
            {
                for (var i = 0; i < num && i < 500; i++)
                {
                    Entity entity;
                    try
                    {
                        entity = Entity.Resolve(world.Manager, mobObjectType);
                    }
                    catch (Exception e)
                    {
                        log.ErrorFormat(e.ToString());
                        return;
                    }

                    entity.Spawned = false;
                    entity.PlayerSpawned = true;

                    var enemy = entity as Enemy;
                    if (enemy != null)
                    {
                        if (hp != null)
                        {
                            enemy.HP = hp.Value;
                            enemy.MaximumHP = enemy.HP;
                        }

                        if (size != null)
                            enemy.SetDefaultSize(size.Value);

                        if (target == true)
                            enemy.AttackTarget = player;
                    }

                    var sX = (x != null && i < x.Length) ? x[i] : pX;
                    var sY = (y != null && i < y.Length) ? y[i] : pY;

                    entity.Move(sX, sY);

                    if (!world.Deleted)
                        world.EnterWorld(entity);
                }
            }));
        }
    }



    class SetGoldCommand : Command
    {
        public SetGoldCommand() : base("setgold", permLevel: 100, alias: "gold") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var amount = int.Parse(args);

            if (string.IsNullOrEmpty(args))
            {
                player.SendInfo("/gold <amount>");
                return false;
            }
            player.Credits = player.Client.Account.Credits += amount;
            player.ForceUpdate(player.Credits);
            return true;
        }
    }

    class ClearMarketCommand : Command
    {

        public ClearMarketCommand() : base("clearmarket", permLevel: 666, alias: "cmar") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var db = player.Manager.Database;
            var removedOffers = DbMarketData.ClearMarket(db);

            foreach (var i in removedOffers)
            {
                var id = db.ResolveId(i.SellerName);
                var acc = db.GetAccount(id);

                db.AddGift(acc, i.ItemType);
            }

            player.SendInfo("Market Successfully cleared!");
            return true;
        }
    }

    class ClearSpawnsCommand : Command
    {
        public ClearSpawnsCommand() : base("clearspawn", permLevel: 200, alias: "cs") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var removed = 0;
            while (removed != lastKilled)
            {
                lastKilled = removed;
                foreach (var entity in player.Owner.Enemies.Values.Where(e => e.Spawned == true))
                {
                    entity.Death(time);
                    removed++;
                }
                foreach (var entity in player.Owner.StaticObjects.Values.Where(e => e.Spawned == true))
                {
                    player.Owner.LeaveWorld(entity);
                    removed++;
                }
                if (++iterations >= 5)
                    break;
            }

            player.SendInfo($"{removed} spawned entities removed!");
            return true;
        }
    }

    class ClearGravesCommand : Command
    {
        public ClearGravesCommand() : base("cleargraves", permLevel: 200, alias: "cgraves") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var removed = 0;
            foreach (var entity in player.Owner.StaticObjects.Values)
            {
                if (entity is Container || entity.ObjectDesc == null)
                    continue;

                if (entity.ObjectDesc.ObjectId.StartsWith("Gravestone") && entity.Dist(player) < 15)
                {
                    player.Owner.LeaveWorld(entity);
                    removed++;
                }
            }

            player.SendInfo($"{removed} gravestones removed!");
            return true;
        }
    }

    class ToggleEffCommand : Command
    {
        public ToggleEffCommand() : base("eff", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            ConditionEffectIndex effect;
            if (!Enum.TryParse(args, true, out effect))
            {
                player.SendError("Invalid effect!");
                return false;
            }

            var target = player.IsControlling ? player.SpectateTarget : player;
            if ((target.ConditionEffects & (ConditionEffects)((ulong)1 << (int)effect)) != 0)
            {
                //remove
                target.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = 0
                });
            }
            else
            {
                //add
                target.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = effect,
                    DurationMS = -1
                });
            }
            return true;
        }
    }

    class GuildRankCommand : Command
    {
        public GuildRankCommand() : base("grank", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player == null)
                return false;

            // verify argument
            var index = args.IndexOf(' ');
            if (string.IsNullOrWhiteSpace(args) || index == -1)
            {
                player.SendInfo("Usage: /grank <player name> <guild rank>");
                return false;
            }

            // get command args
            var playerName = args.Substring(0, index);
            var rank = args.Substring(index + 1).IsInt() ? args.Substring(index + 1).ToInt32() : RankNumberFromName(args.Substring(index + 1));
            if (rank == -1)
            {
                player.SendError("Unknown rank!");
                return false;
            }
            else if (rank % 10 != 0)
            {
                player.SendError("Valid ranks are multiples of 10!");
                return false;
            }

            // get player account
            if (Database.GuestNames.Contains(playerName, StringComparer.InvariantCultureIgnoreCase))
            {
                player.SendError("Cannot rank the unnamed...");
                return false;
            }
            var id = player.Manager.Database.ResolveId(playerName);
            var acc = player.Manager.Database.GetAccount(id);
            if (id == 0 || acc == null)
            {
                player.SendError("Account not found!");
                return false;
            }

            // change rank
            acc.GuildRank = rank;
            acc.FlushAsync();

            // send out success notifications
            player.SendInfo($"You changed the guildrank of player {acc.Name} to {rank}.");
            var target = player.Manager.Clients.Keys.SingleOrDefault(p => p.Account.AccountId == acc.AccountId);
            if (target?.Player == null) return true;
            target.Player.GuildRank = rank;
            target.Player.SendInfo("Your guild rank was changed");
            return true;
        }

        private int RankNumberFromName(string val)
        {
            switch (val.ToLower())
            {
                case "initiate":
                    return 0;
                case "member":
                    return 10;
                case "officer":
                    return 20;
                case "leader":
                    return 30;
                case "founder":
                    return 40;
            }
            return -1;
        }
    }

    class GimmeCommand : Command
    {
        public GimmeCommand() : base("gimme", permLevel: 60, alias: "give") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var gameData = player.Manager.Resources.GameData;

            ushort objType;
            int playerRank = player.Rank;

            // allow both DisplayId and Id for query
            if (!gameData.DisplayIdToObjectType.TryGetValue(args, out objType))
            {
                if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                {
                    player.SendError("Unknown item type!");
                    return false;
                }
            }

            if (!gameData.Items.ContainsKey(objType))
            {
                player.SendError("Not an item!");
                return false;
            }

            var item = gameData.Items[objType];
            List<String> BannedItems = new List<String> { "key", "sigil", "admin", "fame", "credits", "chest" };
            if (playerRank < 70)
            {

                if (BannedItems.Any(item.DisplayName.ToLower().Contains))
                {
                    player.SendError("Insufficient rank for that item.");
                    return false;
                }
            }

            var availableSlot = player.Inventory.GetAvailableInventorySlot(item);
            if (availableSlot != -1)
            {
                player.Inventory[availableSlot] = item;
                return true;
            }

            player.SendError("Not enough space in inventory!");
            return false;
        }
    }

    class TpPosCommand : Command
    {
        public TpPosCommand() : base("tpPos", permLevel: 70, alias: "goto") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            string[] coordinates = args.Split(' ');
            if (coordinates.Length != 2)
            {
                player.SendError("Invalid coordinates!");
                return false;
            }

            int x, y;
            if (!int.TryParse(coordinates[0], out x) ||
                !int.TryParse(coordinates[1], out y))
            {
                player.SendError("Invalid coordinates!");
                return false;
            }
            //if (!player.done_checking)
            //{
            //    player.SendError("You can't teleport right now.");
            //    return false;
            //}
            player.SetNewbiePeriod();
            player.TeleportPosition(time, x + 0.5f, y + 0.5f, true);
            return true;
        }
    }

    class SetpieceCommand : Command
    {
        public SetpieceCommand() : base("setpiece", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string setPiece)
        {
            if (String.IsNullOrWhiteSpace(setPiece))
            {
                var type = typeof(ISetPiece);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
                var msg = types.Aggregate(
                    "Valid SetPieces: ", (c, p) => c + (p.Name) + ", ");
                player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                return false;
            }

            if (!player.Owner.Name.Equals("Nexus"))
            {
                try
                {
                    ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                    "wServer.realm.setpieces." + setPiece, true, true));
                    piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
                    return true;
                }
                catch (Exception)
                {
                    player.SendError("Invalid SetPiece.");
                    return false;
                }
            }
            else
            {
                player.SendInfo("/setpiece not allowed in Nexus. (Yes, I'm talking to you Q.)");
                return false;
            }
        }
    }


    class KillAllCommand : Command
    {
        public KillAllCommand() : base("killAll", permLevel: 85, alias: "ka") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var killed = 0;
            try
            {
                while (killed != lastKilled)
                {
                    lastKilled = killed;
                    foreach (var i in player.Owner.Enemies.Values.Where(e =>
                        e.ObjectDesc != null && e.ObjectDesc.ObjectId != null
                        && e.ObjectDesc.Enemy && e.ObjectDesc.ObjectId != "Tradabad Nexus Crier"
                        && e.ObjectDesc.ObjectId.ContainsIgnoreCase(args)))
                    {
                        i.Spawned = true;
                        i.Death(time);
                        killed++;
                    }
                    if (++iterations >= 5)
                        break;
                }
            }
            catch (Exception)
            {
                player.SendError("Error");
                return false;
            }

            player.SendInfo($"{killed} enemy killed!");
            return true;
        }
    }

    class KickCommand : Command
    {
        public KickCommand() : base("kick", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Manager.Clients.Keys)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    // probably if someone is hidden doesn't want to be kicked, so we leave it as before
                    if (i.Account.Hidden)
                        break;

                    i.Disconnect();
                    player.SendInfo("Player disconnected!");
                    return true;
                }
            }
            player.SendError($"Player '{args}' could not be found!");
            return false;
        }
    }

    class GetQuestCommand : Command
    {
        public GetQuestCommand() : base("getQuest", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Quest == null)
            {
                player.SendError("Player does not have a quest!");
                return false;
            }
            player.SendInfo("Quest location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
            return true;
        }
    }

    class OryxSayCommand : Command
    {
        public OryxSayCommand() : base("oryxSay", permLevel: 70, alias: "osay") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Oryx(player.Owner, args);
            return true;
        }
    }

    class AnnounceCommand : Command
    {
        public AnnounceCommand() : base("announce", permLevel: 100) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Manager.Chat.Announce(args);
            return true;
        }
    }

    class SummonCommand : Command
    {
        public SummonCommand() : base("summonworld", permLevel: 70, "sw") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    i.Value.Teleport(time, player.Id, true);
                    i.Value.SendInfo($"You've been summoned by {player.Name}.");
                    player.SendInfo("Player summoned!");
                    return true;
                }
            }
            player.SendError($"Player '{args}' could not be found!");
            return false;
        }
    }

    class SummonAllWorldCommand : Command
    {
        public SummonAllWorldCommand() : base("summonallworld", permLevel: 200, "saw") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var world in player.Owner.Manager.Worlds)
            {
                foreach (var i in player.Owner.Players)
                {
                    i.Value.Reconnect(player.Owner);
                    i.Value.TeleportPosition(time, player.X, player.Y, true);
                    i.Value.SendInfo($"You've been summoned by {player.Name}.");
                }
            }

            player.SendInfo("All players summoned!");
            return true;
        }
    }

    class KillPlayerCommand : Command
    {
        public KillPlayerCommand() : base("killPlayer", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            foreach (var i in player.Manager.Clients.Keys)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    i.Player.HP = 0;
                    i.Player.Death(player.Name);
                    player.SendInfo("Player killed!");
                    return true;
                }
            }
            player.SendError($"Player '{args}' could not be found!");
            return false;
        }
    }

    class SizeCommand : Command
    {
        public SizeCommand() : base("size", permLevel: 5) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                player.SendError("Usage: /size <positive integer>. Using 0 will restore the default size for the sprite.");
                return false;
            }

            var size = Utils.FromString(args);
            var min = player.Rank < 70 ? 75 : 25;
            var max = player.Rank < 70 ? 125 : 250;
            if (player.Rank > 90)
            {
                min = 0;
                max = 1000;
            }
            if (size < min && size != 0 || size > max)
            {
                player.SendError($"Invalid size. Size needs to be within the range: {min}-{max}. Use 0 to reset size to default.");
                return false;
            }

            var acc = player.Client.Account;
            acc.Size = size;
            acc.FlushAsync();

            var target = player.IsControlling ? player.SpectateTarget : player;
            if (size == 0)
                target.RestoreDefaultSize();
            else
                target.Size = size;

            return true;
        }
    }

    class RebootCommand : Command
    {
        // Command actually closes the program.
        // An external program is used to monitor the world server existance.
        // If !exist it automatically restarts it.

        public RebootCommand() : base("reboot", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            var manager = player.Manager;
            var servers = manager.InterServer.GetServerList();

            // display help if no argument supplied
            if (string.IsNullOrEmpty(name))
            {
                var sb = new StringBuilder("Current servers available for rebooting:\n");
                for (var i = 0; i < servers.Length; i++)
                {
                    if (i != 0)
                        sb.Append(", ");

                    sb.Append($"{servers[i].name} [{servers[i].type}]");
                }

                player.SendInfo("Usage: /reboot < server name | $all | $wserver | $account >");
                player.SendInfo(sb.ToString());
                return true;
            }

            // attempt to find server match
            foreach (var server in servers)
            {
                if (!server.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                RebootServer(player, 0, server.instanceId);
                player.SendInfo("Reboot command sent.");
                return true;
            }

            // no match found, attempt to match special cases
            switch (name.ToLower())
            {
                case "$all":
                    RebootServer(player, 29000, servers
                        .Select(s => s.instanceId)
                        .ToArray());
                    player.SendInfo("Reboot command sent.");
                    return true;
                case "$wserver":
                    RebootServer(player, 0, servers
                        .Where(s => s.type == ServerType.World)
                        .Select(s => s.instanceId)
                        .ToArray());
                    player.SendInfo("Reboot command sent.");
                    return true;
                case "$account":
                    RebootServer(player, 0, servers
                        .Where(s => s.type == ServerType.Account)
                        .Select(s => s.instanceId)
                        .ToArray());
                    player.SendInfo("Reboot command sent.");
                    return true;
            }

            player.SendInfo("Server not found.");
            return false;
        }

        private void RebootServer(Player issuer, int delay, params string[] instanceIds)
        {
            foreach (var instanceId in instanceIds)
            {
                issuer.Manager.InterServer.Publish(Channel.Control, new ControlMsg()
                {
                    Type = ControlType.Reboot,
                    TargetInst = instanceId,
                    Issuer = issuer.Name,
                    Delay = delay
                });
            }
        }
    }

    class ReSkinCommand : Command
    {
        public ReSkinCommand() : base("reskin", permLevel: 80) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var skins = player.Manager.Resources.GameData.Skins
                .Where(d => d.Value.PlayerClassType == player.ObjectType)
                .Select(d => d.Key)
                .ToArray();

            if (String.IsNullOrEmpty(args))
            {
                var choices = skins.ToCommaSepString();
                player.SendError("Usage: /reskin <positive integer>");
                player.SendError("Choices: " + choices);
                return false;
            }

            var skin = (ushort)Utils.FromString(args);

            if (skin != 0 && !skins.Contains(skin))
            {
                player.SendError("Error setting skin. Either the skin type doesn't exist or the skin is for another class.");
                return false;
            }

            var skinDesc = player.Manager.Resources.GameData.Skins[skin];
            var playerExclusive = skinDesc.PlayerExclusive;
            var size = skinDesc.Size;
            if (playerExclusive != null && !player.Name.Equals(playerExclusive))
            {
                skin = 0;
                size = 100;
            }

            player.SetDefaultSkin(skin);
            player.SetDefaultSize(size);
            return true;
        }
    }

    class Max2Command : Command
    {

        public Max2Command() : base("maxop", permLevel: 70) { }


        protected override bool Process(Player player, RealmTime time, string args)
        {

            var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];
            var chr = player.Client.Character;

            chr.MoonPrimed = true;
            chr.Toolbelts = 8;
            chr.LifePotsMoon = 50;
            chr.ManaPotsMoon = 50;
            chr.AttackStatsMoon = 10;
            chr.DefensePotsMoon = 10;
            chr.SpeedPotsMoon = 10;
            chr.DexterityPotsMoon = 10;
            chr.VitalityPotsMoon = 10;
            chr.WisdomPotsMoon = 10;
            chr.CritDmgPotsMoon = 10;
            chr.CritHitPotsMoon = 10;
            player.Stats.Base[0] = pd.Stats[0].MaxValue + chr.LifePotsMoon;
            player.Stats.Base[1] = pd.Stats[1].MaxValue + chr.ManaPotsMoon;
            player.Stats.Base[2] = pd.Stats[2].MaxValue + chr.AttackStatsMoon;
            player.Stats.Base[3] = pd.Stats[3].MaxValue + chr.DefensePotsMoon;
            player.Stats.Base[4] = pd.Stats[4].MaxValue + chr.SpeedPotsMoon;
            player.Stats.Base[5] = pd.Stats[5].MaxValue + chr.DexterityPotsMoon;
            player.Stats.Base[6] = pd.Stats[6].MaxValue + chr.VitalityPotsMoon;
            player.Stats.Base[7] = pd.Stats[7].MaxValue + chr.WisdomPotsMoon;
            player.Stats.Base[11] = pd.Stats[11].MaxValue + chr.CritDmgPotsMoon;
            player.Stats.Base[12] = pd.Stats[12].MaxValue + chr.CritHitPotsMoon;
            player.Stats.Base[10] = pd.Stats[10].MaxValue;
            player.SendInfo("Your character stats have been maxed.");
            return true;
        }
    }
    class MaxCommand : Command
    {
        public MaxCommand() : base("max", permLevel: 60) { }
        protected override bool Process(Player player, RealmTime time, string args)
        {
            var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];
            var chr = player.Client.Character;

            player.Stats.Base[0] = pd.Stats[0].MaxValue + chr.LifePotsMoon;
            player.Stats.Base[1] = pd.Stats[1].MaxValue + chr.ManaPotsMoon;
            player.Stats.Base[2] = pd.Stats[2].MaxValue + chr.AttackStatsMoon;
            player.Stats.Base[3] = pd.Stats[3].MaxValue + chr.DefensePotsMoon;
            player.Stats.Base[4] = pd.Stats[4].MaxValue + chr.SpeedPotsMoon;
            player.Stats.Base[5] = pd.Stats[5].MaxValue + chr.DexterityPotsMoon;
            player.Stats.Base[6] = pd.Stats[6].MaxValue + chr.VitalityPotsMoon;
            player.Stats.Base[7] = pd.Stats[7].MaxValue + chr.WisdomPotsMoon;
            player.Stats.Base[11] = pd.Stats[11].MaxValue + chr.CritDmgPotsMoon;
            player.Stats.Base[12] = pd.Stats[12].MaxValue + chr.CritHitPotsMoon;

            player.SendInfo("Your character stats have been maxed.");
            return true;
        }
    }




    class TpAmountCommand : Command
    {
        public TpAmountCommand() : base("tqamount", permLevel: 0) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);
            player.SendInfo("Currently have:[" + acc.TQamount + "] Realm TQs left.");
            player.SendInfo("Currently have:[" + acc.TQMamount + "] Moon TQs left.");
            player.SendInfo("Currently have:[" + acc.TQDamount + "] Dungeon TQs left.");
            return true;


        }
    }
    internal class RealmCommand : Command
    {
        public RealmCommand() : base("realm", permLevel: 0)
        {
        }

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
    internal class VaultCommand : Command
    {
        public VaultCommand() : base("vault", permLevel: 0)
        {
        }

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
    internal class MoonCommand : Command
    {
        public MoonCommand() : base("moon", permLevel: 0)
        {
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Moon,
                Name = "Moon"
            });
            return true;
        }
    }

    class LootBoostCommand : Command
    {
        public LootBoostCommand() : base("LootBoost", permLevel: 0) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);
            var x = player.Stats.Base[10] + player.Stats.Boost[10] + 50;
            player.SendInfo("There is an event going on giving you +50% extra lootboost [2/25/2021] ends [12/25/2021].");
            player.SendInfo("Character LootBoost: " + x + "%");
            return true;


        }
    }
    class LootboostAddCommand : Command
    {
        public LootboostAddCommand() : base("Lbadd", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Stats.Base[10] = player.Stats.Base[10] + 1000;
            var x = player.Stats.Base[10] + player.Stats.Boost[10];

            player.SendInfo("+ 1000 loot boost :" + x + "LB");
            return true;


        }
    }
    class LootboostSubCommand : Command
    {
        public LootboostSubCommand() : base("Lbsub", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var x = player.Stats.Base[10] + player.Stats.Boost[10];
            player.Stats.Base[10] = player.Stats.Base[10] - 100;
            player.SendInfo("- 100 loot boost :" + x + "LB");
            return true;


        }
    }
    class LootboostResetCommand : Command
    {
        public LootboostResetCommand() : base("Lbreset", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var x = player.Stats.Base[10] + player.Stats.Boost[10];
            player.Stats.Base[10] = 0;
            player.SendInfo("Loot boost reseted successfully!");
            return true;


        }
    }
    class TpInfiniteCommand : Command
    {
        public TpInfiniteCommand() : base("tqadd", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);

            player.SendInfo("Currently have:[" + acc.TQamount + "] left");
            acc.TQamount = 100;
            acc.FlushAsync();
            return true;


        }
    }
    class StarAmountCommand : Command
    {
        public StarAmountCommand() : base("Staramount", permLevel: 666) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);

            player.SendInfo("Currently have:[" + acc.MStars + "]");
            return true;


        }
    }
    class StarAmountrCommand : Command
    {
        public StarAmountrCommand() : base("Staramountr", permLevel: 666) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);
            acc.MStars = 0;
            acc.FlushAsync();

            player.SendInfo("Currently have:[" + acc.MStars + "]");
            return true;


        }
    }

    class AddVaultsCommand : Command
    {
        public AddVaultsCommand() : base("AddVaults", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var manager = player.Manager;
            var db = manager.Database;
            var acc = player.Manager.Database.GetAccount(player.AccountId);
            acc.MRewarded = 0;
            acc.FlushAsync();
            db.AddVaults();
            player.SendInfo("Done");
            return true;


        }
    }
    class RewardsAmountrCommand : Command
    {
        public RewardsAmountrCommand() : base("rewardamountr", permLevel: 666) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);
            acc.MRewarded = 0;
            acc.MasteryRank = 0;
            acc.FlushAsync();
            player.SendInfo("Currently have:[" + acc.MRewarded + "]");
            player.SendInfo("Currently have:[" + acc.MRewarded + "] Mastery Rank");
            return true;


        }
    }
    class MasteryAmountCommand : Command
    {
        public MasteryAmountCommand() : base("masteryamount", permLevel: 0) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {

            var acc = player.Manager.Database.GetAccount(player.AccountId);

            player.SendInfo("Currently have:[" + acc.MasteryPoints + "] Mastery Points and [" + acc.MasteryRank + "] is your current Mastery Rank.");
            return true;


        }
    }
    class TpQuestCommand : Command
    {
        public TpQuestCommand() : base("tq", permLevel: 0) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Manager.Database.GetAccount(player.AccountId);


            if (player.Owner is Moon && acc.TQMamount > 0)
            {
                acc.TQMamount -= 1;
                acc.FlushAsync();
                player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 3000);
                player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
                player.SetNewbiePeriod();
                player.SendInfo("Teleport Successful, [" + acc.TQMamount + "] Moon Teleports left.");
                player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY + 4, true);
                return true;
            }
            else if (player.Owner is Realm && acc.TQamount > 0)
            {
                acc.TQamount -= 1;
                acc.FlushAsync();
                player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 3000);
                player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
                player.SetNewbiePeriod();
                player.SendInfo("Teleport Successful, [" + acc.TQamount + "] Realm Teleports left.");
                player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY + 4, true);
                return true;
            }
            else if (acc.TQDamount > 0 && player.Owner is not Realm && player.Owner is not Moon)
            {
                acc.TQDamount -= 1;
                acc.FlushAsync();
                player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 3000);
                player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3000);
                player.SetNewbiePeriod();
                player.SendInfo("Teleport Successful, [" + acc.TQDamount + "] Dungeon Teleports left.");
                player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY + 2, true);
                return true;
            }
            else
            {
                player.SendInfo("you have no TQs for this place, do /tqamount for more info ");
                return false;
            }
        }
    }

    class RankCommand : Command
    {
        public RankCommand() : base("rank", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var index = args.IndexOf(' ');
            if (string.IsNullOrEmpty(args) || index == -1)
            {
                player.SendInfo("Usage: /rank <player name> <rank>\\n0: Normal Player, 20: Donor, 70: Former Staff, 80: GM, 90: Dev, 100: Owner");
                return false;
            }

            var name = args.Substring(0, index);
            var rank = int.Parse(args.Substring(index + 1));

            if (rank > player.Rank)
                return false;

            if (Database.GuestNames.Contains(name, StringComparer.InvariantCultureIgnoreCase))
            {
                player.SendError("Cannot rank unnamed accounts.");
                return false;
            }

            var id = player.Manager.Database.ResolveId(name);
            if (id == player.AccountId)
            {
                player.SendError("Cannot rank self.");
                return false;
            }

            if (rank <= 40 && rank > 0)
            {
                player.SendError("Reminder, if you are trying to give a donation rank, use this command.");
                player.SendError("/gift [name] [amount] Rank Voucher");
            }

            var acc = player.Manager.Database.GetAccount(id);
            if (id == 0 || acc == null)
            {
                player.SendError("Account not found!");
                return false;
            }

            // kick player from server to set rank
            foreach (var i in player.Manager.Clients.Keys)
                if (i.Account.Name.EqualsIgnoreCase(name))
                    i.Disconnect();

            if (acc.Admin && rank < 60)
            {
                // reset account
                // player.Manager.Database.WipeAccount(
                // acc, player.Manager.Resources.GameData, player.Name);
                // acc.Reload();
            }

            acc.Admin = rank >= 60;   //<--- leave it at 60 unless all the references to trading and such are changed to be based on rank instead of is account admin
            acc.LegacyRank = rank;
            acc.Hidden = false;
            acc.FlushAsync();

            player.SendInfo($"{acc.Name} given legacy rank {acc.LegacyRank}{((acc.Admin) ? " and now has admin status" : "")}.");
            return true;
        }
    }

    class MuteCommand : Command
    {
        private static readonly Regex CmdParams = new Regex(@"^(\w+)( \d+)?$", RegexOptions.IgnoreCase);

        private readonly RealmManager _manager;

        public MuteCommand(RealmManager manager) : base("mute", permLevel: 70)
        {
            _manager = manager;
            _manager.DbEvents.Expired += HandleUnMute;
        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var match = CmdParams.Match(args);
            if (!match.Success)
            {
                player?.SendError("Usage: /mute <player name> <time out in minutes>\\n" +
                                 "Time parameter is optional. If left out player will be muted until unmuted.");
                return false;
            }

            // gather arguments
            var name = match.Groups[1].Value;
            var id = _manager.Database.ResolveId(name);
            var acc = _manager.Database.GetAccount(id);
            int timeout;
            if (string.IsNullOrEmpty(match.Groups[2].Value))
            {
                timeout = -1;
            }
            else
            {
                int.TryParse(match.Groups[2].Value, out timeout);
            }

            // run through checks
            if (id == 0 || acc == null)
            {
                player?.SendError("Account not found!");
                return false;
            }
            if (acc.IP == null)
            {
                player?.SendError("Account has no associated IP address. Player must login at least once before being muted.");
                return false;
            }
            if (acc.IP.Equals(player?.Client.Account.IP))
            {
                player?.SendError("Mute failed. That action would cause yourself to be muted (IPs are the same).");
                return false;
            }
            if (acc.Admin)
            {
                player?.SendError("Cannot mute other admins.");
                return false;
            }

            // mute player if currently connected
            foreach (var client in _manager.Clients.Keys
                        .Where(c => c.Player != null && c.IP.Equals(acc.IP) && !c.Player.Client.Account.Admin))
            {
                client.Player.Muted = true;
                client.Player.ApplyConditionEffect(ConditionEffectIndex.Muted);
            }

            if (player != null)
            {
                if (timeout > 0)
                    _manager.Chat.SendInfo(id, "You have been muted by " + player.Name + " for " + timeout + " minutes.");
                else
                    _manager.Chat.SendInfo(id, "You have been muted by " + player.Name + ".");
            }

            // mute ip address
            if (timeout < 0)
            {
                _manager.Database.Mute(acc.IP);
                player?.SendInfo(name + " successfully muted indefinitely.");
            }
            else
            {
                _manager.Database.Mute(acc.IP, TimeSpan.FromMinutes(timeout));
                player?.SendInfo(name + " successfully muted for " + timeout + " minutes.");
            }

            return true;
        }

        private void HandleUnMute(object entity, DbEventArgs expired)
        {
            var key = expired.Message;

            if (!key.StartsWith("mutes:"))
                return;

            foreach (var client in _manager.Clients.Keys.Where(c =>
                c.Player != null && c.IP.Equals(key.Substring(6)) && !c.Player.Client.Account.Admin))
            {
                client.Player.Muted = false;
                client.Player.ApplyConditionEffect(ConditionEffectIndex.Muted, 0);
                client.Player.SendInfo("You are no longer muted. Please do not spam. Thank you.");
            }
        }
    }

    class UnMuteCommand : Command
    {
        public UnMuteCommand() : base("unmute", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendError("Usage: /unmute <player name>");
                return false;
            }

            // gather needed info
            var id = player.Manager.Database.ResolveId(name);
            var acc = player.Manager.Database.GetAccount(id);

            // run checks
            if (id == 0 || acc == null || player.Id == id || player.AccountId == acc.AccountId)
            {
                player.SendError("Account not found!");
                return false;
            }
            if (acc.IP == null)
            {
                player.SendError("Account has no associated IP address. Player must login at least once before being unmuted.");
                return false;
            }

            // unmute ip address
            player.Manager.Database.IsMuted(acc.IP).ContinueWith(t =>
            {
                if (!t.IsCompleted)
                {
                    player.SendInfo("Db access error while trying to unmute.");
                    return;
                }

                if (t.Result)
                {
                    player.Manager.Database.Mute(acc.IP, TimeSpan.FromSeconds(1));
                    player.SendInfo(name + " successfully unmuted.");
                }
                else
                {
                    player.SendInfo(name + " wasn't muted...");
                }
            });

            // expire event will handle unmuting of connected players
            return true;
        }
    }

    class BanAccountCommand : Command
    {
        public BanAccountCommand() : base("ban", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            BanInfo bInfo;
            if (args.StartsWith("{"))
            {
                bInfo = Utils.FromJson<BanInfo>(args);
            }
            else
            {
                bInfo = new BanInfo();

                // validate command
                var rgx = new Regex(@"^(\w+) (.+)$");
                var match = rgx.Match(args);
                if (!match.Success)
                {
                    player.SendError("Usage: /ban <account id or name> <reason>");
                    return false;
                }

                // get info from args
                bInfo.Name = match.Groups[1].Value;
                if (!int.TryParse(bInfo.Name, out bInfo.accountId))
                {
                    bInfo.accountId = player.Manager.Database.ResolveId(bInfo.Name);
                }
                bInfo.banReasons = match.Groups[2].Value;
                bInfo.banLiftTime = -1;
            }

            // run checks
            if (Database.GuestNames.Any(n => n.ToLower().Equals(bInfo.Name?.ToLower())))
            {
                player.SendError("If you specify a player name to ban, the name needs to be unique.");
                return false;
            }
            if (bInfo.accountId == 0)
            {
                player.SendError("Account not found...");
                return false;
            }
            if (string.IsNullOrWhiteSpace(bInfo.banReasons))
            {
                player.SendError("A reason must be provided.");
                return false;
            }
            var acc = player.Manager.Database.GetAccount(bInfo.accountId);
            if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
            {
                player.SendError("Cannot ban players of equal or higher rank than yourself.");
                return false;
            }

            // ban player + disconnect if currently connected
            player.Manager.Database.Ban(bInfo.accountId, bInfo.banReasons, bInfo.banLiftTime);
            var target = player.Manager.Clients.Keys
                .SingleOrDefault(c => c.Account != null && c.Account.AccountId == bInfo.accountId);
            target?.Disconnect();

            player.SendInfo(!string.IsNullOrEmpty(bInfo.Name) ?
                $"{bInfo.Name} successfully banned." :
                "Ban successful.");
            return true;
        }

        private class BanInfo
        {
            public int accountId;
            public string Name;
            public string banReasons;
            public int banLiftTime;
        }
    }

    class BanIPCommand : Command
    {
        public BanIPCommand() : base("banip", permLevel: 70, alias: "ipban") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var manager = player.Manager;
            var db = manager.Database;

            // validate command
            var rgx = new Regex(@"^(\w+) (.+)$");
            var match = rgx.Match(args);
            if (!match.Success)
            {
                player.SendError("Usage: /banip <account id or name> <reason>");
                return false;
            }

            // get info from args
            int id;
            var idstr = match.Groups[1].Value;
            if (!int.TryParse(idstr, out id))
            {
                id = db.ResolveId(idstr);
            }
            var reason = match.Groups[2].Value;

            // run checks
            if (Database.GuestNames.Any(n => n.ToLower().Equals(idstr.ToLower())))
            {
                player.SendError("If you specify a player name to ban, the name needs to be unique.");
                return false;
            }
            if (id == 0)
            {
                player.SendError("Account not found...");
                return false;
            }
            if (string.IsNullOrWhiteSpace(reason))
            {
                player.SendError("A reason must be provided.");
                return false;
            }
            var acc = db.GetAccount(id);
            if (string.IsNullOrEmpty(acc.IP))
            {
                player.SendError("Failed to ip ban player. IP not logged...");
                return false;
            }
            if (player.AccountId != acc.AccountId && acc.IP.Equals(player.Client.Account.IP))
            {
                player.SendError("IP ban failed. That action would cause yourself to be banned (IPs are the same).");
                return false;
            }
            if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
            {
                player.SendError("Cannot ban players of equal or higher rank than yourself.");
                return false;
            }

            // ban
            db.Ban(acc.AccountId, reason);
            db.BanIp(acc.IP, reason);

            // disconnect currently connected
            var targets = manager.Clients.Keys.Where(c => c.IP.Equals(acc.IP));
            foreach (var t in targets)
                t.Disconnect();

            // send notification
            player.SendInfo($"Banned {acc.Name} (both account and ip).");
            return true;
        }
    }

    class UnBanAccountCommand : Command
    {
        public UnBanAccountCommand() : base("unban", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var db = player.Manager.Database;

            // validate command
            var rgx = new Regex(@"^(\w+)$");
            if (!rgx.IsMatch(args))
            {
                player.SendError("Usage: /unban <account id or name>");
                return false;
            }

            // get info from args
            int id;
            if (!int.TryParse(args, out id))
                id = db.ResolveId(args);

            // run checks
            if (id == 0)
            {
                player.SendError("Account doesn't exist...");
                return false;
            }

            var acc = db.GetAccount(id);

            // unban
            var banned = db.UnBan(id);
            var ipBanned = acc.IP != null && db.UnBanIp(acc.IP);

            // send notification
            if (!banned && !ipBanned)
            {
                player.SendInfo($"{acc.Name} wasn't banned...");
                return true;
            }
            if (banned && ipBanned)
            {
                player.SendInfo($"Success! {acc.Name}'s account and IP no longer banned.");
                return true;
            }
            if (banned)
            {
                player.SendInfo($"Success! {acc.Name}'s account no longer banned.");
                return true;
            }

            player.SendInfo($"Success! {acc.Name}'s IP no longer banned.");
            return true;
        }
    }

    class ClearInvCommand : Command
    {
        public ClearInvCommand() : base("clearinv", permLevel: 20) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            for (int i = 4; i < 12; i++)
                player.Inventory[i] = null;
            player.SendInfo("Inventory Cleared.");
            return true;
        }
    }

    class MusicCommand : Command
    {
        public MusicCommand() : base("music", permLevel: 100) { }

        protected override bool Process(Player player, RealmTime time, string music)
        {
            var resources = player.Manager.Resources;

            if (string.IsNullOrWhiteSpace(music))
            {
                var msg = resources.MusicNames.Aggregate(
                    "Music Choices: ", (c, p) => c + (p + ", "));
                player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                return false;
            }

            var properName = resources.MusicNames
                .FirstOrDefault(s => s.Equals(music, StringComparison.InvariantCultureIgnoreCase));
            if (properName == null)
            {
                player.SendError($"Music \"{music}\" not found!");
                return false;
            }

            var owner = player.Owner;
            owner.Music = properName;

            foreach (var plr in owner.Players.Values)
                plr.SendInfo($"World music changed to {properName}.");

            var i = 0;
            foreach (var plr in owner.Players.Values)
            {
                owner.Timers.Add(new WorldTimer(100 * i, (w, t) =>
                {
                    if (plr == null)
                        return;

                    plr.Client.SendPacket(new SwitchMusic()
                    {
                        Music = properName
                    });
                }));
                i++;
            }
            return true;
        }
    }

    class UrlMusicCommand : Command
    {
        public UrlMusicCommand() : base("urlmusic", permLevel: 30) { }

        protected override bool Process(Player player, RealmTime time, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                player.SendInfo("/urlmusic <url>");
                return false;
            }
            var owner = player.Owner;
            owner.Music = url;

            var i = 0;
            foreach (var plr in owner.Players.Values)
            {
                owner.Timers.Add(new WorldTimer(100 * i, (w, t) =>
                {
                    if (plr == null)
                        return;

                    plr.Client.SendPacket(new SwitchMusic()
                    {
                        Music = url,
                        isMusic = true
                    });
                }));
                i++;
            }
            return true;
        }
    }

    class UrlImageCommand : Command
    {
        public UrlImageCommand() : base("urlimage", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                player.SendInfo("/urlimage <url>");
                return false;
            }

            var owner = player.Owner;

            var i = 0;
            foreach (var plr in owner.Players.Values)
            {
                owner.Timers.Add(new WorldTimer(100 * i, (w, t) =>
                {
                    if (plr == null)
                        return;

                    plr.Client.SendPacket(new SwitchMusic()
                    {
                        Music = url,
                        isMusic = false
                    });
                }));
                i++;
            }
            return true;
        }
    }

    class CloseRealmCommand : Command
    {
        public CloseRealmCommand() : base("closerealm", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var gw = player.Manager.Worlds[World.Realm] as Realm;

            if (gw == null)
            {
                player.SendError("An undefined error occurred.");
                return false;
            }

            if (gw.IsClosing())
            {
                player.SendError("Realm already closing.");
                return false;
            }

            gw.CloseRealm();
            return true;
        }
    }

    class QuakeCommand : Command
    {
        public QuakeCommand() : base("quake", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string worldName)
        {
            var worldProtoData = player.Manager.Resources.Worlds.Data;

            if (String.IsNullOrWhiteSpace(worldName))
            {
                var msg = worldProtoData.Aggregate(
                    "Valid World Names: ", (c, p) => c + ((!p.Value.setpiece) ? (p.Key + ", ") : ""));
                player.SendInfo(msg.Substring(0, msg.Length - 2) + ".");
                return false;
            }

            if (player.Owner is Nexus)
            {
                player.SendError("Cannot use /quake in Nexus.");
                return false;
            }

            var worldNameProper =
                player.Manager.Resources.Worlds.Data.FirstOrDefault(
                    p => p.Key.Equals(worldName, StringComparison.InvariantCultureIgnoreCase)).Key;

            ProtoWorld proto;
            if (worldNameProper == null || (proto = worldProtoData[worldNameProper]).setpiece)
            {
                player.SendError("Invalid world.");
                return false;
            }

            World world;
            DynamicWorld.TryGetWorld(proto, player.Client, out world);
            world = player.Manager.AddWorld(world ?? new World(proto));

            player.Owner.QuakeToWorld(world);
            return true;
        }
    }

    class VisitCommand : Command
    {
        public VisitCommand() : base("visit", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendInfo("Usage: /visit <player name>");
                return true;
            }

            var target = player.Manager.Clients.Keys
                .SingleOrDefault(c => c.Account != null &&
                                      c.Account.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (target?.Player?.Owner == null ||
                !target.Player.CanBeSeenBy(player))
            {
                player.SendError("Player not found!");
                return false;
            }

            var owner = target.Player.Owner;
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                GameId = owner.Id,
                Name = owner.SBName
            });
            return true;
        }
    }

    class HideCommand : Command
    {
        public HideCommand() : base("hide", permLevel: 70, alias: "h") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var acc = player.Client.Account;

            acc.Hidden = !acc.Hidden;
            acc.FlushAsync();

            if (acc.Hidden)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Hidden);
                player.ApplyConditionEffect(ConditionEffectIndex.Invincible);
                player.Manager.Clients[player.Client].Hidden = true;
            }
            else
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Hidden, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Invincible, 0);
                player.Manager.Clients[player.Client].Hidden = false;
            }

            return true;
        }
    }

    class GlowCommand : Command
    {
        public GlowCommand() : base("glow", permLevel: 10) { }

        protected override bool Process(Player player, RealmTime time, string color)
        {
            if (String.IsNullOrWhiteSpace(color))
            {
                player.SendInfo("Usage: /glow 0x<color>");
                return true;
            }

            player.Glow = Utils.FromString(color);

            var acc = player.Client.Account;
            acc.GlowColor = player.Glow;
            acc.FlushAsync();

            return true;
        }
    }

    class LinkCommand : Command
    {
        public LinkCommand() : base("link", permLevel: 40) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player?.Owner == null)
                return false;
            var world = player.Owner;
            if (world is Moon || world is Realm || world is GuildHall || world is Vault || world.Id < 0 || (player.Rank < 999 && (world is Test)))
            {
                player.SendError("Cannot link this world.");
                return false;
            }
            if (!player.Manager.Monitor.AddPortal(world.Id))
            {
                player.SendError("Link already exists.");
                return false;
            }

            return true;
        }
    }

    class UnLinkCommand : Command
    {
        public UnLinkCommand() : base("unlink", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player?.Owner == null)
                return false;

            var world = player.Owner;
            if (world.Id < 0 || (player.Rank < 80 && !(world is Test)) || (player.Rank < 100 && ((world is Test) || (world is Arena) || (world is ArenaSolo) || (world is DeathArena) || (world is GuildHall) || (world is Moon) || (world is Nexus) || (world is Realm) || (world is Vault))))
            {
                player.SendError("Cannot unlink " + world.Name);
                return false;
            }

            if (!player.Manager.Monitor.RemovePortal(player.Owner.Id))
                player.SendError("Link not found.");
            else
                player.SendInfo("Link removed.");

            return true;
        }
    }

    class GiftCommand : Command
    {
        public GiftCommand() : base("gift", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player == null)
                return false;

            var manager = player.Manager;

            // verify argument
            var index = args.IndexOf(' ');
            if (string.IsNullOrWhiteSpace(args) || index == -1)
            {
                player.SendInfo("Usage: /gift <player name> <item name>");
                return false;
            }

            // get command args
            var playerName = args.Substring(0, index);
            var item = GetItem(player, args.Substring(index + 1));
            if (item == null)
            {
                return false;
            }

            // get player account
            if (Database.GuestNames.Contains(playerName, StringComparer.InvariantCultureIgnoreCase))
            {
                player.SendError("Cannot gift the unnamed...");
                return false;
            }
            var id = manager.Database.ResolveId(playerName);
            var acc = manager.Database.GetAccount(id);
            if (id == 0 || acc == null)
            {
                player.SendError("Account not found!");
                return false;
            }

            // add gift
            var result = player.Manager.Database.AddGift(acc, item.ObjectType);
            if (!result)
            {
                player.SendError("Gift not added. Something happened with the adding process.");
                return false;
            }

            // send out success notifications
            player.SendInfoFormat("You gifted {0} one {1}.", acc.Name, item.DisplayName);
            var gifted = player.Manager.Clients.Keys
                .SingleOrDefault(p => p.Account.AccountId == acc.AccountId);
            gifted?.Player?.SendInfoFormat(
                "You received a gift from {0}. Enjoy your {1}.",
                player.Name,
                item.DisplayName);
            return true;
        }

        private Item GetItem(Player player, string itemName)
        {
            var gameData = player.Manager.Resources.GameData;

            ushort objType;

            // allow both DisplayId and Id for query
            if (!gameData.DisplayIdToObjectType.TryGetValue(itemName, out objType))
            {
                if (!gameData.IdToObjectType.TryGetValue(itemName, out objType))
                    player.SendError("Unknown item type!");
                return null;
            }

            if (!gameData.Items.ContainsKey(objType))
            {
                player.SendError("Not an item!");
                return null;
            }


            return gameData.Items[objType];
        }
    }

    class RemovePets : Command
    {
        public RemovePets() : base("removeAllPets", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            // close all worlds / disconnect all players
            foreach (var w in player.Manager.Worlds.Values)
            {
                w.Closed = true;
                foreach (var p in w.Players.Values)
                    p.Client.Disconnect();
            }

            player.Manager.Database.RemoveAllPets(player.Manager.Resources.GameData);

            Program.Stop();
            return true;
        }
    }

    class RemoveServerGold : Command
    {
        public RemoveServerGold() : base("removeAllGold", permLevel: 200) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            // close all worlds / disconnect all players
            foreach (var w in player.Manager.Worlds.Values)
            {
                w.Closed = true;
                foreach (var p in w.Players.Values)
                    p.Client.Disconnect();
            }

            player.Manager.Database.RemoveAllGold();

            Program.Stop();
            return true;
        }
    }

    /*class WellcomePrivateMessage : Command
    {
        public WellcomePrivateMessage() : base("addWelcomeMessage", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Name != "ossimc82" && player.Name != "TheRegal")
                return false;

            var numAccounts = int.Parse(player.Manager.Database.Conn.StringGet("nextAccId"));
            for (var i = 1; i <= numAccounts; i++)
            {
                try
                {
                    var acc = new DbAccount(player.Manager.Database.Conn, i);
                    acc.AddPrivateMessage(player.AccountId, "Welcome to the new Message System",
                        "Hello guys, this is the new offline message system, this message was auto-generated by the server to show you how it works. Its very simple, just click on the \"SEND NEW MESSAGE\" button to send a new message. Please report any bugs on the forums or send me (ossimc82) a pm\n\n- ossimc82\n- Nillys Realm Team");
                    Client c;
                    if ((c = player.Manager.Clients.Keys.FirstOrDefault(_ => _?.Account?.AccountId == acc.AccountId)) !=
                        null)
                    {
                        c.SendPacket(new GlobalNotification
                        {
                            Text = "forcePrivateMessageRefresh"
                        });
                    }
                }
                catch (Exception)
                {
                }
            }
            return true;
        }
    }*/

    class OverrideAccountCommand : Command
    {
        public OverrideAccountCommand() : base("override", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            var acc = player.Client.Account;

            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendError("Usage: /override <player name>");
                return false;
            }

            var id = player.Manager.Database.ResolveId(name);
            var newAcc = player.Manager.Database.GetAccount(id);
            if (id == 0)
            {
                player.SendError("Account not found!");
                return false;
            }
            if (newAcc.Rank > acc.Rank)
            {
                player.SendError("You can't override this account");
                return false;
            }

            acc.AccountIdOverride = id;
            acc.FlushAsync();
            player.SendInfo("Account override set.");
            return true;
        }
    }
    internal class ClaimCommand : Command
    {
        public ClaimCommand(RealmManager manager) : base("Claim", permLevel: 0)
        {

        }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var db = player.Manager.Database;
            // var id = player.Manager.Database.ResolveId(player.Client.Account.Name);
            var acc = db.GetAccount(player.AccountId);
            var x = player.Client.Account.Rank;
            int y = 0;
            if (acc.SkinsClaimed == false)
            {
                db.AddGift(acc, 0x6184);
                db.AddGift(acc, 0x6183);
                db.AddGift(acc, 0x6182);
                player.SendInfo("Claimed [Free Skin Pack], You may proceed to try to claim more packages.");
                acc.SkinsClaimed = true;
                acc.FlushAsync();
            }
            else if (acc.DonorClaim == false)
            {
                if (acc.Rank >= 4)
                {
                    for (int i = 0; i < x;)
                    {
                        db.AddGift(acc, 0x6172);
                        i += 5;
                        y++;
                    }
                    player.SendInfo(y + "+ Support Box's Claimed.");
                    player.SendInfo("Claimed [Support Chest Rework Pack], You may proceed to try to claim more packages.");
                    acc.DonorClaim = true;
                    acc.FlushAsync();
                }
                else
                {
                    player.SendInfo("No Packages to Claim.");
                }

            }
            else if (acc.WelcomeBack == false) //436 of Database.cs
            {
                db.AddGift(acc, 0x44e7);
                db.AddGift(acc, 0x44e5);
                db.AddGift(acc, 0x7811);
                player.SendInfo("Welcome back to the New and Improved [Name] Server.");
                player.SendInfo("Claimed [Welcome Back Pack], You may proceed to try to claim more packages.");
                acc.WelcomeBack = true;
                acc.FlushAsync();
            }
            else
            {
                player.SendInfo("No Packages to Claim.");
            }
            return true;

        }
    }

    internal class Level20Command : Command
    {
        public Level20Command(RealmManager manager) : base("level20", permLevel: 10, alias: "l20")
        {
            _manager = manager;
        }

        private readonly RealmManager _manager;

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Level < 20)
            {
                var statInfo = _manager.Resources.GameData.Classes[player.ObjectType].Stats;
                for (var v = 0; v < statInfo.Length; v++)
                {
                    player.Stats.Base[v] +=
                    (statInfo[v].MaxIncrease + statInfo[v].MinIncrease) * (21 - player.Level) / 2;
                    if (player.Stats.Base[v] > statInfo[v].MaxValue)
                        player.Stats.Base[v] = statInfo[v].MaxValue;

                }

                player.SendInfo("Level 20 reached, now please move items in your inventory for it to register.");
                player.Level = 20;
                return true;
            }
            return false;
        }
    }

    internal class CurrentTime2Command : Command
    {
        public CurrentTime2Command(RealmManager manager) : base("Time", permLevel: 0)
        {
            _manager = manager;
        }

        private readonly RealmManager _manager;

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo("Current time of day: " + player.Manager.CurrentDatetime);
            if (player.Manager.CurrentDatetime >= 47999)
            {

            }

            return true;
        }
    }

    class RenameCommand : Command
    {
        public RenameCommand() : base("rename", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var index = args.IndexOf(' ');
            if (string.IsNullOrWhiteSpace(args) || index == -1)
            {
                player.SendInfo("Usage: /rename <player name> <new player name>");
                return false;
            }

            var playerName = args.Substring(0, index);
            var newPlayerName = args.Substring(index + 1);

            var id = player.Manager.Database.ResolveId(playerName);
            if (id == 0)
            {
                player.SendError("Player account not found!");
                return false;
            }

            if (newPlayerName.Length < 3 || newPlayerName.Length > 15 || !newPlayerName.All(char.IsLetter) ||
                Database.GuestNames.Contains(newPlayerName, StringComparer.InvariantCultureIgnoreCase))
            {
                player.SendError("New name is invalid. Must be between 3-15 char long and contain only letters.");
                return false;
            }

            string lockToken = null;
            var key = Database.NAME_LOCK;
            var db = player.Manager.Database;

            try
            {
                while ((lockToken = db.AcquireLock(key)) == null) ;

                if (db.Conn.HashExists("names", newPlayerName.ToUpperInvariant()))
                {
                    player.SendError("Name already taken");
                    return false;
                }

                var acc = db.GetAccount(id);
                if (acc == null)
                {
                    player.SendError("Account doesn't exist.");
                    return false;
                }

                using (var l = db.Lock(acc))
                    if (db.LockOk(l))
                    {
                        while (!db.RenameIGN(acc, newPlayerName, lockToken)) ;
                        player.SendInfo("Rename successful.");
                    }
                    else
                        player.SendError("Account in use.");
            }
            finally
            {
                if (lockToken != null)
                    db.ReleaseLock(key, lockToken);
            }

            return true;
        }
    }

    class UnnameCommand : Command
    {
        public UnnameCommand() : base("unname", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                player.SendInfo("Usage: /unname <player name>");
                return false;
            }

            var playerName = args;

            var id = player.Manager.Database.ResolveId(playerName);
            if (id == 0)
            {
                player.SendError("Player account not found!");
                return false;
            }

            string lockToken = null;
            var key = Database.NAME_LOCK;
            var db = player.Manager.Database;

            try
            {
                while ((lockToken = db.AcquireLock(key)) == null) ;

                var acc = db.GetAccount(id);
                if (acc == null)
                {
                    player.SendError("Account doesn't exist.");
                    return false;
                }

                using (var l = db.Lock(acc))
                    if (db.LockOk(l))
                    {
                        while (!db.UnnameIGN(acc, lockToken)) ;
                        player.SendInfo("Account succesfully unnamed.");
                    }
                    else
                        player.SendError("Account in use.");
            }
            finally
            {
                if (lockToken != null)
                    db.ReleaseLock(key, lockToken);
            }

            return true;
        }
    }

    internal class WargCommand : Command
    {
        public WargCommand() : base("warg", permLevel: 90) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendError("Usage: /warg <mob name>");
                return false;
            }

            var target = player.GetNearestEntityByName(2900, name);
            if (target == null)
            {
                player.SendError("Mob not found.");
                return false;
            }

            if (target.Controller != null)
            {
                player.SendError("Only one person can control a mob at a time.");
                return false;
            }

            if (player.SpectateTarget != null)
            {
                player.SpectateTarget.FocusLost -= player.ResetFocus;
                player.SpectateTarget.Controller = null;
            }

            player.ApplyConditionEffect(ConditionEffectIndex.Paused);
            target.FocusLost += player.ResetFocus;
            target.Controller = player;
            player.SpectateTarget = target;
            player.Sight.UpdateCount++;

            player.Owner.Timers.Add(new WorldTimer(500, (w, t) =>
            {
                player.Client.SendPacket(new SetFocus()
                {
                    ObjectId = target.Id
                });
            }));
            return true;
        }
    }

    class CompactLOHCommand : Command
    {
        public CompactLOHCommand() : base("compactLOH", permLevel: 80, listCommand: false) { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            return true;
        }
    }
    internal class GodModeCommand : Command
    {
        public GodModeCommand() : base("godmode", permLevel: 70, alias: "god") { }

        protected override bool Process(Player player, RealmTime time, string name)
        {
            if (player.HasConditionEffect(ConditionEffects.Invincible))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = 0,
                });
                player.SendInfo("Godmode Deactivated");
                return false;
            }
            else
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                });
                player.SendInfo("Godmode Activated");
            }
            return true;
        }
    }
    internal class TpQuestModCommand : Command
    {
        public TpQuestModCommand() : base("tqa", permLevel: 70) { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Quest == null)
            {
                player.SendError("No Quests found!");
                return false;
            }

            player.SetNewbiePeriod();
            player.TeleportPosition(time, player.Quest.RealX, player.Quest.RealY, true);
            player.SendInfo("Teleported to Quest Location: (" + player.Quest.X + ", " + player.Quest.Y + ")");
            return true;
        }
    }
    internal class DyeCommand : Command
    {
        public DyeCommand() : base("dye", permLevel: 40, alias: "cloth") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            var gameData = player.Manager.Resources.GameData;
            ushort objType;

            if (string.IsNullOrEmpty(args))
            {
                player.SendError("Usage: /dye <textile name>");
                return false;
            }
            if (!gameData.DisplayIdToObjectType.TryGetValue(args, out objType))
            {
                if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                {
                    player.SendError("Unknown textile");
                    return false;
                }
            }
            var item = gameData.Items[objType];
            if (item.Texture1 != 0 || item.Texture2 != 0)
            {
                if (item.Texture1 != 0)
                    player.Texture1 = item.Texture1;
                if (item.Texture2 != 0)
                    player.Texture2 = item.Texture2;
            }
            else
            {
                player.SendError("This item is not a textile.");
                return false;
            }
            player.SendInfo("Your textile has been updated.");
            return true;
        }
    }
    internal class EventCommand : Command
    {
        public EventCommand() : base("event", permLevel: 200) { }
        private string[] EventsList = new string[] { "Blood Moon", "Oryx Arena", "Loot Boost", "Stat Boost" };
        private EventsInfo eventsInfo = Program.Config.eventsInfo;
        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (String.IsNullOrWhiteSpace(args))
            {
                string events = "";
                player.SendError("Usage: /event <Event Name>");
                for (int i = 0; i < EventsList.Length; i++)
                    events += EventsList[i] + ", ";
                player.SendInfo("Avalibale Events: " + events);
                return false;
            }
            if (args.ContainsIgnoreCase("Loot Boost"))
            {
                var arg = args.Split(' ');
                if (arg.Length == 3)
                {
                    if (Convert.ToDouble(arg[2]) == 0)
                    {
                        eventsInfo.LootBoost = new Tuple<bool, double>(false, 1);
                        player.Manager.Chat.Announce($"Loot boost event has been turned off!");
                        return true;
                    }
                    try
                    {
                        eventsInfo.LootBoost = new Tuple<bool, double>(true, Convert.ToDouble(arg[2]));
                        player.Manager.Chat.Announce($"{player.Name} has hosted a loot boost event!");
                        return true;
                    }
                    catch { return false; }
                }
                else
                {
                    if (arg[2].ContainsIgnoreCase("off"))
                    {
                        eventsInfo.LootBoost = new Tuple<bool, double>(false, 1);
                        player.SendInfo("Loot Boost Event is now OFF");
                        return true;
                    }
                    player.SendError("Usage: /event Loot Boost <Value> | 0 or \"off\" to turn off");
                    return false;
                }
            }
            if (args.ContainsIgnoreCase("Stat Boost"))
            {
                var arg = args.Split(' ');
                if (arg.Length == 4)
                {
                    try
                    {
                        eventsInfo.StatBoost = new Tuple<bool, int, int>(true, Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3]));
                        player.Manager.Chat.Announce($"{player.Name} has hosted a stat boost event!");
                        return true;
                    }
                    catch { return false; }
                }
                else
                {
                    if (args.ContainsIgnoreCase("off"))
                    {
                        eventsInfo.StatBoost = new Tuple<bool, int, int>(false, 0, 0);
                        player.Manager.Chat.Announce($"Stat boost event has been turned off!");
                        return true;
                    }
                    player.SendError("Usage: /event Stat Boost <StatID> <Value> | off to turn off");
                    return false;
                }
            }
            if (args.ContainsIgnoreCase("Oryx Arena"))
            {
                return false;
                if (player.Owner is Nexus)
                {
                    var portal = new Portal(player.Manager, 0x441c, null)
                    {
                        Name = "Locked Oryx's Arena Portal",
                        WorldInstance = player.Owner
                    };

                    portal.Name = "Locked Oryx's Arena Portal";
                    portal.Move(335f, 149f);
                    player.Owner.EnterWorld(portal);
                    player.Manager.Chat.Announce($"{player.Name} has hosted a oryx arena event!");
                }
                else
                {
                    player.SendError("You have to be in nexus to use this command.");
                    return false;
                }

            }
            if (args.ContainsIgnoreCase("Blood Moon"))
            {
                var arg = args.Split(' ');
                if (arg.Length == 3)
                {
                    if (args.ContainsIgnoreCase("off"))
                    {
                        eventsInfo.BloodMoon = new Tuple<bool, string>(false, "off");
                        player.Manager.Chat.Announce($"Blood Moon event has been turned off!");
                        return true;
                    }
                    try
                    {
                        if (arg[2].ContainsIgnoreCase("Legendary") || arg[2].ContainsIgnoreCase("Epic") || arg[2].ContainsIgnoreCase("Rare"))
                        {
                            var tier = arg[2];
                            player.Manager.Chat.Announce($"{player.Name} has hosted a {tier} Blood Moon event!");
                            eventsInfo.BloodMoon = new Tuple<bool, string>(true, tier.ToLower());
                            return true;
                        }

                    }
                    catch { return false; }
                }
                else
                {
                    player.SendError("Usage: /event Blood Moon <Rarity> | Rare, Epic, Legendary | Off to turn off");
                    return false;
                }
            }
            return false;
        }
    }
}
