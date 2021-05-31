using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ rta = () => Behav()//by  ppmaks
        .Init("DS Golden Rat",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                           new StayBack(0.9,8),
                        new Taunt("Squeek squeek!")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Legendary Crate", 0.001),
                    new ItemLoot("Potion Crate", 0.8),
                    new ItemLoot("Items Crate", 0.7),
                    new ItemLoot("Potion of defense", 1),
                    new ItemLoot("Toxic Sewers Key", 0.01),
                    new ItemLoot("Dagger of Toxin", 0.009),
                    new ItemLoot("Murky Toxin", 0.009)
                )
            )
             .Init("DS Orange Turtle",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                           new Wander(0.6),
                        new Taunt("!")       
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Legendary Crate", 0.001),
                    new ItemLoot("Potion Crate", 0.8),
                    new ItemLoot("Items Crate", 0.7),
                    new ItemLoot("Potion of defense", 1),
            new ItemLoot("Toxic Sewers Key", 0.01),
                    new ItemLoot("Dagger of Toxin", 0.009),
                    new ItemLoot("Murky Toxin", 0.009),
                    new ItemLoot("St Patrick's Green Clothing Dye", 0.4)
                )
            )
            .Init("DS Red Turtle",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                           new Wander(0.6),
                        new Taunt("!")
                    )
                ),
                new Threshold(0.01,
                   new ItemLoot("Legendary Crate", 0.001),
                     new ItemLoot("Potion Crate", 0.8),
                    new ItemLoot("Items Crate", 0.7),
                 new ItemLoot("Potion of defense", 1),
                    new ItemLoot("Toxic Sewers Key", 0.01),
                    new ItemLoot("Pernicious Peridot", 0.009),
                     new ItemLoot("Murky Toxin", 0.009)
                )
            )
            .Init("DS Blue Turtle",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Wander(0.6),
                        new Taunt("!")
                    )
                ),
                new Threshold(0.01,
                 new ItemLoot("Legendary Crate", 0.001),
                    new ItemLoot("Potion Crate", 0.8),
                   new ItemLoot("Items Crate", 0.7),
                    new ItemLoot("Potion of defense", 1),
                    new ItemLoot("Toxic Sewers Key", 0.01),
                  new ItemLoot("Virulent Venom", 0.009),
                     new ItemLoot("Murky Toxin", 0.009)
                )
            )
                .Init("DS Purple Turtle",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                           new Wander(0.6),
                        new Taunt("!")
                    )
                ),
                new Threshold(0.01,
                   new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.8),
                  new ItemLoot("Items Crate", 0.7),
                    new ItemLoot("Potion of defense", 1),
                    new ItemLoot("Toxic Sewers Key", 0.1),
                 new ItemLoot("Acidic Armor", 0.009),
                new ItemLoot("Murky Toxin", 0.009)
                )
            )
            .Init("DS Master Rat",
                new State(
                     new State("0",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                         new PlayerWithinTransition(3, "1")
                    ),
                    new State("1",
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("What time is it?"),
                         new PlayerTextTransition("2", "Its Pizza Time!"),
                            new TimedTransition(15000, "poopsex")
                         ),
                    new State("poopsex",
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("You wouldn't be a good student anyway!"),
                        new Shoot(999, 16, 30, projectileIndex: 0),
                         new TimedTransition(15000, "poopsex"),
                         new TimedTransition(500, "6")
                    ),
                    new State("2",
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Taunt("Where is the safest place in the world?"),
                       new TimedTransition(15000, "poopsex"),
                         new PlayerTextTransition("3", "Inside my shell.")
                         ),
                    new State("3",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Taunt("What is fast, quiet and hidden by the night?"),
                       new TimedTransition(15000, "poopsex"),
                         new PlayerTextTransition("4", "A ninja of course!")
                            ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Taunt("How do you like your pizza??"),
                       new TimedTransition(15000, "poopsex"),
                         new PlayerTextTransition("5", "Extra cheese, hold the anchovies.")
                             ),
                    new State("5",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new Taunt("These are my new students please don't kill them :("),
                       new Spawn("DS Orange Turtle", 1, 0, givesNoXp: false),
                        new Spawn("DS Red Turtle", 1, 0, givesNoXp: false),
                         new Spawn("DS Purple Turtle", 1, 0, givesNoXp: false),
                          new Spawn("DS Blue Turtle", 1, 0, givesNoXp: false),
                         new TimedTransition(4000, "6")
                            ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Decay(1000)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Defense", 0.4)
                )
            );
    }
}