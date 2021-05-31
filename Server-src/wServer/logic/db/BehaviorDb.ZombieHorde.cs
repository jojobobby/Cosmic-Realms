using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ xohorde = () => Behav()//by  ppmaks
        .Init("Zombie Archer",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                           new StayBack(0.9, 8)
                    )
                ),
                new Threshold(0.01,
                new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Rogue",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1000),
                       new Wander(0.4),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1500),
                           new Wander(0.6)
                    )
                ),
                new Threshold(0.01,
                 new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
            .Init("Zombie Wizard",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                         new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1350),
                           new Wander(0.4)
                    )
                ),
                new Threshold(0.01,
                new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
            .Init("Zombie Warrior",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Shoot(9, count: 1, projectileIndex: 0, coolDown: 1000),
                          new Follow(0.58, 8, 1),
                        new Wander(0.2)
                    )
                ),
                new Threshold(0.01,
                 new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
                .Init("Zombie Knight",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1000),
                        new Shoot(9, count: 1, projectileIndex: 0, coolDown: 1000),
                          new Follow(0.58, 8, 1),
                        new Wander(0.2)
                    )
                ),
                new Threshold(0.01,
                 new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Paladin",
                new State(
                    new Prioritize(
                        new Follow(0.35, 8, 1),
                        new Wander(0.2)
                        ),
                    new HealSelf(coolDown: 5000),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1000)
                    ),
                new Threshold(0.1,
                new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                    )
            )
             .Init("Zombie Necromancer",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1000),
                         new Wander(0.27),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1350)
                    )
                ),
                new Threshold(0.01,
                new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Huntress",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                            new Wander(0.27),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1350)
                    )
                ),
                new Threshold(0.01,
               new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Mystic",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                            new Wander(0.27),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1350)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Trickster",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                         new Wander(0.6),
                    new StayBack(0.3, 3),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1300)
                    )
                ),
                new Threshold(0.01,
                  new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
             .Init("Zombie Sorcerer",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                        new Follow(0.2, 8, 1),
                         new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 2000),
                           new Wander(0.6)
                    )
                ),
                new Threshold(0.01,
                  new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
          .Init("Zombie Ninja",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                          new Wander(0.6),
                    new StayBack(0.3, 3),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1300)
                    )
                ),
                new Threshold(0.01,
              new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
          .Init("Zombie Assassin",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 1000),
                           new Follow(0.35, 8, 4),
                    new Wander(0.2),
                    new Grenade(3, 140, 4, coolDown: 3000),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1500)
                    )
                ),
                new Threshold(0.01,
                 new ItemLoot("Legendary Crate", 0.001),
                      new ItemLoot("Potion Crate", 0.0925),
                  new ItemLoot("Items Crate", 0.0725),
                    new ItemLoot("Dagger of the Terrible Talon", 0.0625),
                    new ItemLoot("Wand of Ancient Terror", 0.0625),
                 new ItemLoot("Corrupted Cleaver", 0.0625),
                 new ItemLoot("Bow of Nightmares", 0.0625),
                 new ItemLoot("Skull-splitter Sword", 0.0625),
                 new ItemLoot("Candy Corn", 0.6),
                new ItemLoot("Staff of Horrific Knowledge", 0.0625)
                )
            )
            .Init("Zombie Horde",
                new State(
                     new State("0",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                         new PlayerWithinTransition(3, "1")
                    ),
                    new State("1",
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Spawn("Zombie Warrior", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Knight", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Paladin", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Assassin", 1, 0, givesNoXp: false),
                            new TimedTransition(15000, "2")
                    ),
                    new State("2",
                      new ConditionalEffect(ConditionEffectIndex.Invincible),
                      new Spawn("Zombie Necromancer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Huntress", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Mystic", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Trickster", 1, 0, givesNoXp: false),
                          new TimedTransition(15000, "3")
                         ),
                    new State("3",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new Spawn("Zombie Sorcerer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Ninja", 1, 0, givesNoXp: false),
                       new Spawn("Zombie Wizard", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Priest", 1, 0, givesNoXp: false),
                        new TimedTransition(15000, "4")
                            ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new Spawn("Zombie Necromancer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Huntress", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Mystic", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Trickster", 1, 0, givesNoXp: false),
                         new TimedTransition(15000, "5")
                             ),
                    new State("5",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                         new Spawn("Zombie Archer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Rogue", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Wizard", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Warrior", 1, 0, givesNoXp: false),
                         new TimedTransition(4000, "1a")
                          ),
                    new State("1a",
                          new ConditionalEffect(ConditionEffectIndex.Invincible),
                         new Spawn("Zombie Archer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Rogue", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Wizard", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Warrior", 1, 0, givesNoXp: false),
                         new TimedTransition(4000, "1b")
                            ),
                       new State("1b",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                          new Spawn("Zombie Necromancer", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Huntress", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Mystic", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Trickster", 1, 0, givesNoXp: false),
                         new TimedTransition(15000, "1c")
                             ),
                         new State("1c",
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new Spawn("Zombie Warrior", 1, 0, givesNoXp: false),
                        new Spawn("Zombie Knight", 1, 0, givesNoXp: false),
                         new Spawn("Zombie Paladin", 1, 0, givesNoXp: false),
                          new Spawn("Zombie Assassin", 1, 0, givesNoXp: false),
                            new TimedTransition(15000, "6")
                    ),
                    new State("6",
                        new Taunt("Oh no my zombies are gone:("),
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