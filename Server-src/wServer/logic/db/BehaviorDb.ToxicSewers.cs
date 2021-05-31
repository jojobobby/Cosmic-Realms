using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//from LOE-V6, bit chages by ghostmaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ToxicSewers = () => Behav()
            .Init("ds Gulpord the Slime God",
                new State(
                  new ScaleHP2(40,3,15),
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new State("idle",
                        new PlayerWithinTransition(dist: 12, targetState: "begin", seeInvis: true)
                        ),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(time: 3500, targetState: "shoot")
                        ),
                    new State("shoot",
                        new HpLessTransition(0.90, "randomshooting"),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new Shoot(10, 8, 45, 1, coolDown: 2000),
                        new Shoot(10, 5, 72, 0, 0, coolDown: 600, coolDownOffset: 300),
                        new Shoot(10, 5, 72, 0, 3, coolDown: 600, coolDownOffset: 300),
                        new Shoot(10, 5, 72, 0, 36, coolDown: 600, coolDownOffset: 600),
                        new Shoot(10, 5, 72, 0, 39, coolDown: 600, coolDownOffset: 600)
                        ),
                    new State("randomshooting",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new Shoot(10, 8, 45, 0, fixedAngle: 0, coolDown: 600, coolDownOffset: 300),
                        new Shoot(10, 8, 45, 0, fixedAngle: 22, coolDown: 600, coolDownOffset: 600),
                        new ReturnToSpawn(speed: 1),
                        new HpLessTransition(0.70, "tossnoobs"),
                        new TimedTransition(6000, "tossnoobs")
                        ),
                    new State("tossnoobs",
                        new TossObject("DS Boss Minion", 3, 0, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 45, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 90, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 135, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 180, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 225, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 270, coolDown: 99999999),
                        new TossObject("DS Boss Minion", 3, 315, coolDown: 99999999),
                        new TimedTransition(100, "derp")
                        ),
                    new State("derp",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1500),
                        new HpLessTransition(0.50, "baibaiscrubs"),
                        new Shoot(10, 6, 12, 0, coolDown: 1000),
                        new Wander(0.5),
                        new StayCloseToSpawn(0.5, 7)
                        ),
                    new State("baibaiscrubs",
                        new ReturnToSpawn(speed: 2),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new TimedTransition(1500, "seclol")
                        ),
                    new State("seclol",
                        new ChangeSize(20, 0),
                        new TimedTransition(1000, "nubs")
                        ),
                    new State("nubs",
                        new TossObject("DS Gulpord the Slime God M", 3, 32, coolDown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God M", 3, 15, coolDown: 9999999, tossInvis: true),
                        new EntityExistsTransition(target: "DS Gulpord the Slime God M", dist: 999, targetState: "idleeeee")
                        ),
                    new State("idleeeee",
                        new EntitiesNotExistsTransition(999, "nubs2", "DS Gulpord the Slime God M")
                        ),
                    new State("nubs2",
                        new TossObject("DS Gulpord the Slime God s", 3, 32, coolDown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God s", 3, 15, coolDown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God s", 3, 26, coolDown: 9999999, tossInvis: true),
                        new TossObject("DS Gulpord the Slime God s", 3, 21, coolDown: 9999999, tossInvis: true),
                        new EntityExistsTransition(target: "DS Gulpord the Slime God s", dist: 999, targetState: "idleeeeee")
                        ),
                    new State("idleeeeee",
                        new EntitiesNotExistsTransition(999, "seclolagain", "DS Gulpord the Slime God s")
                        ),
                    new State("seclolagain",
                        new ChangeSize(20, 120),
                        new TimedTransition(1000, "GO ANGRY!!!!111!!11")
                        ),
                    new State("GO ANGRY!!!!111!!11",
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(1000, "FOLLOW")
                        ),
                    new State("FOLLOW",
                        new ConditionalEffect(ConditionEffectIndex.StunImmune),
                        new ConditionalEffect(ConditionEffectIndex.ParalyzeImmune),
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(10, 8, 45, 2, coolDown: 2000),
                        new Shoot(3, 1, 0, 1, coolDown: 1000),
                        new Shoot(10, 2, 10, 0, coolDown: 150, angleOffset: 0.1f),
                        new Follow(speed: 0.6, acquireRange: 10, range: 0)
                    )
                ),
                new Threshold(0.000001,
                    new TierLoot(7, ItemType.Weapon, 0.25),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(3, ItemType.Ability, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(3, ItemType.Ring, 0.25),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Gulpord", 0, 1),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Murky Toxin", 0.004, damagebased: true),
                    new ItemLoot("Virulent Venom", 0.006, damagebased: true),
                    new ItemLoot("Dagger of Toxin", 0.006, damagebased: true),
                    new ItemLoot("Sewer Cocktail", 0.006, damagebased: true)
                )
            )
            .Init("DS Boss Minion",
                new State(
                    new Wander(0.6),
                    new Grenade(3, 50, 10, coolDown: 5000)
                )
            )
            .Init("DS Gulpord the Slime God M",
                new State(
                    new Orbit(0.6, 3, target: "ds gulpord the slime god"),
                    new Shoot(10, 8, 45, 1, coolDown: 1500),
                    new Shoot(10, 4, 60, 0, coolDown: 4500)
                )
            )
            .Init("ds gulpord the slime god s",
                new State(
                    new Orbit(0.6, 3, target: "ds gulpord the slime god"),
                    new Shoot(10, 4, 20, 1, coolDown: 2000)
                )
            )
            .Init("DS Natural Slime God",
                new State(
                    new Prioritize(
                        new StayAbove(speed: 1, altitude: 200),
                        new Follow(speed: 1, range: 7),
                        new Wander(speed: 0.4)
                    ),
                    new Shoot(radius: 12, count: 5, shootAngle: 10, projectileIndex: 0, predictive: 1, coolDown: 1000),
                    new Shoot(radius: 10, count: 1, projectileIndex: 1, predictive: 1, coolDown: 650)
                )
            )
            ;
    }
}