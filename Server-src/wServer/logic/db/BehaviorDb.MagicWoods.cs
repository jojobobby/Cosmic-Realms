using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MagicWoods = () => Behav()

        #region Minions
        /*.Init("MW Iatho",
            new State(
                   new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                        new StayCloseToSpawn(0.4, 6),
                        new Charge(2.0, 10f, 4000),
                        new Wander(0.4)
                        ),
                    new NoPlayerWithinTransition(7, "return")
                    ),
                new State("return",
                    new ReturnToSpawn(0.6),
                    new TimedTransition(1520, "Idle")
                   )))
        .Init("MW Iawa",
              new State(
                   new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                          new StayCloseToSpawn(0.4, 6),
                        new Charge(2.0, 10f, 4000),
                        new Wander(0.4)
                        ),
                    new NoPlayerWithinTransition(7, "Idle")
            )))
        .Init("MW Oshyu",
            new State(
                new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(6, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                        new Orbit(0.4, 3.5, 11),
                        new Follow(0.4, range: 3)
                        ),
                    new Shoot(12, 5, 72, coolDown: 1000)
                    )))
            .Init("MW Seus",
            new State(
                new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                        new StayCloseToSpawn(0.4, 6),
                        new Charge(2.0, 10f, 4000),
                        new Wander(0.4)
                        ),
                    new Shoot(10, 1, 20, predictive: 1, coolDown: 500),
                    new NoPlayerWithinTransition(7, "return")
                    ),
                new State("return",
                    new ReturnToSpawn(0.6),
                    new TimedTransition(1520, "Idle")
                   )))
          .Init("MW Eango",
            new State(
                new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                        new StayCloseToSpawn(0.4, 6),
                        new Charge(2.0, 10f, 4000),
                        new Wander(0.4)
                        ),
                    new Shoot(10, 1, 20, projectileIndex: 0, predictive: 0, coolDown: 500),
                    new NoPlayerWithinTransition(7, "return")
                    ),
                new State("return",
                    new ReturnToSpawn( 0.6),
                    new TimedTransition(1520, "Idle")
                   )))
            .Init("MW Ril",
                new State(
                    new State("Idle",
                        new Wander(0.4),
                        new PlayerWithinTransition(5, "chase")
                        ),
                    new State("chase",
                        new Prioritize(
                            new Orbit(0.4, 3.5, 11),
                            new Follow(0.4, range: 2.5)
                            ),
                        new Shoot(12, 5, 72, coolDown: 500),
                        new NoPlayerWithinTransition(7, "Idle")
                        )))
                .Init("MW Tiar",
                    new State(
                        new State("Idle",
                            new Wander(0.4),
                            new PlayerWithinTransition(6, "chase")
                            ),
                        new State("chase",
                            new Prioritize(
                                new Follow(0.4, range: 3),
                                new Wander(0.4)
                                ),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 1250),
                            new NoPlayerWithinTransition(11, "Idle")
                            )))
                 .Init("MW Yimi",
                    new State(
                        new State("Idle",
                            new Wander(0.4),
                            new PlayerWithinTransition(6, "chase")
                            ),
                        new State("chase",
                            new Prioritize(
                                new Follow(0.4, range: 3),
                                new Wander(0.4)
                                ),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 1250),
                            new NoPlayerWithinTransition(11, "Idle")
                            )))
                .Init("MW Gharr",
                    new State(
                        new State("Idle",
                            new Wander(0.4),
                            new PlayerWithinTransition(6, "chase")
                            ),
                        new State("chase",
                            new Prioritize(
                                new Follow(0.4, range: 3),
                                new Wander(0.4)
                                ),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 500),
                            new NoPlayerWithinTransition(11, "Idle")
                            )))
                .Init("MW Zhiar",
                    new State(
                        new State("Idle",
                            new Prioritize(
                                new Wander(0.4),
                                new StayBack(0.4, 5)
                                ),
                            new Shoot(10, 2, 20, predictive: 0, coolDown: 500)
                            )))
         .Init("MW Itani",
                    new State(
                        new State("Idle",
                            new Wander(0.4),
                            new PlayerWithinTransition(6, "chase")
                            ),
                        new State("chase",
                            new Prioritize(
                                new Follow(0.4, range: 3),
                                new Wander(0.4)
                                ),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 1250),
                            new NoPlayerWithinTransition(11, "Idle")
                            )))
             .Init("MW Queq",
                    new State(
                        new State("Idle",
                            new Prioritize(
                                new Wander(0.4),
                                new StayBack(0.4, 5)
                                ),
                            new Shoot(10, 4, 20, predictive: 0, coolDown: 500)
                            )))
                .Init("MW Tal",
                    new State(
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 1),
                            new Wander(0.4)
                            ),
                        new Grenade(4, 20, 8, coolDown: 250)
                            ))
                .Init("MW Uoro",
                        new State(
                            new State("Idle",
                                new Prioritize(
                                new StayCloseToSpawn(0.4, 1),
                                new Wander(0.4)
                                ),
                                new PlayerWithinTransition(5, "chase")
                                ),
                            new State("chase",
                                new Prioritize(
                                    new Wander(0.8),
                                    new Follow(0.8, range: 3)
                                    ),
                                new Shoot(10, 3, 20, predictive: 0, coolDown: 850),
                                new NoPlayerWithinTransition(8, "Idle")
                                )))
                .Init("MW Iri",
                    new State(
                        new Orbit(0.2, 3),
                        new Grenade(4, 20, 8, coolDown: 250)
                        ))
                  .Init("MW Serl",
                    new State(
                        new Wander(0.6),
                        new Shoot(10, 1, 20, predictive: 0, coolDown: 150)
                        ))
                .Init("MW Rilr",
                    new State(
                        new Prioritize(
                        new StayBack(0.2, 5),
                        new Wander(0.2)
                            ),
                        new Shoot(10, 3, 20, predictive: 0, coolDown: 1500)
                        ))
          .Init("MW Radph",
                new State(
                    new State("searching",
                        new Prioritize(
                            new Charge(2),
                            new Wander(0.4)
                            ),
                        new Reproduce(densityMax: 5, densityRadius: 10),
                        new PlayerWithinTransition(2, "creeping")
                        ),
                    new State("creeping",
                        new Shoot(0, 10, 36, projectileIndex: 0, fixedAngle: 0),
                        new Decay(0)
                        )))
             .Init("MW Vorv",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(5, "chase")
                        ),
                    new State("chase",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 6),
                            new Charge(2.0, 10f, 4000),
                            new Wander(0.4)
                            ),
                        new Shoot(10, 2, 20, projectileIndex: 0, predictive: 0, coolDown: 150),
                        new Shoot(10, 2, 20, projectileIndex: 1, predictive: 0, coolDown: 150),
                        new NoPlayerWithinTransition(7, "return")
                        ),
                    new State("return",
                        new ReturnToSpawn(0.6),
                        new TimedTransition(1520, "Idle")
                        )))
            .Init("MW Oeti",
                new State(
                    new State("Idle",
                        new Wander(0.3),
                        new PlayerWithinTransition(5, "chase")
                        ),
                    new State("chase",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 6),
                            new Charge(2.0, 10f, 4000),
                            new Wander(0.4)
                            ),
                        new Shoot(12, projectileIndex: 0, count: 8, shootAngle: 72, predictive: 0.5, coolDown: 1750),
                        new Shoot(12, projectileIndex: 1, count: 5, shootAngle: 72, predictive: 0.5, coolDown: 1750),
                        new NoPlayerWithinTransition(7, "return")
                        ),
                    new State("return",
                        new ReturnToSpawn(0.6),
                        new TimedTransition(1520, "Idle")
                        )))
                .Init("MW Sek",
                    new State(
                        new State("Idle",
                            new Prioritize(
                                new StayCloseToSpawn(0.4, 1),
                                new Wander(0.4)
                                ),
                            new PlayerWithinTransition(5, "chase")
                            ),
                        new State("chase",
                            new Follow(0.8, range: 0.5),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 250)
                            )))
            .Init("MW Scheev",
                new State(
                    new State("Idle",
                        new Wander(0.4),
                        new PlayerWithinTransition(5, "chase")
                        ),
                    new State("chase",
                        new NoPlayerWithinTransition(8, "Idle"),
                        new Follow(0.4, range: 4),
                        new Shoot(10, 1, 20, predictive: 0, coolDown: 1000),
                        new TimedTransition(4250, "Pause")
                        ),
                    new State("Pause",
                        new NoPlayerWithinTransition(8, "Idle"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Wander(0.4),
                        new TimedTransition(3000, "chase")
                        )))
          .Init("MW Laen",
                new State(
                    new State("Idle",
                        new Wander(0.4),
                        new PlayerWithinTransition(5, "chase")
                        ),
                    new State("chase",
                      new NoPlayerWithinTransition(7, "Idle"),
                        new Prioritize(
                            new Orbit(0.4, 3.5, 11),
                            new Follow(0.4, range: 2.5)
                            ),
                        new Shoot(12, 5, 72, coolDown: 500),
                        new TimedTransition(2000, "Shoot_1shot")
                        ),
                    new State("Shoot_1shot",
                        new NoPlayerWithinTransition(7, "Idle"),
                         new Prioritize(
                            new Orbit(0.4, 3.5, 11),
                            new Follow(0.4, range: 2.5)
                            ),
                         new Shoot(10, 1, 20, predictive: 0, coolDown: 1000),
                         new TimedTransition(2150, "chase")
                        )))
        .Init("MW Utanu",
            new State(
                new Shoot(12, projectileIndex: 0, count: 3, shootAngle: 10),
                new Spawn("MW Drac", maxChildren: 5),
                    new State("Idle",
                    new Prioritize(
                        new StayCloseToSpawn(0.2, 1),
                        new Wander(0.2)
                            )
                        ),
                    new State("chase",
                        new Follow(0.2, range: 7)
                        )))
            .Init("MW Deyst",
                new State(
                    new Orbit(0.3, 3.5, 2, "MW Utanu"),
                    new Shoot(10, 1, 20, predictive: 0, coolDown: 250)
                    ))
            .Init("MW Issz",
                new State(
                    new Orbit(0.3, 3.5, 5, "MW Utanu"),
                    new Shoot(10, 1, 20, predictive: 0, coolDown: 250)
                    ))
            .Init("MW Eati",
                new State(
                new State("Idle",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "chase")
                    ),
                new State("chase",
                    new Prioritize(
                        new Follow(0.6, range: 3),
                        new Wander(0.6)
                        ),
                    new Shoot(10, 2, 20, predictive: 0, coolDown: 1000),
                    new Shoot(10, 2, 20, projectileIndex: 1, predictive: 0, coolDown: 1000),
                    new NoPlayerWithinTransition(7, "Idle")
                    )))
                .Init("MW Idrae",
                    new State(
                        new State("Idle",
                            new PlayerWithinTransition(5, "chase")
                            ),
                        new State("chase",
                            new Follow(0.2, range: 2),
                            new Shoot(4, 20, 8, coolDown: 1500)
                        )))
                    .Init("MW Urake",
                        new State(
                          new TransformOnDeath("MW Yangu", 1, 4, 1),
                          new StayBack(0.2, 7),
                          new Shoot(10, 1, 20, predictive: 1, coolDown: 1750)
                        ))
                    .Init("MW Yangu",
                        new State(
                            new TransformOnDeath("MW Lauk", 2, 5, 1),
                            new Wander(0.3),
                            new Shoot(10, 2, 20, predictive: 1, coolDown: 900)
                        ))
                    .Init("MW Lauk",
                        new State(
                            new Prioritize(
                                new Follow(0.4, range: 2),
                                new Wander(0.4)
                                ),
                            new Shoot(10, 3, 20, predictive: 1, coolDown: 600)
                        ))
                    .Init("MW Eashy",
                        new State(
                            new TransformOnDeath("MW Sek", 1, 3, 1),
                            new Spawn("MW Sek", maxChildren: 5),
                            new Charge(0.2, 10f, 4000),
                            new Shoot(10, 1, 20, predictive: 0, coolDown: 2000)
                            ))
                    .Init("MW SayIt",
                        new State(
                           new State("Idle",
                            new Wander(0.4),
                            new PlayerWithinTransition(5, "chase")
                            ),
                        new State("chase",
                            new Prioritize(
                                new StayCloseToSpawn(0.4, 6),
                                new Charge(2.0, 10f, 4000),
                                new Wander(0.4)
                                ),
                            new NoPlayerWithinTransition(7, "return")
                            ),
                        new State("return",
                            new ReturnToSpawn( 0.6),
                            new TimedTransition(1520, "Idle")
                         )))
                    .Init("MW Eendi",
                        new State(
                            new Prioritize(
                                new Charge(0.6, 10f, 4000),
                                new Wander(0.6)
                                )

                            ))*/
        #endregion

                    .Init("MW Fountain Spirit",
                        new State(
                            new DropPortalOnDeath("Portal of Cowardice", 100),
                            new State("Idle1",
                                new MoveTo(1, 119, 97),
                                new TimedTransition(1000, "Idle2")
                                ),
                            new State("Idle2",
                                new PlayerWithinTransition(10, "greet_player")
                                ),
                            new State("greet_player",
                                new Taunt("Ah, so you are the guest I have felt the presence of! Strike my flowing waters and I shall reward you!"),
                                new HpLessTransition(.99, "Its_a_trap")
                                ),
                            new State("Its_a_trap",
                                new Taunt("In an eternity with US!"),
                                //orders fountains in conors to spawn butterflies every half minute.
                                //throws grenades in a plus (4 grenades)
                                new Grenade(10, 50, 7, 90, coolDown: 2000),
                                new Grenade(10, 50, 7, 360, coolDown: 2000),
                                new Grenade(10, 50, 7, 180, coolDown: 2000),
                                new Grenade(10, 50, 7, 0, coolDown: 2000),
                                //also throws grenades in an X (4 grenades)
                                new Grenade(10, 50, 7, 45, coolDown: 3000),
                                new Grenade(10, 50, 7, 135, coolDown: 3000),
                                new Grenade(10, 50, 7, 220, coolDown: 3000),
                                new Grenade(10, 50, 7, 315, coolDown: 3000),
                                new HpLessTransition(.90, "Despawn_waterfountain")
                                ),
                            new State("Despawn_waterfountain",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new ChangeSize(100, 100),
                                new Flash(0xFFFFFF, 1, 10),
                                new MoveTo(119, 98, 2),
                                new TimedTransition(1000, "grow_and_heal")
                                ),
                            new State("grow_and_heal",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new ChangeSize(100, 200),
                                new Flash(0xFFFFFF, 3000, 20),
                                new TimedTransition(7000, "Attack1")
                                ),
                            new State("Attack1",
                                new Shoot(12, projectileIndex: 8, count: 9, shootAngle: 72, predictive: 0.5, coolDown: 750),
                                new Shoot(12, 4, 72, coolDown: 1000),
                                new TimedTransition(10000, "Flashing2")
                                ),
                            new State("Flashing2",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Flash(0xFFFFFF, 1, 7),
                                new TimedTransition(5000, "Attack2")
                                ),
                            new State("Attack2",
                                new Prioritize(
                                    new Follow(0.4),
                                    new Wander(0.4),
                                    new Orbit(0.4, 3.5, 2)
                                    ),
                                new Shoot(10, 2, 20, projectileIndex: 1, predictive: 0, coolDown: 1000),
                                new TimedTransition(10000, "Flashing3")
                                ),
                             new State("Flashing3",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Flash(0xFFFFFF, 1, 7),
                                new TimedTransition(5000, "Attack3")
                                ),
                            new State("Attack3",
                                new Prioritize(
                                    new Follow(0.4),
                                    new Wander(0.4),
                                    new Orbit(0.4, 3.5, 2)
                                    ),
                                new Shoot(10, 2, 20, projectileIndex: 1, predictive: 0, coolDown: 1000),
                                new TimedTransition(10000, "Flashing4")
                                ),
                             new State("Flashing4",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Flash(0xFFFFFF, 1, 7),
                                new TimedTransition(5000, "ReturnToSpawn1")
                                 ),
                             new State("ReturnToSpawn1",
                                 new MoveTo(1, 119, 98),
                                 new Flash(0xFFFFFF, 1, 7),
                                 new TimedTransition(3000, "Attack4")
                                 ),
                             new State("Attack4",
                                 new Shoot(10, 2, 20, projectileIndex: 4, predictive: 0, coolDown: 1000),
                                 new Shoot(10, 2, 20, projectileIndex: 5, predictive: 0, coolDown: 750),
                                 new Shoot(10, 2, 20, projectileIndex: 6, predictive: 0, coolDown: 1250),
                                 new TimedTransition(10000, "Flashing5")
                                 ),
                             new State("Flashing5",
                                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Flash(0xFFFFFF, 1, 7),
                                new TimedTransition(5000, "Attack1")

                    )));
    }
}
