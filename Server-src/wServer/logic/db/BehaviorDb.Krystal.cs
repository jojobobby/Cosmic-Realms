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
        private _ Krystal = () => Behav()


          .Init("Krystaline",
                new State(
                    new State(
                         new DropPortalOnDeath(target: "Crystal Cave Portal"),
                       new ScaleHP2(85, 2, 15),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                   new State("wait",
                        new Taunt(true, "Mere Mortals, Prepare yourselves."),
                        new PlayerWithinTransition(8, "taunt")
                        ),
                   new State("taunt",
                        new Taunt("You have underestimate me."),
                        new TimedTransition(3000, "fight1")
                        )
                    ),
                    new State(
                        new HpLessTransition(0.4, "rage"),
                    new State("fight1",
                        new Grenade(4, 120, range: 12, coolDown: 2000, effect: ConditionEffectIndex.ArmorBroken, effectDuration: 3000, color: 0x00FFFF),
                        new Shoot(10, 12, projectileIndex: 5, coolDown: 2000),
                        new TimedTransition(8000, "charging"),
                        new State("Duoforce1",
                            new Shoot(0, projectileIndex: 2, count: 3, shootAngle: 120, fixedAngle: 0, rotateAngle: 15, coolDown: 75)
                       )
                    ),
                   new State("charging",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0x0000FF, 0.25, 4),
                        new TimedTransition(2000, "fight2")
                        ),
                    new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Solid),
                        new HealSelf(coolDown: 5000, amount: 500),
                        new Shoot(10, 3, shootAngle: 8, projectileIndex: 1, predictive: 0.5, coolDown: 800),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 2000),
                        new Shoot(10, 8, projectileIndex: 2, coolDown: 2000),
                        new TimedTransition(6000, "force1")
                        ),
                    new State(
                        new HealSelf(coolDown: 2500, amount: 250),
                        new Prioritize(
                            new Follow(0.75, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 2000),
                        new Shoot(10, 3, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new TimedTransition(8000, "return"),
                    new State("force1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(600, "force2")
                            ),
                    new State("force2",
                            new TimedTransition(600, "force1")
                            )
                        ),
                    new State("return",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new ReturnToSpawn(3),
                          new TimedTransition(7000, "fight1")
                        )
                     ),
                  new State("rage",
                        new Taunt("You damn!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(2),
                        new Flash(0x00FFFF, 0.25, 4),
                        new TimedTransition(3000, "toon")
                        ),
                     new State(
                         new Flash(0xFF00FF, 0.25, 4),
                        new Taunt("The reflection of the sun heals my body!"),
                        new HealSelf(coolDown: 1750, amount: 250),
                        new Prioritize(
                            new StayCloseToSpawn(0.5, 10),
                            new Charge(1, range: 4, coolDown: 1000),
                            new Follow(1, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 1000),
                        new Shoot(10, 6, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new Shoot(10, 1, predictive: 0.5, projectileIndex: 1, coolDown: 200),
                    new State("toon",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(600, "rageb")
                            ),
                    new State("rageb",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                            new Shoot(10, 8, projectileIndex: 0, coolDown: 600),
                            new TimedTransition(600, "toon")
                            )
                        )
                    ),
                new Threshold(0.000025,
                    new ItemLoot("Greater Potion of Dexterity", 0.33),
                    new ItemLoot("Potion of Attack", 0.33),
                    new ItemLoot(item: "Wisp of Potential", probability: 0.2),
                    new ItemLoot("Crystalized Flame Trap", 0.006, damagebased: true),
                    new ItemLoot("Seal of Gleaming Hope", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Jade", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Amethyst", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Topaz", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot(item: "Potion of Defense", probability: 0.5),
                    new ItemLoot(item: "Potion of Attack", probability: 0.4),
                    new ItemLoot("Fragment of the Earth", 0.01)
                )
            )
        /*
          .Init("Krystaline",
                      new State(
                          new DropPortalOnDeath(target: "Crystal Cave Portal"),
                          new ScaleHP2(50,3,15),
                          new State("Wait",
                              new ConditionalEffect(ConditionEffectIndex.Invincible),
                              new TimedTransition(20, "RemINVINC")
                          ),
                          new State("RemINVINC",
                              new Flash(0xffffff, 2, 100),
                              new ConditionalEffect(ConditionEffectIndex.Invincible),
                              new TimedTransition(2000, "Shotgun")
                          ),
                          new State("FlashRING",
                              new Flash(0xd40000, 2, 100),
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                              new TimedTransition(2000, "RingCharge"),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("RingCharge",
                              new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                              new TimedTransition(800, "ChooseRandom"),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("Shotgun",
                              new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                              new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 1000, projectileIndex: 4),
                              new TimedTransition(3700, "ChooseRandom"),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("Singular",
                              new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 1, predictive: 1, coolDown: 90, projectileIndex: 0),
                              new TimedTransition(1800, "ChooseRandom"),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("PetRing",
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 400, projectileIndex: 4),
                              new TimedTransition(120, "ChooseRandom"),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("Spawn",
                              new TimedTransition(700, "ChooseRandom")
                          ),
                          new State("ChooseRandom",
                              new Flash(0xffffff, 2, 100),
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                              new TimedTransition(1200, "Singular", true),
                              new TimedTransition(1200, "FlashRING", true),
                              new TimedTransition(1200, "Shotgun", true),
                              new TimedTransition(1200, "PetRing", true),
                              new TimedTransition(1200, "Spawn", true),
                              new EntitiesNotExistsTransition(50, "JadeDied", "Jade Statue")
                          ),
                          new State("JadeDied",
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                              new ChangeSize(20, 130),
                              new Flash(0xffffff, 2, 100),
                              new TimedTransition(1500, "ChooseRandomV2")
                          ),
                          new State("ChooseRandomV2",
                              new Flash(0xffffff, 2, 100),
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                              new TimedTransition(1200, "SingularV2", true),
                              new TimedTransition(1200, "FlashRINGV2", true),
                              new TimedTransition(1200, "ShotgunV2", true),
                              new TimedTransition(1200, "PetRingV2", true),
                              new TimedTransition(1200, "ShotgunWIDER", true),
                              new TimedTransition(1200, "Swirl", true),
                              new TimedTransition(1200, "SpawnV2", true)
                          ),
                          new State("SpawnV2",
                              new TimedTransition(700, "ChooseRandomV2")
                          ),
                          new State("FlashRINGV2",
                              new Flash(0xd40000, 2, 100),
                              new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                              new TimedTransition(2000, "RingChargeV2")
                          ),
                          new State("RingChargeV2",
                              new Follow(1.8, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 5),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 220),
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 420), //smonk :3
                              new Shoot(10, count: 20, shootAngle: 18, coolDown: 99999, projectileIndex: 1, coolDownOffset: 620),
                              new TimedTransition(800, "ChooseRandomV2")
                          ),
                          new State("ShotgunV2",
                              new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 500, projectileIndex: 0),
                              new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 580, projectileIndex: 3),
                              new TimedTransition(2300, "ChooseRandomV2")
                          ),
                          new State("ShotgunWIDER",
                              new Shoot(10, count: 5, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 50),
                              new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 50),
                              
                              new Shoot(10, count: 10, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 500),
                              new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 500),
                              
                              new Shoot(10, count: 15, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 0, coolDownOffset: 1000),
                              new Shoot(10, count: 1, predictive: 1, shootAngle: 5, coolDown: 99999, projectileIndex: 3, coolDownOffset: 1000),
                              new TimedTransition(1200, "ChooseRandomV2")
                          ),
                          new State("SingularV2",
                              new Follow(.7, range: 1, duration: 5000, coolDown: 0),
                              new Shoot(10, count: 1, predictive: 1, coolDown: 90, projectileIndex: 0),
                              new TimedTransition(2500, "ChooseRandomV2")
                          ),
                          new State("PetRingV2",
                              new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 0, coolDown: 9400, projectileIndex: 4),
                              new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 10, coolDown: 9400, projectileIndex: 4, coolDownOffset: 100),
                              new Shoot(10, count: 20, shootAngle: 18, fixedAngle: 20, coolDown: 9400, projectileIndex: 4, coolDownOffset: 300),
                              new TimedTransition(120, "ChooseRandomV2")
                          ),
                          new State("Swirl",
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 0, projectileIndex: 4, coolDownOffset: 50),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 90, projectileIndex: 4, coolDownOffset: 200),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 180, projectileIndex: 4, coolDownOffset: 400),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 270, projectileIndex: 4, coolDownOffset: 600),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 45, projectileIndex: 4, coolDownOffset: 800),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 135, projectileIndex: 4, coolDownOffset: 1000),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 225, projectileIndex: 4, coolDownOffset: 1200),
                              new Shoot(10, count: 2, shootAngle: 20, coolDown: 99999, fixedAngle: 315, projectileIndex: 4, coolDownOffset: 1400),
                              new TimedTransition(1500, "ChooseRandomV2")
                          )
                      ),
                     new Threshold(.01,
                         new ItemLoot(item: "Wisp of Potential", probability: 0.2),
                         new ItemLoot(item: "Potion of Attack", probability: 0.4),
                         new ItemLoot("Crystalized Flame Trap", 0.002, damagebased: true, threshold: 0.01),
                         new ItemLoot("Seal of Gleaming Hope", 0.006, damagebased: true, threshold: 0.01),
                         new ItemLoot("Jade", 0.007, damagebased: true, threshold: 0.01),
                         new ItemLoot("Amethyst", 0.007, damagebased: true, threshold: 0.01),
                         new ItemLoot("Topaz", 0.007, damagebased: true, threshold: 0.01),
                         new ItemLoot(item: "Potion of Defense", probability: 0.5),
                         new ItemLoot(item: "Potion of Attack", probability: 0.4),
                         new ItemLoot("Fragment of the Earth", 0.01),
                         new ItemLoot(item: "Potion of Speed", probability: 1.3),
                         new ItemLoot(item: "Potion of Dexterity", probability: 1.3)
                      )
                  )*/
                  ;
    }
}