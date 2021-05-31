

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ LightningDragon = () => Behav()
        .Init("Raijin, the Lightning Guardian",
                new State(
                   new ScaleHP2(85, 2, 15),
                    new DropPortalOnDeath("Lightning Cellar Portal", 100, 40),
                    new HpLessTransition(0.18, "Dead1"),
                    new State("swag",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),

                        new PlayerWithinTransition(8, "Waiting")
                        ),
                    new State("Waiting",
                        new Flash(0x00FF00, 1, 2),
                        new Transform("Legendary Dragon Kakashi"),
                        new Taunt("!!!"),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(8000, "Sentry12")
                        ),
                    new State(
                        new Shoot(8.4, count: 8, projectileIndex: 3, coolDown: 100),
                    new State("Sentry12",
                        new Flash(0x00FFFF, 1, 2),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(8.4, count: 6, shootAngle: 10, projectileIndex: 0, coolDown: 2000),
                        new Shoot(8.4, count: 3, shootAngle: 10, projectileIndex: 0, predictive: 2, coolDown: 600, coolDownOffset: 3000),
                        new Shoot(8.4, count: 1, projectileIndex: 1, coolDown: 1000),
                        new TimedTransition(8000, "Sentry2")
                        ),
                    new State("Sentry2",
                        new Shoot(8.4, count: 10, projectileIndex: 1, coolDown: 3000),
                        new Shoot(8.4, count: 10, shootAngle: 30, projectileIndex: 0, predictive: 1, coolDown: 1000, coolDownOffset: 2000),
                        new TimedTransition(8000, "Sentry3")
                        ),
                    new State("Sentry3",
                        new Flash(0x00FFFF, 1, 2),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(8.4, count: 18, projectileIndex: 0, coolDown: 3000),
                        new Shoot(8.4, count: 3, shootAngle: 5, projectileIndex: 2, coolDown: 600),
                        new TimedTransition(8000, "Sentry4")
                        ),
                    new State("Sentry4",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Grenade(3, 180, range: 7),
                        new Shoot(8.4, count: 12, projectileIndex: 2, coolDown: 2000),
                        new Shoot(8.4, count: 3, shootAngle: 16, projectileIndex: 2, coolDown: 600),
                        new TimedTransition(8000, "Sentry5")
                        ),
                    new State("Sentry5",
                        new Flash(0x00FFFF, 1, 2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(8.4, count: 10, projectileIndex: 0, coolDown: 2000),
                        new Shoot(8.4, count: 1, projectileIndex: 1, coolDown: 600),
                        new TimedTransition(5000, "spiral1")
                        ),
                     new State("spiral1",
                         new Flash(0x00FFFF, 1, 2),
                         new ConditionalEffect(ConditionEffectIndex.Armored),
                         new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(8.4, count: 2, shootAngle: 60, projectileIndex: 1, coolDown: 600),
                        new TimedTransition(7000, "Sentry12"),
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(75, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(75, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(75, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(75, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(75, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(75, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(75, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 4, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(75, "Quadforce1")
                        )
                    )
                   ),
                    new State("Dead1",
                        new Taunt("Finally, rest."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0x0000FF, 0.2, 3),
                        new TimedTransition(3250, "dead")
                        ),
                    new State("dead",
                        new Transform("Legendary Dragon Kakashi"),
                        new Suicide()
                        )
                    ),
                new Threshold(0.00001,
                    new ItemLoot("Potion of Vitality", 1.00),//Lightning Wand
                    new ItemLoot("Potion of Attack", 0.80),
                    new ItemLoot("Potion of Attack", 0.80),
                    new TierLoot(8, ItemType.Weapon, 0.3),
                    new TierLoot(9, ItemType.Weapon, 0.275),
                    new TierLoot(10, ItemType.Weapon, 0.225),
                    new TierLoot(11, ItemType.Weapon, 0.08),
                    new TierLoot(8, ItemType.Armor, 0.2),
                    new TierLoot(9, ItemType.Armor, 0.175),
                    new TierLoot(10, ItemType.Armor, 0.15),
                    new TierLoot(11, ItemType.Armor, 0.1),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(4, ItemType.Ability, 0.15),
                    new TierLoot(5, ItemType.Ability, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new ItemLoot("Potion of Critical Chance", 1),
                    new ItemLoot("Potion of Critical Damage", 1),
                    new ItemLoot("Earth Shard", 0.01),
                    new ItemLoot("Dragon Scale Armor", 0.006, damagebased: true),
                    new ItemLoot("Robe of the Lightning Apprentice", 0.006, damagebased: true),
                    new ItemLoot("Lightning Chamber", 0.001, damagebased: true, threshold: 0.01),//Robe of the Lightning Apprentice
                    new ItemLoot("Conductive Shield", 0.006, damagebased: true),//Lightning Chamber
                    new ItemLoot("Lightning Wand", 0.001, damagebased: true, threshold: 0.01)//Conductive Shield
                )
            )

          .Init("Legendary Dragon Kakashi",
                new State(
                    new TransformOnDeath("Golden Dragon Kakashi", 1, 1, 100),
                    new ScaleHP2(75, 2, 15),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                   new State("wait",
                        new PlayerWithinTransition(8, "taunt")
                        ),
                   new State("taunt",
                        new Taunt( "Life must come easy for you, therefore death must come even faster!"),
                        new TimedTransition(4000, "begin")
                        )
                    ),
                    new State(//new SetAltTexture(2),
                        
                         new HpLessTransition(0.20, "rage"),
                    new State("begin",
                        new Taunt("My rage builds up as we fight, so lets begin!", "You shall see how powerful the thrunder dragon is!", "Lets continue, shall we?", "This power is nothing to be played with!", "You stupid mortals, you fail to understand what you're messing with."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "fight1")
                        ),
                    new State("fight1",
                        new Grenade(4, 80, range: 12, coolDown: 2000),
                        new Shoot(10, 12, projectileIndex: 2, coolDown: 2000, predictive: 0.5),
                        new TimedTransition(8000, "charging"),
                        new State("Duoforce1",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(75, "Duoforce2")
                        ),
                        new State("Duoforce2",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(75, "Duoforce3")
                        ),
                        new State("Duoforce3",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(75, "Duoforce4")
                        ),
                        new State("Duoforce4",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(75, "Duoforce21")
                        ),
                         new State("Duoforce21",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(75, "Duoforce23")
                        ),
                         new State("Duoforce23",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(75, "Duoforce25")
                        ),
                         new State("Duoforce25",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(75, "Duoforce26")
                        ),
                         new State("Duoforce26",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(75, "Duoforce24")
                        ),
                         new State("Duoforce24",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(75, "Duoforce22")
                        ),
                          new State("Duoforce22",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(75, "Duoforce5")
                        ),
                        new State("Duoforce5",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(75, "Duoforce6")
                        ),
                        new State("Duoforce6",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(75, "Duoforce7")
                        ),
                        new State("Duoforce7",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(75, "Duoforce8")
                        ),
                        new State("Duoforce8",
                            new Shoot(0, projectileIndex: 1, count: 3, shootAngle: 120, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(75, "Duoforce1")
                      )
                    ),
                   new State("charging",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Flash(0x0000FF, 0.25, 4),
                        new Shoot(25, projectileIndex: 0, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 1, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 2, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 5, shootAngle: 10, predictive: 0.6, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 0, count: 3, shootAngle: 10, predictive: 0.1, coolDown: 1000, coolDownOffset: 1000),
                        new TimedTransition(8000, "fight2")
                        ),
                     new State("fight2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, 3, shootAngle: 8, projectileIndex: 1, predictive: 0.5, coolDown: 800),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 0.7, coolDown: 2000),
                        new Shoot(10, 8, projectileIndex: 2, coolDown: 2000),
                        new TimedTransition(6000, "force1")
                        ),
                    new State(
                        new Prioritize(
                            new Follow(0.75, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 0.7, coolDown: 2000),
                        new Shoot(10, 3, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new TimedTransition(8000, "return"),
                    new State("force1",
                         new Shoot(10, count: 4, projectileIndex: 0, coolDown: 4000),
                         new Shoot(10, count: 7, shootAngle: 18, projectileIndex: 1, coolDown: 1000),
                         new Grenade(2, 160, range: 8, coolDown: 600),
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new TimedTransition(2600, "force2")
                            ),
                    new State("force2",
                         new Shoot(10, count: 4, projectileIndex: 0, coolDown: 4000),
                        new Shoot(10, count: 7, shootAngle: 18, projectileIndex: 1, coolDown: 1000),
                        new Grenade(2, 160, range: 8, coolDown: 600),
                            new TimedTransition(2600, "force1")
                            )
                        ),
                    new State("return",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new ReturnToSpawn(1),
                          new TimedTransition(5000, "begin")
                        )
                     ),
                    new State(
                         new HpLessTransition(0.05, "death"),
                    new State("rage",
                        new Taunt("YOU SHOULD HAVE NEVER DONE THIS!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new Flash(0x0000FF, 0.25, 4),
                        new TimedTransition(2000, "toon")
                        ),
                     new State(
                         new Flash(0xFF0000, 0.25, 4),
                        new Taunt("YOU'VE MADE A GRAVE MISTAKE!"),
                        new Prioritize(
                            new Charge(1, range: 4, coolDown: 1000),
                            new Follow(1, 8, 1),
                            new Wander(0.25)
                        ),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 1, coolDown: 1000),
                        new Shoot(10, 6, shootAngle: 12, projectileIndex: 2, coolDown: 800),
                        new Shoot(10, 1, predictive: 0.5, projectileIndex: 1, coolDown: 200),
                    new State("toon",
                            new ConditionalEffect(ConditionEffectIndex.Armored),
                            new TimedTransition(3200, "rageb")
                            ),
                    new State("rageb",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, 3, shootAngle: 8, projectileIndex: 1, predictive: 0.5, coolDown: 800),
                        new Shoot(10, 4, projectileIndex: 3, predictive: 0.7, coolDown: 2000),
                        new Shoot(10, 8, projectileIndex: 2, coolDown: 2000),
                        new Grenade(3, 160, range: 8, coolDown: 600),
                            new Shoot(10, 8, projectileIndex: 0, coolDown: 600),
                            new TimedTransition(3200, "toon")
                            )
                         )
                        ),
                      new State("death",
                        new Taunt("YOU!... AUGH!"),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(2),
                        new Flash(0x0000FF, 0.25, 4),
                        new TimedTransition(2000, "toon23")
                               ),
                      new State("toon23",
                        new Suicide()
                          )
                    ),
                new Threshold(0.001,
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Potion of Life", 0.8),
                    new ItemLoot("Dragon Scale Armor", 0.006, damagebased: true),
                    new ItemLoot("Robe of the Lightning Apprentice", 0.006, damagebased: true),
                    new ItemLoot("Lightning Chamber", 0.007, damagebased: true),//Robe of the Lightning Apprentice
                    new ItemLoot("Conductive Shield", 0.008, damagebased: true),//Lightning Chamber
                    new ItemLoot("Lightning Wand", 0.008, damagebased: true),//Conductive Shield
                    new ItemLoot("Potion of Dexterity", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(5, ItemType.Ability, 0.1),
                    new TierLoot(10, ItemType.Armor, 0.1),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new TierLoot(6, ItemType.Ring, 0.025)
                )
            )
           .Init("Golden Dragon Kakashi",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ScaleHP2(40, 3, 15),
                    new State("1",
                        new SetAltTexture(1),
                        new Taunt("STOP!!"),
                        new TimedTransition(750, "2")
                        ),
                        new State("2",
                        new SetAltTexture(2),
                        new Taunt("STOP THIS GOL.."),
                        new TimedTransition(750, "3")
                            ),
                        new State("3",
                        new SetAltTexture(3),
                        new TimedTransition(750, "4")
                                ),
                        new State("4",
                            new Taunt("..."),
                        new SetAltTexture(4)
                        )
                    )
            )
            ;
    }
}