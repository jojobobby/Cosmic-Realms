using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree, ???
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ DeadwaterDocks = () => Behav()
            .Init("Jon Bilgewater the Pirate King",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("Waiting",
                        new HpLessTransition(0.999, "Yay i am good")
                    ),
                    new State("Yay i am good",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Hahaha! Ye can't kill me", "With Dreadstump gone, I’m the new king!", "Dreadstump was a dreadCHUMP! And so are you!"),
                        new TimedTransition(2000, "Phase1")
                    ),

                    new State("Phase1",
                        new HpLessTransition(0.7, "Phase2"),
                        new OrderOnce(100, "Parrot Cage", "Spawn"),
                        new Wander(0.4),
                        new State("Phase1.1",
                            new Shoot(20, count: 1, projectileIndex: 0, coolDown: 500),
                            new Shoot(20, count: 1, projectileIndex: 1, coolDown: 2000),
                            new TimedRandomTransition(6000, false,
                                "Phase1.2",
                                "Phase1.3")
                            ),
                        new State("Phase1.2",
                            new TimedTransition(2000, "Phase1.1"),
                            new Taunt(true,
                                "Check out AWESOME CANNON CLUSTER!",
                                "Dodge this!",
                                "PEWPEWPEW!",
                                "Har har har!!"
                                ),
                            new Timed(2000, new Sequence(
                                new Shoot(20, count: 5, shootAngle: 10, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(20, count: 6, shootAngle: 10, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(20, count: 6, shootAngle: 10, coolDownOffset: 500, coolDown: 2000,
                                    projectileIndex: 1)
                                ))
                            ),
                        new State("Phase1.3",
                            new TimedTransition(1000, "Phase1.1"),
                            new Follow(0.7, 20, 0),
                            new Shoot(20, count: 1, projectileIndex: 0, coolDown: 500),
                            new Shoot(20, count: 1, projectileIndex: 1, coolDown: 2000)
                            )
                        ),
                    new State("Phase2",
                        new HpLessTransition(0.5, "Phase3"),
                        new State("Phase2.0",
                            new Taunt(true, "Now you're making me angry! PARROT BARRIER ACTIVATE!"),
                            new ReturnToSpawn(1.4),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new OrderOnce(100, "Deadwater Docks Parrot", "Orbit"),
                            new OrderOnce(100, "Deadwater Docks Macaw", "Orbit"),
                            new OrderOnce(100, "Parrot Cage", "Wait"),
                            new GroupNotExistTransition(100, "Phase2.1", "Deadwater Parrots")
                            ),
                        new State("Phase2.1",
                            new Taunt(true, "YOU'LL PAY FOR KILLING MY PARROTS!"),
                            new OrderOnce(100, "Parrot Cage", "Spawn"),
                            new TimedTransition(2000, "Phase2.2")
                            ),
                        new State("Phase2.2",
                            new OrderOnce(100, "Parrot Cage", "Spawn"),
                            new Taunt(true, "CANNON BARRAGE!"),
                            new Timed(2000, new Sequence(
                                new Shoot(0, count: 13, fixedAngle: 30, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(0, count: 14, fixedAngle: 30, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(0, count: 15, fixedAngle: 30, coolDownOffset: 500, coolDown: 2000,
                                    projectileIndex: 1)
                                )),
                            new TimedTransition(10000, "Phase2")
                            )
                        ),
                    new State("Phase3",
                        new State("Phase3.1",
                            new Taunt(true, "PARROT BARRIER ACTIVATE!"),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new OrderOnce(100, "Deadwater Docks Parrot", "Orbit"),
                            new OrderOnce(100, "Deadwater Docks Macaw", "Orbit"),
                            new OrderOnce(100, "Parrot Cage", "Wait"),
                            new Spawn("Massive Parrot", 1, 0),
                            new State("1",
                                new EntityExistsTransition("Massive Parrot", 30, "2")
                            ),
                            new State("2",
                                new EntityNotExistsTransition("Massive Parrot", 30, "Phase3.2")
                            )
                        ),
                        new State("Phase3.2",
                            new TimedRandomTransition(3000, true,
                                "Phase3.1",
                                "Phase3.2"),
                            new OrderOnce(8, "Deadwater Docks Parrot", "Die"),
                            new OrderOnce(8, "Deadwater Docks Macaw", "Die"),
                            new OrderOnce(100, "Parrot Cage", "Spawn"),
                            new Taunt(true, "CANNON BARRAGE!"),
                            new Timed(2000, new Sequence(
                                new Shoot(0, count: 13, fixedAngle: 30, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(0, count: 14, fixedAngle: 30, coolDownOffset: 500, coolDown: 500,
                                    projectileIndex: 1),
                                new Shoot(0, count: 15, fixedAngle: 30, coolDownOffset: 500, coolDown: 2000,
                                    projectileIndex: 1)
                                ))
                            )
                        )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Potion of Speed", 0.3, 3),
                    new ItemLoot("Potion of Dexterity", 0.3, 3),
                    new ItemLoot("Gold", 0.3),
                    new ItemLoot("Potion of Life", 0.3),//Gold
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(5, ItemType.Ring, 0.0625),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Luck", 0.05),
                    new ItemLoot("Tricorne of the High Seas", 0.006, damagebased: true),
                    new ItemLoot("Naval Uniform", 0.006, damagebased: true),
                    new ItemLoot("Hallucinogenic Prism of Magic", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Pirate's Sea Hohnica", 0.004, damagebased: true),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Bilgewater", 1),//Pirate's Sea Hohnica
                    new ItemLoot("Pirate King's Cutlass", 0.004, damagebased: true)
               )
            )
            .Init("Parrot Cage",
                new State(
                    new State("Hmm",
                        new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 100, "Die"),
                        new State("Wait"),
                        new State("Spawn",
                            new ReproduceGroup("Deadwater Parrots", 5, 4, coolDown: 800)
                            ),
                        new State("Die",
                            new Decay(100)
                        )
                    )
            )
            )
          .Init("Deadwater Docks Lieutenant",
                new State(
                    new Follow(1, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 1000)
                    ),//Pirate Saber
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Veteran",
                new State(
                    new Follow(0.8, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 500)
                    ),
                new TierLoot(10, ItemType.Weapon, 0.05),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Admiral",
                new State(
                    new Follow(0.6, 8, 1),
                    new Shoot(8, 3, 10, coolDown: 1325)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Brawler",
                new State(
                    new Follow(1.12, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 350)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Sailor",
                new State(
                    new Follow(0.9, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 525)
                    ),
                new Threshold(0.0001,
                new ItemLoot("Pirate Saber", 0.01)
            )
        )
          .Init("Deadwater Docks Commander",
                new State(
                    new Follow(0.90, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 900)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )
          .Init("Deadwater Docks Captain",
                new State(
                    new Follow(0.47, 8, 1),
                    new Shoot(8, 1, 10, coolDown: 3500)
                    ),
                new ItemLoot("Magic Potion", 0.1),
                new ItemLoot("Health Potion", 0.1)
            )

            .Init("Deadwater Docks Parrot", //Spawn from Cage | Orbit Cage, Jon phase |Red
                new State(
                    new State("Hmm",
                        new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 100, "Die"),
                        new State("Cage",
                            new Orbit(0.8, 2, 50, "Parrot Cage", speedVariance: 0.1, radiusVariance: .5),
                            new Shoot(10, count: 1, coolDown: 1000)
                            ),
                        new State("Orbit",
                            new Orbit(0.8, 2, 50, "Jon Bilgewater the Pirate King", speedVariance: 0.1,
                                radiusVariance: 0.5),
                            new Shoot(10, count: 1, coolDown: 1000)
                            ),
                        new State("Die",
                            new Decay(1000)
                            )
                        )
                    )
            )
            .Init("Deadwater Docks Macaw", //Spawn from Cage | Orbit Cage, Jon phase |Blue
                new State(
                    new State("Hmm",
                        new EntityNotExistsTransition("Jon Bilgewater the Pirate King", 100, "Die"),
                        new State("Cage",
                            new Orbit(0.8, 3, 50, "Parrot Cage", speedVariance: 0.1, radiusVariance: .5, orbitClockwise: true),
                            new Shoot(10, count: 1, coolDown: 1000)
                            ),
                        new State("Orbit",
                            new Orbit(0.8, 3, 50, "Jon Bilgewater the Pirate King", speedVariance: 0.1, radiusVariance: .5, orbitClockwise: true),
                            new Shoot(10, count: 1, coolDown: 1000)
                            ),
                        new State("Die",
                            new RemoveTileObject(0x705d, 500),
                            new Decay(1000)
                            )
                        )
                    )
            )
            .Init("Massive Parrot", //Spawn from Jon
                new State(
                    new Follow(0.6, 20, 2),
                    new Shoot(10, count: 1, coolDown: 1000)
                    )
            )
            
        ;
    }
}