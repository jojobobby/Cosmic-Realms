using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ RainbowRoad = () => Behav()//by  ppmaks
            .Init("St. Patricks Event Chest",
                new State(
                    new ScaleHP2(50, 3, 15),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(4, "2")
                    ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 4000)
                    )
                ),
                new Threshold(0.0001,
                    new ItemLoot("Small Intense Clovers Cloth", 0.4),
                    new ItemLoot("Large Intense Clovers Cloth", 0.4),
                    new ItemLoot("Small Rainbow Cloth", 0.4),
                    new ItemLoot("Large Rainbow Cloth", 0.4),
                    new ItemLoot("Small Shamrock Cloth", 0.4),
                    new ItemLoot("Large Shamrock Cloth", 0.4),
                    new ItemLoot("St Patrick's Green Accessory Dye", 0.4),
                    new ItemLoot("St Patrick's Green Clothing Dye", 0.4),
                    new ItemLoot("Miss Shamrock Wizard Skin", 0.02),
                    new ItemLoot("Lucky Clover", 0.05, damagebased: true),
                    new ItemLoot("Potion of Speed"),
                    new ItemLoot("Saint Patty's Brew", 0.4, damagebased: true),
                    new ItemLoot("Sword of the Rainbow's End", 0.006, damagebased: true),
                    new ItemLoot("Clover Bow", 0.006, damagebased: true)
                )
            );
    }
}