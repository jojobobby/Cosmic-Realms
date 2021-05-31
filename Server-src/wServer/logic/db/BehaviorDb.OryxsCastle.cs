using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree, ??
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxsCastle = () => Behav()
            .Init("Oryx Guardian TaskMaster",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new EntitiesNotExistsTransition(100, "Death", "Oryx Stone Guardian Right", "Oryx Stone Guardian Left")
                    ),
                    new State("Death",
                        new RemoveTileObject(0x0d77, 60),
                        new Spawn("Locked Wine Cellar Portal", 1, 1),
                        new Suicide()
                    )
                )
            )
            .Init("Oryx's Living Floor",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(20, "Toss")
                    ),
                    new State("Toss",
                        new TossObject("Quiet Bomb", 10, coolDown: 4000),
                        new NoPlayerWithinTransition(21, "Idle"),
                        new PlayerWithinTransition(5, "Shoot and Toss")
                    ),
                    new State("Shoot and Toss",
                        new NoPlayerWithinTransition(21, "Idle"),
                        new NoPlayerWithinTransition(6, "Toss"),
                        new Shoot(0, 18, fixedAngle: 0, coolDown: new Cooldown(750, 250)),
                        new TossObject("Quiet Bomb", 10, coolDown: 4000)
                    )
                )
            )
            .Init("Quiet Bomb",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new State("Tex1",
                            new TimedTransition(500, "Tex2")
                        ),
                        new State("Tex2",
                            new SetAltTexture(1),
                            new TimedTransition(500, "Tex3")
                        ),
                        new State("Tex3",
                            new SetAltTexture(0),
                            new TimedTransition(500, "Tex4")
                        ),
                        new State("Tex4",
                            new SetAltTexture(1),
                            new TimedTransition(500, "Explode")
                        )
                    ),
                    new State("Explode",
                        new SetAltTexture(0),
                        new Shoot(0, 18, fixedAngle: 0),
                        new Suicide()
                    )
                )
            )
            .Init("Oryx Guardian Sword",
                new State(
                    new Shoot(0, 5, 72, fixedAngle: 36, coolDownOffset: 0, coolDown: 2000),
                    new Shoot(0, 5, 72, fixedAngle: 0, coolDownOffset: 500, coolDown: 2000),
                    new State("Idle",
                        new HpLessTransition(0.5, "2")
                    ),
                    new State("2",
                        new Grenade(4, 50, 10, 270, coolDown: 10000),
                        new Shoot(0, 8),
                        new Decay(0)
                    )
                )
            )
            .Init("Oryx Stone Guardian Left",
                new State(
                     new ScaleHP2(40,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new PlayerWithinTransition(6, "Start")
                    ),
                    new State("Start",
                        new Flash(0xC0C0C0, 1, 3),
                        new TimedTransition(3000, "1")
                    ),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new Wander(0.3),
                        new Shoot(0, 10, 36, 2, 0, coolDown: 2000),
                        new TimedRandomTransition(3000, false, "2", "13", "18", "Throw_Sword")
                    ),
                    new State("1_2",
                        new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new SetAltTexture(0),
                        new Charge(1.5, 8, 0),
                        new Shoot(7, 5, 10, predictive: 1, coolDown: 1000),
                        new TimedRandomTransition(3000, false, "2", "13", "18", "Throw_Sword")
                    ),
                    new State("2",
                        new Prioritize(
                            new Protect(1.5, "Oryx Guardian TaskMaster", 20, 5, 4.25),
                            new Orbit(0.4, 4, 20, "Oryx Guardian TaskMaster", 0, 0)
                        ),
                        new State("pre_spiral_circle",
                            new ConditionalEffect(ConditionEffectIndex.Barrier),
                            new TimedTransition(1500, "3")
                        ),
                        new State("3",
                            new SetAltTexture(3),
                            new Shoot(99, 2, fixedAngle: 0),
                            new TimedTransition(200, "4")
                        ),
                        new State("4",
                            new Shoot(99, 2, fixedAngle: 40),
                            new TimedTransition(200, "5")
                        ),
                        new State("5",
                            new Shoot(99, 2, fixedAngle: 80),
                            new TimedTransition(200, "6")
                        ),
                        new State("6",
                            new Shoot(99, 2, fixedAngle: 120),
                            new TimedTransition(200, "7")
                        ),
                        new State("7",
                            new Shoot(99, 2, fixedAngle: 160),
                            new TimedTransition(200, "8")
                        ),
                        new State("8",
                            new Shoot(99, 2, fixedAngle: 200),
                            new TimedTransition(200, "9")
                        ),
                        new State("9",
                            new Shoot(99, 2, fixedAngle: 240),
                            new TimedTransition(200, "10")
                        ),
                        new State("10",
                            new Shoot(99, 2, fixedAngle: 280),
                            new TimedTransition(200, "11")
                        ),
                        new State("11",
                            new Shoot(99, 2, fixedAngle: 320),
                            new TimedTransition(200, "3")
                        ),
                        new TimedRandomTransition(4800, false, "1", "1_2")
                    ),
                    new State("13",
                        new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new SetAltTexture(0),
                        new Protect(2, "Oryx Guardian TaskMaster", 20, 1, 0.3),
                        new Shoot(0, 10, 36, 2, 0, coolDown: 2000),
                        new TimedTransition(1000, "14")
                    ),
                    new State("14",
                        new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new MoveLine(2, 180, 11),
                        new TimedTransition(1000, "15")
                    ),
                    new State("15",
                        new Grenade(2, 50, 4.33, coolDown: 500),
                        new State("16",
                            new Shoot(0, 2, shootAngle: 120, fixedAngle: 180, coolDownOffset: 0, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 140, fixedAngle: 180, coolDownOffset: 200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 160, fixedAngle: 180, coolDownOffset: 400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 180, fixedAngle: 180, coolDownOffset: 800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 200, fixedAngle: 180, coolDownOffset: 1000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 220, fixedAngle: 180, coolDownOffset: 1200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 240, fixedAngle: 180, coolDownOffset: 1400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 260, fixedAngle: 180, coolDownOffset: 1600, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 280, fixedAngle: 180, coolDownOffset: 1800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 300, fixedAngle: 180, coolDownOffset: 2000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 320, fixedAngle: 180, coolDownOffset: 2200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 340, fixedAngle: 180, coolDownOffset: 2400, coolDown: 999999),
                            new Shoot(0, 1, shootAngle: 0, fixedAngle: 0, coolDownOffset: 2600, coolDown: 999999),
                            new TimedTransition(2600, "17")
                        ),
                        new State("17",
                            new Shoot(0, 2, shootAngle: 120, fixedAngle: 180, coolDownOffset: 0, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 140, fixedAngle: 180, coolDownOffset: 200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 160, fixedAngle: 180, coolDownOffset: 400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 180, fixedAngle: 180, coolDownOffset: 800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 200, fixedAngle: 180, coolDownOffset: 1000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 220, fixedAngle: 180, coolDownOffset: 1200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 240, fixedAngle: 180, coolDownOffset: 1400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 260, fixedAngle: 180, coolDownOffset: 1600, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 280, fixedAngle: 180, coolDownOffset: 1800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 300, fixedAngle: 180, coolDownOffset: 2000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 320, fixedAngle: 180, coolDownOffset: 2200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: 340, fixedAngle: 180, coolDownOffset: 2400, coolDown: 999999),
                            new Shoot(0, 1, shootAngle: 0, fixedAngle: 0, coolDownOffset: 2600, coolDown: 999999),
                            new TimedTransition(2600, "16")
                        ),
                        new TimedRandomTransition(5200, false, "1", "1_2")
                    ),
                    new State("18",
                        new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new SetAltTexture(0),
                        new Protect(2, "Oryx Guardian TaskMaster", 20, 1, 0.3),
                        new TimedTransition(1000, "19")
                    ),
                    new State("19",
                        new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new MoveLine(2, 180, 0.2),
                        new TimedTransition(1000, "20")
                    ),
                    new State("20",
                        new Shoot(0, 7, 26, 1, 180, coolDownOffset: 0),
                        new Shoot(0, 8, 22, 1, 180, coolDownOffset: 200),
                        new Grenade(2.5, 50, 4.33, coolDown: 500),
                        new TimedRandomTransition(5000, false, "1", "1_2")
                    ),
                    new State("Throw_Sword",
                        new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new ReturnToSpawn(2),
                        new TimedTransition(2000, "21")
                    ),
                    new State("21",
                        new MoveLine(1.5, 0, 2),
                        new TimedTransition(600, "22")
                    ),
                    new State("21",
                         new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new SetAltTexture(2),
                        new TossObject("Oryx Guardian Sword", 10, 90, coolDown: 99000),
                        new TimedTransition(4000, "22")
                    ),
                    new State("22",
                         new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new TimedTransition(1000, "23"),
                        new EntityNotExistsTransition("Oryx Guardian Sword", 99, "23")
                    ),
                    new State("23",
                        new TimedRandomTransition(1600, false, "1", "1_2")
                    )
                ),
                new Threshold(0.00001,
                    new TierLoot(7, ItemType.Weapon, 0.25),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(7, ItemType.Armor, 0.4),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(3, ItemType.Ring, 0.25),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Ancient Stone Sword", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Heavy Plated Robe", 0.004, damagebased: true),
                    new ItemLoot("Ring of the Guardian", 0.004, damagebased: true)
                              )
            )
        .Init("Oryx Knight",
                new State(
                      new State("waiting for u bae <3",
                          new PlayerWithinTransition(10, "tim 4 rekkings")
                          ),
                      new State("tim 4 rekkings",
                          new Prioritize(
                              new Wander(0.2),
                              new Follow(0.6, 10, 3, -1, 0)
                             ),
                          new Shoot(10, 3, 20, 0, coolDown: 350),
                          new TimedTransition(5000, "tim 4 singular rekt")
                          ),
                      new State("tim 4 singular rekt",
                          new Prioritize(
                                 new Wander(0.2),
                              new Follow(0.7, 10, 3, -1, 0)
                              ),
                          new Shoot(10, 1, projectileIndex: 0, coolDown: 50),
                          new Shoot(10, 1, projectileIndex: 1, coolDown: 1000),
                          new Shoot(10, 1, projectileIndex: 2, coolDown: 450),
                          new TimedTransition(2500, "tim 4 rekkings")
                         )
                  )
            )
            .Init("Oryx Pet",
                new State(
                      new State("swagoo baboon",
                          new PlayerWithinTransition(10, "anuspiddle")
                          ),
                      new State("anuspiddle",
                          new Prioritize(
                              new Wander(0.2),
                              new Follow(0.6, 10, 0, -1, 0)
                              ),
                          new Shoot(10, 2, shootAngle: 20, projectileIndex: 0, coolDown: 1),
                        new Shoot(10, 1, projectileIndex: 0, coolDown: 1)
                         )
                  )
            )
            .Init("Oryx Insect Commander",
                new State(
                      new State("lol jordan is a nub",
                          new Prioritize(
                              new Wander(0.2)
                              ),
                          new Reproduce("Oryx Insect Minion", 10, 20, 1),
                          new Shoot(10, 1, projectileIndex: 0, coolDown: 900)
                         )
                       ),
                new Threshold(.01,
                     new ItemLoot("Potion Crate", 0.50),
                     new ItemLoot("Items Crate", 0.01),
                    new ItemLoot("Bottled Bloody Fairy", 0.004)
                  )
            )
            .Init("Oryx Insect Minion",
                new State(
                      new State("its SWARMING time",
                          new Prioritize(
                              new Wander(0.2),
                              new StayCloseToSpawn(0.4, 8),
                                 new Follow(0.8, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, projectileIndex: 0, coolDown: 1500),
                          new Shoot(10, 1, projectileIndex: 0, coolDown: 230)
                          )
                  )

            )
            .Init("Oryx Eye Warrior",
                new State(
                    new State("swaggin",
                        new PlayerWithinTransition(10, "penispiddle")
                        ),
                    new State("penispiddle",
                          new Prioritize(
                              new Follow(0.6, 10, 0, -1, 0)
                              ),
                          new Shoot(10, 5, projectileIndex: 0, coolDown: 1000),
                          new Shoot(10, 1, projectileIndex: 1, coolDown: 500)
                         )
                  )
            )
            .Init("Oryx Brute",
                new State(
                      new State("swaggin",
                          new PlayerWithinTransition(10, "piddle")
                        ),
                      new State("piddle",
                          new Prioritize(
                              new Wander(0.2),
                              new Follow(0.4, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, projectileIndex: 1, coolDown: 1000),
                          new Reproduce("Oryx Eye Warrior", 10, 4, 2),
                          new TimedTransition(5000, "charge")
                          ),
                      new State("charge",
                          new Prioritize(
                              new Wander(0.3),
                              new Follow(1.2, 10, 1, -1, 0)
                              ),
                          new Shoot(10, 5, projectileIndex: 1, coolDown: 1000),
                          new Shoot(10, 5, projectileIndex: 2, coolDown: 750),
                          new Reproduce("Oryx Eye Warrior", 10, 4, 2),
                          new Shoot(10, 3, 10, projectileIndex: 0, coolDown: 300),
                          new TimedTransition(4000, "piddle")
                         )
                         ),
                new Threshold(.01,
                     new ItemLoot("Potion Crate", 0.50),
                     new ItemLoot("Items Crate", 0.01),
                    new ItemLoot("Manic Axe Head Shuriken", 0.004)
                  )
            )
            .Init("Oryx Stone Guardian Right",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(1),
                        new PlayerWithinTransition(6, "Start")
                    ),
                    new State("Start",
                        new Flash(0xC0C0C0, 1, 3),
                        new TimedTransition(3000, "1")
                    ),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(0),
                        new Wander(0.3),
                        new Shoot(0, 10, 36, 2, 0, coolDown: 2000),
                        new TimedRandomTransition(3000, false, "2", "13", "18", "Throw_Sword")
                    ),
                    new State("1_2",
                        new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new SetAltTexture(0),
                        new Charge(1.5, 8, 0),
                        new Shoot(7, 5, 10, predictive: 1, coolDown: 1000),
                        new TimedRandomTransition(3000, false, "2", "13", "18", "Throw_Sword")
                    ),
                    new State("2",
                        new Prioritize(
                            new Protect(1.5, "Oryx Guardian TaskMaster", 20, 5, 4.25),
                            new Orbit(0.4, 4, 20, "Oryx Guardian TaskMaster", 0, 0)
                        ),
                        new State("pre_spiral_circle",
                            new ConditionalEffect(ConditionEffectIndex.Barrier),
                            new TimedTransition(1500, "3")
                        ),
                        new State("3",
                            new SetAltTexture(3),
                            new Shoot(99, 2, fixedAngle: 0),
                            new TimedTransition(200, "4")
                        ),
                        new State("4",
                            new Shoot(99, 2, fixedAngle: 40),
                            new TimedTransition(200, "5")
                        ),
                        new State("5",
                            new Shoot(99, 2, fixedAngle: 80),
                            new TimedTransition(200, "6")
                        ),
                        new State("6",
                            new Shoot(99, 2, fixedAngle: 120),
                            new TimedTransition(200, "7")
                        ),
                        new State("7",
                            new Shoot(99, 2, fixedAngle: 160),
                            new TimedTransition(200, "8")
                        ),
                        new State("8",
                            new Shoot(99, 2, fixedAngle: 200),
                            new TimedTransition(200, "9")
                        ),
                        new State("9",
                            new Shoot(99, 2, fixedAngle: 240),
                            new TimedTransition(200, "10")
                        ),
                        new State("10",
                            new Shoot(99, 2, fixedAngle: 280),
                            new TimedTransition(200, "11")
                        ),
                        new State("11",
                            new Shoot(99, 2, fixedAngle: 320),
                            new TimedTransition(200, "3")
                        ),
                        new TimedRandomTransition(4800, false, "1", "1_2")
                    ),
                    new State("13",
                      new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new SetAltTexture(0),
                        new Protect(2, "Oryx Guardian TaskMaster", 20, 1, 0.3),
                        new Shoot(0, 10, 36, 2, 0, coolDown: 2000),
                        new TimedTransition(1000, "14")
                    ),
                    new State("14",
                      new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new MoveLine(2, 0, 11),
                        new TimedTransition(1000, "15")
                    ),
                    new State("15",
                        new Grenade(2, 50, 4.33, coolDown: 500),
                        new State("16",
                            new Shoot(0, 2, shootAngle: -120, fixedAngle: 0, coolDownOffset: 0, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -140, fixedAngle: 0, coolDownOffset: 200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -160, fixedAngle: 0, coolDownOffset: 400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -180, fixedAngle: 0, coolDownOffset: 800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -200, fixedAngle: 0, coolDownOffset: 1000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -220, fixedAngle: 0, coolDownOffset: 1200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -240, fixedAngle: 0, coolDownOffset: 1400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -260, fixedAngle: 0, coolDownOffset: 1600, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -280, fixedAngle: 0, coolDownOffset: 1800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -300, fixedAngle: 0, coolDownOffset: 2000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -320, fixedAngle: 0, coolDownOffset: 2200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -340, fixedAngle: 0, coolDownOffset: 2400, coolDown: 999999),
                            new Shoot(0, 1, shootAngle: 0, fixedAngle: 180, coolDownOffset: 2600, coolDown: 999999),
                            new TimedTransition(2600, "17")
                        ),
                        new State("17",
                            new Shoot(0, 2, shootAngle: -120, fixedAngle: 0, coolDownOffset: 0, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -140, fixedAngle: 0, coolDownOffset: 200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -160, fixedAngle: 0, coolDownOffset: 400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -180, fixedAngle: 0, coolDownOffset: 800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -200, fixedAngle: 0, coolDownOffset: 1000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -220, fixedAngle: 0, coolDownOffset: 1200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -240, fixedAngle: 0, coolDownOffset: 1400, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -260, fixedAngle: 0, coolDownOffset: 1600, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -280, fixedAngle: 0, coolDownOffset: 1800, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -300, fixedAngle: 0, coolDownOffset: 2000, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -320, fixedAngle: 0, coolDownOffset: 2200, coolDown: 999999),
                            new Shoot(0, 2, shootAngle: -340, fixedAngle: 0, coolDownOffset: 2400, coolDown: 999999),
                            new Shoot(0, 1, shootAngle: 0, fixedAngle: 180, coolDownOffset: 2600, coolDown: 999999),
                            new TimedTransition(2600, "16")
                        ),
                        new TimedRandomTransition(5200, false, "1", "1_2")
                    ),
                    new State("18",
                         new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new SetAltTexture(0),
                        new Protect(2, "Oryx Guardian TaskMaster", 20, 1, 0.3),
                        new TimedTransition(1000, "19")
                    ),
                    new State("19",
                       new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new MoveLine(2, 0, 0.2),
                        new TimedTransition(1000, "20")
                    ),
                    new State("20",
                        new Shoot(0, 7, 26, 1, 0, coolDownOffset: 0),
                        new Shoot(0, 8, 22, 1, 0, coolDownOffset: 200),
                        new Grenade(2.5, 50, 4.33, coolDown: 500),
                        new TimedRandomTransition(5000, false, "1", "1_2")
                    ),
                    new State("Throw_Sword",
                         new ConditionalEffect(ConditionEffectIndex.Barrier, true),
                        new ReturnToSpawn(2),
                        new TimedTransition(2000, "21")
                    ),
                    new State("21",
                        new MoveLine(1.5, 180, 2),
                        new TimedTransition(600, "22")
                    ),
                    new State("21",
                         new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new SetAltTexture(2),
                        new TossObject("Oryx Guardian Sword", 10, 90, coolDown: 99000),
                        new TimedTransition(1000, "22")
                    ),
                    new State("22",
                         new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new TimedTransition(4000, "23"),
                        new EntityNotExistsTransition("Oryx Guardian Sword", 99, "23")
                    ),
                    new State("23",
                        new TimedRandomTransition(1600, false, "1", "1_2")
                    )
                ),
                new Threshold(0.00001,
                    new TierLoot(7, ItemType.Weapon, 0.25),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(7, ItemType.Armor, 0.4),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(3, ItemType.Ring, 0.25),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Ancient Stone Sword", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Skull of Solid Defense", 0.004, damagebased: true),
                    new ItemLoot("Ancient Summoner Staff", 0.008, damagebased: true)
                    )
            )
        ;
    }
}