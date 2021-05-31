using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ LostSentry = () => Behav()

         .Init("Lost Sentry",
                new State(
                  new ScaleHP2(75, 2, 15),
                    new DropPortalOnDeath("Lost Halls Portal", 100, 120),
                    new HpLessTransition(0.10, "Dead1"),
                    new State("swag",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(12, "Waiting")
                        ),
                    new State("Waiting",
                        new Flash(0x00FF00, 1, 2),
                        new Taunt("You have awoken me, {PLAYER}."),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(4000, "Sentry1")
                        ),
                    new State("Sentry1",
                        new Flash(0x00FFFF, 1, 2),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),

                             new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(8000, "Sentry2")
                        ),
                    new State("Sentry2",
                           new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(8000, "Sentry3")
                        ),
                    new State("Sentry3",
                        new Flash(0x00FFFF, 1, 2),
                         new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(8000, "Sentry4")
                        ),
                    new State("Sentry4",
                         new Shoot(8.4, count: 8, projectileIndex: 3, coolDown: 100),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Grenade(3, 180, range: 7),
                       new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(8000, "Sentry5")
                        ),
                    new State("Sentry5",
                        new Flash(0x00FFFF, 1, 2),
                         new Shoot(8.4, count: 8, projectileIndex: 3, coolDown: 100),
                        new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(5000, "spiral1")
                        ),
                     new State("spiral1",
                         new Flash(0x00FFFF, 1, 2),
                          new Shoot(8.4, count: 8, projectileIndex: 3, coolDown: 100),
                         new ConditionalEffect(ConditionEffectIndex.Armored),
                          new Shoot(9, count: 8, projectileIndex: 1, coolDown: 500),
                        new Shoot(18, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Shoot(9, 3, projectileIndex: 2, shootAngle: 10, coolDownOffset: 500, predictive: 0.2),
                        new TimedTransition(7000, "Sentry1")
                        ),
                    new State("Dead1",
                        new Taunt("I have failed."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0x0000FF, 0.2, 3),
                        new TimedTransition(3250, "dead")
                        ),
                    new State("dead",
                        new Suicide()
                        )

        ),

                 new Threshold(0.0001,
                    new ItemLoot("Potion of Defense", 0.30),
                    new ItemLoot("Potion of Vitality", 0.80),
                    new ItemLoot("Potion of Vitality", 0.80),
                    new ItemLoot("Potion of Life", 0.80),
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
                    new TierLoot(2, ItemType.Potion),
                    new ItemLoot("Earth Shard", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Cloak of Bloody Surprises", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Marble Protection Ring", 0.008, damagebased: true),
                    new ItemLoot("Lost Passion Armor", 0.008, damagebased: true),
                    new ItemLoot("Harden Helmet of Marble", 0.008, damagebased: true),
                    new ItemLoot("Sharp Hall's Claymore", 0.008, damagebased: true)
                      )

            )
        ;
    }
}