using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ RockDragon = () => Behav()
            .Init("Dragon Head",
                new State(
                    new ScaleHP2(80, 2, 15),
                    new State("Attack",
                        new SetAltTexture(1, 6, 500, true),
                        new Prioritize(
                            new StayCloseToSpawn(2, 20),
                            new Wander(1.5)
                            ),
                        new Shoot(10, count: 1, projectileIndex: 2, coolDown: new Cooldown(2000, 500)),
                        new Shoot(20, count: 5, fixedAngle: 360 / 5, projectileIndex: 3, coolDown: 2000),
                        new Shoot(20, count: 5, fixedAngle: 360 / 5, projectileIndex: 3, coolDown: 2000, coolDownOffset: 500)
                        )
                    ),
                new Threshold(0.0001,
                    new TierLoot(8, ItemType.Weapon, .15),
                    new TierLoot(9, ItemType.Weapon, .1),
                    new TierLoot(10, ItemType.Weapon, .07),
                    new TierLoot(11, ItemType.Weapon, .05),
                    new TierLoot(4, ItemType.Ability, .15),
                    new TierLoot(5, ItemType.Ability, .07),
                    new TierLoot(8, ItemType.Armor, .2),
                    new TierLoot(9, ItemType.Armor, .15),
                    new TierLoot(10, ItemType.Armor, .10),
                    new TierLoot(11, ItemType.Armor, .07),
                    new TierLoot(12, ItemType.Armor, .04),
                    new TierLoot(5, ItemType.Ring, .03),
                    new ItemLoot("Potion of Critical Chance", 0.1),
                    new ItemLoot("Potion of Critical Damage", 0.1),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Vitality", .02),
                    new ItemLoot("Daybreak Tessen", .004, damagebased: true),
                    new ItemLoot("Samurai's Plated Annihilation", .01, damagebased: true),//Daybreak Tessen
                    new ItemLoot("Ray Katana", .004, threshold: 0.01, damagebased: true)
                    )
            )
            .Init("Rock Dragon Bat", // All Texture 1 ? | 0->3 Proj
                new State(
                    new State("Normal",
                        new Prioritize(
                            new Follow(0.6, 10, 3),
                            new Wander(0.4)
                            ),
                        new Shoot(10, count: 3, shootAngle: 7, coolDown: new Cooldown(1200, 200), projectileIndex: 3),
                        new Shoot(7, count: 1, projectileIndex: 0, coolDown: 1500),
                        new Shoot(7, count: 1, projectileIndex: 1, coolDown: 1500, coolDownOffset: 100),
                        new Shoot(7, count: 1, projectileIndex: 2, coolDown: 1500, coolDownOffset: 200),
                        new PlayerWithinTransition(2, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, fixedAngle: 36, projectileIndex: 0),
                        new Suicide()
                        )
                    )
            )
            .Init("Dragon Head Spawner", //Spawn Dragon
                new State(
                    new DropPortalOnDeath("Lair of Draconis Portal", 0.5),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Spawn",
                        new Spawn("Dragon Head", 1, 1, 10000, givesNoXp: false),
                        new TimedTransition(1000, "Wait")
                        ),
                    new State("Wait")
                    )
            )
            .Init("Dragon Marker BR", //  Maybe for Movement And Spawn of Bat
                new State(
                    new State("Spawn",
                        new Reproduce("Rock Dragon Bat", 100, 6, 4000),
                        new EntityNotExistsTransition("Dragon Head", 100, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Dragon Marker BL", //  Maybe for Movement And Spawn of Bat
                new State(
                    new State("Spawn",
                        new Reproduce("Rock Dragon Bat", 100, 6, 4000),
                        new EntityNotExistsTransition("Dragon Head", 100, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Dragon Marker TR", //  Maybe for Movement And Spawn of Bat
                new State(
                    new State("Spawn",
                        new Reproduce("Rock Dragon Bat", 100, 6, 4000),
                        new EntityNotExistsTransition("Dragon Head", 100, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Dragon Marker TL", //   Maybe for Movement And Spawn of Bat
                new State(
                    new State("Spawn",
                        new Reproduce("Rock Dragon Bat", 100, 6, 4000),
                        new EntityNotExistsTransition("Dragon Head", 100, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            ;
    }
}