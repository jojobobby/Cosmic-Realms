using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Testing = () => Behav()
            .Init("XP Gift G",
                new State(
                    new Wander(speed: 0.3)
                ),
                new Threshold(0.01,
                    new TierLoot(tier: 8, type: ItemType.Weapon, probability: 0.15),
                    new TierLoot(tier: 9, type: ItemType.Armor, probability: 0.15),
                    new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.15),
                    new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.15)
                )
            )

            .Init("Jeebs",
                new State(
                    new State("spawn",
                        new Spawn(children: "XP Gift G", maxChildren: 5, initialSpawn: 0, givesNoXp: false),
                        new RandomTransition(0.01, "eventchest"),
                        new TimedTransition(time: 4000, targetState: "checka")
                        ),
                    new State("checka",
                        new EntitiesNotExistsTransition(99, "spawn", "XP Gift G")
                        ),
                    new State("eventchest",
                        new TossObject(child: "Event Chest", range: 16),
                        new TimedTransition(time: 500, targetState: "spawn")
                        )
                )
            )
            .Init("Event Chest",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.00001,
                    new ItemLoot("Potion of Defense", 0.3),
                    new ItemLoot("Potion of Attack", 0.3),
                    new ItemLoot("Potion of Vitality", 0.3),
                    new ItemLoot("Potion of Wisdom", 0.3),
                    new ItemLoot("Potion of Speed", 0.3),
                    new ItemLoot("Potion of Dexterity", 0.3),
                    new ItemLoot("Potion of Mana", 0.3),
                    new ItemLoot("Potion of Life", 0.3),
                    new ItemLoot("Quest Chest Item", 0.05),
                    new ItemLoot("Enforcer", 0.0006),
                    new TierLoot(tier: 13, type: ItemType.Armor, probability: 0.03125),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.0625),
                    new TierLoot(tier: 12, type: ItemType.Weapon, probability: 0.03125),
                    new TierLoot(tier: 11, type: ItemType.Weapon, probability: 0.0625),
                    new TierLoot(tier: 6, type: ItemType.Ability, probability: 0.03125),
                    new TierLoot(tier: 5, type: ItemType.Ability, probability: 0.0625),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.03125),
                    new TierLoot(tier: 5, type: ItemType.Ring, probability: 0.0625)
                )
            )
        ;
    }
}
