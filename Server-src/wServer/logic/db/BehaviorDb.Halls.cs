using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {//Made by: Tidan#6992.
        private _ Halls = () => Behav()
        
        #region LH Marble Colossus
     
        .Init("LH Marble Colossus",
                new State(
                    new ScaleHP2(85, 2, 15),
                     new DropPortalOnDeath("LH Void Spawner", 100, 360),
                    new ConditionalEffect(ConditionEffectIndex.StunImmune),
                    new HpLessTransition(0.12, "deathbegins"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(7, "start")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("start",
                        new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Colossus.mp3"),
                        new Taunt(1.00, "I'm here to keep this gate locked!"),
                        new TimedTransition(3000, "talk1")
                        ),
                    new State("talk1",
                        new Taunt(1.00, "The Void doesn't want any mortals near this place!"),
                        new TimedTransition(3000, "talk2")
                        ),
                    new State("talk2",
                        new Taunt(1.00, "I will accept my masters plans."),
                        new TimedTransition(3000, "talk3")
                        ),
                    new State("talk3",
                        new Taunt(1.00, "Death is the only answer!"),
                        new TimedTransition(3000, "GetReady")
                        )
                        ),
                    new State("GetReady",
                        new Flash(0xFFF240, 1, 1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "fight2")
                        ),
                    new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 4, shootAngle: 4, projectileIndex: 0, coolDown: 1),
                        new Shoot(10, count: 8, projectileIndex: 1, coolDown: 1750),
                        new TimedTransition(5000, "fight3")
                        ),
                    new State("fight3",
                            new Prioritize(
                                 new Follow(0.4, 8, 1),
                                 new Wander(0.5)
                                ),
                    new Taunt(0.50, "My god, minions of Oryx? Your death is near!"),
                        new Shoot(10, count: 8, shootAngle: 6, projectileIndex: 5, coolDown: 3400),
                        new Shoot(10, count: 5, shootAngle: 28, projectileIndex: 2, coolDown: 2600),
                        new TimedTransition(4750, "fight4")
                        ),
                    new State("fight4",
                            new Prioritize(
                                 new StayCloseToSpawn(0.7, 2),
                                 new Wander(0.5)
                                ),
                        new Shoot(10, count: 8, shootAngle: 15, projectileIndex: 4, coolDown: 1750, fixedAngle: 135),
                        new Shoot(10, count: 8, shootAngle: 15, projectileIndex: 4, coolDown: 1750, fixedAngle: 45),
                        new Shoot(10, count: 8, shootAngle: 15, projectileIndex: 4, coolDown: 1750, fixedAngle: 225),
                        new Shoot(10, count: 8, shootAngle: 15, projectileIndex: 4, coolDown: 1750, fixedAngle: 315),
                        new Shoot(10, count: 3, shootAngle: 30, projectileIndex: 3, coolDown: 3400),
                        new Grenade(3, 190, 15, coolDown: 1000),
                        new TimedTransition(4750, "fight5")
                        ),
                     new State("fight5",
                          new Prioritize(
                            new Follow(0.20, 8, 1),
                            new Wander(0.6)
                            ),
                        new Taunt(1.00, "Mortals think they can do anything!"),
                        new TimedTransition(4500, "heal"),
                        new State("1",
                            new Shoot(10, count: 3, projectileIndex: 3, coolDown: 3400),
                            new TimedTransition(300, "2")
                        ),
                        new State("2",
                            new Shoot(10, count: 6, projectileIndex: 3, coolDown: 3400),
                            new TimedTransition(300, "3")
                        ),
                        new State("3",
                            new Shoot(10, count: 12, projectileIndex: 3, coolDown: 3400),
                            new TimedTransition(300, "4")
                        ),
                         new State("4",
                            new Shoot(10, count: 13, projectileIndex: 5, coolDown: 3400),
                            new Shoot(10, count: 9, projectileIndex: 0, coolDown: 3400),
                            new Shoot(10, count: 7, projectileIndex: 1, coolDown: 3400),
                            new TimedTransition(300, "1")
                        )
                    ),
                     new State("heal",
                        new ReturnToSpawn(0.6),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xFFFFFF, 1, 1),
                        new TimedTransition(8000, "spawn")
                        ),
                   new State("spawn",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2000, "fight6")
                    ),
                   new State("fight6",
                        new Swirl(0.4, 4, 8),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 8, shootAngle: 24, projectileIndex: 4, coolDown: 2000),
                        new Shoot(10, count: 6, shootAngle: 16, predictive: 1.5, projectileIndex: 2, coolDown: 1250),
                        new Shoot(10, count: 8, shootAngle: 16, projectileIndex: 0, coolDown: 675),
                        new Grenade(3, 190, 15, coolDown: 1000),
                        new TimedTransition(5750, "fight7")
                       ),
                   new State("fight7",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new StayBack(0.2, 2),
                        new Shoot(10, count: 19, shootAngle: 28, projectileIndex: 2, coolDown: 2000),
                        new Shoot(10, count: 15, shootAngle: 24, projectileIndex: 3, coolDown: 1000),
                        new Grenade(3, 190, 15, coolDown: 1000),
                        new TimedTransition(3750, "rush")
                       ),
                   new State("rush",
                        new Taunt(1.00, "This is your fate!", "Beware!", "Death is what you choose."),
                        new Prioritize(
                        new Follow(.4, 8, 1),
                        new Wander(0.85)
                            ),
                        new Shoot(10, count: 12, shootAngle: 20, projectileIndex: 0, coolDown: 1350),
                        new Shoot(10, count: 6, shootAngle: 24, predictive: 1, projectileIndex: 3, coolDown: 1000),
                        new Shoot(10, count: 10, shootAngle: 24, predictive: 2, projectileIndex: 4, coolDown: 2000),
                        new Shoot(10, count: 5, projectileIndex: 2, coolDown: 900),
                        new TimedTransition(6750, "rush2")
                       ),
                    new State("rush2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(1.00, "Farewell Hero!", "Your death is near."),
                        new Prioritize(
                        new Follow(.4, 8, 1),
                        new Wander(0.75)
                            ),
                        new Shoot(10, count: 20, shootAngle: 1, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(8000, "rush3")
                       ),
                   new State("rush3",
                        new Taunt(1.00, "Seems as if you've been fighting before.", "You have incredible skill."),
                        new Wander(0.65),
                        new Shoot(10, count: 7, shootAngle: 1, projectileIndex: 1, coolDown: 500),
                        new TimedTransition(2000, "fight8")
                       ),
                  new State("fight8",
                      new Flash(0x00FF00, 1, 8),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new TimedTransition(12750, "fight2")
                       ),
                   new State("deathbegins",
                       new Taunt(1.00, "My death will forever be in vein", "I must be strong to be honored", "I'll be back next time.", "Must keep a peaceful demeanor."),
                        new ReturnToSpawn(0.5),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0x0000FF, 1, 6),
                        new TimedTransition(8000, "Dead")
                        ),
                   new State("Dead",
                       new Shoot(10, count: 10, projectileIndex: 1, coolDown: 9999),
                       new Suicide()
                    )
                 ),

            new Threshold(0.01,
                new ItemLoot("Mark of the Marble Colossus", 1),
                    new TierLoot(12, ItemType.Weapon, 0.015),
                    new TierLoot(13, ItemType.Armor, 0.015),
                    new TierLoot(6, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new ItemLoot("Greater Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Special Crate", 0.1),
                    new ItemLoot("Potion of Attack", 1),
                    new ItemLoot("Greater Potion of Critical Chance", 0.04),
                    new ItemLoot("Light Armor Schematic", 0.005, damagebased: true),
                    new ItemLoot("Robe Schematic", 0.005, damagebased: true),
                    new ItemLoot("Heavy Armor Schematic", 0.005, damagebased: true),
                    new ItemLoot("Potion of Critical Damage", 0.04),
                    new ItemLoot("Marble Seal", 0.0006, damagebased: true, threshold: 0.01),
                    new ItemLoot("The Ancient instrument", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Magical Lodestone", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Sword of the Colossus", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Breastplate of New Life", 0.001, damagebased: true, threshold: 0.01),
                    new TierLoot(2, ItemType.Potion)
                    )
            )

        #endregion

        #region LH Pots
         .Init("LH Pot Spawner Random",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new TransformOnDeath("LH Evil Spirit 1"),
                     new State("Randomize2",
                         new PlayerWithinTransition(200, "Spawn", true)
                         ),
                    new State("Spawn",
                        new TossObject("LH Pot 1", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 2", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 3", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 4", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 5", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 6", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TossObject("LH Pot 7", maxRange: 9, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: true),
                        new TimedTransition(6000, "wait")
                        ),
                    new State("wait",
                        new EntitiesNotExistsTransition(250, "Die", "LH Pot 1", "LH Pot 2", "LH Pot 3", "LH Pot 4", "LH Pot 5", "LH Pot 6", "LH Pot 7")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )


        
         .Init("LH Evil Spirit 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.ArmorBroken, true),
                    new Prioritize(
                        new Follow(0.6, 7, 1),
                        new Wander(0.6)
                        ),
                    new Shoot(6, count: 2, shootAngle: 10, coolDown: 1000)
                    )
            )
         .Init("LH Pot 1",
                new State(
                    new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                 new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 2",
                new State(
                     new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                   new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 3",
                new State(
                 new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                  new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 4",
                new State(
                 new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                  new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 5",
                new State(
                   new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                   new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 6",
                new State(
                   new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
         .Init("LH Pot 7",
                new State(
                   new ScaleHP2(40,3,15),
                      new State("nothing"
                    )
                    ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Potion, 0.1),
                   new TierLoot(1, ItemType.Potion, 0.1, numRequired: 0)
                    )
                    )
        #endregion

        #region LH Crusaders
        .Init("LH Commander of the Crusade",
            new State(
                new ScaleHP2(40,3,15),
                new State("fight",
                    new HpLessTransition(0.50, "50"),
                    new Follow(0.2, 8, 1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new Shoot(8.4, count: 4, shootAngle: 8, projectileIndex: 0, coolDown: 1000),
                    new Shoot(8.4, count: 8, projectileIndex: 0, coolDown: 4000),
                    new Spawn("LH Crusade Shipwright", 2, 1, 9999999, givesNoXp: false),
                    new Spawn("LH Crusade Soldier", 2, 1, 9999999, givesNoXp: false),
                    new Spawn("LH Crusade Explorer", 2, 1, 9999999, givesNoXp: false)
                    ),
                 new State("fight2",
                     new Follow(0.2, 8, 1),
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Shoot(8.4, count: 4, shootAngle: 8, projectileIndex: 0, coolDown: 1000),
                    new Shoot(8.4, count: 8, projectileIndex: 0, coolDown: 4000)
                    ),
                new State("50",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0x00FF00, 1, 8),
                    new Spawn("LH Crusade Shipwright", 2, 1, 9999999, givesNoXp: false),
                    new Spawn("LH Crusade Soldier", 2, 1, 9999999, givesNoXp: false),
                    new Spawn("LH Crusade Explorer", 2, 1, 9999999, givesNoXp: false),
                    new TimedTransition(2000, "fight2")
                )
            )
            )
         .Init("LH Crusade Shipwright",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",

                     new Follow(0.2, 8, 1),
                     new Wander(0.1),
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 450)
                )
            )
            )
         .Init("LH Crusade Soldier",
                        new State(

                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Follow(0.2, 8, 1),
                     new Wander(0.1),
                     new Shoot(8.4, count: 3, projectileIndex: 0, coolDown: 650)
                )
            )
            )
        .Init("LH Crusade Explorer",
                              new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Follow(1, 8, 1),
                     new Charge(3, 8, 2000),
                     new Wander(0.1),
                     new Shoot(1, count: 2, projectileIndex: 0, coolDown: 350)
                )
            )
            )
        #endregion

        #region LH Oryx
        .Init("LH Oryx Swordsman",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Wander(0.1),
                     new Orbit(0.65, 4, 10, "LH Champion of Oryx"),
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Shoot(180, count: 4, rotateAngle: 45, projectileIndex: 0, coolDown: 1050),
                     new HpLessTransition(0.50, "fight2")
                       ),
                       new State("fight2",
                           new Follow(1.5, 10, 1),
                            new Flash(0x00FF00, 1, 8),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(180, count: 4, projectileIndex: 0, coolDown: 1000, predictive: 0.4, fixedAngle: 45)
                )
            )
            )
       .Init("LH Oryx Armorbearer",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Wander(0.1),
                     new Orbit(0.8, 7, 10, "LH Champion of Oryx"),
                     new ConditionalEffect(ConditionEffectIndex.Armored),
                     new Shoot(90, count: 8, fixedAngle: 0, projectileIndex: 0, coolDown: 1000),
                     new HpLessTransition(0.50, "fight2")
                    ),
                       new State("fight2",
                           new Follow(1, 10, 1),
                            new Flash(0x00FF00, 1, 8),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(90, count: 4, fixedAngle: 10, projectileIndex: 0, coolDown: 1500, predictive: 0.4),
                            new Shoot(95, count: 3, fixedAngle: 15, projectileIndex: 0, coolDown: 1250, predictive: 0.4),
                            new Shoot(85, count: 2, fixedAngle: 20, projectileIndex: 0, coolDown: 1000, predictive: 0.4),
                           new TimedTransition(4000, "fight")
                )
            )
            )
         .Init("LH Oryx Admiral",
                  new State(
                     new ScaleHP2(40,3,15),
                     new State("fight",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                     new StayBack(2, 5),
                     new Charge(4, 10, 2000),
                     new Follow(1, 10),
                     new Shoot(20, count: 3, projectileIndex: 1, shootAngle: 15, coolDown: 1400, predictive: 0.4),
                     new Shoot(20, count: 2, projectileIndex: 1, shootAngle: 15, coolDown: 1200, predictive: 0.4),
                     new Shoot(20, count: 1, projectileIndex: 1, shootAngle: 15, coolDown: 1000, predictive: 0.4),
                     new TimedTransition(4300, "fight2", false)
                       ),
                     new State("fight2",
                     new StayBack(2, 5),
                     new Charge(4, 10, 2000),
                     new Follow(1, 10),
                     new Shoot(20, count: 3, projectileIndex: 0, shootAngle: 15, coolDown: 1000, predictive: 0.4),
                     new Shoot(20, count: 2, projectileIndex: 0, shootAngle: 15, coolDown: 1200, predictive: 0.4),
                     new Shoot(20, count: 1, projectileIndex: 0, shootAngle: 15, coolDown: 1400, predictive: 0.4),
                     new TimedTransition(4300, "fight", false)
                )
            )
            )
        .Init("LH Champion of Oryx",
                  new State(
                     new ScaleHP2(40,3,15),
                     new State("fight",
                     new StayCloseToSpawn(1, 2),
                     new Spawn("LH Oryx Armorbearer", 5, 1, 9999999, givesNoXp: false),
                     new Spawn("LH Oryx Swordsman", 4, 1, 9999999, givesNoXp: false),
                     new Spawn("LH Oryx Admiral", 1, 1, 9999999, givesNoXp: false),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                     new Shoot(20, count: 5, shootAngle: 15, predictive: 0.4, coolDown: 1000),
                     new HpLessTransition(0.50, "fight2"),
                     new EntitiesNotExistsTransition(10, "fightnon1", "LH Oryx Armorbearer", "LH Oryx Swordsman", "LH Oryx Admiral")
                       ),
                     new State("fightnon1",
                     new Flash(0x00FF00, 90, 90),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(44.5, count: 16, coolDown: 1100),
                     new PlayerWithinTransition(4, "fightnoninvul1", false),
                     new HpLessTransition(0.50, "fight2")
                           ),
                     new State("fightnoninvul1",
                     new Shoot(44.5, count: 16, coolDown: 1100),
                     new NoPlayerWithinTransition(4, "fightnon1"),
                     new HpLessTransition(0.50, "fight2")
                          ),
                     new State("fight2",
                     new Flash(0x00FF00, 1, 8),
                     new StayCloseToSpawn(1, 3),
                     new Spawn("LH Oryx Armorbearer", 5, 1, 9999999, givesNoXp: false),
                     new Spawn("LH Oryx Swordsman", 4, 1, 9999999, givesNoXp: false),
                     new Spawn("LH Oryx Admiral", 1, 1, 9999999, givesNoXp: false),
                     new Shoot(44.5, count: 16, coolDown: 1100),
                     new EntitiesNotExistsTransition(10, "fightnon2", "LH Oryx Armorbearer", "LH Oryx Swordsman", "LH Oryx Admiral"),
                     new HpLessTransition(0.10, "fight3")
                          ),
                     new State("fightnon2",
                     new Flash(0x00FF00, 90, 90),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new Shoot(44.5, count: 16, coolDown: 1100),
                     new PlayerWithinTransition(4, "fightnoninvul2", false),
                     new HpLessTransition(0.10, "fight3")
                               ),
                     new State("fightnoninvul2",
                     new Shoot(44.5, count: 16, coolDown: 1100),
                     new NoPlayerWithinTransition(4, "fightnon2"),
                     new HpLessTransition(0.10, "fight3")
                            ),
                     new State("fight3",
                     new Charge(4, 10, 6000),
                     new Follow(1.5, 6),
                     new Spawn("LH Oryx Admiral", 1, 1, 9999999),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                     new Shoot(44.5, count: 16, coolDown: 1100)
                )
            )
            )
        #endregion

        #region LH Enemy Spawner

             .Init("LH Spawner Random",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                     new State("Randomize2",
                         new PlayerWithinTransition(200, "Randomize", true)
                         ),
                         new State("Randomize",
                         new TimedRandomTransition(50, false, "SpawnLHCrusade", "SpawnLHOryx", "SpawnLHGolem", "SpawnLHGrotto")
                         ),
                        new State("SpawnLHOryx",
                        new Spawn("LH Champion of Oryx", 1, 1, 9999999, givesNoXp: false),
                        new TimedTransition(2000, "Die")
                        ),
                        new State("SpawnLHGolem",
                        new Spawn("LH Tormented Golem", 1, 1, 9999999, givesNoXp: false),
                        new TimedTransition(2000, "Die")
                        ),
                        new State("SpawnLHGrotto",
                        new Spawn("LH Grotto Blob", 1, 1, 9999999, givesNoXp: false),
                        new TimedTransition(2000, "Die")
                        ),
                        new State("SpawnLHCrusade",
                        new Spawn("LH Commander of the Crusade", 1, 1, 9999999, givesNoXp: false),
                        new TimedTransition(2000, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )

        #endregion

        #region LH Grotto
        .Init("LH Grotto Blob",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Spawn("LH Grotto Bat", 2, 1, givesNoXp: false),
                     new Spawn("LH Grotto Rat", 2, 1, givesNoXp: false),
                     new Shoot(26, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(24, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(28, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new HpLessTransition(0.66, "fight2")
                       ),
                     new State("fight2",
                     new ChangeSize(10, 80),
                     new Flash(0x00FF00, 1, 8),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2000, "fight3")
                           ),
                     new State("fight3",
                     new Spawn("LH Grotto Slime", 3, 1, givesNoXp: false),
                     new Spawn("LH Grotto Bat", 2, 1, givesNoXp: false),
                     new Spawn("LH Grotto Rat", 2, 1, givesNoXp: false),
                      new Shoot(26, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(24, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(28, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new HpLessTransition(0.33, "fight4")
                           ),
                     new State("fight4",
                     new ChangeSize(10, 65),
                     new Flash(0x00FF00, 1, 8),
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new TimedTransition(2000, "fight5")
                              ),
                     new State("fight5",
                     new Wander(0.1),
                     new Spawn("LH Grotto Slime", 4, 1, givesNoXp: false),
                     new Spawn("LH Grotto Bat", 2, 1, givesNoXp: false),
                     new Spawn("LH Grotto Rat", 2, 1, givesNoXp: false),
                   new Shoot(26, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(24, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200),
                     new Shoot(28, fixedAngle: 0, count: 3, rotateAngle: 15, coolDown: 200)
                )
            )
            )

         .Init("LH Grotto Slime",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                    new Wander(0.3),
                     new Shoot(radius: 26, count: 1, shootAngle: 15, projectileIndex: 0, coolDown: 400)
                       )
                      )
            )

     .Init("LH Grotto Bat",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                    new Wander(0.3),
                    new Follow(1, 5, 0),
                     new Shoot(radius: 26, count: 1, shootAngle: 15, projectileIndex: 0, coolDown: 800)
                       )
                      )
            )
        .Init("LH Grotto Rat",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Charge(4, 7, 800),
                     new Shoot(radius: 26, count: 1, shootAngle: 15, projectileIndex: 0, coolDown: 800)
                       )
                      )
            )
        #endregion

        #region LH Golems
          .Init("LH Tormented Golem",
                  new State(
                       new ScaleHP2(40,3,15),
                        new State("TossObjects",
                        new TossObject("LH Golem of Fear Anchor", 5, angle: 45, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear Anchor", 5, angle: 135, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear Anchor", 5, angle: 225, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear Anchor", 5, angle: 315, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear", 5, angle: 45, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear", 5, angle: 135, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear", 5, angle: 225, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Fear", 5, angle: 315, coolDown: 9999999, throwEffect: false),
                        new TossObject("LH Golem of Anger", maxRange: 7, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: false),
                        new TossObject("LH Golem of Anger", maxRange: 7, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: false),
                        new TossObject("LH Golem of Sorrow", maxRange: 7, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: false),
                        new TossObject("LH Golem of Sorrow", maxRange: 7, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: false),
                        new TossObject("LH Golem of Sorrow", maxRange: 7, minRange: 0, coolDown: 9999999, minAngle: 0, maxAngle: 300, throwEffect: false),
                      new TimedTransition(1500, "fight")
                            ),
                new State("fight",
                     new Charge(4, 5, 7000),
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new TimedTransition(7000, "fight1/2"),
                     new HpLessTransition(0.66, "fight2")
                       ),
                     new State("fight1/2",
                     new Charge(4, 5, 7000),
                     new StayCloseToSpawn(1, 8),
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new ConditionalEffect(ConditionEffectIndex.ArmorBroken, false, 1500),
                     new HpLessTransition(0.66, "fight2"),
                     new TimedTransition(7000, "fight")
                           ),
                     new State("fight2",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new ReturnToSpawn(1),
                     new Flash(0x00FF00, 1, 8),
                     new TimedTransition(3000, "fight12")
                       ),
                     new State("fight12",
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new TimedTransition(7000, "fight2/3"),
                     new HpLessTransition(0.33, "fight3")
                       ),
                     new State("fight2/3",
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new ConditionalEffect(ConditionEffectIndex.ArmorBroken, false, 1500),
                     new HpLessTransition(0.33, "fight3"),
                     new TimedTransition(7000, "fight12")
                                ),
                     new State("fight3",
                     new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                     new ReturnToSpawn(1),
                     new Flash(0x00FF00, 1, 8),
                     new TimedTransition(3000, "fight13")
                           ),
                     new State("fight13",
                     new Charge(4, 8, 7000),
                     new StayCloseToSpawn(1, 8),
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new TimedTransition(7000, "fight3/3")
                           ),
                     new State("fight3/3",
                     new Charge(4, 8, 7000),
                     new StayCloseToSpawn(1, 8),
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 2000),
                     new ConditionalEffect(ConditionEffectIndex.ArmorBroken, false, 3000),
                     new TimedTransition(7000, "fight13")
                           )
                ),
                new Threshold(0.01,
                    new ItemLoot("TricksterST3", 0.006, damagebased: true, threshold: 0.01),
                     new ItemLoot("Potion of Critical Chance", 0.01),
                    new ItemLoot("Potion of Critical Damage", 0.01)
                )
            )
            
           .Init("LH Golem of Anger",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Follow(1, 7, 0),
                     new Shoot(40, fixedAngle: 0, count: 18, coolDown: 1550, projectileIndex: 1),
                     new Shoot(radius: 26, count: 1, shootAngle: 15, projectileIndex: 0, coolDown: 250)
                    )
                      )
            )
        .Init("LH Golem of Sorrow",
                  new State(
                       new ScaleHP2(40,3,15),
                new State("fight",
                     new Follow(1, 7, 0),
                     new Shoot(45, fixedAngle: 0, count: 16, coolDown: 1550, projectileIndex: 1),
                     new Shoot(2, fixedAngle: 0, count: 2, coolDown: 250, projectileIndex: 0)
                    )
                      )
            )
         .Init("LH Golem of Fear",
                new State(
                      new ScaleHP2(40, 3, 15),
                new State("fight",
                     new Orbit(0.7, 3, 4, "LH Golem of Fear Anchor"),
                     new Shoot(180, count: 4, projectileIndex: 1, coolDown: 800, fixedAngle: 45, coolDownOffset: 400),
                     new Shoot(180, count: 4, projectileIndex: 0, coolDown: 1200, coolDownOffset: 600)

                      )
            )
            )
            .Init("LH Golem of Fear Anchor",
              new State(
              new ConditionalEffect(ConditionEffectIndex.Invincible),
              new State("fight"
                    
            )
                  )
            )



        #endregion

        #region Misc
        .Init("LH Marble Teleporter",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(8, 35, 48, true)
                )
            )
            )
            .Init("LH Void Spawner",
                new State(
                    new DropPortalOnDeath("Void Entity Portal", 100, 360),
                    new Taunt("Use the [Vial of Pure Darkness] to unlock the Void portal."),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1"
                        ),
                     new State("Open",
                         new Taunt("The Void has been opened!"),
                         new Suicide()
                )
            )
            )
                .Init("LH Void Controller",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("1",
                        new ChangeSize(250, 0)
                        ),
                    new State("2",
                      new ApplySetpiece("Void1")
                        ),
                    new State("3",
                      new ApplySetpiece("Void2")
                        )
                    )
                )
            .Init("LH Void Spawner2",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                        new Reproduce("LH Void Fragment", 3, 50, coolDown: 15000)
                        )
                    )
                )
            .Init("LH Void Split",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                   new GroundTransform("LH Void Landmark C")
                        ),
                    new State("3",
                   new GroundTransform("LH Void Platform")
                        )
                    )
                )
          .Init("Jungle Summoner",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                   new GroundTransform("LH Void Landmark C")
                        ),
                    new State("3",
                   new GroundTransform("LH Void Platform")
                        )
                    )
                )
        .Init("Patty Coin Drop",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true)
                        ),
                    new State("2",
                   new GroundTransform("LH Void Landmark C")
                        ),
                    new State("3",
                   new GroundTransform("LH Void Platform")
                        )
                    )
                )
           .Init("LH Void Opener",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new OrderOnce(20,"LH Void Spawner", "Open"),
                        new TimedTransition(2000,"2")
                        ),
                     new State("2",
                         new Suicide()
                )
            )
            )
        #endregion

        #region LH Titan

        .Init("LH Agonized Titan",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath(target: "Cultist Hideout Portal"),
                    new State("Ini",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                       new EntitiesNotExistsTransition(5000, "transition1", "LH Pot Spawner Random")
                    ),
                    new State("transition1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(8, "transition2", true)
                        ),
                    new State("transition2",
                        new Taunt("You have passed my test, your life is worthy."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x00FF00, 0.25, 12),
                        new TimedTransition(6500, "spiral")
                        ),
                    new State("spiral",
                        new Shoot(radius: 24, count: 3, shootAngle: 15, projectileIndex: 1, coolDown: 1000),
                        new Shoot(72, fixedAngle: 0, count: 5, rotateAngle: 15, coolDown: 200),
                        new HpLessTransition(0.50, "50")
                        ),
                     new State("50",
                        new Flash(0x00FF00, 0.25, 12),
                        new Shoot(radius: 24, count: 3, shootAngle: 15, projectileIndex: 6, coolDown: 1000),
                        new Shoot(72, fixedAngle: 0, count: 5, rotateAngle: 15, coolDown: 200),
                        new ReproduceChildren(1, 1, 5000, "LH Evil Spirit 1")
                        )
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Potion),
                    new ItemLoot("TricksterST0", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("TricksterST1", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("TricksterST2", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("TricksterST3", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.04),
                    new ItemLoot("Potion of Critical Damage", 0.04),
                    new ItemLoot("50 Credits", 0.01)
                )
            )

        #endregion

        #region LH Cultist
         .Init("Cultist of the Halls",
                new State(
                   new ScaleHP2(40,3,15),
                      new DropPortalOnDeath("Glowing Realm Portal", 100, 360),
                    new State("default",
                        new PlayerWithinTransition(12, "awoken")
                        ),
                    new State("awoken",
                        new Flash(0xFF0000, 6, 6),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(1.00, "You had a choice!", "Walk away while you can!", "I will not allow anyone near the halls!"),
                        new TimedTransition(4500, "FollowShootPhase")
                        ),
                    new State("FollowShootPhase",
                        new Wander(0.65),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 9, projectileIndex: 1, coolDown: 2500),
                        new Shoot(10, count: 7, projectileIndex: 2, coolDown: 3088),
                        new Shoot(10, count: 2, projectileIndex: 3, coolDown: 1500),
                        new TimedTransition(6750, "FoollowShootPhase2")
                        ),
                    new State("FoollowShootPhase2",
                        new Wander(0.65),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 12, projectileIndex: 2, coolDown: 2500),
                        new Shoot(10, count: 13, projectileIndex: 5, coolDown: 150),
                        new TimedTransition(6000, "WanderAndRek")
                        ),
                    new State("WanderAndRek",
                        new Follow(0.4, 6, 1),
                        new Flash(0xFF0000, 6, 6),
                        new Shoot(10, count: 24, projectileIndex: 10, coolDown: 4000),
                        new TimedTransition(8000, "TellAndMove")
                        ),
                    new State("TellAndMove",
                        new ReturnToSpawn(0.5),
                        new Taunt(1.00, "Must Protect!", "Protect the halls!", "You will never enter!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(4000, "GoBlam")
                        ),
                    new State("GoBlam",
                        new Shoot(10, count: 1, shootAngle: 10, projectileIndex: 12, coolDown: 300, predictive: 0.5),
                        new Shoot(10, count: 2, shootAngle: 20, projectileIndex: 11, coolDown: 300, predictive: 0.5),
                        new Shoot(10, count: 3, shootAngle: 30, projectileIndex: 14, coolDown: 300, predictive: 0.5),
                        new Shoot(10, count: 10, shootAngle: 30, fixedAngle: 0, projectileIndex: 13, coolDown: 300, predictive: 0.5),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new TimedTransition(6000, "FollowShootPhase")
                        )
                       ),
                 new Threshold(0.01,
                     new ItemLoot("Mark of Malus", 1),
                    new TierLoot(9, ItemType.Weapon, 0.275),
                    new TierLoot(10, ItemType.Weapon, 0.225),
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(10, ItemType.Armor, 0.15),
                    new TierLoot(11, ItemType.Armor, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.04),
                    new ItemLoot("Potion of Critical Damage", 0.04),
                    new ItemLoot("Vial of Pure Darkness", 0.006, damagebased: true, numRequired: 1 ,threshold: 0.01),
                    new ItemLoot("Robe of the Ancient Cultist", 0.008, damagebased: true, threshold: 0.01),
                    new ItemLoot("Skull of Corrupted Souls", 0.008, damagebased: true, threshold: 0.01),
                    new ItemLoot("Bloodshed Ring", 0.008, damagebased: true, threshold: 0.01),//Vial of Pure Darkness
                    new ItemLoot("Staff of Unholy Sacrifice", 0.008, damagebased: true, threshold: 0.01),
                    new TierLoot(2, ItemType.Potion)
                      )
            )
        #endregion

        #region LH Void Entity
         .Init("LH Void Entity",
                new State(
                    new ScaleHP2(50, 2, 30),
                    new State("1",
                        new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Void.mp3"),
                        new Taunt("Those who enter my realm hold little value of their lives."),
                        new ChangeSize(250, 0),
                        new SetAltTexture(0),
                        new PlayerWithinTransition(10, "2", true)
                        ),
                    new State("2",
                        new ChangeSize(15, 200),
                        new Taunt("Ah, heroes, blessed souls… I shall destroy you with the power of shadow!"),
                        new TimedTransition(5000, "3")
                        ),
                    new State("3",
                        new TimedTransition(100, "4", true),
                        new TimedTransition(100, "5", true),
                        new TimedTransition(100, "6", true),
                        new TimedTransition(100, "7", true),
                        new TimedTransition(100, "8", true),
                        new TimedTransition(100, "9", true),
                        new TimedTransition(100, "10", true)
                        ),
                new State(
                    new Orbit(1, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new HpLessTransition(0.8, "11"),
                    new State("4",
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "3")
                        ),
                    new State("5",
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "3")
                        ),
                    new State("6",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "3")
                        ),
                    new State("7",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "3")
                        ),
                    new State("8",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "3")
                        ),
                    new State("9",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "3")
                        ),
                    new State("10",
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "3")
                            )
                        ),
                new State(
                    new TimedRandomTransition(60000, true, "mid"),
                    new Orbit(1.5, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new HpLessTransition(0.4, "13"),
                    new State("11",
                        new Taunt("Spirits of transgression, I command you to swarm these blights!"),
                  //      new Order(99, "LH Void Spawner2", "2"),
                   //     new Order(99, "LH Void Controller", "2"),
                       new Order(99, "LH Void Split", "2"),
                        new TimedTransition(2000, "12")
                        ),
                    new State("12",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(100, "4a", true),
                        new TimedTransition(100, "5a", true),
                        new TimedTransition(100, "6a", true),
                        new TimedTransition(100, "7a", true),
                        new TimedTransition(100, "8a", true),
                        new TimedTransition(100, "9a", true),
                        new TimedTransition(100, "10a", true)
                        ),
                    new State("4a",
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "12")
                        ),
                    new State("5a",
                        new Order(99, "Jungle Summoner", "2"),
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "12")
                        ),
                    new State("6a",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "12")
                        ),
                    new State("7a",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "12")
                        ),
                    new State("8a",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "12")
                        ),
                    new State("9a",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "12")
                        ),
                    new State("10a",
                     //   new Order(99, "Patty Coin drop", "2"),
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "12")
                                )
                            ),
                new State(
                    new HpLessTransition(0.2, "ends"),
                    new Orbit(2, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                    new TimedRandomTransition(60000, true, "mid"),
                    new State("13",
                        new Taunt("I have fed off the sorrow and hatred of the lost souls… AND I WILL FEED AGAIN!", "HA HA HA! You shall be enveloped by darkness!"),
                        new TimedTransition(1000, "14")
                        ),
                    new State("14",
                        new TimedTransition(100, "4b", true),
                        new TimedTransition(100, "5b", true),
                        new TimedTransition(100, "6b", true),
                        new TimedTransition(100, "7b", true),
                        new TimedTransition(100, "8b", true),
                        new TimedTransition(100, "9b", true),
                        new TimedTransition(100, "10b", true)
                        ),
                    new State("4b",
                            new Order(99, "Patty Coin Drop", "2"),
                        new Shoot(99, 12, 11, 5, coolDown: 3000),
                        new Shoot(99, 3, 14, 0, coolDown: 600),
                        new TimedTransition(8000, "14")
                        ),
                    new State("5b",
                        new Shoot(99, 8, 14, 4, coolDown: 1500),
                        new Shoot(99, 3, 13, 0, coolDown: 750),
                        new TimedTransition(8000, "14")
                        ),
                    new State("6b",
                        new Shoot(99, 6, 22, 0, coolDown: 2000),
                        new Shoot(99, 3, 20, 3, coolDown: 800),
                        new TimedTransition(8000, "14")
                        ),
                    new State("7b",
                        new Shoot(99, 10, 9, 0, coolDown: 1000),
                        new Shoot(99, 2, 12, 1, coolDown: 400),
                        new TimedTransition(8000, "14")
                        ),
                    new State("8b",
                        new Shoot(99, 9, 13, 6, coolDown: 1000),
                        new TimedTransition(8000, "14")
                        ),
                    new State("9b",
                        new Shoot(99, 10, 7, 7, coolDown: 1000),
                        new Shoot(99, 3, 8, 6, coolDown: 300),
                        new TimedTransition(8000, "14")
                        ),
                    new State("10b",
                        new Shoot(99, 16, null, 4, fixedAngle: 24, coolDown: 1000),
                        new Shoot(99, 2, 12, 5, coolDown: 400),
                        new TimedTransition(8000, "14")
                                )
                            ),
                new State(
                    new State("mid",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(25),
                        new TimedTransition(100, "mid2")
                        ),
                    new State("mid2",
                        new SetAltTexture(26),
                        new TimedTransition(100, "mid3")
                        ),
                    new State("mid3",
                        new SetAltTexture(27),
                        new TimedTransition(100, "mid4")
                        ),
                    new State("mid4",
                        new SetAltTexture(28),
                        new TimedTransition(100, "mid4a")
                        ),
                    new State("mid4a",
                        new SetAltTexture(29),
                        new TimedTransition(100, "mid5")
                        ),
                    new State("mid5",
                        new SetAltTexture(30),
                        new TimedTransition(2000, "mid6")
                        ),
                    new State("mid6",
                         new MoveTo(10, 54, 49),
                        new TimedTransition(1000, "mid7")
                        ),
                    new State("mid7",
                        new SetAltTexture(18),
                        new TimedTransition(50, "mid8")
                        ),
                    new State("mid8",
                        new SetAltTexture(19),
                        new TimedTransition(50, "mid9")
                        ),
                    new State("mid9",
                        new SetAltTexture(20),
                        new TimedTransition(50, "mid10")
                        ),
                    new State("mid10",
                        new SetAltTexture(21),
                        new TimedTransition(50, "mid11")
                        ),
                    new State("mid11",
                        new SetAltTexture(22),
                        new TimedTransition(50, "mid12")
                        ),
                    new State("mid12",
                        new SetAltTexture(23),
                        new TimedTransition(50, "mid13")
                        ),
                    new State("mid13",
                        new SetAltTexture(24),
                        new TimedTransition(50, "mid14")
                        ),
                    new State("mid14",
                        new SetAltTexture(0),
                        new HpLessTransition(0.2, "ends1"),
                        new TimedTransition(2000, "15", true),
                        new TimedTransition(2000, "16", true),
                        new TimedTransition(2000, "17", true),
                        new TimedTransition(2000, "18")
                        ),
                    new State("15",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                         new Shoot(60, 10, 20, 6, coolDown: 150, rotateAngle: 20),
                        new TimedTransition(6000, "19")
                        ),
                    new State("16",
                         new Shoot(60, 10, 20, 6, coolDown: 300, rotateAngle: 20),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("17",
                        new Shoot(99, 18, 20, 5, coolDown: 800),
                     //   new TossObject("LH Greater Void Shade", 15, 90, 2900),
                     //   new TossObject("LH Greater Void Shade", 15, 180, 2900),
                     //   new TossObject("LH Greater Void Shade", 15, 270, 2900),
                     //   new TossObject("LH Greater Void Shade", 15, 360, 2900),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("18",
                        new Shoot(60, 10, 20, 6, coolDown: 400, rotateAngle: 20),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(6000, "19")
                        ),
                    new State("19",
                        new Wander(0.1),
                        new StayCloseToSpawn(0.4, 1),
                        new TimedTransition(2000, "return1")
                            ),
                    new State("return1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(25),
                        new TimedTransition(50, "return2")
                        ),
                    new State("return2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(26),
                        new TimedTransition(50, "return3")
                        ),
                    new State("return3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(27),
                        new TimedTransition(50, "return4")
                        ),
                    new State("return4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(28),
                        new TimedTransition(50, "return5")
                        ),
                    new State("return5",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(29),
                        new TimedTransition(50, "return6")
                        ),
                    new State("return6",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new SetAltTexture(30),
                        new TimedTransition(1000, "return7")
                        ),
                    new State("return7",
                         new MoveTo(10, 54, 49),
                        new TimedTransition(1000, "return8")
                        ),
                    new State("return8",
                        new SetAltTexture(18),
                        new TimedTransition(50, "return9")
                        ),
                    new State("return9",
                        new SetAltTexture(19),
                        new TimedTransition(50, "return10")
                        ),
                    new State("return10",
                        new SetAltTexture(20),
                        new TimedTransition(50, "return11")
                        ),
                    new State("return11",
                        new SetAltTexture(21),
                        new TimedTransition(50, "return12")
                        ),
                    new State("return12",
                        new SetAltTexture(22),
                        new TimedTransition(50, "return13")
                        ),
                    new State("return13",
                        new SetAltTexture(23),
                        new TimedTransition(50, "return14")
                        ),
                    new State("return14",
                        new SetAltTexture(24),
                        new TimedTransition(100, "return15")
                        ),
                    new State("return15",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "12")
                            )
                        ),
                    new State("ends",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.5, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                      //  new Order(99, "LH Void Spawner2", "1"),
                        new Taunt("ALL NOW ENDS!"),
                        new TimedTransition(5000, "mid")
                        ),
                    new State("ends1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Shoot(99, 18, 20, 5, coolDown: 1000, coolDownOffset: 1500),
                        new TimedTransition(10000, "ends2")
                        ),
                    new State("ends2",
                        new TimedTransition(100, "ends3")
                        ),
                    new State("ends3",
                        new Shoot(60, 10, 20, 6, coolDown: 150, rotateAngle: 20),
                        new TimedTransition(8000, "ends4")
                        ),
                    new State("ends4",
                        new Shoot(60, 10, 20, 6, coolDown: 300, rotateAngle: 20),
                        new TimedTransition(8000, "ends5")
                        ),
                    new State("ends5",
                        new Shoot(60, 10, 20, 6, coolDown: 400, rotateAngle: 20),
                        new TimedTransition(8000, "survival1")
                        ),
                    new State("survival1",
                        new TossObject("LH Void Entity N", range: 20, angle: 0, coolDown: 99999),
                        new TossObject("LH Void Entity W", range: 20, angle: 270, coolDown: 99999),
                        new TossObject("LH Void Entity S", range: 20, angle: 180, coolDown: 99999),
                        new TossObject("LH Void Entity E", range: 20, angle: 90, coolDown: 99999),
                        new Taunt("Brace yourselves, for this shall be the last battle your precious realm will ever see!"),
                        new TimedTransition(5000, "survival2")
                        ),
                    new State("survival2",
                      //  new TossObject("LH Greater Void Shade", 10, 45, 12000),
                      //  new TossObject("LH Greater Void Shade", 10, 135, 12000),
                      //  new TossObject("LH Greater Void Shade", 10, 235, 12000),
                      //  new TossObject("LH Greater Void Shade", 10, 315, 12000),
                        new EntitiesNotExistsTransition(99, "speech1", "LH Void Entity N", "LH Void Entity S", "LH Void Entity E", "LH Void Entity W")
                        ),
                    new State("speech1",
                        new Taunt(true, "A fallen king, an imprisoned queen, an effigy of gold."),
                        new TimedTransition(1500, "speech2")
                        ),
                    new State("speech2",
                        new Taunt(true, "A glacial elder who knows more than you think, and a necromancer of old."),
                        new TimedTransition(1500, "speech3")
                        ),
                    new State("speech3",
                        new Taunt(true, "An experiment that surpassed its master, a ventroliquist's final show."),
                        new TimedTransition(1500, "speech4")
                        ),
                    new State("speech4",
                        new Taunt(true, "And a white titan to conquer the Mad God…"),
                        new TimedTransition(1500, "speech5")
                        ),
                    new State("speech5",
                        new Taunt(true, "ALL UNDER MY CONTROL!"),
                        new TimedTransition(2000, "shotgun")
                        ),
                    new State("shotgun",
                        new Shoot(99, 18, null, 21, fixedAngle: 20, coolDown: 490),
                        new TimedTransition(2000, "shotgun2")
                        ),
                    new State("shotgun2",
                        new Shoot(99, 10, 10, 21, coolDown: 490),
                        new TimedTransition(4000, "final")
                        ),
                    new State("final",
                        new Order(199, "LH Void Split", "3"),
                        new Order(199, "Jungle Summoner", "3"),
                        new Order(199, "Patty Coin Drop", "3"),
                        new SetAltTexture(1),
                        new Taunt("No…NO! THIS IS NOT THE END!"),
                        new TimedTransition(3000, "chest1")
                        ),
                    new State("chest1",
                        new SetAltTexture(2),
                        new TimedTransition(100, "chest2")
                        ),
                    new State("chest2",
                        new SetAltTexture(3),
                        new TimedTransition(100, "chest3")
                        ),
                    new State("chest3",
                        new SetAltTexture(4),
                        new TimedTransition(100, "chest4")
                        ),
                    new State("chest4",
                        new SetAltTexture(5),
                        new TimedTransition(1500, "chest5")
                        ),
                    new State("chest5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Wander(0.8),
                        new HpLessTransition(0.08, "death")
                        ),
                    new State("death",
                        new Taunt("Naivety! You really think you have won? The inevitable has merely been delayed. One day you shall all be slain, and on that day I will claim your souls in vengeance."),
                        new Suicide()
                        )
                    ),
                 new Threshold(0.00001,
                     new ItemLoot("Mark of the Void Entity", 1),
                    new TierLoot(12, ItemType.Weapon, 0.024),
                    new TierLoot(13, ItemType.Armor, 0.024),
                    new TierLoot(6, ItemType.Ability, 0.24),
                    new TierLoot(6, ItemType.Ring, 0.06),
                    new ItemLoot("Potion of Mana", 0.9),
                    new ItemLoot("Potion of Life", 0.7),
                    new ItemLoot("50 Credits", 1),
                    new ItemLoot("Special Crate", 0.1),
                    new ItemLoot("Miscellaneous Crate", 0.1),
                    new ItemLoot("Fragments of Void Matter", 0.0045),
                    new ItemLoot("Potion of Greater Life", 0.7),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("Light Armor Schematic", 0.015, damagebased: true),
                    new ItemLoot("Robe Schematic", 0.015, damagebased: true),
                    new ItemLoot("Heavy Armor Schematic", 0.015, damagebased: true),
                    new ItemLoot("Sourcestone", 0.001, damagebased: true, threshold: 0.01),//Cloak of Bloody Concealment
                    new ItemLoot("Armor of Nil", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Voided Quiver", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Bow of the Void", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Omnipotence Ring", 0.0006, damagebased: true, threshold: 0.02),
                    new ItemLoot("Disarray", 0.0006, damagebased: true, threshold: 0.02),
                    new TierLoot(2, ItemType.Potion)
                )
                )
             .Init("LH Void Entity N",
                new State(
                    new ScaleHP2(20, 2, 30),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 2)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "The end of the world is upon us! Relinquish your spirits to the Void, and watch helplessly as your universe is eradicated!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity S",
                new State(
                   new ScaleHP2(20, 2, 30),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 2)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "Eons of meticulous planning have lead to this moment. I will not let such a mindless crowd get in my way!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity E",
                new State(
                   new ScaleHP2(20, 2, 30),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 0)
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "Oryx will kneel before me!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )
            .Init("LH Void Entity W",
                new State(
                    new ScaleHP2(20, 2, 30),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ChangeSize(250, 0),
                        new TimedTransition(5000, "2")
                        ),
                    new State("2",
                        new HpLessTransition(0.4, "3"),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new ChangeSize(20, 200),
                        new Taunt(true, "Join us, heroes! The Void lusts for your souls!"),
                        new Shoot(99, 5, 30, 1),
                        new HpLessTransition(0.4, "3")
                        ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Orbit(0.7, 20, 30, target: "LH Void Controller", orbitClockwise: true),
                        new Taunt(true, "You putrid pawns think you can defy fate? Laughable!"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Suicide()
                        )
                    )
                )

        #endregion

          ;
    }
}