using System;
using wServer.realm;

namespace wServer.logic.behaviors
{
    public class JumpToRandomOffset : Behavior
    {
        private readonly int minX;
        private readonly int maxX;
        private readonly int minY;
        private readonly int maxY;
        private Random rand = new Random();

        public JumpToRandomOffset(int minX, int maxX, int minY, int maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.Move(host.X + rand.Next(minX, maxX), host.Y + rand.Next(minY, maxY));
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}