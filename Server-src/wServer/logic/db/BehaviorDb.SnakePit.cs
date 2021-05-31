using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by ossimc82, GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SnakePit = () => Behav()
            .Init("Stheno the Snake Queen",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1),
                    new Reproduce(children: "Stheno Pet", densityRadius: 99, densityMax: 4, coolDown: 1500),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(dist: 12, targetState: "Silver Blasts", seeInvis: true)
                    ),
                    new State("Silver Blasts",
                        new State("Silver Blasts 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 2")
                        ),
                        new State("Silver Blasts 2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 3")
                        ),
                        new State("Silver Blasts 3",
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 4")
                        ),
                        new State("Silver Blasts 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 5")
                        ),
                        new State("Silver Blasts 5",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 6")
                        ),
                        new State("Silver Blasts 6",
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 7")
                        ),
                        new State("Silver Blasts 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blasts 8")
                        ),
                        new State("Silver Blasts 8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 45, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 135, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 225, projectileIndex: 0),
                            new Shoot(radius: 10, count: 2, shootAngle: 10, angleOffset: 315, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Spawn Stheno Swarm")
                        )
                    ),
                    new State("Spawn Stheno Swarm",
                        new Prioritize(
                            new StayCloseToSpawn(speed: 0.4, range: 4),
                            new Wander(speed: 0.4)
                        ),
                        new Reproduce(children: "Stheno Swarm", densityRadius: 99, densityMax: 6, coolDown: 400),
                        new State("Silver Blast 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 2")
                        ),
                        new State("Silver Blast 2",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 3")
                        ),
                        new State("Silver Blast 3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 4")
                        ),
                        new State("Silver Blast 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 5")
                        ),
                        new State("Silver Blast 5",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 6")
                        ),
                        new State("Silver Blast 6",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 7")
                        ),
                        new State("Silver Blast 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 8")
                        ),
                        new State("Silver Blast 8",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 9")
                        ),
                        new State("Silver Blast 9",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 10")
                        ),
                        new State("Silver Blast 10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 11")
                        ),
                        new State("Silver Blast 11",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 12")
                        ),
                        new State("Silver Blast 12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 13")
                        ),
                        new State("Silver Blast 13",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 14")
                        ),
                        new State("Silver Blast 14",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 15")
                        ),
                        new State("Silver Blast 15",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Silver Blast 16")
                        ),
                        new State("Silver Blast 16",
                            new Shoot(radius: 10, count: 1, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 270, projectileIndex: 0),
                            new Shoot(radius: 10, count: 1, angleOffset: 90, projectileIndex: 0),
                            new TimedTransition(time: 1000, targetState: "Leave me")
                        ),
                        new State("Leave me",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Order(range: 100, children: "Stheno Swarm", targetState: "Despawn"),
                            new TimedTransition(time: 1000, targetState: "Blind Ring Attack + ThrowAttack")
                        )
                    ),
                    new State("Blind Ring Attack + ThrowAttack",
                        new ReturnToSpawn(speed: 0.3),
                        new State("Blind Ring Attack + ThrowAttack 1",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 2")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 2",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 3")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 3",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 4")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 4",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 5")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 5",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 6")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 6",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 7")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 7",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 8")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 8",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 9")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 9",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 10")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 10",
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 11")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 11",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(radius: 10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Blind Ring Attack + ThrowAttack 12")
                        ),
                        new State("Blind Ring Attack + ThrowAttack 12",
                            new Shoot(10, count: 6, projectileIndex: 1),
                            new Grenade(radius: 2.5, damage: 100, range: 10),
                            new TimedTransition(time: 500, targetState: "Silver Blasts")
                        )
                    )
                ),
                new Threshold(0.0001,
                    new ItemLoot("Snake Pit Key", 0.02, threshold: 0.01),
                    new ItemLoot("Snake Oil", 0.4),
                    new ItemLoot("Snake Oil", 0.3),
                    new ItemLoot("Snake Oil", 0.2),
                    new ItemLoot("Snake Oil", 0.1),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new ItemLoot("Snake Skin Armor", 0.1),
                    new ItemLoot("Snake Skin Shield", 0.1),
                    new ItemLoot("Snake Eye Ring", 0.1),
                    new ItemLoot("Willow Bulwark", 0.004, damagebased: true),
                    new ItemLoot("Potion of Speed", 0.3, 3),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Stheno", 1),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Snakes Wonder Harp", 0.004, damagebased: true),
                    new ItemLoot("Acidic Armor", 0.006, damagebased: true),
                    new ItemLoot("Pernicious Peridot", 0.006, damagebased: true),
                    new ItemLoot("Wand of the Bulwark", 0.001, damagebased: true, threshold: 0.01)
                )
            )
            .Init("Stheno Swarm",
                new State(
                    new State("Protect",
                        new Prioritize(
                            new Protect(speed: 0.3, protectee: "Stheno the Snake Queen"),
                            new Wander(speed: 0.75)
                        ),
                        new Shoot(radius: 10, predictive: 0.05, coolDown: 850)
                    ),
                    new State("Despawn",
                        new Suicide()
                    )
                )
            )
            .Init("Stheno Pet",
                new State(
                    new State("Protect",
                        new Shoot(radius: 25, coolDown: 1000),
                        new State("Protect",
                            new EntityNotExistsTransition("Stheno the Snake Queen", 100, "Wander"),
                            new Orbit(speed: 1.1, radius: 5, acquireRange: 50, target: "Stheno the Snake Queen")
                        ),
                        new State("Wander",
                            new Wander(speed: 0.6)
                        )
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Snake Skin Armor", 0.02)
                )
            );
    }
}
