using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GhostlySorcerer = () => Behav()
            .Init("Ghostly Sorcerer",
                new State(
                    new ScaleHP2(40,3,15),
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
                        new Taunt("Cease your actions, travelers - you cannot beat the magic I have practiced for centuries!", "Haha, your attacks are mere jokes compared to mine.", "You shall not take what is mine!"),
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
                        new Taunt("YOU SHALL NOT LEAVE THIS PLACE ALIVE!", "I AM THE GREATEST MAGICIAN TO EVER LIVE!", "YOUR TALE ENDS HERE!"),
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
                new Threshold(0.01,
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Dexterity", 0.3, 4),
                    new ItemLoot("Potion of Mana", 0.3),
                    new ItemLoot("Potion of Life", 0.3),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(6, ItemType.Ability, 0.03125)
                )
            )
        ;
    }
}