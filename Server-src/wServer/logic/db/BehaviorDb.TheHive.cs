using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TheHive = () => Behav()
         
            .Init("TH Maggot Egg1",
                new State(
                    new TransformOnDeath(target: "TH Maggots", min: 2, max: 3, probability: 1),
                    new State("HitMeiDareYou",
                        new HpLessTransition(threshold: 0.98, targetState: "KillSelf")
                    ),
                    new State("KillSelf",
                        new Decay(time: 10)
                    )
                )
            )
            .Init("TH Maggots",
                new State(
                    new Shoot(radius: 6, count: 1, projectileIndex: 0, coolDown: 1000),
                    new Wander(speed: 0.85),
                    new State("1",
                        new TimedTransition(time: 10000, targetState: "2")
                    ),
                    new State("2",
                        new Transform(target: "TH Fat Bees 2")
                    )
                )
            )
            .Init("TH Fat Bees 2",
                new State(
                    new TransformOnDeath(target: "TH Red Fat Bees 2", min: 1, max: 1, probability: 0.25),
                    new Shoot(radius: 6, count: 1, projectileIndex: 0, coolDown: 1000, predictive: 0.5),
                    new Wander(speed: 0.9)
                )
            )
            .Init("TH Red Fat Bees 2",
                new State(
                    new Shoot(radius: 6, count: 1, projectileIndex: 0, coolDown: 1000),
                    new Follow(speed: 1, acquireRange: 5, range: 2, duration: 8000, coolDown: 2000),
                    new Wander(speed: 0.8)
                )
            )
        ;
    }
}