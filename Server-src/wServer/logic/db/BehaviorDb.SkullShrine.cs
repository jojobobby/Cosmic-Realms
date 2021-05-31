using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SkullShrine = () => Behav()
            .Init("Skull Shrine",
                new State(
                      new ScaleHP2(80, 3, 15),
                    new Reproduce("Red Flaming Skull", 40, 20, coolDown: 500),
                    new Reproduce("Blue Flaming Skull", 40, 20, coolDown: 500),
                    new ScaleHP2(90,3,15),
                    new DropPortalOnDeath("Lair of Shaitan Portal", 75),

                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Shoot(30, 9, 10, coolDown: 750, predictive: 1),
                    new HpLessTransition(0.70, "Start2")
                       ),
                    new State("Start2",
                    new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(2000, "Start3")
                            ),
                    new State("Start3",
                    new Shoot(30, 9, 10, coolDown: 750, predictive: 1),
                    new HpLessTransition(0.30, "Start4")
                           ),
                    new State("Start4",
                    new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                   new HpLessTransition(0.70, "Start5")
                             ),
                    new State("Start5",
                    new Shoot(30, 9, 10, coolDown: 750, predictive: 1)
                        )
                ),
                    
                new Threshold(0.00001,
                    new TierLoot(8, ItemType.Weapon, .25),
                    new TierLoot(9, ItemType.Weapon, .125),
                    new TierLoot(8, ItemType.Armor, .25),
                    new TierLoot(9, ItemType.Armor, .25),
                    new TierLoot(4, ItemType.Ability, .125),
                    new TierLoot(3, ItemType.Ring, .25),
                    new TierLoot(4, ItemType.Ring, .125),
                    new TierLoot(10, ItemType.Weapon, .0625),
                    new TierLoot(11, ItemType.Weapon, .0625),
                    new TierLoot(10, ItemType.Armor, .125),
                    new TierLoot(11, ItemType.Armor, .125),
                    new TierLoot(12, ItemType.Armor, .0625),
                    new TierLoot(5, ItemType.Ability, .0625),
                    new TierLoot(5, ItemType.Ring, .0625),
                    new ItemLoot("Potion of Defense", .3),
                    new ItemLoot("Potion of Attack", .3),
                    new ItemLoot("Potion of Vitality", .3),
                    new ItemLoot("Potion of Wisdom", .3),
                    new ItemLoot("Potion of Speed", .3),//Shard of Fire
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Shard of Fire", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Scorchium Stone", 0.004, damagebased: true),
                    new ItemLoot("Molten Mantle", 0.006, damagebased: true),
                    new ItemLoot("Potion of Dexterity", .3),
                    new ItemLoot("Orb of Conflict", .004, damagebased: true)
                    )
            )
            .Init("Red Flaming Skull",
                new State(
                    new State("Orbit Skull Shrine",
                        new Prioritize(
                            new Protect(.3, "Skull Shrine", 30, 15, 15),
                            new Wander(.3)
                            ),
                        new EntityNotExistsTransition("Skull Shrine", 40, "Wander")
                        ),
                    new State("Wander",
                        new Wander(.3)
                        ),
                    new Shoot(12, 2, 10, coolDown: 750)
                    )
            )
            .Init("Blue Flaming Skull",
                new State(
                    new State("Orbit Skull Shrine",
                        new Orbit(1.5, 15, 40, "Skull Shrine", .6, 10, orbitClockwise: false),
                        new EntityNotExistsTransition("Skull Shrine", 40, "Wander")
                        ),
                    new State("Wander",
                        new Wander(1.5)
                        ),
                    new Shoot(12, 2, 10, coolDown: 750)
                    )
            );
    }
}