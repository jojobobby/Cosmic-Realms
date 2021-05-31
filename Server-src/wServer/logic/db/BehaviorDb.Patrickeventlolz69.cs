using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ RainbowRoad2 = () => Behav()//by  ppmaks
            .Init("St. Patricks Event",
                new State(
                    new State("1",
                         new DropPortalOnDeath(target: "Rainbow Road", probability: 1,4545),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("!"),
                        new TimedTransition(4000, "2")
                    ),
                    new State("2",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                         new ChangeSize(-11,75),
                        new StayBack(8,14),
                        new Reproduce("Patty Coin1",1,25,2000)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Small Intense Clovers Cloth", 0.4),
                    new ItemLoot("Large Intense Clovers Cloth", 0.4),
                    new ItemLoot("Small Rainbow Cloth", 0.4),
                    new ItemLoot("Large Rainbow Cloth", 0.4),
                    new ItemLoot("Small Shamrock Cloth", 0.4),
                    new ItemLoot("Large Shamrock Cloth", 0.4),
                    new ItemLoot("St Patrick's Green Accessory Dye", 0.4),
                    new ItemLoot("St Patrick's Green Clothing Dye", 0.4)
                )
            );
    }
}