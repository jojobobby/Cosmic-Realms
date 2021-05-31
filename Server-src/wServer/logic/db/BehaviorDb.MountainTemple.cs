using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MountainTemple = () => Behav()
            .Init("Daichi the Fallen",
                new State(
                    new ScaleHP2(70,1,15),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new PlayerWithinTransition(4, "Phase1", true)
                    ),
                    new State("Phase1",
                        new Taunt("Ha ha fools, you are too late. Lord Xil will soon arrive in this realm."),
                        new TimedTransition(3000, "Phase2")
                    ),
                    new State("2",
                        new SetAltTexture(1),
                        new TimedTransition(500, "3")
                    ),
                    new State("3",
                        new SetAltTexture(2),
                        new TimedTransition(500, "4")
                    ),
                    new State("4",
                        new SetAltTexture(3),
                        new TimedTransition(500, "5")
                    ),
                    new State("5",
                        new SetAltTexture(4),
                        new TimedTransition(500, "6")
                    ),
                    new State("6",
                        new JumpToRandomOffset(8, 8, -4, -4),
                        new TimedTransition(1000, "7")
                    ),
                    new State("7",
                        new SetAltTexture(3),
                        new TimedTransition(500, "8")
                    ),
                    new State("8",
                        new SetAltTexture(2),
                        new TimedTransition(500, "9")
                    ),
                    new State("9",
                        new SetAltTexture(1),
                        new TimedTransition(500, "10")
                    ),
                    new State("10",
                        new SetAltTexture(0),
                        new Spawn("Fire Power", 1, 0),
                        new TimedTransition(500, "11")
                    ),
                    new State("11",
                        new SetAltTexture(1),
                        new TimedTransition(500, "12")
                    ),
                    new State("12",
                        new SetAltTexture(2),
                        new TimedTransition(500, "13")
                    ),
                    new State("13",
                        new SetAltTexture(3),
                        new TimedTransition(500, "14")
                    ),
                    new State("14",
                        new SetAltTexture(4),
                        new TimedTransition(500, "15")
                    ),
                    new State("15",
                        new JumpToRandomOffset(0, 0, 8, 8),
                        new TimedTransition(1000, "16")
                    ),
                    new State("16",
                        new SetAltTexture(3),
                        new TimedTransition(500, "17")
                    ),
                    new State("17",
                        new SetAltTexture(2),
                        new TimedTransition(500, "18")
                    ),
                    new State("18",
                        new SetAltTexture(1),
                        new TimedTransition(500, "19")
                    ),
                    new State("19",
                        new SetAltTexture(0),
                        new Spawn("Water Power", 1, 0),
                        new TimedTransition(500, "20")
                    ),
                    new State("20",
                        new SetAltTexture(1),
                        new TimedTransition(500, "21")
                    ),
                    new State("21",
                        new SetAltTexture(2),
                        new TimedTransition(500, "22")
                    ),
                    new State("22",
                        new SetAltTexture(3),
                        new TimedTransition(500, "23")
                    ),
                    new State("23",
                        new SetAltTexture(4),
                        new TimedTransition(500, "24")
                    ),
                    new State("24",
                        new JumpToRandomOffset(-16, -16, 0, 0),
                        new TimedTransition(1000, "25")
                    ),
                    new State("25",
                        new SetAltTexture(3),
                        new TimedTransition(500, "26")
                    ),
                    new State("26",
                        new SetAltTexture(2),
                        new TimedTransition(500, "27")
                    ),
                    new State("27",
                        new SetAltTexture(1),
                        new TimedTransition(500, "28")
                    ),
                    new State("28",
                        new SetAltTexture(0),
                        new Spawn("Earth Power", 1, 0),
                        new TimedTransition(500, "29")
                    ),
                    new State("29",
                        new SetAltTexture(1),
                        new TimedTransition(500, "30")
                    ),
                    new State("30",
                        new SetAltTexture(2),
                        new TimedTransition(500, "31")
                    ),
                    new State("31",
                        new SetAltTexture(3),
                        new TimedTransition(500, "32")
                    ),
                    new State("32",
                        new SetAltTexture(4),
                        new TimedTransition(500, "33")
                    ),
                    new State("33",
                        new JumpToRandomOffset(0, 0, -8, -8),
                        new TimedTransition(1000, "34")
                    ),
                    new State("34",
                        new SetAltTexture(3),
                        new TimedTransition(500, "35")
                    ),
                    new State("35",
                        new SetAltTexture(2),
                        new TimedTransition(500, "36")
                    ),
                    new State("36",
                        new SetAltTexture(1),
                        new TimedTransition(500, "37")
                    ),
                    new State("37",
                        new SetAltTexture(0),
                        new Spawn("Air Power", 1, 0),
                        new TimedTransition(500, "38")
                    ),
                    new State("38",
                        new SetAltTexture(1),
                        new TimedTransition(500, "39")
                    ),
                    new State("39",
                        new SetAltTexture(2),
                        new TimedTransition(500, "40")
                    ),
                    new State("40",
                        new SetAltTexture(3),
                        new TimedTransition(500, "41")
                    ),
                    new State("41",
                        new SetAltTexture(4),
                        new TimedTransition(500, "42")
                    ),
                    new State("42",
                        new JumpToRandomOffset(8, 8, 4, 4),
                        new TimedTransition(1000, "43")
                    ),
                    new State("43",
                        new SetAltTexture(3),
                        new TimedTransition(500, "44")
                    ),
                    new State("44",
                        new SetAltTexture(2),
                        new TimedTransition(500, "45")
                    ),
                    new State("45",
                        new SetAltTexture(1),
                        new TimedTransition(500, "46")
                    ),
                    new State("46",
                        new SetAltTexture(0),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new HpLessTransition(0.5, "72"),
                        new HpLessTransition(0.75, "Phase3"),
                        new TimedTransition(1000, "Phase2")
                    ),
                    new State("Phase2",
                        new HpLessTransition(0.75, "Phase3"),
                        new State("47",
                            new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0),
                            new TimedTransition(200, "48")
                        ),
                        new State("48",
                            new Shoot(0, 4, 90, 1, 0, 10, coolDown: 200),
                            new TimedTransition(3200, "49")
                        ),
                        new State("49",
                            new Shoot(0, 4, 90, 1, 90, -10, coolDown: 200),
                            new TimedTransition(3200, "50_1")
                        ),
                        new State("50_1",
                            new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0),
                            new TimedTransition(200, "50")
                        )
                    ),
                    new State("50",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new TimedTransition(500, "51")
                    ),
                    new State("51",
                        new SetAltTexture(2),
                        new TimedTransition(500, "52")
                    ),
                    new State("52",
                        new SetAltTexture(3),
                        new TimedTransition(500, "53")
                    ),
                    new State("53",
                        new SetAltTexture(4),
                        new TimedRandomTransition(500, false, "54_red", "54_blue", "54_green", "54_black")
                    ),
                    new State("54_red",
                        new JumpToRandomOffset(8, 8, -4, -4),
                        new TimedTransition(1000, "55")
                    ),
                    new State("54_blue",
                        new JumpToRandomOffset(8, 8, 4, 4),
                        new TimedTransition(1000, "55")
                    ),
                    new State("54_green",
                        new JumpToRandomOffset(-8, -8, 4, 4),
                        new TimedTransition(1000, "55")
                    ),
                    new State("54_black",
                        new JumpToRandomOffset(-8, -8, -4, -4),
                        new TimedTransition(1000, "55")
                    ),
                    new State("55",
                        new SetAltTexture(3),
                        new TimedTransition(500, "56")
                    ),
                    new State("56",
                        new SetAltTexture(2),
                        new TimedTransition(500, "57")
                    ),
                    new State("57",
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new HpLessTransition(0.5, "58_3"),
                        new HpLessTransition(0.75, "58_2"),
                        new TimedTransition(500, "58")
                    ),
                    new State("58",
                        new HpLessTransition(0.75, "59"),
                        new OrderOnce(3, "Water Power", "1"),
                        new OrderOnce(3, "Air Power", "1"),
                        new OrderOnce(3, "Earth Power", "1"),
                        new OrderOnce(3, "Fire Power", "1"),
                        new TimedTransition(4000, "59")
                    ),
                    new State("58_2",
                        new HpLessTransition(0.5, "59"),
                        new OrderOnce(3, "Water Power", "1"),
                        new OrderOnce(3, "Air Power", "1"),
                        new OrderOnce(3, "Earth Power", "1"),
                        new OrderOnce(3, "Fire Power", "1"),
                        new TimedTransition(4000, "59")
                    ),
                    new State("58_3",
                        new HpLessTransition(0.25, "59"),
                        new OrderOnce(3, "Water Power", "1"),
                        new OrderOnce(3, "Air Power", "1"),
                        new OrderOnce(3, "Earth Power", "1"),
                        new OrderOnce(3, "Fire Power", "1"),
                        new TimedTransition(4000, "59")
                    ),
                    new State("59",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new OrderOnce(3, "Water Power", "Ini"),
                        new OrderOnce(3, "Air Power", "Ini"),
                        new OrderOnce(3, "Earth Power", "Ini"),
                        new OrderOnce(3, "Fire Power", "Ini"),
                        new TimedTransition(1000, "60")
                    ),
                    new State("60",
                        new SetAltTexture(1),
                        new TimedTransition(500, "61")
                    ),
                    new State("61",
                        new SetAltTexture(2),
                        new TimedTransition(500, "62")
                    ),
                    new State("62",
                        new SetAltTexture(3),
                        new TimedTransition(500, "63")
                    ),
                    new State("63",
                        new SetAltTexture(4),
                        new TimedTransition(500, "64")
                    ),
                    new State("64",
                        new MoveToSpawn(),
                        new TimedTransition(1000, "43")
                    ),
                    new State("Phase3",
                        new HpLessTransition(0.5, "Phase4"),
                        new State("65",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                            new TimedTransition(1000, "66")
                        ),
                        new State("66",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new TimedTransition(1000, "67")
                        ),
                        new State("67",
                            new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0, coolDown: 3000),
                            new Shoot(12, 3, 10, 7, coolDown: 1500, predictive: 1),
                            new Shoot(0, 2, 30, 6, 0, 90, coolDown: 500),
                            new TimedTransition(6000, "50")
                        )
                    ),
                    new State("Phase4",
                        new State("68",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                            new Taunt("You don’t lack skills! But you have underestimated the power lying within this Temple. Let me show you."),
                            new TimedTransition(1000, "69")
                        ),
                        new State("69",
                            new Shoot(0, 24, projectileIndex: 8, fixedAngle: 0),
                            new TimedTransition(500, "70")
                        ),
                        new State("70",
                            new OrderOnce(12, "Water Power", "2"),
                            new OrderOnce(12, "Air Power", "2"),
                            new OrderOnce(12, "Earth Power", "2"),
                            new OrderOnce(12, "Fire Power", "2"),
                            new TimedTransition(6000, "71")
                        ),
                        new State("71",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new EntitiesNotExistsTransition(20, "72", "Water Elemental", "Air Elemental", "Earth Elemental", "Fire Elemental")
                        ),
                        new State("72",
                            new HpLessTransition(0.25, "Phase5"),
                            new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0, coolDown: 3000),
                            new Shoot(12, 3, 10, 7, coolDown: 1500, predictive: 1),
                            new Shoot(0, 2, 30, 6, 0, 90, coolDown: 250),
                            new State("72",
                                new Spawn("chasingHorror", 1, 0),
                                new EntityExistsTransition("chasingHorror", 20, "73")
                            ),
                            new State("73",
                                new EntityNotExistsTransition("chasingHorror", 20, "72")
                            ),
                            new TimedTransition(6000, "50")
                        )
                    ),
                    new State("Phase5",
                        new State("74",
                            new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                            new Taunt("ENOUGH! You fought well but now it’s time for you to die."),
                            new TimedTransition(1000, "75")
                        ),
                        new State("75",
                            new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Taunt("Unlimited power!"),
                            new TimedTransition(1000, "76")
                        ),
                        new State("76",
                            new Shoot(0, 20, projectileIndex: 0, fixedAngle: 0, coolDown: 3000),
                            new Shoot(12, 3, 10, 7, coolDown: 1500, predictive: 1),
                            new Shoot(0, 2, 30, 6, 0, 90, coolDown: 250),
                            new Shoot(0, 4, 90, 1, 0, 10, coolDown: 200),
                            new State("77",
                                new Spawn("chasingHorror", 1, 0),
                                new EntityExistsTransition("chasingHorror", 20, "78")
                            ),
                            new State("78",
                                new EntityNotExistsTransition("chasingHorror", 20, "77")
                            )
                        )
                    )
                ),
                new Threshold(0.0001,
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new TierLoot(5, ItemType.Ring, 0.0625),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(12, ItemType.Armor, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Daichi", 1),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("Potion of Attack", 0.3),
                    new ItemLoot("Potion of Vitality", 0.3),
                    new ItemLoot("Potion of Wisdom", 0.3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Potion of Speed", 0.3),
                    new ItemLoot("Potion of Critical Chance", 1),
                    new ItemLoot("Potion of Critical Damage", 1),
                    new ItemLoot("Wonder Mage's Staff", 0.004, damagebased: true),
                    new ItemLoot("Kazekiri", 0.006, damagebased: true),
                    new ItemLoot("Kamishimo", 0.006, damagebased: true),
                    new ItemLoot("Wand of the Fallen", 0.004, damagebased: true)
                )
            )
            .Init("chasingHorror",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new TimedRandomTransition(500, false, "1", "2", "3", "4")
                    ),
                    new State("1",
                        new MoveLine(1, 45, 2),
                        new TimedTransition(500, "flash")
                    ),
                    new State("2",
                        new MoveLine(1, 135, 2),
                        new TimedTransition(500, "flash")
                    ),
                    new State("3",
                        new MoveLine(1, 225, 2),
                        new TimedTransition(500, "flash")
                    ),
                    new State("4",
                        new MoveLine(1, 315, 2),
                        new TimedTransition(500, "flash")
                    ),
                    new State("flash",
                        new Flash(0xFFFFFF, 1, 1),
                        new TimedTransition(1000, "5")
                    ),
                    new State("5",
                        new Shoot(12, 1),
                        new Suicide()
                    )
                )
            )
            .Init("Water Elemental",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Protect(0.6, "Daichi the Fallen", 20, 1),
                        new EntityExistsTransition("Daichi the Fallen", 1, "2")
                    ),
                    new State("2",
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new Suicide()
                    )
                )
            )
            .Init("Air Elemental",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Protect(0.6, "Daichi the Fallen", 20, 1),
                        new EntityExistsTransition("Daichi the Fallen", 1, "2")
                    ),
                    new State("2",
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new Suicide()
                    )
                )
            )
            .Init("Earth Elemental",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Protect(0.6, "Daichi the Fallen", 20, 1),
                        new EntityExistsTransition("Daichi the Fallen", 1, "2")
                    ),
                    new State("2",
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new Suicide()
                    )
                )
            )
            .Init("Fire Elemental",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini",
                        new Protect(0.6, "Daichi the Fallen", 20, 1),
                        new EntityExistsTransition("Daichi the Fallen", 1, "2")
                    ),
                    new State("2",
                        new TimedTransition(2000, "3")
                    ),
                    new State("3",
                        new Suicide()
                    )
                )
            )
            .Init("Water Power",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini"),
                    new State("1",
                        new Shoot(0, 20, 16, 0, 0, 45, coolDown: 1000)
                    ),
                    new State("2",
                        new Spawn("Water Elemental", 1, 0)
                    )
                )
            )
            .Init("Air Power",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini"),
                    new State("1",
                        new Shoot(0, 20, 16, 0, 0, 45, coolDown: 1000)
                    ),
                    new State("2",
                        new Spawn("Air Elemental", 1, 0)
                    )
                )
            )
            .Init("Earth Power",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini"),
                    new State("1",
                        new Shoot(0, 20, 16, 0, 0, 45, coolDown: 1000)
                    ),
                    new State("2",
                        new Spawn("Earth Elemental", 1, 0)
                    )
                )
            )
            .Init("Fire Power",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Ini"),
                    new State("1",
                        new Shoot(0, 20, 16, 0, 0, 45, coolDown: 1000)
                    ),
                    new State("2",
                        new Spawn("Fire Elemental", 1, 0)
                    )
                )
            )
            ;
    }
}