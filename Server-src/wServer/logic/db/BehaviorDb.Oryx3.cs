#region
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Oryx = () => Behav()
            .Init("Cyberious, The Commander of the Realm",
                new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                   new ScaleHP2(70, 4 ,15),
                     new State("Beginning221",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                         new PlayerWithinTransition(8, "Beginning")
                         ),
                      new State("Beginning",
                     new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Cyberious.mp3"), 
                     new Flash(0xf389E13, 1, 2),
                     new Taunt("You realmers... Isn't that what you call yourself?"),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2500, "Chat1")//change to preattack2
                          ),
                           new State("Chat1",
                     new Flash(0xf389E13, 1, 2),
                     new Taunt("I am here to act as the gate between Earth and any other world."),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2500, "Chat2")
                          ),
                             new State("Chat2",
                     new Flash(0xf389E13, 1, 2),
                     new Taunt("And since you have defeated by brothers, I suppose I am the last survivor."),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2500, "Chat3")
                                     ),
                             new State("Chat3",
                     new Flash(0xf389E13, 1, 2),
                     new Taunt("You will pay for what you have done."),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2500, "Attack")
                          ),
                    new State("Attack",
                        new Wander(.05),
                        new Grenade(8, 100, coolDown: 4000),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, coolDown: 1000,
                            coolDownOffset: 1000),
                        new HpLessTransition(.75, "Attack2")
                    ),
                    new State("Attack2",
                        new ReturnToSpawn(2),
                        new Flash(0xf389E13, 1, 2),
                        new Taunt("Heh, Funny.... You really think that this fight will be easy?"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, fixedAngle: 0, projectileIndex: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, fixedAngle: 30, projectileIndex: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(2500, "Attack3")
                    ),
                      new State("Attack3",
                        new Flash(0xf389E13, 1, 2),
                        new Taunt("I wont allow you to expand your knowledge past this planet!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "Attack4")
                    ),
                    new State("Attack4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, false, 6500),
                        new ConditionalEffect(ConditionEffectIndex.Solid),
                        new Shoot(25, projectileIndex: 12, count:3, fixedAngle: 220, rotateAngle: 3, coolDown: 200),
                        new Shoot(25, projectileIndex: 12, count:3, fixedAngle: 230, rotateAngle: 3, coolDown: 200),//circle
                        new Shoot(25, projectileIndex: 12, count:3, fixedAngle: 240, rotateAngle: 3, coolDown: 200),

                        new Shoot(10, 6, projectileIndex: 11, predictive: 1, coolDown: 2000),
                        new Shoot(10, 6, projectileIndex: 11, predictive: 1, coolDownOffset: 250, coolDown: 2000),
                        new Shoot(10, 6, projectileIndex: 11, predictive: 1, coolDownOffset: 420, coolDown: 2000),
                        new Shoot(10, 6, projectileIndex: 11, predictive: 1, coolDownOffset: 650, coolDown: 2000),
                        new HpLessTransition(.60, "Attack45")
                         ),
                    new State("Attack45",
                        new Flash(0xf389E13, 1, 2),
                        new Taunt("I am immortal, I will be reborn. You cannot win!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(0.5),
                        new TimedTransition(2500, "Attack5")
                          ),
                        new State("Attack5",
                        new Prioritize(
                            new Follow(0.65, 8, 1),
                            new Wander(0.5)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(10, count: 8, projectileIndex: 13, coolDown: 3000),
                        new Shoot(10, count: 6, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(10, count: 4, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1500),
                        new Shoot(10, count: 1, shootAngle: 6, projectileIndex: 1, coolDown: 2000),
                        new TimedTransition(6500, "Attack6"),
                        new HpLessTransition(.40, "Attack7")
                        ),
                    new State("Attack6",
                        new Wander(0.75),
                        new Shoot(10, count: 6, projectileIndex: 9, coolDown: 1000),
                        new Shoot(10, count: 6, projectileIndex: 8, coolDown: 1000),
                        new Grenade(5, 150, range: 5, coolDown: 2000),
                        new TimedTransition(6500, "Attack5"),
                        new HpLessTransition(.40, "Attack7")
                        ),
                      new State("Attack7",
                        new Flash(0xf389E13, 1, 2),
                        new ReturnToSpawn(3),
                        new Taunt("This must not go on for any longer!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "Attack8")
                    ),
                    new State("Attack8",
                         
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 5000),
                    new Taunt("Vanish."),
                    new ConditionalEffect(ConditionEffectIndex.Solid),
                    new Shoot(10, 3, projectileIndex: 13, predictive: 1, coolDownOffset: 200, coolDown: 2000),
                    new Shoot(10, 3, projectileIndex: 13, predictive: 1, coolDownOffset: 300, coolDown: 2000),
                    new Shoot(10, 3, projectileIndex: 13, predictive: 1, coolDownOffset: 100, coolDown: 2000),
                    new Shoot(10, 3, projectileIndex: 13, predictive: 1, coolDownOffset: 400, coolDown: 2000),

                    new Grenade(2.5, 0, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Diminished, effectDuration: 1500, color: 0xff0000, fixedAngle: 45),
                    new Grenade(2.5, 0, range: 4, coolDown: 1000, effect: ConditionEffectIndex.Sluggish, effectDuration: 1500, color: 0x000000, fixedAngle: 45),
                    new Grenade(2.5, 0, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Diminished, effectDuration: 1500, color: 0xff0000, fixedAngle: 135),
                    new Grenade(2.5, 0, range: 4, coolDown: 1000, effect: ConditionEffectIndex.Sluggish, effectDuration: 1500, color: 0x000000, fixedAngle: 135),
                    new Grenade(2.5, 0, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Diminished, effectDuration: 1500, color: 0xff0000, fixedAngle: 225),
                    new Grenade(2.5, 0, range: 4, coolDown: 1000, effect: ConditionEffectIndex.Sluggish, effectDuration: 1500, color: 0x000000, fixedAngle: 225),
                    new Grenade(2.5, 0, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Diminished, effectDuration: 1500, color: 0xff0000, fixedAngle: 315),
                    new Grenade(2.5, 0, range: 4, coolDown: 1000, effect: ConditionEffectIndex.Sluggish, effectDuration: 1500, color: 0x000000, fixedAngle: 315),

                    new Shoot(15, count: 6, rotateAngle: 5, fixedAngle:5, projectileIndex: 11, coolDown: 100),
                    new Shoot(15, count: 1, rotateAngle: -5, fixedAngle:10, projectileIndex: 9, coolDown: 400),

                    /*
                    new Shoot(15, count: 6, fixedAngle: 0, projectileIndex: 10, coolDown: 3000),
                    new Shoot(15, count: 6, fixedAngle: 45, projectileIndex: 10, coolDown: 3000, coolDownOffset: 200),
                    new Shoot(15, count: 6, fixedAngle: 90, projectileIndex: 10, coolDown: 3000, coolDownOffset: 400),
                    new Shoot(15, count: 6, fixedAngle: 135, projectileIndex: 10, coolDown: 3000, coolDownOffset: 600),
                    new Shoot(15, count: 6, fixedAngle: 180, projectileIndex: 10, coolDown: 3000, coolDownOffset: 800),
                    new Shoot(15, count: 6, fixedAngle: 225, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1000),
                    new Shoot(15, count: 6, fixedAngle: 270, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1200),
                    new Shoot(15, count: 6, fixedAngle: 315, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1400),
                    new Shoot(15, count: 6, fixedAngle: 360, projectileIndex: 10, coolDown: 3000, coolDownOffset: 1600),
                    */
                    new HpLessTransition(.20, "Failure")
                             ),
                       new State("Failure",
                        new Flash(0xf389E13, 1, 2),
                        new ReturnToSpawn(2),
                        new Taunt("I AM A DECENDENT OF THE ORYX FAMILY! I CANNOT LOSE!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "Real Failure")
                    ),
                    new State("Real Failure",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("ENOUGH!", "THIS WILL BE THE END OF YOU!", "FINAL FLASH!!!!!!!", "YOU WILL BE ERASED!", "YOU! I WONT TAKE ANY MORE OF THIS!"),
                            new Shoot(15, 5, 20, 0, predictive: 1, coolDown: 1000),
                            new Grenade(6, 250, range: 16, coolDown: 2500, effect: ConditionEffectIndex.Frozen, effectDuration: 2000, color: 0xff0000),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 0, coolDown: 10000, coolDownOffset: 200),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 20, coolDown: 10000, coolDownOffset: 400),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 40, coolDown: 10000, coolDownOffset: 600),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 60, coolDown: 10000, coolDownOffset: 800),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 80, coolDown: 10000, coolDownOffset: 1000),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 100, coolDown: 10000, coolDownOffset: 1200),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 120, coolDown: 10000, coolDownOffset: 1400),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 140, coolDown: 10000, coolDownOffset: 1600),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 160, coolDown: 10000, coolDownOffset: 1800),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 180, coolDown: 10000, coolDownOffset: 2000),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 180, coolDown: 10000, coolDownOffset: 2200),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 160, coolDown: 10000, coolDownOffset: 2400),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 140, coolDown: 10000, coolDownOffset: 2600),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 120, coolDown: 10000, coolDownOffset: 2800),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 100, coolDown: 10000, coolDownOffset: 3000),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 80, coolDown: 10000, coolDownOffset: 3200),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 60, coolDown: 10000, coolDownOffset: 3400),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 40, coolDown: 10000, coolDownOffset: 3600),
                            new Shoot(21, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 20, coolDown: 10000, coolDownOffset: 3800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 0, coolDown: 10000, coolDownOffset: 4000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 0, coolDown: 10000, coolDownOffset: 4200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 20, coolDown: 10000, coolDownOffset: 4400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 40, coolDown: 10000, coolDownOffset: 4600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 60, coolDown: 10000, coolDownOffset: 4800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 80, coolDown: 10000, coolDownOffset: 5000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 100, coolDown: 10000, coolDownOffset: 5200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 120, coolDown: 10000, coolDownOffset: 5400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 140, coolDown: 10000, coolDownOffset: 5600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 160, coolDown: 10000, coolDownOffset: 5800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 180, coolDown: 10000, coolDownOffset: 6000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 180, coolDown: 10000, coolDownOffset: 6200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 160, coolDown: 10000, coolDownOffset: 6400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 140, coolDown: 10000, coolDownOffset: 6600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 120, coolDown: 10000, coolDownOffset: 6800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 100, coolDown: 10000, coolDownOffset: 7000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 80, coolDown: 10000, coolDownOffset: 7200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 60, coolDown: 10000, coolDownOffset: 7400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 40, coolDown: 10000, coolDownOffset: 7600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 20, coolDown: 10000, coolDownOffset: 7800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 0, coolDown: 10000, coolDownOffset: 8000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 0, coolDown: 10000, coolDownOffset: 8200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 20, coolDown: 10000, coolDownOffset: 8400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 40, coolDown: 10000, coolDownOffset: 8600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 60, coolDown: 10000, coolDownOffset: 8800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 80, coolDown: 10000, coolDownOffset: 9000),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 100, coolDown: 10000, coolDownOffset: 9200),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 120, coolDown: 10000, coolDownOffset: 9400),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 140, coolDown: 10000, coolDownOffset: 9600),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 160, coolDown: 10000, coolDownOffset: 9800),
                            new Shoot(15, projectileIndex: 7, count: 4, fixedAngle: 90, angleOffset: 180, coolDown: 10000, coolDownOffset: 10000),
                            new TimedTransition(25000, "Failure2")
                                 ),
                       new State("Failure2",
                        new Flash(0xffffff, 999, 2),
                        new Taunt("You... won... This planet has gotten too strong!, oryx shall know of this soon.")
                                     

                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Mark of Cyberious", 1),
                    new ItemLoot("Greater Potion of Attack", 0.51),
                    new ItemLoot("Greater Potion of Defense", 0.51),
                    new ItemLoot("Greater Potion of Wisdom", 0.51),
                    new ItemLoot("Greater Potion of Dexterity", 0.51),
                    new ItemLoot("Greater Potion of Vitality", 1),

                    new ItemLoot("Greater Potion of Critical Chance", 0.02),
                    new ItemLoot("Greater Potion of Critical Damage", 0.02),

                    new ItemLoot("Theurgy Wand", 0.01),
                    new ItemLoot("Ceremonial Merlot", 0.01),
                    new ItemLoot("Anointed Robe", 0.01),
                    new ItemLoot("Ring of Pagan Favor", 0.01),
                    new ItemLoot("Special Crate", 0.1),
                    new ItemLoot("Miscellaneous Crate", 0.1),

                    new ItemLoot("Weaponized Shield of Command", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Shield of King Cyberious", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("King Cyberious's Powerful Armor", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Horns of Cyberious", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Oryx's Arena Key", 0.002, damagebased: true),

                    new ItemLoot("Light Armor Schematic", 0.005, damagebased: true),
                    new ItemLoot("Robe Schematic", 0.005, damagebased: true),
                    new ItemLoot("Heavy Armor Schematic", 0.005, damagebased: true),

                    new ItemLoot("Lunar Ascension", 0.03, damagebased: true),

                    new ItemLoot("Grand Master Chest Item", 0.10, threshold: 0.01), //Oryx's Arena Key
                    new ItemLoot("50 Credits", 1),
                    new ItemLoot("50 Credits", 0.50),

                    new ItemLoot("KnightST5", 0.006, damagebased: true),//The Infinity Javelin

                    new ItemLoot("The Infinity Javelin", 0.0006, damagebased: true, threshold: 0.02),

                    new ItemLoot("Cloak of Wild Shadows", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Commander's Horn of War", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Orb of Horrific Dark Magic", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("Scraps of the Descendant", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Scraps of the Descendant", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Cyberious Infused Shard", 0.0045, damagebased: true, threshold: 0.01),
                   
                    new TierLoot(7, ItemType.Ring, 0.010),
                    new TierLoot(6, ItemType.Ring, 0.020),
                    new TierLoot(10, ItemType.Weapon, 0.30),
                    new TierLoot(11, ItemType.Weapon, 0.20),
                    new TierLoot(12, ItemType.Weapon, 0.10),
                    new TierLoot(13, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Ability, 0.08),
                    new TierLoot(7, ItemType.Ability, 0.04),
                    new TierLoot(8, ItemType.Ability, 0.02),
                    new TierLoot(11, ItemType.Armor, 0.30),
                    new TierLoot(12, ItemType.Armor, 0.20),
                    new TierLoot(13, ItemType.Armor, 0.10),
                    new TierLoot(5, ItemType.Ring, 0.30)
                     )
            )

                     .Init("Fate, The Lowend Guardian",
            new State(
                new ScaleHP2(60, 2, 15),
                new StayCloseToSpawn(.2,6),
                new State("1",
                    new Taunt("Remove yourself from this castle at once!"),
                    new ScaleHP2(25,2,25),
                    new Wander(0.6),
                    new Shoot(10, projectileIndex: 0, count: 8, shootAngle: 10, coolDown: 1500),
                    new HpLessTransition(0.5, "2")
                    ),
                new State("2",
                    new Taunt("I SAID BEGONE!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new HealSelf(coolDown: 1000, amount: 1000),
                    new Flash(0xFF0000, 1, 1),
                    new TimedTransition(2000, "3")
                    ),
                new State("3",
                    new Wander(0.6),
                    new Shoot(10, projectileIndex: 1, count: 3, shootAngle: 5, coolDown: 500)
            )
                     
                ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Attack", 0.2),
                    new ItemLoot("Greater Potion of Defense", 0.2),
                    new ItemLoot("Greater Potion of Wisdom", 0.2),
                    new ItemLoot("Greater Potion of Dexterity", 0.2),
                    new ItemLoot("Greater Potion of Vitality", 1),
                    new ItemLoot("Quest Chest", 1),
                    new ItemLoot("50 Credits", 0.5),
                    new ItemLoot("50 Credits", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.02),
                    new ItemLoot("Greater Potion of Critical Damage", 0.02),
                    new ItemLoot("Earth Shard", 0.01),
                    new ItemLoot("Claws Of No Remorse", 0.001, damagebased: true),
                    new ItemLoot("Cloth of Assassination", 0.001, damagebased: true),
                     new ItemLoot("Special Crate", 0.1),

                    new TierLoot(5, ItemType.Ring, 0.2),
                    new TierLoot(6, ItemType.Ring, 0.1),

                    new TierLoot(11, ItemType.Weapon, 0.3),
                    new TierLoot(12, ItemType.Weapon, 0.2),
                    new TierLoot(13, ItemType.Weapon, 0.1),

                    new TierLoot(5, ItemType.Ability, 0.2),
                    new TierLoot(6, ItemType.Ability, 0.1),
                    new TierLoot(7, ItemType.Ability, 0.04),
                    new TierLoot(8, ItemType.Ability, 0.02),

                    new TierLoot(11, ItemType.Armor, 0.3),
                    new TierLoot(12, ItemType.Armor, 0.2),
                    new TierLoot(13, ItemType.Armor, 0.15)

                    
                     )
            
            
           )
         .Init("Abderus, The Mage",
            new State(
                new ScaleHP2(60, 2, 15),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(7, "2")
                    ),
                new State("2",
                     new Taunt("FATE?! Has he failed?"),
                    new Shoot(10, count: 2, shootAngle: 11, coolDown: 100, projectileIndex: 0),
                    new Shoot(radius: 12, count: 6, shootAngle: 20, projectileIndex: 1, predictive: 0.15, coolDown: 500),
                    new Shoot(radius: 8, count: 1, angleOffset: 0.6, predictive: 0.15, coolDown: 900),
                    new Wander(speed: 0.4),
                    new HpLessTransition(0.5, "rageprep")
                    ),
                new State("rageprep",
                    new Taunt("You must not reach lord Cyberious!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 5, 10),
                    new TimedTransition(5000, "rage")
                    ),
                new State("rage",
                    new TossObject("Whachi, The Mage Protectors", range: 1, angle: 0, coolDown: 99999),
                    new TossObject("Whachi, The Mage Protectors", range: 1, angle: 45, coolDown: 99999),
                    new TossObject("Whachi, The Mage Protectors", range: 1, angle: 135, coolDown: 99999),
                    new TossObject("Whachi, The Mage Protectors", range: 1, angle: 225, coolDown: 99999),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(radius: 8, count: 1, angleOffset: 0.6, projectileIndex: 1, predictive: 0.15, coolDown: 900),
                    new Shoot(10, count: 5, shootAngle: 15, coolDown: 2000, projectileIndex: 0),
                     new TimedTransition(5000, "checkforminions")
                    ),
                    new State("checkforminions",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(radius: 8, count: 1, angleOffset: 0.6, projectileIndex: 1, predictive: 0.15, coolDown: 900),
                    new EntitiesNotExistsTransition(9999, "rageprep2", "Whachi, The Mage Protectors")
                            ),
                new State("rageprep2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 5, 10),
                    new TimedTransition(5000, "rage2")
                       ),
                new State("rage2",
                    new Shoot(10, count: 2, shootAngle: 11, coolDown: 150, projectileIndex: 0, predictive: 0.35),
                    new Shoot(radius: 8, count: 1, angleOffset: 0.6, predictive: 0.15, coolDown: 900),
                    new Shoot(10, count: 5, shootAngle: 15, coolDown: 2000, projectileIndex: 0),
                     new TimedTransition(5000, "checkforminions")
                    )
                 ),

                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Attack", 0.2),
                    new ItemLoot("Greater Potion of Defense", 0.2),
                    new ItemLoot("Greater Potion of Wisdom", 1),
                    new ItemLoot("Greater Potion of Dexterity", 0.2),
                    new ItemLoot("Greater Potion of Vitality", 0.2),
                    new ItemLoot("Quest Chest", 1),
                    new ItemLoot("50 Credits", 0.5),
                    new ItemLoot("Orb of Oryx's Mysteries", 0.02),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Earth Shard", 0.01),
                    new ItemLoot("Defender's Majestic Robe", 0.006, damagebased: true),
                    new ItemLoot("Staff of the Everbind Magic", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Robe of the Twin Brother", 0.001, damagebased: true, threshold: 0.01),//Defender's Majestic Robe
                     new ItemLoot("Special Crate", 0.1),
                    new TierLoot(5, ItemType.Ring, 0.2),
                    new TierLoot(6, ItemType.Ring, 0.1),

                    new TierLoot(11, ItemType.Weapon, 0.3),
                    new TierLoot(12, ItemType.Weapon, 0.2),
                    new TierLoot(13, ItemType.Weapon, 0.1),

                    new TierLoot(5, ItemType.Ability, 0.2),
                    new TierLoot(6, ItemType.Ability, 0.1),
                    new TierLoot(7, ItemType.Ability, 0.04),
                    new TierLoot(8, ItemType.Ability, 0.02),

                    new TierLoot(11, ItemType.Armor, 0.3),
                    new TierLoot(12, ItemType.Armor, 0.2),
                    new TierLoot(13, ItemType.Armor, 0.1)
                )
            )
           .Init("Whachi, The Mage Protectors",
            new State(
              new ScaleHP2(10, 2, 15),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                    new Orbit(0.2, 4, 20, "Abderus, The Mage", speedVariance: 0.2),
                    new Shoot(10, count: 2, projectileIndex: 0, shootAngle: 30 , coolDown: 2250)
                    )
                   

                )
            )

              .Init("Abracax, The Forgotten",
            new State(
                new ScaleHP2(33, 2, 15),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(7, "2")
                    ),
                new State("2",
                    new Taunt("What are mortals doing in lord Cyberious's Castle! Remove them Zemi!!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("Zemi, Abracax's Protectors", range: 1, angle: 45, coolDown: 999999),
                    new TossObject("Zemi, Abracax's Protectors", range: 1, angle: 135, coolDown: 999999),
                    new TimedTransition(2100, "checkforminions")
                    ),
                new State("checkforminions",
                    new Flash(0xff0000, 5, 10),
                    new Wander(0.15),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(9999, "rageprep", "Zemi, Abracax's Protectors")
                    ),
                new State("rageprep",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xff0000, 5, 10),
                    new Taunt("Foolish..."),
                    new TimedTransition(5000, "rage")
                    ),
                new State("rage",
                    new Charge(speed: 1, range: 12, coolDown: 2100),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 2000, projectileIndex: 0, coolDownOffset: 5),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 2000, projectileIndex: 0, coolDownOffset: 220),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 2000, projectileIndex: 0, coolDownOffset: 420),
                    new Shoot(10, count: 20, shootAngle: 18, coolDown: 2000, projectileIndex: 0, coolDownOffset: 620),
                    new TimedTransition(8000,"rage2"),
                    new HpLessTransition(0.65, "halfhprage")
                      ),
                new State("rage2",
                    new StayBack(0.5, 7),
                    new Wander(0.3),
                    new Shoot(10, count: 20, shootAngle: 5, coolDown: 1600, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 15, shootAngle: 8, coolDown: 1600, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 10, shootAngle: 12, coolDown: 1600, projectileIndex: 1, coolDownOffset: 420),
                    new TimedTransition(8000, "rage"),
                    new HpLessTransition(0.65, "halfhprage")
                    ),
                new State("halfhprage",
                    new Taunt("You must not reach lord Cyberious!"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new ChangeSize(5, 150),
                    new Flash(0xff0000, 5, 10),
                    new TimedTransition(5000, "ragehp")
                    ),
                   new State("ragehp",
                    new Charge(speed: 1, range: 15, 500),
                    new Shoot(10, count: 10, shootAngle: 18, coolDown: 2300, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 10, shootAngle: 18, coolDown: 2300, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 10, shootAngle: 18, coolDown: 2300, projectileIndex: 1, coolDownOffset: 420),
                    new Shoot(10, count: 10, shootAngle: 18, coolDown: 2300, projectileIndex: 1, coolDownOffset: 620),
                    new TimedTransition(8000, "rage22")
                          ),
                new State("rage22",
                    new StayBack(0.5, 7),
                    new Wander(0.5),
                    new Shoot(10, count: 20, shootAngle: 5, coolDown: 1600, projectileIndex: 1, coolDownOffset: 5),
                    new Shoot(10, count: 15, shootAngle: 8, coolDown: 1600, projectileIndex: 1, coolDownOffset: 220),
                    new Shoot(10, count: 10, shootAngle: 12, coolDown: 1600, projectileIndex: 1, coolDownOffset: 420),
                    new TimedTransition(8000, "ragehp")
                       )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Greater Potion of Attack", 0.2),
                    new ItemLoot("Greater Potion of Defense", 0.2),
                    new ItemLoot("Greater Potion of Wisdom", 0.2),
                    new ItemLoot("Greater Potion of Dexterity", 1),
                    new ItemLoot("Greater Potion of Vitality", 0.2),

                    new ItemLoot("Quest Chest", 1),
                    new ItemLoot("50 Credits", 0.5),
                    new ItemLoot("Earth Shard", 0.01),
                    new ItemLoot("Heavy Guardian's Plate", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Special Crate", 0.1, damagebased: true),

                    new ItemLoot("Greater Potion of Critical Chance", 0.02),
                    new ItemLoot("Greater Potion of Critical Damage", 0.02),

                    new TierLoot(5, ItemType.Ring, 0.2),
                    new TierLoot(6, ItemType.Ring, 0.1),

                    new TierLoot(11, ItemType.Weapon, 0.3),
                    new TierLoot(12, ItemType.Weapon, 0.2),
                    new TierLoot(13, ItemType.Weapon, 0.1),

                    new TierLoot(5, ItemType.Ability, 0.2),
                    new TierLoot(6, ItemType.Ability, 0.1),
                    new TierLoot(7, ItemType.Ability, 0.04),
                    new TierLoot(8, ItemType.Ability, 0.02),

                    new TierLoot(11, ItemType.Armor, 0.3),
                    new TierLoot(12, ItemType.Armor, 0.2),
                    new TierLoot(13, ItemType.Armor, 0.1)

                )
            )
        .Init("Zemi, Abracax's Protectors",
            new State(
                    new ScaleHP2(10, 2, 15),
                new State("1",
                    new Charge(speed: 0.6),
                    new Wander(1),
                    new StayBack(0.1,1),
                    new Shoot(10, count: 3, projectileIndex: 0, coolDown: 1500)
                    )
                )
            )

          .Init("Cyberious TP 1",
                new State(
                    new TransformOnDeath("Cyberious TP 1", 1, 1, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("null",
                        new EntityNotExistsTransition("Fate, The Lowend Guardian",99,"1")
                        ),
                    new State("1",
                        new Taunt("[Teleporter Active]"),
                        new TeleportPlayer(4, 67, 34, true)
                )
            )
            )
          .Init("Cyberious TP 2",
                new State(
                    new TransformOnDeath("Cyberious TP 2", 1, 1, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("null",
                        new EntityNotExistsTransition("Abderus, The Mage", 99, "1")
                        ),
                    new State("1",
                        new Taunt("[Teleporter Active]"),
                        new TeleportPlayer(4, 66, 91, true)
                )
            )
            )
          .Init("Cyberious TP 3",
                new State(
                    new TransformOnDeath("Cyberious TP 3", 1 , 1 , 100),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new State("null",
                        new EntityNotExistsTransition("Abracax, The Forgotten", 99, "1")
                        ),
                    new State("1",
                        new Taunt("[Teleporter Active]"),
                        new TeleportPlayer(4, 137, 67, true)
                )
            )
            )
            


            ;
    }
}