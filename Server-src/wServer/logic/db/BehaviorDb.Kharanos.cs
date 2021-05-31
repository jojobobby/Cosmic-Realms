using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Kharanos = () => Behav()

            .Init("Kharanos, the Lost Dragon",
                new State(
                  new ScaleHP2(85, 2, 15),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(12, "Start", true)
                        ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("{PLAYER} you should leave my domain!"),
                        new TimedTransition(3000, "Start2")
                    ),
                     new State("Start2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedRandomTransition(2000, false, "xxx", "yyy")
                    ),
                     new State(
                         new HpLessTransition(0.50, "After"),
                    new State("xxx",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Shoot(10, projectileIndex: 1, count: 4, shootAngle: 170, rotateAngle: 15, coolDown: 200, fixedAngle: 0),
                        new Grenade(3, 150, 8, coolDown: 250),
                        new TimedTransition(15000, "yyy")
                        ),
                    new State("yyy",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 180, fixedAngle: 0, coolDown: 100, coolDownOffset: 125, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 160, fixedAngle: 0, coolDown: 100, coolDownOffset: 200, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 170, fixedAngle: 0, coolDown: 100, coolDownOffset: 200, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 1, count: 1, fixedAngle: 0,  shootAngle: 20, rotateAngle: -20, coolDown: 350),
                        new TimedTransition(15000, "xxx")
                        )
                             ),
                    new State("After",
                        new Taunt("I suppose I should get real"),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new TimedTransition(3000, "After2")
                          ),
                    new State("After2",
                        new Wander(0.2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 180, fixedAngle: 0, coolDown: 100, coolDownOffset: 125, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 160, fixedAngle: 0, coolDown: 100, coolDownOffset: 200, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 200, fixedAngle: 0, coolDown: 100, coolDownOffset: 200, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 0, count: 2, shootAngle: 140, fixedAngle: 0, coolDown: 100, coolDownOffset: 300, rotateAngle: 10),
                        new Shoot(10, projectileIndex: 2, count: 1, fixedAngle: 0, shootAngle: 20, rotateAngle: -20, coolDown: 200),
                        new HpLessTransition(0.20, "yyy2"),
                        new ConditionalEffect(ConditionEffectIndex.Armored)
                              ),
                    new State("yyy2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 2000),
                         new Wander(0.2),
                        new Follow(0.5, 10, 2, 2000),
                        new Shoot(45, projectileIndex: 0, count: 8, fixedAngle: 0, coolDown: 400, coolDownOffset: 200, rotateAngle: 29),
                        new TimedTransition(15000, "dead")
                          ),
                    new State("dead",
                        new Taunt("The Dragon Master shall know about this."),
                        new TimedTransition(3000, "dead2")

                         ),
                    new State("dead2",
                        new Suicide()
                         )
                    ),
                new Threshold(0.0001,
                     new TierLoot(4, ItemType.Ability, 0.1),
                     new TierLoot(4, ItemType.Ring, 0.05),
                     new TierLoot(9, ItemType.Armor, 0.03),
                     new TierLoot(5, ItemType.Ability, 0.03),
                     new TierLoot(9, ItemType.Weapon, 0.03),
                     new TierLoot(10, ItemType.Armor, 0.02),
                     new TierLoot(10, ItemType.Weapon, 0.02),
                     new TierLoot(11, ItemType.Armor, 0.01),
                     new TierLoot(11, ItemType.Weapon, 0.01),
                     new TierLoot(5, ItemType.Ring, 0.01),
                new ItemLoot("Potion of Critical Chance", 0.02),
                new ItemLoot("Potion of Critical Damage", 0.02),
                new ItemLoot("Potion of Defense", .02),
                new ItemLoot("Potion of Attack", 1),
                new ItemLoot("Potion of Vitality", .02),
                new ItemLoot("Potion of Wisdom", .02),
                new ItemLoot("Potion of Speed", 1),
                new ItemLoot("Potion of Dexterity", .02),
                new ItemLoot("Hide of Depth", 0.001, threshold: 0.01, damagebased: true),
                new ItemLoot("Pressurised Water", 0.001, threshold: 0.01, damagebased: true),
                new ItemLoot("Depth of the Deep Dagger", 0.001, threshold: 0.01, damagebased: true),
                new ItemLoot("Deep Solid Blue Water", 0.001, threshold: 0.01, damagebased: true),
                new ItemLoot("Large Smiley Cloth", 0.03),
                new ItemLoot("Small Smiley Cloth", 0.03)
                    )
            );
    }
}