using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Duelist = () => Behav()
            .Init("Zemith The Assassin",
                new State(
                    new ScaleHP2(50, 1, 15),
                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(9, "2")
                    ),
                    new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("So you're the realmers they talk about."),
                    new TimedTransition(3500, "3")
                    ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Taunt("They must not understand what power I have over you pathetic people."),
                    new Shoot(radius: 160, count: 4, fixedAngle: 25, rotateAngle: 25, coolDown: 1750, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 15, coolDown: 300, projectileIndex: 2),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 15, coolDown: 300, coolDownOffset: 200, projectileIndex: 3),
                    new HpLessTransition(0.90, "4")
                         ),
                    new State("20",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You should have waited!", "Foolish!"),
                        new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 10, coolDown: 300, projectileIndex: 2),
                        new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 10, coolDown: 300, coolDownOffset: 200, projectileIndex: 3),
                        new Grenade(20, 100, 1, 0, 99999, ConditionEffectIndex.Weak, 15000, 0xff0000),
                        new TimedTransition(2000, "5")
                    ),
                    new State("4",
                    new ConditionalEffect(ConditionEffectIndex.Solid),
                    new Taunt("You dare test me?"),
                    new SetAltTexture(1),
                    new Flash(0xFF0000, 3, 5),
                    new TimedTransition(5000, "5"),
                    new HpLessTransition(0.80, "20")
                    ),
                    new State("5",
                    new SetAltTexture(0),
                    new Charge(2, 15, 2500),
                    new Shoot(radius: 160, count: 4, fixedAngle: 25, rotateAngle: 5, coolDown: 3000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 50, rotateAngle: 5, coolDown: 3250, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 75, rotateAngle: 5, coolDown: 3500, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 100, rotateAngle: 5, coolDown: 3750, projectileIndex: 0),
                    new HpLessTransition(0.70, "6")
                    ),
                    new State("6",
                    new ReturnToSpawn(2),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("When do you stop! You are not and never will be a match for me!"),
                    new TimedTransition(3000, "7")
                    ),
                    new State("7",
                    new Spawn("C Fire Spawner YES", 1, 1, 99999, true),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("I will show you where our power differs... Prepare yourself."),
                    new TimedTransition(3000, "8")
                          ),
                    new State("8",
                    new Orbit(0.5, 5, 20, "C Fire Spawner YES"),
                    new Shoot(radius: 25, count: 1, coolDown: 300, projectileIndex: 1),
                    new Shoot(radius: 160, count: 4, fixedAngle: 25, rotateAngle: 5, coolDown: 2000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: -25, coolDown: 500, projectileIndex: 2),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: -25, coolDown: 500, coolDownOffset: 200, projectileIndex: 3),
                    new HpLessTransition(0.50, "91")
                           ),
                    new State("91",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(2),
                        new Taunt("You piss me off."),
                        new TimedTransition(3000, "9")
                          ),
                    new State("9",
                    new Wander(0.1),
                    new Follow(1, 10, 2),
                    new StayCloseToSpawn(1, 7),
                    new Shoot(radius: 120, count: 3, fixedAngle: 0, rotateAngle: 77, coolDown: 1000, projectileIndex: 0),
                    new Shoot(radius: 25, count: 3, coolDown: 750, projectileIndex: 0, predictive: 0.2),
                    new Shoot(radius: 25, count: 1, coolDown: 200, projectileIndex: 1, predictive: 0.2),
                    new TimedTransition(15000, "10"),
                    new HpLessTransition(0.20, "11")
                           ),
                    new State("10",
                    new ReturnToSpawn(1.4),
                    new Orbit(0.3, 7, 20, "C Fire Spawner YES"),
                    new Shoot(radius: 160, count: 1, coolDown: 300, projectileIndex: 1, predictive: 0.2),
                    new Shoot(radius: 160, count: 2, shootAngle: 160, fixedAngle: 41, rotateAngle: 55, coolDown: 400, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 25, coolDown: 500, projectileIndex: 2),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 25, coolDown: 500, coolDownOffset: 200, projectileIndex: 3),
                    new TimedTransition(15000, "9"),
                    new HpLessTransition(0.20, "11")
                           ),
                    new State("11",
                    new Taunt("No, I will never let you end me like this!"),
                    new Shoot(radius: 25, count: 3, coolDown: 750, projectileIndex: 0, predictive: 0.2),
                    new Shoot(radius: 25, count: 1, coolDown: 200, projectileIndex: 1, predictive: 0.2),
                    new Shoot(radius: 180, count: 2, fixedAngle: 0, rotateAngle: 10, shootAngle: 180, coolDown: 100, projectileIndex: 2),
                    new Shoot(radius: 180, count: 2, fixedAngle: 0, rotateAngle: 10, shootAngle: 180, coolDown: 100, coolDownOffset: 100, projectileIndex: 3),
                    new HpLessTransition(0.05, "12")
                           ),
                    new State("12",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("You... will never be forgotten or forgiven!"),
                    new SetAltTexture(3),
                    new TimedTransition(2000, "13")
                        ),
                    new State("13",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(2),
                    new TimedTransition(2500, "14")
                        ),
                    new State("14",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Suicide()
                )

                    ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Life", 0.5),
                    new ItemLoot("Greater Potion of Mana", 0.5),
                    new ItemLoot("Greater Potion of Critical Damage", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 1),
                    new ItemLoot("Greater Potion of Critical Damage", 0.1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.1),
                    new ItemLoot("Omnipotent Token x1", 0.075),
                    new ItemLoot("Everlasting Inferno", 0.0006),
                    new ItemLoot("Conflict's Elimination", 0.0006),
                    new ItemLoot("Revolting Flame Amulet", 0.0006),
                    new ItemLoot("Blazing Combustion", 0.0006),
                    new TierLoot(tier: 7, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 13, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
                )
            )

          .Init("Vargo the Supreme Mage",
                new State(
                    new ScaleHP2(100, 1, 15),
                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(9, "2")
                    ),
                    new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Hmm? Oh my..."),
                    new TimedTransition(2500, "3")
                    ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Your power seems to come from the ancient weapons you hold... I must not underestimate you."),
                    new TimedTransition(2500, "4")
                    ),
                    new State("4",
                    new Shoot(radius: 25, count: 8, shootAngle: 45, fixedAngle: 25, coolDown: 1500, projectileIndex: 0),
                    new Shoot(radius: 25, count: 1, shootAngle: null, fixedAngle: null, coolDown: 500, projectileIndex: 1),
                    new Shoot(radius: 25, count: 2, shootAngle: 20, coolDown: 300, projectileIndex: 7),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new HpLessTransition(0.80, "5")
                    ),
                    new State("5",
                    new Taunt("Say Goodbye."),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 10, coolDown: 200, projectileIndex: 6, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 10, coolDown: 200, projectileIndex: 7, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, coolDown: 1500, projectileIndex: 4),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, coolDown: 2000, projectileIndex: 5),
                    new HpLessTransition(0.60, "6")
                    ),
                    new State("6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("I see you were able to stand after that. Lets see how long you'll last."),
                    new Spawn("Red Orb Vargo", 1, 1, 99999, true),
                    new Spawn("Purple Orb Vargo", 1, 1, 99999, true),
                    new Spawn("Orange Orb Vargo", 1, 1, 99999, true),
                    new Spawn("Blue Orb Vargo", 1, 1, 99999, true),
                    new Spawn("Green Orb Vargo", 1, 1, 99999, true),
                    new TimedTransition(2000, "7")
                    ),
                    new State("7",
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 10, coolDown: 150, projectileIndex: 6, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 10, coolDown: 150, projectileIndex: 3, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 77, coolDown: 1000, projectileIndex: 4, predictive: 0.2),
                    new HpLessTransition(0.40, "8")
                          ),
                    new State("8",
                    new Shoot(radius: 25, count: 8, shootAngle: 45, fixedAngle: 25, coolDown: 2000, projectileIndex: 0),
                    new Shoot(radius: 25, count: 1, shootAngle: null, fixedAngle: null, coolDown: 400, projectileIndex: 1),
                    new Shoot(radius: 25, count: 2, shootAngle: 20, coolDown: 300, projectileIndex: 7, predictive: 0.2),
                    new HpLessTransition(0.20, "91")
                           ),
                    new State("91",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("REMARKABLE."),
                    new OrderOnce(200, "Red Orb Vergo", "wait"),
                    new OrderOnce(200, "Blue Orb Vergo", "wait"),
                    new OrderOnce(200, "Orange Orb Vergo", "wait"),
                    new OrderOnce(200, "Purple Orb Vergo", "wait"),
                    new OrderOnce(200, "Green Orb Vergo", "wait"),
                    new TimedTransition(2000, "9")
                          ),
                    new State("9",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Your determination is remarkable!"),
                    new TimedTransition(2000, "11")
                           ),
                    new State("11",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("But now I'm finished playing. You hold little value to your life!"),
                    new TimedTransition(2000, "12")
                           ),
                    new State("12",
                    new OrderOnce(200, "Red Orb Vergo", "fight"),
                    new OrderOnce(200, "Blue Orb Vergo", "fight"),
                    new OrderOnce(200, "Orange Orb Vergo", "fight"),
                    new OrderOnce(200, "Purple Orb Vergo", "fight"),
                    new OrderOnce(200, "Green Orb Vergo", "fight"),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 5, coolDown: 150, projectileIndex: 1, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 5, coolDown: 150, projectileIndex: 5, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 5, coolDown: 150, projectileIndex: 4, predictive: 0.2),
                    new Shoot(radius: 25, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 77, coolDown: 1500, projectileIndex: 3, predictive: 0.2),
                    new Shoot(radius: 25, count: 1, shootAngle: null, coolDown: 800, projectileIndex: 7),
                    new Shoot(radius: 25, count: 3, shootAngle: 25, coolDown: 2500, projectileIndex: 7),
                    new HpLessTransition(0.05, "13")
                          ),
                    new State("13",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("I cant... This is embarrassing, what a way to die... what remarkable determination you have."),
                    new TimedTransition(2500, "14")
                        ),
                    new State("14",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Suicide()
                )

                    ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Life", 0.5),
                    new ItemLoot("Greater Potion of Mana", 0.5),
                    new ItemLoot("Greater Potion of Critical Damage", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 1),
                    new ItemLoot("Greater Potion of Critical Damage", 0.1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.1),
                    new ItemLoot("Omnipotent Token x1", 0.075),
                    new ItemLoot("Fractal Nova", 0.0002),
                    new ItemLoot("Heretical Garments", 0.0006),
                    new ItemLoot("Infragilis", 0.0006),
                    new TierLoot(tier: 7, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
                )
            )

             .Init("Red Orb Vargo",
                new State(
                    new EntityNotExistsTransition("Vargo the Supreme Mage", 99,"dead"),
                new State("fight",
                    new Orbit(1, 16, 99, "Vargo the Supreme Mage"),
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(120, count: 3, projectileIndex: 0, coolDown: 2000)
                          ),
                  new State("wait",
                      new Orbit(1, 16, 99, "Vargo the Supreme Mage"),
                   new ConditionalEffect(ConditionEffectIndex.Invincible)
                        ),
                  new State("dead",
                      new Suicide()
                )
            )
            )
           .Init("Blue Orb Vargo",
                new State(
                      new EntityNotExistsTransition("Vargo the Supreme Mage", 99, "dead"),
                new State("fight",
                  new Orbit(1, 14, 99, "Vargo the Supreme Mage"),
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(120, count: 3, projectileIndex: 0, coolDown: 2000)
                          ),
                  new State("wait",
                       new Orbit(1, 14, 99, "Vargo the Supreme Mage"),
                   new ConditionalEffect(ConditionEffectIndex.Invincible)
                          ),
                  new State("dead",
                      new Suicide()
                )
            )
            )

           .Init("Green Orb Vargo",
                new State(
                      new EntityNotExistsTransition("Vargo the Supreme Mage", 99, "dead"),
                new State("fight",
                       new Orbit(1, 8, 99, "Vargo the Supreme Mage"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(120, count: 3, projectileIndex: 0, coolDown: 2000)
                          ),
                  new State("wait",
                       new Orbit(1, 8, 99, "Vargo the Supreme Mage"),
                   new ConditionalEffect(ConditionEffectIndex.Invincible)
                          ),
                  new State("dead",
                      new Suicide()
                )
            )
            )

           .Init("Orange Orb Vargo",
                new State(
                      new EntityNotExistsTransition("Vargo the Supreme Mage", 99, "dead"),
                new State("fight",
                     new Orbit(1, 12, 99, "Vargo the Supreme Mage"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(120, count: 3, projectileIndex: 0, coolDown: 2000)
                    ),
                  new State("wait",
                       new Orbit(1, 12, 99, "Vargo the Supreme Mage"),
                   new ConditionalEffect(ConditionEffectIndex.Invincible)
                          ),
                  new State("dead",
                      new Suicide()
                )
            )
            )

           .Init("Purple Orb Vargo",
                new State(
                    new EntityNotExistsTransition("Vargo the Supreme Mage", 99, "dead"),
                new State("fight",
                     new Orbit(1, 10, 99, "Vargo the Supreme Mage"),
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                     new Shoot(120, count: 3, projectileIndex: 0, coolDown: 2000)
                          ),
                   new State("wait",
                   new Orbit(1, 10, 99, "Vargo the Supreme Mage"),
                   new ConditionalEffect(ConditionEffectIndex.Invincible)
                           ),
                  new State("dead",
                      new Suicide()
                )

            )
            )


          .Init("Mage of the Crownguard",
                new State(
                    new ScaleHP2(100, 1, 15),
                    new State("Start",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(9, "2")
                    ),
                    new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Hmm."),
                    new TimedTransition(2000, "3")
                    ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("How did you manage to get this far?"),
                    new TimedTransition(2000, "4")
                         ),
                       new State("4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Master did not speak of any visitors."),
                    new TimedTransition(2000, "5")
                           ),
                    new State("5",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Flash(0xFF0000, 3, 5),
                    new Taunt("Very well then, I will eliminate you where you stand!"),
                    new TimedTransition(2000, "6")
                         ),
                    new State("6",
                    new Shoot(radius: 160, count: 3, fixedAngle: 0, rotateAngle: 50, shootAngle: 10, coolDown: 3000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 3, fixedAngle: 270, rotateAngle: 50, shootAngle: 10, coolDown: 3000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 3, fixedAngle: 180, rotateAngle: 50, shootAngle: 10, coolDown: 3000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 3, fixedAngle: 90, rotateAngle: 50, shootAngle: 10, coolDown: 3000, projectileIndex: 0),
                    new Shoot(radius: 120, count: 3, fixedAngle: null, shootAngle: 20, rotateAngle: null, coolDown: 1250, projectileIndex: 2),
                    new HpLessTransition(0.80, "7")
                    ),
                    new State("7",
                    new Shoot(radius: 160, count: 6, fixedAngle: 180, rotateAngle: 20, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new Shoot(radius: 160, count: 6, fixedAngle: 0, rotateAngle: 20, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                    new Shoot(radius: 120, count: 3, fixedAngle: null, shootAngle: 15, rotateAngle: null, coolDown: 350, projectileIndex: 1),
                    new HpLessTransition(0.60, "9")
                    ),
                    new State("9",
                    new ReturnToSpawn(2),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("You have tremendous skill. But I am still superior"),
                    new TimedTransition(2500, "10")
                    ),
                    new State("10",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Time to demonstrate."),
                    new TimedTransition(2500, "11")
                          ),
                    new State("11",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(radius: 120, count: 6, fixedAngle: 0, shootAngle: 60, rotateAngle: 3, coolDown: 100, projectileIndex: 2),
                    new Shoot(radius: 120, count: 3, fixedAngle: 0, shootAngle: 120, rotateAngle: 65, coolDown: 1000, projectileIndex: 0),
                    new Shoot(radius: 120, count: 1, fixedAngle: null, rotateAngle: null, coolDown: 500, projectileIndex: 3),
                    new Grenade(2, 10, 99, null, 2500, ConditionEffectIndex.Paralyzed, 1500, 0xFFFF00),
                    new TimedTransition(15500, "12")
                           ),
                    new State("12",
                    new Flash(0xFF0000, 3, 5),
                    new Taunt("Hahaha... watching you run is starting to get fun.", "Run for your lives you worthless humans!", "This is starting to take a toll on my body...", "Keep running!"),
                    new HpLessTransition(0.40, "13"),
                    new TimedTransition(5000, "11")
                               ),
                    new State("13",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("This is unforgiveable! You are done for."),
                    new TimedTransition(2500, "91")
                          ),
                    new State("91",
                    new Wander(0.1),
                    new Follow(1, 10, 1),
                    new Shoot(radius: 160, count: 4, fixedAngle: 60, rotateAngle: 25, shootAngle: 90, coolDown: 1000, projectileIndex: 0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 25, shootAngle: 90, coolDown: 1000, projectileIndex: 1),
                    new Shoot(radius: 120, count: 3, fixedAngle: null, shootAngle: 15, rotateAngle: null, coolDown: 500, projectileIndex: 3),
                    new Shoot(radius: 120, count: 1, fixedAngle: null, rotateAngle: null, coolDown: 350, projectileIndex: 3),


                    new Grenade(2, 10, 99, null, 2500, ConditionEffectIndex.Paralyzed, 1000, 0xFFFF00),
                    new Grenade(2, 10, 99, null, 2500, ConditionEffectIndex.Slowed, 3000, 0xFFFF00),
                    new HpLessTransition(0.20, "15")
                           ),
                    new State("15",
                         new ReturnToSpawn(2),
                   new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("THIS IS UNACCEPTABLE!"),
                    new TimedTransition(2500, "222")
                           ),
                    new State("222",
                    new Taunt("You underestimate the power of the crownguard."),
                    new Shoot(radius: 120, count: 6, fixedAngle: 0, shootAngle: 60, rotateAngle: 3, coolDown: 100, projectileIndex: 2),
                    new Shoot(radius: 120, count: 3, fixedAngle: 0, shootAngle: 120, rotateAngle: 65, coolDown: 1000, projectileIndex: 0),
                    new Shoot(radius: 120, count: 1, fixedAngle: null, angleOffset: 0, rotateAngle: null, coolDown: 500, projectileIndex: 3),

                    new Grenade(3, 10, 99, null, 2250, ConditionEffectIndex.Confused, 1250, 0xFFFF00),

                    new Shoot(radius: 180, count: 2, fixedAngle: 0, rotateAngle: 80, shootAngle: 180, coolDown: 2500, projectileIndex: 2),
                    new Shoot(radius: 180, count: 2, fixedAngle: 0, rotateAngle: 125, shootAngle: 180, coolDown: 2500, coolDownOffset: 100, projectileIndex: 3),

                    new Shoot(radius: 120, count: 1, fixedAngle: null, angleOffset: 0, rotateAngle: null, coolDown: 750, projectileIndex: 1),
                    new HpLessTransition(0.05, "16")
                           ),
                    new State("16",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Incredible"),
                    new SetAltTexture(1),
                    new TimedTransition(1500, "17")
                        ),
                    new State("17",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(2),
                    new TimedTransition(1500, "18")
                             ),
                    new State("18",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(3),
                    new TimedTransition(1500, "19")
                             ),
                    new State("19",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Maybe we have underestimated you..."),
                    new SetAltTexture(4),
                    new TimedTransition(1500, "20")
                        ),
                    new State("20",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Suicide()
                )

                    ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Life", 0.5),
                    new ItemLoot("Greater Potion of Mana", 0.5),
                    new ItemLoot("Greater Potion of Critical Damage", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 1),
                    new ItemLoot("Greater Potion of Critical Damage", 0.1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.1),
                    new ItemLoot("Omnipotent Token x1", 0.075),
                    new ItemLoot("The Book of Balance", 0.0006),
                    new ItemLoot("Infinite Knowledge", 0.001),
                    new TierLoot(tier: 7, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
                )
            )
               .Init("Royal Crownguard Dog",
                new State(
                new StayCloseToSpawn(1, 7),
                new ScaleHP2(20, 3, 15),
                new State("fight",
                new Taunt("You are not allowed to enter here.", "Remove yourself.", "You should stop here while you can.", "You do not have a permit to be here."),
                new Follow(0.2, 8, 1),
                new Wander(0.1),
                new ConditionalEffect(ConditionEffectIndex.Armored),
                new Shoot(60, count: 2, projectileIndex: 0, shootAngle: 20, coolDown: 750),
                new Shoot(60, count: 1, projectileIndex: 1, coolDown: 750),
                new HpLessTransition(0.50, "fight2")
               ),
                new State("fight2",
                new Follow(0.3, 8, 1),
                new Charge(1, 10, 5000),
                new ConditionalEffect(ConditionEffectIndex.Barrier),
                new Shoot(60, count: 2, projectileIndex: 0, shootAngle: 20, coolDown: 400),
                new Shoot(60, count: 1, projectileIndex: 1, coolDown: 400)
                )
                 ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Critical Damage", 0.10),
                    new ItemLoot("Greater Potion of Critical Chance", 0.10),
                    new ItemLoot("Critical Damage", 0.50),
                    new ItemLoot("Critical Chance", 0.50),
                    new TierLoot(tier: 6, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
            )
            )
             .Init("Blades of Enchantment",
                new State(
                new StayCloseToSpawn(1, 7),
                new ScaleHP2(20, 3, 15),
                new State("fight",
                new Spawn("C Fire Spawner YES", 1, 1, 99999, true),
                new Orbit(1, 5, 15, "C Fire Spawner YES"),
                new Taunt("You fear magic? That's how I was created!"),
                new Shoot(60, count: 4, projectileIndex: 0, shootAngle: null, rotateAngle: -25, coolDown: 200),
                new Shoot(60, count: 2, projectileIndex: 1, shootAngle: -20, rotateAngle: 139, angleOffset: 180, coolDown: 750),
                new HpLessTransition(0.50, "fight2")
               ),
                new State("fight2",
                new Orbit(1, 8, 15, "C Fire Spawner YES"),
                new Shoot(60, count: 4, projectileIndex: 0, shootAngle: null, rotateAngle: -25, coolDown: 200),
                new Shoot(60, count: 2, projectileIndex: 1, shootAngle: -20, rotateAngle: 139, angleOffset: 180, coolDown: 500)
                )
                 ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Critical Damage", 0.10),
                    new ItemLoot("Greater Potion of Critical Chance", 0.10),
                    new ItemLoot("Slasher", 0.0006),
                    new TierLoot(tier: 6, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
            )
            )

              .Init("Crownguard",
                new State(
                new State("fight",
                new Follow(0.2, 8, 1),
                new Wander(0.1),
                new Shoot(60, count: 3, projectileIndex: 0, shootAngle: 15, coolDown: 550),
                new Charge(1, 10, 5000)
                )
                 ),
                new Threshold(0.0001,
                    new ItemLoot("Critical Damage", 0.50),
                    new ItemLoot("Critical Chance", 0.50)
            )
            )
          .Init("Guardian of the Crownguard",
                new State(
                    new ScaleHP2(90, 2, 15),
                    new State("Start2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(9, "2")
                    ),
                    new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You really should turn back, you do not understand any of what is to come past this area."),
                    new TimedTransition(2500, "3")
                        ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("So stubborn..."),
                    new TimedTransition(2000, "Start")
                    ),
                    new State("Start",
                    new Shoot(radius: 120, count: 6, fixedAngle: 0, shootAngle: 60, rotateAngle: 32, coolDown: 1750, projectileIndex: 0),
                    new Shoot(radius: 120, count: 2, fixedAngle: null, shootAngle: 15, rotateAngle: null, coolDown: 300, projectileIndex: 1),
                    new Shoot(radius: 120, count: 1, fixedAngle: null, rotateAngle: null, coolDown: 750, projectileIndex: 1),
                    new Grenade(3, 100, 10, null, 2000, ConditionEffectIndex.Slowed, 2000),
                    new HpLessTransition(.60, "Start2")
                          ),
                    new State("Start2",
                    new Taunt("I said this once and i'm saying it again... I CANNOT let you pass."),
                    new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start3")
                        ),
                    new State("Start3",
                    new Shoot(radius: 120, count: 6, fixedAngle: 0, shootAngle: 60, rotateAngle: 32, coolDown: 1750, projectileIndex: 0),
                    new Shoot(radius: 120, count: 2, fixedAngle: null, shootAngle: 15, rotateAngle: null, coolDown: 330, projectileIndex: 1),
                    new Shoot(radius: 120, count: 1, fixedAngle: null, rotateAngle: null, coolDown: 550, projectileIndex: 1),
                    new Grenade(3, 100, 15, null, 3000, ConditionEffectIndex.Paralyzed, 1500),
                    new HpLessTransition(.30, "Start4")
                          ),
                    new State("Start4",
                    new Taunt("...."),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start5")
                          ),
                    new State("Start5",
                    new Charge(5, 13, 3000),
                    new Shoot(radius: 120, count: 4, fixedAngle: 0, rotateAngle: 45, coolDown: 1250, projectileIndex: 0),
                    new Shoot(radius: 120, count: 4, fixedAngle: 45, rotateAngle: 45, shootAngle: 90, coolDown: 1750, projectileIndex: 0),
                    new Shoot(radius: 120, count: 3, fixedAngle: null, shootAngle: 30, rotateAngle: null, coolDown: 200, projectileIndex: 1),
                    new Grenade(3, 100, 15, null, 3000, ConditionEffectIndex.Paralyzed, 1500)
                        )
                ),
                new Threshold(.0001,
                    new ItemLoot("Greater Potion of Life", 1),
                    new ItemLoot("Greater Potion of Mana", 1),
                    new ItemLoot("Greater Potion of Critical Damage", 0.5),
                    new ItemLoot("Greater Potion of Critical Chance", 0.5),
                    new ItemLoot("Greater Potion of Critical Damage", 0.1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.1),

                    new ItemLoot("Omnipotent Token x1", 0.075),
                    new ItemLoot("Knight's Vow Shield", 0.001),
                    new ItemLoot("Knight's Vow Armor", 0.001),
                    new TierLoot(tier: 7, type: ItemType.Ability, probability: 0.10),
                    new TierLoot(tier: 6, type: ItemType.Ring, probability: 0.10),
                    new TierLoot(tier: 13, type: ItemType.Weapon, probability: 0.10),
                    new TierLoot(tier: 12, type: ItemType.Armor, probability: 0.10)
                )
            )

         .Init("Defender of Zemith",
             new State(
                 new State("1",
                      new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                      new PlayerWithinTransition(6, "2")
                              ),
                    new State("2",
                    new Prioritize(
                        new Follow(0.35, 8, 1),
                        new Wander(0.6)
                        ),
                    new Shoot(8, count: 6, projectileIndex: 0, coolDown: 2000),
                    new Shoot(8, count: 2, shootAngle:15, projectileIndex: 1, coolDown: 100, coolDownOffset: 1250),
                    new TimedTransition(11000, "3")
                        ),
                    new State("3",
                    new Shoot(8, count: 1, projectileIndex: 0, coolDown: 100),
                    new Shoot(8, count: 8, shootAngle: 45, projectileIndex: 1, coolDown: 2000, coolDownOffset: 1250),
                    new TimedTransition(2000, "2")
                    )
                 )
              )



          .Init("Armored Lizard of the Crownguard",
                new State(
                    new State("shotgun",
                        new Follow(0.5, 8, 1),
                        new Shoot(10, 5, coolDown: 1750, projectileIndex: 0),
                        new Shoot(10, count: 6, shootAngle: 10, projectileIndex: 1, coolDown: 800),
                        new TimedTransition(5500, "turret")
                        ),
                    new State("turret",
                        new Flash(0xFFFF00, 2, 2),
                        new Wander(0.1),
                        new Shoot(10, count: 8, projectileIndex: 1, rotateAngle: 60, shootAngle: 20, coolDown: 500, fixedAngle: 270),
                        new Shoot(10, count: 8, projectileIndex: 1, rotateAngle: 60, shootAngle: 20, coolDown: 1000, fixedAngle: 90),
                        new TimedTransition(3000, "shotgun")
                        )

                    )
            )

         .Init("Grandolf, the Magic Orc",
             new State(
                    new State("1",
                    new Wander(0.2),
                    new Shoot(10, count: 8, shootAngle: 45, predictive: 2.5, projectileIndex: 0, coolDown: 1000),
                    new Shoot(8, count: 5, projectileIndex: 0, coolDown: new Cooldown(750, 500)),
                    new TimedTransition(6250, "2")
                        ),
                    new State("2",
                    new Prioritize(
                        new StayAbove(1, 200),
                        new Follow(1, range: 12),
                        new Wander(0.4)
                        ),
                     new Shoot(10, count: 3, shootAngle: 16, projectileIndex: 1, coolDown: 550),
                     new Shoot(10, count: 8, shootAngle: 45, predictive: 2.5, projectileIndex: 0, coolDown: 1000),
                     new Shoot(10, count: 4, shootAngle: 12, predictive: 2.5, projectileIndex: 0, coolDown: 1500),
                     new TimedTransition(6750, "3")
                    ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You're messing with magical entities, dont get tricked..."),
                    new Flash(0x00FF00, 4, 4),
                    new TimedTransition(3750, "4")
                    ),
                   new State("4",
                       new Prioritize(
                            new Follow(1, range: 7),
                            new Wander(0.5)
                            ),
                     new Shoot(10, count: 1, projectileIndex: 2, coolDown: 200),
                      new Shoot(10, count: 8, shootAngle: 45, predictive: 2.5, projectileIndex: 0, coolDown: 750),
                     new TimedTransition(5750, "1")

                    )
                 )
              )






        /*
          .Init("Any Wall That Breaks After A Miniboss Is Killed",
                new State(
                    new State("Idle",
                        new EntitiesNotExistsTransition(9999, "Rip", "Enemy THat needs to die before passage way is active")
                    ),
                    new State("Rip",
                        new Decay(500)
                    )
               )
            )
        */
        /* ^ for that
         *   <Object type="0x0121" id="Grey Torch Wall">
        <Class>Wall</Class>
        <Texture>
            <File>lofiEnvironment</File>
            <Index>0x02</Index>
        </Texture>
        <Top>
            <Texture>
                <File>lofiEnvironment</File>
                <Index>0x04</Index>
            </Texture>
        </Top>
        <Animation prob="0.4" period="0.4">
            <Frame time="0.05">
                <Texture>
                    <File>lofiEnvironment</File>
                    <Index>0x01</Index>
                </Texture>
            </Frame>
        </Animation>
        <HitSound>monster/stone_walls_hit</HitSound>
        <DeathSound>monster/stone_walls_death</DeathSound>
        <Static/>
        <FullOccupy/>
        <OccupySquare/>
        <EnemyOccupySquare/>
        <BlocksSight/>
    </Object>
         * 
         * 
         * 
         */


         .Init("Kayle of the Crownguard",
            new State(
                new State("Start",
                    new Wander(0.1),
                    new Shoot(15, 3, shootAngle: 10, coolDown: 750),
                    new TimedTransition(2500, "Start2")
                    ),
                new State("Start2",
                    new Charge(2, 12, coolDown: 6000),
                    new Shoot(15, 5, shootAngle: 15, coolDown: 6000),
                    new TimedTransition(1000, "Start")
                    )
                )
            )//Armored Gorilla Bear of Zemith
          .Init("Magi, Wonder Man of Vargo",
            new State(
                new State("1",
                new Prioritize(
                        new Follow(0.4, 8, 1),
                        new Wander(0.6)
                        ),
                     new Shoot(10, count: 5, shootAngle: 14, projectileIndex: 1, coolDown: new Cooldown(1200, 800)),
                     new TimedTransition(6400, "2")
                    ),
                new State("2",
                     new Swirl(0.4, 2, 8),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(10, count: 8, projectileIndex: 0, coolDown: 250),
                     new TimedTransition(1250, "3")
                    ),
                new State("3",
                new Prioritize(
                        new Follow(0.4, 8, 1),
                        new Wander(0.6)
                        ),
                     new Shoot(10, count: 5, shootAngle: 14, projectileIndex: 2, coolDown: new Cooldown(1200, 800)),
                     new TimedTransition(6400, "1")
                    )
                )
            )


            .Init("Student of Zemith",
            new State(
                new State("fight1",
                new Prioritize(
                        new Follow(0.8, 8, 1),
                        new Wander(1)
                        ),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 400),
                     new TimedTransition(4000, "fight2")
                    ),
                new State("fight2",
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 200),
                     new Shoot(10, count: 1, projectileIndex: 1, coolDown: 700),
                     new TimedTransition(2600, "fight1")
                    )
                )
            )

          .Init("Armored Gorilla Bear of Zemith",
            new State(
                new State("Start",
                    new Taunt("ROOAAAARRRR", "ROAAARRR", "ROOOAARRR"),
                    new Charge(2, 12, coolDown: 1000),
                    new Shoot(15, 8, shootAngle:45, rotateAngle:15, fixedAngle:0, projectileIndex:0, coolDown: 250),
                    new TimedTransition(4500, "Start2")
                    ),
                new State("Start2",
                    new Wander(0.4),
                    new Shoot(15, 1, projectileIndex: 1, coolDown: 200),
                    new Shoot(15, 8, shootAngle: 45, rotateAngle: 15, fixedAngle: 0, projectileIndex: 0, coolDown: 500),
                    new Grenade(7, 180, range: 2, coolDown: 350),
                    new TimedTransition(3050, "Start")
                    )
                )
            )

            .Init("Randolf Guardian of Zemith",
            new State(
                new State("Start",
                    new Wander(0.1),
                    new Grenade(5, 125, range: 4, coolDown: 1000),
                    new TimedTransition(3000, "Start2")
                    ),
                new State("Start2",
                    new Grenade(7, 180, range: 3, coolDown: 350),
                    new TimedTransition(1750, "Start")
                    )
                )

            )

           .Init("Vargo's Purple Hollographic Dog",
                new State(
                    new Shoot(10, 8, shootAngle: 45, coolDown: 1250, projectileIndex: 1),
                    new Shoot(10, 3, shootAngle: 10, coolDown: 500, projectileIndex: 0, predictive: 0.2),
                    new Wander(0.3),
                    new StayCloseToSpawn(0.4, 8)
                    )
            )

          .Init("Vargo's Red Hollographic Dog",
                new State(
                    new Shoot(10, 4, shootAngle: 10, coolDown: 750, projectileIndex: 1, predictive: 0.2),
                    new Shoot(10, 8, shootAngle: 45, coolDown: 1500, projectileIndex: 0),
                    new Wander(0.1),
                    new Follow(0.7, 8, 1),
                    new StayCloseToSpawn(0.4, 8)
                    )
            )





         .Init("Leona of the Crownguard",
                new State(
                    new Shoot(10, 5, shootAngle: 10, coolDown: 750, projectileIndex: 1),
                    new Shoot(10, 1, shootAngle: null, coolDown: 1500, predictive: 0.2, projectileIndex: 0),
                    new Wander(0.3),
                    new StayCloseToSpawn(0.4, 8)
                    )
            )
            .Init("Gorge of the Crownguard", 
                new State(
                    new Orbit(0.3, 3, 8, target: "Burning Dog"), //replace Burning Dog with key names for crownguard
                    new State("Shotgun",
                        new Shoot(radius: 120, count: 2, fixedAngle: null, shootAngle: 15, rotateAngle: null, coolDown: 500, projectileIndex: 1),
                        new Shoot(8, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 1000),
                        new TimedTransition(1250, "Shotgun2")
                        ),
                    new State("Shotgun2",
                        new Shoot(radius: 120, count: 1, fixedAngle: null, shootAngle: 20, rotateAngle: null, coolDown: 500, projectileIndex: 1),
                        new Shoot(8, count: 1, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 90, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 180, projectileIndex: 0, coolDown: 1000),
                        new Shoot(8, count: 1, fixedAngle: 270, projectileIndex: 0, coolDown: 1000),
                        new TimedTransition(1250, "Shotgun")
                        )
                    )
                )


           .Init("Burning Dog",
            new State(
                new State("Main",
                    new Wander(1),
                    new State("fight1",
                        new Shoot(10, count: 3, shootAngle: 8, projectileIndex: 0, coolDown: 50, predictive: 0.1),
                        new TimedTransition(4000, "fight2")
                        ),
                    new State("fight2",
                        new Shoot(10, count: 6, projectileIndex: 1, coolDown: 500),
                        new TimedTransition(3000, "fight1")
                        )
                    )
                )
            )


            .Init("Unbound Fighter",
            new State(
                new State("fight1",
                     new Prioritize(
                        new Follow(0.8, 8, 1),
                        new Wander(0.2)
                        ),
                     new Shoot(10, count: 5, shootAngle: 5, projectileIndex: 0, coolDown: 1200),
                     new HpLessTransition(0.60, "fight2")
                    ),
                new State(
                    new Prioritize(
                        new Follow(1, 8, 1),
                        new Wander(0.4)
                        ),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2500),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                new State("fight2",
                     new ChangeSize(25, 75),
                     new Shoot(10, count: 4, projectileIndex: 2, coolDown: 1200, predictive: 0.25),
                     new Shoot(10, count: 5, shootAngle: 5, projectileIndex: 1, coolDown: 600, predictive: 0.1),
                     new TimedTransition(6000, "fight3")
                    ),
                new State("fight3",
                     new Shoot(10, count: 4, projectileIndex: 2, coolDown: 600, predictive: 0.25),
                     new TimedTransition(4000, "fight2")
                        )
                    )
                )
            )

          .Init("Sand Sentry Turret",
                new State(
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(12, count: 1, shootAngle: null, predictive: 0.1, projectileIndex: 0, coolDown: 750),
                        new PlayerWithinTransition(5, "talk1")
                        ),
                    new State("talk1",
                        new Shoot(12, count: 2, shootAngle: 180, fixedAngle:0, rotateAngle: 10, projectileIndex: 0, coolDown: 50),
                        new TimedTransition(3000, "default")
                        )
                      )
                    )

          .Init("Secret Chest A",
                new State(
                    new ScaleHP2(75, 1, 15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                    ),
                new Threshold(0.15,
                  new ItemLoot("Beyonds' Lighter", 0.0002),
                  new ItemLoot("Greater Potion of Mana", 1),
                  new ItemLoot("Greater Potion of Luck", 1),
                  new ItemLoot("Greater Potion of Critical Damage", 0.5),
                  new ItemLoot("Greater Potion of Critical Chance", 0.5),
                  new ItemLoot("Greater Potion of Critical Damage", 0.1),
                  new ItemLoot("Greater Potion of Critical Chance", 0.1),
                  new ItemLoot("Omnipotent Token x5", 0.0005)
                )
            )

         .Init("Hero",
                new State(
                    new ScaleHP2(90, 2, 15),
                    new HpLessTransition(0.10, "spookded"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(10,"talk1")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("talk1",
                        new Taunt("Return while you still can, you are no match for any of us.", "The crownguard will not be trifled with."),
                        new TimedTransition(3000, "talk2")
                        ),
                    new State("talk2",
                        new Taunt("I will erase you where you stand."),
                        new TimedTransition(5000, "go")
                        )
                      ),
                    new State("go",
                        new Prioritize(
                            new Follow(0.8, 8, 1),
                            new Wander(1)
                            ),
                        new Shoot(8, count: 14, shootAngle: 20, projectileIndex: 4, coolDown: 600),
                        new Shoot(8, count: 3, shootAngle: 10, projectileIndex: 1, predictive: 2, coolDown: 1600),
                        new HpLessTransition(0.90,"go2")
                        ),
                    new State("go2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new StayBack(0.4, 2),
                            new Wander(0.5)
                            ),
                        new Shoot(12, count: 6, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 1),
                        new Shoot(8, count: 3, shootAngle: 25, projectileIndex: 1, coolDown: new Cooldown(2000, 1000)),
                        new TimedTransition(9000, "spook")
                        ),
                    new State("spook",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new TimedTransition(3000, "swag21")
                        ),
                    new State("swag21",
                        new Flash(0x0F0F0F, 2, 2),
                        new Grenade(4, 300, coolDown: 3000),
                        new Shoot(12, count: 2, shootAngle: 4, predictive: 0.1, projectileIndex: 0, coolDown: 1),
                        new Shoot(12, count: 2, shootAngle: 8, predictive: 0.1, projectileIndex: 1, coolDown: 100),
                        new Prioritize(
                            new Charge(2, 10, coolDown: 4000),
                            new Wander(0.2)
                            ),
                        new Shoot(8, count: 4, shootAngle: 90, projectileIndex: 2, coolDown: new Cooldown(2000, 1000)),
                        new TimedTransition(9000, "go2"),
                        new HpLessTransition(0.75, "basic2")
                        ),
                    new State("basic2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Follow(0.6),
                            new Wander(0.2)
                            ),
                        new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                        new HpLessTransition(0.65, "basic3")
                        ),
                    new State("basic3",
                        new Wander(0.4),
                        new Shoot(12, count: 4, shootAngle: 8, projectileIndex: 2, coolDown: 2200),
                        new HpLessTransition(0.40, "cryofsin")
                        ),
                    new State("cryofsin",
                        new Taunt(true, "Hehehehe, you mistake me for one of the weak ones", "Prepare yourselves!"),
                        new Flash(0xFF0000, 1, 2),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new TimedTransition(4400, "cry")
                        ),
                    new State("cry",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3500),
                        new Shoot(30, count: 4, shootAngle: 90, projectileIndex: 1, rotateAngle: 25, fixedAngle: 0, coolDown: 1000),
                        new Shoot(30, count: 4, shootAngle: 90, projectileIndex: 1, rotateAngle: 25, fixedAngle: 90, coolDown: 1000),
                        new Shoot(30, count: 4, shootAngle: 90, projectileIndex: 1, rotateAngle: 25, fixedAngle: 180, coolDown: 1000),
                        new Shoot(30, count: 4, shootAngle: 90, projectileIndex: 1, rotateAngle: 25, fixedAngle: 270, coolDown: 1000),
                         new HpLessTransition(0.20, "gofirst")
                        ),
                    new State("gofirst",
                        new HealSelf(coolDown: 1000, amount: 7500),
                        new ConditionalEffect(ConditionEffectIndex.Barrier),
                        new Prioritize(
                            new Follow(1.2),
                            new Wander(0.2)
                            ),
                        new Shoot(12, count: 4, shootAngle: 8, projectileIndex: 2, coolDown: 2200),
                        new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                        new TimedTransition(5000, "cry2222")
                                  ),
                    new State("cry2222",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Grenade(4, 300, coolDown: 3000),
                        new Shoot(12, count: 2, shootAngle: 4, predictive: 0.1, projectileIndex: 0, coolDown: 1),
                        new Shoot(12, count: 2, shootAngle: 8, predictive: 0.1, projectileIndex: 1, coolDown: 100),
                         new TimedTransition(6000, "gofirst")
                        ),
                    new State("spookded",
                        new Flash(0x00FF00, 1, 3),
                        new Taunt("Well done warrior!"),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new Suicide()
                        )
                    ),
                    new Threshold(0.1,
                  new ItemLoot("Greater Potion of Life", 1),
                  new ItemLoot("Greater Potion of Mana", 1),
                  new ItemLoot("Greater Potion of Luck", 1),
                  new ItemLoot("Greater Potion of Critical Damage", 0.5),
                  new ItemLoot("Greater Potion of Critical Chance", 0.5),
                  new ItemLoot("Greater Potion of Critical Damage", 0.1),
                  new ItemLoot("Greater Potion of Critical Chance", 0.1),
                  new ItemLoot("Critical Damage", 0.50),
                  new ItemLoot("Critical Chance", 0.50),//Star in a Bottle
                  new ItemLoot("Star in a Bottle", 0.004),
                  new ItemLoot("Omnipotent Token x1", 0.002)
                       )
            )
            .Init("Vargo Wall Killer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityNotExistsTransition("Vargo the Supreme Mage", 999, "die")
                        ),
                    new State("die",
                        new RemoveTileObject(0x4a3a, 15),
                        new Suicide()
                        )
                    )
            )
        .Init("Miniboss Wall Killer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesNotExistsTransition(999,"die", "Vargo the Supreme Mage")
                        ),
                    new State("die",
                        new RemoveTileObject(0x4a3c, 15),
                        new Suicide()
                        )
                    )
            )
































































        ;
            
    }
}