using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.resources;
using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace common
{
    public abstract class RedisObject
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RedisObject));

        //Note do not modify returning buffer
        private Dictionary<RedisValue, KeyValuePair<byte[], bool>> _entries;

        protected void Init(IDatabase db, string key, string field = null)
        {
            Key = key;
            Database = db;

            if (field == null)
                _entries = db.HashGetAll(key)
                    .ToDictionary(
                        x => x.Name,
                        x => new KeyValuePair<byte[], bool>(x.Value, false));
            else
            {
                var entry = new HashEntry[] { new HashEntry(field, db.HashGet(key, field)) };
                _entries = entry.ToDictionary(x => x.Name, 
                    x => new KeyValuePair<byte[], bool>(x.Value, false));
            }
        }

        public IDatabase Database { get; private set; }
        public string Key { get; private set; }

        public IEnumerable<RedisValue> AllKeys
        {
            get { return _entries.Keys; }
        }

        public bool IsNull
        {
            get { return _entries.Count == 0; }
        }
        public int[] MarketOffers
        {
            get { return GetValue<int[]>("marketOffers") ?? new int[0]; }
            set { SetValue<int[]>("marketOffers", value); }
        }
        protected byte[] GetValueRaw(RedisValue key)
        {
            KeyValuePair<byte[], bool> val;
            if (!_entries.TryGetValue(key, out val))
                return null;

            if (val.Key == null)
                return null;

            return (byte[]) val.Key.Clone();
        }

        protected T GetValue<T>(RedisValue key, T def = default(T))
        {
            KeyValuePair<byte[], bool> val;
            if (!_entries.TryGetValue(key, out val) || val.Key == null)
                return def;

            if (typeof (T) == typeof (int))
                return (T) (object) int.Parse(Encoding.UTF8.GetString(val.Key));

            if (typeof (T) == typeof (uint))
                return (T) (object) uint.Parse(Encoding.UTF8.GetString(val.Key));

            if (typeof (T) == typeof (ushort))
                return (T) (object) ushort.Parse(Encoding.UTF8.GetString(val.Key));

            if (typeof (T) == typeof (bool))
                return (T) (object) (val.Key[0] != 0);

            if (typeof (T) == typeof (DateTime))
                return (T) (object) DateTime.FromBinary(BitConverter.ToInt64(val.Key, 0));

            if (typeof (T) == typeof (byte[]))
                return (T) (object) val.Key;

            if (typeof (T) == typeof (ushort[]))
            {
                var ret = new ushort[val.Key.Length/2];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T) (object) ret;
            }

            if (typeof (T) == typeof (int[]) ||
                typeof (T) == typeof (uint[]))
            {
                var ret = new int[val.Key.Length/4];
                Buffer.BlockCopy(val.Key, 0, ret, 0, val.Key.Length);
                return (T) (object) ret;
            }

            if (typeof (T) == typeof (string))
                return (T) (object) Encoding.UTF8.GetString(val.Key);

            throw new NotSupportedException();
        }

        protected void SetValue<T>(RedisValue key, T val)
        {
            byte[] buff;
            if (typeof (T) == typeof (int) || typeof (T) == typeof (uint) ||
                typeof (T) == typeof (ushort) || typeof (T) == typeof (string))
                buff = Encoding.UTF8.GetBytes(val.ToString());

            else if (typeof (T) == typeof (bool))
                buff = new byte[] {(byte) ((bool) (object) val ? 1 : 0)};

            else if (typeof (T) == typeof (DateTime))
                buff = BitConverter.GetBytes(((DateTime) (object) val).ToBinary());

            else if (typeof (T) == typeof (byte[]))
                buff = (byte[]) (object) val;

            else if (typeof (T) == typeof (ushort[]))
            {
                var v = (ushort[]) (object) val;
                buff = new byte[v.Length*2];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }

            else if (typeof (T) == typeof (int[]) ||
                     typeof (T) == typeof (uint[]))
            {
                var v = (int[]) (object) val;
                buff = new byte[v.Length*4];
                Buffer.BlockCopy(v, 0, buff, 0, buff.Length);
            }

            else
                throw new NotSupportedException();

            if (!_entries.ContainsKey(Key) || _entries[Key].Key == null || !buff.SequenceEqual(_entries[Key].Key))
                _entries[key] = new KeyValuePair<byte[], bool>(buff, true);
        }

        private List<HashEntry> _update;

        public Task FlushAsync(ITransaction transaction = null)
        {
            ReadyFlush();
            return transaction == null ?
                Database.HashSetAsync(Key, _update.ToArray()) :
                transaction.HashSetAsync(Key, _update.ToArray());
        }

        private void ReadyFlush()
        {
            if (_update == null)
                _update = new List<HashEntry>();
            _update.Clear();

            foreach (var name in _entries.Keys)
                if (_entries[name].Value)
                    _update.Add(new HashEntry(name, _entries[name].Key));

            foreach (var update in _update)
                _entries[update.Name] = new KeyValuePair<byte[], bool>(_entries[update.Name].Key, false);
        }

        public async Task ReloadAsync(ITransaction trans = null, string field = null)
        {
            if (field != null && _entries != null)
            {
                var tf = trans != null ? 
                    trans.HashGetAsync(Key, field) :
                    Database.HashGetAsync(Key, field);

                try
                {
                    await tf;
                    _entries[field] = new KeyValuePair<byte[], bool>(
                        tf.Result, false);
                }
                catch { }
                return;
            }

            var t = trans != null ?
                trans.HashGetAllAsync(Key) :
                Database.HashGetAllAsync(Key);

            try
            {
                await t;
                _entries = t.Result.ToDictionary(
                    x => x.Name, x => new KeyValuePair<byte[], bool>(x.Value, false));
            }
            catch { }
        }

        public void Reload(string field = null)
        {
            if (field != null && _entries != null)
            {
                _entries[field] = new KeyValuePair<byte[], bool>(
                    Database.HashGet(Key, field), false);
                return;
            }

            _entries = Database.HashGetAll(Key)
                .ToDictionary(
                    x => x.Name,
                    x => new KeyValuePair<byte[], bool>(x.Value, false));
        }
    }

    public class DbLoginInfo
    {
        private IDatabase db;

        internal DbLoginInfo(IDatabase db, string uuid)
        {
            this.db = db;
            UUID = uuid;
            var json = (string) db.HashGet("logins", uuid.ToUpperInvariant());
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        [JsonIgnore]
        public string UUID { get; private set; }

        [JsonIgnore]
        public bool IsNull { get; private set; }

        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public int AccountId { get; set; }

        public void Flush()
        {
            db.HashSet("logins", UUID.ToUpperInvariant(), JsonConvert.SerializeObject(this));
        }
    }
    public class DbMarketData
    {
        private IDatabase _db;

        public DbMarketData() // empty for deserialization.
        {

        }

        internal DbMarketData(IDatabase db, int id)
        {
            _db = db;

            Id = id;
            var json = (string)db.HashGet("market", id);
            if (json == null)
            {
                IsNull = true;
            }
            else
            {
                JsonConvert.PopulateObject(json, this);
            }
        }

        private static readonly Random _rand = new Random();
        public static int NextId(IDatabase db)
        {
            int id;
            do
            {
                id = _rand.Next(int.MaxValue);
            } while ((string)db.HashGet("market", id) != null);
            return id;
        }

        [JsonProperty("marketId")]
        public int Id;

        [JsonProperty("itemType")]
        public ushort ItemType;

        [JsonProperty("sellerName")]
        public string SellerName;

        [JsonProperty("sellerId")]
        public int SellerId;

        [JsonProperty("currency")]
        public CurrencyType Currency;

        [JsonProperty("price")]
        public int Price;

        [JsonProperty("slotType")]
        public int SlotType;

        [JsonIgnore]
        public bool IsNull { get; private set; }

        public void Flush()
        {
            _db.HashSet("market", Id, JsonConvert.SerializeObject(this));
        }

        private static int[] WEAPON_TYPE = new int[] { 1, 3, 17, 8, 2, 24 };
        private static int[] ABILITY_TYPE = new int[] { 13, 15, 11, 4, 16, 5, 12, 18, 19, 20, 21, 22, 23, 25 };
        private static int[] ARMOR_TYPE = new int[] { 6, 14, 7, 6 };
        private static int[] RING_TYPE = new int[] { 9 };

        public static IEnumerable<DbMarketData> GetAllMarketData(IDatabase db)
        {
            var hashSets = db.HashGetAll("market");

            foreach (var hash in hashSets)
            {
                var jsonData = JsonConvert.DeserializeObject<DbMarketData>(hash.Value);
                yield return jsonData;
            }
        }

        public static DbMarketData[] ClearMarket(Database db)
        {
            var allOffers = new List<DbMarketData>();
            var allOffer = db.Conn.HashGetAll("market");

            foreach(var i in allOffer)
            {
                var data = JsonConvert.DeserializeObject<DbMarketData>(i.Value);
                var id = db.ResolveId(data.SellerName);
                var acc = db.GetAccount(id);

                db.RemoveMarketData(acc, data.Id);

                allOffers.Add(data);
            }

            return allOffers.ToArray();
        }

        public static DbMarketData[] Get(IDatabase db, ushort itemType)
        {
            List<DbMarketData> ret = new List<DbMarketData>();
            var allOffers = db.HashGetAll("market");
            foreach (var i in allOffers)
            {
                DbMarketData l = JsonConvert.DeserializeObject<DbMarketData>(i.Value);
                if (l.ItemType == itemType)
                {
                    ret.Add(l);
                }
            }
            return ret.ToArray();
        }
    }
    public class DbAccount : RedisObject
    {
        public DbAccount(IDatabase db, int accId, string field = null)
        {
            AccountId = accId;
            Init(db, "account." + accId, field);

            if (field != null)
                return;

            if (DiscordId != null)
                DiscordRank = (int) db.HashGet("discordRank", DiscordId);
            
            var time = Utils.FromUnixTimestamp(BanLiftTime);
            if (!Banned || (BanLiftTime <= -1 || time > DateTime.UtcNow)) return;
            Banned = false;
            BanLiftTime = 0;
            FlushAsync();
        }

        public int AccountId { get; private set; }
        public int DiscordRank { get; private set; }

        public int AccountIdOverride
        {
            get { return GetValue<int>("accountIdOverride"); }
            set { SetValue<int>("accountIdOverride", value); }
        }
        public int AccountIdOverrider { get; set; }
        
        internal string LockToken { get; set; }

        public string UUID
        {
            get { return GetValue<string>("uuid"); }
            set { SetValue<string>("uuid", value); }
        }

        public string Name
        {
            get { return GetValue<string>("name"); }
            set { SetValue<string>("name", value); }
        }

        public bool Admin
        {
            get { return GetValue<bool>("admin"); }
            set { SetValue<bool>("admin", value); }
        }

        public bool NameChosen
        {
            get { return GetValue<bool>("nameChosen"); }
            set { SetValue<bool>("nameChosen", value); }
        }

        public bool Verified
        {
            get { return GetValue<bool>("verified"); }
            set { SetValue<bool>("verified", value); }
        }

        public bool AgeVerified
        {
            get { return GetValue<bool>("ageVerified"); }
            set { SetValue<bool>("ageVerified", value); }
        }

        public bool FirstDeath
        {
            get { return GetValue<bool>("firstDeath"); }
            set { SetValue<bool>("firstDeath", value); }
        }
        public bool SkinsClaimed
        {
            get { return GetValue<bool>("skinsclaimed"); }
            set { SetValue<bool>("skinsclaimed", value); }
        }
        public bool WelcomeBack
        {
            get { return GetValue<bool>("welcomeback"); }
            set { SetValue<bool>("welcomeback", value); }
        }
        public bool DonorClaim
        {
            get { return GetValue<bool>("donorclaim"); }
            set { SetValue<bool>("donorclaim", value); }
        }
        public int PetYardType
        {
            get { return GetValue<int>("petYardType"); }
            set { SetValue<int>("petYardType", value); }
        }

        public int GuildId
        {
            get { return GetValue<int>("guildId"); }
            set { SetValue<int>("guildId", value); }
        }

        public int GuildRank
        {
            get { return GetValue<int>("guildRank"); }
            set { SetValue<int>("guildRank", value); }
        }

        public int GuildFame
        {
            get { return GetValue<int>("guildFame"); }
            set { SetValue<int>("guildFame", value); }
        }

        public int VaultCount
        {
            get { return GetValue<int>("vaultCount"); }
            set { SetValue<int>("vaultCount", value); }
        }

        public ushort[] Gifts
        {
            get { return GetValue<ushort[]>("gifts") ?? new ushort[0]; }
            set { SetValue<ushort[]>("gifts", value); }
        }

        public int MaxCharSlot
        {
            get { return GetValue<int>("maxCharSlot"); }
            set { SetValue<int>("maxCharSlot", value); }
        }

        public DateTime RegTime
        {
            get { return GetValue<DateTime>("regTime"); }
            set { SetValue<DateTime>("regTime", value); }
        }

        public bool Guest
        {
            get { return GetValue<bool>("guest"); }
            set { SetValue<bool>("guest", value); }
        }
        
        public int Credits
        {
            get { return GetValue<int>("credits"); }
            set { SetValue<int>("credits", value); }
        }

        public int TotalCredits
        {
            get { return GetValue<int>("totalCredits"); }
            set { SetValue<int>("totalCredits", value); }
        }

        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue<int>("fame", value); }
        }

        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue<int>("totalFame", value); }
        }

        public int Tokens
        {
            get { return GetValue<int>("tokens"); }
            set { SetValue<int>("tokens", value); }
        }

        public int TotalTokens
        {
            get { return GetValue<int>("totalTokens"); }
            set { SetValue<int>("totalTokens", value); }
        }

        public int NextCharId
        {
            get { return GetValue<int>("nextCharId"); }
            set { SetValue<int>("nextCharId", value); }
        }

        public int LegacyRank
        {
            get { return GetValue<int>("rank"); }
            set { SetValue<int>("rank", value); }
        }

        public int CommonMarks
        {
            get { return GetValue<int>("CommonMarks"); }
            set { SetValue<int>("CommonMarks", value); }
        }
        public int RareMarks
        {
            get { return GetValue<int>("RareMarks"); }
            set { SetValue<int>("RareMarks", value); }
        }
        public int EpicMarks
        {
            get { return GetValue<int>("EpicMarks"); }
            set { SetValue<int>("EpicMarks", value); }
        }
        public int LegendaryMarks
        {
            get { return GetValue<int>("LegendaryMarks"); }
            set { SetValue<int>("LegendaryMarks", value); }
        }

        public ushort[] Skins
        {
            get { return GetValue<ushort[]>("skins") ?? new ushort[0]; }
            set { SetValue<ushort[]>("skins", value); }
        }
     
        public int[] LockList
        {
            get { return GetValue<int[]>("lockList") ?? new int[0]; }
            set { SetValue<int[]>("lockList", value); }
        }

        public int[] IgnoreList
        {
            get { return GetValue<int[]>("ignoreList") ?? new int[0]; }
            set { SetValue<int[]>("ignoreList", value); }
        }

        public bool Banned
        {
            get { return GetValue<bool>("banned"); }
            set { SetValue<bool>("banned", value); }
        }
       
        public string Notes
        {
            get { return GetValue<string>("notes"); }
            set { SetValue<string>("notes", value); }
        }

        public bool Hidden
        {
            get { return GetValue<bool>("hidden"); }
            set { SetValue<bool>("hidden", value); }
        }
        public int MStars
        {
            get { return GetValue<int>("mstars"); }
            set { SetValue<int>("mstars", value); }
        }
        public int MRewarded
        {
            get { return GetValue<int>("mrewarded"); }
            set { SetValue<int>("mrewarded", value); }
        }
        public int TQamount
        {
            get { return GetValue<int>("tqammount"); }
            set { SetValue<int>("tqammount", value); }
        }
        public int TQMamount
        {
            get { return GetValue<int>("tqmammount"); }
            set { SetValue<int>("tqmammount", value); }
        }
        public int TQDamount
        {
            get { return GetValue<int>("tqdammount"); }
            set { SetValue<int>("tqdammount", value); }
        }
        public int StarMastery
        {
            get { return GetValue<int>("starmastery"); }
            set { SetValue<int>("starmastery", value); }
        }
        public int VoidMastery
        {
            get { return GetValue<int>("voidmastery"); }
            set { SetValue<int>("voidmastery", value); }
        }
        public int MasteryPoints
        {
            get { return GetValue<int>("masterypoints"); }
            set { SetValue<int>("masterypoints", value); }
        }
      
        public int ItemMasteryPoints
        {
            get { return GetValue<int>("masterypoints"); }
            set { SetValue<int>("masterypoints", value); }
        }
        public bool ChatLevel
        {
            get { return GetValue<bool>("chatlevel"); }
            set { SetValue<bool>("chatlevel", value); }
        }
        public int MasteryRank
        {
            get { return GetValue<int>("masteryrank"); }
            set { SetValue<int>("masteryrank", value); }
        }
        public int GlowColor
        {
            get { return GetValue<int>("glow"); }
            set { SetValue<int>("glow", value); }
        }

        public string PassResetToken
        {
            get { return GetValue<string>("passResetToken"); }
            set { SetValue<string>("passResetToken", value); }
        }

        public string IP
        {
            get { return GetValue<string>("ip"); }
            set { SetValue<string>("ip", value); }
        }

        public int[] PetList
        {
            get { return GetValue<int[]>("petList") ?? new int[0]; }
            set { SetValue<int[]>("petList", value); }
        }
        public uint LastMarketId
        {
            get => GetValue<uint>("lastMarketId");
            set => SetValue("lastMarketId", value);
        }
        public int BanLiftTime
        {
            get { return GetValue<int>("banLiftTime"); }
            set { SetValue<int>("banLiftTime", value); }
        }

        public List<string> Emotes
        {
            get { return GetValue<string>("emotes")?.CommaToArray<string>()?.ToList() ?? new List<string>(); }
            set { SetValue<string>("emotes", value?.ToArray().ToCommaSepString() ?? String.Empty); }
        }

        public Int32 LastSeen
        {
            get { return GetValue<Int32>("lastSeen"); }
            set { SetValue<Int32>("lastSeen", value); }
        }

        public int Size
        {
            get { return GetValue<int>("size"); }
            set { SetValue<int>("size", value); }
        }

        public bool RankManager
        {
            get { return GetValue<bool>("rankManager"); }
            set { SetValue<bool>("rankManager", value); }
        }

        public string DiscordId
        {
            get { return GetValue<string>("discordId"); }
            set { SetValue<string>("discordId", value); }
        }

        public int Rank
        {
            get { return DiscordRank > LegacyRank ? DiscordRank : LegacyRank; }
        }

        public int[] Achievements
        {
            get { return GetValue<int[]>("achievements") ?? new int[0]; }
            set { SetValue<int[]>("achievements", value); }
        }
        
        public string SecretCode
        {
            get { return GetValue<string>("secretCode"); }
            set { SetValue<string>("secretCode", value); }
        }
        public string DiscordAuthAccount
        {
            get { return GetValue<string>("discordAuthAccount"); }
            set { SetValue<string>("discordAuthAccount", value); }
        }
        public int AuthTries
        {
            get { return GetValue<int>("authTries"); }
            set { SetValue<int>("authTries", value); }
        }
        public string DiscordToVerify
        {
            get { return GetValue<string>("discordToVerify"); }
            set { SetValue<string>("discordToVerify", value); }
        }
        public int[] PotionStoragePotions
        {
            get { return GetValue<int[]>("potionStoragePotions") ?? new int[13]; }
            set { SetValue<int[]>("potionStoragePotions", value); }
        }
        public int PotionStorageLevel
        {
            get { return GetValue<int>("potionStorageLevel"); }
            set { SetValue<int>("potionStorageLevel", value); }
        }
        public int PotionStorageSize
        {
            get { return GetValue<int>("potionStorageSize"); }
            set { SetValue<int>("potionStorageSize", value); }
        }
        public int PotionStorageLunar
        {
            get { return GetValue<int>("potionStorageLunar"); }
            set { SetValue<int>("potionStorageLunar", value); }
        }
        public int PotionStorageLunarLevel
        {
            get { return GetValue<int>("potionStorageLunarLevel"); }
            set { SetValue<int>("potionStorageLunarLevel", value); }
        }
        public int PotionStorageLunarSize
        {
            get { return GetValue<int>("PotionStorageLunarSize"); }
            set { SetValue<int>("PotionStorageLunarSize", value); }
        }
        public bool WhiteStarPrizeClaimed
        {
            get { return GetValue<bool>("whiteStarPrizeClaimed"); }
            set { SetValue<bool>("whiteStarPrizeClaimed", value); }
        }
        public PrivateMessages PrivateMessages
        {
            get
            {
                var pMessages = GetValue<string>("privateMessages");
                return pMessages != null
                    ? Utils.FromJson<PrivateMessages>(pMessages)
                    : null;
            }
            set { SetValue<string>("privateMessages", value.ToJson()); }
        }

        public Task AddPrivateMessage(int senderId, string subject, string message)
        {
            var messages = PrivateMessages ?? new PrivateMessages(AccountId, new List<PrivateMessages.PrivateMessage>());
            if (messages.NeedsFix())
                messages.FixFromOldBuild(this);
            var msg = new PrivateMessages.PrivateMessage(senderId, AccountId, subject, message, DateTime.UtcNow.ToUnixTimestamp());
            messages.Messages.Add(msg);
            PrivateMessages = messages;
            return FlushAsync();
        }

        public void RefreshLastSeen()
        {
            LastSeen = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }

    public struct DbClassStatsEntry
    {
        public int BestLevel;
        public int BestFame;
    }

    public class DbClassStats : RedisObject
    {
        public DbAccount Account { get; private set; }

        public DbClassStats(DbAccount acc, ushort? type = null)
        {
            Account = acc;
            Init(acc.Database, "classStats." + acc.AccountId, type?.ToString());
        }

        public void Unlock(ushort type)
        {
            var field = type.ToString();
            string json = GetValue<string>(field);
            if (json == null)
                SetValue<string>(field, JsonConvert.SerializeObject(new DbClassStatsEntry()
                {
                    BestLevel = 0,
                    BestFame = 0
                }));
        }

        public void Update(DbChar character)
        {
            var field = character.ObjectType.ToString();
            var finalFame = Math.Max(character.Fame, character.FinalFame);
            string json = GetValue<string>(field);
            if (json == null)
                SetValue<string>(field, JsonConvert.SerializeObject(new DbClassStatsEntry()
                {
                    BestLevel = character.Level,
                    BestFame = finalFame
                }));
            else
            {
                var entry = JsonConvert.DeserializeObject<DbClassStatsEntry>(json);
                if (character.Level > entry.BestLevel)
                    entry.BestLevel = character.Level;
                if (finalFame > entry.BestFame)
                    entry.BestFame = finalFame;
                SetValue<string>(field, JsonConvert.SerializeObject(entry));
            }
        }

        public DbClassStatsEntry this[ushort type]
        {
            get
            {
                string v = GetValue<string>(type.ToString());
                if (v != null) return JsonConvert.DeserializeObject<DbClassStatsEntry>(v);
                else return default(DbClassStatsEntry);
            }
            set { SetValue<string>(type.ToString(), JsonConvert.SerializeObject(value)); }
        }

    }

    public class DbChar : RedisObject
    {
        public DbAccount Account { get; private set; }
        public int CharId { get; private set; }

        public DbChar(DbAccount acc, int charId)
        {
            Account = acc;
            CharId = charId;
            Init(acc.Database, "char." + acc.AccountId + "." + charId);
        }

        public ushort ObjectType
        {
            get { return GetValue<ushort>("charType"); }
            set { SetValue<ushort>("charType", value); }
        }
        public int AttackStatsMoon
        {
            get { return GetValue<int>("AttackStatsMoon"); }
            set { SetValue<int>("AttackStatsMoon", value); }
        }
        public int DefensePotsMoon
        {
            get { return GetValue<int>("DefensePotsMoon"); }
            set { SetValue<int>("DefensePotsMoon", value); }
        }
        public int SpeedPotsMoon
        {
            get { return GetValue<int>("SpeedPotsMoon"); }
            set { SetValue<int>("SpeedPotsMoon", value); }
        }
        public int WisdomPotsMoon
        {
            get { return GetValue<int>("WisdomPotsMoon"); }
            set { SetValue<int>("WisdomPotsMoon", value); }
        }
        public int VitalityPotsMoon
        {
            get { return GetValue<int>("VitalityPotsMoon"); }
            set { SetValue<int>("VitalityPotsMoon", value); }
        }
        public int DexterityPotsMoon
        {
            get { return GetValue<int>("DexterityPotsMoon"); }
            set { SetValue<int>("DexterityPotsMoon", value); }
        }

        public int Toolbelts
        {
            get { return GetValue<int>("Toolbelts"); }
            set { SetValue<int>("Toolbelts", value); }
        }
        public int Regeneration
        {
            get { return GetValue<int>("Regeneration"); }
            set { SetValue<int>("Regeneration", value); }
        }
        public int LifePotsMoon
        {
            get { return GetValue<int>("LifePotsMoon"); }
            set { SetValue<int>("LifePotsMoon", value); }
        }
        public int ManaPotsMoon
        {
            get { return GetValue<int>("ManaPotsMoon"); }
            set { SetValue<int>("ManaPotsMoon", value); }
        }
        public int CritHitPotsMoon
        {
            get { return GetValue<int>("CritHitPotsMoon"); }
            set { SetValue<int>("CritHitPotsMoon", value); }
        }
        public int CritDmgPotsMoon
        {
            get { return GetValue<int>("CritDmgPotsMoon"); }
            set { SetValue<int>("CritDmgPotsMoon", value); }
        }

        public int MoonTime
        {
            get { return GetValue<int>("MoonTime"); }
            set { SetValue<int>("MoonTime", value); }
        }
        public bool MoonPrimed
        {
            get { return GetValue<bool>("MoonPrimed"); }
            set { SetValue<bool>("MoonPrimed", value); }
        }
        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue<int>("level", value); }
        }

        public int Experience
        {
            get { return GetValue<int>("exp"); }
            set { SetValue<int>("exp", value); }
        }
     
        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue<int>("fame", value); }
        }

        public int FinalFame
        {
            get { return GetValue<int>("finalFame"); }
            set { SetValue<int>("finalFame", value); }
        }

        public ushort[] Items
        {
            get { return GetValue<ushort[]>("items"); }
            set { SetValue<ushort[]>("items", value); }
        }

        public int HP
        {
            get { return GetValue<int>("hp"); }
            set { SetValue<int>("hp", value); }
        }

        public int MP
        {
            get { return GetValue<int>("mp"); }
            set { SetValue<int>("mp", value); }
        }

        public int[] Stats
        {
            get { return GetValue<int[]>("stats"); }
            set { SetValue<int[]>("stats", value); }
        }

        public int Tex1
        {
            get { return GetValue<int>("tex1"); }
            set { SetValue<int>("tex1", value); }
        }

        public int Tex2
        {
            get { return GetValue<int>("tex2"); }
            set { SetValue<int>("tex2", value); }
        }

        public int Skin
        {
            get { return GetValue<int>("skin"); }
            set { SetValue<int>("skin", value); }
        }

        public int PetId
        {
            get { return GetValue<int>("petId"); }
            set { SetValue<int>("petId", value); }
        }

        public byte[] FameStats
        {
            get { return GetValue<byte[]>("fameStats"); }
            set { SetValue<byte[]>("fameStats", value); }
        }

        public DateTime CreateTime
        {
            get { return GetValue<DateTime>("createTime"); }
            set { SetValue<DateTime>("createTime", value); }
        }

        public DateTime LastSeen
        {
            get { return GetValue<DateTime>("lastSeen"); }
            set { SetValue<DateTime>("lastSeen", value); }
        }

        public bool Dead
        {
            get { return GetValue<bool>("dead"); }
            set { SetValue<bool>("dead", value); }
        }

        public int HealthStackCount
        {
            get { return GetValue<int>("hpPotCount"); }
            set { SetValue<int>("hpPotCount", value); }
        }

        public int MagicStackCount
        {
            get { return GetValue<int>("mpPotCount"); }
            set { SetValue<int>("mpPotCount", value); }
        }

        public bool HasBackpack
        {
            get { return GetValue<bool>("hasBackpack"); }
            set { SetValue<bool>("hasBackpack", value); }
        }

        public int XPBoostTime
        {
            get { return GetValue<int>("xpBoost"); }
            set { SetValue<int>("xpBoost", value); }
        }

        public int LDBoostTime
        {
            get { return GetValue<int>("ldBoost"); }
            set { SetValue<int>("ldBoost", value); }
        }

        public int LTBoostTime
        {
            get { return GetValue<int>("ltBoost"); }
            set { SetValue<int>("ltBoost", value); }
        }
    }

    public class DbDeath : RedisObject
    {
        public DbAccount Account { get; private set; }
        public int CharId { get; private set; }

        public DbDeath(DbAccount acc, int charId)
        {
            Account = acc;
            CharId = charId;
            Init(acc.Database, "death." + acc.AccountId + "." + charId);
        }

        public ushort ObjectType
        {
            get { return GetValue<ushort>("objType"); }
            set { SetValue<ushort>("objType", value); }
        }

        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue<int>("level", value); }
        }

        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue<int>("totalFame", value); }
        }

        public string Killer
        {
            get { return GetValue<string>("killer"); }
            set { SetValue<string>("killer", value); }
        }

        public bool FirstBorn
        {
            get { return GetValue<bool>("firstBorn"); }
            set { SetValue<bool>("firstBorn", value); }
        }

        public DateTime DeathTime
        {
            get { return GetValue<DateTime>("deathTime"); }
            set { SetValue<DateTime>("deathTime", value); }
        }
    }

    public struct DbNewsEntry
    {
        [JsonIgnore] public DateTime Date;
        public string Icon;
        public string Title;
        public string Text;
        public string Link;
    }

    public class DbNews // TODO. Check later, range results might be bugged...
    {
        public DbNews(IDatabase db, int count)
        {
            news = db.SortedSetRangeByRankWithScores("news", 0, 10)
                .Select(x =>
                {
                    DbNewsEntry ret = JsonConvert.DeserializeObject<DbNewsEntry>(
                        Encoding.UTF8.GetString(x.Element));
                    ret.Date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double) x.Score);
                    return ret;
                }).ToArray();
        }

        private DbNewsEntry[] news;

        public DbNewsEntry[] Entries
        {
            get { return news; }
        }
    }

    public class DbVault : RedisObject
    {
        public DbAccount Account { get; private set; }

        public DbVault(DbAccount acc)
        {
            Account = acc;
            Init(acc.Database, "vault." + acc.AccountId);
        }

        public ushort[] this[int index]
        {
            get 
            { 
                return GetValue<ushort[]>("vault." + index) ?? 
                    Enumerable.Repeat((ushort)0xffff, 8).ToArray(); 
            }
            set { SetValue<ushort[]>("vault." + index, value); }
        }
    }

    public abstract class RInventory : RedisObject
    {
        public string Field { get; protected set; }

        public ushort[] Items
        {
            get { return GetValue<ushort[]>(Field) ?? Enumerable.Repeat((ushort)0xffff, 20).ToArray(); }
            set { SetValue<ushort[]>(Field, value); }
        }
    }

    public class DbVaultSingle : RInventory
    {
        public DbVaultSingle(DbAccount acc, int vaultIndex)
        {
            Field = "vault." + vaultIndex;
            Init(acc.Database, "vault." + acc.AccountId, Field);

            var items = GetValue<ushort[]>(Field);
            if (items != null)
                return;

            var trans = Database.CreateTransaction();
            SetValue<ushort[]>(Field, Items);
            FlushAsync(trans);
            trans.Execute(CommandFlags.FireAndForget);
        }
    }
    
    public class DbCharInv : RInventory
    {
        public DbCharInv(DbAccount acc, int charId)
        {
            Field = "items";
            Init(acc.Database, "char." + acc.AccountId + "." + charId, Field);
        }
    }

    public struct DbLegendEntry
    {
        public readonly int AccId;
        public readonly int ChrId;

        public DbLegendEntry(int accId, int chrId)
        {
            AccId = accId;
            ChrId = chrId;
        }
    }

    public static class DbLegend
    {
        private const int MaxListings = 20;
        private const int MaxGlowingRank = 20;
        private static readonly Dictionary<string, TimeSpan> TimeSpans = new Dictionary<string, TimeSpan>()
        {
            {"week", TimeSpan.FromDays(7) },
            {"month", TimeSpan.FromDays(30) },
            {"all", TimeSpan.MaxValue }
        };

        public static void Clean(IDatabase db)
        {
            // remove legend entries that expired
            foreach (var span in TimeSpans)
            {
                if (span.Value == TimeSpan.MaxValue)
                {
                    // bound legend by count
                    db.SortedSetRemoveRangeByRankAsync($"legends:{span.Key}:byFame",
                        0, -MaxListings - 1, CommandFlags.FireAndForget);
                    continue;
                }

                // bound legend by time
                var outdated = db.SortedSetRangeByScore(
                    $"legends:{span.Key}:byTimeOfDeath", 0,
                    DateTime.UtcNow.ToUnixTimestamp());
                
                var trans = db.CreateTransaction();
                trans.SortedSetRemoveAsync($"legends:{span.Key}:byFame", outdated, CommandFlags.FireAndForget);
                trans.SortedSetRemoveAsync($"legends:{span.Key}:byTimeOfDeath", outdated, CommandFlags.FireAndForget);
                trans.ExecuteAsync(CommandFlags.FireAndForget);
            }

            // refresh legend hash
            db.KeyDeleteAsync("legend", CommandFlags.FireAndForget);
            foreach (var span in TimeSpans)
            {
                var legendTask = db.SortedSetRangeByRankAsync($"legends:{span.Key}:byFame", 
                    0, MaxGlowingRank - 1, Order.Descending);
                legendTask.ContinueWith(r =>
                {
                    var trans = db.CreateTransaction();
                    foreach (var e in r.Result)
                    {
                        var accId = BitConverter.ToInt32(e, 0);
                        trans.HashSetAsync("legend", accId, "",
                            flags: CommandFlags.FireAndForget);
                    }
                    trans.ExecuteAsync(CommandFlags.FireAndForget);
                });
            }

            db.StringSetAsync("legends:updateTime", DateTime.UtcNow.ToUnixTimestamp(),
                flags: CommandFlags.FireAndForget);
        }

        public static void Insert(IDatabase db,
            int accId, int chrId, int totalFame)
        {
            var buff = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(accId), 0, buff, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(chrId), 0, buff, 4, 4);

            // add entry to each legends list
            var trans = db.CreateTransaction();
            foreach (var span in TimeSpans)
            {
                trans.SortedSetAddAsync($"legends:{span.Key}:byFame", 
                    buff, totalFame, CommandFlags.FireAndForget);

                if (span.Value == TimeSpan.MaxValue)
                    continue;

                double t = DateTime.UtcNow.Add(span.Value).ToUnixTimestamp();
                trans.SortedSetAddAsync($"legends:{span.Key}:byTimeOfDeath", 
                    buff, t, CommandFlags.FireAndForget);
            }
            trans.ExecuteAsync();

            // add legend if character falls within MaxGlowingRank
            foreach (var span in TimeSpans)
            {
                db.SortedSetRankAsync($"legends:{span.Key}:byFame", buff, Order.Descending)
                    .ContinueWith(r =>
                    {
                        if (r.Result >= MaxGlowingRank)
                            return;

                        db.HashSetAsync("legend", accId, "",
                            flags: CommandFlags.FireAndForget);
                    });
            }

            db.StringSetAsync("legends:updateTime", DateTime.UtcNow.ToUnixTimestamp(),
                flags: CommandFlags.FireAndForget);
        }

        public static DbLegendEntry[] Get(IDatabase db, string timeSpan)
        {
            if (!TimeSpans.ContainsKey(timeSpan))
                return new DbLegendEntry[0];
            
            var listings = db.SortedSetRangeByRank(
                $"legends:{timeSpan}:byFame", 
                0, MaxListings - 1, Order.Descending);

            return listings
                .Select(e => new DbLegendEntry(
                    BitConverter.ToInt32(e, 0),
                    BitConverter.ToInt32(e, 4)))
                .ToArray();
        }
    }

    public class DailyCalendar : RedisObject
    {
        public DbAccount Account { get; private set; }

        public DailyCalendar(DbAccount acc)
        {
            Account = acc;
            Init(acc.Database, "calendar." + acc.AccountId);
        }

        public int[] ClaimedDays
        {
            get { return GetValue<int[]>("claims"); }
            set { SetValue("claims", value); }
        }

        public int ConsecutiveDays //ConCur too ?
        {
            get { return GetValue<int>("consecutiveDays"); }
            set { SetValue("consecutiveDays", value); }
        }

        public int NonConsecutiveDays //NonConCur too ?
        {
            get { return GetValue<int>("nonConsecutiveDays"); }
            set { SetValue("nonConsecutiveDays", value); }
        }

        public int UnlockableDays
        {
            get { return GetValue<int>("unlockableDays"); }
            set { SetValue("unlockableDays", value); }
        }

        public DateTime LastTime
        {
            get { return GetValue<DateTime>("lastTime"); }
            set { SetValue("lastTime", value); }
        }
    }


    public class DbGuild : RedisObject
    {
        internal readonly object MemberLock; // maybe use redis locking?

        internal DbGuild(IDatabase db, int id)
        {
            MemberLock = new object();

            Id = id;
            Init(db, "guild." + id);
        }

        public DbGuild(DbAccount acc)
        {
            MemberLock = new object();

            Id = acc.GuildId;
            Init(acc.Database, "guild." + Id);
        }

        public int Id { get; private set; }

        public string Name
        {
            get { return GetValue<string>("name"); }
            set { SetValue<string>("name", value); }
        }

        public int Level
        {
            get { return GetValue<int>("level"); }
            set { SetValue<int>("level", value); }
        }

        public int Fame
        {
            get { return GetValue<int>("fame"); }
            set { SetValue<int>("fame", value); }
        }

        public int TotalFame
        {
            get { return GetValue<int>("totalFame"); }
            set { SetValue<int>("totalFame", value); }
        }

        public int[] Members // list of member account id's
        {
            get { return GetValue<int[]>("members") ?? new int[0]; }
            set { SetValue<int[]>("members", value); }
        }

        public int[] Allies // list of ally guild id's UNIMPLEMENTED
        {
            get { return GetValue<int[]>("allies") ?? new int[0]; }
            set { SetValue<int[]>("allies", value); }
        }

        public string Board
        {
            get { return GetValue<string>("board") ?? ""; }
            set { SetValue<string>("board", value); }
        }
    }

    public class DbIpInfo
    {
        private readonly IDatabase _db;

        internal DbIpInfo(IDatabase db, string ip)
        {
            _db = db;
            IP = ip;
            var json = (string) db.HashGet("ips", ip);
            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        [JsonIgnore]
        public string IP { get; private set; }

        [JsonIgnore]
        public bool IsNull { get; private set; }

        public HashSet<int> Accounts { get; set; }
        public bool Banned { get; set; }
        public string Notes { get; set; }

        public void Flush()
        {
            _db.HashSetAsync("ips", IP, JsonConvert.SerializeObject(this));
        }
    }

    public class DbPetAbility
    {
        private readonly DbPet _owner;
        private readonly string _typeKey;
        private readonly string _levelKey;
        private readonly string _powerKey;

        public DbPetAbility(DbPet owner, int abilityId)
        {
            _owner = owner;
            _typeKey = string.Format("A{0}Type", abilityId);
            _levelKey = string.Format("A{0}Level", abilityId);
            _powerKey = string.Format("A{0}Power", abilityId);
        }

        public PAbility Type
        {
            get { return (PAbility)_owner.GetValue(_typeKey); }
            set { _owner.SetValue(_typeKey, (int)value); }
        }

        public int Level
        {
            get { return _owner.GetValue(_levelKey); }
            set { _owner.SetValue(_levelKey, value); }
        }

        public int Power
        {
            get { return _owner.GetValue(_powerKey); }
            set { _owner.SetValue(_powerKey, value); }
        }
    }

    public class DbPet : RedisObject
    {
        public const int NumAbilities = 3;

        public DbAccount Account { get; private set; }
        public int PetId { get; set; }
        public DbPetAbility[] Ability { get; private set; }

        public DbPet(DbAccount acc, int petId)
        {
            Account = acc;
            PetId = petId;

            Init(acc.Database, $"pet.{acc.AccountId}.{petId}");

            Ability = new DbPetAbility[NumAbilities];
            for (var i = 0; i < NumAbilities; i++)
                Ability[i] = new DbPetAbility(this, i);
        }

        public ushort ObjectType
        {
            get { return GetValue<ushort>("objType"); }
            set { SetValue<ushort>("objType", value); }
        }

        public PRarity Rarity
        {
            get { return (PRarity)GetValue<ushort>("rarity"); }
            set { SetValue<ushort>("rarity", (ushort)value); }
        }

        public int MaxLevel
        {
            get { return GetValue<int>("maxLevel"); }
            set { SetValue<int>("maxLevel", value); }
        }

        internal int GetValue(string key)
        {
            return GetValue<int>(key);
        }

        internal void SetValue(string key, int value)
        {
            SetValue<int>(key, value);
        }
    }

    public class PlayerShopItem : ISellableItem
    {
        public uint Id { get; }
        public ushort ItemId { get; }
        public int Price { get; }
        public int InsertTime { get; }
        public int AccountId { get; }
        public int Count => -1;

        private bool _lastItem;

        public PlayerShopItem(uint id, ushort itemId, int price, int time, int accId)
        {
            Id = id;
            ItemId = itemId;
            Price = price;
            InsertTime = time;
            AccountId = accId;
        }

        public bool IsLastMarketedItem(uint lastMarketId)
        {
            return _lastItem = lastMarketId == Id;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Id);
            wtr.Write(ItemId);
            wtr.Write(Price);
            wtr.Write(InsertTime);
            wtr.Write(Count);
            wtr.Write(_lastItem);
        }
    }

    public class DbMarket
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DbMarket));

        private static readonly object ListLock = new object();

        private readonly List<PlayerShopItem> _entries;

        private readonly IDatabase _db;
        private readonly string _key;

        public DbMarket(IDatabase db)
        {
            _db = db;
            _key = "market";

            var entries = db.HashGetAll(_key)
                .Select(x => new PlayerShopItem(
                    BitConverter.ToUInt32(x.Value, 0),
                    BitConverter.ToUInt16(x.Value, 4),
                    BitConverter.ToInt32(x.Value, 6),
                    BitConverter.ToInt32(x.Value, 10),
                    BitConverter.ToInt32(x.Value, 14)))
                .OrderBy(x => x.InsertTime);

            _entries = new List<PlayerShopItem>(entries);
        }

        public Task<bool> InsertAsync(PlayerShopItem shopItem, ITransaction transaction = null)
        {
            var trans = transaction ?? _db.CreateTransaction();

            var buff = new byte[18];
            Buffer.BlockCopy(BitConverter.GetBytes(shopItem.Id), 0, buff, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(shopItem.ItemId), 0, buff, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(shopItem.Price), 0, buff, 6, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(shopItem.InsertTime), 0, buff, 10, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(shopItem.AccountId), 0, buff, 14, 4);

            trans.AddCondition(Condition.HashNotExists(_key, shopItem.Id));
            var task = trans.HashSetAsync(_key, shopItem.Id, buff)
                .ContinueWith(t => Item2List(shopItem, t, true));

            task.ContinueWith(e =>
                    Log.Error(e.Exception.InnerException.ToString()),
                    TaskContinuationOptions.OnlyOnFaulted);

            if (transaction == null)
                trans.ExecuteAsync();

            return task;
        }

        public Task<bool> RemoveAsync(PlayerShopItem shopItem, ITransaction transaction = null)
        {
            var trans = transaction ?? _db.CreateTransaction();

            trans.AddCondition(Condition.HashExists(_key, shopItem.Id));
            var task = trans.HashDeleteAsync(_key, shopItem.Id)
                .ContinueWith(t => Item2List(shopItem, t, false));

            task.ContinueWith(e =>
                    Log.Error(e.Exception.InnerException.ToString()),
                    TaskContinuationOptions.OnlyOnFaulted);

            if (transaction == null)
                trans.ExecuteAsync();

            return task;
        }

        private bool Item2List(PlayerShopItem shopItem, Task<bool> t, bool add)
        {
            var success = !t.IsCanceled && t.Result;
            if (success)
            {
                using (TimedLock.Lock(ListLock))
                {
                    if (add)
                        _entries.Add(shopItem);
                    else
                        _entries.Remove(shopItem);
                }
            }

            return success;
        }

        public PlayerShopItem GetById(uint id)
        {
            using (TimedLock.Lock(ListLock))
            {
                return _entries.SingleOrDefault(e => e.Id == id);
            }
        }

        public PlayerShopItem[] GetAll()
        {
            using (TimedLock.Lock(ListLock))
            {
                return _entries.ToArray();
            }
        }
    }
    public class DbTinker
    {
        private const string KEY = "tinkerQuests";

        private static readonly ILog log = LogManager.GetLogger(typeof(DbTinker));
        private static readonly object listLock = new object();

        private readonly List<Tinker> entries; 
        private readonly IDatabase db;

        public DbTinker(IDatabase db)
        {
            this.db = db;
            this.entries = new List<Tinker>(db.HashGetAll(KEY).Select(x => Tinker.Load(BitConverter.ToUInt32(x.Value, 0), Encoding.UTF8.GetString(x.Value, 4, ((byte[])x.Value).Length - 4))));
        }

        public Task<bool> InsertAsync(Tinker quest, ITransaction transaction = null)
        {
            var trans = transaction ?? db.CreateTransaction();

            var buff = new byte[4 + quest.ByteLength];
            Buffer.BlockCopy(BitConverter.GetBytes(quest.DbId), 0, buff, 0, 4);
            Buffer.BlockCopy(quest.Bytes, 0, buff, 4, quest.ByteLength);

            trans.AddCondition(Condition.HashNotExists(KEY, quest.DbId));
            var task = trans.HashSetAsync(KEY, quest.DbId, buff)
                .ContinueWith(t => UpdateList(quest, t, true));

            task.ContinueWith(e =>
                    log.Error(e.Exception.InnerException.ToString()),
                    TaskContinuationOptions.OnlyOnFaulted);

            if (transaction == null)
                trans.ExecuteAsync();

            return task;
        }

        public async void UpdateAsync(Tinker quest, ITransaction transaction = null)
        {
            try
            {
                await DeleteAsync(quest, transaction);
                await InsertAsync(quest, transaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private Task<bool> DeleteAsync(Tinker quest, ITransaction transaction = null)
        {
            var trans = transaction ?? db.CreateTransaction();

            trans.AddCondition(Condition.HashExists(KEY, quest.DbId));
            var task = trans.HashDeleteAsync(KEY, quest.DbId)
                .ContinueWith(t => UpdateList(quest, t, false));
            task.ContinueWith(e =>
                log.Error(e.Exception.InnerException.ToString()),
                TaskContinuationOptions.OnlyOnFaulted);

            if (transaction == null)
                trans.ExecuteAsync();

            return task;
        }

        private bool UpdateList(Tinker quest, Task<bool> task, bool add)
        {
            if (!(!task.IsCanceled && task.Result)) return false;
            using (TimedLock.Lock(listLock))
            {
                if (add)
                    entries.Add(quest);
                else
                    entries.Remove(quest);
            }
            return true;
        }

        public Tinker GetQuestForAccount(int accountId)
        {
            using (TimedLock.Lock(listLock))
            {
                return entries.FirstOrDefault(_ => _.OwnerId == accountId);
            }
        }

        public Tinker GenerateNew(DbAccount account)
        {
            var tinker = new Tinker((uint)entries.Count, Tinker.CreateNew(account.AccountId, 0x2352, 1));
            InsertAsync(tinker);
            return tinker;
        }
    }
}