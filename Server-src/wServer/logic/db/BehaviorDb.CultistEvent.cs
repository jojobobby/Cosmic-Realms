using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CultistEvent = () => Behav()
        #region Event
      .Init("Zhin, The Demonic Sacrifice",
            new State(
                 new ScaleHP2(120, 4, 15),
                 new DropPortalOnDeath("Cultist Portal", 100),
                  new State("Beginning222",
                      new PlayerWithinTransition(10, "Beginning"),
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                      ),
              new State("Beginning",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Taunt("Zeraph will be revived, and I will be the sacrifice!"),
                new ChangeSize(10, 200),
                new TimedTransition(2000, "Beginning Two")
              ),
              new State("Beginning Two",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(radius: 10, count: 3, shootAngle: 45, predictive: 1, coolDown: 700),
                new Taunt("I was once a mortal like you, NOW IM MUCH MORE!"),
                new TimedTransition(2100, "Attack 2")
              ),
              new State("Attack",
                new Wander(0.25),
                new StayCloseToSpawn(0.5),
                new StayBack(0.20, 5),
                new Shoot(radius: 10, count: 3, projectileIndex: 4, shootAngle: 15, predictive: 0.5, coolDown: 700, coolDownOffset: 200),
                new Shoot(radius: 49, count: 9, projectileIndex: 5, shootAngle: 45, coolDown: 2500, coolDownOffset: 200),
                new TimedTransition(4500, "Attack 2"),
                new HpLessTransition(.70, "SubAttack")
              ),
              new State("Attack 2",
                new Wander(0.25),
                new StayCloseToSpawn(0.5),
                new StayBack(0.20, 5),
                new Shoot(radius: 10, count: 4, projectileIndex: 4, shootAngle: 15, predictive: 0.5, coolDown: 1200),
                new Shoot(radius: 49, count: 9, projectileIndex: 5, shootAngle: 45, coolDown: 2500),
                new TimedTransition(4500, "Attack"),
                new HpLessTransition(.50, "Help"),
                new HpLessTransition(.70, "SubAttackB")
              ),
              new State("SubAttackB",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new ReturnToSpawn(1),
                new Taunt("You... You damn. ENOUGH!"),
                new HpLessTransition(.50, "Help"),
                new TimedTransition(2000, "SubAttack")
              ),
              new State("SubAttack",
                new Taunt("I WILL NOT LET YOU GET NEAR RAVE DARKMORE!"),
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                new Shoot(radius: 49, count: 9, projectileIndex: 3, shootAngle: 45, coolDown: 800),
                new Shoot(0, 6, projectileIndex: 4, fixedAngle: 0, rotateAngle: 2, coolDown: 200),
                new Shoot(0, 6, projectileIndex: 4, fixedAngle: 5, rotateAngle: 2, coolDown: 200),
                new TimedTransition(4500, "SubAttack2"),
                new HpLessTransition(.50, "Help")
              ),
              new State("SubAttack2",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                new Shoot(radius: 49, count: 9, projectileIndex: 3, shootAngle: 45, coolDown: 1400),
                new Shoot(0, 6, projectileIndex: 1, fixedAngle: 0, rotateAngle: 2, coolDown: 200),
                new Shoot(0, 6, projectileIndex: 1, fixedAngle: 5, rotateAngle: 2, coolDown: 200),
                new TimedTransition(6500, "SubAttack3"),
                new HpLessTransition(.50, "Help")
              ),
              new State("SubAttack3",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                new Shoot(radius: 49, count: 9, projectileIndex: 3, shootAngle: 45, coolDown: 1400),
                new Shoot(0, 6, projectileIndex: 4, fixedAngle: 0, rotateAngle: 2, coolDown: 200),
                new Shoot(0, 6, projectileIndex: 4, fixedAngle: 5, rotateAngle: 2, coolDown: 200),
                new TimedTransition(6500, "SubAttack2"),
                new HpLessTransition(.50, "Help")
              ),
              new State("Help",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new ReturnToSpawn(1),
                new SetAltTexture(2),
                new Taunt("I see what must be done..."),
                new TimedTransition(2000, "Help2")
              ),
              new State("Help2",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new ReturnToSpawn(1),
                new Spawn("C Zhin, The Demonic Sacrifice UP", 1, 1, 99999, true),
                new Spawn("C Zhin, The Demonic Sacrifice DOWN", 1, 1, 99999, true),
                new Spawn("C Zhin, The Demonic Sacrifice LEFT", 1, 1, 99999, true),
                new Spawn("C Zhin, The Demonic Sacrifice RIGHT", 1, 1, 99999, true),
                new Taunt("Shadow clone jutsu"),
                new TimedTransition(3000, "Help3")
              ),
              new State("Help3",
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new ReturnToSpawn(1),
                new SetAltTexture(2),
                new Taunt("Cyberious will beg for our help in the end!"),
                new TimedTransition(3000, "Help44")
              ),
              new State("Help44",
                new ConditionalEffect(ConditionEffectIndex.Armored),
                new ReturnToSpawn(1),
                new SetAltTexture(1),
                new Shoot(radius: 49, count: 9, projectileIndex: 6, shootAngle: 45, coolDown: 1400),
                new Shoot(0, 6, projectileIndex: 4, fixedAngle: 0, rotateAngle: 2, coolDown: 200),
                new Shoot(0, 6, projectileIndex: 2, fixedAngle: 5, rotateAngle: 2, coolDown: 200),
                new HpLessTransition(.10, "Help55")
              ),
              new State("Help55",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(3),
                new ChangeSize(20, 160),
                new Taunt("What the!"),
                new TimedTransition(2000, "Help4")
              ),
              new State("Help4",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(4),
                new Order(20, "C Zhin, The Demonic Sacrifice UP", "Start3"),
                new Order(20, "C Zhin, The Demonic Sacrifice DOWN", "Start3"),
                new Order(20, "C Zhin, The Demonic Sacrifice LEFT", "Start3"),
                new Order(20, "C Zhin, The Demonic Sacrifice RIGHT", "Start3"),
                new Taunt("No more power... My mortal body!!"),
                new TimedTransition(1000, "Help5")
              ),
              new State("Help5",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(5),
                new Taunt("NOO STOP!"),
                new TimedTransition(500, "Help6")
              ),
              new State("Help6",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(6),
                new TimedTransition(500, "Help7")
              ),
              new State("Help7",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(7),
                new TimedTransition(500, "Help8")
              ),
              new State("Help8",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(8),
                new Taunt("This damn blood stone!"),
                new TimedTransition(2500, "Help9")
              ),
              new State("Help9",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(9),
                new Taunt("You have not seen the last of me!"),
                new TimedTransition(250, "Help10")
              ),
              new State("Help10",
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new SetAltTexture(10),
                new TimedTransition(500, "Finished")
              ),
              new State("Finished",
                new Suicide()
              )
            ),
            new Threshold(0.01,
                    new ItemLoot("Staff of Unholy Sacrifice", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Robe of the Ancient Cultist", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Skull of Corrupted Souls", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Bloodshed Ring", 0.004, damagebased: true, threshold: 0.01),
                    new TierLoot(12, ItemType.Weapon, 0.015),
                    new TierLoot(13, ItemType.Armor, 0.015),
                    new TierLoot(5, ItemType.Ability, 0.15),
                    new TierLoot(6, ItemType.Ring, 0.05),
                    new ItemLoot("Greater Potion of Critical Chance", .50),
                    new ItemLoot("Greater Potion of Mana", 0.75),
                    new ItemLoot("50 Credits", 0.1),
                    new ItemLoot("Special Crate", 0.01)

            )
          )
          .Init("C Zhin, The Demonic Sacrifice UP",
            new State(
              new ConditionalEffect(ConditionEffectIndex.Invincible, true),
              new State("Beginning",
                new MoveLine(0.5, 90, 7),
                new Taunt("We will protect until the end!"),
                new TimedTransition(6000, "Start")
              ),
              new State("Start",
                new Orbit(1, 8, 15, "Zhin, The Demonic Sacrifice"),
                new Shoot(radius: 49, count: 4, projectileIndex: 5, shootAngle: 90, coolDown: 2500)
              ),
              new State("Start3",
                new Decay(10000)
              )
            ))
          .Init("C Zhin, The Demonic Sacrifice LEFT",
            new State(
              new ConditionalEffect(ConditionEffectIndex.Invincible, true),
              new State("Beginning",
                new MoveLine(0.50, 0, 7),
                new Taunt("We will protect until the end!"),
                new TimedTransition(6000, "Start")
              ),
              new State("Start",
                new Orbit(1, 8, 15, "Zhin, The Demonic Sacrifice"),
                new Shoot(radius: 49, count: 4, projectileIndex: 5, shootAngle: 90, coolDown: 2500)
              ),
              new State("Start3",
                new Decay(10000)
              )
            ))
          .Init("C Zhin, The Demonic Sacrifice DOWN",
            new State(
              new ConditionalEffect(ConditionEffectIndex.Invincible, true),
              new State("Beginning",
                new MoveLine(0.50, 180, 7),
                new Taunt("We will protect until the end!"),
                new TimedTransition(6000, "Start")
              ),
              new State("Start",
                new Orbit(1, 8, 15, "Zhin, The Demonic Sacrifice"),
                new Shoot(radius: 49, count: 4, projectileIndex: 5, shootAngle: 90, coolDown: 2500)
              ),
              new State("Start3",
                new Decay(10000)
              )
            ))
          .Init("C Zhin, The Demonic Sacrifice RIGHT",
            new State(
              new ConditionalEffect(ConditionEffectIndex.Invincible, true),
              new State("Beginning",
                new MoveLine(0.50, 270, 7),
                new Taunt("We will protect until the end!"),
                new TimedTransition(6000, "Start")
              ),
              new State("Start",
                new Orbit(1, 8, 15, "Zhin, The Demonic Sacrifice", 0, 0),
                new Shoot(radius: 49, count: 4, projectileIndex: 5, shootAngle: 90, coolDown: 2500)
              ),
              new State("Start3",
                new Decay(10000)
              )
            ))
        #endregion

        #region Minions
      .Init("Minir, Follower of the Zahari",
            new State(
                 new ScaleHP2(30, 1, 15),
              new State("fight",
                new Follow(.5, 8, 3, 2000),
                new Wander(.05),
                new Shoot(180, count: 4, projectileIndex: 1, coolDown: 800, shootAngle: 10, coolDownOffset: 400),
                new Shoot(180, count: 4, projectileIndex: 0, coolDown: 1250, coolDownOffset: 600)

              )
                   ),
               new Threshold(0.1,
                    new ItemLoot("Mark of Rave Darkmore", 0.001),
                    new ItemLoot("Minir's Robe of the Cultist", 0.0005, damagebased: true)
            )
          )
          .Init("Bige, Follower of the Zahari",
            new State(
                 new ScaleHP2(30, 1, 15),
              new State("fight",
                new Follow(.5, 8, 3, 2000),
                new Wander(.05),
                //new Shoot(180, count: 2, projectileIndex: 1, coolDown: 800, fixedAngle: 45, coolDownOffset: 400),
                new Shoot(180, count: 2, projectileIndex: 0, shootAngle: 10, coolDown: 1400),
                new Shoot(180, count: 2, projectileIndex: 1, shootAngle: 10, coolDown: 1400),
                new Shoot(180, count: 2, projectileIndex: 2, shootAngle: 10, coolDown: 1400)

              )
                   ),
                 new Threshold(0.1,
                    new ItemLoot("Mark of Rave Darkmore", 0.001),
                    new ItemLoot("Bige's Robe of the Cultist", 0.0005, damagebased: true)
            )

          )

          .Init("Palow, Follower of the Zahari",
            new State(
                 new ScaleHP2(30, 1, 15),
              new State("fight",
                new Follow(.5, 8, 3, 2000),
                new Wander(.05),
                //new Shoot(180, count: 2, projectileIndex: 1, coolDown: 800, fixedAngle: 45, coolDownOffset: 400),
                new Shoot(25, count: 5, projectileIndex: 0, shootAngle: 10, coolDown: 750)

              )
                   ),
                  new Threshold(0.1,
                    new ItemLoot("Mark of Rave Darkmore", 0.001),
                    new ItemLoot("Palow's Robe of the Cultist", 0.0005, damagebased: true)
            )
          )
          .Init("Balo, Follower of the Zahari",
            new State(
                 new ScaleHP2(30, 1, 15),
              new State("fight",
                new Follow(.5, 8, 3, 2000),
                new Wander(.05),
                //new Shoot(180, count: 2, projectileIndex: 1, coolDown: 800, fixedAngle: 45, coolDownOffset: 400),
                new Shoot(170, count: 4, projectileIndex: 0, coolDown: 750)

              )
                   ),
                new Threshold(0.1,
                    new ItemLoot("Mark of Rave Darkmore", 0.001),
                    new ItemLoot("Balo's Robe of the Cultist", 0.0005, damagebased: true)
            )
          )

          .Init("Blue Bloodstoned Demon",
            new State(
              new State("fight",
                new Charge(speed: 1, range: 15, 250),
                new Follow(.5, 8, 2),
                new Wander(.05),
                new Shoot(15, count: 1, projectileIndex: 0, coolDown: 1250)

              )
            )
          )

          .Init("Red Bloodstoned Demon",
            new State(
              new State("fight",
                new Charge(speed: 1, range: 15, 250),
                new Follow(.5, 8, 2),
                new Wander(.05),
                new Shoot(15, count: 1, projectileIndex: 0, coolDown: 1250)

              )
            )
          )

          .Init("Yellow Bloodstoned Demon",
            new State(
              new State("fight",
               new Charge(speed: 1, range: 15, 250),
                new Follow(.5, 8, 2),
                new Wander(.05),
                new Shoot(15, count: 1, projectileIndex: 0, coolDown: 1250)

              )
            )
          )
        #endregion

        #region Misc
                   .Init("Cultist's Sacrifice",
            new State(
              new ScaleHP2(70, 3, 15),
              new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new State("Null"
                ),
                   new State("Talk",
                       new PlayerWithinTransition(7, "Start"),
                       new TimedTransition(1250, "Talk22")
                        ),
                    new State("Talk22",
                       new Taunt("The mortals!"),
                       new TimedTransition(1250, "Talk2")
                        ),
                   new State("Talk2",
                       new Taunt("They stopped me in the middle of the sacrifice!"),
                       new TimedTransition(2000, "Talk3")
                         ),
                   new State("Talk3",
                       new PlayerWithinTransition(7, "Start"),
                      new OrderOnce(20, "Rave Darkmore, The Cultist", "Talk3")
                         ),
                   new State("Leave",
                       new Taunt("Yes Master."),
                       new TimedTransition(750, "Leave2")
                       ),
                   new State("Leave2",
                       new MoveLine(0.5, 0, 20),
                       new TimedTransition(1000, "Leave3")
                               ),
                   new State("Leave3",
                       new Suicide()
                          ),
                   new State("Start4",
                         new OrderOnce(20, "Rave Darkmore, The Cultist", "Talk3")
                       )
                       ),
                   new State("Start",
                       new Taunt("They're here! I must get out of here!"),
                       new TimedTransition(1000, "Start2")
                               ),
                      new State("Start2",
                           new OrderOnce(20, "Rave Darkmore, The Cultist", "Startold"),
                       new MoveLine(0.5, 0, 20)
                                 ),
                      new State("DEMONIC",
                       new Taunt("PLEASE MASTER, YOU MUST NOT UNDERSTAND THE BLOo...d"),
                       new TimedTransition(1000, "Demon")
                               ),
                      new State("Demon",
                       new Transform("Zhin")

                  )
                  )
                )

                .Init("C Fire Spawner YES",
              new State(
                  new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null"
              ),
                  new State("SPAWNNOW",
                  new ConditionalEffect(ConditionEffectIndex.Invincible),
                 new Spawn("Cultist Fire", coolDown: 99999999)
                  )))

          .Init("C Fire Spawner YES FALSE",
              new State(
                  new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null"
              ),
                new TeleportPlayer(11,91,75,true
                  )))

             .Init("Zhin",
              new State(
              new State("Null",
                  new TimedTransition(500, "Attack")
              ),
                  new State("Attack",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Orbit(0.2, 10, 99, "Rave Darkmore, The Cultist"),
                  new Taunt("DIE!"),
                  new Shoot(180, 1, coolDown: 2500)
                      ),
                  new State("Attack2",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1000),
                      new Taunt("PROTECT MASTER"),
                      new Follow(0.5, 20, 2),
                      new Shoot(180, 3, shootAngle: 10, 0)
              )))




                    .Init("Zhin, The Demonic Sacrifice Clone",
              new State(
                       new ConditionalEffect(ConditionEffectIndex.Invincible),

              new State("Null"
              ),
                new State("Nothing",
                         new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              ),

                  new State("Shoot",
                      new Shoot(180, count: 1, shootAngle: 90, rotateAngle: 10, fixedAngle: 0, coolDown: 1250)
              )))






                     .Init("Rave Clone DOWN",

              new State(
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null",
                  new MoveLine(0.5, 180, 11),
                  new TimedTransition(5000, "Attack")
              ),
                new State("Nothing",
                         new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              ),
                  new State("Attack",
                      new Shoot(180, count: 2, projectileIndex: 1, shootAngle: 10, coolDown: 1250)
              )))


                     .Init("Rave Clone UP",

              new State(
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null",
                   new MoveLine(0.5, 90, 11),
                  new TimedTransition(5000, "Attack")
              ),
                new State("Nothing",
                         new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              ),
                  new State("Attack",
                      new Shoot(180, count: 2, projectileIndex: 1, shootAngle: 10, coolDown: 1250)
              )))




    .Init("Rave Clone LEFT",

              new State(
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null",
                  new MoveLine(0.5, 0, 11),
                  new TimedTransition(5000, "Attack")
              ),
                new State("Nothing"
                    ,
                         new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              ),
                  new State("Attack",
                      new Shoot(180, count: 2, projectileIndex: 1, shootAngle: 10, coolDown: 1250)
              )))



    .Init("Rave Clone RIGHT",

              new State(
                 new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null",
                  new MoveLine(0.5, 270, 11),
                  new TimedTransition(5000, "Attack")
              ),
                new State("Nothing",
                         new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              ),
                  new State("Attack",
                      new Shoot(180, count: 2, projectileIndex: 1, shootAngle: 10, coolDown: 1250)
              )



                  








            )
          )
          .Init("Cultist Fire",
              new State(
                  new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("Null",
                  new EntityNotExistsTransition("Rave Darkmore, The Cultist", 60, "gone")
              ),
                new State("gone",
                    new Suicide()
              )
                  )
                      )
        #endregion

        #region Boss


          .Init("Rave Darkmore, The Cultist",
            new State(
             new ScaleHP2(85, 2, 100),
              new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new State("Null",
                      new TossObject("Cultist's Sacrifice", 4, 0, tossInvis: true, coolDown: 99999999),
                      new TossObject("Zhin, The Demonic Sacrifice Clone", 12, 45, tossInvis: true, coolDown: 99999999),
                      new TossObject("Zhin, The Demonic Sacrifice Clone", 12, 135, tossInvis: true, coolDown: 99999999),
                      new TossObject("Zhin, The Demonic Sacrifice Clone", 12, 225, tossInvis: true, coolDown: 99999999),
                      new TossObject("Zhin, The Demonic Sacrifice Clone", 12, 315, tossInvis: true, coolDown: 99999999),
                  new PlayerWithinTransition(15, "Talk")
                ),
                new State("Talk",
                    new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Universal.mp3"),
                     new TimedTransition(500, "Talk55")
                      ),
                new State("Talk55",
                  new Taunt("Why has the sacrifice not started!"),
                  new TimedTransition(2000, "Talk2")
                ),
                new State("Talk2",
                  new OrderOnce(20, "Cultist's Sacrifice", "Talk")
                ),
                new State("Talk3",
                  new Taunt("hmmm, I hope they followed you, I have something in store just for them, you may leave now."),
                 new PlayerWithinTransition(7, "Start"),
                  new TimedTransition(2000, "Talk4")
                      ),
                new State("Talk4",
                  new OrderOnce(20, "Cultist's Sacrifice", "Leave"),
                 new PlayerWithinTransition(7, "Start")
                )
                 ),
                new State("Startold",
                  new Taunt("You work for me!"),
                  new TimedTransition(1750, "Startold2")
                       ),
                new State("Startold2",
                  new Taunt("[Demonic Summoning]"),
                  new OrderOnce(30, "Cultist's Sacrifice", "DEMONIC"),
                  new TimedTransition(1750, "Start")
                       ),
                new State("Start",
                    new Taunt("Foolish..."),
                    new TimedTransition(2000, "Battle")
                    ),
                new State("Battle",
                  new OrderOnce(20, "Zhin, The Demonic Sacrifice Clone", "Shoot"),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedRandomTransition(2000, false, "Basic1", "Basic2", "Basic3"),
                  new HpLessTransition(.50, "Worthy")
                     ),
                new State("Basic1",
                  new ConditionalEffect(ConditionEffectIndex.Armored),
                  new Shoot(180, count: 3, projectileIndex: 5, coolDown: 100, fixedAngle: 0, shootAngle: 120, rotateAngle: 10),
                  new Shoot(180, count: 2, projectileIndex: 3, coolDown: 200, shootAngle: 180, fixedAngle: 0, rotateAngle: 45, coolDownOffset: 0),
                  new TimedTransition(15000, "Battle"),
                  new HpLessTransition(.50, "Worthy")
                      ),
                new State("Basic2",
                  new Shoot(180, count: 1, projectileIndex: 5, coolDown: 900, shootAngle: 180, coolDownOffset: 550, predictive: 0.3),
                  new Shoot(180, count: 3, projectileIndex: 5, coolDown: 100, shootAngle: 120, fixedAngle: 0, rotateAngle: 10),
                  new Shoot(180, count: 3, projectileIndex: 7, coolDown: 100, shootAngle: 120, fixedAngle: 7, rotateAngle: 10),
                   new TimedTransition(15000, "Battle"),
                   new HpLessTransition(.50, "Worthy")
                       ),
                new State("Basic3",
                    new Shoot(80, count: 2, projectileIndex: 4, coolDown: 2500, predictive: 0.6, shootAngle: 100),
                    new Shoot(180, count: 3, projectileIndex: 7, coolDown: 100, shootAngle: 120, fixedAngle: 0, rotateAngle: 10),
                    new TimedTransition(7500, "Basic4"),
                    new HpLessTransition(.50, "Worthy")
                     ),
                new State("Basic4",
                    new Shoot(10, count: 2, projectileIndex: 4, coolDown: 5000, predictive: 0.6, shootAngle: 100),
                    new Shoot(180, count: 3, projectileIndex: 7, coolDown: 100, shootAngle: 120, fixedAngle: 0, rotateAngle: -10),
                    new TimedTransition(7500, "Battle"),
                    new HpLessTransition(.50, "Worthy")
                    ),
                new State("Worthy",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You will pay for this."),
                    new HealSelf(9999999, 5000000),
                    new TimedTransition(2500, "Beyond")
                    ),
                new State("Beyond",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xffffff, 5, 5),
                     new OrderOnce(30, "Zhin", "Attack2"),
                    new TimedTransition(2000, "HAttack1")
                      ),
                new State("HAttack1",
                  new MoveTo(10, 98, 57),
                  new ConditionalEffect(ConditionEffectIndex.Armored),
                  new Shoot(180, count: 1, projectileIndex: 2, coolDown: 1500, shootAngle: 10, fixedAngle: 225, rotateAngle: -45, coolDownOffset: 550),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 20, fixedAngle: 225, rotateAngle: -45, coolDownOffset: 750),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 30, fixedAngle: 225, rotateAngle: -45, coolDownOffset: 950),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 40, fixedAngle: 225, rotateAngle: -45, coolDownOffset: 1150),
                  new HpLessTransition(.70, "BAttack1"),
                  new TimedTransition(11000, "HAttack2")
                        ),
                new State("HAttack2",
                  new MoveTo(10, 98, 71),
                  new ConditionalEffect(ConditionEffectIndex.Armored),
                  new Shoot(180, count: 1, projectileIndex: 2, coolDown: 1500, shootAngle: 10, fixedAngle: 315, rotateAngle: -45, coolDownOffset: 550),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 20, fixedAngle: 315, rotateAngle: -45, coolDownOffset: 750),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 30, fixedAngle: 315, rotateAngle: -45, coolDownOffset: 950),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 40, fixedAngle: 315, rotateAngle: -45, coolDownOffset: 1150),
                  new HpLessTransition(.70, "BAttack1"),
                  new TimedTransition(11000, "HAttack3")
                         ),
                new State("HAttack3",
                  new MoveTo(10, 84, 71),
                  new ConditionalEffect(ConditionEffectIndex.Armored),
                  new Shoot(180, count: 1, projectileIndex: 2, coolDown: 1500, shootAngle: 10, fixedAngle: 225, rotateAngle: 45, coolDownOffset: 550),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 20, fixedAngle: 225, rotateAngle: 45, coolDownOffset: 750),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 30, fixedAngle: 225, rotateAngle: 45, coolDownOffset: 950),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 40, fixedAngle: 225, rotateAngle: 45, coolDownOffset: 1150),
                  new HpLessTransition(.70, "BAttack1"),
                  new TimedTransition(11000, "HAttack4")
                         ),
                new State("HAttack4",
                  new MoveTo(10, 84, 57),
                  new ConditionalEffect(ConditionEffectIndex.Armored),
                  new Shoot(180, count: 1, projectileIndex: 2, coolDown: 1500, shootAngle: 10, fixedAngle: 315, rotateAngle: 45, coolDownOffset: 550),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 20, fixedAngle: 315, rotateAngle: 45, coolDownOffset: 750),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 30, fixedAngle: 315, rotateAngle: 45, coolDownOffset: 950),
                  new Shoot(180, count: 2, projectileIndex: 2, coolDown: 1500, shootAngle: 40, fixedAngle: 315, rotateAngle: 45, coolDownOffset: 1150),
                  new HpLessTransition(.70, "BAttack1"),
                  new TimedTransition(11000, "HAttack1")
                      ),
                new State("BAttack1",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new MoveToSpawn(),
                    new Taunt("Tch!"),
                  new TimedTransition(3000, "BAttack2")
                      ),
                new State("BAttack2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Shoot(180, count: 1, projectileIndex: 3, coolDown: 1200, shootAngle: 0, coolDownOffset: 550, predictive: 0.2),
                    new Shoot(180, count: 2, projectileIndex: 6, coolDown: 100, shootAngle: 180, fixedAngle: 315, rotateAngle: 10, coolDownOffset: 350),
                    new Shoot(180, count: 3, projectileIndex: 7, coolDown: 500, shootAngle: 10, fixedAngle: 315, rotateAngle: 40, coolDownOffset: 250),
                    new HpLessTransition(.50, "BAttack3"),
                    new TimedTransition(8000, "BAttack22")
                        ),
                 new State("BAttack22",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Shoot(180, count: 1, projectileIndex: 3, coolDown: 1200, shootAngle: 0, coolDownOffset: 550, predictive: 0.2),
                    new Shoot(180, count: 2, projectileIndex: 6, coolDown: 100, shootAngle: 180, fixedAngle: 315, rotateAngle: 10, coolDownOffset: 350),
                    new Shoot(180, count: 3, projectileIndex: 7, coolDown: 500, shootAngle: 10, fixedAngle: 315, rotateAngle: 40, coolDownOffset: 250),
                    new HpLessTransition(.50, "BAttack3"),
                   new TimedTransition(8000, "BAttack2")
                        ),
                new State("BAttack3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You.... You waste so much of my time!"),
                    new PlayerTextTransition("WHATDIDYOUSAY", "lol", 24),
                    new TimedTransition(2500, "BAttack4")
                         ),
                new State("WHATDIDYOUSAY",
                    new Taunt("You dare laugh at me?"),
                    new Grenade(30, 10, 50, 0, coolDown: 9999999, ConditionEffectIndex.Hallucinating, 9999999),
                    new Grenade(30, 10, 50, 0, coolDown: 9999999, ConditionEffectIndex.Healing, 9999999),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "BAttack4")
                        ),
                new State("BAttack4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("You end is near."),
                     new Spawn("Rave Clone UP", 1, 1, 99999, true),
                     new Spawn("Rave Clone DOWN", 1, 1, 99999, true),
                     new Spawn("Rave Clone LEFT", 1, 1, 99999, true),
                     new Spawn("Rave Clone RIGHT", 1, 1, 99999, true),
                     new TimedTransition(3000, "BAttack5")
                      ),
                new State("BAttack5",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("All of this humiliation from a mere mortal!"),
                      new TimedTransition(2000, "BAttack6")
                       ),
                new State("BAttack6",
                     new Shoot(180, count: 3, projectileIndex: 5, coolDown: 100, shootAngle: 120, fixedAngle: 8, rotateAngle: -5, coolDownOffset: 250),
                     new Shoot(180, count: 1, projectileIndex: 4, coolDown: 550, shootAngle: 0, coolDownOffset: 450, predictive: 0.4),
                     new Shoot(180, count: 1, projectileIndex: 2, coolDown: 300, shootAngle: 0, coolDownOffset: 550, predictive: 0.4),
                     new HpLessTransition(.25, "CAttack1"),
                     new TimedTransition(8000, "BAttack66")
                       ),
                new State("BAttack66",
                     new Shoot(180, count: 3, projectileIndex: 5, coolDown: 100, shootAngle: 120, fixedAngle: 8, rotateAngle: 5, coolDownOffset: 250),
                     new Shoot(180, count: 1, projectileIndex: 4, coolDown: 550, shootAngle: 0, coolDownOffset: 450, predictive: 0.4),
                     new Shoot(180, count: 1, projectileIndex: 2, coolDown: 300, shootAngle: 0, coolDownOffset: 550, predictive: 0.4),
                     new HpLessTransition(.25, "CAttack1"),
                     new TimedTransition(8000, "BAttack6")
                        ),
                new State("CAttack1",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("..."),
                       new OrderOnce(30, "Rave Clone UP", "Nothing"),
                         new OrderOnce(30, "Rave Clone DOWN", "Nothing"),
                           new OrderOnce(30, "Rave Clone LEFT", "Nothing"),
                             new OrderOnce(30, "Rave Clone RIGHT", "Nothing"),
                               new OrderOnce(30, "Zhin, The Demonic Sacrifice Clone", "Nothing"),
                       new TimedTransition(2000, "CAttack3")
                       ),
                new State("CAttack3",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new PlayerWithinTransition(8, "CAttack4"),
                         new TimedTransition(6500, "CAttack6")
                           ),
                new State("CAttack4",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("Hahaha..."),
                     //new TeleportPlayer(40, 91, 67, true),
                     //new OrderOnce(30, "C Fire Spawner YES", "SPAWNNOW"),
                    new TimedTransition(1250, "CAttack5")
                         ),
                new State("CAttack5",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("It's just you and me now! Feel my wrath!"),
                      new OrderOnce(30, "Rave Clone UP", "Attack"),
                         new OrderOnce(30, "Rave Clone DOWN", "Attack"),
                           new OrderOnce(30, "Rave Clone LEFT", "Attack"),
                             new OrderOnce(30, "Rave Clone RIGHT", "Attack"),
                               new OrderOnce(30, "Zhin, The Demonic Sacrifice Clone", "Shoot"),
                      new TimedTransition(2000, "CAttack6")
                      ),
                new State("CAttack6",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 1500),
                    new HpLessTransition(.05, "Done"),
                    new Shoot(180, count: 1, projectileIndex: 7, coolDown: 300, shootAngle: 120, fixedAngle: 0, rotateAngle: 21, coolDownOffset: 250),
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                     new Shoot(180, count: 3, projectileIndex: 6, coolDown: 100, shootAngle: 120, fixedAngle: 8, rotateAngle: -5, coolDownOffset: 250),
                       new TimedTransition(8000, "CAttack7")
                        ),
                new State("CAttack7",
                    new HpLessTransition(.05, "Done"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Grenade(2, 200, 3, 0, coolDown: 999999),
                    new Grenade(2, 200, 3, 90, coolDown: 999999),
                    new Grenade(2, 200, 3, 180, coolDown: 999999),
                    new Grenade(2, 200, 3, 270, coolDown: 999999),
                     new TimedTransition(2000, "CAttack8")
                       ),
                new State("CAttack8",
                    new HpLessTransition(.05, "Done"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                     new Grenade(2, 200, 3, 45, coolDown: 999999),
                    new Grenade(2, 200, 3, 135, coolDown: 999999),
                    new Grenade(2, 200, 3, 225, coolDown: 999999),
                    new Grenade(2, 200, 3, 315, coolDown: 999999),
                    new TimedTransition(1000, "CAttack9")
                     ),
                new State("CAttack9",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new HpLessTransition(.05, "Done"),
                    new Shoot(180, count: 3, projectileIndex: 6, coolDown: 100, shootAngle: 120, fixedAngle: 8, rotateAngle: -5, coolDownOffset: 250),
                    new Shoot(180, count: 3, projectileIndex: 7, coolDown: 300, shootAngle: 120, fixedAngle: 0, rotateAngle: 21, coolDownOffset: 250),
                    new TimedTransition(8000, "CAttack6")
                     ),
                new State("Done",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("Who would have thought, a mere mortal..."),
                      new OrderOnce(30, "Cultist Fire", "Done"),
                    new TimedTransition(2000, "Done3")
                           ),
                new State("Done3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Taunt("..."),
                     new TimedTransition(2000, "Done4")
                     ),
                new State("Done4",
                    new Suicide()
                    )
                   ),
                new Threshold(0.0001,
                    new ItemLoot("Mark of Rave Darkmore", 1),
                    new TierLoot(13, ItemType.Weapon, 0.015),
                    new TierLoot(14, ItemType.Armor, 0.015),
                    new TierLoot(7, ItemType.Ability, 0.15),
                    new TierLoot(6, ItemType.Ring, 0.05),
                    new ItemLoot("Greater Potion of Mana", 1),
                    new ItemLoot("Greater Potion of Life", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 1),
                    new ItemLoot("50 Credits", 1),
                    new ItemLoot("50 Credits", 0.1),
                    new ItemLoot("Light Armor Schematic", 0.005, damagebased: true),
                    new ItemLoot("Robe Schematic", 0.005, damagebased: true),
                    new ItemLoot("Heavy Armor Schematic", 0.005, damagebased: true),
                    new ItemLoot("Special Crate", 0.35),
                    new ItemLoot("Miscellaneous Crate", 0.15),
                    new ItemLoot("Rave's Sacrificial Staff", 0.0006, damagebased: true, threshold: 0.02),
                    new ItemLoot("Bloody Ritual robe", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Skull of the Demonic Bearer", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Blood Essence Ring", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Pumpkin Plant Bow", 0.0006, damagebased: true, threshold: 0.02),
                    new ItemLoot("Seal of a Evil Spirit", 0.006, damagebased: true)
                )
            )
            





        #endregion

        ;
    }
}