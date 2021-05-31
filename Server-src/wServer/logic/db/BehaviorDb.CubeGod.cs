using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CubeGod = () => Behav()
            .Init("Cube God",
                new State(
                    new ScaleHP2(90, 2, 15),
                     new Wander(.2),
                     new Reproduce("Cube Overseer", 0, 2, coolDown: 1500),
                    new StayAbove(0.2, 150),
                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2500),
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500),
                    new HpLessTransition(.70, "Start2")
                          ),
                    new State("Start2",
                    new Flash(0xffffff, 2, 4),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start3")
                        ),
                    new State("Start3",
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500),
                     new HpLessTransition(.30, "Start4")
                          ),
                    new State("Start4",
                    new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start5")
                          ),
                    new State("Start5",
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500)
                        )
                ),
                new Threshold(.0001,
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
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Potion of Critical Chance", 0.75),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("Dirk of Cronus", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Royal Glutinous Hide", .01, damagebased: true),
                    new ItemLoot("Necklace of Magic", 0.0006, damagebased: true, threshold: 0.02)
                )
            )
            .Init("Cube Overseer",
                new State(
                    new EntityNotExistsTransition("Cube God", 30, "dead"),
                    new Prioritize(
                        new Orbit(.25, 10, 30, "Cube God", .075, 5),
                        new Wander(.375)
                           ),
                      new State("dead",
                          new Suicide()
                        ),
                    new Reproduce("Cube Defender", 30, 6, coolDown: 10000),
                    new Reproduce("Cube Blaster", 30, 6, coolDown: 10000),
                    new Shoot(10, 4, 10, 0, coolDown: 750),
                    new Shoot(10, projectileIndex: 1, coolDown: 1500)
                )
            )
            .Init("Cube Defender",
                new State(
                    new EntityNotExistsTransition("Cube God", 30, "dead"),
                    new Prioritize(
                        new Orbit(1.05, 5, 15, "Cube Overseer", .15, 3),
                        new Wander(1.05)
                    ),
                      new State("dead",
                          new Suicide()
                          ),
                    new Shoot(10, coolDown: 500)
                )
            )
            .Init("Cube Blaster",
                new State(
                     new EntityNotExistsTransition("Cube God", 30, "dead"),
                    new State("Orbit",
                        new Prioritize(
                            new Orbit(1.05, 7.5, 40, "Cube Overseer", .15, 3),
                            new Wander(1.05)
                               ),
                      new State("dead",
                          new Suicide()
                        ),
                        new EntityNotExistsTransition("Cube Overseer", 10, "Follow")
                    ),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.75, 10, 1, 5000),
                            new Wander(1.05)
                        ),
                        new EntityNotExistsTransition("Cube Defender", 10, "Orbit"),
                        new TimedTransition(5000, "Orbit")
                    ),
                    new Shoot(10, 2, 10, 1, predictive: 1, coolDown: 500),
                    new Shoot(10, predictive: 1, coolDown: 1500)
                )
            );
    }
}