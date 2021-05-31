using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ DeepSea = () => Behav()
              .Init("Giant Carukia barnesi",
                new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                    new ScaleHP2(40,3,15),
                    new HpLessTransition(0.10, "death1"),
                    new State(
                        new State("default",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new TimedTransition(1000, "default1")
                        ),
                        new State("default1",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Flash(0xFFFFFF, 2, 2),
                            new TimedTransition(6000, "default2")
                        ),
                        new State("default2",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt("The water will not fear you anymore!", "Drown in the fear of the sea!.", "You do not seem worthy enough to enter these depths."),
                            new TimedTransition(4000, "default3")
                        ),
                        new State("default3",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt("Leave my sea at once!", "You may never enter this sea, Leave if you wish to live!"),
                            new TimedTransition(3000, "startup")
                        )
                    ),
                    new State("startup",
                        new Taunt(true, "I have gathered the power of Thessal after her death, you will pay!"),
                        new SetAltTexture(0),
                         new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(10, 38, projectileIndex: 2, coolDown: 9999),
                        new TimedTransition(4000, "spawnhelpers")
                    ),
                    new State("spawnhelpers",
                        new Shoot(10, 14, shootAngle: 4, projectileIndex: 5, predictive: 2, coolDown: 2000),
                        new TimedTransition(3000, "begin")
                    ),
                    new State("begin",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(3000, "StartWaves1")
                    ),
                    new State(
                        new TimedTransition(8000, "WeirdMovement1"),
                        new State("StartWaves1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 90, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 180, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 270, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 0, projectileIndex: 0, coolDown: 400),
                            new TimedTransition(800, "StartWaves2")
                        ),
                        new State("StartWaves2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 45, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 135, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 225, projectileIndex: 0, coolDown: 400),
                            new Shoot(10, count: 7, shootAngle: 2, fixedAngle: 315, projectileIndex: 0, coolDown: 400),
                            new TimedTransition(800, "StartWaves1")
                        )
                    ),
                    new State(
                        new Shoot(10, 8, projectileIndex: 1, coolDown: 2000),

                        new State(
                            new TimedTransition(24000, "Return"),
                            new State("WeirdMovement1", 
                                new TimedTransition(4000, "Flash")
                            ),
                            new State("Flash",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Flash(0xFF0000, 1, 1),
                                new TimedTransition(3000, "WeirdMovement2")
                            ),
                            new State("WeirdMovement2",
                                new Grenade(1, 150, range: 8, coolDown: 1000),
                                new Shoot(10, count: 2, shootAngle: 18, projectileIndex: 3, coolDown: 1000),
                                new Shoot(10, count: 8, shootAngle: 8, projectileIndex: 4, coolDown: 200),
                                new TimedTransition(4000, "fast")
                            )
                        ),
                        new State(
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new State("Return",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new ReturnToSpawn(speed: 1.2),
                                new TimedTransition(2000, "Reform")
                            ),

                            new State("Reform",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new SetAltTexture(1),
                                new TimedTransition(0, "checker")
                            ),
                            new State("checker",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new EntitiesNotExistsTransition(20, "Ongo", "Split Yazanahar")
                            )
                        ),
                        new State("Ongo",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Prioritize(
                                new StayCloseToSpawn(0.5, 3),
                                new Wander(0.05)
                            ),
                            new Grenade(2, 400, range: 8, coolDown: 400),
                            new Shoot(10, 6, projectileIndex: 1, coolDown: 4000),
                            new Shoot(10, 10, shootAngle: 10, projectileIndex: 5, coolDown: 5000),
                            new Shoot(10, 5, shootAngle: 4, projectileIndex: 2, coolDown: 1600),
                            new TimedTransition(8000, "fast")
                        ),
                        new State("fast",
                            new Taunt("I will wash you away, {PLAYER}"),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new SetAltTexture(0),
                            new Prioritize(
                                new Orbit(0.7, 2, target: null)
                            ),
                            new Shoot(10, 20, projectileIndex: 2, coolDown: 2000),
                            new Shoot(10, 7, shootAngle: 12, predictive: 1, projectileIndex: 0, coolDown: 6000),
                            new Shoot(10, 5, shootAngle: 4, projectileIndex: 2, coolDown: 1600),
                            new Shoot(10, 10, shootAngle: 4, projectileIndex: 4, coolDown: 2900),
                            new TimedTransition(15000, "Return2")
                        ),
                        new State("Return2",
                            new Flash(0x000000, 2, 2),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ReturnToSpawn(speed: 1.2),
                            new TimedTransition(6000, "DropDown")
                        ),
                        new State("DropDown",
                            new HealSelf(coolDown: 6000, amount: 1000),
                            new Shoot(10, 6, shootAngle: 22, projectileIndex: 6, coolDown: 1600),
                            new Shoot(10, 3, shootAngle: 8, projectileIndex: 2, coolDown: 400),
                            new Shoot(10, 9, shootAngle: 8, projectileIndex: 4, predictive: 1, coolDown: 400, coolDownOffset: 800),
                            new Taunt("I guard this sea!", "Remove yourselves."),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new TimedTransition(10000, "tttt")
                        ),
                        new State("tttt",
                            new Taunt("Thessal did not die for nothing!"),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new TimedTransition(4000, "swagche")
                        ),
                        new State("swagche",
                            new HealSelf(coolDown: 1000, amount: 300),
                            new Shoot(10, 24, projectileIndex: 5, coolDown: 6000),
                            new Shoot(10, 4, projectileIndex: 4, predictive: 1, shootAngle: 12, coolDown: 100),
                            new Shoot(10, 1, projectileIndex: 6, coolDown: 100),
                            new Shoot(10, 1, projectileIndex: 6, coolDown: 100, coolDownOffset: 100),
                            new TimedTransition(9000, "startup")
                        ),
                        new State("death1",
                            new Taunt(true, "My Sea..."),
                            new Flash(0x000055, 1, 1),
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new ReturnToSpawn(speed: 2),
                            new TimedTransition(8600, "death")
                        ),
                        new State("death",
                            new Suicide()
                        )
                    )

                    ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Mana", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.02),
                    new TierLoot(11, ItemType.Weapon, 0.4),
                    new TierLoot(6, ItemType.Ability, 0.3),
                    new TierLoot(11, ItemType.Armor, 0.2),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(12, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Ring, 0.025),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Deep Blue Shoulder Pads", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Dagger of Skinned Ocean", 0.006, damagebased: true),
                    new ItemLoot("Awoken Dagger of Skinned Ocean", 0.002, damagebased: true, threshold: 0.01),
                    new ItemLoot("Awoken Sword of Ocean Waves", 0.002, damagebased: true, threshold: 0.01),
                    new ItemLoot("Sword of Ocean Waves", 0.004, damagebased: true)
                    )
            );
    }
}