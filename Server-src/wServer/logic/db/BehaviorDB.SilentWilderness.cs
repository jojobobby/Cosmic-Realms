using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SilentWilderness = () => Behav()
        #region Bosss
            .Init("SW First Core",
                new State(
                    new TransformOnDeath("SW Boss", 1, 1, 1),
                    new TransformOnDeath("SW Core One", 1, 1, 1),
                    new TransformOnDeath("SW Core Two", 1, 1, 1),
                    new TransformOnDeath("SW Core Three", 1, 1, 1),
                    new State("Begin",
                        new HpLessTransition(0.99, "Start")
                        ),
                    new State("Start",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                       new Shoot(radius: 36, count: 10, shootAngle: 36, fixedAngle: 36, projectileIndex: 1, coolDown: 750, coolDownOffset: 0),
                        new TimedTransition(3750, "Fight")
                        ),
                    new State("Fight",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Suicide()
                        )
                )
            )

               .Init("SW Boss",
                new State(
                    new StayCloseToSpawn(1, 6),
                    new State("Begin",
                      new Follow(0.5, 15, 0),
                      new Shoot(radius: 36, count: 4, shootAngle: 90, fixedAngle: 45, rotateAngle: 45, projectileIndex: 0, coolDown: 500, coolDownOffset: 0),
                      new Shoot(radius: 36, count: 4, shootAngle: 90, fixedAngle: 0, rotateAngle: 10, projectileIndex: 1, coolDown: 100, coolDownOffset: 0),
                      new HpLessTransition(0.80, "Start")
                    ),
                     new State("Start",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new ReturnToSpawn(1, 0),
                         new OrderOnce(10, "SW Core Three", "Start2"),
                         new OrderOnce(10, "SW Core Two", "Start2"),
                         new OrderOnce(10, "SW Core One", "Start2"),
                         new TimedTransition(500,"Start2")
                         ),
                     new State("Start2",
                           new Shoot(radius: 36, count: 8, shootAngle: 45, fixedAngle: 45, rotateAngle: 5, projectileIndex: 2, coolDown: 300, coolDownOffset: 0),
                           new Shoot(radius: 36, count: 1, shootAngle: 0, fixedAngle: 45, rotateAngle: -5, projectileIndex: 3, coolDown: 175, coolDownOffset: 0)


                    )
            )
            )

           .Init("SW Core Three", // A to I = 9Segment +1Tail | Proj Invis ? 
                new State(
                    new State("Begin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new StayCloseToSpawn(1, 4),
                    new Protect(1, "SW Boss", 20, 1, 2)
                        ),
                    new State("Start2",
                        new ReturnToSpawn(1,0)
                    )
            ))
          .Init("SW Core Two", // A to I = 9Segment +1Tail | Proj Invis ? 
                new State(
                    new State("Begin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new StayCloseToSpawn(1, 2),
                    new Protect(1, "SW Core Three", 20, 1, 2)
                          ),
                    new State("Start2",
                          new ReturnToSpawn(1, 0)
                    )
            )
                    )
          .Init("SW Core One", // A to I = 9Segment +1Tail | Proj Invis ? 
                new State(
                    new State("Begin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new StayCloseToSpawn(1,0),
                    new Protect(1, "SW Core Two", 20, 1, 2)
                              ),
                    new State("Start2",
                       new ReturnToSpawn(1, 0)
                    )
            ))



        #endregion
        #region Event
            .Init("Amalgamation",
                new State(
                    new State("Begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(15, "Start", true)
                        ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("So!pt og .waay"),
                        new Flash(0x00ff00, 1, 3),
                        new TimedTransition(5000, "Start2")
                        ),
                    new State("Start2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(10, "Start3", true)//eilSm da!s
                        ),
                    new State("Start3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("ouY mkae eilsm da!s"),
                        new TimedTransition(4000, "Start4")
                        ),
                    new State("Start4",
                    new TossObject("Amalgamation Child", 5, null, 8500, 0, false, 1, null, 0, 360, 3, 10),
                    new TossObject("Amalgamation Child", 5, null, 8500, 0, false, 1, null, 0, 360, 3, 10),
                    new TossObject("Amalgamation Child", 5, null, 8500, 0, false, 1, null, 0, 360, 3, 10),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(0, 8, 45, 0, 0, null, 0, null, 0, 3000, 0, false),
                        new Shoot(50, 2, 25, 2, null, null, 0, null, 0.1, 200, 0, false),
                        new HpLessTransition(0.60, "Mad")

                    ),
                    new State("Mad",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("oYu akme silem vyre ngea!ry"),
                        new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                        new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                        new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                        new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                        new Flash(0xff0000, 5, 10),
                        new TimedTransition(5000, "Mad1")
                        ),
                    new State("Mad1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(0, 8, 45, 0, 0, null, 0, null, 0, 2550, 0, false),
                        new Shoot(50, 1, null, 2, null, null, 0, null, 0.1, 150, 0, false),
                        new Shoot(0, 8, 45, 1, 45, 25, 0, null, 0, 1750, 0, false),
                        new TimedTransition(7500, "spawnmore"),
                        new HpLessTransition(0.05, "Done")
                        ),
                    new State("Done",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("lSiem ied onw"),
                        new Flash(0xff0000, 3, 0),
                        new TimedTransition(5000, "Dead")
                           ),
                    new State("spawnmore",
                    new Flash(0xff0000, 5, 5),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                    new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                    new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                    new TossObject("Amalgamation Child", 5, null, 999999, 0, false, 1, null, 0, 360, 3, 10),
                    new TimedTransition(5000, "Mad1")
                        ),
                    new State("Dead",
                        new Suicide()
                        )

                    ),
                  new Threshold(0.1,
                    new TierLoot(8, ItemType.Weapon, .15),
                    new TierLoot(5, ItemType.Ability, .07),
                    new TierLoot(12, ItemType.Armor, .04),
                    new TierLoot(5, ItemType.Ring, .03),
                    new ItemLoot("Potion of Critical Chance", 0.1),
                    new ItemLoot("Potion of Critical Damage", 0.1),
                    new ItemLoot("Potion of Defense", .02),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Potion of Vitality", .02),
                    new ItemLoot("Potion of Wisdom", .02),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Dexterity", .02),
                    new ItemLoot("Daybreak Tessen", .004, damagebased: true),
                    new ItemLoot("Samurai's Plated Annihilation", .01, damagebased: true),//Daybreak Tessen
                    new ItemLoot("Ray Katana", .004, threshold: 0.01, damagebased: true)
            )
            )
            .Init("Amalgamation Child",
                new State(
                      new ConditionalEffect(ConditionEffectIndex.Armored, false, 1250),
                    new State("Begin",
                        new Wander(0.75),
                        new Follow(1, 10, 0, 2000, 2000),
                        new Shoot(radius: 30, count: 1, shootAngle: 0, projectileIndex: 1, coolDown: 100, coolDownOffset: 250),
                        new PlayerWithinTransition(0.5, "Suicide", true),
                        new TimedTransition(7500, "Suicide")
                        ),
                    new State("Suicide",
                        new Charge(1.5, 10, 9999),
                        new Shoot(radius: 30, count: 8, shootAngle: 45, projectileIndex: 0, coolDown: 9999, coolDownOffset: 250),
                        new TimedTransition(1000, "dead")
                        ),
                    new State("dead",
                        new Suicide())))
        #endregion
        #region Minions
            .Init("SW Sludge abomination",
                new State(
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new State("Attack",
                        new Shoot(8, count: 2, projectileIndex: 0, shootAngle: 8, coolDown: 1000),
                        new Shoot(10, count: 8, projectileIndex: 1, shootAngle: 43, coolDown: 125),
                        new TimedTransition(0, "Attack")
                    )
                ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
            )
            .Init("SW Sludge amalgam",
                    new State(
                        new Prioritize(
                            new Follow(0.15, 15, 0)
                            ),
                        new State("Attack",
                            new Shoot(5, count: 6, projectileIndex: 0, shootAngle: 22, coolDown: 750),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
            )
            .Init("SW Sludge carcass",
                    new State(
                        new Prioritize(
                            new Follow(0.1, 20, 0)
                         ),
                        new State("Attack",
                            new Shoot(7, count: 3, projectileIndex: 0, shootAngle: 12, coolDown: 500),
                            new Shoot(7, count: 5, projectileIndex: 1, shootAngle: 22, coolDown: 500),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
                )
        .Init("SW Orange Swarm",
                new State(
                    new State("circle",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Follow(4, acquireRange: 11, range: 3.5, duration: 1000, coolDown: 5000),
                            new Orbit(1.9, 3.5, acquireRange: 12),
                            new Wander(0.4)
                        ),
                        new Shoot(4, count: 2, shootAngle: 15, predictive: 0.1, coolDown: 500),
                        new TimedTransition(3000, "dart_away")
                    ),
                    new State("dart_away",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new StayBack(2, distance: 5),
                            new Wander(0.4)
                        ),
                        new Shoot(8, count: 10, shootAngle: 36, fixedAngle: 10, coolDown: 100000, coolDownOffset: 800),
                        new Shoot(8, count: 10, shootAngle: 36, fixedAngle: 28, coolDown: 100000, coolDownOffset: 1400),
                        new TimedTransition(1600, "circle")
                    )
                )
            )
         .Init("SW Red Swarm",
                new State(
                    new State("circle",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Follow(4, acquireRange: 11, range: 3.5, duration: 1000, coolDown: 5000),
                            new Orbit(1.9, 3.5, acquireRange: 12),
                            new Wander(0.4)
                        ),
                        new Shoot(4, count: 2, shootAngle: 15, predictive: 0.1, coolDown: 500),
                        new TimedTransition(3000, "dart_away")
                    ),
                    new State("dart_away",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new StayBack(2, distance: 5),
                            new Wander(0.4)
                        ),
                        new Shoot(8, count: 10, shootAngle: 36, fixedAngle: 10, coolDown: 100000, coolDownOffset: 800),
                        new Shoot(8, count: 10, shootAngle: 36, fixedAngle: 28, coolDown: 100000, coolDownOffset: 1400),
                        new TimedTransition(1600, "circle")
                    )
                )
            )
        .Init("SW Giant Flytrap",
                    new State(
                        new State("Attack",
                            new Shoot(4, count: 4, projectileIndex: 0, shootAngle: 12, predictive: 1, coolDown: 1000),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
                )
        .Init("SW Sentient Flytrap",
                    new State(
                        new State("Attack",
                            new Shoot(6, count: 1, projectileIndex: 0, coolDown: 100),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
                )
         .Init("SW Spiky Vine",
                    new State(
                        new State("Attack",
                            new SetAltTexture(2, 2, 500, true),
                            new Shoot(20, count: 1, projectileIndex: 0, coolDown: 545),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
                )
          .Init("SW Giant Nepenthe",
                    new State(
                        new State("Attack",
                            new Shoot(3.8, count: 4, fixedAngle: 30, coolDown: 220),
                            new TimedTransition(300, "Attack2")
                        ),
                        new State("Attack2",
                            new Shoot(3.8, count: 4, fixedAngle: 60, coolDown: 220),
                            new TimedTransition(300, "Attack3")
                        ),
                        new State("Attack3",
                            new Shoot(3.8, count: 4, fixedAngle: 90, coolDown: 220),
                            new TimedTransition(300, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
                )
        .Init("SW Soulsucker Sprout",
                    new State(
                        new State("Attack",
                            new Shoot(1.5, count: 8, coolDown: 250),
                            new TimedTransition(0, "Attack")
                        )
                    ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                    )
                )
        .Init("SW Bonefly Nest",
                new State(
                    new State("Spawn",
                        new Spawn(children: "SW Orange Swarm", maxChildren: 2, initialSpawn: 0),
                        new TimedTransition(time: 10000, targetState: "isAlive")
                        ),
                    new State("isAlive",
                        new EntitiesNotExistsTransition(20, "Spawn", "SW Orange Swarm")
                        )
                ),
               new Threshold(0.1,
                    new ItemLoot("Overgrown Stone Shard", 0.001)
                )
            )
        #endregion
        #region Misc
        #endregion




        ;

    }
}
