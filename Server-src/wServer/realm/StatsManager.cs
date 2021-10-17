﻿using common.resources;
using System;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm;


namespace wServer.realm
{
    public class StatsManager
    {
        internal const int NumStatTypes = 13;
        private const float MinAttackMult = 0.5f;
        private const float MaxAttackMult = 2f;

        public const float MIN_ATTACK_FREQ = .0015f;
        public const float MAX_ATTACK_FREQ = .008f;

        internal readonly Player Owner;
        internal readonly BaseStatManager Base;
        internal readonly BoostStatManager Boost;

        private readonly SV<int>[] _stats;
        public Inventory Inventory { get; private set; }
        public int this[int index] => Base[index] + Boost[index];

        public StatsManager(Player owner)
        {
            Owner = owner;
            Base = new BaseStatManager(this);
            Boost = new BoostStatManager(this);

            _stats = new SV<int>[NumStatTypes];
            for (var i = 0; i < NumStatTypes; i++)
                _stats[i] = new SV<int>(Owner, GetStatType(i), this[i], i != 0 && i != 1); // make maxHP and maxMP global update
        }

        public void ReCalculateValues(InventoryChangedEventArgs e = null)
        {
            Base.ReCalculateValues(e);
            Boost.ReCalculateValues(e);

            for (var i = 0; i < _stats.Length; i++)
                _stats[i].SetValue(this[i]);
        }

        internal void StatChanged(int index)
        {
            _stats[index].SetValue(this[index]);
        }

        public int GetAttackDamage(int min, int max, bool isAbility = false)
        {
           var ret = Owner.Client.Random.NextIntRange((uint)min, (uint)max) * GetAttackMult(isAbility) * CriticalModifier();
           //Log.Info($"Dmg: {ret}");
           return (int)ret;
        }

        public float GetAttackMult(bool isAbility)
        {
            if (isAbility)
                return 1;

            if (Owner.HasConditionEffect(ConditionEffects.Weak))
                return MinAttackMult;

            var mult = MinAttackMult + (this[2] / 75f) * (MaxAttackMult - MinAttackMult);

            if (Owner.HasConditionEffect(ConditionEffects.Damaging))
                mult *= 1.5f;

            if (Owner.HasConditionEffect(ConditionEffects.Awoken))
                mult *= 1.20f;


            if (Owner.HasConditionEffect(ConditionEffects.Strength))
                mult *= 1.25f;

            return mult;
        }

        public static float GetDefenseDamage(Entity host, int dmg, int def)
        {
            double Solid = 2;
            double Barrier = 1.5;
            double Armored = 1.25;
            double Diminished = 0.75;
            if (host.HasConditionEffect(ConditionEffects.Armored))
                def *= (int)(Armored);
            if (host.HasConditionEffect(ConditionEffects.Barrier))
                def *= (int)(Barrier);
            if (host.HasConditionEffect(ConditionEffects.Solid))
                def *= (int)(Solid);
            if (host.HasConditionEffect(ConditionEffects.Diminished))
                def *= (int)(Diminished);
            if (host.HasConditionEffect(ConditionEffects.ArmorBroken))
                def = 0;
          
            float limit = dmg * 0.25f;//0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (host.HasConditionEffect(ConditionEffects.Curse))
                ret = (int)(ret * 1.20);
          
            if (host.HasConditionEffect(ConditionEffects.Invulnerable) ||
                host.HasConditionEffect(ConditionEffects.Invincible))
                ret = 0;
            return ret;
        }

        public float GetDefenseDamage(int dmg, bool noDef)
        {
            var def = this[3];
            double Solid = 2;
            double Barrier = 1.5;
            double Armored = 1.25;
            double Diminished = 0.50;
            if (Owner.HasConditionEffect(ConditionEffects.Armored))
                def = (int)(def * Armored);
            if (Owner.HasConditionEffect(ConditionEffects.Diminished))
                def = (int)(def * Diminished);
            if (Owner.HasConditionEffect(ConditionEffects.Barrier))
                def = (int)(def * Barrier);
            if (Owner.HasConditionEffect(ConditionEffects.Solid))
                def = (int)(def * Solid);

            if (Owner.HasConditionEffect(ConditionEffects.ArmorBroken) || noDef)
                def = 0;
        
            float limit = dmg * 0.25f;//0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (Owner.HasConditionEffect(ConditionEffects.Petrify))
                ret = (int)(ret * .9);
            if (Owner.HasConditionEffect(ConditionEffects.Curse))
                ret = (int)(ret * 1.20);
            if (Owner.HasConditionEffect(ConditionEffects.Invulnerable) ||
                Owner.HasConditionEffect(ConditionEffects.Invincible) || 
                Owner.HasConditionEffect(ConditionEffects.NoDamage))//NoDamage
                ret = 0;
            return ret;
        }

        public float GetAttackFreq()
        {
            if (Owner.Inventory[0] == null)
                return 0;

            if (Owner.HasConditionEffect(ConditionEffects.Dazed))
                return MIN_ATTACK_FREQ;

            var baseFreq = MIN_ATTACK_FREQ + ((Owner.Stats[5] / 75.0f) * (MAX_ATTACK_FREQ - MIN_ATTACK_FREQ));

            if (Owner.HasConditionEffect(ConditionEffects.Berserk))
                baseFreq = baseFreq * 1.5f;
            if (Owner.HasConditionEffect(ConditionEffects.Haste))
                baseFreq = baseFreq * 1.25f;
            if (Owner.HasConditionEffect(ConditionEffects.Sluggish))
                baseFreq = baseFreq * 0.75f;
            if (Owner.HasConditionEffect(ConditionEffects.Tired))
                baseFreq = baseFreq * 0.85f;

            return  (1 / baseFreq) * (1 / Owner.Inventory[0].RateOfFire);
        } 

        public float CriticalModifier()
        {
            var returnamount = 0;
            var rand2Chance = Owner.Stats[7]; //Wisdomi
            if (rand2Chance >= 30 && Owner.Inventory[1].ObjectType.ToString() == "0x7a47") //Wis above30 | scale
            {
                returnamount = (Owner.Stats[7] / 100);
            }

            var randChance = Owner.Client.Random.NextIntRange(0, 200); //Raw crit chance
            var erandChance = Owner.Client.Random.NextIntRange(0, 100); //Raw Spirited crit chance
            var ret = 1.0f + returnamount; //raw crit Multiplier

            var critChance = CritChance(); //base crit chance stat
            var mult = CritMultiplier(); //base crit multiplier stat

            if (randChance < critChance)
                ret *= mult;

            if (Owner.EquipStatus.ContainsKey(EquippedStatus.Critical)) //If critical passive is on
            {
                if (erandChance < 1)
                    ret *= 5.0f;
            }

            return ret;
            
        }


        public int CritChance()
        {
            if (Owner.HasConditionEffect(ConditionEffects.Bloodlust))
                return Owner.Stats[12] + 25;
            else
                return Owner.Stats[12];
        }
        public float CritMultiplier()
        {
            float ret = (100 + Owner.Stats[11]) / 100f;
            return ret;
        }

        public static float GetSpeed(Entity entity, float stat)
        {
            var ret = 4 + 5.6f * (stat / 75f);
            if (entity.HasConditionEffect(ConditionEffects.Speedy))
                ret *= 1.5f;
            if (entity.HasConditionEffect(ConditionEffects.Swift))
                ret *= 1.25f;
            if (entity.HasConditionEffect(ConditionEffects.Frozen))
                ret = 3;
            if (entity.HasConditionEffect(ConditionEffects.Slowed))
                ret = 6;
            if (entity.HasConditionEffect(ConditionEffects.Paralyzed))
                ret = 0;
            return ret;
        }

        public float GetSpeed()
        {
            return GetSpeed(Owner, this[4]);
        }

        public float GetHPRegen()
        {
            var vit = this[6];
            if (Owner.HasConditionEffect(ConditionEffects.Sick))
                vit = 0;
            return 6 + vit * .16f;
        }

        public float GetMPRegen()
        {
            if (Owner.HasConditionEffect(ConditionEffects.Quiet))
                return 0;
            if (Owner.HasConditionEffect(ConditionEffects.Enchanted))
                return 0.5f + this[7] * .22f;
            return 0.5f + this[7] * .11f;
        }

        public static string StatIndexToName(int index)
        {
            switch (index)
            {
                case 0: return "MaxHitPoints";
                case 1: return "MaxMagicPoints";
                case 2: return "Attack";
                case 3: return "Defense";
                case 4: return "Speed";
                case 5: return "Dexterity";
                case 6: return "HpRegen";
                case 7: return "MpRegen";
                case 8: return "DamageMin";
                case 9: return "DamageMax";
                case 10: return "LuckBoost";
                case 11: return "CriticalDmg";
                case 12: return "CriticalHit";
            }
            return null;
        }

        public static int GetStatIndex(string name)
        {
            switch (name)
            {
                case "MaxHitPoints": return 0;
                case "MaxMagicPoints": return 1;
                case "Attack": return 2;
                case "Defense": return 3;
                case "Speed": return 4;
                case "Dexterity": return 5;
                case "HpRegen": return 6;
                case "MpRegen": return 7;
                case "DamageMin": return 8;
                case "DamageMax": return 9;
                case "LuckBoost": return 10;
                case "CriticalDmg": return 11;
                case "CriticalHit": return 12;
            }
            return -1;
        }

        public static int GetStatIndex(StatsType stat)
        {
            switch (stat)
            {
                case StatsType.MaximumHP:
                    return 0;
                case StatsType.MaximumMP:
                    return 1;
                case StatsType.Attack:
                    return 2;
                case StatsType.Defense:
                    return 3;
                case StatsType.Speed:
                    return 4;
                case StatsType.Dexterity:
                    return 5;
                case StatsType.Vitality:
                    return 6;
                case StatsType.Wisdom:
                    return 7;
                case StatsType.DamageMin:
                    return 8;
                case StatsType.DamageMax:
                    return 9;
                case StatsType.Luck:
                    return 10;
                case StatsType.CriticalDmg:
                    return 11;
                case StatsType.CriticalHit:
                    return 12;
                default:
                    return -1;
            }
        }

        public static StatsType GetStatType(int stat)
        {
            switch (stat)
            {
                case 0:
                    return StatsType.MaximumHP;
                case 1:
                    return StatsType.MaximumMP;
                case 2:
                    return StatsType.Attack;
                case 3:
                    return StatsType.Defense;
                case 4:
                    return StatsType.Speed;
                case 5:
                    return StatsType.Dexterity;
                case 6:
                    return StatsType.Vitality;
                case 7:
                    return StatsType.Wisdom;
                case 8:
                    return StatsType.DamageMin;
                case 9:
                    return StatsType.DamageMax;
                case 10:
                    return StatsType.Luck;
                case 11:
                    return StatsType.CriticalDmg;
                case 12:
                    return StatsType.CriticalHit;
                default:
                    return StatsType.None;
            }
        }

        public static StatsType GetBoostStatType(int stat)
        {
            switch (stat)
            {
                case 0:
                    return StatsType.HPBoost;
                case 1:
                    return StatsType.MPBoost;
                case 2:
                    return StatsType.AttackBonus;
                case 3:
                    return StatsType.DefenseBonus;
                case 4:
                    return StatsType.SpeedBonus;
                case 5:
                    return StatsType.DexterityBonus;
                case 6:
                    return StatsType.VitalityBonus;
                case 7:
                    return StatsType.WisdomBonus;
                case 8:
                    return StatsType.DamageMinBonus;
                case 9:
                    return StatsType.DamageMaxBonus;
                case 10:
                    return StatsType.LuckBonus;
                case 11:
                    return StatsType.CriticalDmgBonus;
                case 12:
                    return StatsType.CriticalHitBonus;
                default:
                    return StatsType.None;
            }
        }
    }
}