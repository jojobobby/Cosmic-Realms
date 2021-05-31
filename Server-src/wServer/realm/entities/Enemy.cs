using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common.resources;
using wServer.logic;
using wServer.networking.packets.outgoing;
using wServer.realm.terrain;
using wServer.realm.worlds;
using wServer.networking.packets;
using wServer.logic.loot;




namespace wServer.realm.entities
{
    public class Enemy : Character
    {
        public World Owner2 { get; private set; }
        public bool isPet; // TODO quick hack for backwards compatibility
        bool stat;
        public Enemy ParentEntity;
        public int Howmany;
        public int Maxhowmany;
        public float SizeBoost;
        public int Rarity;


        DamageCounter counter;
        public Enemy(RealmManager manager, ushort objType)
            : base(manager, objType)
        {
            stat = ObjectDesc.MaxHP == 0;
            counter = new DamageCounter(this);
        }

        public DamageCounter DamageCounter { get { return counter; } }
          
        public WmapTerrain Terrain { get; set; }

        Position? pos;
        public Position SpawnPoint { get { return pos ?? new Position() { X = X, Y = Y }; } }

        public override void Init(World owner)
        {
            var BloodMoonInfo = Program.Config.eventsInfo.BloodMoon;
            base.Init(owner);

            if (ObjectDesc.StasisImmune)
                ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.StasisImmune,
                    DurationMS = -1
                });

            if (!ObjectDesc.Enemy || owner.Id < 0)
                return;

            var rarityChance = RandomUtil.RandInt(0, 100);
            Rarity = 0;
            float size = Size;

            if (rarityChance < 1 || isMoon("legendary")) //legendary - red
            {
                Rarity = 4;
                SetDefaultSize((int)(size * 1.75f));
                HP = (int)(HP * 1.5f);
                SizeBoost = 0.3f;
            } else if (rarityChance < 5 || isMoon("epic")) //epic - purple
            {
                Rarity = 3;
                SetDefaultSize((int)(size * 1.5f));
                HP = (int)(HP * 1.35f);
                SizeBoost = 0.2f;
            }
            else if (rarityChance < 10 || isMoon("rare")) //rare - blue
            {
                Rarity = 2;
                SetDefaultSize((int)(size * 1.25f));
                HP = (int)(HP * 1.25f);
                SizeBoost = 0.1f;
            } else if (rarityChance < 20) //uncommon - gray
            {
                Rarity = 1;
                SetDefaultSize((int)(size * 1.1f));
                HP = (int)(HP * 1.1f);
                SizeBoost = 0.05f;
            }
        }

        private bool isMoon(string type)
        {
            var info = Program.Config.eventsInfo.BloodMoon;
            return info.Item1 && info.Item2 == type;
        }

        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.EnemyRarity) Rarity = (int)val;
            base.ImportStats(stats, val);
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.EnemyRarity] = Rarity;
            base.ExportStats(stats);
        }

        public event EventHandler<BehaviorEventArgs> OnDeath;

        public void Death(RealmTime time)
        {
            try
            {
                counter.Death(time);
                if (CurrentState != null)
                    CurrentState.OnDeath(new BehaviorEventArgs(this, time));
                OnDeath?.Invoke(this, new BehaviorEventArgs(this, time));
                Owner.LeaveWorld(this);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public int Damage(Player from, RealmTime time, int dmg, bool noDef, params ConditionEffect[] effs)
        {

           

            if (stat) return 0;
            if (HasConditionEffect(ConditionEffects.Invincible))
                return 0;
            if (!HasConditionEffect(ConditionEffects.Paused) &&
                !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = this.ObjectDesc.Defense;
                if (noDef)
                    def = 0;

                dmg = (int)StatsManager.GetDefenseDamage(this, dmg, def);

                if (from.CurseEffect())
                {
                    EffectChance(from, 2, 2500, false, ConditionEffectIndex.Curse);
                }
                if (from.ParalyzeEffect())
                {
                    EffectChance(from, 2, 2500, false, ConditionEffectIndex.Paralyzed);
                }
                if (from.TouchEffect())
                {
                    EffectChance(from, 2, 0, true, 0);
                }

                int effDmg = dmg;

                if (effDmg > HP)
                    effDmg = HP;
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;

                ApplyConditionEffect(effs);

                if (from != null)
                {
                    if (!Manager.Resources.Settings.DisableAlly)
                        Owner.BroadcastPacketNearby(new Damage()
                        {
                            TargetId = Id,
                            Effects = 0,
                            DamageAmount = (ushort)dmg,
                            Kill = HP < 0,
                            BulletId = 0,
                            ObjectId = from.Id
                        }, this, null, PacketPriority.Low);
                    else
                        from.Client.SendPacket(new Damage()
                        {
                            TargetId = Id,
                            Effects = 0,
                            DamageAmount = (ushort)dmg,
                            Kill = HP < 0,
                            BulletId = 0,
                            ObjectId = from.Id
                        });

                    counter.HitBy(from, time, null, dmg);
                }
                else
                {
                    Owner.BroadcastPacketNearby(new Damage()
                    {
                        TargetId = Id,
                        Effects = 0,
                        DamageAmount = (ushort)dmg,
                        Kill = HP < 0,
                        BulletId = 0,
                        ObjectId = -1
                    }, this, null, PacketPriority.Low);
                }
                
                if (HP < 0 && Owner != null)
                    Death(time);

                return effDmg;
            }
            return 0;
        }
        
        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (stat) return false;
            if (HasConditionEffect(ConditionEffects.Invincible))
                return false;
            if (projectile.ProjectileOwner is Player p &&
                !HasConditionEffect(ConditionEffects.Paused) &&
                !HasConditionEffect(ConditionEffects.Stasis))
            {
                var def = this.ObjectDesc.Defense;
                if (projectile.ProjDesc.ArmorPiercing)
                    def = 0;
                int dmg = (int)StatsManager.GetDefenseDamage(this, projectile.Damage, def);
                if (!HasConditionEffect(ConditionEffects.Invulnerable))
                    HP -= dmg;
                ApplyConditionEffect(projectile.ProjDesc.Effects);
                if (!Manager.Resources.Settings.DisableAlly)
                    Owner.BroadcastPacketNearby(new Damage()
                    {
                        TargetId = this.Id,
                        Effects = projectile.ConditionEffects,
                        DamageAmount = (ushort)dmg,
                        Kill = HP < 0,
                        BulletId = projectile.ProjectileId,
                        ObjectId = projectile.ProjectileOwner.Self.Id
                    }, this, p, PacketPriority.Low);

                if (p.CurseEffect())
                {
                    EffectChance(p, 2, 2500, false, ConditionEffectIndex.Curse);
                }
                if (p.ParalyzeEffect())
                {
                    EffectChance(p, 2, 2500, false, ConditionEffectIndex.Paralyzed);
                }
                if (p.TouchEffect())
                {
                    EffectChance(p, 2, 0, true, 0);
                }

                counter.HitBy(projectile.ProjectileOwner as Player, time, projectile, dmg);
                if (HasConditionEffect(ConditionEffects.Invincible) || HasConditionEffect(ConditionEffects.Invulnerable)) { }
                else
                    (projectile.ProjectileOwner as Player).LifeManaSteal(this, dmg);
                    
             

                if (HP < 0 && Owner != null)
                {
                    Death(time);
                }
                return true;
            }
            return false;
        }

        public void EffectChance(Player from, int EffChance, int time, bool healing, ConditionEffectIndex effs = 0)
        {
            int HP7PERC = Convert.ToInt32((from.Stats.Base[0] + from.Stats.Boost[0]) * 0.10);
            var Chance = RandomUtil.RandInt(1, 100);
            if (Chance <= EffChance && effs != 0)
            {
                ApplyConditionEffect(effs, time);
            }
            if (Chance <= EffChance && healing == true)
            {
                var pkts = new List<Packet>();
                pkts.Add(new ShowEffect()
                {
                    EffectType = EffectType.Potion,
                    TargetObjectId = from.Id,
                    Color = new ARGB(0xffffffff)
                });
                pkts.Add(new Notification()
                {
                    Color = new ARGB(0xff00ff00),
                    ObjectId = from.Id,
                    Message = "+" + HP7PERC
                });

                from.HP += HP7PERC;
                if (from.HP >= from.Stats[0]){
                    from.HP = from.Stats[0];
                }
            }
        }

        float bleeding = 0;
        public override void Tick(RealmTime time)
        {
            if (pos == null)
                pos = new Position() { X = X, Y = Y };

            if (!stat && HasConditionEffect(ConditionEffects.Bleeding))
            {
                if (bleeding > 1)
                {
                    HP -= (int)bleeding;
                    bleeding -= (int)bleeding;
                }
                bleeding += 28 * (time.ElaspedMsDelta / 1000f);
            }
            base.Tick(time);
        }
    }
}
