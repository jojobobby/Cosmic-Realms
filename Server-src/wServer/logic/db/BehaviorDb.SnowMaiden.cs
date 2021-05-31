using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SnowMaiden = () => Behav()
             .Init("Katara",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(8, "taunt1")
                        ),
                    new State("taunt1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("Ice is not your enemy but your friend!", "The magical wonders of snow", "follow a cold path and you will find what you're looking for.", "I am apart of the snow.", "The cold is no enemy"),
                        new TimedTransition(5000, "RingAttack1")
                        ),
                    new State("RingAttack1",
                        new Flash(0xFF0000, 2, 2),
                        new Wander(0.2),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(8, count: 8, projectileIndex: 0, coolDown: 4550, coolDownOffset: 1500),
                        new Shoot(8, count: 12, projectileIndex: 3, coolDown: 2000, shootAngle: 22),
                        new TimedTransition(6400, "Rush1")
                        ),
                    new State("Rush1",
                        new Prioritize(
                            new Follow(0.5, 8, 1),
                            new Wander(0.4)
                            ),
                        new Shoot(8, count: 6, shootAngle: 14, projectileIndex: 1, coolDown: 3200),
                        new Shoot(8, count: 8, predictive: 1, shootAngle: 28, projectileIndex: 1, coolDown: 2440),
                        new Shoot(10, count: 5, projectileIndex: 0, coolDown: 1600),
                        new TimedTransition(6000, "MaintainDist")
                        ),
                    new State("MaintainDist",
                        new Prioritize(
                            new StayBack(0.4),
                            new Wander(0.4)
                            ),
                        new Shoot(9, count: 7, projectileIndex: 3, coolDown: new Cooldown(3000, 1000)),
                        new Shoot(10, count: 8, shootAngle: 14, projectileIndex: 2, coolDown: 1500),
                        new TimedTransition(6200, "BurstFlame")
                        ),
                    new State("BurstFlame",
                        new Flash(0x00F0FF, 2, 2),
                        new Wander(0.3),
                        new Follow(0.5, 8, 1),
                        new Grenade(3, 110, range: 8, coolDown: 1000),
                        new Shoot(8, count: 9, projectileIndex: 4, coolDown: 750, shootAngle: 12),
                        new TimedTransition(8000, "Fire")
                        ),
                    new State("Fire",
                        new Wander(0.3),
                        new ReturnToSpawn(speed: 1),
                        new TossObject("1Wilderness Protector1", coolDown: 500),
                        new Shoot(9, count: 14, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, count: 6, projectileIndex: 2, predictive: 2, coolDown: 1000),
                        new TimedTransition(6000, "RingAttack1")
                        )
                    ),
                new Threshold(.0001,
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Greater Potion of Wisdom", 1.0),
                    new ItemLoot("Potion of Defense", 1.0),
                    new ItemLoot("Potion of Mana", 1.0),
                    new TierLoot(10, ItemType.Weapon, 0.06),
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new TierLoot(5, ItemType.Ability, 0.06),
                    new TierLoot(6, ItemType.Ability, 0.04),
                    new TierLoot(11, ItemType.Armor, 0.06),
                    new TierLoot(12, ItemType.Armor, 0.05),
                    new TierLoot(5, ItemType.Ring, 0.05),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Necklace of Frozen Flames", 0.006, damagebased: true),
                    new ItemLoot("Frozen Ice Shard", 0.0045, threshold: 0.01, damagebased: true)
                )
            )

            .Init("1Wilderness Protector1",
            new State(
                new State("fight",
                    new Follow(0.65, 8, 1),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 750),
                     new TimedTransition(4000, "swag2")
                    ),
                new State("swag2",
                     new Follow(0.65, 8, 1),
                     new Shoot(8.4, count: 3, shootAngle: 16, projectileIndex: 1, coolDown: 400),
                     new TimedTransition(4000, "fight")
                    )
                )
            )
        .Init("1Wilderness Swordsmen1",
            new State(
                new State("fight",
                    new Follow(1, 8, 1),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 300),
                     new TimedTransition(8000, "swag2")
                    ),
                new State("swag2",
                     new StayBack(0.5, 3),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 300),
                     new TimedTransition(2000, "fight")
                    )
                )
            )
          .Init("1Flying Bug1",
            new State(
                new State("fight",
                    new Follow(1, 8, 1),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 300),
                     new TimedTransition(8000, "swag2")
                    ),
                new State("swag2",
                     new StayBack(0.5, 3),
                     new Shoot(10, count: 1, projectileIndex: 0, coolDown: 300),
                     new TimedTransition(2000, "fight")
                    )
                )
            )
            ;
    }
}