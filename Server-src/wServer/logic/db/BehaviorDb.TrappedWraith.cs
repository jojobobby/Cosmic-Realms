#region
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TrappedWraith = () => Behav()


         .Init("Mysterious Crystal Scorpion",
                new State(
                    new StayCloseToSpawn(0.2, 5),
                    new ScaleHP2(60,3,15),
                    new State("Phase1",
                        new Wander(0.3),
                        new Shoot(24, 6, 20, 1, angleOffset: 180, coolDown: 500),
                        new Shoot(24, 3, 30, 0, coolDown: 1000),
                        new State("Invulnerable",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(1000, "Uninvulnerable")
                        ),
                        new State("Uninvulnerable",
                            new TimedTransition(2500, "Invulnerable")
                        ),
                        new HpLessTransition(0.75, "Phase2")
                    ),
                    new State("Phase2",
                        new Wander(0.4),
                        new Shoot(0, 8, 45, 3, 0, coolDown: 400),
                        new Grenade(3.5, 85, 3.5, 0, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 60, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 120, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 180, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 240, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 300, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 360, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new HpLessTransition(0.5, "Phase3")
                    ),
                    new State("Phase3",
                        new Taunt("Run, little bunny, run like a coward!"),
                        new Prioritize(
                            new Charge(1, 10, coolDown: 2000),
                            new Wander(0.5)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(0, 8, 45, 2, 0, coolDown: 400),
                        new Shoot(24, 3, 120, 0, coolDown: 400),
                        new HpLessTransition(0.25, "Phase4")
                    ),
                    new State("Phase4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new ChangeSize(50, 225),
                        new HealSelf(1000, 2000),
                        new TimedTransition(4000, "Phase5")
                    ),
                    new State("Phase5",
                        new Wander(1),
                        new Shoot(0, 8, 45, 2, 0, coolDown: 400),
                        new Shoot(24, 3, 30, 0, coolDown: 1000),
                        new Shoot(24, 6, 20, 1, angleOffset: 180, coolDown: 500),
                        new Grenade(3.5, 85, 3.5, 45, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 135, 2000, ConditionEffectIndex.ArmorBroken, 2000),
                        new Grenade(3.5, 85, 3.5, 270, 2000, ConditionEffectIndex.ArmorBroken, 2000)
                    )
                ),
               new Threshold(0.00001,
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
                     new ItemLoot("Potion of Vitality", 1.00),
                     new ItemLoot("Potion of Wisdom", 1.00),
                     new ItemLoot("Potion of Mana", 0.50),
                     new ItemLoot("Potion of Critical Chance", 0.02),
                     new ItemLoot("Potion of Critical Damage", 0.02),
                     new ItemLoot("Sapphire Wakizashi", .01, damagebased: true),
                     new ItemLoot("Cloak of Sapphire Cloth", .001, threshold: 0.01, damagebased: true),
                     new ItemLoot("Blue power crystal", 0.0045, damagebased: true, threshold: 0.01),
                     new ItemLoot("Queen Crystal Helmet", 0.004, damagebased: true)
                     ))




        ;
    }
}