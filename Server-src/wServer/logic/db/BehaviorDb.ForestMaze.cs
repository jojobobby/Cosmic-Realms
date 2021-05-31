using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;
//by hydranoid800, GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ ForestMaze = () => Behav()
            .Init("Mama Megamoth",
                new State(
                    new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Woodland.mp3"),
                    new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1, timeout: 0),
                    new Charge(speed: 1, range: 10, coolDown: 2000),
                    new Wander(speed: 0.2),
                    new Reproduce(children: "Mini Megamoth", densityRadius: 10, densityMax: 4, coolDown: 4000),
                    new Shoot(radius: 10, count: 1, projectileIndex: 0, coolDown: 150)
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Weapon, 0.4),
                    new TierLoot(3, ItemType.Weapon, 0.4),
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new ItemLoot("Shroud of Sagebrush", 0.4),
                    new ItemLoot("Trinket of the Groves", 0.04),
                    new ItemLoot("Caduceus of Nature", 0.04),
                    new ItemLoot("Sprig of the Copse", 0.04),
                    new ItemLoot("Speed Sprout", 0.04),
                    new ItemLoot("Speed Sprout", 0.3),
                    new ItemLoot("Speed Sprout", 0.2),
                    new ItemLoot("Speed Sprout", 0.1)
                )
            )
            .Init("Mini Megamoth",
                new State(
                    new State("1",
                        new EntityExistsTransition(target: "Mama Megamoth", dist: 100, targetState: "1"),
                        new EntityNotExistsTransition(target: "Mama Megamoth", dist: 100, targetState: "2")
                    ),
                    new State("1",
                        new Protect(speed: 1, protectee: "Mama Megamoth", acquireRange: 100, protectionRange: 3, reprotectRange: 1),
                        new Wander(speed: 0.1),
                        new TimedTransition(time: 5000, targetState: "3")
                    ),
                    new State("3",
                        new Wander(speed: 0.5),
                        new Shoot(radius: 8, count: 1, coolDown: 1000),
                        new TimedTransition(time: 3000, targetState: "1")
                    ),
                    new State("2",
                        new Wander(speed: 0.5),
                        new Shoot(radius: 10, count: 1, projectileIndex: 0, predictive: 1, coolDown: 1000),
                        new TimedTransition(time: 5000, targetState: "4")
                    ),
                    new State("4",
                        new Wander(speed: 0.1),
                        new Shoot(radius: 8, count: 1, coolDown: 1000),
                        new TimedTransition(time: 3000, targetState: "2")
                    )
                )
            );
    }
}
