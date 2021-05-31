using System;
using System.Threading;
using common.resources;
using common;
using System.Collections.Concurrent;
using System.Collections.Generic;
using wServer.networking;
using System.Threading.Tasks;
using System.Linq;
using wServer.logic;
using log4net;
using wServer.realm.commands;
using wServer.realm.entities.vendors;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using System.Globalization;
using wServer.realm.entities.player.equipstatus;

namespace wServer.realm
{
    public struct RealmTime
    {
        public long TickCount;
        public long TotalElapsedMs;
        public int TickDelta;
        public int ElaspedMsDelta;
        public int ElaspedMsDeltas;
    }

    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Normal,
        Creation,
    }

    public enum PacketPriority
    {
        High,
        Normal,
        Low // no guarantees that packets of low priority will be sent
    }

    public class RealmManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RealmManager));

        private readonly bool _initialized;
        public string InstanceId { get; private set; }
        public bool Terminating { get; private set; }

        public Resources Resources { get; private set; }
        public Database Database { get; private set; }
        public int CurrentDatetime { get; set; }
        public ServerConfig Config { get; private set; }
        public int TPS { get; private set; }
        public ConnectManager ConMan { get; private set; }
        public BehaviorDb Behaviors { get; private set; }
        public ISManager InterServer { get; private set; }
        public ISControl ISControl { get; private set; }
        public ChatManager Chat { get; private set; }
        public DbServerManager DbServerController { get; private set; }
        public CommandManager Commands { get; private set; }
        public EquippedStatusManager EquippedManager { get; private set; }
        public DbTinker Tinker { get; private set; }
        public PortalMonitor Monitor { get; private set; }
        public DbEvents DbEvents { get; private set; }

        private Thread _network;
        private Thread _logic;
        public NetworkTicker Network { get; private set; }
        public FLLogicTicker Logic { get; private set; }

        public readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        public readonly ConcurrentDictionary<Client, PlayerInfo> Clients = new ConcurrentDictionary<Client, PlayerInfo>();
        public Dictionary<List<string>, Item> Recipes = new Dictionary<List<string>, Item>();



        private int _nextWorldId = 0;
        private int _nextClientId = 0;

        public RealmManager(Resources resources, Database db, ServerConfig config)
        {
            Log.Info("Initializing Realm Manager...");

            InstanceId = Guid.NewGuid().ToString();
            Database = db;
            Resources = resources;
            Config = config;
            Config.serverInfo.instanceId = InstanceId;
            TPS = config.serverSettings.tps;
            CurrentDatetime = 0;

            // all these deal with db pub/sub... probably should put more thought into their structure... 
            InterServer = new ISManager(Database, config);
            ISControl = new ISControl(this);
            Chat = new ChatManager(this);
            DbServerController = new DbServerManager(this); // probably could integrate this with ChatManager and rename...
            DbEvents = new DbEvents(this);

            // basic server necessities
            ConMan = new ConnectManager(this,
                config.serverSettings.maxPlayers,
                config.serverSettings.maxPlayersWithPriority);
            Behaviors = new BehaviorDb(this);
            Commands = new CommandManager(this);
            EquippedManager = new EquippedStatusManager();

            // some necessities that shouldn't be (will work this out later)
            MerchantLists.Init(this);
            Tinker = new DbTinker(db.Conn);

            var serverMode = config.serverSettings.mode;
            switch (serverMode)
            {
                case ServerMode.Single:
                    InitializeNexusHub();
                    AddWorld("Realm");
                    AddWorld("Moon");
                    // AddWorld("DeathArena");
                    break;
                case ServerMode.Nexus:
                    InitializeNexusHub();
                    break;
                case ServerMode.Realm:
                    AddWorld("Realm");
                    break;
            }

            // add portal monitor to nexus and initialize worlds
            if (Worlds.ContainsKey(World.Nexus))
                Monitor = new PortalMonitor(this, Worlds[World.Nexus]);
            foreach (var world in Worlds.Values)
                OnWorldAdded(world);

            //Monitor.AddPortal(World.Spawners);
            //Monitor.AddPortal(World.MarketPlace);

            AddRecipes();
            Log.Info("World Recipes Added");

            _initialized = true;

            Log.Info("Realm Manager initialized.");
        }

        public void AddRecipes()
        {
            //Mark of Cyberious
            AddRecipe("Cyberious's Plate", "Plate of Tidale", "Plate of Tidale", "Fragments of Void Matter");
            //rings
            //Bracer of the Galactic Guardian
            //Celestial's Gilded Armor
            AddRecipe("Composed Moon Essence", "Fragment of the Moon", "Fragment of the Moon", "Fragment of the Moon", "Fragment of the Moon", "Fragment of the Moon", "Fragment of the Moon");//

            AddRecipe("Lunar Ascension", "Fragment of the Moon", "Fragment of the Earth", "Cyberious Infused Shard");//

            AddRecipe("Legendary Sor Crystal", "Free Legendary Sor Converter", "Free Sor Crystal");//
            AddRecipe("Legendary Sor Crystal", "Legendary Sor Converter", "Sor Crystal");//

            AddRecipe("Predator Necklace", "Hunter Necklace", "Legendary Sor Crystal", "Fragment of the Earth");//
            AddRecipe("Lunar Ascension", "Fragment of the Moon", "Fragment of the Earth", "Cyberious Infused Shard");//



            AddRecipe("Lunar Glutinous Hide", "Royal Glutinous Hide", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");
            AddRecipe("Celestial's Gilded Armor", "Heaven's Gilded Armor", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");//
            AddRecipe("Commander's Guard", "Gladiator Guard", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");//
            AddRecipe("Celestial Ogmur", "Shield of Ogmur", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");//
            AddRecipe("Helm of the Lunar Juggernaut", "Helm of the Juggernaut", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");//
            AddRecipe("Necklace of Lunar Magic", "Necklace of Magic", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");//

            AddRecipe("Compacted Lunar Trap", "Moon Essence Trap", "Moon Essence Trap", "Cyberious's Plate", "Scraps of the Descendant", "Composed Moon Essence");
            AddRecipe("Super Alien Core: Power", "Advanced Alien Tech", "Powered Reactor", "Alien Core: Ruby", "Alien Core: Diamond", "Alien Core: Gold", "Alien Core: Emerald");

            AddRecipe("Alien Core: Ruby", "Destructor AI Module", "Advanced Alien Tech", "Power Battery");
            AddRecipe("Alien Core: Diamond", "Engine Cooling Module", "Advanced Alien Tech", "Power Battery");
            AddRecipe("Alien Core: Gold", "Energy Converter Module", "Advanced Alien Tech", "Power Battery");
            AddRecipe("Alien Core: Emerald", "Atomic Battery Module", "Advanced Alien Tech", "Power Battery");

            //Armors
            AddRecipe("Powered Loot Drop Potion", "Loot Drop Potion", "Loot Drop Potion");
            AddRecipe("Enchanted Loot Drop Potion", "Powered Loot Drop Potion", "Powered Loot Drop Potion");

            AddRecipe("The Mythical Armor", "Heavy Armor Schematic", "Ancient Burgeon Shroom Armor", "Breastplate of New Life", "Amethyst", "Cyberious's Plate", "Cyberious Infused Shard");

            AddRecipe("Helm of the Golden King", "Helm of the Golden Warrior", "Jade", "Power Battery", "Shard of Fire", "Cyberious Infused Shard");

            AddRecipe("Overflowing Power Spell", "Empty Power Spell", "Power Battery", "Blue power crystal", "Fragment of the Moon");
            AddRecipe("Super Powered Dimensional Orb", "Dull Dimensional Orb", "Power Battery", "Amethyst", "Fragment of the Moon");

            //updated
            AddRecipe("Legendary Leaf Dragon Hide Armor", "Leaf Dragon Hide Armor", "Leaf Dragon Hide Armor", "Amethyst", "Light Armor Schematic", "Cyberious Infused Shard");
            AddRecipe("Legendary Fire Dragon Battle Armor", "Fire Dragon Battle Armor", "Fire Dragon Battle Armor", "Jade", "Heavy Armor Schematic", "Cyberious Infused Shard");
            AddRecipe("Legendary Water Dragon Silk Robe", "Water Dragon Silk Robe", "Water Dragon Silk Robe", "Topaz", "Robe Schematic", "Cyberious Infused Shard");

            AddRecipe("Ring of Pure Decades", "Ring of Decades", "Ring of Decades", "Scraps of the Descendant", "Fragment of the Earth");
            AddRecipe("Frozen Necklace of Magic", "Frozen Ice Shard", "Necklace of Magic", "Cyberious's Plate", "Cyberious Infused Shard");//
            AddRecipe("Ring of Pure Centuries", "Ring of Pure Decades", "Frozen Ice Shard", "Jade", "Cyberious Infused Shard");

            AddRecipe("Burning Coral Silk Armor", "Light Armor Schematic", "Hide of the Seas", "Cyberious's Plate", "Scraps of the Descendant");

            AddRecipe("Hide of the Enraged", "Light Armor Schematic", "Plate of Tidale", "Cyberious's Plate", "Cyberious Infused Shard");
            AddRecipe("Armor of the Enraged", "Heavy Armor Schematic", "Plate of Tidale", "Cyberious's Plate", "Cyberious Infused Shard");
            AddRecipe("Robe of the Enraged", "Robe Schematic", "Plate of Tidale", "Cyberious's Plate", "Cyberious Infused Shard");

            AddRecipe("High Extrinsic Armor", "Heavy Armor Schematic", "Scrapped Ship Plates", "Alien U.F.O Plate", "Plate of Tidale");//
            AddRecipe("High Extrinsic Robe", "Robe Schematic", "Scrapped Ship Plates", "Alien U.F.O Plate", "Plate of Tidale");//
            AddRecipe("High Extrinsic Light Armor", "Light Armor Schematic", "Scrapped Ship Plates", "Alien U.F.O Plate", "Plate of Tidale");//



            //Empowered Queen Crystal Helmet



            //Frozen Necklace of Magic

            AddRecipe("Necklace of Magic", "Frozen Necklace of Magic", "Shard of Fire");

            AddRecipe("Necklace of Magic", "Burning Necklace of Magic", "Frozen Ice Shard");


            //Abilities
            AddRecipe("Magic Jade Scroll", "Spell Scroll", "Jade", "Cyberious's Plate");
            AddRecipe("Shromite's Scroll", "Spell Scroll", "Super Magic Mushroom", "Amethyst");
            //Weapons
            AddRecipe("Staff of the Topaz Core", "Topaz", "Staff of the Cosmic Whole");//

            AddRecipe("Oryx's Dagger of Foul Malevolence", "Dagger of Foul Malevolence", "Cyberious's Plate", "Scraps of the Descendant");//
            AddRecipe("Oryx's Bow of Covert Havens", "Bow of Covert Havens", "Cyberious's Plate", "Scraps of the Descendant");//
            AddRecipe("Oryx's Staff of the Cosmic Whole", "Staff of the Cosmic Whole", "Cyberious's Plate", "Scraps of the Descendant");//
            AddRecipe("Oryx's Sword of Acclaim", "Sword of Acclaim", "Cyberious's Plate", "Scraps of the Descendant");//
            AddRecipe("Oryx's Wand of Recompense", "Wand of Recompense", "Cyberious's Plate", "Scraps of the Descendant");//
            AddRecipe("Oryx's Masamune", "Masamune", "Cyberious's Plate", "Scraps of the Descendant");//


            AddRecipe("Frozen Coral Bow", "Frozen Ice Shard", "Coral Bow");//
            AddRecipe("Frozen Crystal Sword", "Frozen Ice Shard", "Crystal Sword", "Cyberious Infused Shard", "Engine Cooling Module");// Unbound Ring of the Sphinx
            AddRecipe("Frozen Staff of Esben", "Frozen Ice Shard", "Staff of Esben");//
            AddRecipe("Frozen Crystal Wand", "Frozen Ice Shard", "Crystal Wand");// "Golden Ankh"

            AddRecipe("Exalted Stone Sword",  "Spell Scroll", "Jade", "Cyberious's Plate", "Ancient Stone Sword");

            AddRecipe("Snake Skin Bulwark", "Wand of the Bulwark", "Shard of Fire", "Power Battery", "Red Ichor");
            AddRecipe("Fortification", "Topaz", "Jade", "Amethyst", "Snake Skin Bulwark", "Cyberious Infused Shard", "Glowing Void Matter");
            //Recreation

            AddRecipe("Recreation", "Disarray");
            AddRecipe("Disarray", "Recreation");

            AddRecipe("Unbound Ring of the Sphinx", "Ring of the Sphinx", "Sprite Essence", "Eye of Osiris");
            AddRecipe("Unbound Ring of the Nile", "Ring of the Nile", "Sprite Essence", "Pharaoh's Mask");
            AddRecipe("Unbound Ring of the Pyramid", "Ring of the Pyramid", "Sprite Essence", "Golden Ankh");

            AddRecipe("Bone-locked Skull of Rage", "Avatar's Skull", "Skull of Endless Torment", "Amethyst", "Cyberious's Plate", "Red Ichor");

            //Bracer of the Galactic Guardian
            AddRecipe("Bracer of the Galactic Guardian", "Bracer of the Guardian", "Bracer of the Guardian", "Blue power crystal", "Composed Moon Essence", "Fragment of the Earth", "Cyberious Infused Shard");

            AddRecipe("Technological Fury", "Conducting Wand", "Conducting Wand", "Power Battery");
            AddRecipe("Scientific Knowledge", "Scepter of Fulmination", "Scepter of Fulmination", "Power Battery");
            AddRecipe("Robe of the Technological Gemstone", "Robe of the Mad Scientist", "Robe of the Mad Scientist", "Power Battery");
            AddRecipe("Matured Product Bracelet", "Experimental Ring", "Experimental Ring", "Power Battery");








            //Trick White Bag
            //new 
            AddRecipe("Prism of White Bags", "Trick White Bag", "Trick White Bag");

            AddRecipe("Shimmering Crystal Scorpion Helm", "Crystal Scorpion Helm", "Blue power crystal", "Cyberious's Plate", "Amethyst", "Topaz");

            AddRecipe("Deep Blue Abyssal Armor", "Deep Blue Shoulder Pads", "Blue power crystal", "Heavy Armor Schematic");

            AddRecipe("Holy Sanction Wand", "St. Abraham's Wand", "St. Abraham's Wand", "Carnelian");

            AddRecipe("Wand of Mythical Fusion", "Sprite Essence", "Sprite Essence", "Sprite Wand");

            AddRecipe("Frozen Dirk of Cronus", "Dirk of Cronus", "Frozen Ice Shard");//

            AddRecipe("Dirk of the Frozen Cronus", "Frozen Dirk of Cronus", "Cyberious Infused Shard", "Scraps of the Descendant");//

            AddRecipe("True Dirk of Cronus", "Dirk of Cronus", "Cyberious Infused Shard", "Scraps of the Descendant");//

            AddRecipe("Dirk of the Frozen Cronus", "True Dirk of Cronus", "Frozen Ice Shard");//



            AddRecipe("Lucky Clover", "Saint Patty's Brew", "Saint Patty's Brew", "Saint Patty's Brew", "Saint Patty's Brew");

            AddRecipe("Greater Potion of Defense", "Potion of Defense", "Potion of Defense");
            AddRecipe("Greater Potion of Speed", "Potion of Speed", "Potion of Speed");
            AddRecipe("Greater Potion of Vitality", "Potion of Vitality", "Potion of Vitality");
            AddRecipe("Greater Potion of Wisdom", "Potion of Wisdom", "Potion of Wisdom");
            AddRecipe("Greater Potion of Dexterity", "Potion of Dexterity", "Potion of Dexterity");
            AddRecipe("Greater Potion of Life", "Potion of Life", "Potion of Life");
            AddRecipe("Potion of Critical Damage", "Potion of Attack", "Greater Potion of Attack");
            AddRecipe("Potion of Critical Chance", "Potion of Dexterity", "Greater Potion of Dexterity");
            AddRecipe("Greater Potion of Critical Damage", "Potion of Critical Damage", "Potion of Critical Damage");
            AddRecipe("Greater Potion of Critical Chance", "Potion of Critical Chance", "Potion of Critical Chance");
            AddRecipe("Greater Potion of Attack", "Potion of Attack", "Potion of Attack");
            AddRecipe("Greater Potion of Mana", "Potion of Mana", "Potion of Mana");//Greater Potion of Luck
            AddRecipe("Greater Potion of Luck", "Potion of Luck", "Potion of Luck");
            AddRecipe("Potion of Luck", "Potion of Mana", "Potion of Life", "Potion of Attack", "Potion of Dexterity", "Potion of Critical Damage", "Potion of Critical Chance");

            // Materials Concentrated Void Matter

            AddRecipe("Frozen Ice Shard", "Frost Elementalist Robe", "Frost Drake Hide Armor", "Frost Citadel Armor");
            AddRecipe("Alien U.F.O Plate", "Scrapped Ship Plates", "Scrapped Ship Plates");
            AddRecipe("Plate of Tidale", "Scraps of the Descendant", "Scraps of the Descendant", "Scraps of the Descendant");
            AddRecipe("Advanced Alien Tech", "Basic Alien Tech", "Basic Alien Tech");


            AddRecipe("Large Toolbelt", "Medium Toolbelt", "Medium Toolbelt");
            AddRecipe("Medium Toolbelt", "Small Toolbelt", "Small Toolbelt");
            AddRecipe("Small Toolbelt", "Tiny Toolbelt", "Tiny Toolbelt");
            AddRecipe("Tiny Toolbelt", "Backpack", "Backpack");
            AddRecipe("Peppermint Candy-Plated Armor", "Candy-Coated Armor", "Candy-Coated Armor", "Topaz", "Sprite Essence", "Cyberious's Plate", "Cyberious Infused Shard");
            AddRecipe("Peppermint Sugar-Rush", "Candy Ring", "Candy Ring", "Frozen Ice Shard", "Carnelian", "Cyberious's Plate");

            AddRecipe("Magical Watermelon", "Candy Corn", "Candy Corn");
            AddRecipe("Claws of Judgement", "Claws of No Remorse", "Cyberious Infused Shard", "Cyberious's Plate", "Ectoplasm", "Advanced Alien Tech", "Glowing Void Matter");
            AddRecipe("Grand Chapters of Hallow Glory", "Tome of Purification", "Tome of Purification", "Carnelian");
            AddRecipe("Garment of Supreme Deacon", "Chasuble of Holy Light", "Chasuble of Holy Light", "Carnelian");
            AddRecipe("Necklace of Angel's Cross", "Ring of Divine Faith", "Ring of Divine Faith", "Carnelian");

            AddRecipe("Mors omnibus", "Edictum Praetoris", "Ancient Summoner Staff");
            AddRecipe("Memento nihil", "Memento Mori", "Skull of Solid Defense");
            AddRecipe("Toga praetexta", "Toga Picta", "Heavy Plated Robe");
            AddRecipe("Exsilium", "Interregnum", "Ring of the Guardian");


            AddRecipe("Oryx's Mystery Chest", "Light Armor Schematic", "Robe Schematic", "Heavy Armor Schematic", "Scraps of the Descendant", "Scraps of the Descendant");
            AddRecipe("Executioner's Blade", "Lunar Descension");
            AddRecipe("Lunar Descension", "Executioner's Blade");
            
            AddRecipe("Dragon Soul's Katana", "Ray Katana", "Ray Katana", "Cyberious Infused Shard", "Ectoplasm", "Glowing Void Matter", "Fragments of Void Matter");



            AddRecipe("Earth's Remembrance", "Leaf Bow", "Doom Bow", "Luminous Supernova", "Coral Bow", "Glowing Void Matter", "Fragments of Void Matter");
            AddRecipe("Executioner's Blade", "Voided Sword of the Colossus", "Exalted Stone Sword", "Enchanted Fury Blades", "Frozen Crystal Sword", "Fire Cutlass", "Blade of Demons");




            AddRecipe("Luminous Supernova", "Bow of the Morning Star", "Holy Crossbow", "Demonic Bow", "Cyberious Infused Shard", "Basic Alien Tech", "Fragments of Void Matter");//
            //Dagger of the Endless Magic


            //new
            AddRecipe("Dagger of the Endless Magic", "Awoken Spriteful Dirk", "Spriteful Dirk", "Cyberious Infused Shard", "Scraps of the Descendant", "Sprite Essence");
            AddRecipe("Blade of Demons", "Demon Blade", "Demon Blade", "Cyberious Infused Shard", "Scraps of the Descendant", "Fragments of Void Matter");
            AddRecipe("Bow of Doom", "Doom Bow", "Doom Bow", "Cyberious Infused Shard", "Scraps of the Descendant", "Fragments of Void Matter");
            AddRecipe("Bow of Coral", "Coral Bow", "Coral Bow", "Cyberious Infused Shard", "Scraps of the Descendant", "Fragments of Void Matter");
            AddRecipe("Burning Sacrifices", "Awoken Demonic Bow", "Demonic Bow", "Shard of Fire", "Scraps of the Descendant");
            //new

            AddRecipe("The Millionth Shot", "Awoken Holy Crossbow", "Thousand Shot", "Cyberious Infused Shard", "Scraps of the Descendant", "Fragments of Void Matter", "Carnelian");

            AddRecipe("Frozen Plated Crystal Armor", "Armor of Pierced Crystals", "Frozen Ice Shard", "Frost Citadel Armor", "Scraps of the Descendant", "Heavy Armor Schematic");//

            AddRecipe("Pirate King's Golden Cutlass", "Pirate King's Cutlass", "Topaz", "Energy Converter Module", "Scraps of the Descendant");//

            AddRecipe("Glowing Void Matter", "Fragments of Void Matter", "Fragments of Void Matter", "Fragments of Void Matter");

            AddRecipe("Voided Sword of the Colossus", "Cyberious Infused Shard", "Glowing Void Matter", "Glowing Void Matter", "Sword of the Colossus", "Scraps of the Descendant", "Alien U.F.O Plate");


            AddRecipe("Enchanted Fury Blades", "Awoken Blades of the Slayer", "Blades of the Slayer", "Shard of Fire", "Scraps of the Descendant");



            AddRecipe("Fire Cutlass", "Pirate King's Golden Cutlass", "Shard of Fire", "Fragments of Void Matter", "Cyberious Infused Shard", "Alien U.F.O Plate", "Destructor AI Module");//





            AddRecipe("Slithering Creature's Ring", "Snake Eye Ring", "Spider's Eye Ring");

            AddRecipe("Ring of the Ocean", "Coral Ring", "Captain's Ring", "Sprite Essence");


            AddRecipe("Burning Necklace of Magic", "Shard of Fire", "Necklace of Magic", "Cyberious's Plate");//

            AddRecipe("Enchanted Harlequin Hide", "Harlequin Armor", "Cyberious's Plate", "Cyberious Infused Shard", "Light Armor Schematic");//


            //new Harlequin Armor
            AddRecipe("Obsidian Silk Armor", "Burning Coral Silk Armor", "Cyberious Infused Shard", "Fragments of Void Matter", "Light Armor Schematic", "Carnelian");
            AddRecipe("Hide of the Seas", "Spectral Cloth Armor", "Coral Silk Armor", "Light Armor Schematic", "Plate of Tidale");





            AddRecipe("Gladiator's Unlit Candles", "Golden Gladiator Helmet", "Fragments of Void Matter", "Frozen Ice Shard", "Cyberious's Plate");










            /*
            AddRecipe("Earth's Remembrance", "Crown"); 
            AddRecipe("Marble Seal", "Crown");
            AddRecipe("Dragon Soul's Katana", "Crown");
            AddRecipe("Disarray", "Crown");
            AddRecipe("Omnipotence Ring", "Crown");
            AddRecipe("Necklace of Magic", "Crown");
            */
        }

        public void AddRecipe(string result, params string[] recipe)
        {
            Recipes.Add(recipe.Select(_ => _.ToLower()).ToList(), Resources.GameData.Items[Resources.GameData.IdToObjectType[result]]);
        }

        public Item GetCraftResult(List<Item> items)
        {
            foreach (var r in Recipes)
            {
                List<string> requirements = r.Key.Select(_ => _).ToList();
                bool pass = false;
                foreach (Item i in items)
                {
                    if (requirements.Count == 0)
                        pass = true;
                    if (requirements.Count < 0)
                    {
                        pass = false;
                        break;
                    }
                    if (requirements.Contains(i.ObjectId.ToLower()))
                        requirements.Remove(i.ObjectId.ToLower());
                }
                if (requirements.Count == 0 && !pass)
                    return r.Value;
            }
            return null;
        }

        private void InitializeNexusHub()
        {
            // load world data
            foreach (var wData in Resources.Worlds.Data.Values)
                if (wData.id < 0)
                    AddWorld(wData);
        }

        public void Run()
        {
            Log.Info("Starting Realm Manager...");

            // start server logic management
            Logic = new FLLogicTicker(this);
            _logic = new Thread(Logic.TickLoop)
            {
                Name = "Logic Thread",
                CurrentCulture = CultureInfo.InvariantCulture
            };

            //var logic = new Task(() => Logic.TickLoop(), TaskCreationOptions.LongRunning);
            //logic.ContinueWith(Program.Stop, TaskContinuationOptions.OnlyOnFaulted);
            _logic.Start();

            // start received packet processor
            Network = new NetworkTicker(this);
            _network = new Thread(Network.TickLoop)
            {
                Name = "Network Thread",
                CurrentCulture = CultureInfo.InvariantCulture
            };

            //var network = new Task(() => Network.TickLoop(), TaskCreationOptions.LongRunning);
            //network.ContinueWith(Program.Stop, TaskContinuationOptions.OnlyOnFaulted);
            _network.Start();

            Log.Info("Realm Manager started.");
        }

        public void Stop()
        {
            Log.Info("Stopping Realm Manager...");

            Terminating = true;
            EquippedManager.Dispose();
            InterServer.Dispose();
            Resources.Dispose();
            Network.Shutdown();

            Log.Info("Realm Manager stopped.");
        }

        public bool TryConnect(Client client)
        {
            if (client?.Account == null)
                return false;
            if (Clients.Keys.Contains(client))
                Disconnect(client);
            client.Id = Interlocked.Increment(ref _nextClientId);
            var plrInfo = new PlayerInfo()
            {
                AccountId = client.Account.AccountId,
                GuildId = client.Account.GuildId,
                Name = client.Account.Name,
                WorldInstance = -1
            };
            Clients[client] = plrInfo;

            // recalculate usage statistics
            Config.serverInfo.players = ConMan.GetPlayerCount();
            Config.serverInfo.maxPlayers = Config.serverSettings.maxPlayers;
            Config.serverInfo.queueLength = ConMan.QueueLength();
            Config.serverInfo.playerList.Add(plrInfo);
            return true;
        }

        public void Disconnect(Client client)
        {
            var player = client.Player;
            player?.Owner?.LeaveWorld(player);

            PlayerInfo plrInfo;
            Clients.TryRemove(client, out plrInfo);

            // recalculate usage statistics
            Config.serverInfo.players = ConMan.GetPlayerCount();
            Config.serverInfo.maxPlayers = Config.serverSettings.maxPlayers;
            Config.serverInfo.queueLength = ConMan.QueueLength();
            Config.serverInfo.playerList.Remove(plrInfo);
        }

        private void AddWorld(string name, bool actAsNexus = false)
        {
            AddWorld(Resources.Worlds.Data[name], actAsNexus);
        }

        private void AddWorld(ProtoWorld proto, bool actAsNexus = false)
        {
            int id;
            if (actAsNexus)
            {
                id = World.Nexus;
            }
            else
            {
                id = (proto.id < 0)
                    ? proto.id
                    : Interlocked.Increment(ref _nextWorldId);
            }

            World world;
            DynamicWorld.TryGetWorld(proto, null, out world);
            if (world != null)
            {
                AddWorld(id, world);
                return;
            }

            AddWorld(id, new World(proto));
        }

        private void AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = id;
            Worlds[id] = world;
            if (_initialized)
                OnWorldAdded(world);
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = Interlocked.Increment(ref _nextWorldId);
            Worlds[world.Id] = world;
            if (_initialized)
                OnWorldAdded(world);
            return world;
        }

        public World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");
            if (Worlds.TryRemove(world.Id, out world))
            {
                OnWorldRemoved(world);
                return true;
            }
            else
                return false;
        }

        void OnWorldAdded(World world)
        {
            world.Manager = this;
            Log.InfoFormat("World {0}({1}) added. {2} Worlds existing.", world.Id, world.Name, Worlds.Count);
        }

        void OnWorldRemoved(World world)
        {
            //world.Manager = null;
            Monitor.RemovePortal(world.Id);
            Log.InfoFormat("World {0}({1}) removed.", world.Id, world.Name);
        }

        public World GetRandomGameWorld()
        {
            var realms = Worlds.Values
                .OfType<Realm>()
                .Where(w => !w.Closed)
                .ToArray();

            return realms.Length == 0 ?
                Worlds[World.Nexus] :
                realms[Environment.TickCount % realms.Length];
        }

    }
}
