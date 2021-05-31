using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree, fsod, ???
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TheShatters = () => Behav()
            .Init("shtrs Abandoned Switch 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(1, 192, 192, true)
                    ),
                    new State("2")
                )
            )
            .Init("shtrs Abandoned Switch 5",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(1, 386, 9, true)
                    ),
                    new State("2")
                )
            )
            .Init("shtrs KillWall 1",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntityNotExistsTransition("shtrs Abandoned Switch 1", 5, "2")
                    ),
                    new State("2",
                        new RemoveTileObject(0x717a, 5)
                    )
                )
            )
            .Init("shtrs KillWall 2",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntitiesNotExistsTransition(80, "2", "shtrs Abandoned Switch 2")
                    ),
                    new State("2",
                        new RemoveTileObject(0x717a, 5)
                    )
                )
            )
            .Init("shtrs KillWall 3",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntitiesNotExistsTransition(300, "2", "shtrs Abandoned Switch 3")
                    ),
                    new State("2",
                        new RemoveTileObject(0x717a, 5)
                    )
                )
            )
            .Init("shtrs KillWall 4",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntityNotExistsTransition("shtrs Twilight Archmage", 30, "2")
                    ),
                    new State("2",
                        new RemoveTileObject(0x717a, 5)
                    )
                )
            )
            .Init("shtrs Bridge Closer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new TeleportPlayer(1, 22, 0)
                    ),
                    new State("2",
                        new Decay(1000)
                    )
                )
            )
            .Init("shtrs Spawn Bridge",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1"),
                    new State("2",
                        new SetAltTexture(1),
                        new GroundTransform("shtrs Bridge", 1),
                        //new TeleportPlayer(1, 25, 0)
                        new TeleportPlayer(1, 390, 191, true)
                    )
                )
            )
            .Init("shtrs Player Check Archmage",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("1",
                        new EntitiesNotExistsTransition(120, "2", "shtrs Abandoned Switch 3", "shtrs Abandoned Switch 4")
                    ),
                    new State("2",
                        new SetAltTexture(1),
                        new TeleportPlayer(1, 70, 0)
                    ),
                    new State("3",
                        new Decay(30000)
                    )
                )
            )
            .Init("shtrs Bridge Balloon Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            )
            .Init("shtrs King Balloon Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            )
            .Init("shtrs Loot Balloon King",
                new State(
                    new ScaleHP2(60,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Life", 0.3, 3),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Light Armor Schematic", 0.02, damagebased: true),
                    new ItemLoot("Robe Schematic", 0.02, damagebased: true),
                    new ItemLoot("Heavy Armor Schematic", 0.02, damagebased: true),
                    new ItemLoot("Magic Cards", 0.0006, damagebased: true, threshold: 0.01),
                    new ItemLoot("The Forgotten Crown", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Timeworn Scepter", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Mark of the Forgotten King", 1)//Timeworn Scepter
                )
            )
            .Init("shtrs Loot Balloon Bridge",
                new State(
                    new ScaleHP2(60,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Bracer of the Guardian", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Mark of the Forgotten King", 0.25)
                )
            )
            .Init("shtrs Mage Balloon Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                )
            )
            .Init("shtrs Loot Balloon Mage",
                new State(
                    new ScaleHP2(50,3,15),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Mana", 0.3, 3),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("The Twilight Gemstone", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Sentient Staff", 0.006, damagebased: true),
                    new ItemLoot("The Robe of Twilight", 0.006, damagebased: true),
                    new ItemLoot("Ancient Spell: Pierce", 0.004, damagebased: true),
                    new ItemLoot("The Forgotten Ring", 0.006, damagebased: true),
                    new ItemLoot("Mark of the Forgotten King", 0.25)
                )
            )
            .Init("shtrs Bridge Sentinel",
                new State(
                    new ScaleHP2(60,3,15),
                    new OrderOnDeath(50, "shtrs Soul Spawner", "7"),
                    new OrderOnDeath(50, "shtrs Bridge Balloon Spawner", "2"),
                    new OrderOnDeath(50, "shtrs Spawn Bridge", "2"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(3, "2", true)
                    ),
                    new State("2",
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new OrderOnce(99, "shtrs Bridge Closer", "2"),
                        new OrderOnce(999, "shtrs Abandoned Switch 1", "2"),
                        new TimedTransition(2000,  "4")
                    ),
                    new State("4",
                        new Shoot(0, 7, 15, projectileIndex: 6, fixedAngle: 0, coolDown: 2000),
                        new OrderOnce(24, "shtrs Bridge Obelisk A", "2"),
                        new OrderOnce(24, "shtrs Bridge Obelisk C", "2"),
                        new OrderOnce(24, "shtrs Bridge Obelisk F", "2"),
                        new OrderOnce(24, "shtrs Bridge Obelisk B", "2"),
                        new OrderOnce(24, "shtrs Bridge Obelisk D", "2"),
                        new OrderOnce(24, "shtrs Bridge Obelisk E", "2"), 
                        new EntitiesNotExistsTransition(24, "5", "shtrs Bridge Obelisk A", "shtrs Bridge Obelisk B", "shtrs Bridge Obelisk D", "shtrs Bridge Obelisk E")
                    ),
                    new State("5",
                        new Shoot(0, 7, 15, projectileIndex: 6, fixedAngle: 0, coolDown: 2000),
                        new State("6",
                            new Taunt("Who has woken me...? Leave this place."),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 4500),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 7500),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 8000),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 100000, coolDownOffset: 8500),
                            new TimedTransition(8500, "7")
                        ),
                        new State("7",
                            new Taunt("Go."),
                            new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1500),
                            new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1750),
                            new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 2000),
                            new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 2250),
                            new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 2500),
                            new Shoot(0, 2, 10, 1, 270, coolDown: 100000, coolDownOffset: 2750),
                            new TimedTransition(2750, "8")
                        ),
                        new State("8",
                            new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1500),
                            new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1750),
                            new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 2000),
                            new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 2250),
                            new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 2500),
                            new TimedTransition(2500, "9")
                        ),
                        new State("9",
                            new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1500),
                            new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 1750),
                            new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 2000),
                            new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 2250),
                            new Shoot(0, 2, 10, 1, 270, coolDown: 100000, coolDownOffset: 2500),
                            new TimedTransition(2500, "10")
                        ),
                        new State("10",
                            new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1500),
                            new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1750),
                            new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 2000),
                            new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 2250),
                            new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 2500),
                            new TimedTransition(2500, "11")
                        ),
                        new State("11",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("Good. Pests must be gone."),
                            new HpLessTransition(0.8, "12")
                        ),
                        new State("12",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt("You live still? DO NOT TEMPT FATE!"),
                            new TimedTransition(5000, "13")
                        ),
                        new State("13",
                            new OrderOnce(50, "shtrs Soul Spawner", "2"),
                            new Taunt("CONSUME!"),
                            new TimedTransition(5000, "14")
                        ),
                        new State("14",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new OrderOnce(50, "shtrs Soul Spawner", "3"),
                            new HpLessTransition(0.4, "15")
                        ),
                        new State("15",
                            new Taunt("FOOLS! YOU DO NOT UNDERSTAND!"),
                            new OrderOnce(50, "shtrs Soul Spawner", "1"),
                            new Shoot(0, 18, 10, 0, 180, coolDown: 1000),
                            new HpLessTransition(0.1, "20"),
                            new State("16",
                                new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 250),
                                new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 500),
                                new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 750),
                                new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1000),
                                new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1250),
                                new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1500),
                                new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1750),
                                new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 2000),
                                new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 2250),
                                new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 2500),
                                new Shoot(0, 2, 10, 1, 270, coolDown: 100000, coolDownOffset: 2750),
                                new TimedTransition(2750, "17")
                            ),
                            new State("17",
                                new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 250),
                                new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 500),
                                new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 750),
                                new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1000),
                                new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                                new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1500),
                                new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1750),
                                new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 2000),
                                new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 2250),
                                new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 2500),
                                new TimedTransition(2500, "18")
                            ),
                            new State("18",
                                new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 250),
                                new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 500),
                                new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 750),
                                new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1000),
                                new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                                new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1500),
                                new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 1750),
                                new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 2000),
                                new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 2250),
                                new Shoot(0, 2, 10, 1, 270, coolDown: 100000, coolDownOffset: 2500),
                                new TimedTransition(2500, "19")
                            ),
                            new State("19",
                                new Shoot(0, 2, 10, 1, 252, coolDown: 100000, coolDownOffset: 250),
                                new Shoot(0, 2, 10, 1, 234, coolDown: 100000, coolDownOffset: 500),
                                new Shoot(0, 2, 10, 1, 216, coolDown: 100000, coolDownOffset: 750),
                                new Shoot(0, 2, 10, 1, 198, coolDown: 100000, coolDownOffset: 1000),
                                new Shoot(0, 2, 10, 1, 180, coolDown: 100000, coolDownOffset: 1250),
                                new Shoot(0, 2, 10, 1, 162, coolDown: 100000, coolDownOffset: 1500),
                                new Shoot(0, 2, 10, 1, 144, coolDown: 100000, coolDownOffset: 1750),
                                new Shoot(0, 2, 10, 1, 126, coolDown: 100000, coolDownOffset: 2000),
                                new Shoot(0, 2, 10, 1, 108, coolDown: 100000, coolDownOffset: 2250),
                                new Shoot(0, 2, 10, 1, 90, coolDown: 100000, coolDownOffset: 2500),
                                new TimedTransition(2500, "18")
                            )
                        ),
                        new State("20",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Taunt("I tried to protect you... I have failed. You release a great evil upon this Realm..."),
                            new OrderOnce(50, "shtrs Soul Spawner", "2"),
                            new Shoot(0, 14, coolDown: 100000, coolDownOffset: 10000),
                            new Decay(10000)
                        )
                    )
                ),
                    new Threshold(0.01,
                            new ItemLoot("Potion of Attack", 0.3, 3),
                            new ItemLoot("Potion of Defense", 0.3, 3),
                            new TierLoot(11, ItemType.Weapon, 0.0625),
                            new TierLoot(12, ItemType.Weapon, 0.03125),
                            new TierLoot(12, ItemType.Armor, 0.0625),
                            new TierLoot(13, ItemType.Armor, 0.03125),
                            new TierLoot(6, ItemType.Armor, 0.03125),
                            new TierLoot(6, ItemType.Ring, 0.03125),
                            new ItemLoot("50 Credits", 0.01),
                            new ItemLoot("Potion of Critical Chance", 0.02),
                            new ItemLoot("Potion of Critical Damage", 0.02),
                            new ItemLoot("Bracer of the Guardian", 0.004, damagebased: true, threshold: 0.01),
                            new ItemLoot("Mark of the Forgotten King", 0.25)
                        )
            )
            .Init("shtrs Soul Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invisible),
                    new State("1"),
                    new State("2",
                        new Spawn("shtrs Blobomb", 1, 0)
                    ),
                    new State("3",
                        new Spawn("shtrs Blobomb", 1, 0),
                        new TimedTransition(250, "4")
                    ),
                    new State("4",
                        new Spawn("shtrs Blobomb", 1, 0),
                        new TimedTransition(250, "5")
                    ),
                    new State("5",
                        new Spawn("shtrs Blobomb", 1, 0),
                        new TimedTransition(250, "6")
                    ),
                    new State("6",
                        new TimedTransition(4000, "3")
                    ),
                    new State("7",
                        new Decay(1000)
                    )
                )
            )
            .Init("shtrs Blobomb",
                new State(
                    new State("active",
                        new Follow(.7, 30, range: 1),
                        new PlayerWithinTransition(2, "blink")
                    ),
                    new State("blink",
                        new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                        new TimedTransition(2000, "explode")
                    ),
                    new State("explode",
                        new Flash(0xfFF0000, flashRepeats: 5, flashPeriod: 0.1),
                        new Shoot(15, 20),
                        new Decay(100)
                    )
                )
            )
            .Init("shtrs Bridge Obelisk A",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Knight", 1, 0),
                        new Spawn("shtrs Stone Mage", 1, 0),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Bridge Obelisk C",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Paladin", 1, 0),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Bridge Obelisk F",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("shtrs Stone Paladin", 1, 0),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Bridge Obelisk B",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new State("8",
                            new EntityNotExistsTransition("shtrs Bridge Obelisk A", 20, "9")
                        ),
                        new State("9",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Spawn("shtrs Stone Knight", 1, 0),
                            new Spawn("shtrs Stone Mage", 1, 0)
                        ),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Bridge Obelisk D",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new State("8",
                            new EntityNotExistsTransition("shtrs Bridge Obelisk B", 20, "9")
                        ),
                        new State("9",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Spawn("shtrs Stone Knight", 1, 0),
                            new Spawn("shtrs Stone Mage", 1, 0)
                        ),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Bridge Obelisk E",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                    ),
                    new State("2",
                        new Taunt("DO NOT WAKE THE BRIDGE GUARDIAN!"),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xFF0000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Shoot(0, 4, 90, 0, 45, coolDown: 200),
                        new TimedTransition(8000, "5")
                    ),
                    new State("5",
                        new State("8",
                            new EntityNotExistsTransition("shtrs Bridge Obelisk D", 20, "9")
                        ),
                        new State("9",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Spawn("shtrs Stone Knight", 1, 0),
                            new Spawn("shtrs Stone Mage", 1, 0)
                        ),
                        new TimedTransition(10000, "3")
                    )
                )
            )
            .Init("shtrs Twilight Archmage",
                new State(
                    new ScaleHP2(70,3,15),
                    new OrderOnDeath(50, "shtrs Mage Balloon Spawner", "2"),
                    new OrderOnDeath(50, "shtrs MagiGenerators", "4"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new EntitiesNotExistsTransition(20, "2", "shtrs Archmage of Flame", "shtrs Glassier Archmage")
                    ),
                    new State("2",
                        new Taunt("Ha... ha... hahahahahaha! You will make a fine sacrifice!"),
                        new Order(200, "shtrs Player Check Archmage", "3"),
                        new SetAltTexture(1),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new Taunt("You will find that it was... unwise..to wake me."),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Taunt("How best to kill you...", "Let us see what I can conjure up!", "What shall I use to sacrifice you?", "Ahh... lets see..."),
                        new TimedTransition(1000, "5")
                    ),
                    new State("5",
                        new HpLessTransition(0.5, "11"),
                        new OrderOnce(999, "shtrs Spawn Bridge", "1"),
                        new State("6",
                            new SetAltTexture(1),
                            new TimedRandomTransition(100, false, "7", "9")
                        ),
                        new State("7",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("Toahc Xifls’TOh Merilmeril Qualtinoc!", "I will freeze the life from you!", "Ahh... Ice should do the trick."),
                            new TimedTransition(3000, "8")
                        ),
                        new State("8",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new SetAltTexture(2),
                            new TossObject("shtrs Ice Portal", 20, coolDown: 100000, throwEffect: true),
                            new Spawn("shtrs Ice Shield 2", 1, 0),
                            new TimedTransition(10000, "6")
                        ),
                        new State("9",
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("Firos Xifls’TOh Gantsualo Quantinoftus!", "The fire I command will consume you!", "Burning. This will suffice."),
                            new TimedTransition(3000, "10")
                        ),
                        new State("10",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Firebomb", range: 20, minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 20, coolDown: 100000, throwEffect: true),
                            new TossObject("shtrs Fire Portal", 20, coolDown: 100000, throwEffect: true),
                            new Shoot(20, 5, 7, 1, coolDown: 250),
                            new TimedTransition(10000, "6")
                        )
                    ),
                    new State("11",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("You leave me no choice... Inferno! Blizzard!"),
                        new TimedTransition(3000, "12")
                    ),
                    new State("12",
                        new OrderOnce(50, "shtrs MagiGenerators", "2"),
                        new TossObject("shtrs Inferno", 3, 270, coolDown: 100000, tossInvis: true),
                        new TossObject("shtrs Blizzard", 3, 90, coolDown: 100000, tossInvis: true),
                        new TimedTransition(5000, "13")
                    ),
                    new State("13",
                        new EntitiesNotExistsTransition(30, "14", "shtrs Inferno", "shtrs Blizzard")
                    ),
                    new State("14",
                        new SetAltTexture(2),
                        new ChangeSize(50, 200),
                        new Taunt("Darkness give me strength!"),
                        new TimedTransition(3000, "15")
                    ),
                    new State("15",
                        new OrderOnce(50, "shtrs MagiGenerators", "3"),
                        new SetAltTexture(1),
                        new HpLessTransition(0.1, "30"),
                        new State("16",
                            new MoveLine(1, 90, 7),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(5000, "17")
                        ),
                        new State("17",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "18")
                        ),
                        new State("18",
                            new MoveLine(1, 0, 4),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "19")
                        ),
                        new State("19",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "20")
                        ),
                        new State("20",
                            new MoveLine(1, 0, 4),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "21")
                        ),
                        new State("21",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "22")
                        ),
                        new State("22",
                            new MoveLine(1, 270, 15),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "23")
                        ),
                        new State("23",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "24")
                        ),
                        new State("24",
                            new MoveLine(1, 180, 4),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "25")
                        ),
                        new State("25",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "26")
                        ),
                        new State("26",
                            new MoveLine(1, 180, 4),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "27")
                        ),
                        new State("27",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "28")
                        ),
                        new State("28",
                            new ReturnToSpawn(1, 0),
                            new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000),
                            new TimedTransition(5000, "29")
                        ),
                        new State("29",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 3750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 4250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 8750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 9250),
                            new Shoot(0, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12250),
                            new Shoot(0, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12500),
                            new Shoot(0, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 100000, coolDownOffset: 12750),
                            new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13000),
                            new Shoot(0, 20, projectileIndex: 6, fixedAngle: 0, coolDown: 100000, coolDownOffset: 13250),
                            new TimedTransition(15250, "16")
                        )
                    ),
                    new State("30",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("I... will... retuuurrnnnn..."),
                        new Flash(0xFF0000, 1, 1),
                        new Shoot(0, 20, projectileIndex: 5, fixedAngle: 0, coolDown: 100000, coolDownOffset: 10000),
                        new Decay(10000)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Potion of Mana", 0.3, 3),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.03125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new TierLoot(13, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Armor, 0.03125),
                    new TierLoot(6, ItemType.Ring, 0.03125),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("The Twilight Gemstone", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Sentient Staff", 0.006, damagebased: true),
                    new ItemLoot("The Robe of Twilight", 0.006, damagebased: true),
                    new ItemLoot("Ancient Spell: Pierce", 0.004, damagebased: true),
                    new ItemLoot("The Forgotten Ring", 0.006, damagebased: true),
                    new ItemLoot("Mark of the Forgotten King", 0.25)
                )
            )
            .Init("shtrs MagiGenerators",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Shoot(3, 10, coolDown: 1500),
                        new Shoot(15, 1, projectileIndex: 1, coolDown: 2500)
                    ),
                    new State("2",
                        new SetAltTexture(1)
                    ),
                    new State("3",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(3, 10, coolDown: 1500),
                        new Shoot(15, 1, projectileIndex: 1, coolDown: 2500)
                    ),
                    new State("4",
                        new Shoot(15, 10, coolDown: 1000)
                    )
                )
            )
            .Init("shtrs The Cursed Crown",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("1",
                        new PlayerWithinTransition(1, "2", true)
                    ),
                    new State("2",
                        new OrderOnce(50, "shtrs Royal Guardian L", "2"),
                        new EntitiesNotExistsTransition(999, "3", "shtrs Royal Guardian L")
                    ),
                    new State("3",
                        new MoveLine(1, 270, 12),
                        new Taunt("..."),
                        new TimedTransition(5000, "4")
                    ),
                    new State("4",
                        new PlayerWithinTransition(7, "5")
                    ),
                    new State("5",
                        new Taunt("You have made a grave mistake coming here. I will destroy you, and reclaim my place in the Realm."),
                        new SetAltTexture(3),
                        new TimedTransition(500, "6")
                    ),
                    new State("6",
                        new SetAltTexture(4),
                        new TimedTransition(500, "7")
                    ),
                    new State("7",
                        new SetAltTexture(5),
                        new Flash(0xFF0000, 0.5, 8),
                        new TimedTransition(2000, "8")
                    ),
                    new State("8",
                        new Transform("shtrs The Forgotten King")
                    )
                )
            )
            .Init("shtrs Royal Guardian L",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable)
                    ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Follow(0.6, 8, 5),
                        new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0, coolDown: 2500),
                        new Shoot(10, projectileIndex: 1)
                    )
                )
            )
            .Init("shtrs Royal Guardian J",
                new State(
                    new State("shoot",
                        new Shoot(0, 8, projectileIndex: 0, fixedAngle: 0, coolDown: 2000),
                        new State("random_orbit",
                            new TimedRandomTransition(500, true, "1", "2", "3")
                        ),
                        new State("1",
                            new Orbit(0.6, 2, 5, "shtrs The Forgotten King")
                        ),
                        new State("2",
                            new Orbit(0.6, 1, 5, "shtrs The Forgotten King", orbitClockwise: true)
                        ),
                        new State("3",
                            new Orbit(0.4, 1.5, 5, "shtrs The Forgotten King", orbitClockwise: false)
                        )
                    )
                )
            )
            .Init("shtrs The Forgotten King",
                new State(
                    new ScaleHP2(80,3,15),
                    new OrderOnDeath(99, "shtrs King Balloon Spawner", "2"),
                    new OrderOnDeath(99, "shtrs Goo Spawner", "4"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true), //order close teleporter
                        new TimedTransition(1000, "2")
                    ),
                    new State("2",
                        new Spawn("shtrs Green Crystal", 1, 0),
                        new Spawn("shtrs Yellow Crystal", 1, 0),
                        new Spawn("shtrs Red Crystal", 1, 0),
                        new Spawn("shtrs Blue Crystal", 1, 0),
                        new TimedTransition(4000, "3")
                    ),
                    new State("3",
                        new Taunt("I will make quick work of you. Be gone."),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new OrderOnce(20, "shtrs Green Crystal", "2"),
                        new OrderOnce(20, "shtrs Yellow Crystal", "2"),
                        new OrderOnce(20, "shtrs Red Crystal", "2"),
                        new OrderOnce(20, "shtrs Blue Crystal", "2"),
                        new OrderOnce(999, "shtrs Abandoned Switch 5", "2"),
                        new EntitiesNotExistsTransition(999, "5", "shtrs Green Crystal", "shtrs Yellow Crystal", "shtrs Red Crystal", "shtrs Blue Crystal")
                    ),
                    new State("5",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("You would challenge me? I am the King of this place."),
                        new ChangeSize(50, 150),
                        new Shoot(24, 2, 0, 3, coolDown: 500),
                        new Shoot(24, 2, 0, 2, coolDown: 500),
                        new Shoot(0, 2, 180, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(0, 2, 170, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(0, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(0, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(0, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(0, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(0, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(0, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(0, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(0, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(0, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(0, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(0, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(0, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(0, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(0, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(0, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(0, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(0, 3, 7, 1, 90, coolDown: 4250, coolDownOffset: 2250),
                        new Shoot(0, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(0, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(0, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(0, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(0, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(0, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(0, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(0, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(0, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(0, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(0, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(0, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(0, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(0, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(0, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new Shoot(0, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new HpLessTransition(0.8, "6")
                    ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new MoveLine(1, 90, 12),
                        new TimedTransition(5000, "7")
                    ),
                    new State("7",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("You fools. Guards, surround me.", "Guards, come to my aid!", "Do not think, even for a moment, that you will best me."),
                        new Spawn("shtrs Royal Guardian J", 7, 0),
                        new TimedTransition(5000, "8")
                    ),
                    new State("8",
                        new EntitiesNotExistsTransition(20, "9", "shtrs Royal Guardian J")
                    ),
                    new State("9",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt("Be consumed, you wretched whelps.", "Laughable attempt.", "Do not mistake your success so far as progress."),
                        new Shoot(24, 2, 0, 3, coolDown: 1000),
                        new Shoot(24, 2, 0, 2, coolDown: 1000),
                        new Shoot(0, 8, projectileIndex: 1, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1000),
                        new Shoot(0, 10, projectileIndex: 1, fixedAngle: 0, coolDown: 100000, coolDownOffset: 1500),
                        new Shoot(0, 12, projectileIndex: 1, fixedAngle: 0, coolDown: 100000, coolDownOffset: 2000),
                        new HpLessTransition(0.6, "10"),
                        new TimedTransition(3000, "7")
                    ),
                    new State("10",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("So. You are still here. Adorable. Perhaps a nice dip in the LAVA!?"),
                        new TimedTransition(3000, "11")
                    ),
                    new State("11", //przez 25s 180 stopni 100 razy
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("TENTACLES OF WRATH!"),
                        new Shoot(0, 6, projectileIndex: 4, fixedAngle: 0, rotateAngle: 1.44, coolDown: 200),
                        new Shoot(0, 6, projectileIndex: 4, fixedAngle: 5, rotateAngle: 1.44, coolDown: 200),
                        new State("12",
                            new Shoot(24, 2, 0, 3, coolDown: 500),
                            new Shoot(24, 2, 0, 2, coolDown: 500),
                            new TimedTransition(5000, "13")
                        ),
                        new State("13",
                            new Taunt("Fools..."),
                            new OrderOnce(99, "shtrs Goo Spawner", "2")
                        ),
                        new TimedTransition(25000, "14")
                    ),
                    new State("14",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new HpLessTransition(0.4, "15"),
                        new TimedTransition(5000, "11")
                    ),
                    new State("15",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new ReturnToSpawn(1, 0),
                        new Taunt("Drown, and be swallowed by those who have failed before."),
                        new TimedTransition(5000, "16")
                    ),
                    new State("16",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Shoot(24, 2, 0, 3, coolDown: 500),
                        new Shoot(24, 2, 0, 2, coolDown: 500),
                        new Shoot(0, 2, 180, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(0, 2, 170, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(0, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(0, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(0, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(0, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(0, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(0, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(0, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(0, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(0, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(0, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(0, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(0, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(0, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(0, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(0, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(0, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(0, 3, 7, 1, 90, coolDown: 4250, coolDownOffset: 2250),
                        new Shoot(0, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(0, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(0, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(0, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(0, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(0, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(0, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(0, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(0, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(0, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(0, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(0, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(0, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(0, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(0, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new Shoot(0, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new HpLessTransition(0.2, "17")
                    ),
                    new State("17",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("YOU TEST THE PATIENCE OF A GOD!"),
                        new TimedTransition(4000, "18")
                    ),
                    new State("18",
                        new Taunt("DIE! DIE!! DIE!!!"),
                        new Shoot(10, 2, 0, 3, coolDown: 500),
                        new Shoot(10, 2, 0, 2, coolDown: 500),
                        new Shoot(10, 2, 180, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(10, 2, 170, 1, 90, coolDown: 4250, coolDownOffset: 0),
                        new Shoot(10, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(10, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 250),
                        new Shoot(10, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(10, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 500),
                        new Shoot(10, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(10, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 750),
                        new Shoot(10, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(10, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 1000),
                        new Shoot(10, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(10, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 1250),
                        new Shoot(10, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(10, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 1500),
                        new Shoot(10, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(10, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 1750),
                        new Shoot(10, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(10, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2000),
                        new Shoot(10, 3, 7, 1, 90, coolDown: 4250, coolDownOffset: 2250),
                        new Shoot(10, 2, 10, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(10, 2, 20, 1, 90, coolDown: 4250, coolDownOffset: 2500),
                        new Shoot(10, 2, 30, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(10, 2, 40, 1, 90, coolDown: 4250, coolDownOffset: 2750),
                        new Shoot(10, 2, 50, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(10, 2, 60, 1, 90, coolDown: 4250, coolDownOffset: 3000),
                        new Shoot(10, 2, 70, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(10, 2, 80, 1, 90, coolDown: 4250, coolDownOffset: 3250),
                        new Shoot(10, 2, 90, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(10, 2, 100, 1, 90, coolDown: 4250, coolDownOffset: 3500),
                        new Shoot(10, 2, 110, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(10, 2, 120, 1, 90, coolDown: 4250, coolDownOffset: 3750),
                        new Shoot(10, 2, 130, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(10, 2, 140, 1, 90, coolDown: 4250, coolDownOffset: 4000),
                        new Shoot(10, 2, 150, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new Shoot(10, 2, 160, 1, 90, coolDown: 4250, coolDownOffset: 4250),
                        new Shoot(10, 6, projectileIndex: 4, fixedAngle: 0, rotateAngle: 1.44, coolDown: 200),
                        new Shoot(10, 6, projectileIndex: 4, fixedAngle: 5, rotateAngle: 1.44, coolDown: 200),
                        new TimedTransition(25000, "19")
                    ),
                    new State("19",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Taunt(true, "Ha... haa..."),
                        new HpLessTransition(0.1, "20"),
                        new TimedTransition(10000, "18")
                    ),
                    new State("20",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt("Impossible... IMPOSSIBLE!!!"),
                        new Flash(0xFF0000, 0.5, 8),
                        new Shoot(0, 30, projectileIndex: 0, fixedAngle: 0, coolDown: 10000, coolDownOffset: 2000),
                        new Decay(2000)
                    )
                ),
                new Threshold(0.01,
                        new ItemLoot("Potion of Life", 0.3, 3),
                        new TierLoot(11, ItemType.Weapon, 0.0625),
                        new TierLoot(12, ItemType.Weapon, 0.03125),
                        new TierLoot(12, ItemType.Armor, 0.0625),
                        new TierLoot(13, ItemType.Armor, 0.03125),
                        new TierLoot(6, ItemType.Armor, 0.03125),
                        new TierLoot(6, ItemType.Ring, 0.03125),
                        new ItemLoot("50 Credits", 0.01),
                        new ItemLoot("Potion of Critical Chance", 0.02),
                        new ItemLoot("Potion of Critical Damage", 0.02),
                        new ItemLoot("Light Armor Schematic", 0.02, damagebased: true),
                        new ItemLoot("Robe Schematic", 0.02, damagebased: true),
                        new ItemLoot("Heavy Armor Schematic", 0.02, damagebased: true),
                        new ItemLoot("Magic Cards", 0.0006, damagebased: true, threshold: 0.01),
                        new ItemLoot("The Forgotten Crown", 0.001, damagebased: true, threshold: 0.01),
                        new ItemLoot("Timeworn Scepter", 0.001, damagebased: true, threshold: 0.01),
                        new ItemLoot("Mark of the Forgotten King", 1)//Timeworn Scepter
                    )

            )
            .Init("shtrs Goo Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invisible),
                    new State("1"),
                    new State("2",
                        new Spawn("shtrs Lava Souls", 1, 0),
                        new TimedTransition(100, "3")
                    ),
                    new State("3",
                        new TimedTransition(12000, "2")
                    ),
                    new State("4",
                        new Decay(1000)
                    )
                )
            )
            .Init("shtrs Lava Souls",
                new State(
                    new State("1",
                        new Follow(0.1, 20, 0),
                        new PlayerWithinTransition(2, "2")
                    ),
                    new State("2",
                        new Flash(0xFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new Flash(0xFF0000, flashRepeats: 5, flashPeriod: 0.1),
                        new Shoot(0, 9, fixedAngle: 0),
                        new Decay(100)
                    )
                )
            )
             .Init("shtrs Stone Paladin",
                new State(
                    new State("Attacking",
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 100, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 110, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 120, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 130, coolDownOffset: 800, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 140, coolDownOffset: 1000, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 150, coolDownOffset: 1200, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 1800, shootAngle: 90),
                        new Shoot(0, 4, coolDown: 10000, fixedAngle: 90, coolDownOffset: 2000, shootAngle: 45),
                        new TimedTransition(2000, "Wait")
                     ),
                     new State("Wait",
                         new Follow(0.4, range: 2),
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new TimedTransition(2000, "Attacking")
                     )
               )
            )
            .Init("shtrs Titanum",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new State("1",
                        new PlayerWithinTransition(6, "2")
                     ),
                     new State("2",
                         new Spawn("shtrs Stone Knight", 1, 0),
                         new Spawn("shtrs Stone Mage", 1, 0)
                     )
                )
            )
            .Init("shtrs Paladin Obelisk",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Armored),
                    new State("1",
                        new PlayerWithinTransition(6, "2")
                     ),
                     new State("2",
                         new Spawn("shtrs Stone Paladin", 1, 0)
                     )
               )
            )
            .Init("shtrs Ice Mage",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "2", true)
                     ),
                     new State("2",
                         new Follow(0.3, 10, 1),
                         new Shoot(10, 5, 10, coolDown: 1500),
                         new TimedTransition(15000, "3")
                     ),
                     new State("3",
                         new Spawn("shtrs Ice Shield", 1, 0),
                         new TimedTransition(4000, "4")
                     ),
                     new State("4",
                         new Follow(0.3, 10, 1),
                         new Shoot(10, 5, 10, coolDown: 1500)
                     )
               )
            )
            .Init("shtrs Ice Adept",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "2", true)
                     ),
                     new State("2",
                         new Prioritize(
                            new Follow(0.5, 10, 3),
                            new Orbit(0.5, 2)
                         ),
                         new State("Random_Shoot1",
                            new Shoot(10, 1, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot2", "Random_Shoot3")
                            ),
                        new State("Random_Shoot2",
                            new Shoot(10, 2, 10, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot1", "Random_Shoot3")
                            ),
                        new State("Random_Shoot3",
                            new Shoot(10, 3, 10, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot1", "Random_Shoot2")
                         ),
                         new Shoot(10, 8, 22.5, 1, coolDown: 4000),
                         new TimedTransition(10000, "3")
                     ),
                     new State("3",
                         new TossObject("shtrs Ice Portal", 10, coolDown: 100000, throwEffect: true),
                         new TimedTransition(4000, "4")
                     ),
                     new State("4",
                         new Prioritize(
                            new Follow(0.5, 10, 3),
                            new Orbit(0.5, 2)
                         ),
                         new State("Random_Shoot1",
                            new Shoot(10, 1, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot2", "Random_Shoot3")
                            ),
                        new State("Random_Shoot2",
                            new Shoot(10, 2, 10, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot1", "Random_Shoot3")
                            ),
                        new State("Random_Shoot3",
                            new Shoot(10, 3, 10, projectileIndex: 0, coolDown: 999999, predictive: 1),
                            new TimedRandomTransition(1000, false, "Random_Shoot1", "Random_Shoot2")
                         ),
                         new Shoot(10, 8, 22.5, 1, coolDown: 4000)
                     )
               )
            )
            .Init("shtrs Glassier Archmage",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "2", true)
                     ),
                     new State("2",
                         new Prioritize(
                             new StayBack(0.3, 5),
                             new Follow(0.5, range: 7)
                         ),
                         new Shoot(20, projectileIndex: 2, coolDown: 400),
                         new Shoot(20, 18, 20, 1, 0, coolDown: 2500),
                         new TimedTransition(10000, "3")
                     ),
                     new State("3",
                         new ConditionalEffect(ConditionEffectIndex.Armored, true),
                         new SetAltTexture(1),
                         new Shoot(0, 25, fixedAngle: 0, coolDown: 5000),
                         new TimedTransition(2000, "4")
                     ),
                     new State("4",
                         new StayBack(0.3, 5),
                         new Shoot(20, projectileIndex: 2, coolDown: 400),
                         new Shoot(20, 18, 20, 1, 0, coolDown: 2500),
                         new TimedTransition(10000, "5")
                     ),
                     new State("5",
                         new RemoveConditionalEffect(ConditionEffectIndex.Armored),
                         new SetAltTexture(0),
                         new Shoot(0, 25, fixedAngle: 0, coolDown: 5000),
                         new TimedTransition(2000, "2")
                     )
               )
            )
            .Init("shtrs Fire Mage",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "2", true)
                     ),
                     new State("2",
                         new Follow(0.3, range: 3),
                         new Shoot(10, 5, 7, projectileIndex: 1, coolDown: 250),
                         new Shoot(10, 5, projectileIndex: 0, coolDown: 2500)
                     )
               )
            )
            .Init("shtrs Fire Adept",
                new State(
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(6, "2", true)
                     ),
                     new State("2",
                         new Follow(0.3, range: 3),
                         new Shoot(10, 5, 7, projectileIndex: 1, coolDown: 250),
                         new TimedTransition(10000, "3")
                     ),
                     new State("3",
                         new TossObject("shtrs Fire Portal", 10, coolDown: 100000, throwEffect: true),
                         new TimedTransition(4000, "4")
                     ),
                     new State("4",
                         new Follow(0.3, range: 3),
                         new Shoot(10, 5, 7, projectileIndex: 1, coolDown: 250)
                     )
               )
            )
            .Init("shtrs Archmage of Flame",
                new State(
                    new State("wait",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ConditionalEffect(ConditionEffectIndex.Petrify),
                        new PlayerWithinTransition(6, "Follow")
                    ),
                    new State("Follow",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Follow(0.5, range: 4),
                        new TimedRandomTransition(10000, false, "Throw", "Fire"),
                        new PlayerWithinTransition(5, "Random_State")
                    ),
                    new State("Random_State",
                        new TimedRandomTransition(1000, false, "Throw", "Fire")
                    ),
                    new State("Throw",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TossObject("shtrs Firebomb", minAngle: 0, maxAngle: 360, minRange: 0, maxRange: 6, coolDown: 100000, throwEffect: true),
                        new TimedTransition(5000, "Follow")
                    ),
                    new State("Fire",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Shoot(0, 8, fixedAngle: 0, rotateAngle: 22.5, coolDown: 300),
                        new TimedTransition(5000, "Follow")
                    )
                )
            )
            .Init("shtrs Stone Mage",
                new State(
                    new Follow(0.3, range: 3),
                    new Shoot(10, 2, 0, projectileIndex: 1, coolDown: 250)
               )
            )
            .Init("shtrs Stone Knight",
                new State(
                    new Prioritize(
                        new Charge(4, 5, coolDown: 5000),
                        new Follow(0.36, range: 3)
                    ),
                    new Shoot(5, 6, projectileIndex: 0, coolDown: 1000)
               )
            )
            .Init("shtrs Ice Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(1000, "Spin")
                    ),
                    new State("Spin",
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 2700),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 2700, coolDownOffset: 250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 2700, coolDownOffset: 500),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 2700, coolDownOffset: 750),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 2700, coolDownOffset: 1000),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 75, coolDown: 2700, coolDownOffset: 1250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 2700, coolDownOffset: 1500),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 2700, coolDownOffset: 1750),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 2700, coolDownOffset: 2000),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 2700, coolDownOffset: 2250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 2700, coolDownOffset: 2500),
                        new TimedTransition(2700, "Boom")
                    ),
                    new State("Boom",
                        new Shoot(0, 36, fixedAngle: 0),
                        new Decay(500)
                    )
                )
            )
            .Init("shtrs Fire Portal",
                new State(
                    new State("Idle",
                        new TimedTransition(1000, "Spin")
                    ),
                    new State("Spin",
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 2700),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 2700, coolDownOffset: 250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 2700, coolDownOffset: 500),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 2700, coolDownOffset: 750),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 2700, coolDownOffset: 1000),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 75, coolDown: 2700, coolDownOffset: 1250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 2700, coolDownOffset: 1500),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 2700, coolDownOffset: 1750),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 2700, coolDownOffset: 2000),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 2700, coolDownOffset: 2250),
                        new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 2700, coolDownOffset: 2500),
                        new TimedTransition(2700, "Boom")
                    ),
                    new State("Boom",
                        new Shoot(0, 36, fixedAngle: 0),
                        new Decay(500)
                    )
                )
            )
            .Init("shtrs Firebomb",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Idle",
                        new TimedTransition(2200, "Explode")
                    ),
                    new State("Explode",
                        new Aoe(2, true, 50, 100, false, 0xFF0000),
                        new Shoot(0, projectileIndex: 0, count: 8, fixedAngle: 0),
                        new Decay(100)
                    )
                )
            )
            .Init("shtrs Inferno",
                new State(
                    new Orbit(0.6, 4, 15, "shtrs Blizzard"),
                    new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 4000),
                    new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 2500),
                    new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 7000),
                    new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 90, coolDown: 5000)
                )
            )

            .Init("shtrs Blizzard",
                new State(
                    new State("Follow",
                        new Follow(0.3, range: 3),
                        new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 0, rotateAngle: 45, coolDown: 500),
                        new TimedTransition(7000, "Spin")
                    ),
                    new State("Spin",
                        new State("Quadforce1",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(200, "Quadforce2")
                        ),
                        new State("Quadforce2",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(200, "Quadforce3")
                        ),
                        new State("Quadforce3",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(200, "Quadforce4")
                        ),
                        new State("Quadforce4",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(200, "Quadforce5")
                        ),
                        new State("Quadforce5",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(200, "Quadforce6")
                        ),
                        new State("Quadforce6",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(200, "Quadforce7")
                        ),
                        new State("Quadforce7",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(200, "Quadforce8")
                        ),
                        new State("Quadforce8",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(200, "Quadforce9")
                        ),
                        new State("Quadforce9",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 120, coolDown: 300),
                            new TimedTransition(200, "Quadforce10")
                        ),
                        new State("Quadforce10",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 135, coolDown: 300),
                            new TimedTransition(200, "Quadforce11")
                        ),
                        new State("Quadforce11",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 150, coolDown: 300),
                            new TimedTransition(200, "Quadforce12")
                        ),
                        new State("Quadforce12",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 165, coolDown: 300),
                            new TimedTransition(200, "Quadforce13")
                        ),
                        new State("Quadforce13",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 180, coolDown: 300),
                            new TimedTransition(200, "Quadforce14")
                        ),
                        new State("Quadforce14",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 195, coolDown: 300),
                            new TimedTransition(200, "Quadforce15")
                        ),
                        new State("Quadforce15",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 210, coolDown: 300),
                            new TimedTransition(200, "Quadforce16")
                        ),
                        new State("Quadforce16",
                            new Shoot(0, projectileIndex: 0, count: 6, shootAngle: 60, fixedAngle: 225, coolDown: 300),
                            new TimedTransition(200, "Follow")
                            )
                        )
                )
            )
            .Init("shtrs Ice Shield 2",
                new State(
                    new State(
                        new HpLessTransition(.2, "charge"),
                        new Orbit(0.5, 1, 10, "shtrs Twilight Archmage"),
                        new Charge(0.6, 7, coolDown: 5000),
                        new Shoot(0, 10, projectileIndex: 0, coolDown: 1000, fixedAngle: 0),
                        new TimedTransition(10000, "charge")
                    ),
                    new State("charge",
                        new Charge(1, 15, coolDown: 5000),
                        new TimedTransition(2500, "Death"),
                        new PlayerWithinTransition(2, "Death")
                    ),
                    new State("Death",
                        new Shoot(13, 25, 14.4, projectileIndex: 1, fixedAngle: 1, coolDown: 10000),
                        new Decay(500)
                    )
                )
            )
            .Init("shtrs Green Crystal",
                new State(
                    new State("orbit",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 1, 5, "shtrs The Forgotten King")
                    ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new HealGroup(8, "Crystals", 3500, 1000),
                        new Orbit(0.6, 2, 10, null)
                    )
                )
            )
            .Init("shtrs Yellow Crystal",
                new State(
                    new State("orbit",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 1, 5, "shtrs The Forgotten King", orbitClockwise: true)
                    ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 2, 10, null, orbitClockwise: true),
                        new Shoot(5, 4, 4, projectileIndex: 0)
                    )
                )
            )
            .Init("shtrs Red Crystal",
                new State(
                    new State("orbit",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 1, 5, "shtrs The Forgotten King")
                    ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 2, 5, null),
                        new TossObject("shtrs Fire Portal", 10, coolDown: 15000, throwEffect: true)
                    )
                )
            )
            .Init("shtrs Blue Crystal",
                new State(
                    new State("orbit",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 1, 5, "shtrs The Forgotten King", orbitClockwise: true)
                    ),
                    new State("2",
                        new RemoveConditionalEffect(ConditionEffectIndex.Invincible),
                        new Orbit(0.6, 2, 5, null, orbitClockwise: true),
                        new TossObject("shtrs Ice Portal", 10, coolDown: 15000, throwEffect: true)
                    )
                )
            )
            ;
    }
}