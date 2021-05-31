using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PirateCave = () => Behav()
            .Init("Dreadstump the Pirate King",
                new State(
                    new TransformOnDeath(target: "Glowing Realm Portal"),
                    new State("Start",
                        new Prioritize(
                            new Wander(speed: 0.65),
                            new StayCloseToSpawn(speed: 0.65, range: 6)
                        ),
                        new HpLessTransition(threshold: 0.9999, targetState: "Boast")
                    ),
                    new State("Boast",
                        new Taunt("Hah! I'll drink my rum out of your skull!"),
                        new Prioritize(
                            new Wander(speed: 0.65),
                            new StayCloseToSpawn(speed: 0.65, range: 6)
                        ),
                        new Shoot(radius: 8, projectileIndex: 0, coolDown: 500),
                        new TimedTransition(time: 3000, targetState: "Normal")
                    ),
                    new State("Normal",
                        new Taunt("Arrrr..."),
                        new Shoot(radius: 8, projectileIndex: 0, coolDown: 500),
                        new Shoot(radius: 10, projectileIndex: 1, coolDown: 2500),
                        new Prioritize(
                            new Wander(speed: 0.65),
                            new StayCloseToSpawn(speed: 0.65, range: 6)
                        ),
                        new TimedTransition(time: 6500, targetState: "Cannon_Cluster")
                    ),
                    new State("Cannon_Cluster",
                        new Taunt("Eat cannonballs!"),
                        new Prioritize(
                            new StayCloseToSpawn(speed: 0.4, range: 6),
                            new Orbit(speed: 0.4, radius: 5, acquireRange: 5)
                            ),
                        new Shoot(radius: 8, projectileIndex: 1, coolDown: 999999, coolDownOffset: 1800),
                        new Shoot(radius: 8, projectileIndex: 1, coolDown: 999999, coolDownOffset: 2200),
                        new Shoot(radius: 8, projectileIndex: 1, coolDown: 999999, coolDownOffset: 2600),
                        new TimedTransition(time: 4000, targetState: "Normal")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Weapon, 0.4),
                    new TierLoot(3, ItemType.Weapon, 0.4),
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(1, ItemType.Armor, 0.4),
                    new TierLoot(2, ItemType.Armor, 0.4),
                    new TierLoot(3, ItemType.Armor, 0.4),
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new TierLoot(1, ItemType.Ring, 0.4),
                    new ItemLoot("Pirate Rum", 0.4),
                    new ItemLoot("First Mate's Hook", 0.05),
                    new ItemLoot("Pirate Rum", 0.3),
                    new ItemLoot("Pirate Rum", 0.2),
                    new ItemLoot("Pirate Rum", 0.1)
                )
            );
    }
}