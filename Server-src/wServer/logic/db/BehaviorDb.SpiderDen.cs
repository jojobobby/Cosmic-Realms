using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ SpiderDen = () => Behav()
            .Init("Arachna the Spider Queen",
                new State(
                    new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1),
                    new State("start_the_fun",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, perm: true),
                        new PlayerWithinTransition(dist: 11, targetState: "set_web"),
                        new HpLessTransition(threshold: 0.9999, targetState: "set_web")
                    ),
                    new State("set_web",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject(child: "Arachna Web Spoke 1", range: 11, angle: 240, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 2", range: 11, angle: 0, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 3", range: 11, angle: 120, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 4", range: 11, angle: 300, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 5", range: 11, angle: 60, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 6", range: 11, angle: 180, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 7", range: 6.5, angle: 240, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 8", range: 6.5, angle: 0, coolDown: 10000),
                        new TossObject(child: "Arachna Web Spoke 9", range: 6.5, angle: 120, coolDown: 10000),
                        new TimedTransition(time: 3000, targetState: "eat_flies")
                    ),
                    new State("eat_flies",
                        new Shoot(radius: 15, count: 1, projectileIndex: 0, predictive: 0.15, coolDown: 950),
                        new Shoot(radius: 15, count: 1, projectileIndex: 1, coolDown: 1500, coolDownOffset: 1100),
                        new Shoot(radius: 99, count: 12, projectileIndex: 0, shootAngle: 30, coolDown: 2600),
                        new StayCloseToSpawn(speed: 1, range: 7),
                        new StayBack(speed: 0.9, distance: 7),
                        new Wander(speed: 0.8)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Healing Ichor", 1),
                    new ItemLoot("Healing Ichor", 0.25),
                    new ItemLoot("Spider's Eye Ring", 0.35, damagebased: true),
                    new ItemLoot("Poison Fang Dagger", 0.08, damagebased: true),
                    new ItemLoot("Golden Dagger", 0.3),
                    new TierLoot(6, ItemType.Weapon, 0.4),
                    new TierLoot(7, ItemType.Weapon, 0.25),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(3, ItemType.Ability, 0.25),
                    new TierLoot(3, ItemType.Ring, 0.25)
                )
            )
            .Init("Arachna Web Spoke 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 60, fixedAngle: 60, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 120, fixedAngle: 120, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
               )
            )
            .Init("Arachna Web Spoke 2",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 120, fixedAngle: 120, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 180, fixedAngle: 180, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 240, fixedAngle: 240, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 3",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 240, fixedAngle: 240, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 300, fixedAngle: 300, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 4",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 120, fixedAngle: 120, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 5",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 240, fixedAngle: 240, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 6",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 7",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 120, fixedAngle: 120, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 8",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 120, fixedAngle: 120, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 240, fixedAngle: 240, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("Arachna Web Spoke 9",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 0, fixedAngle: 0, coolDown: 200),
                        new Shoot(radius: 99, count: 1, projectileIndex: 0, shootAngle: 240, fixedAngle: 240, coolDown: 200),
                        new EntityNotExistsTransition(target: "Arachna the Spider Queen", dist: 99, targetState: "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
           .Init("Spider Egg Sac",
                new State(
                    new TransformOnDeath(target: "Green Den Spider Hatchling", min: 2, max: 7),
                    new State("idle",
                        new PlayerWithinTransition(dist: 2, targetState: "Explode")
                    ),
                    new State("Explode",
                        new Suicide()
                    )
                )
            )
           .Init("Green Den Spider Hatchling",
                new State(
                    new Shoot(radius: 9, predictive: 0.5, coolDown: 1000),
                    new Charge(speed: 0.8, range: 8),
                    new Wander(speed: 0.4)
                )
             );
    }
}