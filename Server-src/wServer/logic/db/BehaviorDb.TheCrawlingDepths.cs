using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TheCrawlingDepths = () => Behav()
            .Init("Son of Arachna",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                        new PlayerWithinTransition(9, "MakeWeb", true)
                    ),
                    new State("MakeWeb",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new TossObject("Epic Arachna Web Spoke 1", range: 10, angle: 0, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 7", range: 6, angle: 0, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 2", range: 10, angle: 60, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 3", range: 10, angle: 120, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 8", range: 6, angle: 120, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 4", range: 10, angle: 180, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 5", range: 10, angle: 240, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 9", range: 6, angle: 240, coolDown: 100000, throwEffect: true),
                        new TossObject("Epic Arachna Web Spoke 6", range: 10, angle: 300, coolDown: 100000, throwEffect: true),
                        new TimedTransition(3500, "Attack")
                    ),
                    new State("Attack",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new StayCloseToSpawn(1, 12),
                        new Shoot(0, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                        new Shoot(10, projectileIndex: 1, coolDown: 3000, predictive: 1),
                        new Shoot(0, projectileIndex: 5, count: 7, coolDown: 3000, coolDownOffset: 4000),
                        new Shoot(25, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 4900),
                        new Shoot(25, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 6000),
                        new Shoot(25, projectileIndex: 2, count: 7, coolDown: 3000, coolDownOffset: 6000),
                        new State("Follow",
                            new Prioritize(
                                new StayAbove(.6, 1),
                                new StayBack(.6, distance: 8),
                                new Wander(.7)
                            )
                        ),
                        new EntitiesNotExistsTransition(50, "Attack2", "Yellow Son of Arachna Giant Egg Sac", "Red Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", "Silver Son of Arachna Giant Egg Sac")
                    ),
                    new State("Attack2",
                        new StayCloseToSpawn(1, 12),
                        new Shoot(0, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                        new Shoot(10, projectileIndex: 1, coolDown: 3000, predictive: 1),
                        new StayCloseToSpawn(1, 12),
                        new Wander(.7)
                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Greater Potion of Mana", 0.3, 3),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Lantern", 0.05),
                    new ItemLoot("Doku No Ken", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Symbiotic Ripper", 0.006, damagebased: true),
                    new ItemLoot("Parasitic Concoction", 0.006, damagebased: true),
                     new ItemLoot("Kiritsukeru", 0.006, damagebased: true),
                    new ItemLoot("Reinforced Root Hide", 0.006, damagebased: true),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of the Son of Arachna", 1)
                )
            )
            .Init("Epic Arachna Web Spoke 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(0, count: 1, fixedAngle: 180, coolDown: 150),
                    new Shoot(0, count: 1, fixedAngle: 120, coolDown: 150),
                    new Shoot(0, count: 1, fixedAngle: 240, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 2",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 3",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 4",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 5",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 6",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 7",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 180, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 8",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 360, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 240, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 300, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
           .Init("Epic Arachna Web Spoke 9",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(200, count: 1, fixedAngle: 0, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 60, coolDown: 150),
                    new Shoot(200, count: 1, fixedAngle: 120, coolDown: 150),
                    new State("1",
                        new EntitiesNotExistsTransition(50, "2", "Son of Arachna")
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
            .Init("Blue Son of Arachna Giant Egg Sac",
                new State(
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 5),
                    new TransformOnDeath("Blue Egg Summoner")
                ),
                new Threshold(0.01,
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Wisdom", 0.3)
                )
            )
            .Init("Crawling Spider Hatchling",
                new State(
                    new Wander(.4),
                    new Shoot(7, count: 1, shootAngle: 0, coolDown: 1300, coolDownOffset: 650),
                    new Shoot(7, count: 1, shootAngle: 0, projectileIndex: 1, predictive: 1, coolDown: 1300, coolDownOffset: 1300)
                )
            )
            .Init("Red Son of Arachna Giant Egg Sac",
                new State(
                    new TransformOnDeath("Crawling Red Spotted Spider", 3, 3),
                    new TransformOnDeath("Red Egg Summoner")
                ),
                new Threshold(0.01,
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Vitality", 0.3)
                )
            )
            .Init("Crawling Red Spotted Spider",
                new State(
                    new Wander(.4),
                    new Shoot(8, count: 1, shootAngle: 0, coolDown: 750)
                )
            )
            .Init("Silver Son of Arachna Giant Egg Sac",
                new State(
                    new TransformOnDeath("Crawling Grey Spider", 3, 3),
                    new TransformOnDeath("Silver Egg Summoner")
                ),
                new Threshold(0.01,
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Wisdom", 0.3),
                    new ItemLoot("Potion of Defense", 0.3)
                )
            )
            .Init("Crawling Grey Spider",
                new State(
                    new Prioritize(
                        new Charge(2, 8, 5000),
                        new Wander(.4)
                    ),
                    new Shoot(9, count: 1, shootAngle: 0, coolDown: 850)
                )
            )
            .Init("Yellow Son of Arachna Giant Egg Sac",
                new State(
                    new TransformOnDeath("Yellow Egg Summoner")
                ),
                new Threshold(0.01,
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Wisdom", 0.3),
                    new ItemLoot("Potion of Dexterity", 0.3)
                )
            )
            .Init("Crawling Green Spider",
                new State(
                    new Prioritize(
                        new Follow(.6, 11, 1),
                        new Wander(.4)
                    ),
                    new Shoot(8, count: 3, shootAngle: 10, coolDown: 400)
                )
            )
        ;
    }
}