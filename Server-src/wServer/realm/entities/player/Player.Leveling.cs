using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using wServer.logic;
using wServer.networking.packets;
using wServer.networking;
using wServer.realm.terrain;
using log4net;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds;
using wServer.realm.worlds.logic;
using System.Timers;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities
{
    public partial class Player
    {
        public static int GetExpGoal(int level)
        {
            return 50 + (level - 1) * 100;
        }
        public static int GetLevelExp(int level)
        {
            if (level == 1) return 0;
            return 50 * (level - 1) + (level - 2) * (level - 1) * 50;
        }
        public static int GetFameGoal(int fame)
        {
            if (fame >= 2000) return 0;
            else if (fame >= 800) return 2000;
            else if (fame >= 400) return 800;
            else if (fame >= 150) return 400;
            else if (fame >= 20) return 150;
            else return 20;
        }

        public int GetStars()
        {
            int ret = 0;
            foreach (var i in FameCounter.ClassStats.AllKeys)
            {
                var entry = FameCounter.ClassStats[ushort.Parse(i)];
                if (entry.BestFame >= 2000) ret += 5;
                else if (entry.BestFame >= 800) ret += 4;
                else if (entry.BestFame >= 400) ret += 3;
                else if (entry.BestFame >= 150) ret += 2;
                else if (entry.BestFame >= 20) ret += 1;
            }
            return ret;
        }
        public void GiftForStars()
        {
            int ret = 0;
            foreach (var i in FameCounter.ClassStats.AllKeys)
            {
                var entry = FameCounter.ClassStats[ushort.Parse(i)];
                if (entry.BestFame >= 2000) ret += 5;
                else if (entry.BestFame >= 800) ret += 4;
                else if (entry.BestFame >= 400) ret += 3;
                else if (entry.BestFame >= 150) ret += 2;
                else if (entry.BestFame >= 20) ret += 1;
            }
            if (ret == (FameCounter.ClassStats.AllKeys.Count() * 5) && !Client.Account.WhiteStarPrizeClaimed)
            {
                Manager.Chat.Oryx(Owner, $"{Name} has achieved White Star!");
                Manager.Database.AddGift(Client.Account, 0x1111);
                SendInfo("You got something");
                Client.Account.WhiteStarPrizeClaimed = true;
                Client.Account.FlushAsync();
            }
        }

        static readonly Dictionary<string, Tuple<int, int, int>> QuestDat =
            new Dictionary<string, Tuple<int, int, int>>()  //Priority, Min, Max
        {
            // wandering quest enemies
            { "Scorpion Queen",                 Tuple.Create(1, 1, 6) },
            { "Bandit Leader",                  Tuple.Create(1, 1, 6) },
            { "Hobbit Mage",                    Tuple.Create(3, 3, 8) },
            { "Undead Hobbit Mage",             Tuple.Create(3, 3, 8) },
            { "Giant Crab",                     Tuple.Create(3, 3, 8) },
            { "Desert Werewolf",                Tuple.Create(3, 3, 8) },
            { "Sandsman King",                  Tuple.Create(4, 4, 9) },
            { "Goblin Mage",                    Tuple.Create(4, 4, 9) },
            { "Elf Wizard",                     Tuple.Create(4, 4, 9) },
            { "Dwarf King",                     Tuple.Create(5, 5, 10) },
            { "Swarm",                          Tuple.Create(6, 6, 11) },
            { "Shambling Sludge",               Tuple.Create(6, 6, 11) },
            { "Great Lizard",                   Tuple.Create(7, 7, 12) },
            { "Wasp Queen",                     Tuple.Create(8, 7, 20) },
            { "Horned Drake",                   Tuple.Create(8, 7, 20) },

            // setpiece bosses
            { "Deathmage",                      Tuple.Create(5, 6, 11) },
            { "Great Coil Snake",               Tuple.Create(6, 6, 12) },
            { "Lich",                           Tuple.Create(8, 6, 20) },
            { "Actual Lich",                    Tuple.Create(8, 7, 20) },
            { "Ent Ancient",                    Tuple.Create(9, 7, 20) },
            { "Actual Ent Ancient",             Tuple.Create(9, 7, 20) },
            { "Oasis Giant",                    Tuple.Create(10, 8, 20) },
            { "Phoenix Lord",                   Tuple.Create(10, 9, 20) },
            { "Ghost King",                     Tuple.Create(11,10, 20) },
            { "Actual Ghost King",              Tuple.Create(11,10, 20) },
            { "Cyclops God",                    Tuple.Create(12,10, 20) },
            { "Red Demon",                      Tuple.Create(13,15, 20) },
            { "Beroth, the Earth Dragon",       Tuple.Create(13,15, 20) },
            
            // events
            { "Dragon Head",                    Tuple.Create(15,15, 20) },
            { "Zhin, The Demonic Sacrifice",    Tuple.Create(15,15, 20) },
            { "Lunar: Cube God",                Tuple.Create(15,15, 20) },
            { "Lunar: Grand Sphinx",            Tuple.Create(15,15, 20) },
            { "Rave Darkmore, The Cultist",     Tuple.Create(15,15, 20) },
            { "Kharanos, the Lost Dragon",      Tuple.Create(15,15, 20) },
            { "The Keyper",                     Tuple.Create(15,15, 20) },
            { "Defender of the Bee",            Tuple.Create(15,15, 20) },
            { "Mysterious Crystal Scorpion",    Tuple.Create(15,15, 20) },
            { "Pentaract Tower",                Tuple.Create(15,15, 20) },
            { "Skull Shrine",                   Tuple.Create(15,15, 20) },
            { "Pentaract Tower Ultra",          Tuple.Create(15,15, 20) },
            { "Cube God",                       Tuple.Create(15,15, 20) },
            { "Grand Sphinx",                   Tuple.Create(15,15, 20) },
            { "Lord of the Lost Lands",         Tuple.Create(15,15, 20) },
            { "Hermit God",                     Tuple.Create(15,15, 20) },
            { "Ghost Ship",                     Tuple.Create(15,15, 20) },
            
            { "Lucky Ent God",                  Tuple.Create(16,15, 20) },
            { "Lucky Djinn",                    Tuple.Create(16,15, 20) },
            { "Zombie Horde",                   Tuple.Create(15,15, 20) },
            { "Permafrost Lord",                Tuple.Create(15,15, 20) },
            { "Ghostly Sorcerer",               Tuple.Create(15,15, 20) },
            { "Raijin, the Lightning Guardian", Tuple.Create(15,15, 20) },
            { "Alien U.F.O",                    Tuple.Create(15,15, 20) },
            { "Legendary Dragon Kakashi",       Tuple.Create(15,15, 20) },
            { "Primaeval The Ancient Totem",    Tuple.Create(15,15, 20) },
            { "Bloody Tissue",                  Tuple.Create(15,15, 20) },
            { "Spectre's Guardian",             Tuple.Create(15,15, 20) },
            { "md Janus the Doorwarden",        Tuple.Create(15,15, 20) },
            { "BB Biff the Buffed Bunny",       Tuple.Create(15,15, 20) },
            { "King Toadstool",                 Tuple.Create(15,15, 20) },
            { "Krystaline",                     Tuple.Create(15,15, 20) },

                // dungeon bosses
            { "Katara",                         Tuple.Create(15,1, 20) },
            { "The Void Entity",                Tuple.Create(15,1, 20) },
            { "Cultist of the Halls",           Tuple.Create(15,1, 20) },
            { "The Haunted Omen",               Tuple.Create(15,1, 20) },
            { "Crystal Core",                   Tuple.Create(15,1, 20) },
            { "Forbidden Crystal Prisoner",     Tuple.Create(15,1, 20) },
            { "LH Marble Colossus",             Tuple.Create(15,1, 20) },
            { "Spectre's Phantom",              Tuple.Create(15,1, 20) },
            { "Giant Carukia Barnesi",          Tuple.Create(15,1, 20) },
            { "Son of Belladonn",               Tuple.Create(15,1, 20) },
            { "Rusted Factory Owner",           Tuple.Create(15,1, 20) },
            { "Evil Chicken God",               Tuple.Create(15,1, 20) },
            { "Bonegrind the Butcher",          Tuple.Create(15,1, 20) },
            { "Dreadstump the Pirate King",     Tuple.Create(15,1, 20) },
            { "Mama Megamoth",                  Tuple.Create(15,1, 20) },
            { "Arachna the Spider Queen",       Tuple.Create(15,1, 20) },
            { "Stheno the Snake Queen",         Tuple.Create(15,1, 20) },
            { "Mixcoatl the Masked God",        Tuple.Create(15,1, 20) },
            { "Limon the Sprite God",           Tuple.Create(15,1, 20) },
            { "Septavius the Ghost God",        Tuple.Create(15,1, 20) },
            { "Davy Jones",                     Tuple.Create(15,1, 20) },
            { "Lord Ruthven",                   Tuple.Create(15,1, 20) },
            { "Archdemon Malphas",              Tuple.Create(15,1, 20) },
            { "Thessal the Mermaid Goddess",    Tuple.Create(15,1, 20) },
            { "Dr Terrible",                    Tuple.Create(15,1, 20) },
            { "Horrific Creation",              Tuple.Create(15,1, 20) },
            { "Masked Party God",               Tuple.Create(15,1, 20) },
            { "Oryx Stone Guardian Left",       Tuple.Create(15,1, 20) },
            { "Oryx Stone Guardian Right",      Tuple.Create(15,1, 20) },
            { "Oryx the Mad God 1",             Tuple.Create(15,1, 20) },
            { "Tidale, The Defender of the Ancients",             Tuple.Create(15,1, 20) },
            { "Cyberious, The Commander of the Realm",             Tuple.Create(15,1, 20) },
            { "Gigacorn",                       Tuple.Create(15,1, 20) },
            { "Desire Troll",                   Tuple.Create(15,1, 20) },
            { "Spoiled Creampuff",              Tuple.Create(15,1, 20) },
            { "MegaRototo",                     Tuple.Create(15,1, 20) },
            { "Swoll Fairy",                    Tuple.Create(15,1, 20) },
            { "BedlamGod",                      Tuple.Create(15,1, 20) },
            { "Troll 3",                        Tuple.Create(15,1, 20) },
            { "Arena Ghost Bride",              Tuple.Create(15,1, 20) },
            { "Arena Statue Left",              Tuple.Create(15,1, 20) },
            { "Arena Statue Right",             Tuple.Create(15,1, 20) },
            { "Arena Grave Caretaker",          Tuple.Create(15,1, 20) },
            { "Tomb Defender",                  Tuple.Create(15,1, 20) },
            { "Tomb Support",                   Tuple.Create(15,1, 20) },
            { "Tomb Attacker",                  Tuple.Create(15,1, 20) },
            { "Active Sarcophagus",             Tuple.Create(15,1, 20) },
            { "shtrs Bridge Sentinel",          Tuple.Create(15,1, 20) },
            { "shtrs The Forgotten King",       Tuple.Create(15,1, 20) },
            { "shtrs Twilight Archmage",        Tuple.Create(15,1, 20) },
            { "NM Black Dragon God",            Tuple.Create(15,1, 20) },
            { "NM Black Dragon God Hardmode",   Tuple.Create(15,1, 20) },
            { "NM Red Dragon God",              Tuple.Create(15,1, 20) },
            { "NM Red Dragon God Hardmode",     Tuple.Create(15,1, 20) },
            { "NM Blue Dragon God",             Tuple.Create(15,1, 20) },
            { "NM Blue Dragon God Hardmode",    Tuple.Create(15,1, 20) },
            { "NM Green Dragon God",            Tuple.Create(15,1, 20) },
            { "NM Green Dragon God Hardmode",   Tuple.Create(15,1, 20) },
            { "lod Ivory Wyvern",               Tuple.Create(15,1, 20) },
            { "The Puppet Master",              Tuple.Create(15,1, 20) },
            { "Jon Bilgewater the Pirate King", Tuple.Create(15,1, 20) },
            { "Epic Larva",                     Tuple.Create(15,1, 20) },
            { "Epic Mama Megamoth",             Tuple.Create(15,1, 20) },
            { "Murderous Megamoth",             Tuple.Create(15,1, 20) },
            { "Son of Arachna",                 Tuple.Create(15,1, 20) },
            { "Golden Oryx Effigy",             Tuple.Create(15,1, 20) },
            { "Murderous Megamoth Deux",        Tuple.Create(15,1, 20) },
            { "Lord Ruthven Deux",              Tuple.Create(15,1, 20) },
            { "NM Green Dragon God Deux",       Tuple.Create(15,1, 20) },
            { "Archdemon Malphas Deux",         Tuple.Create(15,1, 20) },
            { "Stheno the Snake Queen Deux",    Tuple.Create(15,1, 20) },
            { "Golden Oryx Effigy Deux",        Tuple.Create(15,1, 20) },
            { "Oryx the Mad God Deux",          Tuple.Create(15,1, 20) },
            { "md1 Head of Shaitan",            Tuple.Create(15,1, 20) },
            { "TestChicken 2",                  Tuple.Create(15,1, 20) },
            { "DS Gulpord the Slime God",       Tuple.Create(15,1, 20) },
            { "Daichi the Fallen",              Tuple.Create(15,1, 20) },
            { "Dracula",                        Tuple.Create(15,1, 20) },


            //Moon 1 Enemies
            { "Actias Luna",                 Tuple.Create(1, 6, 6) },
            { "Lunar Protector",                  Tuple.Create(1, 1, 6) },

            //Moon 2 Enemies
            { "Spinning Tornado",                 Tuple.Create(7, 12, 6) },
            { "Mutated Venenum",                  Tuple.Create(7, 12, 6) },

            //Moon 3 Enemies
         //   { "Actias Luna",                 Tuple.Create(8, 15, 6) },
         //   { "Lunar Protector",                  Tuple.Create(8, 15, 6) },

            //Moon Events
            { "Lunar: Lord of the Lost Lands",          Tuple.Create(15,1, 20) },
            { "Eternal, The Devourer",          Tuple.Create(15,1, 20) },
            { "Celestial Observer",             Tuple.Create(15,1, 20) },
            { "The Jade Rabbit",                Tuple.Create(15,1, 20) },
        };

        Entity FindQuest(Position? destination = null)
        {
            Entity ret = null;
            double? bestScore = null;
            var pX = !destination.HasValue ? X : destination.Value.X;
            var pY = !destination.HasValue ? Y : destination.Value.Y;
            
            foreach (var i in Owner.Quests.Values
                .OrderBy(quest => MathsUtils.DistSqr(quest.X, quest.Y, pX, pY)))
            {
                if (i.ObjectDesc == null || !i.ObjectDesc.Quest) continue;

                Tuple<int, int, int> x;
                if (!QuestDat.TryGetValue(i.ObjectDesc.ObjectId, out x))
                    continue;

                if ((Level >= x.Item2 && Level <= x.Item3))
                {
                    var score = (20 - Math.Abs((i.ObjectDesc.Level ?? 0) - Level)) * x.Item1 -   //priority * level diff
                            this.Dist(i) / 100;    //minus 1 for every 100 tile distance
                    if (bestScore == null || score > bestScore)
                    {
                        bestScore = score;
                        ret = i;
                    }
                }
            }
            return ret;
        }
        private Entity FindSpecialEnemyyById(string objectId)
         => Owner.SpecialEnemies.Values
             .Where(enemy => enemy != null && enemy.ObjectDesc != null)
             .FirstOrDefault(enemy => enemy.ObjectDesc.ObjectId == objectId);
        Entity questEntity;
        public Entity Quest { get { return questEntity; } }
        public void HandleQuest(RealmTime time, bool force = false, Position? destination = null)
        {
            if (force || time.TickCount % 500 == 0 || questEntity == null || questEntity.Owner == null)
            {
                var newQuest = FindQuest(destination);
                if (newQuest != null && newQuest != questEntity)
                {
                    Owner.Timers.Add(new WorldTimer(100, (w, t) =>
                    {
                        _client.SendPacket(new QuestObjId()
                        {
                            ObjectId = newQuest.Id
                        });
                    }));
                    questEntity = newQuest;
                }
            }
        }
        //vlntns Botany Bella
        private Entity spooky;
        private Entity spooky2;
        private Entity spooky3;
        private Entity spooky4;
        private Entity spooky5;
        private Entity spooky6;
        public void HandleSpecialEnemies(RealmTime time, bool force = false)
        {
            if (this == null || Owner == null || Owner.SpecialEnemies == null || time.TickCount % 500 == 0)
                return;

            if (force || spooky == null || spooky2 == null || spooky3 == null || spooky4 == null || spooky5 == null || spooky6 == null)//|| avatar == null || crystal == null)
            {
                var new6Spooky = FindSpecialEnemyyById("vlntns Botany Bella");
                var new5Spooky = FindSpecialEnemyyById("F.E.R.A.L.");
                var new4Spooky = FindSpecialEnemyyById("shtrs Defense System");
                var new3Spooky = FindSpecialEnemyyById("Ghost of Skuld");
                var new2Spooky = FindSpecialEnemyyById("Kage Kami");
                var newSpooky = FindSpecialEnemyyById("Mysterious Crystal");

                if (newSpooky != null && newSpooky != spooky) spooky = newSpooky;
                if (new2Spooky != null && new2Spooky != spooky2) spooky2 = new2Spooky;
                if (new3Spooky != null && new3Spooky != spooky3) spooky3 = new3Spooky;
                if (new4Spooky != null && new4Spooky != spooky4) spooky4 = new4Spooky;
                if (new5Spooky != null && new5Spooky != spooky5) spooky5 = new5Spooky;
                if (new6Spooky != null && new6Spooky != spooky6) spooky6 = new6Spooky;
            }
        }
        public void CalculateFame()
        {
            var newFame = (Experience < 200 * 1000) ?
                Experience / 1000 :
                200 + (Experience - 200 * 1000) / 1000;

            if (newFame == Fame) 
                return;

            var stats = FameCounter.ClassStats[ObjectType];
            var newGoal = GetFameGoal(stats.BestFame > newFame ? stats.BestFame : newFame);

            if (newGoal > FameGoal)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFF00FF00),
                        Message = "{\"key\": \"server.class_quest_complete\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFF00FF00),
                        Message = "{\"key\": \"server.class_quest_complete\"}"
                    }, PacketPriority.Low);
                Stars = GetStars();
            }
            else if (newFame != Fame)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFFE25F00),
                        Message = "+" + (newFame - Fame) + "Fame"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFFE25F00),
                        Message = "+" + (newFame - Fame) + "Fame"
                    }, PacketPriority.Low);
            }
            GiftForStars();
            Fame = newFame;
            FameGoal = newGoal;
        }

        private bool CheckLevelUp()
        {
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
            {
                var chr = _client.Character;
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                var statInfo = Manager.Resources.GameData.Classes[ObjectType].Stats;
                var rand = new Random();
                for (var i = 0; i < statInfo.Length; i++)
                {
                    var min = statInfo[i].MinIncrease;
                    var max = statInfo[i].MaxIncrease + 1;
                    Stats.Base[i] += rand.Next(min, max);

                    if (Stats.Base[0] > statInfo[0].MaxValue)
                    {
                        Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                    }
                    if (Stats.Base[1] > statInfo[1].MaxValue)
                    {
                        Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                    }
                    if (Stats.Base[2] > statInfo[2].MaxValue)
                    {
                        Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                    }
                    if (Stats.Base[3] > statInfo[3].MaxValue)
                    {
                        Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                    }
                    if (Stats.Base[4] > statInfo[4].MaxValue)
                    {
                        Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                    }
                    if (Stats.Base[5] > statInfo[5].MaxValue)
                    {
                        Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                    }
                    if (Stats.Base[6] > statInfo[6].MaxValue)
                    {
                        Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                    }
                    if (Stats.Base[7] > statInfo[7].MaxValue)
                    {
                        Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                    }
                    if (Stats.Base[11] > statInfo[11].MaxValue)
                    {
                        Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                    }
                    if (Stats.Base[12] > statInfo[12].MaxValue)
                    {
                        Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                    }

                }
                HP = Stats[0];
                MP = Stats[1];
                if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                {
                    Level++;
                    ExperienceGoal = GetExpGoal(Level);
                    for (var i = 0; i < statInfo.Length; i++)
                    {
                        var min = statInfo[i].MinIncrease;
                        var max = statInfo[i].MaxIncrease + 1;
                        Stats.Base[i] += rand.Next(min, max);

                        if (Stats.Base[0] > statInfo[0].MaxValue)
                        {
                            Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                        }
                        if (Stats.Base[1] > statInfo[1].MaxValue)
                        {
                            Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                        }
                        if (Stats.Base[2] > statInfo[2].MaxValue)
                        {
                            Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                        }
                        if (Stats.Base[3] > statInfo[3].MaxValue)
                        {
                            Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                        }
                        if (Stats.Base[4] > statInfo[4].MaxValue)
                        {
                            Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                        }
                        if (Stats.Base[5] > statInfo[5].MaxValue)
                        {
                            Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                        }
                        if (Stats.Base[6] > statInfo[6].MaxValue)
                        {
                            Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                        }
                        if (Stats.Base[7] > statInfo[7].MaxValue)
                        {
                            Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                        }
                        if (Stats.Base[11] > statInfo[11].MaxValue)
                        {
                            Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                        }
                        if (Stats.Base[12] > statInfo[12].MaxValue)
                        {
                            Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                        }

                    }
                    HP = Stats[0];
                    MP = Stats[1];
                    if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                    {
                        Level++;
                        ExperienceGoal = GetExpGoal(Level);
                        for (var i = 0; i < statInfo.Length; i++)
                        {
                            var min = statInfo[i].MinIncrease;
                            var max = statInfo[i].MaxIncrease + 1;
                            Stats.Base[i] += rand.Next(min, max);

                            if (Stats.Base[0] > statInfo[0].MaxValue)
                            {
                                Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                            }
                            if (Stats.Base[1] > statInfo[1].MaxValue)
                            {
                                Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                            }
                            if (Stats.Base[2] > statInfo[2].MaxValue)
                            {
                                Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                            }
                            if (Stats.Base[3] > statInfo[3].MaxValue)
                            {
                                Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                            }
                            if (Stats.Base[4] > statInfo[4].MaxValue)
                            {
                                Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                            }
                            if (Stats.Base[5] > statInfo[5].MaxValue)
                            {
                                Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                            }
                            if (Stats.Base[6] > statInfo[6].MaxValue)
                            {
                                Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                            }
                            if (Stats.Base[7] > statInfo[7].MaxValue)
                            {
                                Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                            }
                            if (Stats.Base[11] > statInfo[11].MaxValue)
                            {
                                Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                            }
                            if (Stats.Base[12] > statInfo[12].MaxValue)
                            {
                                Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                            }

                        }
                        HP = Stats[0];
                        MP = Stats[1];
                        if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                        {
                            Level++;
                            ExperienceGoal = GetExpGoal(Level);
                            for (var i = 0; i < statInfo.Length; i++)
                            {
                                var min = statInfo[i].MinIncrease;
                                var max = statInfo[i].MaxIncrease + 1;
                                Stats.Base[i] += rand.Next(min, max);

                                if (Stats.Base[0] > statInfo[0].MaxValue)
                                {
                                    Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                }
                                if (Stats.Base[1] > statInfo[1].MaxValue)
                                {
                                    Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                }
                                if (Stats.Base[2] > statInfo[2].MaxValue)
                                {
                                    Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                }
                                if (Stats.Base[3] > statInfo[3].MaxValue)
                                {
                                    Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                }
                                if (Stats.Base[4] > statInfo[4].MaxValue)
                                {
                                    Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                }
                                if (Stats.Base[5] > statInfo[5].MaxValue)
                                {
                                    Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                }
                                if (Stats.Base[6] > statInfo[6].MaxValue)
                                {
                                    Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                }
                                if (Stats.Base[7] > statInfo[7].MaxValue)
                                {
                                    Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                }
                                if (Stats.Base[11] > statInfo[11].MaxValue)
                                {
                                    Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                }
                                if (Stats.Base[12] > statInfo[12].MaxValue)
                                {
                                    Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                }

                            }
                            HP = Stats[0];
                            MP = Stats[1];
                            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                            {
                                Level++;
                                ExperienceGoal = GetExpGoal(Level);
                                for (var i = 0; i < statInfo.Length; i++)
                                {
                                    var min = statInfo[i].MinIncrease;
                                    var max = statInfo[i].MaxIncrease + 1;
                                    Stats.Base[i] += rand.Next(min, max);

                                    if (Stats.Base[0] > statInfo[0].MaxValue)
                                    {
                                        Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                    }
                                    if (Stats.Base[1] > statInfo[1].MaxValue)
                                    {
                                        Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                    }
                                    if (Stats.Base[2] > statInfo[2].MaxValue)
                                    {
                                        Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                    }
                                    if (Stats.Base[3] > statInfo[3].MaxValue)
                                    {
                                        Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                    }
                                    if (Stats.Base[4] > statInfo[4].MaxValue)
                                    {
                                        Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                    }
                                    if (Stats.Base[5] > statInfo[5].MaxValue)
                                    {
                                        Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                    }
                                    if (Stats.Base[6] > statInfo[6].MaxValue)
                                    {
                                        Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                    }
                                    if (Stats.Base[7] > statInfo[7].MaxValue)
                                    {
                                        Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                    }
                                    if (Stats.Base[11] > statInfo[11].MaxValue)
                                    {
                                        Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                    }
                                    if (Stats.Base[12] > statInfo[12].MaxValue)
                                    {
                                        Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                    }

                                }
                                HP = Stats[0];
                                MP = Stats[1];
                                if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                                {
                                    Level++;
                                    ExperienceGoal = GetExpGoal(Level);
                                    for (var i = 0; i < statInfo.Length; i++)
                                    {
                                        var min = statInfo[i].MinIncrease;
                                        var max = statInfo[i].MaxIncrease + 1;
                                        Stats.Base[i] += rand.Next(min, max);

                                        if (Stats.Base[0] > statInfo[0].MaxValue)
                                        {
                                            Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                        }
                                        if (Stats.Base[1] > statInfo[1].MaxValue)
                                        {
                                            Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                        }
                                        if (Stats.Base[2] > statInfo[2].MaxValue)
                                        {
                                            Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                        }
                                        if (Stats.Base[3] > statInfo[3].MaxValue)
                                        {
                                            Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                        }
                                        if (Stats.Base[4] > statInfo[4].MaxValue)
                                        {
                                            Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                        }
                                        if (Stats.Base[5] > statInfo[5].MaxValue)
                                        {
                                            Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                        }
                                        if (Stats.Base[6] > statInfo[6].MaxValue)
                                        {
                                            Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                        }
                                        if (Stats.Base[7] > statInfo[7].MaxValue)
                                        {
                                            Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                        }
                                        if (Stats.Base[11] > statInfo[11].MaxValue)
                                        {
                                            Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                        }
                                        if (Stats.Base[12] > statInfo[12].MaxValue)
                                        {
                                            Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                        }

                                    }
                                    HP = Stats[0];
                                    MP = Stats[1];
                                    if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                                    {
                                        Level++;
                                        ExperienceGoal = GetExpGoal(Level);
                                        for (var i = 0; i < statInfo.Length; i++)
                                        {
                                            var min = statInfo[i].MinIncrease;
                                            var max = statInfo[i].MaxIncrease + 1;
                                            Stats.Base[i] += rand.Next(min, max);

                                            if (Stats.Base[0] > statInfo[0].MaxValue)
                                            {
                                                Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                            }
                                            if (Stats.Base[1] > statInfo[1].MaxValue)
                                            {
                                                Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                            }
                                            if (Stats.Base[2] > statInfo[2].MaxValue)
                                            {
                                                Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                            }
                                            if (Stats.Base[3] > statInfo[3].MaxValue)
                                            {
                                                Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                            }
                                            if (Stats.Base[4] > statInfo[4].MaxValue)
                                            {
                                                Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                            }
                                            if (Stats.Base[5] > statInfo[5].MaxValue)
                                            {
                                                Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                            }
                                            if (Stats.Base[6] > statInfo[6].MaxValue)
                                            {
                                                Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                            }
                                            if (Stats.Base[7] > statInfo[7].MaxValue)
                                            {
                                                Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                            }
                                            if (Stats.Base[11] > statInfo[11].MaxValue)
                                            {
                                                Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                            }
                                            if (Stats.Base[12] > statInfo[12].MaxValue)
                                            {
                                                Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                            }

                                        }
                                        HP = Stats[0];
                                        MP = Stats[1];
                                        if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                                        {
                                            Level++;
                                            ExperienceGoal = GetExpGoal(Level);
                                            for (var i = 0; i < statInfo.Length; i++)
                                            {
                                                var min = statInfo[i].MinIncrease;
                                                var max = statInfo[i].MaxIncrease + 1;
                                                Stats.Base[i] += rand.Next(min, max);

                                                if (Stats.Base[0] > statInfo[0].MaxValue)
                                                {
                                                    Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                                }
                                                if (Stats.Base[1] > statInfo[1].MaxValue)
                                                {
                                                    Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                                }
                                                if (Stats.Base[2] > statInfo[2].MaxValue)
                                                {
                                                    Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                                }
                                                if (Stats.Base[3] > statInfo[3].MaxValue)
                                                {
                                                    Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                                }
                                                if (Stats.Base[4] > statInfo[4].MaxValue)
                                                {
                                                    Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                                }
                                                if (Stats.Base[5] > statInfo[5].MaxValue)
                                                {
                                                    Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                                }
                                                if (Stats.Base[6] > statInfo[6].MaxValue)
                                                {
                                                    Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                                }
                                                if (Stats.Base[7] > statInfo[7].MaxValue)
                                                {
                                                    Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                                }
                                                if (Stats.Base[11] > statInfo[11].MaxValue)
                                                {
                                                    Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                                }
                                                if (Stats.Base[12] > statInfo[12].MaxValue)
                                                {
                                                    Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                                }

                                            }
                                            HP = Stats[0];
                                            MP = Stats[1];
                                            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                                            {
                                                Level++;
                                                ExperienceGoal = GetExpGoal(Level);
                                                for (var i = 0; i < statInfo.Length; i++)
                                                {
                                                    var min = statInfo[i].MinIncrease;
                                                    var max = statInfo[i].MaxIncrease + 1;
                                                    Stats.Base[i] += rand.Next(min, max);

                                                    if (Stats.Base[0] > statInfo[0].MaxValue)
                                                    {
                                                        Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                                    }
                                                    if (Stats.Base[1] > statInfo[1].MaxValue)
                                                    {
                                                        Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                                    }
                                                    if (Stats.Base[2] > statInfo[2].MaxValue)
                                                    {
                                                        Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                                    }
                                                    if (Stats.Base[3] > statInfo[3].MaxValue)
                                                    {
                                                        Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                                    }
                                                    if (Stats.Base[4] > statInfo[4].MaxValue)
                                                    {
                                                        Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                                    }
                                                    if (Stats.Base[5] > statInfo[5].MaxValue)
                                                    {
                                                        Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                                    }
                                                    if (Stats.Base[6] > statInfo[6].MaxValue)
                                                    {
                                                        Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                                    }
                                                    if (Stats.Base[7] > statInfo[7].MaxValue)
                                                    {
                                                        Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                                    }
                                                    if (Stats.Base[11] > statInfo[11].MaxValue)
                                                    {
                                                        Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                                    }
                                                    if (Stats.Base[12] > statInfo[12].MaxValue)
                                                    {
                                                        Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                                    }

                                                }
                                                HP = Stats[0];
                                                MP = Stats[1];

                                                if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
                                                {
                                                    Level++;
                                                    ExperienceGoal = GetExpGoal(Level);
                                                    for (var i = 0; i < statInfo.Length; i++)
                                                    {
                                                        var min = statInfo[i].MinIncrease;
                                                        var max = statInfo[i].MaxIncrease + 1;
                                                        Stats.Base[i] += rand.Next(min, max);
                                                     

                                                        if (Stats.Base[0] > statInfo[0].MaxValue)
                                                        {
                                                            Stats.Base[0] = statInfo[0].MaxValue + chr.LifePotsMoon;
                                                        }
                                                        if (Stats.Base[1] > statInfo[1].MaxValue)
                                                        {
                                                            Stats.Base[1] = statInfo[1].MaxValue + chr.ManaPotsMoon;
                                                        }
                                                        if (Stats.Base[2] > statInfo[2].MaxValue)
                                                        {
                                                            Stats.Base[2] = statInfo[2].MaxValue + chr.AttackStatsMoon;
                                                        }
                                                        if (Stats.Base[3] > statInfo[3].MaxValue)
                                                        {
                                                            Stats.Base[3] = statInfo[3].MaxValue + chr.DefensePotsMoon;
                                                        }
                                                        if (Stats.Base[4] > statInfo[4].MaxValue)
                                                        {
                                                            Stats.Base[4] = statInfo[4].MaxValue + chr.SpeedPotsMoon;
                                                        }
                                                        if (Stats.Base[5] > statInfo[5].MaxValue)
                                                        {
                                                            Stats.Base[5] = statInfo[5].MaxValue + chr.DexterityPotsMoon;
                                                        }
                                                        if (Stats.Base[6] > statInfo[6].MaxValue)
                                                        {
                                                            Stats.Base[6] = statInfo[6].MaxValue + chr.VitalityPotsMoon;
                                                        }
                                                        if (Stats.Base[7] > statInfo[7].MaxValue)
                                                        {
                                                            Stats.Base[7] = statInfo[7].MaxValue + chr.WisdomPotsMoon;
                                                        }
                                                        if (Stats.Base[11] > statInfo[11].MaxValue)
                                                        {
                                                            Stats.Base[11] = statInfo[11].MaxValue + chr.CritDmgPotsMoon;
                                                        }
                                                        if (Stats.Base[12] > statInfo[12].MaxValue)
                                                        {
                                                            Stats.Base[12] = statInfo[12].MaxValue + chr.CritHitPotsMoon;
                                                        }


                                                    }
                                                    HP = Stats[0];
                                                    MP = Stats[1];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var playerDesc = Manager.Resources.GameData.Classes[ObjectType];

                if (Level == 20)
                {
                    foreach (var i in Owner.Players.Values)
                    {
                        i.SendInfo(Name + " achieved level 20 as a " + playerDesc.ObjectId + "!");
                    }
                }
                else
                {
                    // to get exp scaled to new exp goal
                    InvokeStatChange(StatsType.Experience, Experience - GetLevelExp(Level), true);
                }

                questEntity = null;

                Stats.Boost.ActivateBoost[2].Push(10, true);
                Stats.ReCalculateValues();

                Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    Stats.Boost.ActivateBoost[2].Pop(10, true);
                    Stats.ReCalculateValues();

                }));

                return true;
            }
            CalculateFame();
            return false;
        }

        public bool EnemyKilled(Enemy enemy, int exp, bool killer)
        {
            if (enemy == questEntity)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFF00FF00),
                        Message = "{\"key\":\"server.quest_complete\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFF00FF00),
                        Message = "{\"key\":\"server.quest_complete\"}"
                    }, PacketPriority.Low);
            }
            if (exp != 0)
            {
                Experience += exp;
            }
            FameCounter.Killed(enemy, killer);
            
           

           
            return CheckLevelUp();

        }

    }
}
