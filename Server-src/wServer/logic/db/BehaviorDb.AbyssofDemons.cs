using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ AbyssofDemons = () => Behav()
            .Init("Malphas Protector",
                new State(
                    new Shoot(radius: 5, count: 3, shootAngle: 5, projectileIndex: 0, predictive: 0.45, coolDown: 1200),
                    new Orbit(speed: 1.2, radius: 6, acquireRange: 20, target: "Archdemon Malphas", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    ),
                new Threshold(0.01,
                    new ItemLoot(item: "Magic Potion", probability: 0.06),
                    new ItemLoot(item: "Health Potion", probability: 0.04)
                    )
            )
            .Init("Malphas Missile",
                new State(
                    new State("Start",
                        new TimedTransition(time: 50, targetState: "Attacking")
                        ),
                    new State("Attacking",
                        new Follow(speed: 0.8, acquireRange: 10, range: 0.2),
                        new PlayerWithinTransition(dist: 1.3, targetState: "FlashBeforeExplode"),
                        new TimedTransition(time: 5000, targetState: "FlashBeforeExplode")
                        ),
                    new State("FlashBeforeExplode",
                        new Flash(color: 0xFFFFFF, flashPeriod: 0.1, flashRepeats: 6),
                        new TimedTransition(time: 600, targetState: "Explode")
                        ),
                    new State("Explode",
                        new Shoot(radius: 0, count: 8, shootAngle: 45, projectileIndex: 0, fixedAngle: 0),
                        new Suicide()
                        )
                    )
            )
            .Init("Archdemon Malphas",
                new State(
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new State("start_the_fun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new MoveLine(1, 315, 0.5),
                        new PlayerWithinTransition(dist: 11, targetState: "he_is_never_alone", seeInvis: true)
                    ),
                    new State("he_is_never_alone",
                        new State("Missile_Fire",
                            new Shoot(0, 3, 120, 0, 0, 10, coolDown: 250),
                            new Reproduce(children: "Malphas Missile", densityRadius: 24, densityMax: 4, coolDown: 1800),
                            new State("invulnerable",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(time: 2000, targetState: "vulnerable")
                            ),
                            new State("vulnerable",
                                new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(time: 2000, targetState: "invulnerable")
                            ),
                            new HpLessTransition(0.8, "Pause1")
                        ),
                        new State("Pause1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new TimedTransition(time: 2500, targetState: "Small_target")
                        ),
                        new State("Small_target",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Reproduce(children: "Malphas Missile", densityRadius: 24, densityMax: 4, coolDown: 1800),
                          new Shoot(10, 12, projectileIndex: 2, coolDown: 3000),
                          new Shoot(10, 3, fixedAngle: 0, rotateAngle: 10, projectileIndex: 0, coolDownOffset: 0, coolDown: 200),
                            new HpLessTransition(0.6, "Size_matters")
                        ),
                        new State("Size_matters",
                            new Reproduce(children: "Malphas Missile", densityRadius: 24, densityMax: 4, coolDown: 1800),
                            new State("Growbig",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(time: 1800, targetState: "Shot_rotation1")
                            ),
                            new State("Shot_rotation1",
                                new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(10, 3, fixedAngle: 0, rotateAngle: 10, projectileIndex: 0, coolDownOffset: 0, coolDown: 200),
                                new Shoot(radius: 24, count: 3, shootAngle: 15, projectileIndex: 2, coolDown: 600),
                                new TimedTransition(time: 750, targetState: "Shot_rotation2")
                            ),
                            new State("Shot_rotation2",
                                new Wander(0.1),
                                new Shoot(10, 3, fixedAngle: 0, rotateAngle: 5, projectileIndex: 0, coolDownOffset: 0, coolDown: 200),
                                 new Shoot(radius: 24, count: 4, shootAngle: 15, predictive: 0.40, projectileIndex: 2, coolDown: 700),
                                new TimedTransition(time: 750, targetState: "Shot_rotation3")
                            ),
                            new State("Shot_rotation3",
                                new Shoot(radius: 8, count: 1, projectileIndex: 2, predictive: 0.2, coolDown: 900),
                                new Shoot(radius: 0, count: 3, shootAngle: 120, projectileIndex: 4, angleOffset: 0.7, defaultAngle: 80, coolDown: 700),
                                new TimedTransition(time: 750, targetState: "Shot_rotation1")
                            ),
                            new HpLessTransition(0.4, "Pause2")
                        ),
                        new State("Pause2",
                            new ChangeSize(rate: -11, target: 50),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Prioritize(
                                new StayCloseToSpawn(speed: 0.4, range: 12),
                                new StayBack(0.5, 4)
                            ),
                            new TimedTransition(time: 2500, targetState: "Bring_on_the_flamers")
                        ),
                        new State("Bring_on_the_flamers",
                            new Reproduce(children: "Malphas Protector", densityRadius: 24, densityMax: 3, coolDown: 1000),
                            new ChangeSize(rate: -11, target: 50),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Prioritize(
                                new StayCloseToSpawn(speed: 0.4, range: 12),
                                new StayBack(0.5, 4)
                            ),
                            new Shoot(radius: 8, count: 1, predictive: 0.25, coolDown: 2100),
                            new State("cum_flamer",
                                new TossObject(child: "Malphas Flamer", range: 6, angle: 0, coolDown: 9000, throwEffect: true),
                                new TossObject(child: "Malphas Flamer", range: 6, angle: 90, coolDown: 9000, throwEffect: true),
                                new TossObject(child: "Malphas Flamer", range: 6, angle: 180, coolDown: 9000, throwEffect: true),
                                new TossObject(child: "Malphas Flamer", range: 6, angle: 270, coolDown: 9000, throwEffect: true),
                                new TimedTransition(2000, "reproduce_flamer")
                            ),
                            new State("reproduce_flamer",
                                new Reproduce(children: "Malphas Flamer", densityRadius: 24, densityMax: 5, coolDown: 500)
                            ),
                            new HpLessTransition(0.2, "Temporary_exhaustion")
                        ),
                        new State("Temporary_exhaustion",
                            new Reproduce(children: "Malphas Protector", densityRadius: 24, densityMax: 3, coolDown: 1000),
                            new Flash(color: 0x484848, flashPeriod: 0.6, flashRepeats: 5),
                            new Prioritize(
                                new StayCloseToSpawn(speed: 0.4, range: 12),
                                new StayBack(0.5, 4)
                            )
                        )
                    ),
                    new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1, timeout: 0)
                       ),
                   new Threshold(0.0001,
                    new TierLoot(tier: 6, type: ItemType.Armor, probability: 0.4),
                    new TierLoot(tier: 7, type: ItemType.Weapon, probability: 0.25),
                    new TierLoot(tier: 9, type: ItemType.Weapon, probability: 0.125),
                    new TierLoot(tier: 3, type: ItemType.Ability, probability: 0.25),
                    new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.125),
                    new TierLoot(tier: 7, type: ItemType.Armor, probability: 0.4),
                    new TierLoot(tier: 9, type: ItemType.Armor, probability: 0.25),
                    new TierLoot(tier: 3, type: ItemType.Ring, probability: 0.25),
                    new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.125),
                    new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.0625),
                    new TierLoot(tier: 10, type: ItemType.Armor, probability: 0.125),
                    new ItemLoot(item: "Potion of Defense", probability: 1, numRequired: 3),
                    new ItemLoot(item: "Mark of Malphas", 0, 1),
                    new ItemLoot(item: "Wine Cellar Incantation", probability: 0.05),
                    new ItemLoot(item: "Demon Blade", probability: 0.006, damagebased: true),
                    new ItemLoot(item: "Sword of Illumination", probability: 0.006, damagebased: true),
                    new ItemLoot(item: "Abyss of Demons Key", 0.01),
                    new ItemLoot(item: "Baby Demon's Egg", 0.03, damagebased: true),
                    new ItemLoot(item: "Demonic Bow", 0.004, damagebased: true),
                    new ItemLoot(item: "Blades of the Slayer", 0.004, damagebased: true),
                    new ItemLoot(item: "Awoken Demonic Bow", 0.004, damagebased: true, threshold: 0.1),
                    new ItemLoot(item: "Awoken Blades of the Slayer", 0.004, damagebased: true, threshold: 0.1)


                           )
              )

            
            .Init("Malphas Flamer",
                new State(
                    new State("Attacking",
                        new State("Charge",
                            new Prioritize(
                                new Follow(speed: 0.7, acquireRange: 10, range: 0.1)
                                ),
                            new PlayerWithinTransition(dist: 2, targetState: "Bullet1", seeInvis: true)
                            ),
                        new State("Bullet1",
                            new ChangeSize(rate: 20, target: 130),
                            new Flash(color: 0xFFAA00, flashPeriod: 0.2, flashRepeats: 20),
                            new Shoot(radius: 8, coolDown: 200),
                            new TimedTransition(time: 4000, targetState: "Wait1")
                            ),
                        new State("Wait1",
                            new ChangeSize(rate: -20, target: 70),
                            new Charge(speed: 3, range: 20, coolDown: 600)
                            ),
                        new HpLessTransition(threshold: 0.2, targetState: "FlashBeforeExplode")
                    ),
                    new State("FlashBeforeExplode",
                        new Flash(color: 0xFF0000, flashPeriod: 0.75, flashRepeats: 1),
                        new TimedTransition(time: 300, targetState: "Explode")
                        ),
                    new State("Explode",
                        new Shoot(radius: 0, count: 8, shootAngle: 45, defaultAngle: 0),
                        new Decay(time: 100)
                        )
                    ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.1),
                    new ItemLoot("Magic Potion", 0.1)
                    )
            )
            .Init("White Demon of the Abyss",
                new State(
                    new Prioritize(
                        new StayAbove(speed: 0.5, altitude: 200),
                        new Follow(speed: 1, range: 7),
                        new Wander(speed: 0.4)
                    ),
                    new Shoot(radius: 10, count: 3, shootAngle: 20, predictive: 1, coolDown: 700)
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Attack", 0.1)
                )
            )
            ;
    }
}