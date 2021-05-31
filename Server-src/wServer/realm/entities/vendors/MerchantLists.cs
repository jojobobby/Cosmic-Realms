using System;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using log4net;
using wServer.realm.terrain;

namespace wServer.realm.entities.vendors
{
    public class ShopItem : ISellableItem
    {
        public ushort ItemId { get; private set; }
        public int Price { get; }
        public int Count { get; }
        public string Name { get; }

        public ShopItem(string name, ushort price, int count = -1)
        {
            ItemId = ushort.MaxValue;
            Price = price;
            Count = count;
            Name = name;
        }

        public void SetItem(ushort item)
        {
            if (ItemId != ushort.MaxValue)
                throw new AccessViolationException("Can't change item after it has been set.");

            ItemId = item;
        }
    }

    internal static class MerchantLists
    {


        private static readonly ILog Log = LogManager.GetLogger(typeof(MerchantLists));

        private static readonly List<ISellableItem> Weapons = new List<ISellableItem>
        {
            new ShopItem("Dagger of Foul Malevolence", 500),
            new ShopItem("Bow of Covert Havens", 500),
            new ShopItem("Staff of the Cosmic Whole", 500),
            new ShopItem("Wand of Recompense", 500),
            new ShopItem("Sword of Acclaim", 500)
        };

        private static readonly List<ISellableItem> Abilities = new List<ISellableItem>
        {
            new ShopItem("Cloak of Ghostly Concealment", 500),
            new ShopItem("Quiver of Elvish Mastery", 500),
            new ShopItem("Elemental Detonation Spell", 500),
            new ShopItem("Tome of Holy Guidance", 500),
            new ShopItem("Helm of the Great General", 500),
            new ShopItem("Colossus Shield", 500),
            new ShopItem("Seal of the Blessed Champion", 500),
            new ShopItem("Baneserpent Poison", 500),
            new ShopItem("Bloodsucker Skull", 500),
            new ShopItem("Giantcatcher Trap", 500),
            new ShopItem("Planefetter Orb", 500),
            new ShopItem("Prism of Apparitions", 500),
            new ShopItem("Scepter of Storms", 500)
        };

        private static readonly List<ISellableItem> Armor = new List<ISellableItem>
        {
            new ShopItem("Robe of the Illusionist", 50),
            new ShopItem("Robe of the Grand Sorcerer", 500),
            new ShopItem("Studded Leather Armor", 50),
            new ShopItem("Hydra Skin Armor", 500),
            new ShopItem("Mithril Armor", 50),
            new ShopItem("Acropolis Armor", 500)
        };

        private static readonly List<ISellableItem> Rings = new List<ISellableItem>
        {
            new ShopItem("Ring of Paramount Attack", 100),
            new ShopItem("Ring of Paramount Defense", 100),
            new ShopItem("Ring of Paramount Speed", 100),
            new ShopItem("Ring of Paramount Dexterity", 100),
            new ShopItem("Ring of Paramount Vitality", 100),
            new ShopItem("Ring of Paramount Wisdom", 100),
            new ShopItem("Ring of Paramount Health", 100),
            new ShopItem("Ring of Paramount Magic", 100),
            new ShopItem("Ring of Unbound Attack", 750),
            new ShopItem("Ring of Unbound Defense", 750),
            new ShopItem("Ring of Unbound Speed", 750),
            new ShopItem("Ring of Unbound Dexterity", 750),
            new ShopItem("Ring of Unbound Vitality", 750),
            new ShopItem("Ring of Unbound Wisdom", 750),
            new ShopItem("Ring of Unbound Health", 750),
            new ShopItem("Ring of Unbound Magic", 750)
        };

        private static readonly List<ISellableItem> Keys = new List<ISellableItem>
        { 
            new ShopItem("Snake Pit Key", 500),//Crystal Cave Key Forgotten Garden Key Spectre's Lair Key Void Entity Key Draconis Key
            new ShopItem("Sprite World Key", 500),
            new ShopItem("Candy Key", 500),
            new ShopItem("Treasure Map", 500),
            new ShopItem("Undead Lair Key", 500),
            new ShopItem("Abyss of Demons Key", 500),
            new ShopItem("Manor Key", 500),
            new ShopItem("Theatre Key",  500),
            new ShopItem("Toxic Sewers Key", 500),
            new ShopItem("Lost Halls Key", 5000),
            new ShopItem("Oryx's Castle Key", 5000),
            new ShopItem("Lab Key", 750),
            new ShopItem("Shaitan's Key", 750),
            new ShopItem("Davy's Key", 750),
            new ShopItem("Mountain Temple Key", 500),
            //new ShopItem("Draconis Key", 750),
            new ShopItem("Ocean Trench Key", 750),
            new ShopItem("Crystal Cave Key", 750),
            new ShopItem("Forgotten Garden Key", 750),
            new ShopItem("Tomb of the Ancients Key", 750),
            new ShopItem("Woodland Labyrinth Key", 750),
            new ShopItem("Ice Tomb Key", 750),
            new ShopItem("Spectre's Lair Key", 750),
            new ShopItem("The Crawling Depths Key", 750),
            new ShopItem("Deadwater Docks Key", 750),
            new ShopItem("Ice Cave Key", 500),
            new ShopItem("Shatters Key", 1000),
            new ShopItem("Cultist Hideout Key", 20000)
        };

        private static readonly List<ISellableItem> PurchasableFame = new List<ISellableItem>
        {
            new ShopItem("50 Fame", 55),
            new ShopItem("100 Fame", 110),
            new ShopItem("500 Fame", 550),
            new ShopItem("1000 Fame", 1100),
            new ShopItem("5000 Fame", 5500)
        };

        private static readonly List<ISellableItem> Donator = new List<ISellableItem>
        {
            new ShopItem("Earth Scroll 3", 6000),//Trick White Bag
            new ShopItem("Earth Scroll 6", 4000),
            new ShopItem("Earth Scroll 9", 2500),
            new ShopItem("Trick White Bag", 250),
            new ShopItem("Loot Drop Potion", 3500),
        };

        private static readonly List<ISellableItem> Consumables = new List<ISellableItem>
        {
            new ShopItem("Loot Drop Potion", 5000),
            new ShopItem("Standard Chest", 1000),
            new ShopItem("Grand Master Chest Item", 10000),
            new ShopItem("Grand Champion Chest Item", 20000),
            new ShopItem("Potion Crate", 500)
        };

        private static readonly List<ISellableItem> Dyes = new List<ISellableItem>
        {
            new ShopItem("Frozen Ice Shard", 20000),
            new ShopItem("Topaz", 20000),
            new ShopItem("Jade", 20000),
            new ShopItem("Amethyst", 20000),
            new ShopItem("Power Battery", 20000),
            new ShopItem("Carnelian", 20000),
            new ShopItem("Sprite Essence", 20000),
            new ShopItem("Fragments of Void Matter", 20000),
            new ShopItem("Ectoplasm", 30000),
            new ShopItem("Cyberious Infused Shard", 40000),
            new ShopItem("Basic Alien Tech", 30000),
            new ShopItem("Red Ichor", 30000),
            new ShopItem("Super Magic Mushroom", 30000),
            new ShopItem("Shard of Fire", 30000),
            new ShopItem("Blue Power Crystal", 60000),
            new ShopItem("Scrapped Ship Plates", 10000),
            new ShopItem("Amalgamation Mold", 20000),
            new ShopItem("Support Box", 40000),
            new ShopItem("Glowing Void Matter", 60000),
            new ShopItem("Cyberious's Plate", 35000),
            new ShopItem("Alien U.F.O Plate", 30000),
            new ShopItem("Destructor AI Module", 30000),
            new ShopItem("Engine Cooling Module", 30000),
            new ShopItem("Energy Converter Module", 30000),
            new ShopItem("Atomic Battery Module", 30000),
            new ShopItem("Advanced Alien Tech", 50000),
            new ShopItem("Plate of Tidale", 10000),
            new ShopItem("Scraps of the Descendant", 5000),
            new ShopItem("Spell Scroll", 20000),

        };

        private static readonly List<ISellableItem> Special = new List<ISellableItem>
        {
            new ShopItem("Amulet of Resurrection", 50000)
        };

        static void InitDyes(RealmManager manager)
        {
            var d1 = new List<ISellableItem>();
            var d2 = new List<ISellableItem>();
            foreach (var i in manager.Resources.GameData.Items.Values)
            {
                if (!i.Class.Equals("Dye"))
                    continue;

                if (i.Texture1 != 0)
                {
                    ushort price = 60;
                    if (i.ObjectId.Contains("Cloth") && i.ObjectId.Contains("Large"))
                        price *= 2;
                    d1.Add(new ShopItem(i.ObjectId, price));
                    continue;
                }

                if (i.Texture2 != 0)
                {
                    ushort price = 60;
                    if (i.ObjectId.Contains("Cloth") && i.ObjectId.Contains("Small"))
                        price *= 2;
                    d2.Add(new ShopItem(i.ObjectId, price));
                    continue;
                }
            }
           Shops[TileRegion.Store_10] = new Tuple<List<ISellableItem>, CurrencyType, int>(d1, CurrencyType.Gold, 0);
           Shops[TileRegion.Store_11] = new Tuple<List<ISellableItem>, CurrencyType, int>(d2, CurrencyType.Gold, 0);
        }

        public static readonly Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, /*Rank Req*/int>> Shops =
            new Dictionary<TileRegion, Tuple<List<ISellableItem>, CurrencyType, int>>()
        {
            { TileRegion.Store_1, new Tuple<List<ISellableItem>, CurrencyType, int>(Weapons, CurrencyType.Fame, 0) },
            { TileRegion.Store_2, new Tuple<List<ISellableItem>, CurrencyType, int>(Abilities, CurrencyType.Fame, 0) },
            { TileRegion.Store_3, new Tuple<List<ISellableItem>, CurrencyType, int>(Armor, CurrencyType.Fame, 0) },
            { TileRegion.Store_4, new Tuple<List<ISellableItem>, CurrencyType, int>(Rings, CurrencyType.Fame, 0) },
            { TileRegion.Store_5, new Tuple<List<ISellableItem>, CurrencyType, int>(Keys, CurrencyType.Fame, 2) },
            { TileRegion.Store_7, new Tuple<List<ISellableItem>, CurrencyType, int>(Consumables, CurrencyType.Fame, 0) },
            { TileRegion.Store_8, new Tuple<List<ISellableItem>, CurrencyType, int>(Special, CurrencyType.Fame, 0) },
            { TileRegion.Store_9, new Tuple<List<ISellableItem>, CurrencyType, int>(Donator, CurrencyType.Gold, 0) },
            { TileRegion.Store_15, new Tuple<List<ISellableItem>, CurrencyType, int>(Dyes, CurrencyType.Gold, 0) },
            { TileRegion.Store_14, new Tuple<List<ISellableItem>, CurrencyType, int>(PurchasableFame, CurrencyType.Fame, 2) },

        };
     
        public static void Init(RealmManager manager)
        {

            InitDyes(manager);
            foreach (var shop in Shops)
                foreach (var shopItem in shop.Value.Item1.OfType<ShopItem>())
                {
                    ushort id;
                    if (!manager.Resources.GameData.IdToObjectType.TryGetValue(shopItem.Name, out id))
                        Log.WarnFormat("Item name: {0}, not found.", shopItem.Name);
                    else
                        shopItem.SetItem(id);
                    
                }
        }
     
    }
}