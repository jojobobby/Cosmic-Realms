using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Moon = () => Behav()

        #region Eternal the devourer

           .Init("Eternal, The Devourer",
                new State(
                    new StayCloseToSpawn(0.8, 12),
                    new ScaleHP2(30, 3, 15),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(15, "2")
                        ),
                    new State("2",
                         new ReproduceChildren(3, .5, 5000, "Zeraphic, Guardians of Eternal"),
                        new Taunt("You invited yourself to the moon, now witness my power."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1000, "3")
                        ),
                    new State("3",
                         new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 5, coolDown: 400),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new Shoot(20, 1, projectileIndex: 0, predictive: 1, coolDown: 500),
                         new Shoot(15, 3, 8, 1, predictive: 1, coolDown: 6000),
                         new Shoot(20, 16, 22.5, 4, 360, coolDown: 5000),
                         new EntityNotExistsTransition("Zeraphic, Guardians of Eternal", 20, "4")
                        ),
                    new State("4",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You are powerless against me!"),
                        new TimedTransition(1000, "5")
                        ),
                    new State("5",
                        new Follow(0.5, range: 40),
                         new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 5, coolDown: 400),
                         new Shoot(20, 1, projectileIndex: 0, predictive: 1, coolDown: 1000),
                         new Shoot(15, 3, 8, 1, predictive: 1, coolDown: 3000),
                         new Shoot(20, 8, 45, 3, 360, coolDown: 2000),
                         new Shoot(5, 2, 15, 2, predictive: 1, coolDown: 2000),
                         new HpLessTransition(.75, "6")
                        ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("DEATH AWAITS YOU EARTHLINGS, I WORK FOR ORYX! THE RULER OF THE GALAXY!"),
                       
                        new TimedTransition(1000, "7")
                        ),
                    new State("7",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(1),
                        new ReproduceChildren(3, .5, 5000, "Zeraphic, Guardians of Eternal"),
                        new StayAbove(1, 1),
                         new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 5, coolDown: 400),
                        new Shoot(20, 1, projectileIndex: 0, predictive: 1, coolDown: 1000),
                        new Shoot(15, 3, 8, 1, predictive: 1, coolDown: 3000),
                        new Shoot(20, 8, 45, 3, 360, coolDown: 2000),
                        new Shoot(5, 2, 15, 2, predictive: 1, coolDown: 2000),
                        new HpLessTransition(.50, "8")
                        ),
                    new State("8",
                       new ReproduceChildren(3, .5, 5000, "Zeraphic, Guardians of Eternal"),
                        new Wander(0.4),
                        new Shoot(20, 8, 45, 3, 360, coolDown: 2000),
                        new HpLessTransition(.20, "9")
                        ),
                    new State("9",
                        new Wander(0.4),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("The moon is my home! Earth is for fools!"),
                        new Shoot(20, 8, 45, 3, 360, coolDown: 1000),
                         new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 5, coolDown: 400),
                        new Shoot(20, 16, 22.5, 4, 360, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(20, 16, 22.5, 4, 360, coolDown: 1000, coolDownOffset: 1000),
                        new TimedTransition(1000, "10")
                        ),
                    new State("10",
                        new Follow(0.5, range: 40),
                        new Wander(0.4),
                         new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 5, coolDown: 400),
                        new Shoot(20, 1, projectileIndex: 0, predictive: 1, coolDown: 1000),
                         new Shoot(15, 3, 8, 1, predictive: 1, coolDown: 3000),
                         new Shoot(20, 8, 45, 3, 360, coolDown: 2000),
                         new Shoot(20, 16, 22.5, 4, 360, coolDown: 3500),
                         new Shoot(5, 2, 15, 2, predictive: 1, coolDown: 2000)
                             )
                        ),
                    new Threshold(0.02,
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),
                    new ItemLoot("Eye of Celestial Vision", .004, damagebased: true),
                    new ItemLoot("Winged Lunar Beetle Wings", .004, damagebased: true),
                    new ItemLoot("Nails of the Beast", .004, damagebased: true),
                    new ItemLoot("Eternal Pain", .004, damagebased: true),
                    new ItemLoot("Fragment of the Moon", .01, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .15, damagebased: true),
                    new ItemLoot("Moon Coin", .25, damagebased: true),
                    new ItemLoot("Moon Fragments", .30, damagebased: true)
                        )
                   )
                   .Init("Zeraphic, Guardians of Eternal",
                       new State(
                           new State("1",
                           new Orbit(0.4, 3.5),
                           new HealEntity(20, "Eternal, The Devourer", 80, 1000),
                           new Shoot(24, 8, 45, 0, 360, coolDown: 5000),
                           new Shoot(5, 1, projectileIndex: 1, predictive: 0.8, coolDown: 1000)
                               ),
                           new State("2",
                               new Follow(1.5, range: 16),
                               new HealEntity(1, "Eternal, The Devourer", 80, coolDown: 999999999),
                               new Shoot(10, 1, projectileIndex: 2, predictive: 1, coolDown: 1000)
                           )
                              
                        )
                       )

        #endregion

        #region Jade rabbit
           .Init("Running Jade Baby Rabbit",
            new State(
                    new State("fight1",
                        new Taunt(0.20, "Mommy!", "MOM!", "*SOB*"),
                        new Wander(0.5),
                            new StayBack(1, 8),
                            new TimedTransition(8000, "despawn")
                            ),
                     new State("despawn",
                       new Suicide()
                        )
                    )
                        )

            .Init("The Jade Baby Rabbit",
            new State(
                    new State("fight1",
                       new Prioritize(
                            new Follow(0.5, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 500),
                        new TimedTransition(4000, "fight2")
                        ),
                    new State("fight2",
                        new Taunt(0.20, "Mom, I want to help!", "Dont worry mommy!", "Mom dont die!", "We love you mom!", "We'll help you mom!"),
                       new Prioritize(
                            new Orbit(0.3, 3, 10, "The Jade Rabbit", speedVariance: 0.1, radiusVariance: 0.5),
                            new Charge(1, 10, coolDown: 2000),
                            new Wander(0.5)
                            ),
                       new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 1200),
                       new TimedTransition(4000, "fight1")
                        )
                    )
                 )
         .Init("The Jade Rabbit",
                new State(
                    new StayCloseToSpawn(0.8, 12),
                    new ScaleHP2(40, 3, 15),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("default",
                        new PlayerWithinTransition(8, "taunt1")
                        ),
                     new State("taunt1",
                        new Spawn("Circle Rabbit", 1, 1, 99999),
                        new Taunt(1.00, "Who? Who are you? Humans?"),
                        new TimedTransition(6000, "fight1")
                         )
                        ),
                     new State(
                    new State("fight1",
                        new Wander(0.4),
                        new HpLessTransition(0.6, "rage"),
                        new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 1000),
                        new Shoot(10, count: 3, shootAngle: 14, projectileIndex: 1, coolDown: 1500, coolDownOffset: 500),
                        new Shoot(10, count: 7, shootAngle: 16, projectileIndex: 0, coolDown: 3000, coolDownOffset: 1500),
                        new TimedTransition(5250, "fight2")
                        ),
                     new State("fight2",
                         new HpLessTransition(0.6, "rage"),
                        new Taunt(1.00, "Children stay behind me!", "No need to worry my children", "Mother will protect you my babies!"),
                        new Prioritize(
                            new Follow(0.65, 8, 1),
                            new Wander(0.5)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 1500, coolDownOffset: 500),
                        new Shoot(10, count: 8, projectileIndex: 3, coolDown: 1000, fixedAngle: 0),
                        new Shoot(10, count: 6, projectileIndex: 3, coolDown: 1500, coolDownOffset: 1500, fixedAngle: 60),
                        new Shoot(10, count: 4, projectileIndex: 3, coolDown: 2500, coolDownOffset: 2500, fixedAngle: 0),
                        new Shoot(10, count: 3, shootAngle: 6, projectileIndex: 1, coolDown: 4000),
                        new TimedTransition(6500, "fight3")
                        ),
                    new State("fight3",
                        new Orbit(0.6, 6, 15, "Circle Rabbit"),
                        new HpLessTransition(0.6, "rage"),
                        new Shoot(10, count: 6, projectileIndex: 2, coolDown: 200, fixedAngle: 0),
                        new Shoot(10, count: 6, projectileIndex: 0, coolDown: 1500, fixedAngle: 0),
                        new Grenade(5, 150, range: 5, coolDown: 2000),
                        new TimedTransition(6500, "fight4")
                        ),
                     new State("fight4",
                        new Prioritize(
                            new Charge(1, range: 10, coolDown: 3000),
                            new Follow(0.55, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 1500),
                        new Shoot(10, count: 6, projectileIndex: 3, coolDown: 1000, fixedAngle: 0),
                        new Shoot(10, count: 3, shootAngle: 8, projectileIndex: 4, coolDown: 1500, coolDownOffset: 600),
                        new TimedTransition(6000, "fight1")
                        ),
                     new State("rage",
                         new ReproduceChildren(5, .5, 5000, "The Jade Baby Rabbit"),
                        new HpLessTransition(0.10, "dead"),
                        new Flash(0xFF0000, 1, 2),
                        new Orbit(0.6, 6, 15, "Circle Rabbit"),
                        new Taunt("I dont think i'll be able to win."),
                        new Prioritize(
                            new Charge(1, range: 10, coolDown: 3000),
                            new Follow(0.75, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 3, shootAngle: 10, projectileIndex: 1, coolDown: 2000),
                        new Shoot(10, count: 6, projectileIndex: 2, coolDown: 1600),
                        new Shoot(10, count: 6, shootAngle: 8, projectileIndex: 4, coolDown: 3000, coolDownOffset: 1000)
                           ),
                     new State("dead",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 1, 2),
                        new Taunt("Good bye my children.", "Run my children! Dont look back!"),

                            new Spawn("Running Jade Baby Rabbit", 3, 3, 99999),
                            new TimedTransition(2000, "dead2")
                             ),
                     new State("dead2",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 1, 2),
                           new Suicide()
                         ))
                    ),
                   new Threshold(0.02,
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),
                    new ItemLoot("Glowing Moonstone Aegis", .004, damagebased: true),
                    new ItemLoot("Seal of Planatary Unification", .001, damagebased: true),
                    new ItemLoot("Heavy Moon Gem Sword", .004, damagebased: true),
                    new ItemLoot("Chained Rabbit's Foot", .001, damagebased: true),
                    new ItemLoot("Fragment of the Moon", .01),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .15, damagebased: true),
                    new ItemLoot("Moon Coin", .25, damagebased: true),
                    new ItemLoot("Moon Fragments", .30, damagebased: true),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02)
               
                  
                )
            )


        #endregion

        #region Celestial Observer
       .Init("Celestial Observer",
        new State(
             new ScaleHP2(50, 3, 15),
            new StayCloseToSpawn(0.8, 5),
            new Follow(1, range: 7),
            new Wander(0.4),
            new State("Part One",
                new Shoot(20, projectileIndex: 3, count: 12, shootAngle: 30, predictive: 0, coolDown: 1000),
                new Shoot(20, projectileIndex: 0, count: 3, shootAngle: 15, predictive: 0, coolDown: 1050),
                new Taunt("Twilight!"),
                new HpLessTransition(0.8, "Part Two")
                ),
            new State("Part Two",
                new Shoot(20, projectileIndex: 1, count: 3, shootAngle: 15, predictive: 0, coolDown: 1550),
                new TimedTransition(3100, "Alter"),
                new HpLessTransition(0.48, "Part Three")
                ),
            new State("Alter",
                new Shoot(20, projectileIndex: 2, count: 3, shootAngle: 15, predictive: 0, coolDown: 1000),
                new TimedTransition(2000, "Part Two"),
                new HpLessTransition(0.48, "Part Three")
                ),
            new State("Part Three",
                new Shoot(20, projectileIndex: 4, count: 4, shootAngle: 12, predictive: 0, coolDown: 1000),
                new Shoot(20, projectileIndex: 3, count: 12, shootAngle: 30, predictive: 0, coolDown: 1000),
                new TimedTransition(2000, "Alter Two"),
                new HpLessTransition(0.15, "Part Four")
                ),
            new State("Alter Two",
                new Shoot(20, projectileIndex: 4, count: 4, shootAngle: 12, predictive: 0, coolDown: 1000),
                new TimedTransition(2000, "Part Three"),
                new HpLessTransition(0.15, "Part Four")
                ),
            new State("Part Four",
                new Taunt("Darkness and Twilight!"),
                 new Shoot(20, projectileIndex: 3, count: 12, shootAngle: 30, predictive: 0, coolDown: 1000),
                new Shoot(20, projectileIndex: 3, count: 10, shootAngle: 29, predictive: 0, coolDown: 1000),
                new TimedTransition(2000, "Alter Three")
                ),
            new State("Alter Three",
                 new Shoot(20, projectileIndex: 3, count: 12, shootAngle: 30, predictive: 0, coolDown: 1000),
                new Shoot(20, projectileIndex: 4, count: 5, shootAngle: 16, predictive: 0, coolDown: 1000),
                new TimedTransition(2000, "Part Four")
                )
            
            ),
                new Threshold(0.01,
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),
                    new ItemLoot("Bow of The Lunar Crescent", .01, damagebased: true),
                    new ItemLoot("Moon Essence Trap", .004, damagebased: true),
                    new ItemLoot("Re's Wrath", .001, damagebased: true),
                    new ItemLoot("Fragment of the Moon", .01, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .25, damagebased: true),
                    new ItemLoot("Moon Coin", .35, damagebased: true),
                    new ItemLoot("Moon Fragments", .50, damagebased: true),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02)
                )
            )
        


        
       





        #endregion

        #region Lotll

            .Init("Lunar: Lord of the Lost Lands",
                new State(
                    new ScaleHP2(40, 3, 15),
                    new State("Waiting",
                        new HpLessTransition(0.99, "Start1.0")
                        ),
                    new State("Start1.0",
                        new HpLessTransition(0.1, "Dead"),
                        new State("Start",
                            new SetAltTexture(0),
                            new Prioritize(
                                new Wander(0.8)
                                ),
                            new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 180, coolDown: 2000),
                            new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 0, coolDown: 2000),
                            new TossObject("Lunar: Guardian of the Lost Lands", 5, coolDown: 2000, throwEffect: true, minAngle: 0, maxAngle: 360, minRange: 2, maxRange: 5),
                            new TimedTransition(100, "Spawning Guardian")
                            ),
                        new State("Spawning Guardian",
                            new TossObject("Lunar: Guardian of the Lost Lands", 5, coolDown: 2000, throwEffect: true, minAngle: 0, maxAngle: 360, minRange: 2, maxRange: 5),
                            new TimedTransition(3100, "Attack")
                            ),
                        new State("Attack",
                            new SetAltTexture(0),
                            new Wander(0.8),
                            new PlayerWithinTransition(1, "Follow"),
                            new TimedTransition(10000, "Gathering"),
                            new State("Attack1.0",
                                new TimedRandomTransition(3000, false,
                                    "Attack1.1",
                                    "Attack1.2"),
                                new State("Attack1.1",
                                    new Shoot(12, count: 7, shootAngle: 10, coolDown: 2000),
                                    new Shoot(12, count: 7, shootAngle: 190, coolDown: 2000),
                                    new TimedTransition(2000, "Attack1.0")
                                    ),
                                new State("Attack1.2",
                                    new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 180, coolDown: 3000),
                                    new Shoot(0, count: 7, shootAngle: 10, fixedAngle: 0, coolDown: 3000),
                                    new TimedTransition(2000, "Attack1.0")
                                    )
                                )
                            ),
                        new State("Follow",
                            new Prioritize(
                                new Follow(1, 20, 3),
                                new Wander(0.4)
                                ),
                            new Shoot(20, count: 7, shootAngle: 10, coolDown: 1300),
                            new TimedTransition(5000, "Gathering")
                            ),
                        new State("Gathering",
                            new Taunt(0.99, "Gathering power!"),
                            new TimedTransition(2000, "Gathering1.0")
                            ),
                        new State("Gathering1.0",
                            new TimedTransition(5000, "Protection5"),
                            new State("Gathering1.1",
                                new Shoot(30, 4, fixedAngle: 90, projectileIndex: 1, coolDown: 2000),
                                new TimedTransition(1500, "Gathering1.2")
                                ),
                            new State("Gathering1.2",
                                new Shoot(30, 4, fixedAngle: 45, projectileIndex: 1, coolDown: 2000),
                                new TimedTransition(1500, "Gathering1.1")
                                )
                                 ),
                        new State("Protection5",
                            new SetAltTexture(1),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                             new TimedTransition(250, "Protection4")
                                 ),
                        new State("Protection4",
                            new SetAltTexture(2),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new TimedTransition(250, "Protection3")
                            ),
                        new State("Protection3",
                            new SetAltTexture(3),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 0, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 45, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 90, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 135, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 180, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 225, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 270, coolDown: 50000, throwEffect: true),
                            new TossObject("Lunar: Protection Crystal", 4, angle: 315, coolDown: 50000, throwEffect: true),
                            new EntityExistsTransition("Lunar: Protection Crystal", 10, "Waiting")
                            )
                        ),
                    new State("Waiting",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(4),
                        new EntityNotExistsTransition("Lunar: Protection Crystal", 10, "Start1.0")
                        ),
                    new State("Dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(5),
                        new Taunt(0.99, "THE GODDESS OF THE MOON SHALL HAVE REVENGE!"),
                        new Flash(0xFF0000, .1, 1000),
                        new TimedTransition(2000, "Suicide")
                        ),
                    new State("Suicide",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                        new Shoot(0, 8, fixedAngle: 360 / 8, projectileIndex: 1),
                        new Suicide()
                        )
                ),
                new Threshold(0.01,
                    new ItemLoot("Moon Medallion", .01),
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),
                    new ItemLoot("Fragment of the Moon", .01, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .15, damagebased: true),
                    new ItemLoot("Moon Coin", .25, damagebased: true),
                    new ItemLoot("Moon Fragments", .30, damagebased: true),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Starlight Ring", .001, damagebased: true),
                    new ItemLoot("Robe of Translunary", .004, damagebased: true),
                    new ItemLoot("Moon-blind Skull", .01, damagebased: true),
                    new ItemLoot("Leading Light", .004, damagebased: true)
                    )
            )
          .Init("Lunar: Protection Crystal",
                new State(
                    new Prioritize(
                        new Orbit(0.3, 4, 10, "Lunar: Lord of the Lost Lands")
                        ),
                    new Shoot(8, count: 4, shootAngle: 7, coolDown: 500)
                    )
            )
            .Init("Lunar: Guardian of the Lost Lands",
                new State(
                    new State("Full",
                        new Prioritize(
                            new Follow(0.6, 20, 6),
                            new Wander(0.2)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, coolDown: 3000, projectileIndex: 1),
                        new Shoot(10, count: 5, shootAngle: 10, coolDown: 1500),
                        new HpLessTransition(0.25, "Low")
                        ),
                    new State("Low",
                        new Prioritize(
                            new StayBack(0.6, 5),
                            new Wander(0.2)
                            ),
                        new Shoot(10, count: 8, fixedAngle: 360 / 8, coolDown: 3000, projectileIndex: 1),
                        new Shoot(10, count: 5, shootAngle: 10, coolDown: 1500)
                        )
                    ),
                new ItemLoot("Health Potion", 1),
                new ItemLoot("Magic Potion", 1)
            )
        #endregion

        #region Sphinx

            .Init("Lunar: Grand Sphinx",
                new State(
                    new ScaleHP2(40, 3, 15),
                    new State("Spawned",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Reproduce("Horrid Reaper", 30, 4, coolDown: 100),
                        new TimedTransition(500, "Attack1")
                        ),
                    new State("Attack1",
                        new Prioritize(
                            new Wander(0.5)
                            ),
                        new Shoot(12, count: 1, coolDown: 800, coolDownOffset: 500),
                        new Shoot(12, count: 3, shootAngle: 10, coolDown: 1000),
                        new Shoot(12, count: 1, shootAngle: 130, coolDown: 1000),
                        new Shoot(12, count: 1, shootAngle: 230, coolDown: 1000),
                        new TimedTransition(6000, "TransAttack2")
                        ),
                    new State("TransAttack2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.5),
                        new Flash(0x00FF0C, .25, 8),
                        new Taunt(0.99, "I am only a reflection of the earth's grand sphinx. Trapped on this planet for eternity!"),
                        new TimedTransition(2000, "Attack2")
                        ),
                    new State("Attack2",
                        new Prioritize(
                            new Wander(0.5)
                            ),
                        new Shoot(0, count: 8, shootAngle: 10, fixedAngle: 0, rotateAngle: 70, coolDown: 2000,
                            projectileIndex: 1),
                        new Shoot(0, count: 8, shootAngle: 10, fixedAngle: 180, rotateAngle: 70, coolDown: 2000,
                            projectileIndex: 1),
                        new TimedTransition(6200, "TransAttack3")
                        ),
                    new State("TransAttack3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.5),
                        new Flash(0x00FF0C, .25, 8),
                        new TimedTransition(2000, "Attack3")
                        ),
                    new State("Attack3",
                        new Prioritize(
                            new Wander(0.5)
                            ),
                        new Shoot(20, count: 9, fixedAngle: 360 / 9, projectileIndex: 2, coolDown: 2300),
                        new TimedTransition(6000, "TransAttack1"),
                        new State("Shoot1",
                            new Shoot(20, count: 2, shootAngle: 4, projectileIndex: 2, coolDown: 700),
                            new TimedRandomTransition(1000, false,
                                "Shoot1",
                                "Shoot2"
                                )
                            ),
                        new State("Shoot2",
                            new Shoot(20, count: 8, shootAngle: 5, projectileIndex: 2, coolDown: 1100),
                            new TimedRandomTransition(1000, false,
                                "Shoot1",
                                "Shoot2"
                                )
                            )
                        ),
                    new State("TransAttack1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Wander(0.5),
                        new Flash(0x00FF0C, .25, 8),
                        new TimedTransition(2000, "Attack1"),
                        new HpLessTransition(0.15, "Order")
                        ),
                    new State("Order",
                        new Wander(0.5),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new OrderOnce(30, "Lunar: Horrid Reaper", "Die"),
                        new TimedTransition(1900, "Attack1")
                        )
                    ),
                   new Threshold(.01, 
                    new ItemLoot("Moon Medallion", .01),
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),

                    new ItemLoot("Dull Dimensional Orb", 0.004, damagebased: true),//Strange Cubic Dagger
                    new ItemLoot("Grand Amulet of Protection", 0.004, damagebased: true),
                    new ItemLoot("Strange Cubic Dagger", 0.001, damagebased: true),

                    new ItemLoot("Fragment of the Moon", .01, damagebased: true),
                     new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                        new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .15, damagebased: true),
                    new ItemLoot("Moon Coin", .25, damagebased: true),
                    new ItemLoot("Moon Fragments", .30, damagebased: true)
                        )
            )
            .Init("Lunar: Horrid Reaper",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("Move",
                        new Prioritize(
                            new StayCloseToSpawn(3, 10),
                            new Wander(3)
                            ),
                        new EntityNotExistsTransition("Lunar: Grand Sphinx", 50, "Die"), //Just to be sure
                        new TimedRandomTransition(2000, true, "Attack")
                        ),
                    new State("Attack",
                        new Shoot(0, count: 6, fixedAngle: 360 / 6, coolDown: 700),
                        new PlayerWithinTransition(2, "Follow"),
                        new TimedRandomTransition(5000, true, "Move")
                        ),
                    new State("Follow",
                        new Prioritize(
                            new Follow(0.35, 10, 3)
                            ),
                        new Shoot(7, count: 1, coolDown: 700),
                        new TimedRandomTransition(5000, true, "Move")
                        ),
                    new State("Die",
                        new Taunt(0.99, "[Reflection Fades Away]"),
                        new Decay(1000)
                        )
                    )
            )










        #endregion

        #region Cube God

          .Init("Lunar: Cube God",
                     new State(
                    new ScaleHP2(35, 2, 15),
                     new Wander(.2),
                    new StayAbove(0.2, 150),
                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2500),
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500),
                    new HpLessTransition(.70, "Start2")
                          ),
                    new State("Start2",
                    new Flash(0xffffff, 2, 4),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start3")
                        ),
                    new State("Start3",
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500),
                     new HpLessTransition(.30, "Start4")
                          ),
                    new State("Start4",
                    new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start5")
                          ),
                    new State("Start5",
                    new Shoot(25, 9, 10, 0, predictive: 1, coolDown: 750),
                    new Shoot(25, 4, 10, 1, predictive: 1, coolDown: 1500)
                        )
                ),
                new Threshold(.01,
                    new ItemLoot("Moon Medallion", .01),
                    new ItemLoot("Shimmering Lunar Robe", .01),
                    new ItemLoot("Shimmering Lunar Armor", .01),
                    new ItemLoot("Shimmering Lunar Hide", .01),

                    new ItemLoot("Cubic Amulet", 0.004, damagebased: true),
                    new ItemLoot("Empty Power Spell", 0.004, damagebased: true),//Dull Dimensional Orb

                    

                    new ItemLoot("Fragment of the Moon", .01, damagebased: true),
                      new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                        new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 1, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.50, damagebased: true),
                    new ItemLoot("Moon Trophy", .15, damagebased: true),
                    new ItemLoot("Moon Coin", .25, damagebased: true),
                    new ItemLoot("Moon Fragments", .30, damagebased: true)
                )
            )
        



        #endregion

        #region Moon Gods

         .Init("Assistant of the Cult",
                new State(
                    new Prioritize(
                        new Follow(0.6, 10, 3),
                        new StayAbove(1, 200),
                        new Wander(0.4)
                        ),

                    new Shoot(20, 2, projectileIndex: 0, shootAngle: 12, coolDown: 1000)
                    )
            )

               .Init("Commander of the Cult",
                new State(
                    new Prioritize(
                        new Follow(0.6, 10, 3),
                        new StayAbove(1, 200),
                        new Wander(0.4)
                        ),
                    new Shoot(22, 1, projectileIndex: 0, shootAngle: 12, predictive: 0.6, coolDown: 2500),
                    new Spawn("Assistant of the Cult", maxChildren: 3, coolDown: 14000, givesNoXp: false)
                    )
             ,
                new Threshold(.01,
                new ItemLoot("Moon Fragments", .05, damagebased: true),
                new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                new ItemLoot("Health Potion", 0.4),
                new ItemLoot("Magic Potion", 0.4)
                    )
            )



          .Init("Skeleton Astronaut", 
                new State(
                    new State("Normal",
                        new Prioritize(
                            new Follow(0.6, 10, 3),
                            new Wander(0.4)
                            ),
                        new Shoot(10, count: 4, shootAngle: 12, coolDown: new Cooldown(1200, 200), projectileIndex: 1),
                        new Shoot(7, count: 2, shootAngle: 12, projectileIndex: 2, coolDown: 1500),
                        new Shoot(7, count: 2, shootAngle: 12, projectileIndex: 2, coolDown: 1750, coolDownOffset: 100),
                        new Shoot(7, count: 2, shootAngle: 12, projectileIndex: 2, coolDown: 1900, coolDownOffset: 200),
                        new PlayerWithinTransition(1, "Explode")
                        ),
                    new State("Explode",
                        new Shoot(0, count: 10, fixedAngle: 36, projectileIndex: 1),
                        new Suicide()
                        )
                    )
            )

         .Init("Purple Moon Slime",
                new State(
                   new Follow(0.5, 9, 1),
                    new Shoot(10, 4, shootAngle: 12, 1, coolDown: 1500, predictive: 0.6),
                    new Shoot(10, 2, shootAngle: 12, 2, coolDown: 4500, predictive: 0.6)
                )
             ,
                new Threshold(.01,
                new ItemLoot("Moon Fragments", .05, damagebased: true),
                new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                new ItemLoot("Health Potion", 0.4),
                new ItemLoot("Magic Potion", 0.4)
            )
            )
       





          .Init("Mutated Venenum",
            new State(
                new Shoot(10, count: 5, shootAngle: 12, projectileIndex: 0, coolDown: new Cooldown(2600, 1000)),
                new Shoot(10, count: 1, projectileIndex: 0, coolDown: 4000),
                new State("movement1",
                     new Follow(0.5, 9, 1),
                     new TimedTransition(4600, "movement2")
                    ),
                new State("movement2",
                     new Swirl(0.4, 5),
                     new TimedTransition(4000, "movement1")
                    )
                         ),
                new Threshold(.01,
                new ItemLoot("Moon Fragments", .05, damagebased: true),
                new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                new ItemLoot("Health Potion", 0.4),
                new ItemLoot("Magic Potion", 0.4)
               )
                    )



         .Init("Actias Luna",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.4, 15),
                        new StayAbove(1, 200),
                        new Follow(0.2, range: 7),
                        new Wander(0.4)
                        ),
                    new Spawn("Actias Venti", maxChildren: 3, coolDown: 14000, givesNoXp: false),
                    new Shoot(10, count: 5, shootAngle: 20, predictive: 1, coolDown: 1000)
                    ),
                new Threshold(.01,
                new ItemLoot("Moon Fragments", .05, damagebased: true),
                new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                new ItemLoot("Health Potion", 0.4),
                new ItemLoot("Magic Potion", 0.4)
               )
            )
         .Init("Actias Venti",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.4, 15),
                        new StayAbove(1, 200),
                        new Follow(0.2, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, count: 2, shootAngle: 20, predictive: 0.3, coolDown: 500)
                    ),
                new Threshold(.01,
                new ItemLoot("Health Potion", 0.4),
                new ItemLoot("Magic Potion", 0.4)
               )
            )


            .Init("Lunar Protector",
                new State(
                    new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 5, predictive: 1, coolDown: 1200),
                    new Prioritize(
                        new StayCloseToSpawn(0.8, 12),
                        new Follow(1, range: 7),
                        new Wander(0.4)
                        ),
                    new Spawn("Lunar Helper", maxChildren: 3, coolDown: 1200, givesNoXp: false),
                    new Taunt(0.7, 10000,
                        "Remove yourselves!",
                        "You're not welcome here!"
                        )
                    ),
                new Threshold(.01,
                    new ItemLoot("Moon Fragments", .05, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4),
                    new TierLoot(4, ItemType.Ring, 0.07),
                    new TierLoot(4, ItemType.Ability, 0.17)
                )
            )
           .Init("Lunar Helper",
                new State(
                    new Prioritize(
                        new StayCloseToSpawn(1.4, 15),
                        new Follow(1.4, range: 7),
                        new Wander(0.4)
                        ),
                    new Shoot(10, count: 2, shootAngle: 7, predictive: 0.5, coolDown: 300)
                    ),
                 new ItemLoot("Health Potion", 0.2),
                 new ItemLoot("Magic Potion", 0.2)
            )

            .Init("Spinning Tornado",
                     new State(
                        new State("Circle",
                            new Wander(0.5),
                            new Swirl(0.7, 2, 16),
                            new Shoot(45, 8, projectileIndex: 0, coolDown: 500),
                            new Shoot(15, 3, projectileIndex: 1, coolDown: 1000),
                            new HpLessTransition(0.50, "Shoot")
                            ),
                        new State("Shoot",
                              new Wander(0.5),
                            new Swirl(0.7, 2, 16),
                         new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0xF0E68C, 1, 6),
                            new Shoot(45, 8, projectileIndex: 0, coolDown: 300),
                            new Shoot(15, 3, projectileIndex: 1, coolDown: 700)
                            )
                ),
                new Threshold(0.01,
                    new ItemLoot("Moon Fragments", .05, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                    new ItemLoot("Moon Fragments", .05, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )

          .Init("Spinning Tornado Anchor",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("suicide2",
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Wander(1),
                    new Follow(0.5, 14, 3),
                    new EntityNotExistsTransition("Spinning Tornado", 7, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                )
            )
            )

          .Init("Moon Statue",
                     new State(
                        new State("Circle",
                            new Grenade(4, 150, 10, null, 750),
                            new HpLessTransition(0.50, "Shoot")
                            ),
                        new State("Shoot",
                         new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Flash(0xF0E68C, 1, 6),
                            new Grenade(6, 200, 10, null, 750)
                            )
                ),
                new Threshold(0.01,
                    new ItemLoot("Moon Fragments", .05, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                    new ItemLoot("Moon Fragments", .05, damagebased: true),
                    new ItemLoot("Celestial Enhancer", 0.1, damagebased: true),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )

          .Init("Circle Rabbit",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("suicide2",
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new EntityNotExistsTransition("The Jade Rabbit", 20, "suicide")
                    ),
                new State("suicide",
                    new Suicide()
                )
            )
            )

        #endregion 
        ;

    }
}