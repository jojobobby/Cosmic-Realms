using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.logic.behaviors;
using common.resources;
//by Classic White
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Foundry = () => Behav()

                       .Init("Rusted Factory Owner",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune, true),
                        new PlayerWithinTransition(9, "MakeWeb", true)
                    ),
                    new State("MakeWeb",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(3500, "Attack2")
                    ),
                    new State("Attack2",
                        new StayCloseToSpawn(1, 12),
                        new Shoot(0, projectileIndex: 5, count: 7, coolDown: 3000, coolDownOffset: 4000),
                        new Shoot(25, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 4900),
                        new Shoot(25, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 6000),
                        new Shoot(25, projectileIndex: 2, count: 7, coolDown: 3000, coolDownOffset: 6000),
                        new Shoot(0, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                        new Shoot(10, projectileIndex: 1, coolDown: 3000, predictive: 1),
                        new StayCloseToSpawn(1, 12),
                        new Wander(.7)
                    )
                ),
                new Threshold(0.000001,
                    new ItemLoot("Potion of Attack", 1, 3),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Plague Poison", 0.005, damagebased: true),
                    new ItemLoot("Hivemind Circlet", 0.006, damagebased: true),
                    new ItemLoot("Rags of the Host", 0.006, damagebased: true),
                    new ItemLoot("Resurrected Warrior's Armor", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Cloak of the Rusted Thief", 0.006, damagebased: true)
                    )
            );
         
    }
}