using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using wServer.realm.entities;
using wServer.realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using common.resources;
using StackExchange.Redis;
using wServer.networking.packets;
using wServer.networking.packets.outgoing;
using wServer.networking.packets.outgoing.pets;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using PetYard = wServer.realm.worlds.logic.PetYard;
using wServer.realm.worlds.logic;

namespace wServer.logic.loot
{
    public class LootDef
    {
        public readonly Item Item;
        public double Probabilty;
        public readonly int NumRequired;
        public readonly double Threshold;
        public readonly bool Damagebased;

        public LootDef(Item item, double probabilty, int numRequired, double threshold = 0, bool damagebased = false)
        {
            Item = item;
            Probabilty = probabilty;
            NumRequired = numRequired;
            Threshold = threshold;
            Damagebased = damagebased;
        }
    }

    public class Loot : List<MobDrops>
    {
        static readonly Random Rand = new Random();

        static readonly ushort BROWN_BAG = 0x0500;
        static readonly ushort PINK_BAG = 0x0506;
        static readonly ushort PURPLE_BAG = 0x0507;
        static readonly ushort MATERIAL_BAG = 0x0508;
        static readonly ushort CYAN_BAG = 0x0509;
        static readonly ushort POTION_BAG = 0x050B;
        static readonly ushort SET_BAG = 0x050C;
        static readonly ushort TIERED_BAG = 0x050E;
        static readonly ushort WHITE_BAG = 0x050F;
        static readonly ushort FORGE_BAG = 0x3118;
        static readonly ushort LEGENDARY_BAG = 0x4018;
        static readonly ushort STELLAR_BAG = 0x6020;
        static readonly ushort RADIANT_BAG = 0x4a50;
        //boosted bags
        static readonly ushort BOOSTED_BROWN_BAG = 0x4330;
        static readonly ushort BOOSTED_PINK_BAG = 0x4331;
        static readonly ushort BOOSTED_PURPLE_BAG = 0x4332;
        static readonly ushort BOOSTED_MATERIAL_BAG = 0x4333;
        static readonly ushort BOOSTED_CYAN_BAG = 0x4334;
        static readonly ushort BOOSTED_POTION_BAG = 0x4335;
        static readonly ushort BOOSTED_SET_BAG = 0x4336;
        static readonly ushort BOOSTED_TIERED_BAG = 0x4336;
        static readonly ushort BOOSTED_WHITE_BAG = 0x4337;
        static readonly ushort BOOSTED_FORGE_BAG = 0x3119;
        static readonly ushort BOOSTED_LEGENDARY_BAG = 0x4339;
        static readonly ushort BOOSTED_STELLAR_BAG = 0x4340;
        static readonly ushort BOOSTED_RADIANT_BAG = 0x4a51;

        public Loot(params MobDrops[] lootDefs)   //For independent loots(e.g. chests)
        {
            AddRange(lootDefs);
        }

        public IEnumerable<Item> GetLoots(RealmManager manager, int min, int max)   //For independent loots(e.g. chests)
        {
            var consideration = new List<LootDef>();
            foreach (var i in this)
            {
                i.Populate(consideration);
            }

            var retCount = Rand.Next(min, max);
            foreach (var i in consideration)
            {
                if (Rand.NextDouble() < i.Probabilty)
                {
                    yield return i.Item;
                    retCount--;
                }
                if (retCount == 0)
                {
                    yield break;
                }
            }
        }

        private List<LootDef> GetPossibleDrops()
        {
            var possibleDrops = new List<LootDef>();
            foreach (var i in this)
            {
                i.Populate(possibleDrops);
            }
            return possibleDrops;
        }
        public void Handle(Enemy enemy, RealmTime time)
        {
            // enemies that shouldn't drop loot
            if (enemy.Owner is Arena || enemy.Owner is ArenaSolo)
            {
                return;
            }

            // generate a list of all possible loot drops
            var possibleDrops = GetPossibleDrops();
            var reqDrops = possibleDrops.ToDictionary(drop => drop, drop => drop.NumRequired);

            // generate public loot
            var publicLoot = new List<Item>();
            foreach (var i in possibleDrops)
            {
                if (i.Threshold <= 0 && Rand.NextDouble() < i.Probabilty)
                {
                    publicLoot.Add(i.Item);
                    reqDrops[i]--;
                }
            }
            //changed from multiplying lb to solid lb due to it having a greater effect on richer players, players that aren't 
            //maxed in lb will be worse off than players with max lb characters.
            // generate individual player loot
            var eligiblePlayers = enemy.DamageCounter.GetPlayerData();
            var privateLoot = new Dictionary<Player, IList<Item>>();
            var x1 = eligiblePlayers.Select(_ => _.Item2).ToList();
            var x2 = 0;
            if (!x1.Any())
                x2 = enemy.MaximumHP * 4; //lil fair ;)
            else
                x2 = x1.Max();
            foreach (var player in eligiblePlayers)
            {
                var eventBoostInfo = Program.Config.eventsInfo.LootBoost;
                var eventBoost = eventBoostInfo.Item1 ? eventBoostInfo.Item2 : 0; //Event LB, lowest amount: 30%, Highest 50%.
                var damagelootboost = 0.01 * (player.Item2 / (enemy.MaximumHP * 0.01)); // (50000 / (50000 * 0.01)) * 0.01, | 1% dmg dealt = 1% Extra Loot Boost.
                var lootDropBoost = player.Item1.LDBoostTime > 0 ? .50 : 0; // Lootboost Potions.
                var luckStatBoost = (player.Item1.Stats.Boost[10] + player.Item1.Stats.Base[10]) / 100.0; // Lootboost stat.
                var dmgpercentage = (float)Math.Round(player.Item2 / (double)enemy.MaximumHP * 100, 2); // Calculate what % of damage you dealt.
                var sizeBoost = enemy.SizeBoost;

                if (player.Item1.AccountId == 4802) //Person Specific Extra LB
                {
                    luckStatBoost = (player.Item1.Stats.Boost[10] + player.Item1.Stats.Base[10] + 150) / 100.0;
                }
                var loot = new List<Item>();
                foreach (var i in possibleDrops)
                {
                    if (player.Item2! >= enemy.MaximumHP / 200) // deal atleast 0.5% damage to qualify for loot.
                    {
                        var DMGExtraLB = player.Item2 / x2; // Maximum lb 25% (deal 100% of the bosses hp for a 25% increase to lb)
                        var LootChance = i.Probabilty * (1 + sizeBoost); // Item Drop Chance * Solo Boss Drop LB which is (BOSSHP * 4) / DMG)
                        if (i.Item.Tier >= 0 && i.Item.Potion != true)// If item is tiered, dont drop loot modified with loot boost stats.
                        {
                            if (Rand.NextDouble() < LootChance * (1 + lootDropBoost + eventBoost))
                            {
                                loot.Add(i.Item);
                                reqDrops[i]--;
                            }
                        }
                        else if ((i.Item.BagType == 9 || i.Item.LG || i.Item.MY || i.Item.MLG) && dmgpercentage < 2) { } //If item is a legendary, you need to deal 2% of the bosses health to qualify.
                        else if ((i.Item.BagType == 10 || i.Item.Mythical) && dmgpercentage < 4) { } //If item is a mythical, you need to deal 4% of the bosses health to qualify.
                        else if ((i.Item.BagType == 12 || i.Item.Radiant) && dmgpercentage < 5) { } //If item is a Radiant, you need to deal 5% of the bosses health to qualify.
                        else if (i.Damagebased)//This is to make sure loot like Mighty Chest aren't giving you extra lb from soloing them.
                        {
                            if (Rand.NextDouble() < LootChance * (1 + (lootDropBoost + luckStatBoost + eventBoost + damagelootboost + DMGExtraLB))) //MAX: 50 + (50 - 145) + 100 + 25 = 225% increase to 325% increase
                            {
                                loot.Add(i.Item);
                                reqDrops[i]--;
                            }
                        }
                        else if (Rand.NextDouble() < LootChance * (1 + (lootDropBoost + luckStatBoost + eventBoost)))
                        {
                            loot.Add(i.Item);
                            reqDrops[i]--;
                        }
                    }
                }
                privateLoot[player.Item1] = loot;
            }

            // add required drops that didn't drop already
            foreach (var i in possibleDrops)
            {
                if (i.Threshold <= 0)
                {
                    // add public required loot
                    while (reqDrops[i] > 0)
                    {
                        publicLoot.Add(i.Item);
                        reqDrops[i]--;
                    }
                    continue;
                }

                // add private required loot
                var ePlayers = eligiblePlayers.Where(p => i.Threshold <= p.Item2).ToList();
                if (ePlayers.Count() <= 0)
                {
                    continue;
                }

                while (reqDrops[i] > 0 && ePlayers.Count() > 0)
                {
                    // make sure a player doesn't recieve more than one required loot
                    var reciever = ePlayers.RandomElement(Rand);
                    ePlayers.Remove(reciever);

                    // don't assign item if player already recieved one with random chance
                    if (privateLoot[reciever.Item1].Contains(i.Item))
                    {
                        continue;
                    }

                    privateLoot[reciever.Item1].Add(i.Item);
                    reqDrops[i]--;
                }
            }

            AddBagsToWorld(enemy, publicLoot, privateLoot);
        }

        private void AddBagsToWorld(Enemy enemy, IList<Item> shared, IDictionary<Player, IList<Item>> playerLoot)
        {
            foreach (var i in playerLoot)
            {
                if (i.Value.Count > 0)
                {
                    ShowBags(enemy, i.Value, i.Key);
                }
            }
            ShowBags(enemy, shared);
        }

        private static void ShowBags(Enemy enemy, IEnumerable<Item> loots, params Player[] owners)
        {
            var ownerIds = owners.Select(x => x.AccountId).ToArray();
            var items = new Item[8];
            var idx = 0;
            var boosted = false;
            var data = enemy.DamageCounter.GetPlayerData();
            var bagType = 0;
            var damage = 0;
            var whitebags = new List<string>();
            var found = false;
            float dmgpercentage = 0;
            string x2 = "";
            int color = 0;
            string bagURL = "";
            if (owners.Count() == 1 && owners[0].LDBoostTime > 0)
            {
                boosted = true;
            }

            if (enemy.ObjectDesc.TrollWhiteBag)
            {
                bagType = 9;
            }

            foreach (var i in loots)
            {
                if (i.BagType > bagType)
                {
                    bagType = i.BagType;
                }

                items[idx] = i;
                idx++;

                if (i.Mythical)
                {
                    x2 = "Mythical";
                    color = 15269888;
                    bagURL = "https://i.ibb.co/ZS9ZvDs/MYBGASMALL.png";
                    try
                    {
                        if (!found)
                            foreach (var p in data)
                            {
                                if (p.Item1.Name == owners[0].Name)
                                {
                                    damage = p.Item2;
                                    found = true;
                                }
                            }
                        dmgpercentage = (float)Math.Round(damage / (double)enemy.MaximumHP * 100, 2);
                        if (dmgpercentage > 100)
                            dmgpercentage = 100;
                        enemy.Manager.Chat.LootNotifier($"[{owners[0].Name}] just got some {x2} loot, [{i.ObjectId}] with {dmgpercentage}% damage dealt!");
                       // wServer.networking.webhooks.Webhooks.SendToDiscordAsLootLog("Cyberious Loot", x2, owners[0].Name + " just got: **" + i.ObjectId + "**\nDamage Dealt: " + dmgpercentage + "%", color, bagURL);
                        owners[0].Client.SendPacket(new GlobalNotification() { Text = x2 + "PopupUI" });
                    }
                    catch { }
                }

                if (i.LG || i.MLG || i.MY)
                {
                    x2 = "Legendary";
                    color = 16573187;
                    bagURL = "https://i.ibb.co/zQF0fjm/LGBAGSMALL.png";
                    try
                    {
                        //check for player dmg
                        if (!found)
                            foreach (var p in data)
                            {
                                if (p.Item1.Name == owners[0].Name)
                                {
                                    damage = p.Item2;
                                    found = true;
                                }
                            }
                        dmgpercentage = (float)Math.Round(damage / (double)enemy.MaximumHP * 100, 2);
                        if (dmgpercentage > 100)
                            dmgpercentage = 100;
                        enemy.Manager.Chat.LootNotifier($"[{owners[0].Name}] just got some {x2} loot, [{i.ObjectId}] with {dmgpercentage}% damage dealt!");
                       // wServer.networking.webhooks.Webhooks.SendToDiscordAsLootLog("Cyberious Loot", x2, owners[0].Name + " just got: **" + i.ObjectId + "**\nDamage Dealt: " + dmgpercentage + "%", color, bagURL);
                        owners[0].Client.SendPacket(new GlobalNotification() { Text = x2 + "PopupUI" });
                    }
                    catch { }

                }

                if (i.BagType == 12)
                {
                    x2 = "Radiant";
                    color = 7684266;
                    bagURL = "https://i.ibb.co/56f9Jyb/Radiant-Bag.png";
                    try
                    {
                        if (!found)
                            foreach (var p in data)
                            {
                                if (p.Item1.Name == owners[0].Name)
                                {
                                    damage = p.Item2;
                                    found = true;
                                }
                            }
                        dmgpercentage = (float)Math.Round(damage / (double)enemy.MaximumHP * 100, 2);
                        if (dmgpercentage > 100)
                            dmgpercentage = 100;
                        enemy.Manager.Chat.LootNotifier($"[{owners[0].Name}] JUST GOT {x2} ITEM! ({i.ObjectId})");
                       // wServer.networking.webhooks.Webhooks.SendToDiscordAsLootLog("Cyberious Loot", x2, owners[0].Name + " just got: **" + i.ObjectId + "**\nDamage Dealt: " + dmgpercentage + "%", color, bagURL);
                        owners[0].Client.SendPacket(new GlobalNotification() { Text = x2 + "PopupUI" });

                    }
                    catch { }
                }

                if (idx != 8)
                {
                    continue;
                }

                ShowBag(enemy, ownerIds, bagType, items, boosted);

                bagType = 0;
                items = new Item[8];
                idx = 0;
            }
            if (idx > 0)
            {
                ShowBag(enemy, ownerIds, bagType, items, boosted);
            }
        }

        private static void ShowBag(Enemy enemy, int[] owners, int bagType, Item[] items, bool boosted)
        {
            ushort bag = BROWN_BAG;
            switch (bagType)
            {
                case 0: bag = BROWN_BAG; break;
                case 1: bag = PINK_BAG; break;
                case 2: bag = PURPLE_BAG; break;
                case 5: bag = MATERIAL_BAG; break;
                case 3: bag = CYAN_BAG; break;
                case 4: bag = POTION_BAG; break;
                case 6: bag = TIERED_BAG; break;
                case 7: bag = SET_BAG; break;
                case 8: bag = WHITE_BAG; break;
                case 11: bag = FORGE_BAG; break;
                case 9: bag = LEGENDARY_BAG; break;
                case 10: bag = STELLAR_BAG; break;
                case 12: bag = RADIANT_BAG; break;
            }

            // allow white bags to override boosted bag

            if (boosted)
            {
                switch (bagType)
                {
                    case 0: bag = BOOSTED_BROWN_BAG; break;
                    case 1: bag = BOOSTED_PINK_BAG; break;
                    case 2: bag = BOOSTED_PURPLE_BAG; break;
                    case 5: bag = BOOSTED_MATERIAL_BAG; break;
                    case 3: bag = BOOSTED_CYAN_BAG; break;
                    case 4: bag = BOOSTED_POTION_BAG; break;
                    case 6: bag = BOOSTED_TIERED_BAG; break;
                    case 7: bag = BOOSTED_SET_BAG; break;
                    case 8: bag = BOOSTED_WHITE_BAG; break;
                    case 11: bag = BOOSTED_FORGE_BAG; break;
                    case 9: bag = BOOSTED_LEGENDARY_BAG; break;
                    case 10: bag = BOOSTED_STELLAR_BAG; break;
                    case 12: bag = BOOSTED_RADIANT_BAG; break;
                }
            }

            var container = new Container(enemy.Manager, bag, 1000 * 60, true);
            for (int j = 0; j < 8; j++)
            {
                container.Inventory[j] = items[j];
            }
            container.BagOwners = owners;
            container.Move(
                enemy.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5),
                enemy.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(bagType > 3 ? 110 : 80);
            enemy.Owner.EnterWorld(container);
        }
    }
}
