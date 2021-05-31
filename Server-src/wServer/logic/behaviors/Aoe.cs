using System;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class Aoe : Behavior
    {
        //State storage: nothing

        private readonly float radius;
        private readonly bool players;
        private readonly int minDamage;
        private readonly int maxDamage;
        private readonly bool noDef;
        private readonly ARGB color;

        public Aoe(double radius, bool players, int minDamage, int maxDamage, bool noDef, uint color)
        {
            this.radius = (float)radius;
            this.players = players;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.noDef = noDef;
            this.color = new ARGB(color);
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            var pos = new Position
            {
                X = host.X,
                Y = host.Y
            };

            var damage = Random.Next(minDamage, maxDamage);

            host.Owner.AOE(pos, radius, players, enemy =>
            {
                if (!players)
                {
                    if (enemy is Enemy)
                        (enemy as Enemy).Damage(null, time, damage, noDef);

                    host.Owner.BroadcastPacketNearby(new ShowEffect()
                    {
                        EffectType = EffectType.AreaBlast,
                        TargetObjectId = host.Id,
                        Pos1 = new Position { X = radius, Y = 0 },
                        Color = color
                    }, host, null, PacketPriority.Low);
                }
                else
                {
                    host.Owner.BroadcastPacketNearby(new networking.packets.outgoing.Aoe
                    {
                        Pos = pos,
                        Radius = radius,
                        Damage = (ushort)damage,
                        Duration = 0,
                        Effect = 0,
                        OrigType = host.ObjectType,
                        ObjectName = host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId

                    }, host, null, PacketPriority.Low);
                }
            });
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}