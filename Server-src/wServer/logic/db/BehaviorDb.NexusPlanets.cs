using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ NexusPlanets = () => Behav()
            .Init("Realm Portal",
                new State(
                    new Orbit(speed: 0.003, radius: 5, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )
        .Init("Nexus Portal",
                new State(
                    new Orbit(speed: 0.003, radius: 4, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )
         .Init("Locked Oryx's Arena Portal",
                new State(
                    new Orbit(speed: 0.003, radius: 2, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )
          .Init("Oryx's Arena Portal",
                new State(
                    new Orbit(speed: 0.003, radius: 6, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )
        .Init("Sadala",
                new State(
                    new Orbit(speed: 0.003, radius: 5, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )
          .Init("Wormhole",
                new State(
                    new Orbit(speed: 0.0025, radius: 7, acquireRange: 20, target: "Sun", speedVariance: 0, radiusVariance: 2, orbitClockwise: true)
                    )
            )
          .Init("Sun",
                new State(
            )
        )
        .Init("Moon",
                new State(
                    new Orbit(speed: 0.05, radius: 1, acquireRange: 20, target: "Realm Portal", speedVariance: 0, radiusVariance: 0, orbitClockwise: true),
                    new Orbit(speed: 0.05, radius: 1, acquireRange: 20, target: "Nexus Portal", speedVariance: 0, radiusVariance: 0, orbitClockwise: true)
                    )
            )

         .Init("DPS chart 100",
                new State(
                   new TransformOnDeath("DPS chart 100", 1, 1, 100),
                   new HealSelf(5000,999999)
                    )
            )
         .Init("DPS chart 50",
                new State(
                    new TransformOnDeath("DPS chart 50", 1, 1, 100),
                   new HealSelf(5000, 999999)
                    )
            )
            
         .Init("DPS chart",
                new State(
                    new TransformOnDeath("DPS chart", 1, 1, 100),
                   new HealSelf(5000, 999999)
                    )
            )
        .Init("Nexus Room to Spawn",
                new State(
                    new TransformOnDeath("Nexus Room to Spawn", 1, 1, 100),
                   new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(1, 81, 174, true)
                )
            )
            )
        .Init("Nexus Spawn to Room",
                new State(
                    new TransformOnDeath("Nexus Spawn to Room", 1, 1, 100),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(4, 338, 147, true)
                )
            )
            )
            ;
    }
}