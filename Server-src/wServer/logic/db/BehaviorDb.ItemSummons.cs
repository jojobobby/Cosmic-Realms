using wServer.logic.behaviors;
using wServer.logic.loot;
using common.resources;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ItemSummons = () => Behav()

              
               .Init("Marble Seal Assistant",
                new State(
                    new State("Summon",
                    new HealPlayer(20, 1000, 50),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    //new Follow(1.5, 10, 3),
                        new TimedTransition(5000,"Destroy")
                        ),
                    new State("Destroy",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Suicide()
                    )
            )
            )
            ;

    }
}