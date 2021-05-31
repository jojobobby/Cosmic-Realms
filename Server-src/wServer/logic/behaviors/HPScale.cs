using System.Linq;
using wServer.realm;
using wServer.realm.entities;

// HPScale by GhostMaree
namespace wServer.logic.behaviors
{
    class HPScale : Behavior
    {

        //State storage: scalehp state
        class HPScaleState
        {
           
            public int x = 0;
            public int scaledtimes = 0;
        }


        private readonly int amountPerPlayer;

        public HPScale(int amountPerPlayer)
        {
            this.amountPerPlayer = amountPerPlayer;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new HPScaleState
            {
                x = 0,
                scaledtimes = 0
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            HPScaleState scstate = (HPScaleState)state;
            var entity = host as Character;
            if (scstate.x == 0 || scstate.x < host.Owner.Players.Count())
            {
                if (!(host is Enemy)) return;
                scstate.x = host.Owner.Players.Count;
                if (scstate.x > 0)
                {
                    (host as Enemy).MaximumHP = host.ObjectDesc.MaxHP + (scstate.x * amountPerPlayer);
                    (host as Enemy).HP += (scstate.x - scstate.scaledtimes) * amountPerPlayer;
                    scstate.scaledtimes = scstate.x;

                    entity.ScaledHP = host.ObjectDesc.MaxHP + (scstate.x * amountPerPlayer);
                }
            }
            state = scstate;
        }
    }
}