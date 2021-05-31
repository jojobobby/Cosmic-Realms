using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Spectre = () => Behav()
             .Init("Spectre's Guardian",
                 new State(
                     new ScaleHP2(80,2,15),
                     new DropPortalOnDeath("Spectre's Lair Portal", 100),
                             new StayCloseToSpawn(0.3, range: 7),
                            new Wander(0.5),
                              new Shoot(10, count: 10, coolDown: 2000),
                              new Shoot(10, count: 5, shootAngle: 4, projectileIndex: 1, coolDown: 1000)
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
                     new ItemLoot("Potion of Life", 1.00),
                     new ItemLoot("Potion of Vitality", 1.00),
                     new ItemLoot("Potion of Wisdom", 1.00),
                     new ItemLoot("Potion of Mana", 1.00),
                     new ItemLoot("Potion of Critical Chance", 0.02),
                     new ItemLoot("Potion of Critical Damage", 0.02),
                     new ItemLoot("Ectoplasm", 0.0045, damagebased: true, threshold: 0.01),
                     new ItemLoot("Immortal Mantle", 0.006, damagebased: true),
                     new ItemLoot("Amulet of Cursed Death", 0.001, damagebased: true, threshold: 0.02),
                     new ItemLoot("Supernatural Staff", 0.04, damagebased: true)
                     )
          )


         .Init("Spectre's Phantom",
                new State(
                    new ScaleHP2(80,3,15),
                    new ConditionalEffect(ConditionEffectIndex.StasisImmune),
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                    new State("default",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(8, "taunt1")
                        ),
                    new State("taunt1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("Fear before death!"),
                        new TimedTransition(3000, "taunt2")
                        ),
                    new State("taunt2",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("The dead gives me power, The more power, the better!"),
                        new TimedTransition(3000, "taunt3")
                        ),
                    new State("taunt3",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Flash(0xFFFFFF, 2, 2),
                        new Taunt("And that's why YOU WILL BURN IN HELL"),
                        new TimedTransition(3000, "CircleShot")
                        ),
                    new State("CircleShot",
                       new Prioritize(
                            new StayCloseToSpawn(0.6, 5),
                            new Wander(0.2)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Shoot(10, count: 10, projectileIndex: 5, coolDown: 500),
                        new Shoot(10, count: 3, shootAngle: 16, projectileIndex: 4, coolDown: 2000, coolDownOffset: 200),
                        new Shoot(10, count: 4, shootAngle: 16, projectileIndex: 4, coolDown: 2000, coolDownOffset: 500),
                        new Shoot(10, count: 5, shootAngle: 16, projectileIndex: 4, coolDown: 2000, coolDownOffset: 900),
                        new Shoot(10, count: 6, shootAngle: 16, projectileIndex: 4, coolDown: 2000, coolDownOffset: 1300),
                        new Shoot(10, count: 7, shootAngle: 16, projectileIndex: 4, coolDown: 2000, coolDownOffset: 1700),
                        new TimedTransition(9000, "LinePhase")
                        ),
                    new State("LinePhase",
                        new Prioritize(
                            new Follow(0.4, 8, 1),
                            new StayCloseToSpawn(0.4, 6)
                            ),
                        new Taunt("Imbrace death!"),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 5),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 5, coolDownOffset: 10),
                        new Shoot(10, count: 1, projectileIndex: 1, coolDown: 5),
                        new Shoot(10, count: 1, projectileIndex: 1, coolDown: 5, coolDownOffset: 10),
                        new TimedTransition(8000, "ViciousPhase")
                    ),
                   new State("ViciousPhase",
                        new Prioritize(
                            new StayCloseToSpawn(0.4, 4),
                            new Wander(0.2)
                            ),
                        new Taunt("Your life was never important, Die!", "Death feeds me!"),
                        new Shoot(10, count: 7, projectileIndex: 2, coolDown: 1500),
                        new Grenade(3, 360, 6, coolDown: 3500),
                        new TimedTransition(7000, "ShotgunPhase")
                       ),
                   new State("ShotgunPhase",
                        new Prioritize(
                            new StayCloseToSpawn(1, 5),
                            new Wander(0.2)
                            ),
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new Shoot(10, count: 12, shootAngle: 18, projectileIndex: 3, coolDown: 2750),
                        new Shoot(10, count: 6, shootAngle: 20, predictive: 3, projectileIndex: 4, coolDown: 1500),
                        new Shoot(10, count: 6, shootAngle: 20, predictive: 3, projectileIndex: 5, coolDown: 1500),
                        new TimedTransition(7000, "CircleShot")
                       )

                     ),
                  new Threshold(0.00001,
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(4, ItemType.Ability, 0.1),
                    new TierLoot(10, ItemType.Armor, 0.1),
                    new TierLoot(3, ItemType.Ring, 0.05),
                    new TierLoot(11, ItemType.Armor, 0.05),//Spectre's Lair Key
                    new TierLoot(11, ItemType.Weapon, 0.05),
                    new ItemLoot("Spectre's Lair Key", 0.01),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Ectoplasm", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Spectral Spell", 0.004, damagebased: true),
                    new ItemLoot("Ghostly Withered Skull", 0.004, damagebased: true),
                    new ItemLoot("Phantom Pendant", 0.006, damagebased: true)
                    )
            )
        ;
    }
}