using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;
//by ???, GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ WoodlandLabyrinth = () => Behav()
        .Init("Murderous Megamoth",
             new State(
                 new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Woodland.mp3"),
                  new ScaleHP2(40,3,15),
                 new Prioritize(
                     new Follow(2.5, 10),
                     new Wander(0.2)
                 ),
                 new Reproduce("Mini Larva", coolDown: 500, densityMax: 10, densityRadius: 4),
                 new Spawn("Mini Larva", 10, 0),
                 new Shoot(25, projectileIndex: 0, count: 2, shootAngle: 10, coolDown: 500, coolDownOffset: 500),
                 new Shoot(25, projectileIndex: 1, count: 1, shootAngle: 0, coolDown: 500, coolDownOffset: 500)
             ),
             new Threshold(0.00001,
                 new ItemLoot("Potion of Vitality", 0.5, 3),
                 new TierLoot(8, ItemType.Weapon, 0.25),
                 new TierLoot(9, ItemType.Weapon, 0.125),
                 new TierLoot(8, ItemType.Armor, 0.25),
                 new TierLoot(9, ItemType.Armor, 0.25),
                 new TierLoot(4, ItemType.Ability, 0.125),
                 new TierLoot(12, ItemType.Weapon, 0.03125),
                 new TierLoot(10, ItemType.Armor, 0.125),
                 new TierLoot(11, ItemType.Armor, 0.125),
                 new TierLoot(12, ItemType.Armor, 0.0625),
                 new TierLoot(13, ItemType.Armor, 0.03125),
                 new ItemLoot("Wine Cellar Incantation", 0.05),
                 new ItemLoot("Mark of the Megamoth", 0, 1),
                 new ItemLoot("Potion of Attack", 0.6),
                 new ItemLoot("Potion of Life", 0.6),
                 new ItemLoot("50 Credits", 0.01),
                     new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                 new ItemLoot("Leaf Bow", 0.004, damagebased: true),
                   new ItemLoot("Bergenia Bow", 0.006, damagebased: true),
                    new ItemLoot("Chrysanthemum Corsage", 0.006, damagebased: true),
                       new ItemLoot("Watarimono", 0.006, damagebased: true),
                    new ItemLoot("Traveler's Trinket", 0.006, damagebased: true)
             )
         )
        .Init("Mini Larva",
            new State(
                new Wander(0.1),
                new Protect(1, "Murderous Megamoth", 100, 5, 5),
                new Shoot(10, count: 4, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
            )
         )
        .Init("Epic Mama Megamoth",
            new State(
                new ScaleHP2(40,3,15),
                new TransformOnDeath("Murderous Megamoth"),
                new State("idle",
                    new HpLessTransition(.3, "change"),
                    new Prioritize(
                        new Follow(2.0, 10),
                        new Wander(0.2)
                    ),
                    new Reproduce("Woodland Mini Megamoth", coolDown: 500, densityMax: 8, densityRadius: 5),
                    new Spawn("Woodland Mini Megamoth", 8, 0),
                    new Shoot(25, projectileIndex: 0, count: 3, shootAngle: 10, coolDown: 400)
                ),
                new State("change",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xfFF0000, 1, 900001),
                    new Decay(3000)
                )
            ),
            new Threshold(0.01,
                new ItemLoot("Sprig of the Copse", 0.006, damagebased: true),
                new ItemLoot("Caduceus of Nature", 0.006, damagebased: true)
            )
        )
        .Init("Woodland Mini Megamoth",
            new State(
                new Wander(0.1),
                new Shoot(25, projectileIndex: 0, count: 1, shootAngle: 0),
                new Protect(1, "Epic Mama Megamoth", 20, 5, 1)
            )
         )
        .Init("Epic Larva",
            new State(
                new ScaleHP2(40,3,15),
                new TransformOnDeath("Epic Mama Megamoth"),
                new State("shoot1",
                    new Prioritize(
                        new Follow(0.75, 8, 1),
                        new Wander(0.25)
                    ),
                    new HpLessTransition(.3, "change"),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 45, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 135, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 225, coolDownOffset: 1250),
                    new Shoot(0, 3, shootAngle: 15, projectileIndex: 0, fixedAngle: 315, coolDownOffset: 1250),
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 1500),
                    new Shoot(10, count: 4, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2, coolDownOffset: 2000)
                ),
                new State("change",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Flash(0xfFF0000, 1, 900001),
                    new Decay(3000)
                )
            ),
            new Threshold(0.01,
                new ItemLoot("Shroud of Sagebrush", 0.006, damagebased: true),
                new ItemLoot("Trinket of the Groves", 0.006, damagebased: true)
            )
        )
        .Init("Woodland Ultimate Squirrel",
            new State(
                new Prioritize(
                    new Follow(0.4, 6, 1, -1, 0),
                    new Wander(0.3)
                    ),
                new Shoot(radius: 7, count: 1, projectileIndex: 0, shootAngle: 20, coolDown: 2000)
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin Mage",
            new State(
                new Prioritize(
                    new Wander(0.4),
                    new Shoot(radius: 10, count: 2, projectileIndex: 0, predictive: 1, coolDown: 500, shootAngle: 2)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Goblin",
            new State(
                new Prioritize(
                    new Wander(0.4),
                    new Follow(0.7, 10, 3, -1, 0),
                    new Shoot(radius: 4, count: 1, projectileIndex: 0, coolDown: 500)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Wooland Armor Squirrel",
            new State(
                new Prioritize(
                    new Follow(0.6, 6, 1, -1, 0),
                    new Wander(0.7),
                    new Shoot(radius: 7, count: 3, projectileIndex: 0, predictive: 1, coolDown: 1000, shootAngle: 15)
                    )
                ),
                new Threshold(0.025,
                    new ItemLoot("Speed Sprout", 0.05)
                )
            )
        .Init("Woodland Silence Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Weakness Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            )
        .Init("Woodland Paralyze Turret",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("idle",
                    new Shoot(10, count: 8, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2)
                    )
                )
            );
                 }
}
