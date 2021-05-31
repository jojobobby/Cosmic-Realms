using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by Tidan
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ WineCellar = () => Behav()
              .Init("Tidale, The Defender of the Ancients",
                new State(
                    new ScaleHP2(60,3,15),
                    new State("Attack",
                        new Wander(.05),
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
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!"),
                        new Spawn("Henchman of Oryx", 5, coolDown: 5000),
                        new HpLessTransition(.2, "prepareRage")
                    ),
                    new State("prepareRage",
                        new Follow(.1, 15, 3),
                        new Taunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, 30, fixedAngle: 0, projectileIndex: 7, coolDown: 4000, coolDownOffset: 4000),
                        new Shoot(25, 30, fixedAngle: 30, projectileIndex: 8, coolDown: 4000, coolDownOffset: 4000),
                        new TimedTransition(10000, "rage")
                    ),
                    new State("rage",
                        new Follow(.1, 15, 3),
                        new Shoot(25, 30, projectileIndex: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, projectileIndex: 8, coolDown: 90000001, coolDownOffset: 8500),
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
                        new TossObject("Monstrosity Scarab", 7, 0, coolDown: 1000),
                        new Taunt(1, 6000, "Puny mortals! My {HP} HP will annihilate you!")
                    )
                ),
               new Threshold(0.00001,
                    new ItemLoot("Potion of Vitality", 1.51),
                    new ItemLoot("Potion of Vitality", 0.51),
                    new ItemLoot("Mark of Tidale", 1),
                    new TierLoot(7, ItemType.Ring, 0.010),
                    new TierLoot(6, ItemType.Ring, 0.026),
                    new TierLoot(10, ItemType.Weapon, 0.47),
                    new TierLoot(11, ItemType.Weapon, 0.36),
                    new TierLoot(12, ItemType.Weapon, 0.25),
                    new TierLoot(13, ItemType.Weapon, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.27),
                    new TierLoot(6, ItemType.Ability, 0.15),
                    new TierLoot(11, ItemType.Armor, 0.37),
                    new TierLoot(12, ItemType.Armor, 0.26),
                    new TierLoot(13, ItemType.Armor, 0.15),
                    new TierLoot(5, ItemType.Ring, 0.36),
                    new ItemLoot("Potion of Attack", 1.51),
                    new ItemLoot("Potion of Defense", 1.51),
                    new ItemLoot("Potion of Wisdom", 0.51),
                        //ST
                        new ItemLoot("Fragment of the Earth", 0.01),
                    new ItemLoot("Greater Potion of Critical Chance", 1),
                    new ItemLoot("Potion of Critical Damage", 1),
                    new ItemLoot("Standard Chest", 1),
                    new ItemLoot("KnightST0", 0.006, damagebased: true),
                    new ItemLoot("KnightST1", 0.006, damagebased: true),
                    new ItemLoot("KnightST2", 0.006, damagebased: true),
                    new ItemLoot("KnightST3", 0.006, damagebased: true),
                    //materials
                    new ItemLoot("Ring of Decades", 0.05, damagebased: true),
                    new ItemLoot("Scraps of the Descendant", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Scraps of the Descendant", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Cyberious Infused Shard", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Cloak of Wild Shadows", 0.001, damagebased: true, threshold: 0.01)

                )
            )





         .Init("Oryx the Mad God 2 - 3",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TransformOnDeath("Oryx the Mad God 3"),
                      new State("Oryx32",
                        new Flash(0xf389E13, 2.5, 50),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(1000, "Oryx3")
                          ),
                    new State("Oryx3",
                        new Follow(.1, 2.5, 3),
                        new Flash(0xf389E13, 2.5, 50),
                        new Taunt("ARGHHH!!! MY LIFE IS FULL OF LIES!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "oryx32")
                           ),
                    new State("oryx32",
                        new Follow(.1, 15, 3),
                        new Flash(0xf389E13, 2.5, 50),
                        new TossObject("Oryx Void Mimic Entity", 3, 120, coolDown: 99999),
                        new Taunt("VOID ENTITY WHERE ARE YOU, I DEMAND YOUR ASSISTANCE!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                         ),
                     new State("OryxandVoid",
                        new Follow(.1, 15, 3),
                        new Flash(0xf389E13, 1, 5),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("THERE IS NO TIME FOR ARGUING, IF YOU DONT WANT TO DIE YOU WILL GRANT ME MORE POWER!"),
                        new TimedTransition(1500, "12")
                           ),
                        new State("12",
                        new Order(100, "Oryx Void Mimic Entity", "Continue1"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(100, "0")
                               ),
                        new State("0",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xf389E13, 1, 5)
                              ),
                     new State("OryxandVoid2",
                        new Follow(.1, 15, 3),
                        new Flash(0xf389E13, 1, 5),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("THERE IS NO TIME FOR A COST, I DEMAND POWER NOW OR YOU'RE DYING WITH ME!"),
                         new TimedTransition(1500, "2")
                           ),
                        new State("2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(100, "Oryx Void Mimic Entity", "Continue2"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new TimedTransition(0, "0")
                            ),
                     new State("OryxandVoid3",
                        new Flash(0xf389E13, 1, 200),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("MY BODY FEELS NUMB! FINALLY THE POWER HAS COME TO ME!"),
                        new Flash(0xf389E13, 2.5, 50),
                        new TimedTransition(2000, "oryx3spawn")
                    ),
                     new State("oryx3spawn",
                         new Suicide()

                    )
                )
            )



           .Init("Oryx Void Mimic Entity",
                      new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("pathetic",
                        new Orbit(0.5, 3, target: "Oryx the Mad God 2 - 3", radiusVariance: 0.5),
                        new Taunt("We meet again, Oryx... the ruler of the realm asking for help from a prisoner?"),
                        new TimedTransition(1500, "a1")
                          ),
                        new State("a1",
                        new Order(100, "Oryx the Mad God 2 - 3", "OryxandVoid"),
                        new Orbit(1, 2, target: "Oryx the Mad God 2 - 3", radiusVariance: 0.5),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(100, "0")
                       ),
                        new State("0",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xf389E13, 1, 5)
                              ),
                        new State("Continue1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Orbit(1, 2, target: "Oryx the Mad God 2 - 3", radiusVariance: 0.5),
                        new Taunt("I shall grant you some of the voids power but at a cost!"),
                        new TimedTransition(1500, "a2")
                     ),
                        new State("a2",
                        new Order(100, "Oryx the Mad God 2 - 3", "OryxandVoid2"),
                        new Orbit(1, 2, target: "Oryx the Mad God 2 - 3", radiusVariance: 0.5),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(100, "0")
                    ),
                        new State("Continue2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Orbit(3, 1, target: "Oryx the Mad God 2 - 3", radiusVariance: 0.5),
                        new Taunt("If u insist..."),
                        new TimedTransition(1500, "a3")
                          ),
                        new State("a3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Order(100, "Oryx the Mad God 2 - 3", "OryxandVoid3"),
                        new TimedTransition(1500, "Finished")
                             ),
                        new State("Finished",
                            new Suicide()
                                )
            )
                    )

















        //ghostmaree


            .Init("Henchman of Oryx",
                new State(
                    new Prioritize(
                        new Protect(0.4, "Tidale, The Defender of the Ancients", 10, 10, 1),
                        new Protect(0.4, "Oryx the Mad God Deux", 10, 10, 1),
                        new Wander(.2),
                        new Follow(.2, 8, 3, coolDown: 0)
                        ),
                    new Spawn("Abomination of Oryx", 1, 0),
                    new Spawn("Aberrant of Oryx", 1, 0),
                    new Spawn("Monstrosity of Oryx", 1, 0),
                    new Spawn("Vintner of Oryx", 1, 0),
                    new Spawn("Bile of Oryx", 1, 0),
                    new Shoot(8, projectileIndex: 0, predictive: 1, coolDown: 1500),
                    new Shoot(8, projectileIndex: 1, count: 3, shootAngle: 20, coolDown: 1500, coolDownOffset: 1000)
                    )
            )
            .Init("Monstrosity Scarab",
                new State(
                    new State("searching",
                        new Prioritize(
                            new Follow(2, range: 0)
                            ),
                        new PlayerWithinTransition(2, "creeping"),
                        new TimedTransition(5000, "creeping")
                        ),
                    new State("creeping",
                        new Shoot(2, 10, 36, fixedAngle: 0),
                        new Decay(0)
                        )
                    )
            )
            .Init("Bile of Oryx",
                new State(
                    new Wander(.1),
                    new Protect(.7, "Henchman of Oryx", 15, 5)
                    )
            )
            .Init("Monstrosity of Oryx",
                new State(
                    new Wander(.4),
                    new TossObject("Monstrosity Scarab", 8, coolDown: 6000, throwEffect: true)
                    )
            )
            .Init("Abomination of Oryx",
                new State(
                    new Wander(.3),
                    new Charge(.9, 6, 2000),
                    new Shoot(8, 5, 5, 0, coolDown: 4000),
                    new Shoot(8, 5, 5, 1, coolDown: 4000),
                    new Shoot(8, 5, 5, 2, coolDown: 4000),
                    new Shoot(8, 5, 5, 3, coolDown: 4000),
                    new Shoot(8, 5, 5, 4, coolDown: 4000)
                    )
            )
            .Init("Aberrant of Oryx",
                new State(
                    new Wander(.3),
                    new Protect(.7, "Henchman of Oryx", 15, 5),
                    new TossObject("Aberrant Blaster", 8, coolDown: 6000, throwEffect: true)
                    )
            )
            .Init("Aberrant Blaster",
                new State(
                    new State("Search_Player",
                        new PlayerWithinTransition(5, "HeSeeYou")
                        ),
                    new State("HeSeeYou",
                        new Shoot(10, 5, 10, 0),
                        new Suicide()
                        )
                    )
            )
            .Init("Vintner of Oryx",
                new State(
                    new StayBack(0.6, 4),
                    new Wander(.3),
                    new Shoot(8, 1, projectileIndex: 0, coolDown: 1500)
                    )
            )
        .Init("Cyberious Portal Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Castle of Cyberious Portal", 100),
                    new State("Idle",
                        new EntitiesNotExistsTransition(300, "Suicide", "Tidale, The Defender of the Ancients")
                        ),
                    new State("Suicide",
                        new Suicide()
                        )
                    )
            )
        ;
    }
}