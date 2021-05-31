using wServer.realm;

namespace wServer.logic.transitions
{
    class RandomTransitionSpawner : Transition
    {
        private double _probability;
        private bool _activated;

        public RandomTransitionSpawner(double probability, params string[] states)
            : base(states)
        {
            _probability = probability;
            _activated = false;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (_activated) return false;

            _activated = false;

            SelectedState = 0;
            return Random.NextDouble() < _probability;
        }
    }
}