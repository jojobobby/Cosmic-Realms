using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.behaviors.PetBehaviors;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Misc = () => Behav()//Sir Bagston
            .Init("White Fountain",
                new State(
                    new HealPlayer(30, 1500),
                    new HealPlayerMP(30, 1500)
                )
            )
        .Init("Sir Bagston",
                new State(
                    new PetFollow(),
                    new HealPlayer(8,9000, 45),
                    new HealPlayerMP(8, 9000, 25)
                )
            )
            .Init("Fake White Bag",
                new State(
                    new Decay(5000)
                )
            );
    }
}