using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Pentaract = () => Behav()
            .Init("Pentaract Eye",
                new State(
                    new Prioritize(
                        new Swirl(2, 8, 20, true),
                        new Protect(2, "Pentaract Tower", 20, 6, 4)
                        ),
                    new Shoot(9, 1, coolDown: 1000)
                    )
            )
            .Init("Pentaract Tower",
                new State(
                   new ScaleHP2(60, 3, 15),
                    new Spawn("Pentaract Eye", 5, coolDown: 10000, givesNoXp: false),
                    new TransformOnDeath("Pentaract Tower Corpse"),
                    new TransferDamageOnDeath("Pentaract Tower Corpse"),
                    new State("Start",
                    new Grenade(3, 150, 12, coolDown: 1250),
                   new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new Shoot(10, count: 2, shootAngle: 7, predictive: 0.2, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(10, count: 8, shootAngle: 16, predictive: 0.2, projectileIndex: 1, coolDown: new Cooldown(500, 3000)),
                        new HpLessTransition(.70, "Start2")
                        ),
                    new State("Start2",
                        new Flash(0xffffff, 2, 4),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start3")
                           ),
                    new State("Start3",
                    new Grenade(3, 150, 12, coolDown: 1250),
                    new Shoot(10, count: 2, shootAngle: 7, predictive: 0.2, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(10, count: 8, shootAngle: 16, predictive: 0.2, projectileIndex: 1, coolDown: new Cooldown(500, 3000)),
                        new HpLessTransition(.30, "Start4")
                           ),
                    new State("Start4",
                        new Flash(0xffffff, 2, 4),
                   new ConditionalEffect(ConditionEffectIndex.Invulnerable, false, 3000),
                    new TimedTransition(2000, "Start5")
                           ),
                    new State("Start5",
                    new Grenade(3, 150, 12, coolDown: 1250),
                    new Shoot(10, count: 2, shootAngle: 7, predictive: 0.2, projectileIndex: 0, coolDown: 4000, coolDownOffset: 1000),
                    new Shoot(10, count: 8, shootAngle: 16, predictive: 0.2, projectileIndex: 1, coolDown: new Cooldown(500, 3000))
                    )
            )
            )
            .Init("Pentaract",
                new State(
                    new ScaleHP2(40,3,15),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    )
            )
            .Init("Pentaract Tower Corpse",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Waiting",
                        new TimedTransition(150000, "Spawn"),
                        new EntityNotExistsTransition("Pentaract Tower", 50, "Die")
                        ),
                    new State("Spawn",
                        new Transform("Pentaract Tower")
                        ),
                    new State("Die",
                        new Suicide()
                        )
                    ),
                new Threshold(0.00001,
                    new TierLoot(8, ItemType.Weapon, .15),
                    new TierLoot(9, ItemType.Weapon, .1),
                    new TierLoot(10, ItemType.Weapon, .07),
                    new TierLoot(11, ItemType.Weapon, .05),
                    new TierLoot(4, ItemType.Ability, .15),
                    new TierLoot(5, ItemType.Ability, .07),
                    new TierLoot(8, ItemType.Armor, .2),
                    new TierLoot(9, ItemType.Armor, .15),
                    new TierLoot(10, ItemType.Armor, .10),
                    new TierLoot(11, ItemType.Armor, .07),
                    new TierLoot(12, ItemType.Armor, .04),
                    new TierLoot(3, ItemType.Ring, .15),
                    new TierLoot(4, ItemType.Ring, .07),
                    new TierLoot(5, ItemType.Ring, .03),
                    new ItemLoot("Potion of Defense", .3),
                    new ItemLoot("Potion of Attack", .3),
                    new ItemLoot("Potion of Vitality", .4),
                    new ItemLoot("Potion of Wisdom", .4),
                    new ItemLoot("Potion of Speed", .4),
                    new ItemLoot("Potion of Dexterity", .6),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Seal of Blasphemous Prayer", .004, damagebased: true),
                    new ItemLoot("Null-Magic Ring", .01, damagebased: true),
                    new ItemLoot("Magic-Resistance Robe", .01, damagebased: true),
                    new ItemLoot("Awoken Staff of Mother Nature", .002, damagebased: true, threshold: 0.01),
                    new ItemLoot("Staff of Mother Nature", .004, damagebased: true),
                    new TierLoot(2, ItemType.Potion, numRequired: 1)

                     )
            )
            ;
    }
}