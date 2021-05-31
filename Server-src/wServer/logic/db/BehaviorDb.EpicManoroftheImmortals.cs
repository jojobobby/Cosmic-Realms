using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EpicManoroftheImmortals = () => Behav()
            .Init("Dracula",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("Regular",
                        new SetAltTexture(0),
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
                            new HpLessTransition(threshold: 0.65, targetState: "pre ThrowCoffins"),
                            new EntitiesNotExistsTransition(99, "pre ThrowCoffins", "Coffin Creature"),
                            new HpLessTransition(threshold: 0.75, targetState: "Daggers2"),
                            new HpLessTransition(threshold: 1, targetState: "Daggers1")
                        )
                    ),
                    new State("pre ThrowCoffins",
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 600),
                        new Shoot(radius: 24, count: 12, shootAngle: 30, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1000),
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1400),
                        new Shoot(radius: 24, count: 12, shootAngle: 30, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 1800),
                        new Shoot(radius: 24, count: 24, shootAngle: 15, projectileIndex: 1, fixedAngle: 0, coolDown: 6000, coolDownOffset: 2200),
                        new TimedTransition(time: 2400, targetState: "ThrowCoffins")
                    ),
                    new State("ThrowCoffins",
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
                            new ReturnToSpawn(speed: 0.85),
                            new TimedTransition(time: 1600, targetState: "BatForms")
                        ),
                        new State("BatForms",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new State("Split",
                                new ChangeSize(rate: -120, target: 0),
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
                        new ChangeSize(rate: 120, target: 240),
                        new OrderOnce(range: 99, children: "Bat Swarm 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Bat Swarm 2", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 1", targetState: "Die"),
                        new OrderOnce(range: 99, children: "Vampire Bat Swarmer 2", targetState: "Die"),
                        new TimedTransition(time: 1000, targetState: "Regular")
                    ),
                    new State("TurnIntoBats",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(rate: -120, target: 0),
                        new Reproduce(children: "Vampire Bat", densityRadius: 99, densityMax: 15, coolDown: 100),
                        new Follow(speed: 1, acquireRange: 12, range: 1),
                        new TimedTransition(time: 10000, targetState: "TurnBack")
                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Holy Salt", 1),
                    new ItemLoot("Holy Salt", 0.3),
                    new ItemLoot("Holy Salt", 0.2),
                    new ItemLoot("Holy Salt", 0.1),
                    new TierLoot(6, ItemType.Ability, 0.03125),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("Greater Potion of Attack", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Life", 0.3),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("St. Abraham's Wand", probability: 0.008),
                    new ItemLoot("Chasuble of Holy Light", probability: 0.015),
                    new ItemLoot("Ring of Divine Faith", probability: 0.015),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Holy Crossbow", 0.004, damagebased: true),
                    new ItemLoot("Awoken Holy Crossbow", 0.002, damagebased: true, threshold: 0.01),
                    new ItemLoot("Tome of Exorcism", 0.001, damagebased: true, threshold: 0.01)
                    )
            )
            .Init("Vampire Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Dracula Chest", probability: 0.1),
                    new State("Ini",
                        new EntityNotExistsTransition("Dracula", 99, "Die")
                    ),
                    new State("Die",
                        new Decay(0)
                    )
                )
            )
            .Init("Dracula Chest",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFFFFFF, 0.5, 1),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new ItemLoot("Holy Salt", 1),
                    new ItemLoot("Holy Salt", 0.3),
                    new ItemLoot("Holy Salt", 0.2),
                    new ItemLoot("Holy Salt", 0.1),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new TierLoot(6, ItemType.Ability, 0.03125),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("Greater Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Mana", 0.3),
                    new ItemLoot("Potion of Life", 0.3),
                    new ItemLoot("Tome of Exorcism", 0.001, damagebased: true)
                )
            
            )
            ;
    }
}