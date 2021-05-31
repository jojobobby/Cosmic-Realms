#region

using wServer.logic.behaviors;
using wServer.logic.loot;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ BlueDemon = () => Behav()
        /*    .Init("Blue Demon",
                new State(
                    new Shoot(10, projectileIndex: 0, count: 5, shootAngle: 5, predictive: 1, coolDown: 1200),
                    new Shoot(11, projectileIndex: 1, coolDown: 1400),
                    new Prioritize(
                        new StayCloseToSpawn(0.8, 5),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Taunt(0.7, 10000,
                        "I will deliver your soul to my master!, {PLAYER}!",
                        "The ocean calls your name, {PLAYER}!",
                        "Your life is an affront to the Angler. You will die!"
                        )
                    ),
                new MostDamagers(30,
                new ItemLoot("Golden Sword", 0.07),
                new ItemLoot("Living Ocean Bow", 0.015),
                new ItemLoot("Steel Shield", 0.06),
                new ItemLoot("Ring of Greater Defense", 0.05),
                new ItemLoot("Potion of Speed", 0.05),
                new ItemLoot("Steel Helm", 0.04),
                new ItemLoot("Fire Bow", 0.07)
            )
            )
           */
            ;
    }
}