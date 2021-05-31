using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EpicSnakePit = () => Behav()
            .Init("Defender of the Bee",
             new State(
                 new ScaleHP2(40, 3, 15),
                    new HpLessTransition(0.11, "Die"),
                    new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("default",
                        new PlayerWithinTransition(9, "taunt1")
                        
                        ),
                    new State("taunt1",
                        new Taunt("You have awoken me."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2500, "taunt2")
                        ),
                    new State("taunt2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("My long 1000 year slumber has offically ended!"),
                        new TimedTransition(2500, "Fight")
                     ),
                    new State("Fight",
                        new Prioritize(
                            new Follow(0.6, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(25, projectileIndex: 2, count: 2, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 3, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 8, shootAngle: 8, projectileIndex: 0, coolDown: new Cooldown(1200, 600)),
                        new Shoot(10, count: 12, projectileIndex: 2, coolDown: new Cooldown(3000, 200)),
                        new Grenade(3, 300, range: 8, coolDown: new Cooldown(4000, 2000)),
                        new TimedTransition(6500, "Fight2")
                        ),
                    new State("Fight2",
                        new Taunt("Behold, the power of an ancient god.", "We ancient gods have immense power, behold.", "You have little value for your own life, witness power!"),
                        new Prioritize(
                            new Follow(0.4, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(25, projectileIndex: 2, count: 2, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 3, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 4, shootAngle: 8, predictive: 1, projectileIndex: 0, coolDown: new Cooldown(1000, 200)),
                        new Shoot(10, count: 18, projectileIndex: 2, coolDown: new Cooldown(8000, 200)),
                        new Grenade(1, 100, range: 8, coolDown: new Cooldown(2000, 2000)),
                        new TimedTransition(7000, "Fight3")
                        ),
                    new State("Fight3",
                        new Flash(0x0000FF, 1, 1),
                        new Prioritize(
                            new StayBack(0.3, 2),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 8, shootAngle: 12, projectileIndex: 1, coolDown: new Cooldown(1000, 200)),
                        new Shoot(10, count: 4, shootAngle: 4, projectileIndex: 0, coolDown: 2000),
                        new TimedTransition(6000, "wanderandshoot")
                        ),
                    new State(
                    new State("wanderandshoot",
                        new Sequence(
                            new Timed(3000,
                                new Follow(0.4, 8, 1)
                                ),
                            new Timed(4000,
                                new Wander(0.6)
                                ),
                            new Timed(3000,
                                new Charge(1, range: 8, coolDown: 3000)
                                )
                        ),
                        new Shoot(25, projectileIndex: 2, count: 2, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(10, count: 1, projectileIndex: 3, coolDown: 100),
                        new Shoot(10, count: 6, shootAngle: 10, projectileIndex: 0, coolDown: new Cooldown(4000, 2000)),
                        new Shoot(10, count: 10, projectileIndex: 5, coolDown: 2000),
                        new Shoot(10, count: 3, shootAngle: 6, predictive: 0.5, projectileIndex: 3, coolDown: new Cooldown(2000, 2000)),
                        new TimedTransition(10000, "returntospawn")
                        )
                      ),
                    new State("returntospawn",
                        new ReturnToSpawn(speed: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3000, "stayinplace")
                        ),
                    new State(
                        new TimedTransition(18000, "chargerush"),
                    new State("stayinplace",
                        new Shoot(10, count: 6, shootAngle: 6, projectileIndex: 3, coolDown: 1000),
                        new Shoot(10, count: 12, projectileIndex: 4, coolDown: 2000),
                        new TimedTransition(6000, "stayinplace1")
                        ),
                    new State("stayinplace1",
                        new Grenade(2, 180, 8, coolDown: 900),
                        new Shoot(10, count: 24, projectileIndex: 5, coolDown: 2000),
                        new TimedTransition(3000, "stayinplace2")
                        ),
                    new State("stayinplace2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(25, projectileIndex: 2, count: 2, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 3, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 6, shootAngle: 6, projectileIndex: 3, coolDown: 1000),
                        new Shoot(10, count: 12, projectileIndex: 4, coolDown: 2000),
                        new TimedTransition(6000, "stayinplace3")
                        ),
                    new State("stayinplace3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, count: 24, projectileIndex: 5, coolDown: 2000),
                        new Grenade(2, 300, 8, coolDown: 900),
                        new TimedTransition(3000, "stayinplace")
                        )
                      ),
                    new State("chargerush",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(3000, "Rush1")
                        ),
                    new State("Rush1",
                        new Taunt("My power is coming back to me!", "This ancient power, welcome home.", "Haha, pathetic, I have way more power."),
                        new Prioritize(
                            new Follow(1, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(25, projectileIndex: 2, count: 2, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 3, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 4, shootAngle: 10, projectileIndex: 0, coolDown: 1000),
                        new Shoot(10, count: 8, projectileIndex: 1, coolDown: new Cooldown(1200, 200)),
                        new Shoot(10, count: 2, shootAngle: 20, predictive: 1, projectileIndex: 2, coolDown: 500),
                        new TimedTransition(8000, "Rush2")
                        ),
                    new State("Rush2",
                        new Swirl(0.3, radius: 6),
                        new Shoot(10, count: 16, shootAngle: 14, projectileIndex: 5, coolDown: 500),
                        new Shoot(10, count: 1, projectileIndex: 5, coolDown: 100),
                        new TimedTransition(8000, "Wander1")
                        ),
                    new State("Wander1",
                        new HealSelf(coolDown: 2500, amount: 4000),
                        new Wander(0.1),
                        new Shoot(10, count: 10, shootAngle: 18, projectileIndex: 1, coolDown: 1000),
                        new Shoot(10, count: 5, shootAngle: 10, projectileIndex: 0, coolDown: 1000),
                        new Shoot(10, count: 10, projectileIndex: 4, coolDown: 2000),
                        new TimedTransition(7200, "Fight")
                        ),
                    new State("Die",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("My remains will rebuild apon this same earth again! I will unite with my old friends again! Primeval!"),
                        new ReturnToSpawn(speed: 1),
                         new Flash(0xFF0000, 1, 1),
                        new TimedTransition(4000, "rip")
                        ),
                    new State("rip",
                        new Suicide()
                        )
                 )
                 ),
              new Threshold(0.0001,
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Amulet of the Bee Tribe", 0.006, damagebased: true),
                    new ItemLoot("Black Honey Quiver", 0.004, damagebased: true),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new TierLoot(5, ItemType.Ability, 0.3),
                    new TierLoot(11, ItemType.Armor, 0.3),
                    new TierLoot(11, ItemType.Weapon, 0.3),
                    new TierLoot(10, ItemType.Weapon, 0.5),
                    new TierLoot(5, ItemType.Ring, 0.3)
                )
            )
            



            ;
    }
}