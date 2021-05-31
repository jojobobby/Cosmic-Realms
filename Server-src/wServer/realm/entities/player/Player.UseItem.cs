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
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using log4net;
using common;

namespace wServer.realm.entities
{
    partial class Player
    {
        private string[] potionNames = { "Life", "Mana", "Attack", "Defense", "Speed", "Dexterity", "Vitality", "Wisdom", "", "", "Luck", "CritDmg", "CritHit" };
        public bool ResolveStorageSize()
        {
            var acc = Client.Account;
            if (acc.PotionStorageLevel <= 0)
                acc.PotionStorageLevel = 1;
            if (acc.PotionStorageLunarLevel <= 0)
                acc.PotionStorageLunarLevel = 1;
            if (acc.PotionStorageLevel > 10)
                acc.PotionStorageLevel = 10;
            if (acc.PotionStorageLunarLevel > 25)
                acc.PotionStorageLunarLevel = 25;
            acc.PotionStorageSize = acc.PotionStorageLevel * 10;
            acc.PotionStorageLunarSize = acc.PotionStorageLunarLevel;
            acc.FlushAsync();
            return true;
        }
        public bool AddPotion(ActivateEffect eff, bool isLuckPotion = false)
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            if (isLuckPotion)
                idx = 10;
            var acc = Client.Account;
            var potionStoragePotionsList = acc.PotionStoragePotions;
            var amountToAdd = eff.Amount;
            if (idx == 0 || idx == 1)
                amountToAdd /= 5;
            if (amountToAdd > 10)
                amountToAdd = 10;
            ResolveStorageSize();
            if (potionStoragePotionsList[idx] < 0)
                potionStoragePotionsList[idx] = 0;
            if (potionStoragePotionsList[idx] >= acc.PotionStorageSize)
            {
                potionStoragePotionsList[idx] = acc.PotionStorageSize;
                SendInfo("Your storage is full of " + potionNames[idx] + "! This potion broke in your hands...");
                acc.FlushAsync();
                return false;
            }
            else if (potionStoragePotionsList[idx] + amountToAdd > acc.PotionStorageSize)
            {
                potionStoragePotionsList[idx] = acc.PotionStorageSize;
                SendInfo("Added " + (acc.PotionStoragePotions[idx] - acc.PotionStorageSize) + " Potions of " + potionNames[idx] + " to your storage!");
            }
            else if (amountToAdd == 1)
            {
                potionStoragePotionsList[idx] += amountToAdd;
                SendInfo("Added Potion of " + potionNames[idx] + " to your storage!");
            }
            else
            {
                potionStoragePotionsList[idx] += amountToAdd;
                SendInfo("Added " + amountToAdd + " Potions of " + potionNames[idx] + " to your storage!");
            }
            acc.PotionStoragePotions = potionStoragePotionsList;
            acc.FlushAsync();
            return true;
        }
        public bool AddLunarPotion(ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var acc = Client.Account;
            var amountToAdd = eff.Amount;
            if (amountToAdd > 3)
                amountToAdd = 3;
            ResolveStorageSize();
            if (acc.PotionStorageLunar < 0)
                acc.PotionStorageLunar = 0;
            if (acc.PotionStorageLunar >= acc.PotionStorageLunarSize)
            {
                acc.PotionStorageLunar = acc.PotionStorageLunarSize;
                SendInfo("You can't fit any more Celestial Enhancers into your storage, it broke in your hands!");
                acc.FlushAsync();
                return false;
            }
            else if (acc.PotionStorageLunar + amountToAdd > acc.PotionStorageLunarSize)
            {
                acc.PotionStorageLunar = acc.PotionStorageLunarSize;
                SendInfo("Added " + (acc.PotionStoragePotions[idx] - acc.PotionStorageLunarSize) + " Celestial Enhancers to your storage!");
            }
            else if (amountToAdd == 1)
            {
                acc.PotionStorageLunar += amountToAdd;
                SendInfo("Added Celestial Enhancer to your storage!");
            }
            else
            {
                acc.PotionStorageLunar += amountToAdd;
                SendInfo("Added " + amountToAdd + " Celestial Enhancers to your storage!");
            }
            acc.FlushAsync();
            return true;
        }
    }
    partial class Player
    {
        private Random rng = new Random();
        public const int MaxAbilityDist = 14;
        public bool Cancel;
        private int AmountofUses;
        private float Setlocation1;
        private float Setlocation2;
        public static readonly ConditionEffect[] NegativeEffs = new ConditionEffect[]
        {
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Slowed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Paralyzed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Weak,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Stunned,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Confused,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Blind,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Quiet,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.ArmorBroken,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Bleeding,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Dazed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Sick,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Drunk,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Hallucinating,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Hexed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Unstable,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Darkness,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Curse,
                DurationMS = 0
            },

            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Tired,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Sluggish,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect= ConditionEffectIndex.Frozen,
                DurationMS = 0
            }
        };

        private readonly object _useLock = new object();


        public void UseItem(RealmTime time, int objId, int slot, Position pos)
        {
            using (TimedLock.Lock(_useLock))
            {
                var entity = Owner.GetEntity(objId);
                if (entity == null)
                {
                    Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }

                if (entity is Player && objId != Id)
                {
                    Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }

                var container = entity as IContainer;

                // eheh no more clearing BBQ loot bags
                if (this.Dist(entity) > 3)
                {
                    Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }

                var cInv = container?.Inventory.CreateTransaction();

                // get item
                Item item = null;
                foreach (var stack in Stacks.Where(stack => stack.Slot == slot))
                {
                    item = stack.Pull();

                    if (item == null)
                        return;

                    break;
                }
                if (item == null)
                {
                    if (container == null)
                        return;

                    item = cInv[slot];
                }

                if (item == null)
                    return;

                // make sure not trading and trying to cunsume item
                if (tradeTarget != null && item.Consumable)
                    return;

                if (MP < item.MpCost)
                {
                    Client.SendPacket(new InvResult() { Result = 1 });
                    return;
                }


                // use item
                var slotType = 10;
                if (slot < cInv.Length)
                {
                    slotType = container.SlotTypes[slot];

                    if (item.Consumable)
                    {
                        var gameData = Manager.Resources.GameData;
                        var db = Manager.Database;

                        Item successor = null;
                        if (item.SuccessorId != null)
                            successor = gameData.Items[gameData.IdToObjectType[item.SuccessorId]];
                        cInv[slot] = successor;

                        var trans = db.Conn.CreateTransaction();
                        if (container is GiftChest)
                            if (successor != null)
                                db.SwapGift(Client.Account, item.ObjectType, successor.ObjectType, trans);
                            else
                                db.RemoveGift(Client.Account, item.ObjectType, trans);
                        var task = trans.ExecuteAsync();
                        task.ContinueWith(t =>
                        {
                            var success = !t.IsCanceled && t.Result;
                            if (!success || !Inventory.Execute(cInv)) // can result in the loss of an item if inv trans fails...
                            {
                                entity.ForceUpdate(slot);
                                return;
                            }

                            if (slotType > 0)
                            {
                                FameCounter.UseAbility();
                            }
                            else
                            {
                                if (item.ActivateEffects.Any(eff => eff.Effect == ActivateEffects.Heal ||
                                                                    eff.Effect == ActivateEffects.HealNova ||
                                                                    eff.Effect == ActivateEffects.Magic ||
                                                                    eff.Effect == ActivateEffects.MagicNova))
                                {
                                    FameCounter.DrinkPot();
                                }
                            }


                            Activate(time, item, pos);
                        });
                        task.ContinueWith(e =>
                            Log.Error(e.Exception.InnerException.ToString()),
                            TaskContinuationOptions.OnlyOnFaulted);
                        return;
                    }

                    if (slotType > 0)
                    {
                        FameCounter.UseAbility();
                    }
                }
                else
                {
                    FameCounter.DrinkPot();
                }

                   if (item.Consumable || item.SlotType == slotType )
                    Activate(time, item, pos);
                else
                    Client.SendPacket(new InvResult() { Result = 1 });
            }
        }
        private void AEExplode(RealmTime time, ActivateEffect eff)
        {
            var damage = eff.TotalDamage;
            var range = eff.Radius;
            var enemies = new List<Enemy>();
            this.AOE(range, false, enemy => enemies.Add(enemy as Enemy));
            if (enemies.Count() > 0)
                foreach (var enemy in enemies)
                    enemy?.Damage(this, time, damage, true);
            Client?.SendPacket(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position { X = range, Y = 0 }
            }, PacketPriority.Low);
        }
        private void Activate(RealmTime time, Item item, Position target)
        {
            if (freeAbilityUse) { 
                freeAbilityUse = false;
            }
            else { 
                MP -= item.MpCost;
            }


            if (HP < item.HpCost) {
                HP = HP + item.HpCost; 
            }
            else {
                HP -= item.HpCost;
            }

            if (item.SlotType == 54) //added to test bard effect
            {
                Client?.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.INSPIRED_EFFECT_TYPE,
                    TargetObjectId = Id,
                    Color = new ARGB(0x6a0dad), //flash color
                    Pos2 = new Position { X = 1, Y = 1 }, // flash X = period, Y = repeat times
                    Pos1 = new Position { X = 4, Y = 0 }
                }, PacketPriority.Low);
            }

            foreach (var eff in item.ActivateEffects)
            {

                switch (eff.Effect)
                {
                    case ActivateEffects.GenericActivate:
                        AEGenericActivate(time, item, target, eff);
                        break;
                    case ActivateEffects.BulletNova:
                        AEBulletNova(time, item, target, eff);
                        break;
                    case ActivateEffects.Shoot:
                        AEShoot(time, item, target, eff);
                        break;
                    case ActivateEffects.StatBoostSelf:
                        AEStatBoostSelf(time, item, target, eff);
                        break;
                    case ActivateEffects.StatBoostAura:
                        AEStatBoostAura(time, item, target, eff);
                        break;
                    case ActivateEffects.ConditionEffectSelf:
                        AEConditionEffectSelf(time, item, target, eff);
                        break;
                    case ActivateEffects.ConditionEffectAura:
                        AEConditionEffectAura(time, item, target, eff);
                        break;
                    case ActivateEffects.ClearConditionEffectAura:
                        AEClearConditionEffectAura(time, item, target, eff);
                        break;
                    case ActivateEffects.Heal:
                        AEHeal(time, item, target, eff);
                        break;
                    case ActivateEffects.HealNova:
                        AEHealNova(time, item, target, eff);
                        break;
                    case ActivateEffects.Magic:
                        AEMagic(time, item, target, eff);
                        break;
                    case ActivateEffects.MagicNova:
                        AEMagicNova(time, item, target, eff);
                        break;
                    case ActivateEffects.ItemMasteryPoints:
                        ItemMasteryPoints(time, item, target, eff);
                        break;//AETQMscroll
                    case ActivateEffects.TQMscroll:
                        AETQMscroll(time, item, target, eff);
                        break;
                    case ActivateEffects.TQDscroll:
                        AETQDscroll(time, item, target, eff);
                        break;
                    case ActivateEffects.TQscroll:
                        AETQscroll(time, item, target, eff);
                        break;
                    case ActivateEffects.Teleport:
                        AETeleport(time, item, target, eff);
                        break;
                    case ActivateEffects.LHelmJug:
                        AELHelmJug(time, item, target, eff);
                        break;
                    case ActivateEffects.VampireBlast:
                        AEVampireBlast(time, item, target, eff);
                        break;
                    case ActivateEffects.Trap:
                        AETrap(time, item, target, eff);
                        break;
                    case ActivateEffects.RandomAB:
                        AERandomAB(time, item, target, eff);
                        break;
                    case ActivateEffects.StasisBlast:
                        StasisBlast(time, item, target, eff);
                        break;
                    case ActivateEffects.HealBlast:
                        AEHealBlast(time, item, target, eff);
                        break;
                    case ActivateEffects.Decoy:
                        AEDecoy(time, item, target, eff);
                        break;
                    case ActivateEffects.SupportiveTeam:
                        AESupportiveTeam(time, item, target, eff);
                        break;//AEBulletNova2
                    case ActivateEffects.OffensiveTeam:
                        AEOffensiveTeam(time, item, target, eff);
                        break;//AEBulletNova2 AEBurningLightning
                    case ActivateEffects.BurningLightning:
                        AEBurningLightning(time, item, target, eff);
                        break;
                    case ActivateEffects.Shockwave:
                        AEShockwave(time, item, target, eff);
                        break;
                    case ActivateEffects.Lightning:
                        AELightning(time, item, target, eff);
                        break;
                    case ActivateEffects.BulletNova2:
                        AEBulletNova2(time, item, target, eff);
                        break;
                    case ActivateEffects.PoisonGrenade:
                        AEPoisonGrenade(time, item, target, eff);//SupportVoucher
                        break;
                    case ActivateEffects.SupportVoucher:
                        AESupportVoucher(time, item, target, eff);//AEUseBoxLG  AEUseBoxMC
                        break;
                    case ActivateEffects.RemoveNegativeConditions:
                        AERemoveNegativeConditions(time, item, target, eff);
                        break;
                    case ActivateEffects.RemoveNegativeConditionsSelf:
                        AERemoveNegativeConditionSelf(time, item, target, eff);
                        break;
                    case ActivateEffects.FixedStat:
                        AEFixedStat(time, item, target, eff);
                        break;
                    case ActivateEffects.TOMEAB:
                        AETOMEAB(time, item, target, eff);
                        break;
                    case ActivateEffects.IncrementStat:
                        AEIncrementStat(time, item, target, eff);
                        break;
                    case ActivateEffects.IncrementStatMoon:
                        AEIncrementStatMoon(time, item, target, eff);
                        break;
                    case ActivateEffects.MoonPrimed:
                        AEMoonPrimed(time, item, target, eff);
                        break;
                    case ActivateEffects.FFragments:
                        AE5Fragments(time, item, target, eff);
                        break;
                    case ActivateEffects.AEUseBox:
                        AEUseBox(time, item, target, eff);
                        break;
                    case ActivateEffects.AEUseBoxST:
                        AEUseBoxST(time, item, target, eff);
                        break;
                    case ActivateEffects.AEUseBoxPOT:
                        AEUseBoxPOT(time, item, target, eff);//AEUseBoxLG AE5Fragments
                        break;
                    //case ActivateEffects.FiveOrbs:
                    //AEOrb(time, item, target, eff);//AEUseBoxLG
                    //break;
                    case ActivateEffects.AEUseBoxDonor:
                        AEUseBoxDonor(time, item, target, eff);//AEUseBoxLG
                        break;
                    case ActivateEffects.AEMarvBox:
                        AEAEMarvBox(time, item, target, eff);//AEUseBoxLG
                        break;
                    case ActivateEffects.AEUseBoxLG:
                        AEUseBoxLG(time, item, target, eff);//AEUseBoxLG  AEUseBoxMC
                        break;
                    case ActivateEffects.AEUseBoxMC:
                        AEUseBoxMC(time, item, target, eff);//AEUseBoxLG  AEUseBoxMC
                        break;
                    case ActivateEffects.AEUseBoxtiered:
                        AEUseBoxtiered(time, item, target, eff);//AEUseBoxLG  AEUseBoxMC AENoDamageAbility
                        break;
                    case ActivateEffects.AEVoidKey:
                        AEVoidKey(time, item, target, eff);
                        break;
                    case ActivateEffects.Create:
                        AECreate(time, item, target, eff);
                        break;
                    case ActivateEffects.Dye:
                        AEDye(time, item, target, eff);
                        break;
                    case ActivateEffects.IceTomeAB:
                        AEIceTomeAB(time, item, target, eff);
                        break;
                    case ActivateEffects.ShurikenAbility:
                        AEShurikenAbility(time, item, target, eff);
                        break;
                    case ActivateEffects.ShurikenAbilityATK:
                        AEShurikenAbilityATK(time, item, target, eff);
                        break;
                    case ActivateEffects.NoDamageAbility:
                        AENoDamageAbility(time, item, target, eff);
                        break;
                    case ActivateEffects.Fame:
                        AEAddFame(time, item, target, eff);
                        break;
                    case ActivateEffects.MoonFame:
                        AEAddMoonFame(time, item, target, eff);
                        break;
                    case ActivateEffects.Gold:
                        AEAddGold(time, item, target, eff);
                        break;
                    case ActivateEffects.Backpack:
                        AEBackpack(time, item, target, eff);
                        break;
                    case ActivateEffects.Toolbelt:
                        AEToolbelt(time, item, target, eff);
                        break;
                    case ActivateEffects.XPBoost:
                        AEXPBoost(time, item, target, eff);
                        break;
                    case ActivateEffects.LDBoost:
                        AELDBoost(time, item, target, eff);
                        break;
                    case ActivateEffects.LTBoost:
                        AELTBoost(time, item, target, eff);
                        break;
                    case ActivateEffects.UnlockPortal:
                        AEUnlockPortal(time, item, target, eff);
                        break;
                    case ActivateEffects.CreatePet:
                        AECreatePet(time, item, target, eff);
                        break;
                    case ActivateEffects.Pet:
                        AEPet(time, item, target, eff);
                        break;
                    case ActivateEffects.PetWhite:
                        AEPetWhite(time, item, target, eff);
                        break;
                    case ActivateEffects.UnlockEmote:
                        AEUnlockEmote(time, item, eff);
                        break;
                    case ActivateEffects.HealingGrenade:
                        AEHealingGrenade(time, item, target, eff);
                        break;
                    case ActivateEffects.Gift:
                        AEGift(time, item, target, eff);
                        break;
                    case ActivateEffects.Explode:
                        AEExplode(time, eff);
                        break;
                    case ActivateEffects.Lifesteal:
                        AELifesteal(eff);
                        break;
                    case ActivateEffects.UnlockSkin:
                        AEUnlockSkin(time, item, eff);
                        break;
                    case ActivateEffects.CommonMarks:
                        AECommonMarks(time, item, eff);
                        break;
                    case ActivateEffects.RareMarks:
                        AERareMarks(time, item, eff);
                        break;
                    case ActivateEffects.EpicMarks:
                        AEEpicMarks(time, item, eff);
                        break;
                    case ActivateEffects.LegendaryMarks:
                        AELegendaryMarks(time, item, eff);
                        break;
                    case ActivateEffects.UnlockCharSlot:
                        AEUnlockCharSlot(time, item, target, eff);
                        break;
                    case ActivateEffects.UnlockVaultChest:
                        AEUnlockVaultChest(time, item, target, eff);
                        break;
                    case ActivateEffects.VargoBulletNova:
                        AEVargoBulletNova(time, item, target, eff);
                        break;
                    case ActivateEffects.Sigil:
                        AESigil(time, item, target, eff);
                        break;
                    default:
                        Log.WarnFormat("Activate effect {0} not implemented.", eff.Effect);
                        break;
                }
            }
            for (var i = 0; i < 4; i++)
            {
                switch (Inventory[i].ObjectId)
                {
                    case "Drannol's Judgement":
                        HP -= item.MpCost * 2;
                        SendInfo("1");
                        break;
                    case "The Infernus" when item.MpCost > 0:
                        BurstFire(time, item, target);
                        SendInfo("2");
                        break;
                    case "Meteor" when
                  (Inventory[1].ObjectId == "Burning Tome" || Inventory[1].ObjectId == "Scorching Scepter"):
                        DamageGrenade(time, target);
                        SendInfo("3");
                        break;
                }
            }
        }
        private void BurstFire(RealmTime time, Item item, Position target)
        {
            this.AOE(8, false, enemy => BurnEnemy(Owner, enemy as Enemy, 3000));
            BroadcastSync(new ShowEffect
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xFF0000),
                Pos1 = new Position { X = 8 }
            }, p => this.DistSqr(p) < RadiusSqr);
        }
        private void DamageGrenade(RealmTime time, Position target)
        {
            if (MathsUtils.DistSqr(target.X, target.Y, X, Y) > MaxAbilityDist * MaxAbilityDist) return;
            BroadcastSync(new ShowEffect
            {
                EffectType = EffectType.Throw,
                Color = new ARGB(0xFF0000),
                TargetObjectId = Id,
                Pos1 = target
            }, p => this.DistSqr(p) < RadiusSqr);

            var x = new Placeholder(Manager, 1500);
            x.Move(target.X, target.Y);
            Owner.EnterWorld(x);
            Owner.Timers.Add(new WorldTimer(1500, (world, t) => {
                world.BroadcastPacketNearby(new ShowEffect
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xFF0000),
                    TargetObjectId = x.Id,
                    Pos1 = new Position { X = 5 }
                }, x, null, PacketPriority.High);


                world.AOE(target, 3, false,
            enemy => DamageEnemy(world, enemy as Enemy));
            }));
        }
        private void DamageEnemy(World world, Enemy enemy)
        {
            var remainingDmg = (int)StatsManager.GetDefenseDamage(enemy, (Stats[0] + Stats[1]) ^ 4, enemy.ObjectDesc.Defense);
            var perDmg = remainingDmg * 1000 / 1000;

            WorldTimer tmr = null;
            var x = 0;

            Func<World, RealmTime, bool> poisonTick = (w, t) => {
                if (enemy.Owner == null || w == null)
                    return true;

                w.BroadcastPacketConditional(new ShowEffect
                {
                    EffectType = EffectType.Dead,
                    TargetObjectId = enemy.Id,
                    Color = new ARGB(0xFFFFFF)
                }, p => enemy.DistSqr(p) < RadiusSqr);

                if (x % 4 == 0) // make sure to change this if timer delay is changed
                {
                    var thisDmg = perDmg;
                    if (remainingDmg < thisDmg)
                        thisDmg = remainingDmg;

                    enemy.Damage(this, t, thisDmg, true);
                    remainingDmg -= thisDmg;
                    if (remainingDmg <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            };

            tmr = new WorldTimer(250, poisonTick);
            world.Timers.Add(tmr);
        }
        private void AEBurningLightning(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            const double coneRange = Math.PI / 4;
            var mouseAngle = Math.Atan2(target.Y - Y, target.X - X);

            // get starting target
            var startTarget = this.GetNearestEntity(MaxAbilityDist, false, e => e is Enemy &&
                Math.Abs(mouseAngle - Math.Atan2(e.Y - Y, e.X - X)) <= coneRange);

            // no targets? bolt air animation
            if (startTarget == null)
            {
                var noTargets = new Packet[3];
                var angles = new[] { mouseAngle, mouseAngle - coneRange, mouseAngle + coneRange };
                for (var i = 0; i < 3; i++)
                {
                    var x = (int)(MaxAbilityDist * Math.Cos(angles[i])) + X;
                    var y = (int)(MaxAbilityDist * Math.Sin(angles[i])) + Y;
                    noTargets[i] = new ShowEffect
                    {
                        EffectType = EffectType.Trail,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFF4500),
                        Pos1 = new Position
                        {
                            X = x,
                            Y = y
                        },
                        Pos2 = new Position { X = 350 }
                    };
                }
                BroadcastSync(noTargets, p => this.DistSqr(p) < RadiusSqr);
                return;
            }

            var current = startTarget;
            var targets = new Entity[eff.MaxTargets];
            for (var i = 0; i < targets.Length; i++)
            {
                targets[i] = current;
                var next = current.GetNearestEntity(10, false, e => {
                    if (!(e is Enemy) ||
                        e.HasConditionEffect(ConditionEffects.Invincible) ||
                        e.HasConditionEffect(ConditionEffects.Stasis) ||
                        Array.IndexOf(targets, e) != -1)
                        return false;

                    return true;
                });

                if (next == null)
                    break;

                current = next;
            }

            var pkts = new List<Packet>();
            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null)
                    break;

                var prev = i == 0 ? this : targets[i - 1];

                (targets[i] as Enemy).Damage(this, time, eff.TotalDamage, true);

                if (eff.ConditionEffect != null)
                    targets[i].ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = eff.ConditionEffect.Value,
                        DurationMS = (int)(eff.EffectDuration * 1000)
                    });


                pkts.Add(new ShowEffect
                {
                    EffectType = EffectType.Lightning,
                    TargetObjectId = prev.Id,
                    Color = new ARGB(0xFF4500),
                    Pos1 = new Position
                    {
                        X = targets[i].X,
                        Y = targets[i].Y
                    },
                    Pos2 = new Position { X = 350 }
                });
            }
            BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
        }
        private void BurnEnemy(World world, Enemy enemy, int damage)
        {
            var remainingDmg = (int)StatsManager.GetDefenseDamage(enemy, damage, enemy.ObjectDesc.Defense);
            var perDmg = remainingDmg * 1000 / 7000;

            WorldTimer tmr = null;
            var x = 0;

            Func<World, RealmTime, bool> burnTick = (w, t) => {
                if (enemy.Owner == null || w == null)
                    return true;

                w.BroadcastPacketConditional(new ShowEffect
                {
                    EffectType = EffectType.Dead,
                    TargetObjectId = enemy.Id,
                    Color = new ARGB(0xbd460a)
                }, p => enemy.DistSqr(p) < RadiusSqr);

                if (x % 4 == 0) // make sure to change this if timer delay is changed
                {
                    var thisDmg = perDmg;
                    if (remainingDmg < thisDmg)
                        thisDmg = remainingDmg;

                    enemy.Damage(this, t, thisDmg, true);
                    remainingDmg -= thisDmg;
                    if (remainingDmg <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            };

            tmr = new WorldTimer(250, burnTick);
            world.Timers.Add(tmr);
        }
        private void AEUnlockCharSlot(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            // i bet its bugged in some way, i wouldn't use it


            var acc = Client.Account;
            if (Owner is Vault)
            {
                SendInfo("You unlocked new Character Slot!");
                acc.MaxCharSlot++;
                acc.FlushAsync();
            }
            else
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                if (availableSlot != -1)
                {
                    Inventory[availableSlot] = item;
                }
                SendInfo("You can only use this item in the vault.");
            }
        }
        private void AEUnlockVaultChest(RealmTime time, Item item, Position target, ActivateEffect eff)
        {

            // i bet its bugged in some way, i wouldn't use it

            var acc = Client.Account;
            if (Owner is Vault)
            {
                SendInfo("You unlocked new Vault chest!");
                acc.VaultCount++;
                acc.FlushAsync();
            }
            else
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                if (availableSlot != -1)
                {
                    Inventory[availableSlot] = item;
                }
                SendInfo("You can only use this item in the vault.");
            }
        }
        private void AELifesteal(ActivateEffect eff)
        {
            var duration = eff.DurationMS;
            Lifesteal = true;
            Owner?.Timers.Add(new WorldTimer(duration, (world, t) =>
            {
                Lifesteal = false;
                return true;
            }));
        }
        private void AEUnlockEmote(RealmTime time, Item item, ActivateEffect eff)
        {
            if (Client.Player.Owner == null || Client.Player.Owner is Test)
            {
                SendInfo("Can't use emote unlocks in test worlds.");
                return;
            }

            var emotes = Client.Account.Emotes;
            if (!emotes.Contains(eff.Id))
                emotes.Add(eff.Id);
            Client.Account.Emotes = emotes;
            Client.Account.FlushAsync();
            SendInfo($"{eff.Id} Emote unlocked successfully");
        }
        private void AEGift(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            ushort gift = eff.Gifts[rng.Next(eff.Gifts.Length)];

            if (Stars < 5)
            {
                SendError("You must have at least 5 stars before you can use crates.");
                return;
            }


            for (int i = 4; i < Inventory.Length; i++)
                if (Inventory[i] == null)
                {
                    Inventory[i] = Manager.Resources.GameData.Items[gift];
                    SaveToCharacter();
                    return;
                }
        }
        private void AEHealBlast(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var amount = eff.Amount;
            var range = eff.Range;

            var pkts = new List<Packet>
            {
                new ShowEffect()
                {
                    EffectType = EffectType.Diffuse,
                    Color = new ARGB(0xe39393),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = target.X + eff.Range, Y = target.Y }
                }
            };
            var enemies = new List<Enemy>();
            var players = new List<Player>();
            Owner.AOE(target, range, true, player =>
            {
                if (player.HasConditionEffect(ConditionEffects.Sick))
                {
                    pkts.Add(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xff00ff00),
                        Message = "Cannot Be Healed"
                    });
                }
                else if (!player.HasConditionEffect(ConditionEffects.Sick))
                {
                    players.Add(player as Player);
                    ActivateHealHp(player as Player, amount, pkts);
                }
            });
            BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
        }
        private void AELegendaryMarks(RealmTime time, Item item, ActivateEffect eff)
        {
            var acc = Client.Account;
            acc.LegendaryMarks += 1;
            acc.FlushAsync();

            if (acc.LegendaryMarks >= 4)
            {
                acc.LegendaryMarks = 0;
                Manager.Database.AddGift(acc, 0x3037);
                acc.FlushAsync();

                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffff00),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffff00),
                    Message = "Legendary Marks Quest Completed"
                }, PacketPriority.Low);

                SendInfo("+1 Legendary Mark [4/4], Check your gift chest for a Grand Champion Chest");

            }
            else
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffff00),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffff00),
                    Message = "[" + acc.LegendaryMarks + "/4] Legendary Marks"
                }, PacketPriority.Low);

                SendInfo("+1 Legendary Mark [" + acc.LegendaryMarks + "/4] Collected Until Reward");
            }

        }

        private void AEEpicMarks(RealmTime time, Item item, ActivateEffect eff)
        {
            var acc = Client.Account;
            acc.EpicMarks += 1;
            acc.FlushAsync();

            if (acc.EpicMarks >= 4)
            {
                acc.EpicMarks = 0;
                Manager.Database.AddGift(acc, 0x3035);
                acc.FlushAsync();

                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0x8000ff),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0x8000ff),
                    Message = "Epic Marks Quest Completed"
                }, PacketPriority.Low);

                SendInfo("+1 Epic Mark [4/4], Check your gift chest for a Grand Master Chest");

            }
            else
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0x8000ff),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0x8000ff),
                    Message = "[" + acc.EpicMarks + "/4] Epic Marks"
                }, PacketPriority.Low);

                SendInfo("+1 Epic Mark [" + acc.EpicMarks + "/4] Collected Until Reward");
            }

        }


        private void AERareMarks(RealmTime time, Item item, ActivateEffect eff)
        {
            var acc = Client.Account;
            acc.RareMarks += 1;
            acc.FlushAsync();

            if (acc.RareMarks >= 4)
            {
                acc.RareMarks = 0;
                Manager.Database.AddGift(acc, 0xd8e);
                acc.FlushAsync();

                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0x4c4cff),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0x4c4cff),
                    Message = "Rare Marks Quest Completed"
                }, PacketPriority.Low);

                SendInfo("+1 Rare Mark [4/4], Check your gift chest for a Mighty Chest");

            }
            else
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0x4c4cff),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0x4c4cff),
                    Message = "[" + acc.RareMarks + "/4] Rare Marks"
                }, PacketPriority.Low);

                SendInfo("+1 Rare Mark [" + acc.RareMarks + "/4] Collected Until Reward");
            }

        }

        private void AECommonMarks(RealmTime time, Item item, ActivateEffect eff)
        {
            var acc = Client.Account;
            acc.CommonMarks += 1;
            acc.FlushAsync();

            if (acc.CommonMarks >= 6)
            {
                acc.CommonMarks = 0;
                Manager.Database.AddGift(acc, 0xd8c);
                acc.FlushAsync();

                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xFFFFFF),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFFFFFF),
                    Message = "Common Marks Quest Completed"
                }, PacketPriority.Low);

                SendInfo("+1 Common Mark [6/6], Check your gift chest for a Standard Chest");

            }
            else
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xFFFFFF),
                    Pos1 = new Position() { X = 3 }
                }, PacketPriority.Low);

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFFFFFF),
                    Message = "[" + acc.CommonMarks + "/6] Common Marks"
                }, PacketPriority.Low);

                SendInfo("+1 Common Mark [" + acc.CommonMarks + "/6] Collected Until Reward");
            }

        }
        
            private void AEUnlockSkin(RealmTime time, Item item, ActivateEffect eff)
            {
                var acc = Client.Account;
                if (Client.Player.Owner == null || Client.Player.Owner is Test)
                {
                    SendInfo("Can't use skins in test worlds.");
                    return;
                }

                var skins = Client.Account.Skins.ToList();
                if (!skins.Contains(eff.SkinType))
                {
                    Client.Manager.Database.PurchaseSkin(Client.Account, eff.SkinType, 0);
                    Client.SendPacket(new ReskinUnlock
                    {
                        SkinId = eff.SkinType
                    });
                    SendInfo($"New skin unlocked successfully. Change skins in your Vault, or start a new character to use.");
                    return;
                }
                else
                {
                    acc.Fame += 150;
                    acc.FlushAsync();
                    SendInfo("You already own this skin. [+150 fame]");
                    return;
                }
            }
            
           

        private void AECreatePet(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!Manager.Config.serverSettings.enablePets)
            {
                SendError("Cannot create pet. Pets are currently disabled.");
                return;
            }

            var petYard = Owner as PetYard;
            if (petYard == null)
            {
                SendError("server.use_in_petyard");
                return;
            }

            var pet = Pet.Create(Manager, this, item);
            if (pet == null)
                return;

            var sPos = petYard.GetPetSpawnPosition();
            pet.Move(sPos.X, sPos.Y);
            Owner.EnterWorld(pet);

            Client.SendPacket(new HatchPetMessage()
            {
                PetName = pet.Skin,
                PetSkin = pet.SkinId
            });
        }//VoidDragonEgg
        private void ItemMasteryPoints(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var acc = Manager.Database.GetAccount(AccountId);
            acc.ItemMasteryPoints += eff.Amount;
            acc.FlushAsync();
            SendInfo($"You have gained { eff.Amount } Mastery Points. A total of [{ acc.ItemMasteryPoints }] Mastery Points");
        }

        private void AEPetWhite(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var entity = Resolve(Manager, eff.ObjectId);
            if (entity == null)
                return;
            entity.Move(target.X, target.Y);
            entity.SetPlayerOwner(this);
            Owner.EnterWorld(entity);
        }
        private void AEPet(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var entity = Resolve(Manager, eff.ObjectId);
            if (entity == null)
                return;
            entity.Move(X, Y);
            entity.SetPlayerOwner(this);
            Owner.EnterWorld(entity);
        }

        private void AEUnlockPortal(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var gameData = Manager.Resources.GameData;

            // find locked portal
            var portals = Owner.StaticObjects.Values
                .Where(s => s is Portal && s.ObjectDesc.ObjectId.Equals(eff.LockedName) && s.DistSqr(this) <= 9)
                .Select(s => s as Portal);
            if (!portals.Any())
                return;
            var portal = portals.Aggregate(
                (curmin, x) => (curmin == null || x.DistSqr(this) < curmin.DistSqr(this) ? x : curmin));
            if (portal == null)
                return;

            // get proto of world
            ProtoWorld proto;
            if (!Manager.Resources.Worlds.Data.TryGetValue(eff.DungeonName, out proto))
            {
                Log.Error("Unable to unlock portal. \"" + eff.DungeonName + "\" does not exist.");
                return;
            }

            if (proto.portals == null || proto.portals.Length < 1)
            {
                Log.Error("World is not associated with any portals.");
                return;
            }

            // create portal of unlocked world
            var portalType = (ushort)proto.portals[0];
            var uPortal = Resolve(Manager, portalType) as Portal;
            if (uPortal == null)
            {
                Log.ErrorFormat("Error creating portal: {0}", portalType);
                return;
            }

            var portalDesc = gameData.Portals[portal.ObjectType];
            var uPortalDesc = gameData.Portals[portalType];

            // create world
            World world;
            if (proto.id < 0)
                world = Manager.GetWorld(proto.id);
            else
            {
                DynamicWorld.TryGetWorld(proto, Client, out world);
                world = Manager.AddWorld(world ?? new World(proto));
            }
            uPortal.WorldInstance = world;

            // swap portals
            if (!portalDesc.NexusPortal || !Manager.Monitor.RemovePortal(portal))
                Owner.LeaveWorld(portal);
            uPortal.Move(portal.X, portal.Y);
            uPortal.Name = uPortalDesc.DisplayId;
            var uPortalPos = new Position() { X = portal.X - .5f, Y = portal.Y - .5f };
            if (!uPortalDesc.NexusPortal || !Manager.Monitor.AddPortal(world.Id, uPortal, uPortalPos))
                Owner.EnterWorld(uPortal);

            // setup timeout
            if (!uPortalDesc.NexusPortal)
            {
                var timeoutTime = gameData.Portals[portalType].Timeout;
                Owner.Timers.Add(new WorldTimer(timeoutTime * 1000, (w, t) => w.LeaveWorld(uPortal)));
            }

            // announce
            Owner.BroadcastPacket(new Notification
            {
                Color = new ARGB(0xFF00FF00),
                ObjectId = Id,
                Message = "Unlocked by " + Name
            }, null, PacketPriority.Low);
            foreach (var player in Owner.Players.Values)
                player.SendInfo(string.Format("{{\"key\":\"{{server.dungeon_unlocked_by}}\",\"tokens\":{{\"dungeon\":\"{0}\",\"name\":\"{1}\"}}}}", world.SBName, Name));
        }

        private void AELTBoost(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (LTBoostTime < 0 || (LTBoostTime > eff.DurationMS && eff.DurationMS >= 0))
                return;

            LTBoostTime += eff.DurationMS;
            InvokeStatChange(StatsType.LTBoostTime, LTBoostTime / 1000, true);
        }

        private void AELDBoost(RealmTime time, Item item, Position target, ActivateEffect eff)
        {

            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var statInfo = Manager.Resources.GameData.Classes[ObjectType].Stats;
            var acc = Client.Account;
            var chr = _client.Character;

            Stats.Base[10] += eff.Amount;
            if (Stats.Base[10] > statInfo[10].MaxValue)
            {
                Stats.Base[10] = statInfo[10].MaxValue;
                AddPotion(eff, true);
            }


            if (LDBoostTime < 0)
                return;

            LDBoostTime += eff.DurationMS;
            InvokeStatChange(StatsType.LDBoostTime, LDBoostTime / 1000, true);
        }

        private void AEXPBoost(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (XPBoostTime < 0)
                return;

            XPBoostTime += eff.DurationMS;
            XPBoosted = true;
            InvokeStatChange(StatsType.XPBoostTime, XPBoostTime / 1000, true);
        }
        private void AEToolbelt(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var chr = _client.Character;
            var gameData = Manager.Resources.GameData;
            if (chr.Toolbelts >= eff.Amount)
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                Inventory[availableSlot] = item;
                SendInfo("Toolbelt used is less or equal to the one equipped. " + chr.Toolbelts);
                return;
            }
            else
            {
                chr.Toolbelts = eff.Amount;
                chr.Regeneration = eff.DurationMS;

            }
        }
        private void AEBackpack(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            HasBackpack = true;
        }

        private void AEAddFame(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (Owner is Test || Client.Account == null)
                return;

            var acc = Client.Account;
            acc.Fame += eff.Amount;
            acc.FlushAsync();
            SendInfo("+" + eff.Amount + " Fame.");
            return;

        }

        private void AEAddMoonFame(RealmTime time, Item item, Position target, ActivateEffect eff)
        {

            var gameData = Manager.Resources.GameData;


            if (Owner is Test || Client.Account == null)
                return;


            var pick = Random.Next(eff.FameMin, eff.FameMax);

            var acc = Client.Account;
            acc.Fame += pick;
            acc.FlushAsync();
            SendInfo("You've found [+" + pick + "] Alien Fame in this rock.");
            return;

        }

        private void AEAddGold(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (Owner is Test || Client.Account == null)
                return;

            var acc = Client.Account;
            acc.Credits += eff.Amount;
            acc.FlushAsync();
            SendInfo("+" + eff.Amount + " Credits.");
            return;

        }
        //NoDamage
        private void AEShurikenAbility(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.NinjaSpeedy))
            {
                ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.NinjaSpeedy, 0);
        }

        private void AENoDamageAbility(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.NoDamage))
            {
                ApplyConditionEffect(ConditionEffectIndex.NoDamage);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.NoDamage, 0);
        }

        private void AEShurikenAbilityATK(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.Strength))
            {
                ApplyConditionEffect(ConditionEffectIndex.Strength);
                return;
            }

            if (MP >= item.MpEndCost)
            {
                MP -= item.MpEndCost;
                AEShoot(time, item, target, eff);
            }

            ApplyConditionEffect(ConditionEffectIndex.Strength, 0);
        }

        private void AEDye(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (item.Texture1 != 0)
                Texture1 = item.Texture1;
            if (item.Texture2 != 0)
                Texture2 = item.Texture2;
        }
        private void AEVoidKey(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var gameData = Manager.Resources.GameData;
            var isChest = true;

            ushort objType;
            if (!gameData.IdToObjectType.TryGetValue(eff.Id, out objType))
                return; // object not found, ignore
            if (!gameData.Portals.ContainsKey(objType))
                isChest = true;

            var entity = Resolve(Manager, objType);
            var timeoutTime = 0;
            if (!isChest)
                timeoutTime = gameData.Portals[objType].Timeout;

            Owner.BroadcastPacket(new Notification
            {
                Color = new ARGB(0xFF00FF00),
                ObjectId = Id,
                Message = "Void unlocked by " + Name
            }, null, PacketPriority.Low);

            entity.Move(X, Y);
            Owner.EnterWorld(entity);

        }
        private void AECreate(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var gameData = Manager.Resources.GameData;
            var isChest = false;

            ushort objType;
            if (!gameData.IdToObjectType.TryGetValue(eff.Id, out objType))
                return; // object not found, ignore
            if (!gameData.Portals.ContainsKey(objType))
                isChest = true;
            if ((Client.Player.Owner is DeathArena))
            {
                SendInfo("Cannot be used in Oryx's Arena.");
                return;
            }
            if (isChest && !(Client.Player.Owner is Vault))
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                Inventory[availableSlot] = item;
                SendInfo("This item can be opened only in vault.");
                return;
            }

            var entity = Resolve(Manager, objType);
            var timeoutTime = 0;
            if (!isChest)
                timeoutTime = gameData.Portals[objType].Timeout;

            entity.Move(X, Y);
            Owner.EnterWorld(entity);

            if (!isChest)
            {
                (entity as Portal).PlayerOpened = true;
                (entity as Portal).Opener = Name;

                Owner.Timers.Add(new WorldTimer(timeoutTime * 1000, (world, t) => world.LeaveWorld(entity)));

                Owner.BroadcastPacket(new Notification
                {
                    Color = new ARGB(0xFF00FF00),
                    ObjectId = Id,
                    Message = "Opened by " + Name
                }, null, PacketPriority.Low);
                foreach (var player in Owner.Players.Values)
                    player.SendInfo("{\"key\":\"{server.dungeon_opened_by}\",\"tokens\":{\"dungeon\":\"" + gameData.Portals[objType].DungeonName + "\",\"name\":\"" + Name + "\"}}");
            }
        }
        private void AESigil(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var gameData = Manager.Resources.GameData;
            ushort killerObjType;
            ushort fullAltarType;
            if (!gameData.IdToObjectType.TryGetValue(eff.ObeliskKiller, out killerObjType))
                return;
            if (!gameData.IdToObjectType.TryGetValue(eff.ActivatedObelisk, out fullAltarType))
                return;
            if (!(Client.Player.Owner is Nexus))
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                Inventory[availableSlot] = item;
                SendInfo("This item can be only used on Nexus.");
                return;
            }
            var killer = Resolve(Manager, killerObjType);
            foreach (var enemy in Owner.Enemies.Values.Where(e =>
                        e.ObjectDesc != null
                        && e.ObjectDesc.ObjectId != null
                        && e.ObjectDesc.Enemy
                        && e.ObjectDesc.ObjectId.ContainsIgnoreCase("Obelisk")))
            {
                if (enemy.ObjectType == fullAltarType)
                {
                    var availableSlot = Inventory.GetAvailableInventorySlot(item);
                    Inventory[availableSlot] = item;
                    SendInfo("This obelisk is already active...");
                    return;
                }
            }
            switch (item.DisplayName)
            {
                case "Sigil of the Void":
                    killer.Manager.Chat.RaidNotifier("Sigil of the Void", "Sigil of the Void", 9399039);
                    break;
                case "Sigil of the Soul":
                    killer.Manager.Chat.RaidNotifier("Sigil of the Soul", "Sigil of the Soul", 10263736);
                    break;
                case "Sigil of the Mind":
                    killer.Manager.Chat.RaidNotifier("Sigil of the Mind", "Sigil of the Mind", 16752448);
                    break;
                case "Sigil of the Body":
                    killer.Manager.Chat.RaidNotifier("Sigil of the Body", "Sigil of the Body", 6983679);
                    break;
            }
            killer.Move(X, Y);
            Owner.EnterWorld(killer);
        }
        private void AEUseBox(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>

                //Item Rare

                {"ST Chest",
                "Earth Scroll 6",
                "500 Tradeable Credits",
                "Carnelian",
                "Power Battery",
                "Sprite Essence",
                "Blue power crystal",
                "Cyberious's Plate",
                "Oryx's Dagger of Foul Malevolence",
                "Oryx's Bow of Covert Havens",
                "Oryx's Staff of the Cosmic Whole",
                "Oryx's Sword of Acclaim",
                "Oryx's Wand of Recompense",
                "Oryx's Masamune",
                "Frozen Ice Shard",
                "Alien U.F.O Plate",
                "Scrapped Ship Plates",
                "Topaz",
                "Amethyst",
                "Jade",
                "Spell Scroll",
                "Super Magic Mushroom",
                "Cultist Chest Item",
                "Forgotten King Chest Item",
                "Void Entity Chest Item",
                "Marble Colossus Chest Item",
                "Hide of the Enraged",
                "Armor of the Enraged",
                "Robe of the Enraged",
                "Advanced Alien Tech",
                "Basic Alien Tech",
                "Destructor AI Module",
                "Engine Cooling Module",
                "Energy Converter Module",
                "Atomic Battery Module",
                "Cyberious Infused Shard",
                "Scraps of the Descendant",
                "Mighty Chest",
                "Ectoplasm",
                "Red Ichor",


                //Item common


                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                "Mighty Chest",
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've been rewarded with a: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }//SupportVoucher

        private void AESupportVoucher(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var acc = Client.Account;
            var chr = _client.Character;
            var x = Client.Account.Rank;
            var db = Manager.Database;
            var y = 0;
            if (acc.Rank < 40) {
                acc.LegacyRank += eff.Amount;
                acc.FlushAsync();
                if (acc.Rank > 40)
                {
                    acc.LegacyRank = 40;
                    acc.FlushAsync();
                }
                for (int i = 5; i <= eff.Amount;)
                {
                    db.AddGift(acc, 0x4017);
                    db.AddGift(acc, 0x6172);
                    i += 5;
                    y++;

                }
                SendInfo(y + " Support Boxes | Credits Recieved. Check gift chests for rewards.");
                SendInfo("Successfully Redeemed $" + eff.Amount + " rank");
                acc.DonorClaim = true;
                acc.FlushAsync();
            }
            else
            {
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                Inventory[availableSlot] = item;
                SendInfo("You already own the maximum donor rank, cannot exceed rank amount.");
                return;
            }
        }
        private void AEUseBoxST(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>
                {"Harmonious Harp",
                "Angel's Fanfare",
                "Wings of Sanctity",
                "Heavenly Halo",
                "Pernicious Peridot",
                "Acidic Armor",
                "Virulent Venom",
                "Dagger of Toxin",
                "Kiritsukeru",
                "Watarimono",
                "Reinforced Root Hide",
                "Traveler's Trinket",
                "Swashbuckler's Sickle",
                "Tricorne of the High Seas",
                "Naval Uniform",
                "First Mate's Hook",
                "Bergenia Bow",
                "Hollyhock Hide",
                "Plant Harvester Trap",
                "Chrysanthemum Corsage",
                "Theurgy Wand",
                "Ceremonial Merlot",
                "Anointed Robe",
                "Ring of Pagan Favor",
                "Ring of the Inferno",
                "Molten Mantle",
                "Scorchium Stone",
                "Staff of Eruption",
                "Radiant Heart",
                "Luminous Armor",
                "Crystalline Kunai",
                "Quartz Cutter",
                "Null-Magic Ring",
                "Magic-Resistance Robe",
                "Kamishimo",
                "Kazekiri",
                "Garden's Leaf Armor",
                "Crown of the Garden",
                "Hallow Grass Shield",
                "Leaf Greatsword",
                "Symbiotic Ripper",
                "Parasitic Concoction",
                "Rags of the Host",
                "Hivemind Circlet",
                "Abomination's Wrath",
                "Grotesque Scepter",
                "Garment of the Beast",
                "Horrific Claws",
                "KnightST2",
                "KnightST1",
                "KnightST3",
                "KnightST5",
                "KnightST0",
                "TricksterST0",
                "TricksterST1",
                "TricksterST2",
                "TricksterST3",
                "Ring of the Guardian",
                "Heavy Plated Robe",
                "Skull of Solid Defense",
                "Ancient Summoner Staff",
                "Edictum Praetoris",
                "Memento Mori",
                "Toga Picta",
                "Interregnum",
                "Supernatural Staff",
                "Spectral Spell",
                "Immortal Mantle",
                "Phantom Pendant",
                "Marble Protection Ring",
                "Lost Passion Armor",
                "Sharp Hall's Claymore",
                "Harden Helmet of Marble",
                "Bow of Reanimation",
                "Quiver of Reanimation",
                "Amulet of Reanimation",
                "Hide of Reanimation"
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've been rewarded with a: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }
        private void AEUseBoxtiered(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>
                {"Staff of the Cosmic Whole",
                "Staff of the Vital Unity",
                "Staff of the Fundamental Core",
                "Staff of Astral Knowledge",
                "Staff of Diabolic Secrets",
                "Archon Sword",
                "Skysplitter Sword",
                "Sword of Acclaim",
                "Sword of Splendor",
                "Sword of Majesty",
                "Ichimonji",
                "Muramasa",
                "Masamune",
                "Wand of Shadow",
                "Wand of Ancient Warning",
                "Wand of Recompense",
                "Wand of Evocation",
                "Wand of Retribution",
                "Emeraldshard Dagger",
                "Agateclaw Dagger",
                "Dagger of Foul Malevolence",
                "Dagger of Sinister Deeds",
                "Dagger of Dire Hatred",
                "Bow of Fey Magic",
                "Bow of Innocent Blood",
                "Bow of Covert Havens",
                "Bow of Mystical Energy",
                "Bow of Deep Enchantment",
                "Skull-splitter Sword",
                "Bow of Eternal Frost",
                "Frostbite",
                "Present Dispensing Wand",
                "An Icicle",
                "Staff of Yuletide Carols",
                "Dagger of the Terrible Talon",
                "Wand of Ancient Terror",
                "Bow of Nightmares",
                "Corrupted Cleaver",
                "Staff of Horrific Knowledge",
                "Staff of the summer solstice",
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've been rewarded with tiered item: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }


        private void AEUseBoxPOT(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>
                {"Potion of Dexterity",
                "Potion of Vitality",
                "Potion of Life",
                "Potion of Mana",
                "Potion of Speed",
                "Potion of Wisdom",
                "Potion of Attack",
                "Potion of Defense",
                "Potion of Critical Chance",
                "Potion of Critical Damage",
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've got a: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }
        private void AEUseBoxMC(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>
                {"Char Slot Unlocker",
                "Vault Chest Unlocker", 
                "Effusion of Dexterity",
                "Effusion of Life",
                "Effusion of Mana",
                "Effusion of Defense",
                "Oryx Stout",
                "Realm-wheat Hefeweizen",
                "Epic Sprite World Key",
                "Haunted Omen's Key",
                "Crystal Cave Key",
                "Spectre's Lair Key",
                "Draconis Key",
                "Undead Lair Key",
                "Pirate Cave Key",
                "Spider Den Key",
                     "Undead Lair Key",
                "Pirate Cave Key",
                "Spider Den Key",
                     "Undead Lair Key",
                "Pirate Cave Key",
                "Spider Den Key",
                     "Undead Lair Key",
                "Pirate Cave Key",
                "Spider Den Key",
                "Abyss of Demons Key",
                "Snake Pit Key",
                 "Abyss of Demons Key",
                "Snake Pit Key",
                 "Abyss of Demons Key",
                "Snake Pit Key",
                 "Abyss of Demons Key",
                "Snake Pit Key",
                "Tomb of the Ancients Key",
                "Sprite World Key",
                "Wine Cellar Incantation",
                "Ocean Trench Key",
                "Totem Key",
                "Ice Cave Key",
                "Manor Key",
                "Davy's Key",
                "Lab Key",
                "Candy Key",
                "Cemetery Key",
                "Forest Maze Key",
                "Woodland Labyrinth Key",
                "Deadwater Docks Key",
                "The Crawling Depths Key",
                "Shatters Key",
                "Shaitan's Key",
                "Theatre Key",
                "Puppet Master's Encore Key",
                "Toxic Sewers Key",
                "The Hive Key",
                "Wine Cellar Key",
                "Ice Tomb Key",
                "Mountain Temple Key",
                "Rainbow Road Key",
                "Treasure Map",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                "Special Crate",
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've got a: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }

        private void AEUseBoxDonor(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {
                var acc = Client.Account;
                var gameData = Manager.Resources.GameData;
                var LootBoost = new List<string>
                {
                    "Backpack",
                    "Lucky Clover",
                    "Enchanted Loot Drop Potion",
                    "Large Toolbelt",
                    "Golden Lucky Clover",
                    "Apple",
                    "Lunar Ascension",
                    "Dungeon Scroll 3"
                };
                var Crates = new List<string>
                {
                    "Cultist Chest Item",
                    "Forgotten King Chest Item",
                    "Void Entity Chest Item",
                    "Marble Colossus Chest Item"
                };
                var Crates3 = new List<string>
                {
                "ST Chest",
                "500 Tradeable Credits",
                "Carnelian",
                "Power Battery",
                "Sprite Essence",
                "Blue power crystal",
                "Cyberious's Plate",
                "Oryx's Dagger of Foul Malevolence",
                "Oryx's Bow of Covert Havens",
                "Oryx's Staff of the Cosmic Whole",
                "Oryx's Sword of Acclaim",
                "Oryx's Wand of Recompense",
                "Oryx's Masamune",
                "Frozen Ice Shard",
                "Alien U.F.O Plate",
                "Scrapped Ship Plates",
                "Topaz",
                "Amethyst",
                "Jade",
                "Spell Scroll",
                "Super Magic Mushroom",
                "Hide of the Enraged",
                "Armor of the Enraged",
                "Robe of the Enraged",
                "Advanced Alien Tech",
                "Basic Alien Tech",
                "Destructor AI Module",
                "Engine Cooling Module",
                "Energy Converter Module",
                "Atomic Battery Module",
                "Cyberious Infused Shard",
                "Scraps of the Descendant",
                "Mighty Chest",
                "Ectoplasm",
                "Red Ichor",
                "Mighty Chest",
                "Mighty Chest"
                };
                var LootBoostPick = Random.Next(LootBoost.Count);
                var CratesPick = Random.Next(Crates.Count);
                var Crates3Pick = Random.Next(Crates3.Count);
                
                var etem3 = gameData.IdToObjectType[Crates3[Crates3Pick]];
                var etem2 = gameData.IdToObjectType[Crates[CratesPick]];
                var etem = gameData.IdToObjectType[LootBoost[LootBoostPick]];

                SendInfo("You've redeemed: [" + LootBoost[LootBoostPick] + "], [" + Crates3[Crates3Pick] + "], [" + Crates[CratesPick] + "]");
                SendInfo("These items have been sent to your gift chest.");

                Manager.Database.AddGift(acc, etem3);
                Manager.Database.AddGift(acc, etem2);
                Manager.Database.AddGift(acc, etem);

            }
        }
        private void AEUseBoxLG(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {



                var gameData = Manager.Resources.GameData;

                var list = new List<string>
                {
                    "Backpack",
                    "Loot Drop Potion",
                    "XP Booster",
                    "Backpack",
                    "Loot Drop Potion",
                    "XP Booster",
                    "Backpack",
                    "Loot Drop Potion",
                    "XP Booster",
                    "Char Slot Unlocker",
                    "Vault Chest Unlocker",
                    "Lucky Clover",
                    "Tiny Toolbelt",
                    "XP Booster",
                    "Dungeon Scroll 3"
                };


                var pick = Random.Next(list.Count);
                SendInfo("You've got Special Item Item: [" + list[pick] + "]");


                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                var etem = gameData.Items[gameData.IdToObjectType[list[pick]]];
                Inventory[availableSlot] = etem;


            }
        }
        private void AEAEMarvBox(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            {
                var acc = Client.Account;
                var gameData = Manager.Resources.GameData;
                var Item = new List<string>
                {
                    "Vitamine Buster",
                    "Helm of the Swift Bunny",
                    "Dagger of the Hasteful Rabbit",
                    "Sword of the Rainbow's End",
                    "Clover Bow",
                    "Clover Bow",
                    "Clover Bow",
                    "Apple",
                    "Lunar Ascension"
                };
           
                var ItemPick = Random.Next(Item.Count);
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                var etem = gameData.Items[gameData.IdToObjectType[Item[ItemPick]]];
                Inventory[availableSlot] = etem;
                SendInfo("You've unboxed a: [" + Item[ItemPick] + "]");



            }
        }
       
        private void AEIncrementStat(RealmTime time, Item item, Position target, ActivateEffect eff) // here
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var statInfo = Manager.Resources.GameData.Classes[ObjectType].Stats; 
            for(int i = 0; i <= 12; i++)
            {
                if (i == idx)
                {
                    if (Stats.Base[i] >= statInfo[i].MaxValue)
                    {
                        AddPotion(eff);
                    }
                    else if (i == idx)
                        if (Stats.Base[idx] + eff.Amount > statInfo[idx].MaxValue)
                            Stats.Base[idx] = statInfo[idx].MaxValue;
                        else
                            Stats.Base[idx] += eff.Amount;
                }
            }


        }
        private void AEIncrementStatMoon(RealmTime time, Item item, Position target, ActivateEffect eff) // here
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var acc = _client.Character;//
            var Random = new Random().Next(0, 10);
            var chr = Client.Account;


            if (acc.MoonPrimed == true)
            {
                if (Random == 2 && acc.AttackStatsMoon <= 9)
                {
                    Stats.Base[2] += eff.Amount;
                    acc.AttackStatsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Attack. Total [{ acc.AttackStatsMoon}] / 10");
                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0x800080),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x800080),
                            Message = "{\"key\": \"+1 Attack\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x800080),
                            Message = "{\"key\": \"+1 Attack\"}"
                        }, PacketPriority.Low);

                }
                if (Random == 3 && acc.DefensePotsMoon <= 9)
                {
                    Stats.Base[3] += eff.Amount;
                    acc.DefensePotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Defense. Total [{ acc.DefensePotsMoon}] / 10");
                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0x000000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x000000),
                            Message = "{\"key\": \"+1 Defense\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x000000),
                            Message = "{\"key\": \"+1 Defense\"}"
                        }, PacketPriority.Low);
                }
                if (Random == 4 && acc.SpeedPotsMoon <= 9)
                {
                    Stats.Base[4] += eff.Amount;
                    acc.SpeedPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Speed. Total [{ acc.SpeedPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0x00FF00),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x00FF00),
                            Message = "{\"key\": \"+1 Speed\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x00FF00),
                            Message = "{\"key\": \"+1 Speed\"}"
                        }, PacketPriority.Low);
                }
                if (Random == 6 && acc.VitalityPotsMoon <= 9)
                {
                    Stats.Base[6] += eff.Amount;
                    acc.VitalityPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Vitality. Total [{ acc.VitalityPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFF0000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFF0000),
                            Message = "{\"key\": \"+1 Vitality\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFF0000),
                            Message = "{\"key\": \"+1 Vitality\"}"
                        }, PacketPriority.Low);
                }
                if (Random == 7 && acc.WisdomPotsMoon <= 9)
                {
                    Stats.Base[7] += eff.Amount;
                    acc.WisdomPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Wisdom. Total [{ acc.WisdomPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xadd8e6),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xadd8e6),
                            Message = "{\"key\": \"+1 Wisdom\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xadd8e6),
                            Message = "{\"key\": \"+1 Wisdom\"}"
                        }, PacketPriority.Low);

                }
                if (Random == 5 && acc.DexterityPotsMoon <= 9)
                {
                    Stats.Base[5] += eff.Amount;
                    acc.DexterityPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Dexterity. Total [{ acc.DexterityPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xffa500),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xffa500),
                            Message = "{\"key\": \"+1 Dexterity\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xffa500),
                            Message = "{\"key\": \"+1 Dexterity\"}"
                        }, PacketPriority.Low);
                }
                if (Random == 0 && acc.LifePotsMoon <= 45)
                {
                    Stats.Base[0] += eff.Amount * 5;
                    acc.LifePotsMoon += eff.Amount * 5;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Health. Total [{ acc.LifePotsMoon}] / 50");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xadd8e6),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xadd8e6),
                            Message = "{\"key\": \"+5 Life\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xadd8e6),
                            Message = "{\"key\": \"+5 Life\"}"
                        }, PacketPriority.Low);


                }
                if (Random == 1 && acc.ManaPotsMoon <= 45)
                {
                    Stats.Base[1] += eff.Amount * 5;
                    acc.ManaPotsMoon += eff.Amount * 5;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Mana. Total [{ acc.ManaPotsMoon}] / 50");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFFFF00),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFFFF00),
                            Message = "{\"key\": \"+5 Mana\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFFFF00),
                            Message = "{\"key\": \"+5 Mana\"}"
                        }, PacketPriority.Low);
                }
                if (Random == 8 && acc.CritHitPotsMoon <= 9)
                {
                    Stats.Base[12] += eff.Amount;
                    acc.CritHitPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Critical Chance. Total [{ acc.CritHitPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0x800080),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x800080),
                            Message = "{\"key\": \"+1 Critical Hit\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0x800080),
                            Message = "{\"key\": \"+1 Critical Hit\"}"
                        }, PacketPriority.Low);

                }
                if (Random == 9 && acc.CritDmgPotsMoon <= 9)
                {
                    Stats.Base[11] += eff.Amount;
                    acc.CritDmgPotsMoon += eff.Amount;
                    acc.FlushAsync();
                    SendInfo($"You have gained { eff.Amount } extra Critical Damage. Total [{ acc.CritDmgPotsMoon}] / 10");

                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xFF0000),
                        Pos1 = new Position() { X = 1 }
                    }, PacketPriority.Low);

                    if (!Manager.Resources.Settings.DisableAlly)
                        BroadcastSync(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFF0000),
                            Message = "{\"key\": \"+1 Critical Damage\"}"
                        }, p => this.DistSqr(p) < RadiusSqr);
                    else
                        Client.SendPacket(new Notification()
                        {
                            ObjectId = Id,
                            Color = new ARGB(0xFF0000),
                            Message = "{\"key\": \"+1 Critical Damage\"}"
                        }, PacketPriority.Low);

                }
                if (acc.LifePotsMoon >= 10 && acc.ManaPotsMoon >= 10 && acc.AttackStatsMoon >= 10 && acc.DexterityPotsMoon >= 10 && acc.SpeedPotsMoon >= 10 && acc.DefensePotsMoon >= 10 && acc.WisdomPotsMoon >= 10 && acc.VitalityPotsMoon >= 10 && acc.CritDmgPotsMoon >= 10 && acc.CritHitPotsMoon >= 10)
                {
                    AddLunarPotion(eff);
                }
                else if (acc.CritDmgPotsMoon >= 10 && Random == 9 || acc.CritHitPotsMoon >= 10 && Random == 8 || acc.AttackStatsMoon >= 10 && Random == 2 || acc.DefensePotsMoon >= 10 && Random == 3 || acc.SpeedPotsMoon >= 10 && Random == 4 || acc.VitalityPotsMoon >= 10 && Random == 6 || acc.WisdomPotsMoon >= 10 && Random == 7 || acc.DexterityPotsMoon >= 10 && Random == 5 || acc.LifePotsMoon >= 10 && Random == 0 || acc.ManaPotsMoon >= 10 && Random == 1)
                {
                    SendInfo($"The Celestial Enhancer broke in your hands.");
                }
            }
            else
            {
                SendInfo($"Character must be Moon Primed to use this item.");

                var availableSlot = Inventory.GetAvailableInventorySlot(item);

                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, PacketPriority.Low);
            }

        }
        private void AE5Fragments(RealmTime time, Item item, Position target, ActivateEffect eff) // here
        {
          
            ushort gift = 0x913;
            SendInfo($"You have forged A [Crown].");
            var availableSlot = Inventory.GetAvailableInventorySlot(item);
            Inventory[availableSlot] = Manager.Resources.GameData.Items[gift];
            

        }
        private void AEMoonPrimed(RealmTime time, Item item, Position target, ActivateEffect eff) // here
        {
            var chr = _client.Character;
            var pd = Manager.Resources.GameData.Classes[ObjectType];
            var statInfo = Manager.Resources.GameData.Classes[ObjectType].Stats;
            var acc = Manager.Database.GetAccount(AccountId);
            //
            chr.MoonTime += 120000;
            var math = (chr.MoonTime / 100) / 60;


            if (Stats.Base[0] >= pd.Stats[0].MaxValue && Stats.Base[1] >= pd.Stats[1].MaxValue && Stats.Base[2] >= pd.Stats[2].MaxValue && Stats.Base[3] >= pd.Stats[3].MaxValue && Stats.Base[4] >= pd.Stats[4].MaxValue && Stats.Base[5] >= pd.Stats[5].MaxValue && Stats.Base[6] >= pd.Stats[6].MaxValue && Stats.Base[7] >= pd.Stats[7].MaxValue && Stats.Base[11] >= pd.Stats[11].MaxValue && Stats.Base[12] >= pd.Stats[12].MaxValue)
            {

                chr.MoonPrimed = true;
                SendInfo($"You have been granted +30 minutes on the moon.");
                SendInfo($"You can now enter the moon with the gods permission.");
                SendInfo($"This character can now use Celestial Enhancers.");
                SendInfo($"Time left on the moon: " + math + " minutes.");

                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFEFCD7),
                        Message = "{\"key\": \"Moon Unlocked\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFEFCD7),
                        Message = "{\"key\": \"Moon Unlocked\"}"
                    }, PacketPriority.Low);

            }
            else
            {
                SendInfo($"Character must be maxed 10/10 before you can Ascend.");
                var availableSlot = Inventory.GetAvailableInventorySlot(item);
                Inventory[availableSlot] = item;

            }
        }

        private void AEFixedStat(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            Stats.Base[idx] = eff.Amount;
        }

        private void AERemoveNegativeConditionSelf(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            ApplyConditionEffect(NegativeEffs);
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, PacketPriority.Low);
        }

        private void AERemoveNegativeConditions(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            this.AOE(eff.Range, true, player => player.ApplyConditionEffect(NegativeEffs));
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = eff.Range }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = eff.Range }
                }, PacketPriority.Low);
        }

        private void AEPoisonGrenade(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.Throw,
                    Color = new ARGB(0xffddff00),
                    TargetObjectId = Id,
                    Pos1 = target
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.Throw,
                    Color = new ARGB(0xffddff00),
                    TargetObjectId = Id,
                    Pos1 = target
                }, p => this.DistSqr(p) < RadiusSqr);

            var x = new Placeholder(Manager, 2500);
            x.Move(target.X, target.Y);
            Owner.EnterWorld(x);
            Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffddff00),
                    TargetObjectId = x.Id,
                    Pos1 = new Position() { X = eff.Radius }
                }, PacketPriority.Low);

                world.AOE(target, eff.Radius, false,
                    enemy => PoisonEnemy(world, enemy as Enemy, eff));
            }));
        }
        private void AEBulletNova2(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            const double coneRange = Math.PI / 4;
            var mouseAngle = Math.Atan2(target.Y - Y, target.X - X);

            // get starting target
            var startTarget = this.GetNearestEntity(MaxAbilityDist, false, e => e is Enemy &&
                Math.Abs(mouseAngle - Math.Atan2(e.Y - Y, e.X - X)) <= coneRange);

            var prjs = new Projectile[20];
            var prjDesc = item.Projectiles[0]; //Assume only one
            var batch = new Packet[21];
            var pkts = new List<Packet>();

            for (var i = 0; i < 20; i++)
            {

                var StartingPos2 = new Position()
                {
                    X = startTarget.X,
                    Y = startTarget.Y
                };

                var proj = CreateProjectile(prjDesc, item.ObjectType,
                    Random.Next(prjDesc.MinDamage, prjDesc.MaxDamage),
                    time.TotalElapsedMs, StartingPos2, (float)(i * (Math.PI * 2) / 20));
                Owner.EnterWorld(proj);
                FameCounter.Shoot(proj);
                batch[i] = new ServerPlayerShoot()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Id,
                    ContainerType = item.ObjectType,
                    StartingPos = new Position()
                    {
                        X = startTarget.X,
                        Y = startTarget.Y
                    },
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                };
                prjs[i] = proj;
            }
            batch[20] = new ShowEffect()
            {
                EffectType = EffectType.Trail,
                Pos1 = new Position()
                {
                    X = startTarget.X,
                    Y = startTarget.Y
                },
                TargetObjectId = Id,
                Color = new ARGB(0xFFFF00AA)
            };
            Client.SendPackets(batch);

        }
























































        private void AELightning(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            const double coneRange = Math.PI / 4;
            var mouseAngle = Math.Atan2(target.Y - Y, target.X - X);

            // get starting target
            var startTarget = this.GetNearestEntity(MaxAbilityDist, false, e => e is Enemy &&
                Math.Abs(mouseAngle - Math.Atan2(e.Y - Y, e.X - X)) <= coneRange);

            var amount = eff.TotalDamage;
            var hits = eff.MaxTargets;

            var Wisdom = (Stats.Base[7] + Stats.Boost[7]);

            var WisAmountDMG = eff.TotalDamage / 50;
            var WisAmountTGR = eff.MaxTargets / 75;

            amount = eff.TotalDamage + (WisAmountDMG * Wisdom);
            hits = eff.MaxTargets - (WisAmountTGR * Wisdom);



            // no targets? bolt air animation
            if (startTarget == null)
            {
                var noTargets = new Packet[3];
                var angles = new double[] { mouseAngle, mouseAngle - coneRange, mouseAngle + coneRange };
                for (var i = 0; i < 3; i++)
                {
                    var x = (int)(MaxAbilityDist * Math.Cos(angles[i])) + X;
                    var y = (int)(MaxAbilityDist * Math.Sin(angles[i])) + Y;
                    noTargets[i] = new ShowEffect()
                    {
                        EffectType = EffectType.Trail,
                        TargetObjectId = Id,
                        Color = new ARGB(0xffff0088),
                        Pos1 = new Position()
                        {
                            X = x,
                            Y = y
                        },
                        Pos2 = new Position() { X = 350 }
                    };
                }
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(noTargets, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPackets(noTargets, PacketPriority.Low);
                return;
            }

            var current = startTarget;
            var targets = new Entity[hits];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = current;
                var next = current.GetNearestEntity(10, false, e =>
                {
                    if (!(e is Enemy) ||
                        e.HasConditionEffect(ConditionEffects.Invincible) ||
                        e.HasConditionEffect(ConditionEffects.Stasis) ||
                        Array.IndexOf(targets, e) != -1)
                        return false;

                    return true;
                });

                if (next == null)
                    break;

                current = next;
            }
            var pkts = new List<Packet>();
            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null)
                    break;

                var prev = i == 0 ? this : targets[i - 1];

                (targets[i] as Enemy).Damage(this, time, amount, false);

                if (eff.ConditionEffect != null)
                    targets[i].ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = eff.ConditionEffect.Value,
                        DurationMS = (int)(eff.EffectDuration * 1000)
                    });

                pkts.Add(new ShowEffect()
                {
                    EffectType = EffectType.Lightning,
                    TargetObjectId = prev.Id,
                    Color = new ARGB(0xffff0088),
                    Pos1 = new Position()
                    {
                        X = targets[i].X,
                        Y = targets[i].Y
                    },
                    Pos2 = new Position() { X = 350 }
                });


            }




            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }
        private Decoy TrickDecoy;

        private void AEDecoy(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var Wisdom = (Stats.Base[7] + Stats.Boost[7]);

            var WisAmountDMG = (eff.DurationMS / 125);
            int Calculation = (WisAmountDMG * Wisdom) + eff.DurationMS;

            if (DecoyStillActive == true)
            {
                Xdecoy = target.X;
                Ydecoy = target.Y;
                SendInfo($"True");
                return;
            }

            _canDecoyCooldownTime = Calculation + 3000;
            DecoyStillActive = true;
            var decoy22 = new Decoy(this, Calculation, 4, target);
            TrickDecoy = decoy22;
            TrickDecoy.Move(X, Y);
            Owner.EnterWorld(TrickDecoy);
            SendInfo($"False");
            
        }
        /*
        private void AEDecoy(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            int TeleportSeconds = Convert.ToInt32(eff.Radius);
            if (eff.Radius == 0f)
            {
                TeleportSeconds = 8;
            }

            var time2 = eff.Radius;
            AmountofUses++;
            if (AmountofUses == 1)
            {

                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xffffff),
                        Message = "{\"key\": \"Prism Linked\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xffffff),
                        Message = "{\"key\": \"Prism Linked\"}"
                    }, PacketPriority.Low);
                Setlocation1 = X;
                Setlocation2 = Y;
                AmountofUses++;
            }
            if (AmountofUses == 2)
            {
                _canTpCooldownTime14 = TeleportSeconds * 1000;
            }
            if (AmountofUses == 4)
            {
                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "Teleporting Canceled"
                }, PacketPriority.Low);
                AmountofUses = 0;
                Cancel = true;
            }
            if (AmountofUses == 3)
            {
                Cancel = false;

                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "Teleporting in " + eff.Radius + "s"
                }, PacketPriority.Low);

                Owner.Timers.Add(new WorldTimer(_canTpCooldownTime14, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        Position pos = new Position();
                        var decoy = new Decoy(this, eff.DurationMS, 4);
                        decoy.Move(pos.X, pos.Y);
                        Owner.EnterWorld(decoy);

                        TeleportPosition(time, x: Setlocation1, y: Setlocation2, true);
                        AmountofUses = 0;
                    }
                }));
                Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(3000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(4000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(6000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(7000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
                Owner.Timers.Add(new WorldTimer(8000, (world, t) =>
                {
                    if (Cancel == false)
                    {
                        time2 -= 1;
                        if (time2 > 0)
                        {
                            Client.SendPacket(new Notification()
                            {
                                ObjectId = Id,
                                Color = new ARGB(0xffffff),
                                Message = "Teleporting in " + time2 + "s"
                            }, PacketPriority.Low);
                        }
                    }
                }));
            }


            if (eff.NumShots > 0)
            {
                var prjDesc = item.Projectiles[0];
                var batch = new Packet[eff.NumShots];
                Position pos = new Position();
                Owner?.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                {

                    for (var i = 0; i < eff.NumShots; i++)
                    {
                        var proj = CreateProjectile(prjDesc, item.ObjectType,
                        Random.Next(prjDesc.MinDamage, prjDesc.MaxDamage),
                        t.TotalElapsedMs, pos, (float)(i * (Math.PI * 2) / eff.NumShots));
                        batch[i] = new ServerPlayerShoot()
                        {
                            BulletId = proj.ProjectileId,
                            OwnerId = Id,
                            ContainerType = item.ObjectType,
                            StartingPos = pos,
                            Angle = proj.Angle,
                            Damage = (short)proj.Damage
                        };
                        if (proj != null)
                        {
                            Owner?.EnterWorld(proj);
                            FameCounter.Shoot(proj);
                        }
                    }
                    Client.SendPackets(batch);
                }));
            }

        }

    */
        private void AEIceTomeAB(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            int Wisdom = Stats.Boost[7] + Stats[7];
            float Calculations = Wisdom / 1.25f;
            int CalculationsNonFloat = (int)Math.Round(Calculations);
            var pkts = new List<Packet>();
            var players = new List<Player>();
            var Random = new Random().Next(0, 4);
            if (Wisdom <= 500)
            {
                if (Random == 1)
                {
                    ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Slowed, DurationMS = 3000 });
                    Stats.Boost.ActivateBoost[7].Push(CalculationsNonFloat, true);
                    Stats.ReCalculateValues();
                    Owner.Timers.Add(new WorldTimer(7000, (world, t) =>
                    {
                        Stats.Boost.ActivateBoost[7].Pop(CalculationsNonFloat, true);
                        Stats.ReCalculateValues();
                    }));
                }
            }
           
        }

        private void AETOMEAB(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            int Wisdom = Stats.Boost[7] + Stats[7];
            float Calculations = Wisdom / 1.25f;
            int CalculationsNonFloat = (int)Math.Round(Calculations);
            var pkts = new List<Packet>();
            var players = new List<Player>();
            var Random = new Random().Next(0, 4);
            if (Random == 1)
            {
                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "Exorcism"
                }, PacketPriority.Low);

                var damage = 100 * Wisdom;
                var range = 6;
                var enemies = new List<Enemy>();
                this.AOE(range, false, enemy => enemies.Add(enemy as Enemy));
                if (enemies.Count() > 0)
                    foreach (var enemy in enemies)
                        enemy?.Damage(this, time, damage, true);

                Client?.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position { X = range, Y = 0 }
                }, PacketPriority.Low);

            }
        }
        private void AERandomAB(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            var Random = new Random().Next(0, 8);
            if (Random == 0)
            {
                ApplyConditionEffect(ConditionEffectIndex.Strength, 3000);

            }
            if (Random == 1)
            {

                ApplyConditionEffect(ConditionEffectIndex.Damaging, 3000);
            }


            //OffensiveTeam
        }

        private void AEOffensiveTeam(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            var Random = new Random().Next(0, 100);
            if (Random <= 6)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x0000ff),
                        Message = "{\"key\": \"Offense\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x0000ff),
                        Message = "{\"key\": \"Offense\"}"
                    }, PacketPriority.Low);

                this.AOE(5, true, player =>
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Damaging,
                        DurationMS = 4000
                    });
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Armored,
                        DurationMS = 4000
                    });

                });
            }

        }

        private void AEShockwave(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            var Random = new Random().Next(0, 100);
            if (Random <= 10)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x0000ff),
                        Message = "{\"key\": \"Shockwave\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x0000ff),
                        Message = "{\"key\": \"Shockwave\"}"
                    }, PacketPriority.Low);

                this.AOE(5, false, player =>
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Paralyzed,
                        DurationMS = 10000
                    });

                });
            }

        }

        private void AESupportiveTeam(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            var Random = new Random().Next(0, 100);
            if (Random <= 5)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x800080),
                        Message = "{\"key\": \"Indestructible\"}"
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new Notification()
                    {
                        ObjectId = Id,
                        Color = new ARGB(0x800080),
                        Message = "{\"key\": \"Indestructible\"}"
                    }, PacketPriority.Low);

                this.AOE(5, true, player =>
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Invulnerable,
                        DurationMS = 4000
                    });

                });
                HP += 250;

            }

        }
        private void StasisBlast(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>
            {
                new ShowEffect()
                {
                    EffectType = EffectType.Concentrate,
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position() {X = target.X + 3, Y = target.Y},
                    Color = new ARGB(0xffffffff)
                }
            };

            Owner.AOE(target, 3, false, enemy =>
            {
                if (enemy.HasConditionEffect(ConditionEffects.StasisImmune))
                {
                    pkts.Add(new Notification()
                    {
                        ObjectId = enemy.Id,
                        Color = new ARGB(0xff00ff00),
                        Message = "Immune"
                    });
                }
                else if (!enemy.HasConditionEffect(ConditionEffects.Stasis))
                {
                    enemy.ApplyConditionEffect(ConditionEffectIndex.Stasis, eff.DurationMS);

                    Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                        enemy.ApplyConditionEffect(ConditionEffectIndex.StasisImmune, 3000)));

                    pkts.Add(new Notification()
                    {
                        ObjectId = enemy.Id,
                        Color = new ARGB(0xffff0000),
                        Message = "Stasis"
                    });
                }
            });
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }

        private void AETrap(RealmTime time, Item item, Position target, ActivateEffect eff)
        {

            int amount = eff.TotalDamage;
            float hits = eff.MaxTargets;

            var Wisdom = (Stats.Base[7] + Stats.Boost[7]);

            float WisAmountDMG = eff.TotalDamage / 75;
            float WisAmountTGR = eff.Radius / 140;
            float WisAmountTGRR = eff.EffectDuration / 200;

            amount = eff.TotalDamage + (int)(WisAmountDMG * Wisdom);
            hits = eff.Radius + (WisAmountTGR * Wisdom);
            float duration = eff.EffectDuration + (WisAmountTGRR * Wisdom);

            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.BeachBall,
                    Color = new ARGB(item.ObjectType),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = X, Y = Y }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.BeachBall,
                    Color = new ARGB(item.ObjectType),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = X, Y = Y }
                }, PacketPriority.Low);


            Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                var trap = new Trap(
                    this,
                    hits,
                    amount,
                    eff.ConditionEffect ?? ConditionEffectIndex.Slowed,
                    duration);
                trap.Move(target.X, target.Y);
                world.EnterWorld(trap);
            }));
        }

        private void AEVampireBlast(RealmTime time, Item item, Position target, ActivateEffect eff)
        {


            
            var RealDamage = (eff.TotalDamage / 2);

            var Wisdom = (Stats.Base[7] + Stats.Boost[7]);
            var Vitality = (Stats.Base[6] + Stats.Boost[6]) / 2;

            var WisAmountTGR = eff.Radius / 150;
            var damage = (Wisdom * RealDamage / 100) + (Vitality * RealDamage / 100) + RealDamage;
            var range = (int)(WisAmountTGR * Wisdom) + eff.Radius;

            var pkts = new List<Packet>
            {
                new ShowEffect()
                {
                    EffectType = EffectType.Trail,
                    TargetObjectId = Id,
                    Pos1 = target,
                    Color = new ARGB(0xFFFF0000)
                },
                new ShowEffect
                {
                    EffectType = EffectType.Diffuse,
                    Color = new ARGB(0xFFFF0000),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = target.X + range, Y = target.Y }
                }
            };

          
            var enemies = new List<Enemy>();
            var totalDmg = 0;
            Owner.AOE(target, range, false, enemy => enemies.Add(enemy as Enemy));
            if (enemies.Count() > 0)
                foreach (var enemy in enemies)
                    enemy?.Damage(this, time, (int)damage, true);


            Owner.AOE(target, range, false, enemy =>
            {
                enemies.Add(enemy as Enemy);
                totalDmg += (enemy as Enemy).Damage(this, time, (int)damage, false);

            });



            var players = new List<Player>();
            this.AOE(range, true, player =>
            {
                if (!player.HasConditionEffect(ConditionEffects.Sick))
                {
                    players.Add(player as Player);
                    ActivateHealHp(player as Player, totalDmg, pkts);
                }
            });

            if (enemies.Count > 0)
            {
                var rand = new Random();
                for (var i = 0; i < 5; i++)
                {
                    var a = enemies[rand.Next(0, enemies.Count)];
                    var b = players[rand.Next(0, players.Count)];
                    pkts.Add(new ShowEffect()
                    {
                        EffectType = EffectType.Flow,
                        TargetObjectId = b.Id,
                        Pos1 = new Position() { X = a.X, Y = a.Y },
                        Color = new ARGB(0xffffffff)
                    });
                }
            }
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }//LHelmJug

        private void AELHelmJug(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            Client.SendPacket(new Notification()
            {
                ObjectId = Id,
                Color = new ARGB(0xBAACBC),
                Message = "{\"key\": \"Lunar Eyesight\"}"
            }, PacketPriority.Low);


            ApplyConditionEffect(NegativeEffs);

            Stats.Boost.ActivateBoost[3].Push(25, true);
            Stats.ReCalculateValues();
            Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                Stats.Boost.ActivateBoost[3].Pop(25, true);
                Stats.ReCalculateValues();

            }));
        }
        private void AETeleport(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            TeleportPosition(time, target, true);
        }
        private void AETQDscroll(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var acc = Manager.Database.GetAccount(AccountId);
            var pick = Random.Next(1, eff.Amount);

            acc.TQDamount += pick;
            acc.FlushAsync();

            Client.SendPacket(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffff),
                Pos1 = new Position() { X = 2 }
            }, PacketPriority.Low);

            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Dungeon TQ Uses"
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Dungeon TQ Uses"
                }, PacketPriority.Low);

            SendInfo($"You now have { pick } extra Dungeon tq's. Total [{ acc.TQDamount}]");
        }

        private void AETQMscroll(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var acc = Manager.Database.GetAccount(AccountId);
            var pick = Random.Next(1, eff.Amount);

            acc.TQMamount += pick;
            acc.FlushAsync();

            Client.SendPacket(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffff),
                Pos1 = new Position() { X = 2 }
            }, PacketPriority.Low);

            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Moon TQ Uses"
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Moon TQ Uses"
                }, PacketPriority.Low);

            SendInfo($"You now have { pick } extra Moon tq's. Total [{ acc.TQMamount}]");
        }
        private void AETQscroll(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var acc = Manager.Database.GetAccount(AccountId);
            var pick = Random.Next(1, eff.Amount);

            acc.TQamount += pick;
            acc.FlushAsync();

            Client.SendPacket(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffff),
                Pos1 = new Position() { X = 2 }
            }, PacketPriority.Low);

            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Earth TQ Uses"
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new Notification()
                {
                    ObjectId = Id,
                    Color = new ARGB(0xffffff),
                    Message = "+" + pick + " Earth TQ Uses"
                }, PacketPriority.Low);

            SendInfo($"You now have { pick } extra earth tq's. Total [{ acc.TQamount}]");
        }
        //TQscroll
        private void AEMagicNova(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            this.AOE(eff.Range, true, player =>
                ActivateHealMp(player as Player, eff.Amount, pkts));
            pkts.Add(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = eff.Range }
            });
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }

        private void AEMagic(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var pkts = new List<Packet>();
            ActivateHealMp(this, eff.Amount, pkts);
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }

        private void AEHealNova(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var amount = eff.Amount;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                amount = (int)UseWisMod(eff.Amount, 0);
                range = UseWisMod(eff.Range);
            }

            var pkts = new List<Packet>();
            this.AOE(range, true, player =>
            {
                if (!player.HasConditionEffect(ConditionEffects.Sick))
                    ActivateHealHp(player as Player, amount, pkts);
            });
            pkts.Add(new ShowEffect()
            {
                EffectType = EffectType.AreaBlast,
                TargetObjectId = Id,
                Color = new ARGB(0xffffffff),
                Pos1 = new Position() { X = range }
            });
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPackets(pkts, PacketPriority.Low);
        }

        private void AEHeal(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!HasConditionEffect(ConditionEffects.Sick))
            {
                var pkts = new List<Packet>();
                ActivateHealHp(this, eff.Amount, pkts);
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(pkts, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPackets(pkts, PacketPriority.Low);
            }
        }

        private void AEConditionEffectAura(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var duration = eff.DurationMS;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);
                range = UseWisMod(eff.Range);
            }

            this.AOE(range, true, player =>
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = eff.ConditionEffect.Value,
                    DurationMS = duration
                });
            });
            var color = 0xffffffff;
            if (eff.ConditionEffect.Value == ConditionEffectIndex.Damaging)
                color = 0xffff0000;
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(color),
                    Pos1 = new Position() { X = range }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(color),
                    Pos1 = new Position() { X = range }
                }, PacketPriority.Low);
        }

        private void AEClearConditionEffectAura(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            this.AOE(eff.Range, true, player =>
            {
                var condition = eff.CheckExistingEffect;
                ConditionEffects conditions = 0;
                conditions |= (ConditionEffects)(1 << (Byte)condition.Value);
                if (!condition.HasValue || player.HasConditionEffect(conditions))
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = eff.ConditionEffect.Value,
                        DurationMS = 0
                    });
                }
            });
        }

        private void AEConditionEffectSelf(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var duration = eff.DurationMS;
            if (eff.UseWisMod)
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);

            ApplyConditionEffect(new ConditionEffect()
            {
                Effect = eff.ConditionEffect.Value,
                DurationMS = duration
            });
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff),
                    Pos1 = new Position() { X = 1 }
                }, PacketPriority.Low);
        }

        private void AEStatBoostAura(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var amount = eff.Amount;
            var duration = eff.DurationMS;
            var range = eff.Range;
            if (eff.UseWisMod)
            {
                amount = (int)UseWisMod(eff.Amount, 0);
                duration = (int)(UseWisMod(eff.DurationSec) * 1000);
                range = UseWisMod(eff.Range);
            }

            this.AOE(range, true, player =>
            {
                var p = player as Player;

                p.Stats.Boost.ActivateBoost[idx].Push(amount, eff.NoStack);
                p.Stats.ReCalculateValues();


                // hack job to allow instant heal of nostack boosts
                if (eff.NoStack && amount > 0 && idx == 0)
                    p.HP = Math.Min(p.Stats[0], p.HP + amount);

                Owner.Timers.Add(new WorldTimer(duration, (world, t) =>
                {
                    p.Stats.Boost.ActivateBoost[idx].Pop(amount, eff.NoStack);
                    p.Stats.ReCalculateValues();
                }));
            });

            if (!eff.NoStack)
            {
                if (!Manager.Resources.Settings.DisableAlly)
                    BroadcastSync(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xffffffff),
                        Pos1 = new Position() { X = range }
                    }, p => this.DistSqr(p) < RadiusSqr);
                else
                    Client.SendPacket(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = Id,
                        Color = new ARGB(0xffffffff),
                        Pos1 = new Position() { X = range }
                    }, PacketPriority.Low);
            }
        }

        private void AEStatBoostSelf(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var idx = StatsManager.GetStatIndex((StatsType)eff.Stats);
            var s = eff.Amount;
            Stats.Boost.ActivateBoost[idx].Push(s, eff.NoStack);
            Stats.ReCalculateValues();
            Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
            {
                Stats.Boost.ActivateBoost[idx].Pop(s, eff.NoStack);
                Stats.ReCalculateValues();
            }));
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.Potion,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff)
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.Potion,
                    TargetObjectId = Id,
                    Color = new ARGB(0xffffffff)
                }, PacketPriority.Low);
        }

        private void AEShoot(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var arcGap = item.ArcGap * Math.PI / 180;
            var startAngle = Math.Atan2(target.Y - Y, target.X - X) - (item.NumProjectiles - 1) / 2 * arcGap;
            var prjDesc = item.Projectiles[0]; //Assume only one

            var sPkts = new Packet[item.NumProjectiles];
            for (var i = 0; i < item.NumProjectiles; i++)
            {
                var proj = CreateProjectile(prjDesc, item.ObjectType,
                    (int)Stats.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage, true),
                    time.TotalElapsedMs, new Position() { X = X, Y = Y }, (float)(startAngle + arcGap * i));
                Owner.EnterWorld(proj);
                if (!Manager.Resources.Settings.DisableAlly)
                    sPkts[i] = new AllyShoot()
                    {
                        OwnerId = Id,
                        Angle = proj.Angle,
                        ContainerType = item.ObjectType,
                        BulletId = proj.ProjectileId
                    };
                FameCounter.Shoot(proj);
            }
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(sPkts, p => p != this && this.DistSqr(p) < RadiusSqr);
        }

        private void AEBulletNova(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var prjs = new Projectile[20];
            var prjDesc = item.Projectiles[0]; //Assume only one
            var batch = new Packet[21];
            for (var i = 0; i < 20; i++)
            {
                var proj = CreateProjectile(prjDesc, item.ObjectType,
                    Random.Next(prjDesc.MinDamage, prjDesc.MaxDamage),
                    time.TotalElapsedMs, target, (float)(i * (Math.PI * 2) / 20));
                Owner.EnterWorld(proj);
                FameCounter.Shoot(proj);
                batch[i] = new ServerPlayerShoot()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Id,
                    ContainerType = item.ObjectType,
                    StartingPos = target,
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                };
                prjs[i] = proj;
            }
            batch[20] = new ShowEffect()
            {
                EffectType = EffectType.Trail,
                Pos1 = target,
                TargetObjectId = Id,
                Color = new ARGB(0xFFFF00AA)
            };

            Client.SendPackets(batch);
        }
        private void AEVargoBulletNova(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var projV = (int)(20 * (1 + (Stats[7] / 100.0)));
            var prjs = new Projectile[projV];
            var prjDesc = item.Projectiles[0]; //Assume only one
            var batch = new Packet[projV+1];
            for (var i = 0; i < projV; i++)
            {
                var proj = CreateProjectile(prjDesc, item.ObjectType,
                    Random.Next((int)(prjDesc.MinDamage * (2 * (1 + (Stats[7] / 100.0)))), (int)(prjDesc.MaxDamage * (2 * (1 + (Stats[7] / 100.0))))),
                    time.TotalElapsedMs, target, (float)(i * (Math.PI * 2) / projV));
                Owner.EnterWorld(proj);
                FameCounter.Shoot(proj);
                batch[i] = new ServerPlayerShoot()
                {
                    BulletId = proj.ProjectileId,
                    OwnerId = Id,
                    ContainerType = item.ObjectType,
                    StartingPos = target,
                    Angle = proj.Angle,
                    Damage = (short)proj.Damage
                };
                prjs[i] = proj;
            }
            batch[projV] = new ShowEffect()
            {
                EffectType = EffectType.Trail,
                Pos1 = target,
                TargetObjectId = Id,
                Color = new ARGB(0xFFFF00AA)
            };

            Client.SendPackets(batch);
        }


        private void AEGenericActivate(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            var targetPlayer = eff.Target.Equals("player");
            var centerPlayer = eff.Center.Equals("player");
            var duration = (eff.UseWisMod) ?
                (int)(UseWisMod(eff.DurationSec) * 1000) :
                eff.DurationMS;
            var range = (eff.UseWisMod)
                ? UseWisMod(eff.Range)
                : eff.Range;

            if (eff.ConditionEffect != null)
                Owner.AOE((eff.Center.Equals("mouse")) ? target : new Position { X = X, Y = Y }, range, targetPlayer, entity =>
                {
                    if (!entity.HasConditionEffect(ConditionEffects.Stasis) &&
                       !entity.HasConditionEffect(ConditionEffects.Invincible))
                    {
                        entity.ApplyConditionEffect(new ConditionEffect()
                        {
                            Effect = eff.ConditionEffect.Value,
                            DurationMS = duration
                        });
                    }
                });
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = (EffectType)eff.VisualEffect,
                    TargetObjectId = Id,
                    Color = new ARGB(eff.Color),
                    Pos1 = (centerPlayer) ? new Position() { X = range } : target,
                    Pos2 = new Position() { X = target.X - range, Y = target.Y }
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = (EffectType)eff.VisualEffect,
                    TargetObjectId = Id,
                    Color = new ARGB(eff.Color),
                    Pos1 = (centerPlayer) ? new Position() { X = range } : target,
                    Pos2 = new Position() { X = target.X - range, Y = target.Y }
                }, PacketPriority.Low);
        }

        private void AEHealingGrenade(RealmTime time, Item item, Position target, ActivateEffect eff)
        {
            if (!Manager.Resources.Settings.DisableAlly)
                BroadcastSync(new ShowEffect()
                {
                    EffectType = EffectType.Throw,
                    Color = new ARGB(0xffddff00),
                    TargetObjectId = Id,
                    Pos1 = target
                }, p => this.DistSqr(p) < RadiusSqr);
            else
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.BeachBall,
                    Color = new ARGB(item.ObjectType),
                    TargetObjectId = Id,
                    Pos1 = target,
                    Pos2 = new Position { X = X, Y = Y }
                }, PacketPriority.Low);

            var x = new Placeholder(Manager, 2500);
            x.Move(target.X, target.Y);
            Owner.EnterWorld(x);
            Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                Client.SendPacket(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffddff00),
                    TargetObjectId = x.Id,
                    Pos1 = new Position() { X = eff.Radius }
                }, PacketPriority.Low);

                world.AOE(target, eff.Radius, true,
                    player => HealingPlayersPoison(world, player as Player, eff));
            }));
        }

        static void ActivateHealHp(Player player, int amount, List<Packet> pkts)
        {
            var maxHp = player.Stats[0];
            var newHp = Math.Min(maxHp, player.HP + amount);
            

            pkts.Add(new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            });
            pkts.Add(new Notification()
            {
                Color = new ARGB(0xff00ff00),
                ObjectId = player.Id,
                Message = "+" + (newHp - player.HP)
            });

             if (newHp == player.HP)
              return;
            player.HP = newHp;
        }

        public void ActivateHealHpEnemy(Player player, int amount, List<Packet> pkts)
        {
            var maxHp = player.Stats[0];
            var newHp = Math.Min(maxHp, player.HP + amount);

            pkts.Add(new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            });
            pkts.Add(new Notification()
            {
                Color = new ARGB(0xff00ff00),
                ObjectId = player.Id,
                Message = "+" + (amount)
            });

            if (newHp == player.HP)
                return;
            player.HP = newHp;
        }

        static void ActivateHealMp(Player player, int amount, List<Packet> pkts)
        {
            var maxMp = player.Stats[1];
            var newMp = Math.Min(maxMp, player.MP + amount);
            if (newMp == player.MP)
                return;

            pkts.Add(new ShowEffect()
            {
                EffectType = EffectType.Potion,
                TargetObjectId = player.Id,
                Color = new ARGB(0xffffffff)
            });
            pkts.Add(new Notification()
            {
                Color = new ARGB(0xff9000ff),
                ObjectId = player.Id,
                Message = "+" + (newMp - player.MP)
            });

            player.MP = newMp;
        }

        public void PoisonEnemy(World world, Enemy enemy, ActivateEffect eff)
        {
            int Wisdom = Stats.Base[7] + Stats.Boost[7];

            var DPW = eff.Amount;
            var amount = Wisdom * eff.Amount; //damage per 1 wis EX. 5 amount with 100 wis = 500 extra wis damage.

            var TotalDamage = eff.TotalDamage + amount;

            var remainingDmg = (int)StatsManager.GetDefenseDamage(enemy, TotalDamage, enemy.ObjectDesc.Defense);
            var perDmg = remainingDmg * 1000 / eff.DurationMS;

            WorldTimer tmr = null;
            var x = 0;

            Func<World, RealmTime, bool> poisonTick = (w, t) =>
            {
                if (enemy.Owner == null || w == null)
                    return true;
                
                if (x % 4 == 0) // make sure to change this if timer delay is changed
                {
                    var thisDmg = perDmg;
                    if (remainingDmg < thisDmg)
                        thisDmg = remainingDmg;

                    enemy.Damage(this, t, thisDmg, true);
                    remainingDmg -= thisDmg;

                    LifeManaSteal(enemy, thisDmg);

                    if (remainingDmg <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            };

            tmr = new WorldTimer(250, poisonTick);
            world.Timers.Add(tmr);
        }

        void HealingPlayersPoison(World world, Player player, ActivateEffect eff)
        {
            var remainingHeal = eff.TotalDamage;
            var perHeal = eff.TotalDamage * 1000 / eff.DurationMS;

            WorldTimer tmr = null;
            var x = 0;

            Func<World, RealmTime, bool> healTick = (w, t) =>
            {
                if (player.Owner == null || w == null)
                    return true;

                if (x % 4 == 0) // make sure to change this if timer delay is changed
                {
                    var thisHeal = perHeal;
                    if (remainingHeal < thisHeal)
                        thisHeal = remainingHeal;

                    List<Packet> pkts = new List<Packet>();

                    Player.ActivateHealHp(player, thisHeal, pkts);
                    if (!Manager.Resources.Settings.DisableAlly)
                        w.BroadcastPackets(pkts, null, PacketPriority.Low);
                    else
                        Client.SendPackets(pkts, PacketPriority.Low);
                    remainingHeal -= thisHeal;
                    if (remainingHeal <= 0)
                        return true;
                }
                x++;

                tmr.Reset();
                return false;
            };

            tmr = new WorldTimer(250, healTick);
            world.Timers.Add(tmr);
        }

        private float UseWisMod(float value, int offset = 1)
        {
            double totalWisdom = Stats.Base[7] + Stats.Boost[7];

            if (totalWisdom < 30)
                return value;

            double m = (value < 0) ? -1 : 1;
            double n = (value * totalWisdom / 150) + (value * m);
            n = Math.Floor(n * Math.Pow(10, offset)) / Math.Pow(10, offset);
            if (n - (int)n * m >= 1 / Math.Pow(10, offset) * m)
            {
                return ((int)(n * 10)) / 10.0f;
            }

            return (int)n;
        }
    }
}