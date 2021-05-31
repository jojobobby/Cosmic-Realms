using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.transitions
{
    class EntityHpLessTransition : Transition
    {
        //State storage: none

        private readonly double Dist;
        private readonly string Entity;
        private readonly double Threshold;

        public EntityHpLessTransition(double dist, string entity, double threshold, string targetState) : base(targetState)
        {
            Dist = dist;
            Entity = entity;
            Threshold = threshold;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            var entity = host.GetNearestEntityByName(Dist, Entity);
            if (entity == null)
                return false;
            return ((double)(entity as Enemy).HP / (host as Enemy).MaximumHP) <= Threshold;
        }
    }
}
