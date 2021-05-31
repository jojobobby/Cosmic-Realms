using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PermafrostLord = () => Behav()
            .Init("Permafrost Lord",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath("Frozen Depths Portal"),
                    new State("1",
                        new Wander(0.3),
                        new Shoot(14, 7, 12.8, 8, coolDown: 1000),
                        new HpLessTransition(0.8, "2")
                    ),
                    new State("2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt("Ice, sorrow, darkness!"),
                        new Wander(0.3),
                        new Shoot(14, 6, 60, 1, coolDown: 1000),
                        new Shoot(14, 6, 60, 2, coolDown: 1000),
                        new Shoot(14, 6, 60, 3, coolDown: 1000),
                        new HpLessTransition(0.6, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt("The lands will be painted white!"),
                        new Wander(0.3),
                        new Shoot(0, 9, 40, 8, 0, 20, coolDown: 1000),
                        new HpLessTransition(0.4, "4")
                    ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ReturnToSpawn(1),
                        new Taunt("With the winter’s might!"),
                        new Shoot(0, 4, 90, 7, 0, 45, coolDown: 1000),
                        new Shoot(0, 1, 0, 4, 0, 45, coolDown: 1000),
                        new Shoot(0, 1, 0, 5, 0, 45, coolDown: 1000),
                        new Shoot(0, 1, 0, 6, 0, 45, coolDown: 1000),
                        new HpLessTransition(0.2, "5")
                    ),
                    new State("5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Flash(0xFF0000, 10, 6000),
                        new Taunt("Enough!"),
                        new Prioritize(
                            new Follow(0.5, 10, 1),
                            new Wander(0.3)
                        ),
                        new Shoot(14, 3, 45, 1, coolDown: 1000),
                        new Shoot(14, 3, 45, 2, coolDown: 1000),
                        new Shoot(14, 3, 45, 3, coolDown: 1000),
                        new Shoot(14, 2, 22.5, 8, coolDown: 1000),
                        new Shoot(0, 4, 90, 7, 0, 45, coolDown: 1000)
                    )
                ),
                new Threshold(0.0001,
                    new ItemLoot(item: "Potion of Defense", probability: 0.3),
                    new ItemLoot(item: "Potion of Attack", probability: 0.3),
                    new ItemLoot(item: "Potion of Vitality", probability: 0.3),
                    new ItemLoot(item: "Potion of Wisdom", probability: 0.3),
                    new ItemLoot(item: "Potion of Speed", probability: 0.3),
                    new ItemLoot(item: "Potion of Dexterity", probability: 0.3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot(item: "Frost Citadel Armor", probability: 0.0045),
                    new ItemLoot(item: "Frost Drake Hide Armor", probability: 0.0045),
                    new ItemLoot(item: "Frost Elementalist Robe", probability: 0.0045),
                    new ItemLoot(item: "The Frost Orb", probability: 0.006, damagebased: true),
                    new ItemLoot(item: "Ice King's Frozen Shuriken", probability: 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Frozen Ice Shard", 0.0045, damagebased: true, threshold: 0.01)//
                    )
            );
    }
}