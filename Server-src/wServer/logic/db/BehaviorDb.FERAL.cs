using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ FERAL = () => Behav()
              .Init("F.E.R.A.L.",
                new State(
                    new ScaleHP2(40,3,15),
                    new HpLessTransition(0.2, "rage"),
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("default",
                        new PlayerWithinTransition(8, "taunt1")
                        ),
                     new State("taunt1",
                        new Taunt(1.00, "Glllurp"),
                        new TimedTransition(6000, "fight1")
                         )
                        ),
                     new State(
                         new Reproduce("Mini Bot", 20, 5, 6000),
                    new State("fight1",
                        new Wander(0.4),
                        new Shoot(10, count: 3, shootAngle: 14, projectileIndex: 1, coolDown: 1500),
                        new Shoot(10, count: 7, shootAngle: 16, projectileIndex: 2, coolDown: 3000),
                        new TimedTransition(5250, "fight2")
                        ),
                     new State("fight2",
                        new Taunt(1.00, "Worrp!", "Qoorp!", "Sloorp!"),
                        new Prioritize(
                            new Follow(0.65, 8, 1),
                            new Wander(0.5)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 8, projectileIndex: 3, coolDown: 2000),
                        new Shoot(10, count: 6, projectileIndex: 3, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(10, count: 4, projectileIndex: 3, coolDown: 3000, coolDownOffset: 1500),
                        new Shoot(10, count: 3, shootAngle: 6, projectileIndex: 1, coolDown: 2000),
                        new TimedTransition(6500, "fight3")
                        ),
                    new State("fight3",
                        new Wander(0.75),
                        new Shoot(10, count: 6, projectileIndex: 2, coolDown: 1000),
                        new Shoot(10, count: 6, projectileIndex: 1, coolDown: 1500),
                        new Grenade(5, 150, range: 5, coolDown: 2000),
                        new TimedTransition(6500, "fight4")
                        ),
                     new State("fight4",
                        new Prioritize(
                            new Charge(1, range: 10, coolDown: 3000),
                            new Follow(0.55, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 6, projectileIndex: 3, coolDown: 1000),
                        new Shoot(10, count: 3, shootAngle: 8, projectileIndex: 1, coolDown: 1500, coolDownOffset: 600),
                        new TimedTransition(6000, "fight1")
                        ),
                     new State("rage",
                        new Flash(0xFF0000, 1, 2),
                        new Taunt("WQUORP!", "SNQUORP!"),
                        new Prioritize(
                            new Charge(1, range: 10, coolDown: 3000),
                            new Follow(0.75, 8, 1),
                            new Wander(0.5)
                            ),
                        new Shoot(10, count: 6, projectileIndex: 2, coolDown: 1600),
                        new Shoot(10, count: 6, shootAngle: 8, projectileIndex: 2, coolDown: 1500, coolDownOffset: 1000)

                         ))
                    ),
                new Threshold(0.00001,
                    new TierLoot(10, ItemType.Weapon, 0.75),
                    new TierLoot(5, ItemType.Ability, 0.5),
                    new TierLoot(10, ItemType.Armor, 0.5),
                    new TierLoot(11, ItemType.Armor, 0.05),
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Ring, 0.025),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Potion of Attack", 0.80),
                    new ItemLoot("Potion of Attack", 0.80),
                    new ItemLoot("Abomination's Wrath", 0.004, damagebased: true),
                    new ItemLoot("Horrific Claws", 0.004, damagebased: true),
                    new ItemLoot("Experimental Poison", 0.004, damagebased: true),
                    new ItemLoot("Staff of Experimental Magic", 0.004, damagebased: true),
                    new ItemLoot("Awoken Staff of Experimental Magic", 0.002, damagebased: true, threshold: 0.01)
                    )
            )
            ;
    }
}