using System;
using System.Collections.Generic;
using Mono.Game;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities
{
    class Decoy : StaticObject, IPlayer
    {
        static Random rand = new Random();

        Player player;
        public int decoyduration;
        Vector2 direction;
        float speed;
        public Position target1;

        Vector2 GetRandDirection()
        {
            double angle = rand.NextDouble() * 2 * Math.PI;
            return new Vector2(
                (float)Math.Cos(angle),
                (float)Math.Sin(angle)
            );
        }
        public Decoy(Player player, int duration, float tps, Position target2)
            : base(player.Manager, 0x0715, duration, true, true, true)
        {
            player.DecoyStillActive = true;
            this.player = player;
            this.decoyduration = duration;
            this.speed = 1;
            player.timeDecoy = duration;
            target1 = target2;
            direction = new Vector2(target2.X, target2.Y);
            direction.Normalize();
            
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.Texture1] = player.Texture1;
            stats[StatsType.Texture2] = player.Texture2;
            base.ExportStats(stats);
        }

        public bool exploded = false;
        public override void Tick(RealmTime time)
        {
            if (target1.X != player.Xdecoy && player.Xdecoy != 0 || target1.Y != player.Ydecoy && player.Ydecoy != 0)
            {
                this.ValidateAndMove(
                player.Xdecoy,
                player.Ydecoy
            );
            }
           /* if (HP < decoyduration)
            {
                this.ValidateAndMove(
                    direction.X * time.ElaspedMsDelta / 1000,
                    direction.Y * time.ElaspedMsDelta / 1000
                );
            }*/
            if (HP < 250 && !exploded)
            {
                exploded = true;
                Owner.BroadcastPacketNearby(new ShowEffect()
                {
                    EffectType = EffectType.AreaBlast,
                    Color = new ARGB(0xffff0000),
                    TargetObjectId = Id,
                    Pos1 = new Position() { X = 1 }
                }, this, null, PacketPriority.Low);
            }
            base.Tick(time);
        }

        public void Damage(int dmg, Entity src) { }

        public bool IsVisibleToEnemy() { return true; }
    }
}
