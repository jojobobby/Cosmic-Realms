using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using common;
using wServer.logic;
using wServer.networking.packets;
using wServer.networking;
using wServer.realm.terrain;
using wServer.realm.entities;
using log4net;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using DiscordWebhook;

namespace wServer.realm.entities
{
    interface IPlayer
    {
        void Damage(int dmg, Entity src);
        bool IsVisibleToEnemy();
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        new static readonly ILog Log = LogManager.GetLogger(typeof(Player));

        private readonly Client _client;

        public int totalMoonPots = 0;

        public Client Client => _client;
        public int Rng2;
        //Stats
        private readonly SV<int> _accountId;
        public int AccountId {
            get { return _accountId.GetValue(); }
            set { _accountId.SetValue(value); }
        }

        private readonly SV<int> _experience;
        public int Experience {
            get { return _experience.GetValue(); }
            set { _experience.SetValue(value); }
        }
        public int Rn4g = 0;
        private readonly SV<int> _experienceGoal;
        public int ExperienceGoal {
            get { return _experienceGoal.GetValue(); }
            set { _experienceGoal.SetValue(value); }
        }

        private readonly SV<int> _level;
        public int Level {
            get { return _level.GetValue(); }
            set { _level.SetValue(value); }
        }

        public int timeMoon;
        public int xMoon;
        public int timeDecoy;
        public int x;//acc.MoonPrimed
        private readonly SV<int> _currentFame;
        public bool StatBoostsTF;
        public int _canTpCooldownTime2;
        public int _canTpCooldownTime222;
        public int _canTpCooldownTime22;
        public int _canTpCooldownTime3;
        public int _canTpCooldownTime4;
        public int _canTpCooldownTime5;
        public int _canTpCooldownTime6;
        public int _canTpCooldownTime7;
        public int _canTpCooldownTime8;
        public int _canTpCooldownTime9;
        public int _canTpCooldownTime10;
        public int _canTpCooldownTime11;
        public int _canTpCooldownTime12;
        public int _canTpCooldownTime13;
        public int _canTpCooldownTime14;
        public int _canTpCooldownTime15;

        public bool AllyShotsBool;

        public bool DecoyStillActive;
        public float Xdecoy;
        public float Ydecoy;
        public bool UltraInstinct;

        public bool freeAbilityUse;
        public int CurrentFame {
            get { return _currentFame.GetValue(); }
            set { _currentFame.SetValue(value); }
        }

        private readonly SV<int> _fame;
        public int Fame {
            get { return _fame.GetValue(); }
            set { _fame.SetValue(value); }
        }

        private readonly SV<int> _fameGoal;
        public int FameGoal {
            get { return _fameGoal.GetValue(); }
            set { _fameGoal.SetValue(value); }
        }

        private readonly SV<int> _stars;
        public int Stars {
            get { return _stars.GetValue(); }
            set { _stars.SetValue(value); }
        }

        private readonly SV<string> _guild;
        public string Guild {
            get { return _guild.GetValue(); }
            set { _guild.SetValue(value); }
        }

        private readonly SV<int> _guildRank;
        public int GuildRank {
            get { return _guildRank.GetValue(); }
            set { _guildRank.SetValue(value); }
        }

        private readonly SV<int> _credits;
        public int Credits {
            get { return _credits.GetValue(); }
            set { _credits.SetValue(value); }
        }

        private readonly SV<bool> _nameChosen;
        public bool NameChosen {
            get { return _nameChosen.GetValue(); }
            set { _nameChosen.SetValue(value); }
        }

        private readonly SV<int> _texture1;
        public int Texture1 {
            get { return _texture1.GetValue(); }
            set { _texture1.SetValue(value); }
        }

        private readonly SV<int> _texture2;
        public int Texture2 {
            get { return _texture2.GetValue(); }
            set { _texture2.SetValue(value); }
        }


        private int _originalSkin;
        private readonly SV<int> _skin;
        public int Skin {
            get { return _skin.GetValue(); }
            set { _skin.SetValue(value); }
        }

        private readonly SV<int> _glow;
        public int Glow {
            get { return _glow.GetValue(); }
            set { _glow.SetValue(value); }
        }

        private readonly SV<int> _mp;
        public int MP {
            get { return _mp.GetValue(); }
            set { _mp.SetValue(value); }
        }

        private readonly SV<bool> _hasBackpack;
        public bool HasBackpack {
            get { return _hasBackpack.GetValue(); }
            set { _hasBackpack.SetValue(value); }
        }




        private readonly SV<bool> _xpBoosted;
        public bool XPBoosted {
            get { return _xpBoosted.GetValue(); }
            set { _xpBoosted.SetValue(value); }
        }

        private readonly SV<int> _oxygenBar;
        public int OxygenBar {
            get { return _oxygenBar.GetValue(); }
            set { _oxygenBar.SetValue(value); }
        }

        private readonly SV<int> _rank;
        public int Rank {
            get { return _rank.GetValue(); }
            set { _rank.SetValue(value); }
        }

        private readonly SV<int> _admin;
        public int Admin {
            get { return _admin.GetValue(); }
            set { _admin.SetValue(value); }
        }

        private readonly SV<int> _tokens;
        public int Tokens {
            get { return _tokens.GetValue(); }
            set { _tokens.SetValue(value); }
        }

        public int XPBoostTime { get; set; }
        public int LDBoostTime { get; set; }
        public int LTBoostTime { get; set; }
        public ushort SetSkin { get; set; }
        public int SetSkinSize { get; set; }
        public Pet Pet { get; set; }

        public int? GuildInvite { get; set; }
        public bool Muted { get; set; }

        public RInventory DbLink { get; private set; }
        public int[] SlotTypes { get; private set; }
        public Inventory Inventory { get; private set; }

        
        public ItemStacker HealthPots { get; private set; }
        public ItemStacker MagicPots { get; private set; }
        public ItemStacker[] Stacks { get; private set; }

        public readonly StatsManager Stats;
        public bool Lifesteal { get; set; }

        public bool sended { get; set; }
        public int missed { get; set; }
        public int hitted { get; set; }
        public bool done_checking { get; set; }
        public int tickTimes { get; set; }

        protected override void ImportStats(StatsType stats, object val)
        {
            var chr = _client.Character;

            var items = Manager.Resources.GameData.Items;
            base.ImportStats(stats, val);
            switch (stats)
            {

                case StatsType.AccountId: AccountId = ((string)val).ToInt32(); break;
                case StatsType.Experience: Experience = (int)val; break;
                case StatsType.ExperienceGoal: ExperienceGoal = (int)val; break;
                case StatsType.Level: Level = (int)val; break;
                case StatsType.Fame: Fame = (int)val; break;
                case StatsType.CurrentFame: CurrentFame = (int)val; break;
                case StatsType.FameGoal: FameGoal = (int)val; break;
                case StatsType.Stars: Stars = (int)val; break;
                case StatsType.Guild: Guild = (string)val; break;
                case StatsType.GuildRank: GuildRank = (int)val; break;
                case StatsType.Credits: Credits = (int)val; break;
                case StatsType.NameChosen: NameChosen = (int)val != 0; break;
                case StatsType.Texture1: Texture1 = (int)val; break;
                case StatsType.Texture2: Texture2 = (int)val; break;
                case StatsType.Skin: Skin = (int)val; break;
                case StatsType.Glow: Glow = (int)val; break;
                case StatsType.MP: MP = (int)val; break;
                case StatsType.Inventory0: Inventory[0] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory1: Inventory[1] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory2: Inventory[2] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory3: Inventory[3] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory4: Inventory[4] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory5: Inventory[5] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory6: Inventory[6] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory7: Inventory[7] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory8: Inventory[8] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory9: Inventory[9] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory10: Inventory[10] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.Inventory11: Inventory[11] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack0: Inventory[12] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack1: Inventory[13] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack2: Inventory[14] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack3: Inventory[15] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack4: Inventory[16] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack5: Inventory[17] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack6: Inventory[18] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.BackPack7: Inventory[19] = (int)val == -1 ? null : items[(ushort)(int)val]; break;
                case StatsType.MaximumHP: Stats.Base[0] = (int)val; break;
                case StatsType.MaximumMP: Stats.Base[1] = (int)val; break;
                case StatsType.Attack: Stats.Base[2] = (int)val; break;
                case StatsType.Defense: Stats.Base[3] = (int)val; break;
                case StatsType.Speed: Stats.Base[4] = (int)val; break;
                case StatsType.Dexterity: Stats.Base[5] = (int)val; break;
                case StatsType.Vitality: Stats.Base[6] = (int)val; break;
                case StatsType.Wisdom: Stats.Base[7] = (int)val; break;
                case StatsType.DamageMin: Stats.Base[8] = (int)val; break;
                case StatsType.DamageMax: Stats.Base[9] = (int)val; break;
                case StatsType.Luck: Stats.Base[10] = (int)val; break;
                case StatsType.CriticalDmg: Stats.Base[11] = (int)val; break;
                case StatsType.CriticalHit: Stats.Base[12] = (int)val; break;
                case StatsType.HealthStackCount: HealthPots.Count = (int)val; break;
                case StatsType.MagicStackCount: MagicPots.Count = (int)val; break;
                case StatsType.HasBackpack: HasBackpack = (int)val == 1; break;
                case StatsType.XPBoostTime: XPBoostTime = (int)val * 1000; break;
                case StatsType.LDBoostTime: LDBoostTime = (int)val * 1000; break;
                case StatsType.LTBoostTime: LTBoostTime = (int)val * 1000; break;
                case StatsType.Rank: Rank = (int)val; break;
                case StatsType.Admin: Admin = (int)val; break;
                case StatsType.Tokens: Tokens = (int)val; break;
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            var chr = _client.Character;

            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId.ToString();
            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;
            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;
            stats[StatsType.Guild] = Guild;
            stats[StatsType.GuildRank] = GuildRank;
            stats[StatsType.Credits] = Credits;
            stats[StatsType.NameChosen] = // check from account in case ingame registration
                (_client.Account?.NameChosen ?? NameChosen) ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;
            stats[StatsType.Skin] = Skin;
            stats[StatsType.Glow] = Glow;
            stats[StatsType.MP] = MP;
            stats[StatsType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.Inventory11] = Inventory[11]?.ObjectType ?? -1;
            stats[StatsType.BackPack0] = Inventory[12]?.ObjectType ?? -1;
            stats[StatsType.BackPack1] = Inventory[13]?.ObjectType ?? -1;
            stats[StatsType.BackPack2] = Inventory[14]?.ObjectType ?? -1;
            stats[StatsType.BackPack3] = Inventory[15]?.ObjectType ?? -1;
            stats[StatsType.BackPack4] = Inventory[16]?.ObjectType ?? -1;
            stats[StatsType.BackPack5] = Inventory[17]?.ObjectType ?? -1;
            stats[StatsType.BackPack6] = Inventory[18]?.ObjectType ?? -1;
            stats[StatsType.BackPack7] = Inventory[19]?.ObjectType ?? -1;
            stats[StatsType.MaximumHP] = Stats[0]; //FIX
            stats[StatsType.MaximumMP] = Stats[1];// + chr.ManaPotsMoon;
            stats[StatsType.Attack] = Stats[2];// + chr.AttackStatsMoon;
            stats[StatsType.Defense] = Stats[3];// + chr.DefensePotsMoon;
            stats[StatsType.Speed] = Stats[4];// + chr.SpeedPotsMoon;
            stats[StatsType.Dexterity] = Stats[5];// + chr.DexterityPotsMoon;
            stats[StatsType.Vitality] = Stats[6];// + chr.VitalityPotsMoon;
            stats[StatsType.Wisdom] = Stats[7];// + chr.WisdomPotsMoon;
            stats[StatsType.DamageMin] = Stats[8];
            stats[StatsType.DamageMax] = Stats[9];
            stats[StatsType.Luck] = Stats[10];
            stats[StatsType.CriticalDmg] = Stats[11];// + chr.CritDmgPotsMoon;
            stats[StatsType.CriticalHit] = Stats[12];// + chr.CritHitPotsMoon;
            stats[StatsType.HPBoost] = Stats.Boost[0];
            stats[StatsType.MPBoost] = Stats.Boost[1];
            stats[StatsType.AttackBonus] = Stats.Boost[2];
            stats[StatsType.DefenseBonus] = Stats.Boost[3];
            stats[StatsType.SpeedBonus] = Stats.Boost[4];
            stats[StatsType.DexterityBonus] = Stats.Boost[5];
            stats[StatsType.VitalityBonus] = Stats.Boost[6];
            stats[StatsType.WisdomBonus] = Stats.Boost[7];
            stats[StatsType.DamageMinBonus] = Stats.Boost[8];
            stats[StatsType.DamageMaxBonus] = Stats.Boost[9];
            stats[StatsType.LuckBonus] = Stats.Boost[10];
            stats[StatsType.CriticalDmgBonus] = Stats.Boost[11];
            stats[StatsType.CriticalHitBonus] = Stats.Boost[12];
            stats[StatsType.HealthStackCount] = HealthPots.Count;
            stats[StatsType.MagicStackCount] = MagicPots.Count;
            stats[StatsType.HasBackpack] = (HasBackpack) ? 1 : 0;
            stats[StatsType.XPBoost] = (XPBoostTime != 0) ? 1 : 0;
            stats[StatsType.XPBoostTime] = XPBoostTime / 1000;
            stats[StatsType.LDBoostTime] = LDBoostTime / 1000;
            stats[StatsType.LTBoostTime] = LTBoostTime / 1000;
            stats[StatsType.OxygenBar] = OxygenBar;
            stats[StatsType.Rank] = Rank;
            stats[StatsType.Admin] = Admin;
            stats[StatsType.Tokens] = Tokens;
        }

        public void SaveToCharacter()
        {
            var chr = _client.Character;
            chr.Level = Level;
            chr.Experience = Experience;
            chr.Fame = Fame;
            chr.HP = Math.Max(1, HP);
            chr.MP = MP;
            chr.Stats = Stats.Base.GetStats();
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Skin = _originalSkin;
            chr.FameStats = FameCounter.Stats.Write();
            chr.LastSeen = DateTime.Now;
            chr.HealthStackCount = HealthPots.Count;
            chr.MagicStackCount = MagicPots.Count;
            chr.HasBackpack = HasBackpack;
            chr.XPBoostTime = XPBoostTime;
            chr.LDBoostTime = LDBoostTime;
            chr.LTBoostTime = LTBoostTime;
            chr.PetId = Pet?.PetId ?? 0;
            chr.Items = Inventory.GetItemTypes();
        }

        public Player(Client client, bool saveInventory = true)
            : base(client.Manager, client.Character.ObjectType)
        {
            var settings = Manager.Resources.Settings;
            var gameData = Manager.Resources.GameData;

            _client = client;

            // found in player.update partial
            Sight = new Sight(this);
            _clientEntities = new UpdatedSet(this);

            _accountId = new SV<int>(this, StatsType.AccountId, client.Account.AccountId, true);
            _experience = new SV<int>(this, StatsType.Experience, client.Character.Experience, true);
            _experienceGoal = new SV<int>(this, StatsType.ExperienceGoal, 0, true);
            _level = new SV<int>(this, StatsType.Level, client.Character.Level);
            _currentFame = new SV<int>(this, StatsType.CurrentFame, client.Account.Fame, true);
            _fame = new SV<int>(this, StatsType.Fame, client.Character.Fame, true);
            _fameGoal = new SV<int>(this, StatsType.FameGoal, 0, true);
            _stars = new SV<int>(this, StatsType.Stars, 0);
            _guild = new SV<string>(this, StatsType.Guild, "");
            _guildRank = new SV<int>(this, StatsType.GuildRank, -1);
            _credits = new SV<int>(this, StatsType.Credits, client.Account.Credits, true);
            _nameChosen = new SV<bool>(this, StatsType.NameChosen, client.Account.NameChosen, false, v => _client.Account?.NameChosen ?? v);
            _texture1 = new SV<int>(this, StatsType.Texture1, client.Character.Tex1);
            _texture2 = new SV<int>(this, StatsType.Texture2, client.Character.Tex2);
            _skin = new SV<int>(this, StatsType.Skin, 0);
            _glow = new SV<int>(this, StatsType.Glow, 0);
            _mp = new SV<int>(this, StatsType.MP, client.Character.MP);
            _hasBackpack = new SV<bool>(this, StatsType.HasBackpack, client.Character.HasBackpack, true);
            _xpBoosted = new SV<bool>(this, StatsType.XPBoost, client.Character.XPBoostTime != 0, true);
            _oxygenBar = new SV<int>(this, StatsType.OxygenBar, -1, true);
            _rank = new SV<int>(this, StatsType.Rank, client.Account.Rank);
            _admin = new SV<int>(this, StatsType.Admin, client.Account.Admin ? 1 : 0);
            _tokens = new SV<int>(this, StatsType.Tokens, client.Account.Tokens, true);

            Name = client.Account.Name;
            HP = client.Character.HP;
            ConditionEffects = 0;

            XPBoostTime = client.Character.XPBoostTime;
            LDBoostTime = client.Character.LDBoostTime;
            LTBoostTime = client.Character.LTBoostTime;

            var s = (ushort)client.Character.Skin;
            if (gameData.Skins.Keys.Contains(s))
            {
                SetDefaultSkin(s);
                SetDefaultSize(gameData.Skins[s].Size);
            }

            var guild = Manager.Database.GetGuild(client.Account.GuildId);
            if (guild?.Name != null)
            {
                Guild = guild.Name;
                GuildRank = client.Account.GuildRank;
            }
            var chr = _client.Character;
            HealthPots = new ItemStacker(this, 254, 0x0A22,
                client.Character.HealthStackCount, 3 + chr.Toolbelts);
            MagicPots = new ItemStacker(this, 255, 0x0A23,
                client.Character.MagicStackCount, 3 + chr.Toolbelts);
            Stacks = new ItemStacker[] { HealthPots, MagicPots };

            // inventory setup
            DbLink = new DbCharInv(Client.Account, Client.Character.CharId);
            Inventory = new Inventory(this,
                Utils.ResizeArray(
                    (DbLink as DbCharInv).Items
                        .Select(_ => (_ == 0xffff || !gameData.Items.ContainsKey(_)) ? null : gameData.Items[_])
                        .ToArray(),
                    settings.InventorySize)
                );

            if (!saveInventory)
                DbLink = null;

            Stats = new StatsManager(this);
            Inventory.InventoryChanged += (sender, e) =>
            {
                UpdateSteal();
                Stats.ReCalculateValues(e);
                OnEquippedChanged();
            };

            SlotTypes = Utils.ResizeArray(
                gameData.Classes[ObjectType].SlotTypes,
                settings.InventorySize);

            // set size of player if using set skin
            var skin = (ushort)Skin;
            if (gameData.SkinTypeToEquipSetType.ContainsKey(skin))
            {
                var setType = gameData.SkinTypeToEquipSetType[skin];
                var ae = gameData.EquipmentSets[setType].ActivateOnEquipAll
                    .SingleOrDefault(e => e.SkinType == skin);

                if (ae != null)
                    Size = ae.Size;
            }

            // override size
            if (Client.Account.Size > 0)
                Size = Client.Account.Size;

            Manager.Database.IsMuted(client.IP)
                .ContinueWith(t =>
                {
                    Muted = !Client.Account.Admin && t.IsCompleted && t.Result;
                });

            Manager.Database.IsLegend(AccountId)
                .ContinueWith(t =>
                {
                    Glow = t.Result && client.Account.GlowColor == 0
                        ? 0xFF0000
                        : client.Account.GlowColor;
                });

            done_checking = false;
            tickTimes = 0;
            missed = 0;
        }

        byte[,] tiles;
        public FameCounter FameCounter { get; private set; }

        public Entity SpectateTarget { get; set; }
        public bool IsControlling => SpectateTarget != null && !(SpectateTarget is Player);
        
        public int[] LifeSteal { get; private set;}
        public int[] MagicSteal { get; private set; }
        //CurseEffect
        public int LifeStealSum() => LifeSteal.Sum();
        public int MagicStealSum() => MagicSteal.Sum();

        public void UpdateSteal()
        {
            if (LifeSteal == null || MagicSteal == null)
            {
                LifeSteal = new int[4];
                MagicSteal = new int[4];
            }

            for (var i = 0; i < 4; i++)
            {
                LifeSteal[i] = 0;
                MagicSteal[i] = 0;

                var item = Inventory[i];
                if (item != null)
                {
                    foreach (var steal in item.Steal)
                    {
                        if (steal.Key == "life")
                            LifeSteal[i] = steal.Value;
                        else if (steal.Key == "mana")
                            MagicSteal[i] = steal.Value;
                    }
                }
            }
        }

        public bool CurseEffect()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item != null)
                {
                   if (item.CurseEffect)
                   {
                       return true;
                   }
                }
            }
            return false;
        }
        public bool ParalyzeEffect()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item != null)
                {
                    if (item.ParalyzeEffect)
                    {
                        return true;
                    }
                }
            }
            return false;//WisDmgEffect
        }
        public bool WisDmgEffect()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item != null)
                {
                    if (item.WisDmgEffect)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool TouchEffect()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item != null)
                {
                    if (item.TouchEffect)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void LifeManaSteal(Entity from, int dmg)
        {
            if (dmg < 10)
                return;

            var lifeSteal = LifeStealSum();
            var magicSteal = MagicStealSum();

            if (lifeSteal > 0)
            {
                if (from.HasConditionEffect(ConditionEffects.Sick) || HasConditionEffect(ConditionEffects.Bleeding))
                    return;

                var maxHP = Stats[0];
                var totalSteal = maxHP > HP + lifeSteal ? lifeSteal : maxHP - HP;

                HP = HP + totalSteal;
                if (totalSteal > 0)
                    Client.SendPacket(Flow(new ARGB(0xFF0000), from));
            }

            if (magicSteal > 0)
            {
                if (from.HasConditionEffect(ConditionEffects.Quiet) || from.HasConditionEffect(ConditionEffects.Sick) || HasConditionEffect(ConditionEffects.ManaBurn))
                    return;

                var maxMP = Stats[1];
                var totalSteal = maxMP > MP + magicSteal ? magicSteal : maxMP - MP;

                MP = MP + totalSteal;
                if (totalSteal > 0)
                    Client.SendPacket(Flow(new ARGB(0x0000FF), from));
            }
        }

        public ShowEffect Flow(ARGB color, Entity from) => new ShowEffect()
        {
            EffectType = EffectType.Flow,
            TargetObjectId = Id,
            Pos1 = new Position() { X = from.X, Y = from.Y },
            Color = color
        };

        public void ResetFocus(object target, EventArgs e)
        {
            var entity = target as Entity;
            entity.FocusLost -= ResetFocus;
            entity.Controller = null;

            if (Owner == null)
                return;

            SpectateTarget = null;
            Owner.Timers.Add(new WorldTimer(3000, (w, t) =>
                ApplyConditionEffect(ConditionEffectIndex.Paused, 0)));
            Client.SendPacket(new SetFocus()
            {
                ObjectId = Id
            });
        }

        public override void Init(World owner)
        {
            var eventsInfo = Program.Config.eventsInfo;
            var x = 0;
            var y = 0;

            var chr = _client.Character;

            var spawnRegions = owner.GetSpawnPoints();
            if (spawnRegions.Any())
            {
                var sRegion = spawnRegions.ElementAt(RandomUtil.RandInt(0, spawnRegions.Length));
                x = sRegion.Key.X;
                y = sRegion.Key.Y;
            }
            Move(x + 0.5f, y + 0.5f);
            tiles = new byte[owner.Map.Width, owner.Map.Height];

            ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 5000);
            UpdateSteal();
            OnEquippedChanged();

            if (Client.Account.DonorClaim == false && Client.Account.Rank <= 4)
            {
                Client.Account.DonorClaim = true;
            }

            if (Client.Account.WelcomeBack == false)
            {
                SendInfo($"You have unclaimed packages, /Claim to claim them.");
            }
            if (Client.Account.SkinsClaimed == false)
            {
                SendInfo($"You have unclaimed packages, /Claim to claim them.");
            }
            else if (Client.Account.DonorClaim == false && Client.Account.Rank <= 40 && Client.Account.Rank >= 5)
            {
                SendInfo($"You have unclaimed packages, /Claim to claim them.");
            }

            if (owner.Name.Equals("Nexus"))
            {
                    HealthPots = new ItemStacker(
                  this,
                  254,
                  0x0A22,
                  chr.Toolbelts,
                  chr.Toolbelts);

                    MagicPots = new ItemStacker(
                        this,
                        255,
                        0x0A23,
                        chr.Toolbelts,
                        chr.Toolbelts);
            }

            // spawn pet if player has one attached
            var petId = chr.PetId;
            if (petId > 0 && Manager.Config.serverSettings.enablePets)
            {
                var dbPet = new DbPet(Client.Account, petId);
                if (dbPet.ObjectType != 0)
                {
                    var pet = new Pet(Manager, this, dbPet);
                    pet.Move(X, Y);
                    owner.EnterWorld(pet);
                    Pet = pet;
                }
            }

            FameCounter = new FameCounter(this);
            FameGoal = GetFameGoal(FameCounter.ClassStats[ObjectType].BestFame);
            ExperienceGoal = GetExpGoal(chr.Level);
            Stars = GetStars();

            if (owner.Name.Equals("Ocean Trench"))
                OxygenBar = 100;
            if (owner.Name.Equals("Deep Sea"))
                OxygenBar = 100;


            SetNewbiePeriod();

           if (owner.Name.Equals("Moon") && chr.MoonPrimed == false)
           {
                ReconnectToNexus2();
                SendInfo($"The gods demand you to ascend before adventuring to the moon.");
           }
           totalMoonPots = chr.LifePotsMoon + chr.ManaPotsMoon + chr.AttackStatsMoon + chr.DefensePotsMoon + chr.AttackStatsMoon + chr.DexterityPotsMoon + chr.VitalityPotsMoon + chr.WisdomPotsMoon + chr.CritDmgPotsMoon + chr.CritHitPotsMoon;


            if (owner.IsNotCombatMapArea)
            {
                Client.SendPacket(new GlobalNotification
                {
                    Text = Client.Account.Gifts.Length > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });

                if (DeathArena.Instance?.CurrentState != DeathArena.ArenaState.NotStarted && DeathArena.Instance?.CurrentState != DeathArena.ArenaState.Ended)
                {
                    Client.SendPacket(new GlobalNotification
                    {
                        Type = GlobalNotification.ADD_ARENA,
                        Text = $"{{\"name\":\"Oryx Arena\",\"open\":{DeathArena.Instance?.CurrentState == DeathArena.ArenaState.CountDown}}}"
                    });
                }

                if (worlds.logic.Arena.Instance?.CurrentState != worlds.logic.Arena.ArenaState.NotStarted)
                {
                    Client.SendPacket(new GlobalNotification
                    {
                        Type = GlobalNotification.ADD_ARENA,
                        Text = $"{{\"name\":\"Public Arena\",\"open\":{worlds.logic.Arena.Instance?.CurrentState == worlds.logic.Arena.ArenaState.CountDown}}}"
                    });
                }
            }
            if (!owner.SafeMapArea)
            {
                ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 1000);
            }

            if (eventsInfo.StatBoost.Item1)
            {
                if(eventsInfo.StatBoost.Item3 != 0)
                {
                    Stats.Boost.ActivateBoost[eventsInfo.StatBoost.Item2].Push(eventsInfo.StatBoost.Item3);
                    Stats.ReCalculateValues();
                }
            }
            base.Init(owner);
        }//Deeparmor

        public void SendProjectile(RealmTime time)
        {
            if (tickTimes < 30) return;
            var entity = Resolve(Manager, "GhostMaree");
            var prjDesc = entity.ObjectDesc.Projectiles[0];
            Position pos = new Position() { X = X, Y = Y };
            Projectile proj = CreateProjectile(prjDesc, entity.ObjectType, 1, time.TotalElapsedMs, pos, 0);
            Client.SendPacket(new ServerPlayerShoot()
            {
                BulletId = proj.ProjectileId,
                OwnerId = Id,
                ContainerType = entity.ObjectType,
                StartingPos = pos,
                Angle = proj.Angle,
                Damage = (short)proj.Damage
            });
            Owner?.Timers.Add(new WorldTimer(3000, (world, t) =>
            {
                if (_client.Player != null)
                    if (!proj._used)
                        missed++;
                done_checking = false;
            }));
            tickTimes = 0;
            done_checking = true;
        }
        void HandleGodMode(RealmTime time)
        {
            var entity = Resolve(Manager, "GhostMaree");
            var prjDesc = entity.ObjectDesc.Projectiles[0];
            var batch = new Packet[4];
            Position pos = new Position() { X = X, Y = Y };
            var prjs = new Projectile[4];
            for (var i = 0; i < 4; i++)
            {
                Projectile proj = CreateProjectile(prjDesc, entity.ObjectType, 1, time.TotalElapsedMs, pos, 0);
                batch[i] = new ServerPlayerShoot()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Id,
                    ContainerType = entity.ObjectType,
                    StartingPos = pos,
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                };
                if (proj != null)
                    Owner?.EnterWorld(proj);
                prjs[i] = proj;
                Client.SendPacket(batch[i]);
            }

            Owner?.Timers.Add(new WorldTimer(1000, (world, t) =>
            {
                if (_client.Player != null)
                    for (var i = 0; i < 4; i++)
                        if (prjs[i] != null)
                            if (!prjs[i]._used)
                                missed++;
            }));
        }

        public override void Tick(RealmTime time)
        {
            if (!KeepAlive(time) || Client.State == ProtocolState.Reconnecting)
                return;

            HandleSpecialEnemies(time);
            HandleQuest(time);
            CheckTradeTimeout(time);

            if (!HasConditionEffect(ConditionEffects.Paused))
            {
                HandleToolBelt(time);
                EquippedStatusTick(time);
                HandleRegen(time);
                HandleEffects(time);
                HandleOceanTrenchGround(time);
                TickActivateEffects(time);
                FameCounter.Tick(time);
            }
            if (DecoyStillActive == true)
            {
                if (DecoyCooledDown() == true)
                {
                    DecoyStillActive = false;
                }
            }

            var chr = _client.Character;
            if (chr.MoonPrimed == true)
            {
                if (CanEnterMoon() == true)
                {
                    chr.MoonPrimed = false;
                }
            }

            if (Stats.Base[10] >= 50)
            {
                if (Rank < 80)
                    Stats.Base[10] = 50;
            }

            if (Rn4g == 1)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Healing, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 2)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Damaging, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 3)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Berserk, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 4)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Invulnerable, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 5)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Armored, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 6)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Barrier, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 7)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Enchanted, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 8)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Strength, DurationMS = 3000 });
                Rn4g = 20;
            }
            if (Rn4g == 9)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Awoken, DurationMS = 3000 });
                Rn4g = 20;
            }
            else if (Rn4g == 10)
            {
                ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Haste, DurationMS = 3000 });
                Rn4g = 20;
            }

            base.Tick(time);

            SendUpdate(time);
            SendNewTick(time);

            if (Owner != null && !Owner.SafeMapArea && HP <= 0)
            {
                Death("Unknown", rekt: true);
                return;
            }
        }

        void TickActivateEffects(RealmTime time)
        {
            var dt = time.ElaspedMsDelta;

            if (Client.Player.Owner.SafeMapArea == true)
                return;

            if (XPBoostTime != 0)

                if (XPBoostTime > 0)
                    XPBoostTime = Math.Max(XPBoostTime - dt, 0);
            if (XPBoostTime == 0)
                XPBoosted = false;

            if (LDBoostTime > 0)
                LDBoostTime = Math.Max(LDBoostTime - dt, 0);

            if (LTBoostTime > 0)
                LTBoostTime = Math.Max(LTBoostTime - dt, 0);
        }

        float _hpRegenCounter;
        float _mpRegenCounter;
        void HandleToolBelt(RealmTime time)
        {
            var chr = _client.Character;
            if (chr.Regeneration > 0 && (Stacks[0].Count < 3 + chr.Toolbelts || Stacks[1].Count < 3 + chr.Toolbelts))
            {
                if (_canTpCooldownTime13 <= 0)
                    _canTpCooldownTime13 = chr.Regeneration * 60;
                if (_canTpCooldownTime13 >= 0)
                    _canTpCooldownTime13 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime13 <= 0)
                {
                    if (Stacks[0].Count < 3 + chr.Toolbelts)
                        Stacks[0].Put(Stacks[0].Item);
                    if (Stacks[1].Count < 3 + chr.Toolbelts)
                        Stacks[1].Put(Stacks[1].Item);
                    SendInfo("Your toolbelt feels a bit heavier...");
                }
            }
        }
        public int HPRegenADD;
        void HandleRegen(RealmTime time)
        {
            if (HPRegenADD < 0)
            {
                HPRegenADD = 0;
            }
            // hp regen
            if (HP == Stats[0] || !CanHpRegen())
                _hpRegenCounter = 0 + HPRegenADD;


            else
            {
                _hpRegenCounter += Stats.GetHPRegen() * time.ElaspedMsDelta / 1000f;
                var regen = (int)_hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0], HP + regen);
                    _hpRegenCounter -= regen;
                }
            }
            if (MP == Stats[1] || !CanMpRegen())
                _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += Stats.GetMPRegen() * time.ElaspedMsDelta / 1000f;
                var regen = (int)_mpRegenCounter;
                if (regen > 0)
                {
                    MP = Math.Min(Stats[1], MP + regen);
                    _mpRegenCounter -= regen;
                }
            }
        }

        public void TeleportPosition(RealmTime time, float x, float y, bool ignoreRestrictions = false)
        {
            TeleportPosition(time, new Position() { X = x, Y = y }, ignoreRestrictions);
        }

        public void TeleportPosition(RealmTime time, Position position, bool ignoreRestrictions = false)
        {



            FameCounter.Teleport();
            HandleQuest(time, true, position);

            var id = (IsControlling) ? SpectateTarget.Id : Id;
            var tpPkts = new Packet[] {
                new Goto () {
                ObjectId = id,
                Pos = position
                },
                new ShowEffect () {
                EffectType = EffectType.Teleport,
                TargetObjectId = id,
                Pos1 = position,
                Color = new ARGB (0xFFFFFFFF)
                }
            };
            foreach (var plr in Owner.Players.Values)
            {
                plr.AwaitGotoAck(time.TotalElapsedMs);
                plr.Client.SendPackets(tpPkts, PacketPriority.Low);
            }
        }

        public void Teleport(RealmTime time, int objId, bool ignoreRestrictions = false)
        {
            var obj = Owner.GetEntity(objId);
            if (obj == null)
            {
                SendError("Target does not exist.");
                return;
            }

            if (!ignoreRestrictions)
            {
                if (Id == objId)
                {
                    SendInfo("You are already at yourself, and always will be!");
                    return;
                }

                if (!Owner.AllowTeleport)
                {
                    SendError("Cannot teleport here.");
                    return;
                }

                if (HasConditionEffect(ConditionEffects.Paused))
                {
                    SendError("Cannot teleport while paused.");
                    return;
                }

                if (!(obj is Player))
                {
                    SendError("Can only teleport to players.");
                    return;
                }

                if (!TPCooledDown())
                {
                    SendError("Too soon to teleport again!");
                    return;
                }


                SetTPDisabledPeriod();
                SetNewbiePeriod();
            }
               
            
            ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 3500);
            ApplyConditionEffect(ConditionEffectIndex.Stunned, 3500);
            TeleportPosition(time, obj.X, obj.Y, false);
        }

        public bool IsInvulnerable() => HasConditionEffect(ConditionEffects.Paused) ||
                HasConditionEffect(ConditionEffects.Stasis) ||
                HasConditionEffect(ConditionEffects.Invincible) ||
                HasConditionEffect(ConditionEffects.Invulnerable) ||
                UltraInstinct;
        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player || IsInvulnerable())
                return false;

            var dmg = (int)Stats.GetDefenseDamage(projectile.Damage, projectile.ProjDesc.ArmorPiercing);

            if (!Manager.Resources.Settings.DisableAlly)
                Owner.BroadcastPacketNearby(new Damage()
                {
                    TargetId = this.Id,
                    Effects = HasConditionEffect(ConditionEffects.Invincible) ? 0 : projectile.ConditionEffects,
                    DamageAmount = (ushort)dmg,
                    Kill = HP <= 0,
                    BulletId = projectile.ProjectileId,
                    ObjectId = projectile.ProjectileOwner.Self.Id
                }, this, this, PacketPriority.Low);

            ApplyConditionEffect(projectile.ProjDesc.Effects);

            if (!HasConditionEffect(ConditionEffects.Invulnerable) && !UltraInstinct)
            {
                HP -= dmg;
                EquippedStatusHit(dmg);
            }

            if (HP <= 0 && !Owner.SafeMapArea)
                Death(projectile.ProjectileOwner.Self.ObjectDesc.DisplayId ??
                      projectile.ProjectileOwner.Self.ObjectDesc.ObjectId,
                      projectile.ProjectileOwner.Self);

            return base.HitByProjectile(projectile, time);
        }

        

        public void Damage(int dmg, Entity src)
        {
            if (IsInvulnerable() || UltraInstinct)
                return;

            dmg = (int)Stats.GetDefenseDamage(dmg, false);
            if (!HasConditionEffect(ConditionEffects.Invulnerable) || UltraInstinct == false)
                HP -= dmg;
            if (!Manager.Resources.Settings.DisableAlly)
                Owner.BroadcastPacketNearby(new Damage()
                {
                    TargetId = Id,
                    Effects = 0,
                    DamageAmount = (ushort)dmg,
                    Kill = HP <= 0,
                    BulletId = 0,
                    ObjectId = src.Id
                }, this, this, PacketPriority.Low);

            if (HP <= 0 && !Owner.SafeMapArea)
                Death(src.ObjectDesc.DisplayId ??
                      src.ObjectDesc.ObjectId,
                      src);
        }

        public void GrenadeDamage(int dmg, string name)
        {
            if (IsInvulnerable() || UltraInstinct)
                return;
            dmg = (int)Stats.GetDefenseDamage(dmg, false);
            if (!HasConditionEffect(ConditionEffects.Invulnerable) || UltraInstinct == false)
                HP -= dmg;
            if (HP <= 0 && !Owner.SafeMapArea)
                Death(name);
        }
        public int statsmaxed;
        void GenerateGravestone(bool phantomDeath = false) //donators
        {


            var playerDesc = Manager.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] == t.MaxValue).Count();
            ushort objType;
            int time;
            var chr = _client.Character;
            switch (maxed)
            {
                case 10: objType = 0x6459; time = 600000; break;
                case 9: objType = 0x6472; time = 600000; break;
                case 8: objType = 0x0735; time = 600000; break;
                case 7: objType = 0x0734; time = 600000; break;
                case 6: objType = 0x072b; time = 600000; break;
                case 5: objType = 0x072a; time = 600000; break;
                case 4: objType = 0x0729; time = 600000; break;
                case 3: objType = 0x0728; time = 600000; break;
                case 2: objType = 0x0727; time = 600000; break;
                case 1: objType = 0x0726; time = 600000; break;
                default:
                    objType = 0x0725; time = 300000;
                    if (Level < 20) { objType = 0x0724; time = 60000; }
                    if (Level <= 1) { objType = 0x0723; time = 30000; }
                    break;
            }
            statsmaxed = maxed;
            if (chr.MoonPrimed)
            {
                maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= (t.MaxValue + 10)).Count();
                switch (maxed)
                {
                    case 12: objType = 0x6470; time = 120000; break;
                    case 11: objType = 0x6470; time = 120000; break;
                    case 10: objType = 0x6470; time = 120000; break;
                    case 9: objType = 0x6469; time = 600000; break;
                    case 8: objType = 0x6468; time = 600000; break;
                    case 7: objType = 0x6467; time = 600000; break;
                    case 6: objType = 0x6465; time = 600000; break;
                    case 5: objType = 0x6464; time = 600000; break;
                    case 4: objType = 0x6463; time = 600000; break;
                    case 3: objType = 0x6462; time = 600000; break;
                    case 2: objType = 0x6461; time = 600000; break;
                    case 1: objType = 0x6460; time = 600000; break;
                    case 0: objType = 0x6459; time = 600000; break;
                    
                }
                statsmaxed = maxed + 10;
                if (statsmaxed > 20)
                {
                    statsmaxed = 20;
                }
            }
            var obj = new StaticObject(Manager, objType, time, true, true, false);
            obj.Move(X, Y);
            obj.Name = (!phantomDeath) ? Name + " ("+ statsmaxed + "/20, " + Client.Character.FinalFame + ")" : $"{Name} got rekt";
            Owner.EnterWorld(obj);
        }

        private bool Arena(string killer)
        {
            if (!(Owner is Arena) && !(Owner is ArenaSolo))
                return false;

            foreach (var player in Owner.Players.Values)
                player.SendInfo("{\"key\":\"{arena.death}\",\"tokens\":{\"player\":\"" + Name + "\",\"enemy\":\"" + killer + "\"}}");

            ReconnectToNexus();
            return true;
        }

        private bool NonPermaKillEnemy(Entity entity, string killer)
        {
            if (entity == null)
            {
                return false;
            }

            if (!entity.Spawned && entity.Controller == null)
                return false;

            foreach (var player in Owner.Players.Values)
                player.SendInfo(Name + " was sent home crying by a phantom " + killer);

            GenerateGravestone(true);
            ReconnectToNexus();
            return true;
        }

        private bool Rekted(bool rekt)
        {
            if (!rekt)
                return false;

            GenerateGravestone(true);
            ReconnectToNexus();
            return true;
        }

        private bool TestWorld(string killer)
        {
            if (!(Owner is Test))
                return false;

            GenerateGravestone();
            ReconnectToNexus();
            return true;
        }

        bool _dead;
        bool Resurrection()
        {
            for (int i = 0; i < 4; i++)
            {
                var item = Inventory[i];

                if (item == null || !item.Resurrects)
                    continue;

                Inventory[i] = null;
                foreach (var player in Owner.Players.Values)
                    player.SendInfo($"{Name}'s {item.DisplayName} breaks and he disappears leaving his gravestone");

                ReconnectToNexus();
                return true;
            }
            return false;
        }
        private void ReconnectToNexus2()
        {
            HP = 250;
            _client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Nexus,
                Name = "Nexus"
            });
        }
        private void ReconnectToNexus()
        {
            HP = 250;
            _client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = World.Nexus,
                Name = "Nexus"
            });
        }

        private void AnnounceDeath(string killer, Inventory inventory)
        {
            var playerDesc = Manager.Resources.GameData.Classes[ObjectType];
            var maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= t.MaxValue).Count();
            var deathMessage = string.Format(
                "{{\"key\":\"{{server.death}}\",\"tokens\":{{\"player\":\"{0}\",\"level\":\"{1}\",\"fame\":\"{2}\",\"maxed\":\"{3}\",\"enemy\":\"{4}\"}}}}",
                Name, Level, _client.Character.FinalFame, maxed, killer);
            int toMax = 10;
            // notable deaths
            if ((maxed >= 6 || Fame >= 1000) && !Client.Account.Admin)
            {
                string items = "";
                foreach(var item in inventory)
                    if (item != null)
                        items += item.DisplayName + ", ";
                if (_client.Character.MoonPrimed)
                {
                    maxed = playerDesc.Stats.Where((t, i) => Stats.Base[i] >= (t.MaxValue + 10)).Count();
                    statsmaxed = maxed + 10;
                    if (statsmaxed > 20)
                    {
                        statsmaxed = 20;
                    }
                    toMax = 20;
                }
                foreach (var w in Manager.Worlds.Values)
                    foreach (var p in w.Players.Values)
                        p.SendHelp(deathMessage);
                return;
            }

            var pGuild = Client.Account.GuildId;

            // guild case, only for level 20
            if (pGuild > 0 && Level == 20)
            {
                foreach (var w in Manager.Worlds.Values)
                {
                    foreach (var p in w.Players.Values)
                    {
                        if (p.Client.Account.GuildId == pGuild)
                        {
                            p.SendGuildDeath(deathMessage);
                        }
                    }
                }

                foreach (var i in Owner.Players.Values)
                {
                    if (i.Client.Account.GuildId != pGuild)
                    {
                        i.SendInfo(deathMessage);
                    }
                }
            }
            // guild less case
            else
            {
                foreach (var i in Owner.Players.Values)
                {
                    i.SendInfo(deathMessage);
                }
            }

        }

        public void Death(string killer, Entity entity = null, WmapTile tile = null, bool rekt = false)
        {
            if (_client.State == ProtocolState.Disconnected || Client.State == ProtocolState.Reconnecting || _dead)
                return;

            if (entity != null)
            {
                if (entity.PlayerSpawned)
                    rekt = true;
            }

            if (Owner.Id == -2) //Nexus has the ID -2, according to your Nexus.jw
            {
                HP += 100; //Meh, we dont want people to rejoin the nexus with 0 HP, so this is fine.
                ReconnectToNexus();
                return;
            }
            else if (Rekted(rekt))
                return;
            else if (Arena(killer))
                return;
            else if (NonPermaKillEnemy(entity, killer))
                return;
            else if (TestWorld(killer))
                return;
            if (Resurrection())
            {
                _dead = false;
                return; 
            }
            else
            {
                _dead = true;
                SaveToCharacter();
                Manager.Database.Death(Manager.Resources.GameData, _client.Account,
                    _client.Character, FameCounter.Stats, killer);
            }
          

            SaveToCharacter();
            Manager.Database.Death(Manager.Resources.GameData, _client.Account,
                _client.Character, FameCounter.Stats, killer);

            GenerateGravestone();
            AnnounceDeath(killer, Inventory);

            _client.SendPacket(new Death()
            {
                AccountId = AccountId.ToString(),
                CharId = _client.Character.CharId,
                KilledBy = killer,
                ZombieId = -1
            });

            Owner.Timers.Add(new WorldTimer(1000, (w, t) =>
            {
                if (_client.Player != this)
                    return;

                _client.Disconnect();
            }));
        }

        public void Reconnect(World world)
        {
            Client.Reconnect(new Reconnect()
            {
                Host = "",
                Port = 2050,
                GameId = world.Id,
                Name = world.Name
            });
        }

        public void Reconnect(object portal, World world)
        {
            ((Portal)portal).WorldInstanceSet -= Reconnect;

            if (world == null)
                SendError("Portal Not Implemented!");
            else
                Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = world.Id,
                    Name = world.Name
                });
        }

        public int GetCurrency(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    return Credits;
                case CurrencyType.Fame:
                    return CurrentFame;
                case CurrencyType.Tokens:
                    return Tokens;
                default:
                    return 0;
            }
        }

        public void SetCurrency(CurrencyType currency, int amount)
        {
            switch (currency)
            {
                case CurrencyType.Gold:
                    Credits = amount; break;
                case CurrencyType.Fame:
                    CurrentFame = amount; break;
                case CurrencyType.Tokens:
                    Tokens = amount; break;
            }
        }

        public override void Move(float x, float y)
        {
            if (SpectateTarget != null && !(SpectateTarget is Player))
                SpectateTarget.MoveEntity(x, y);
            else
                base.Move(x, y);

            if ((int)X != Sight.LastX || (int)Y != Sight.LastY)
            {
                if (IsNoClipping())
                    _client.Manager.Logic.AddPendingAction(t => _client.Disconnect());

                Sight.UpdateCount++;
            }
        }

        public override void Dispose()
        {
            if (SpectateTarget != null)
            {
                SpectateTarget.FocusLost -= ResetFocus;
                SpectateTarget.Controller = null;
            }
            _clientEntities.Dispose();

            Pet?.Dispose();
            Pet = null;
            DbLink = null;
            Inventory = null;
            HealthPots = null;
            MagicPots = null;
            Stacks = null;

            base.Dispose();
        }

        // allow other admins to see hidden people
        public override bool CanBeSeenBy(Player player)
        {
            if (Client?.Account != null && Client.Account.Hidden)
            {
                return player.Admin != 0;
            }
            else
            {
                return true;
            }
        }

        public void SetDefaultSkin(int skin)
        {
            _originalSkin = skin;
            Skin = skin;
        }

        public void RestoreDefaultSkin()
        {
            Skin = _originalSkin;
        }
    }
}