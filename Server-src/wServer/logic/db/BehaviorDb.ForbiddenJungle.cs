using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ForbiddenJungle = () => Behav()
            .Init("Great Temple Snake",
                new State(
                    new Shoot(radius: 6, count: 2, shootAngle: 12, projectileIndex: 0, defaultAngle: 0, predictive: 0.6, coolDown: 600, coolDownOffset: 200),
                    new Shoot(radius: 6, count: 6, shootAngle: 60, projectileIndex: 1, defaultAngle: 0, predictive: 1, coolDown: 3000, coolDownOffset: 400),
                    new Prioritize(
                        new Follow(speed: 0.65, acquireRange: 8, range: 2.4, duration: 3000, coolDown: 800),
                        new Wander(speed: 0.25)
                    )
                )
            )
            .Init("Great Snake Egg",
                new State(
                    new TransformOnDeath(target: "Great Temple Snake"),
                    new State("Ini",
                        new TimedTransition(time: 5700, targetState: "Hatch")
                    ),
                    new State("Hatch",
                        new Decay(time: 200)
                    )
                )
            )
            .Init("Great Coil Snake",
                new State(
                    new DropPortalOnDeath(target: "Forbidden Jungle Portal", probability: 0.4, timeout: 30),
                    new State("Ini",
                        new Prioritize(
                            new Follow(speed: 0.4, acquireRange: 8, range: 4.5, duration: 1200, coolDown: 1000),
                            new Wander(speed: 0.25)
                        ),
                        new Shoot(radius: 7.5, count: 1, projectileIndex: 0, predictive: 0.6, coolDown: 800, coolDownOffset: 400),
                        new Shoot(radius: 7.5, count: 1, projectileIndex: 0, predictive: 1, coolDown: 800, coolDownOffset: 600),
                        new Shoot(radius: 7.5, count: 10, shootAngle: 36, projectileIndex: 1, predictive: 1, coolDown: 2000, coolDownOffset: 0),
                        new State("Ini2",
                            new PlayerWithinTransition(dist: 12, targetState: "Ini3")
                        ),
                        new State("Ini3",
                            new TimedTransition(time: 3400, targetState: "ThrowEgg")
                        )
                    ),
                    new State("ThrowEgg",
                        new Prioritize(
                            new Follow(speed: 0.4, acquireRange: 8, range: 4.5, duration: 1200, coolDown: 1000),
                            new Wander(speed: 0.25)
                        ),
                        new State("Choose",
                            new TimedRandomTransition(0, false, "Choose1", "Choose2")
                        ),
                        new State("Choose1",
                            new TimedRandomTransition(0, false, "ThrowUp", "ThrowDown")
                        ),
                        new State("Choose2",
                            new TimedRandomTransition(0, false, "ThrowRight", "ThrowLeft")
                        ),
                        new State("ThrowUp",
                            new TossObject(child: "Great Snake Egg", range: 4, angle: 270, coolDown: 5000),
                            new TimedTransition(time: 200, targetState: "Choose")
                        ),
                        new State("ThrowDown",
                            new TossObject(child: "Great Snake Egg", range: 4, angle: 90, coolDown: 5000),
                            new TimedTransition(time: 200, targetState: "Choose")
                        ),
                        new State("ThrowRight",
                            new TossObject(child: "Great Snake Egg", range: 4, angle: 0, coolDown: 5000),
                            new TimedTransition(time: 200, targetState: "Choose")
                        ),
                        new State("ThrowLeft",
                            new TossObject(child: "Great Snake Egg", range: 4, angle: 180, coolDown: 5000),
                            new TimedTransition(time: 200, targetState: "Choose")
                        ),
                        new TimedTransition(time: 2400, targetState: "Ini")
                     )
                )
            )
            .Init("Mixcoatl the Masked God",
                new State(
                    new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1),
                    new State("Ini",
                        new Wander(speed: 0.3),
                        new StayCloseToSpawn(speed: 0.3, range: 4),
                        new HpLessTransition(threshold: 0.995, targetState: "Summon")
                    ),
                    new State("Summon",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("Position",
                            new ReturnToSpawn(speed: 1.4),
                            new TimedTransition(time: 600, targetState: "Animate1")
                        ),
                        new State("Animate1",
                            new SetAltTexture(minValue: 3),
                            new TimedTransition(time: 200, targetState: "Animate2")
                        ),
                        new State("Animate2",
                            new SetAltTexture(minValue: 4),
                            new OrderOnce(range: 99, children: "Boss Totem", targetState: "Spirit"),
                            new TimedTransition(time: 200, targetState: "Animate3")
                        ),
                        new State("Animate3",
                            new SetAltTexture(minValue: 5),
                            new TimedTransition(time: 200, targetState: "Animate4")
                        ),
                        new State("Animate4",
                            new SetAltTexture(minValue: 6),
                            new TimedTransition(time: 200, targetState: "Animate1")
                        ),
                        new TimedTransition(time: 4000, targetState: "Stage1")
                    ),
                    new State("Stage1",
                        new SetAltTexture(minValue: 0),
                        new State("ChooseAttack",
                            new TimedTransition(time: 0, targetState: "Shotgun")
                        ),
                        new State("Fireball",
                            new Shoot(radius: 8, count: 1, shootAngle: 8, projectileIndex: 2, predictive: 0.9, coolDown: 500),
                            new TimedTransition(time: 3000, targetState: "Shotgun")
                        ),
                        new State("Shotgun",
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 0, coolDown: 3200, coolDownOffset: 0),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 45, coolDown: 3200, coolDownOffset: 200),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 90, coolDown: 3200, coolDownOffset: 400),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 135, coolDown: 3200, coolDownOffset: 600),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 180, coolDown: 3200, coolDownOffset: 800),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 225, coolDown: 3200, coolDownOffset: 1000),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 270, coolDown: 3200, coolDownOffset: 1200),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 315, coolDown: 3200, coolDownOffset: 1400),
                            new TimedTransition(time: 3000, targetState: "Fireball")
                        ),
                        new Wander(speed: 0.3),
                        new StayCloseToSpawn(speed: 0.45, range: 10),
                        new HpLessTransition(threshold: 0.5, targetState: "Rage"),
                        new TimedTransition(time: 10000, targetState: "Summon")
                    ),
                    new State("Rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("Position",
                            new ReturnToSpawn(speed: 1.4),
                            new TimedTransition(time: 600, targetState: "Animate1_2")
                        ),
                        new State("Animate1_2",
                            new SetAltTexture(minValue: 1),
                            new TimedTransition(time: 200, targetState: "Animate2_2")
                        ),
                        new State("Animate2_2",
                            new SetAltTexture(minValue: 2),
                            new OrderOnce(range: 99, children: "Boss Totem", targetState: "Power"),
                            new Shoot(radius: 0, count: 8, shootAngle: 45, projectileIndex: 0, coolDown: 600, fixedAngle: 0)
                        ),
                        new TimedTransition(time: 4000, targetState: "Stage2")
                    ),
                    new State("Stage2",
                        new SetAltTexture(minValue: 0),
                        new Protect(speed: 0.5, protectee: "Jungle Fire", acquireRange: 3.5, protectionRange: 3, reprotectRange: 0),
                        new Shoot(radius: 2.6, count: 8, shootAngle: 45, projectileIndex: 0, coolDown: 500, fixedAngle: 0),
                        new State("ChooseAttack2",
                            new TimedTransition(time: 0, targetState: "Shotgun2")
                        ),
                        new State("Shotgun2",
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 0, coolDown: 3200, coolDownOffset: 0),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 45, coolDown: 3200, coolDownOffset: 200),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 90, coolDown: 3200, coolDownOffset: 400),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 135, coolDown: 3200, coolDownOffset: 600),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 180, coolDown: 3200, coolDownOffset: 800),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 225, coolDown: 3200, coolDownOffset: 1000),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 270, coolDown: 3200, coolDownOffset: 1200),
                            new Shoot(radius: 0, count: 3, shootAngle: 11, projectileIndex: 1, fixedAngle: 315, coolDown: 3200, coolDownOffset: 1400),
                            new TimedTransition(time: 3000, targetState: "ChooseAttack2")
                        ),
                        new Prioritize(
                            new Follow(speed: 0.62, acquireRange: 8, range: 1, duration: 1200, coolDown: 600),
                            new Orbit(speed: 0.6, radius: 2.5, acquireRange: 4),
                            new Wander(speed: 0.3)
                        ),
                        new StayCloseToSpawn(speed: 0.45, range: 10)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(6, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new ItemLoot("Pollen Powder", 1),
                    new ItemLoot("Pollen Powder", 0.3),
                    new ItemLoot("Pollen Powder", 0.2),
                    new ItemLoot("Pollen Powder", 0.1),
                    new ItemLoot("Robe of the Tlatoani", 0.08),
                    new ItemLoot("Staff of the Crystal Serpent", 0.08),
                    new ItemLoot("Cracked Crystal Skull", 0.08),
                    new ItemLoot("Crystal Bone Ring", 0.08),
                    new ItemLoot("Wine Cellar Incantation", 0.05)
                )
            )
            .Init("Boss Totem",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("Ini"),
                    new State("Spirit",
                        new Spawn(children: "Totem Spirit", maxChildren: 6, initialSpawn: 0, coolDown: 3000),
                        new EntityNotExistsTransition(target: "Mixcoatl the Masked God", dist: 99, targetState: "Ini"),
                        new TimedTransition(time: 3000, targetState: "Ini")
                    ),
                    new State("Power",
                        new Shoot(radius: 0, count: 6, shootAngle: 60, projectileIndex: 0, fixedAngle: 0, coolDown: 600, coolDownOffset: 0),
                        new Shoot(radius: 0, count: 6, shootAngle: 60, projectileIndex: 0, fixedAngle: 30, coolDown: 600, coolDownOffset: 200),
                        new TimedTransition(time: 2400, targetState: "Regular"),
                        new EntityNotExistsTransition(target: "Mixcoatl the Masked God", dist: 99, targetState: "Ini")
                    ),
                    new State("Regular",
                        new Shoot(radius: 0, count: 6, shootAngle: 60, projectileIndex: 0, fixedAngle: 0, coolDown: 3000, coolDownOffset: 0, predictive: 0.5),
                        new Shoot(radius: 0, count: 6, shootAngle: 60, projectileIndex: 0, fixedAngle: 30, coolDown: 3000, coolDownOffset: 600, predictive: 0.5),
                        new EntityNotExistsTransition(target: "Mixcoatl the Masked God", dist: 99, targetState: "Ini")
                    )
                )
            )
            .Init("Totem Spirit",
                new State(
                    new Prioritize(
                        new Protect(speed: 0.7, protectee: "Jungle Totem", acquireRange: 7, protectionRange: 7, reprotectRange: 3.8),
                        new Follow(speed: 0.4, acquireRange: 5, range: 4.5, duration: 1200, coolDown: 1400),
                        new Wander(speed: 0.25)
                    ),
                    new Shoot(radius: 4.5, count: 1, projectileIndex: 0, predictive: 0.6, coolDown: 800, coolDownOffset: 400),
                    new Shoot(radius: 4.5, count: 1, projectileIndex: 0, predictive: 1, coolDown: 800, coolDownOffset: 600)
                )
            )
        ;
    }
}