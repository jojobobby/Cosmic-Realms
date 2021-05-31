using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ManoroftheImmortals = () => Behav()
            .Init("Lord Ruthven",
                new State(
                    new ScaleHP2(40,1,15),
                    new DropPortalOnDeath(target: "Epic Manor of the Immortals Portal"),
                    new State("Ini",
                        new Wander(speed: 0.3),
                        new StayCloseToSpawn(speed: 0.3, range: 4),
                        new HpLessTransition(threshold: 0.995, targetState: "Regular"),
                        new PlayerWithinTransition(dist: 9, targetState: "Regular")
                    ),
                    new State("Regular",
                        new Wander(speed: 0.3),
                        new StayCloseToSpawn(speed: 0.3, range: 4),
                        new State("Choose",
                            new HpLessTransition(threshold: 0.25, targetState: "Daggers4"),
                            new HpLessTransition(threshold: 0.5, targetState: "Daggers3"),
                            new HpLessTransition(threshold: 0.75, targetState: "Daggers2"),
                            new HpLessTransition(threshold: 1, targetState: "Daggers1")
                        ),
                        new State("Daggers1",
                            new Shoot(radius: 24, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(radius: 24, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(radius: 24, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                            new HpLessTransition(threshold: 0.75, targetState: "CheckCoffins"),
                            new TimedTransition(time: 6000, targetState: "CheckCoffins")
                        ),
                        new State("Daggers2",
                            new Shoot(radius: 24, count: 5, shootAngle: 15, projectileIndex: 0, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(radius: 24, count: 5, shootAngle: 15, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(radius: 24, count: 5, shootAngle: 15, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                            new HpLessTransition(threshold: 0.5, targetState: "CheckCoffins"),
                            new TimedTransition(time: 6000, targetState: "CheckCoffins")
                        ),
                        new State("Daggers3",
                            new Shoot(radius: 24, count: 9, shootAngle: 22, projectileIndex: 0, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(radius: 24, count: 9, shootAngle: 22, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(radius: 24, count: 9, shootAngle: 22, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                            new HpLessTransition(threshold: 0.25, targetState: "CheckCoffins"),
                            new TimedTransition(time: 6000, targetState: "CheckCoffins")
                        ),
                        new State("Daggers4",
                            new Shoot(radius: 24, count: 18, shootAngle: 20, projectileIndex: 0, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(radius: 24, count: 18, shootAngle: 20, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(radius: 24, count: 18, shootAngle: 20, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                            new TimedTransition(time: 6000, targetState: "CheckCoffins")
                        ),
                        new State("CheckCoffins",
                            new HpLessTransition(threshold: 0.50, targetState: "pre ThrowCoffins"),
                            new EntitiesNotExistsTransition(99, "pre ThrowCoffins", "Coffin Creature")
                        )
                    ),
                    new State("pre ThrowCoffins",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(radius: 24, count: 12, shootAngle: 30, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1000),
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1400),
                        new Shoot(radius: 24, count: 12, shootAngle: 30, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1800),
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 2200),
                        new TimedTransition(time: 2400, targetState: "ThrowCoffins")
                    ),
                    new State("ThrowCoffins",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new State("Position",
                            new ReturnToSpawn(speed: 0.9),
                            new TimedTransition(time: 1000, targetState: "Shoot")
                        ),
                        new State("Shoot",
                            new ReturnToSpawn(speed: 0.9),
                            new TossObject(child: "Coffin Creature", range: 8.5, angle: 0, coolDown: 5000),
                            new TossObject(child: "Coffin Creature", range: 8.5, angle: 90, coolDown: 5000),
                            new TossObject(child: "Coffin Creature", range: 8.5, angle: 180, coolDown: 5000),
                            new TossObject(child: "Coffin Creature", range: 8.5, angle: 270, coolDown: 5000),
                            new TimedTransition(time: 1200, targetState: "SwarmorBats")
                        ),
                        new State("SwarmorBats",
                            new HpLessTransition(threshold: 0.68, targetState: "DualSwarms"),
                            new TimedTransition(time: 1000, targetState: "TurnIntoBats")
                        )
                    ),
                    new State("DualSwarms",
                        new State("Position",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new ReturnToSpawn(speed: 0.85),
                            new TimedTransition(time: 1600, targetState: "BatForms")
                        ),
                        new State("BatForms",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new State("Split",
                                new ChangeSize(rate: -100, target: 0),
                                new Spawn(children: "Bat Swarm 1", maxChildren: 1, initialSpawn: 0),
                                new TimedTransition(time: 1600, targetState: "Attack")
                            ),
                            new Reproduce(children: "Vampire Bat", densityRadius: 99, densityMax: 10, coolDown: 100),
                            new State("Attack",
                                new Spawn(children: "Bat Swarm 2", maxChildren: 1, initialSpawn: 0),
                                new StayCloseToSpawn(speed: 2, range: 15),
                                new Follow(speed: 1, acquireRange: 12, range: 1),
                                new TimedTransition(time: 10000, targetState: "Regroup")
                            ),
                            new State("Regroup",
                                new ReturnToSpawn(speed: 1.6),
                                new TimedTransition(time: 1000, targetState: "Done")
                            ),
                            new State("Done",
                                new ReturnToSpawn(speed: 1.1),
                                new TimedTransition(time: 1000, targetState: "TurnBack")
                            )
                        )
                    ),
                    new State("TurnBack",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(rate: 100, target: 120),
                        new OrderOnce(range: 99, children: "Bat Swarm 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Bat Swarm 2", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 2", targetState: "Die"),
                        new TimedTransition(time: 1000, targetState: "Regular")
                    ),
                    new State("TurnIntoBats",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(rate: -100, target: 0),
                        new Reproduce(children: "Vampire Bat", densityRadius: 99, densityMax: 15, coolDown: 100),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new TimedTransition(time: 10000, targetState: "TurnBack")
                    )
                ),
                new Threshold(0.00001,
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new ItemLoot("Holy Water", 1),
                    new ItemLoot("Holy Water", 0.3),
                    new ItemLoot("Holy Water", 0.2),
                    new ItemLoot("Holy Water", 0.1),
                    new TierLoot(7, ItemType.Weapon, 0.25),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(7, ItemType.Armor, 0.4),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new ItemLoot("Bone Dagger", 0.05),
                    new ItemLoot("St. Abraham's Wand", probability: 0.0041),
                    new ItemLoot("Chasuble of Holy Light", probability: 0.01),
                    new ItemLoot("Ring of Divine Faith", probability: 0.01),
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Holy Crossbow", 0.004, damagebased: true),
                    new ItemLoot("Awoken Holy Crossbow", 0.002, damagebased: true, threshold: 0.01),
                    new ItemLoot("Wine Cellar Incantation", probability: 0.05),
                    new ItemLoot("Mark of Ruthven", 0, 1),
                    new ItemLoot(item: "Tome of Purification", probability: 0.004, damagebased: true)
                )
            )
            .Init("Coffin Creature",
                new State(
                    new TimedTransition(time: 20000, targetState: "Die"),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.5),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.5),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.4),
                    new TransformOnDeath(target: "Lil Feratu", probability: 0.3),
                    new State("Ini",
                        new TimedTransition(time: 400, targetState: "Opening")
                    ),
                    new State("Opening",
                        new TimedTransition(time: 600, targetState: "Shoot")
                    ),
                    new State("Shoot",
                        new Shoot(radius: 9, count: 1, projectileIndex: 0, coolDown: 200),
                        new TimedTransition(time: 3000, targetState: "Closing")
                    ),
                    new State("Closing",
                        new TimedTransition(time: 600, targetState: "Ini")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            .Init("Lil Feratu",
                new State(
                    new Shoot(radius: 10, count: 5, shootAngle: 15, projectileIndex: 0, predictive: 0.75, coolDown: 1000),
                    new Prioritize(
                        new Follow(speed: 0.4, acquireRange: 10, range: 2, duration: 2000, coolDown: 2600),
                        new StayBack(speed: 0.4, distance: 2, entity: null),
                        new Charge(speed: 0.85, range: 8, coolDown: 1000),
                        new Wander(speed: 0.4)
                    )
                )
            )
            .Init("Bat Swarm 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new Reproduce(children: "Vampire Bat Swarmer 1", densityRadius: 99, densityMax: 10, coolDown: 100),
                        new EntitiesNotExistsTransition(99, "Die", "Lord Ruthven", "Dracula")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            .Init("Bat Swarm 2",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new Reproduce(children: "Vampire Bat Swarmer 2", densityRadius: 99, densityMax: 10, coolDown: 100),
                        new EntitiesNotExistsTransition(99, "Die", "Lord Ruthven", "Dracula")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            .Init("Vampire Bat",
                new State(
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8, protectee: "Lord Ruthven", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75),
                            new Protect(speed: 1.8, protectee: "Dracula", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75),
                            new Wander(speed: 0.8)
                        ),
                        new Shoot(radius: 24, count: 1, projectileIndex: 0, coolDown: 100)
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            .Init("Vampire Bat Swarmer 1",
                new State(
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8, protectee: "Bat Swarm 1", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75),
                            new Wander(speed: 0.8)
                        ),
                        new Shoot(radius: 24, count: 1, projectileIndex: 0, coolDown: 100),
                        new EntityNotExistsTransition(target: "Bat Swarm 1", dist: 99, targetState: "Die")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            .Init("Vampire Bat Swarmer 2",
                new State(
                    new State("Ini",
                        new Prioritize(
                            new Protect(speed: 1.8, protectee: "Bat Swarm 2", acquireRange: 7, protectionRange: 2, reprotectRange: 0.75),
                            new Wander(speed: 0.8)
                        ),
                        new Shoot(radius: 24, count: 1, projectileIndex: 0, coolDown: 100),
                        new EntityNotExistsTransition(target: "Bat Swarm 2", dist: 99, targetState: "Die")
                    ),
                    new State("Die",
                        new Suicide()
                    )
                )
            )
            ;
    }
}