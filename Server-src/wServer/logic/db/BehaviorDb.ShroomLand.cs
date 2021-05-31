using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//from LOE-V6, bit chages by ghostmaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ShroomLand = () => Behav()
              .Init("King Toadstool",
                new State(
                   // new DropPortalOnDeath("Shroom Island Portal", 100, timeout: 180),
                   new ScaleHP2(80,3,15),
                    new HpLessTransition(0.14, "spookded"),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(10, "talk1", "Activation Shroom")
                        ),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("talk1",
                        new Taunt("Revealing the magic of the Shrooms is a sin.", "You've released a great evil upon yourself."),
                        new TimedTransition(3300, "talk2")
                        ),
                    new State("talk2",
                        new Taunt("I will not be soft on you, The magic of the mushrooms power me to even greater heights."),
                        new TimedTransition(3300, "talk3")
                        ),
                    new State("talk3",
                        new Taunt("You are no match for me."),
                        new TimedTransition(3300, "talk4")
                        ),
                    new State("talk4",
                        new Taunt("Perish!"),
                        new TimedTransition(3300, "go")
                        )
                      ),
                    new State("go",
                        new Prioritize(
                            new Follow(0.8, 8, 1),
                            new Wander(1)
                            ),
                        new Shoot(8, count: 14, shootAngle: 20, projectileIndex: 4, coolDown: 600),
                        new Shoot(8, count: 3, shootAngle: 10, projectileIndex: 1, predictive: 2, coolDown: 1600),
                        new TimedTransition(9000, "go2")
                        ),
                    new State("go2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new StayBack(0.4, 4),
                            new Swirl(1, 10)
                            ),
                        new Shoot(12, count: 6, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 200),
                        new Shoot(8, count: 10, projectileIndex: 2, coolDown: new Cooldown(2000, 1000)),
                        new TimedTransition(9000, "spook")
                        ),
                    new State("spook",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(1),
                        new TimedTransition(3000, "go3")
                        ),
                    new State("go3",
                        new Taunt("My minions will fight with me until the end!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Shroomite", 6, 0, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 45, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 90, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 135, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 180, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 225, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 270, coolDown: 9999999),
                        new TossObject("Shroomite", 6, 315, coolDown: 9999999),
                        new TossObject("Shroomite", 9, 0, coolDown: 9999999),
                        new TossObject("Shroomite", 9, 90, coolDown: 9999999),
                        new TossObject("Shroomite", 9, 180, coolDown: 9999999),
                        new TossObject("Shroomite", 9, 270, coolDown: 9999999),
                        new TimedTransition(3200, "checkforbombers")
                        ),
                    new State("checkforbombers",
                        new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 270, coolDown: 3000),
                        new Shoot(10, count: 9, shootAngle: 8, projectileIndex: 3, coolDownOffset: 1100, angleOffset: 90, coolDown: 3000),
                        new Shoot(12, count: 5, shootAngle: 12, projectileIndex: 4, coolDown: 1000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(9999, "tauntu", "Shroomite")
                        ),
                    new State("tauntu",
                        new Taunt("Your magic belongs to me."),
                        new TimedTransition(4000, "swag21")
                        ),
                    new State("swag21",
                        new Flash(0x0F0F0F, 2, 2),
                        new Grenade(4, 300, coolDown: 3000),
                        new Shoot(12, count: 10, shootAngle: 4, predictive: 1, projectileIndex: 0, coolDown: 200),
                        new Prioritize(
                            new Charge(2, 10, coolDown: 4000),
                            new Wander(0.2)
                            ),
                        new Shoot(12, count: 12, projectileIndex: 2, coolDown: 4000),
                        new TimedTransition(12000, "basic2")
                        ),
                    new State("basic2",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Follow(0.6),
                            new Wander(0.2)
                            ),
                        new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                        new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                        new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                        new TimedTransition(5000, "basic3")
                        ),
                    new State("basic3",
                        new Wander(0.4),
                        new Shoot(8, count: 10, projectileIndex: 4, coolDown: 1000),
                        new Shoot(12, count: 6, shootAngle: 8, projectileIndex: 2, coolDown: 2200),
                        new TimedTransition(5000, "cryofsin")
                        ),
                    new State("cryofsin",
                        new Taunt(true, "...!", "!!!!"),
                        new Flash(0xFF0000, 1, 2),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(1),
                        new TimedTransition(4400, "cry")
                        ),
                    new State("cry",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Al lar kall zanus du era!", "Rul ah ka tera nol zan!"),
                        new Shoot(30, count: 34, projectileIndex: 0, coolDown: 1000),
                        new TimedTransition(4000, "gofirst")
                        ),
                    new State("gofirst",
                        new HealSelf(coolDown: 1000, amount: 10000),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Follow(1.2),
                            new Wander(0.2)
                            ),
                        new Shoot(8, count: 4, projectileIndex: 4, coolDown: 400),
                        new Shoot(8, count: 6, projectileIndex: 4, coolDown: 1400),
                        new Shoot(12, count: 18, projectileIndex: 0, coolDown: 2500),
                        new TimedTransition(5000, "go")
                        ),
                    new State("spookded",
                        new Flash(0x00FF00, 1, 3),
                        new Taunt("Death awaits if you dare to enter the island!"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ReturnToSpawn(1),
                        new TimedTransition(6000, "ded")
                        ),
                    new State("ded",
                        new Suicide()
                        )
                    ),
                    new Threshold(0.00001,
                       new ItemLoot("Reforged Shroom bracelet", 0.006, damagebased: true),
                       new ItemLoot("Ancient Burgeon Shroom Armor", 0.004, damagebased: true),
                       new ItemLoot("Super Magic Mushroom", 0.0045, damagebased: true, threshold: 0.01),
                       new ItemLoot("Magic Tome of Mush", 0.004, damagebased: true),
                       new ItemLoot("Spell Scroll", 0.0045, damagebased: true, threshold: 0.01),
                       new ItemLoot("Greater Potion of Attack", 1),
                       new ItemLoot("Shield of Crystallized Magic", 0.001, damagebased: true, threshold: 0.01),
                       new ItemLoot("Potion of Critical Chance", 0.02),
                       new ItemLoot("Earth Shard", 0.01),
                       new ItemLoot("Potion of Critical Damage", 0.02),
                       new ItemLoot("Potion of Wisdom", 0.8),
                       new ItemLoot("Potion of Dexterity", 0.8),
                       new ItemLoot("Potion of Speed", 0.8),
                       new TierLoot(2, ItemType.Potion)
                       )
            )

          .Init("Shroomite",
            new State(
                new State("fight1",
                    new Prioritize(
                        new Follow(0.4, 8, 1),
                        new Wander(1)
                        ),
                    new Shoot(10, count: 4, projectileIndex: 1, coolDown: 2000),
                     new HpLessTransition(0.15, "fight2")
                    ),
                new State(
                    new Follow(1, 8, 1),
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                new State("fight2",
                     new Flash(0xFF0000, 1, 3),
                     new PlayerWithinTransition(2, "ded")
                    ),
                new State("ded",
                     new Shoot(10, count: 12, projectileIndex: 0, coolDown: 9999),
                     new Suicide()
                        )
                    )
                )
            )

          .Init("Activation Shroom",
            new State(
                new State("1",
                    new Shoot(10, count: 8, projectileIndex: 0, coolDown: 2500),
                     new HpLessTransition(0.15, "2"),
                     new HealSelf(coolDown: 8000)
                    ),
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new State("2",
                     new Flash(0xFF00FF, 1, 1),
                     new TimedTransition(1600, "3")
                    ),
                new State("3",
                     new Shoot(10, count: 12, projectileIndex: 0, coolDown: 9999),
                     new Suicide()
                        )
                    )
                )
            )
            ;
    }
}