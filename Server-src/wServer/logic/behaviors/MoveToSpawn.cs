using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    public class MoveToSpawn : Behavior
    {
        public MoveToSpawn()
        {
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            if (!(host is Enemy)) return;
            var spawn = (host as Enemy).SpawnPoint;
            host.Move(spawn.X, spawn.Y);
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}