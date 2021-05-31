using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Mothership = () => Behav()
             .Init("Alien U.F.O",
                new State(
                  new ScaleHP2(70,2,15),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "taunt1")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("taunt1",
                        new Flash(0x00FF00, 1, 2),
                        new TimedTransition(3600, "taunt2")
                        ),
                    new State("taunt2",
                        new Taunt("Master, the scouter says this planet is a low threat!", "Low threat mortals? I will overtake your planet!."),
                        new TimedTransition(3000, "taunt3")
                        ),
                    new State("taunt3",
                        new Taunt("I've sent a notice to master, We'll start by killing everyone."),
                        new Flash(0x00FF00, 5, 2),
                        new TimedTransition(5000, "fight1")
                        )
                     ),
                    new State(
                    
                    new State("fight1",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(10, count: 1, shootAngle: 12, predictive: 0.2, projectileIndex: 2, coolDown: new Cooldown(1000, 2000)),
                        new Shoot(10, count: 2, shootAngle: 8, projectileIndex: 3, predictive: 0.3, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(10, count: 3, shootAngle: 6, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(8000, "fight2")
                        ),
                    new State("fight2",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05)
                        ),
                        new Shoot(10, count: 2, shootAngle: 6, projectileIndex: 0, coolDown: 400),
                        new Shoot(10, count: 4, shootAngle: 8, projectileIndex: 2, predictive: 0.2, coolDown: 2000, coolDownOffset: 2000),
                        new Shoot(10, count: 1, projectileIndex: 3, predictive: 0.5, coolDown: new Cooldown(200, 200)),
                        new TimedTransition(8000, "followabit1")
                        ),
                    new State("followabit1",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new Follow(0.3, 8, 1),
                            new StayCloseToSpawn(0.5, 3)
                        ),
                        new Shoot(10, count: 2, shootAngle: 14, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(3000, "GoBackHome2")
                        ),
                    new State("GoBackHome2",
                        new HpLessTransition(0.50, "Overdrive"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(speed: 0.6),
                        new TimedTransition(3600, "fight3")
                        ),
                    new State("fight3",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(10, count: 1, projectileIndex: 0, predictive: 0.2, coolDown: 1000),
                        new Shoot(10, count: 3, shootAngle: 20, projectileIndex: 0, coolDown: 2000),
                        new Shoot(10, count: 3, projectileIndex: 1, coolDown: 3000),
                        new TimedTransition(6000, "GoBackHome1")
                        ),
                    new State("GoBackHome1",
                        new HpLessTransition(0.50, "Overdrive"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(speed: 0.6),
                        new TimedTransition(3600, "chargingshock")
                        ),
                    new State("chargingshock",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Taunt("This planet! How are they this powerful!"),
                        new Flash(0x000000, 1, 1),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 2),
                            new Wander(0.05)
                        ),
                        new Shoot(10, count: 5, projectileIndex: 2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 2, projectileIndex: 1, predictive: 0.2, coolDown: 2500),
                        new Shoot(10, count: 1, projectileIndex: 3, predictive: 0.2, coolDown: 400),
                        new TimedTransition(5000, "followabit2")
                        ),
                    new State("followabit2",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new Follow(0.5, 8, 1),
                            new StayCloseToSpawn(0.5, 3)
                        ),
                        new Shoot(10, count: 5, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(4000, "rumble1")
                        ),
                    new State("rumble1",
                        new HpLessTransition(0.50, "Overdrive"),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 2),
                            new Wander(0.05)
                        ),
                        new Shoot(10, count: 4, shootAngle: 7, predictive: 0.2, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                        new Shoot(10, count: 4, shootAngle: 16, projectileIndex: 2, coolDown: new Cooldown(500, 3000)),
                        new TimedTransition(7400, "fight1")
                        )
                     ),

                    new State(
                        
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("Overdrive",
                        new Flash(0x00FFFF, 1, 2),
                        new ReturnToSpawn(speed: 0.6),
                        new TimedTransition(5200, "OverdriveTaunt")
                        ),
                    new State("OverdriveTaunt",
                        new Taunt("You damn! YOU RUINED THE SHIP!"),
                        new TimedTransition(4000, "Go")
                        ),
                    new State("Go",
                        new Taunt("ENOUGH!"),
                        new TimedTransition(5000, "Zfight1")
                        )
                     ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                    new State("Zfight1",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                        new Shoot(10, count: 7, shootAngle: 12, predictive: 0.2, projectileIndex: 2, coolDown: new Cooldown(1000, 2000)),
                        new Shoot(10, count: 13, shootAngle: 8, projectileIndex: 3, predictive: 0.3, coolDown: 3000, coolDownOffset: 1000),
                        new TimedTransition(8000, "Zfight2")
                        ),
                    new State("Zfight2",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05),
                            new Follow(0.5, 8, 1)
                        ),
                        new Shoot(10, count: 16, shootAngle: 8, predictive: 0.2, projectileIndex: 2, coolDown: 2000, coolDownOffset: 2000),
                        new Shoot(10, count: 5, projectileIndex: 3, predictive: 0.5, coolDown: new Cooldown(200, 200)),
                        new TimedTransition(8000, "Zfollowabit1")
                        ),
                    new State("Zfollowabit1",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new Follow(0.3, 8, 1),
                            new StayCloseToSpawn(0.5, 3)
                        ),
                        new Shoot(10, count: 11, shootAngle: 14, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(3000, "ZGoBackHome2")
                        ),
                    new State("ZGoBackHome2",
                        new HpLessTransition(0.05, "Dead2"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(speed: 0.6),
                        new TimedTransition(3600, "Zfight3")
                        ),
                    new State("Zfight3",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 3),
                            new Wander(0.05),
                            new Follow(0.5, 8, 1)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invincible, false, 3000),
                        new Shoot(10, count: 7, shootAngle: 20, projectileIndex: 0, predictive: 0.2, coolDown: 2000),
                        new Shoot(10, count: 14, projectileIndex: 1, coolDown: 3000),
                        new TimedTransition(6000, "ZGoBackHome1")
                        ),
                    new State("ZGoBackHome1",
                        new HpLessTransition(0.05, "Dead2"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(speed: 0.6),
                        new TimedTransition(3600, "Zchargingshock")
                        ),
                    new State("Zchargingshock",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 2),
                            new Wander(0.05)
                        ),
                        new Flash(0xffffff, 1, 2),
                        new Shoot(10, count: 9, projectileIndex: 2, coolDown: 1000, predictive: 0.2, coolDownOffset: 1000),
                        new Shoot(10, count: 11, predictive: 0.2, projectileIndex: 1, coolDown: 2500),
                        new Shoot(10, count: 5, projectileIndex: 3, coolDown: 400),
                        new TimedTransition(5000, "Zfollowabit2")
                        ),
                    new State("Zfollowabit2",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new Follow(0.2, 8, 1),
                            new StayCloseToSpawn(0.5, 3)
                        ),
                        new Shoot(10, count: 12, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(4000, "Zrumble1")
                        ),
                    new State("Zrumble1",
                        new HpLessTransition(0.05, "Dead2"),
                        new Prioritize(
                            new StayCloseToSpawn(0.2, 2),
                            new Follow(0.5, 8, 1),
                            new Wander(0.05)
                        ),
                        new Shoot(10, count: 12, shootAngle: 7, predictive: 0.2, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                        new Shoot(10, count: 8, shootAngle: 16, predictive: 0.2, projectileIndex: 2, coolDown: new Cooldown(500, 3000)),
                        new TimedTransition(7400, "Zfight1")
                          ),
                    new State("Dead2",
                        new Taunt("No... NOO..."),
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new TimedTransition(2000, "Dead3")
                                ),
                    new State("Dead3",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("This was recorded, You guys will be killed once back up gets here! We didn't die for nothing!"),
                         new TimedTransition(2000, "Dead4")
                          ),
                    new State("Dead4",
                          new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                          new Suicide()
                        )
                      )

                    ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Defense", 1.0),
                    new ItemLoot("Greater Potion of Dexterity", 1.0),
                    new ItemLoot("Potion of Mana", 0.6),
                    new TierLoot(2, ItemType.Potion, numRequired: 2),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Powered Reactor", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Atomic Battery Module", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Energy Converter Module", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Engine Cooling Module", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Basic Alien Tech", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Scrapped Ship Plates", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Destructor AI Module", 0.0045, damagebased: true, threshold: 0.01)
                    )
            )


            ;
    }
}