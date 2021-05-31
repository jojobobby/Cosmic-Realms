using wServer.logic.behaviors;
using wServer.logic.transitions;
using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Specialized = () => Behav()
          .Init("Raid Portal Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Castle of Cyberious Portal", 100),
                    new State("Suicide",
                        new Suicide()
                        )
                    )
            )

          .Init("Banner Portal Spawner",
                new State(
                    new TransformOnDeath("Banner Portal Spawner"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesExistsTransition(999, "spawn", "Activated Mind Obelisk", "Activated Body Obelisk", "Activated Soul Obelisk", "Activated Void Obelisk")
                        ),
                    new State("spawn",
                        new ApplySetpiece("RaidSpawner"),
                        new TimedTransition(30000, "die")
                        ),
                    new State("die",
                        new ApplySetpiece("RaidNexusFixer"),
                        new Suicide()
                        )
                    )
            )
           .Init("Mind Obelisk Killer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Activated Mind Obelisk", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Body Obelisk Killer",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Activated Body Obelisk", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Soul Obelisk Killer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Activated Soul Obelisk", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Void Obelisk Killer",
                new State(
                   new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Activated Void Obelisk", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )

            .Init("Activated Mind Obelisk",
                new State(
                    new TransformOnDeath("Mind Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesExistsTransition(999, "die", "Activated Mind Obelisk", "Activated Body Obelisk", "Activated Soul Obelisk", "Activated Void Obelisk")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Activated Body Obelisk",
                new State(
                    new TransformOnDeath("Body Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesExistsTransition(999, "die", "Activated Mind Obelisk", "Activated Body Obelisk", "Activated Soul Obelisk", "Activated Void Obelisk")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Activated Soul Obelisk",
                new State(
                    new TransformOnDeath("Soul Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesExistsTransition(999, "die", "Activated Mind Obelisk", "Activated Body Obelisk", "Activated Soul Obelisk", "Activated Void Obelisk")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
            .Init("Activated Void Obelisk",
                new State(
                    new TransformOnDeath("Void Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntitiesExistsTransition(999, "die", "Activated Mind Obelisk", "Activated Body Obelisk", "Activated Soul Obelisk", "Activated Void Obelisk")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )

            .Init("Mind Obelisk",
                new State(
                    new TransformOnDeath("Activated Mind Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Mind Obelisk Killer", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )

                    )
            )
            .Init("Body Obelisk",
                new State(
                    new TransformOnDeath("Activated Body Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Body Obelisk Killer", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )

                    )
            )
            .Init("Soul Obelisk",
                new State(
                    new TransformOnDeath("Activated Soul Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Soul Obelisk Killer", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )

                    )
            )
            .Init("Void Obelisk",
                new State(
                    new TransformOnDeath("Activated Void Obelisk"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("wait",
                        new EntityExistsTransition("Void Obelisk Killer", 999, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )

                    )
            )
            .Init("Rock Candy Grenade",
                new State(
                    new State("Explode",
                        new Prioritize(
                            new StayCloseToSpawn(3, 3)
                        ),
                        new State("Explode 1",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 2")
                        ),
                        new State("Explode 2",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 3")
                        ),
                        new State("Explode 3",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 4")
                        ),
                        new State("Explode 4",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 5")
                        ),
                        new State("Explode 5",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 6")
                        ),
                        new State("Explode 6",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 7")
                        ),
                        new State("Explode 7",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 8")
                        ),
                        new State("Explode 8",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 9")
                        ),
                        new State("Explode 9",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new TimedTransition(100, "Explode 10")
                        ),
                        new State("Explode 10",
                            new JumpToRandomOffset(-2, 2, -2, 2),
                            new Sound(),
                            new Aoe(2, false, 100, 200, true, 0xFF6633),
                            new Decay(0)
                        )
                    )
                )//Nexus Crier Healing Flowers  new TossObject(child: "Malphas Flamer", range: 6, angle: 0, coolDown: 9000, throwEffect: true),
            )
          .Init("Healing Flowers Spawner",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("taunt",
                    new TossObject(child: "Healing Flowers", range: 0, angle: 0, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1, angle: 180, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1.5, angle: 135, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1, angle: 90, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1.5, angle: 45, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1, angle: 0, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1.5, angle: 225, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1, angle: 270, coolDown: 99999, throwEffect: true),
                    new TossObject(child: "Healing Flowers", range: 1.5, angle: 315, coolDown: 99999, throwEffect: true),
                    new TimedTransition(250, "wait")
                        ),
                    new State("wait",
                    new Suicide()
                        )
                    )
            )
           .Init("Healing Flowers",
                new State(
                     new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                      new State("taunt",
                       new PlayerWithinTransition(1,"start", true)
                        ),
                      new State("start",
                      new HealPlayer(1, 99999, 75),
                      new Suicide()
                        )
                    )
            )
          .Init("Nexus Crier",
                new State(
                        new StayCloseToSpawn(1, 9),
                        new Wander(0.1),
                    new State("taunt",
                        new Taunt("Exchange items in the market with other players!", "Welcome to Cosmic Realms!", "Fight the evil enemies of the realm for better gear!"),
                    new TimedTransition(10000, "wait")
                        ),
                    new State("wait",
                    new TimedTransition(10000, "taunt")
                        )
                    )
            )

            .Init("Nexus Crier on Onsen",
                new State(
                        new StayCloseToSpawn(1, 1),
                        new Wander(0.1),
                    new State("taunt",
                    new Taunt("Cough, welcome to my shop!", "Give me those materials, i'll forge something even more powerful!", "Overpriced? you're silly!", "How's my buddy doing in spawn? I hope he's alright."),
                    new TimedTransition(7500, "wait")
                        ),
                    new State("wait",
                    new TimedTransition(7500, "taunt")
                        )
                    )
            )
            .Init("Genesis Spell Portal 1",
                new State(
                    new State("wait",
                    new Aoe(10, false, 250, 750, true, 0x9400D3),
                    new TimedTransition(1000, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
         .Init("Debug Spawn",
                new State(
                    new State("wait",
                    new Aoe(5, false, 70, 80, true, 0x9400D3),
                    new TimedTransition(1000, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            );
    }
}