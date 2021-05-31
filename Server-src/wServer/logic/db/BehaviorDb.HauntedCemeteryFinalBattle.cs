using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ HauntedCemeteryFinalBattle = () => Behav()
            .Init("Zombie Hulk",
                new State(
                    new Wander(speed: 0.35),
                    new State("Attack2",
                        new SetAltTexture(0),
                        new StayBack(speed: 0.55, distance: 7, entity: null),
                        new Follow(speed: 0.3, acquireRange: 11, range: 5),
                        new Shoot(radius: 10, count: 1, projectileIndex: 1, coolDown: 400, coolDownOffset: 500),
                        new TimedRandomTransition(3000, true, "Attack1", "Attack3")
                    ),
                    new State("Attack1",
                        new SetAltTexture(1),
                        new Shoot(radius: 8, count: 3, shootAngle: 40, projectileIndex: 0, coolDown: 400, coolDownOffset: 250),
                        new Shoot(radius: 8, count: 2, shootAngle: 60, projectileIndex: 0, coolDown: 400, coolDownOffset: 250),
                        new Charge(speed: 1.1, range: 11, coolDown: 200),
                        new TimedRandomTransition(3000, true, "Attack2", "Attack3")
                    ),
                    new State("Attack3",
                        new SetAltTexture(0),
                        new Wander(speed: 0.4),
                        new Shoot(radius: 10, count: 3, shootAngle: 30, projectileIndex: 1, coolDown: 400, coolDownOffset: 500),
                        new TimedRandomTransition(3000, true, "Attack1", "Attack2")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(6, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(3, ItemType.Ring, 0.25)
                )
            )
            .Init("Classic Ghost",
                new State(
                    new Wander(speed: 0.4),
                    new Follow(speed: 0.55, acquireRange: 10, range: 3, duration: 1000, coolDown: 2000),
                    new Orbit(speed: 0.55, radius: 4, acquireRange: 7, target: null),
                    new Shoot(radius: 5, count: 4, shootAngle: 16, projectileIndex: 0, coolDown: 1000, coolDownOffset: 0)
                )
            )
            .Init("Werewolf",
                new State(
                    new Spawn(children: "Mini Werewolf", maxChildren: 2, initialSpawn: 0, coolDown: 5400, givesNoXp: false),
                    new Spawn(children: "Mini Werewolf", maxChildren: 3, initialSpawn: 0, coolDown: 5400, givesNoXp: false),
                    new StayCloseToSpawn(speed: 0.8, range: 6),
                    new State("Circling",
                        new Shoot(radius: 10, count: 3, shootAngle: 20, projectileIndex: 0, coolDown: 1600),
                        new Prioritize(
                            new Orbit(speed: 0.4, radius: 5.4, acquireRange: 8, target: null),
                            new Wander(speed: 0.4)
                        ),
                        new TimedTransition(time: 3400, targetState: "Engaging")
                    ),
                    new State("Engaging",
                        new Shoot(radius: 5.5, count: 5, shootAngle: 13, projectileIndex: 0, coolDown: 1600),
                        new Follow(speed: 0.6, acquireRange: 10, range: 1),
                        new TimedTransition(time: 2600, targetState: "Circling")

                    )
                )
            )
            .Init("Mini Werewolf",
                new State(
                    new Shoot(radius: 4, count: 1, projectileIndex: 0, coolDown: 1000),
                    new Prioritize(
                            new Follow(speed: 0.6, acquireRange: 15, range: 1),
                            new Protect(speed: 0.8, protectee: "Werewolf", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(speed: 0.4)
                    )
                )
            )
            .Init("Ghost of Skuld",
                new State(
                    new SetAltTexture(11),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Start",
                        new PlayerWithinTransition(10, "1")
                        ),
                    new State("1",
                        new TimedTransition(0, "2")
                        ),
                    new State("2",
                        new SetAltTexture(1),
                        new TimedTransition(100, "3")
                        ),
                    new State("3",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "4")
                        ),
                    new State("4",
                        new Taunt(true, "I am near, when you find me make sure to say 'Ready'."),
                        new TimedTransition(5000, "5")
                        ),
                    //repeat
                    new State("5",
                        new PlayerTextTransition("85", "ready"),
                        new TimedTransition(2000, "4")
                        ),
                    new State("6",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(13),
                        new TimedTransition(2000, "85")
                        ),
                    new State("6_1",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(12),
                       new TimedTransition(2000, "85")
                        ),
                    new State("6_2",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(13),
                         new TimedTransition(2000, "85")
                        ),
                    new State("6_3",
                        new PlayerTextTransition("85", "ready"),
                        new SetAltTexture(12),
                         new TimedTransition(2000, "85")
                        ),
                    new State("6_4",
                        new SetAltTexture(0),
                        new PlayerTextTransition("85", "ready"),
                         new TimedTransition(2000, "85")
                        ),
                    new State("7",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 1"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 1"),
                        new EntityExistsTransition("Classic Ghost", 999, "Check 1")
                        ),
                    new State("Check 1",
                        new EntitiesNotExistsTransition(100, "8", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("8",
                        new SetAltTexture(1),
                        new TimedTransition(0, "9")
                        ),
                    new State("9",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "10")
                        ),
                    new State("10",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "11")
                        ),
                    new State("11",
                        new SetAltTexture(12),
                        new TimedTransition(500, "12")
                        ),
                    new State("12",
                        new SetAltTexture(13),
                        new TimedTransition(500, "13")
                        ),
                    new State("13",
                        new SetAltTexture(12),
                        new TimedTransition(500, "14")
                        ),
                    new State("14",
                        new SetAltTexture(13),
                        new TimedTransition(500, "15")
                        ),
                    new State("15",
                        new SetAltTexture(12),
                        new TimedTransition(500, "16")
                        ),
                    new State("16",
                        new SetAltTexture(13),
                        new TimedTransition(500, "17")
                        ),
                    new State("17",
                        new SetAltTexture(12),
                        new TimedTransition(500, "18")
                        ),
                    new State("18",
                        new SetAltTexture(13),
                        new TimedTransition(500, "19")
                        ),
                    new State("19",
                        new SetAltTexture(12),
                        new TimedTransition(500, "20")
                        ),
                    new State("20",
                        new SetAltTexture(13),
                        new TimedTransition(500, "21")
                        ),
                    new State("21",
                        new SetAltTexture(12),
                        new TimedTransition(500, "22")
                        ),
                    new State("22",
                        new SetAltTexture(13),
                        new TimedTransition(500, "23")
                        ),
                    new State("23",
                        new SetAltTexture(12),
                        new TimedTransition(500, "24")
                        ),
                    new State("24",
                        new SetAltTexture(13),
                        new TimedTransition(500, "25")
                        ),
                    new State("25",
                        new SetAltTexture(0),
                        new TimedTransition(100, "26")
                        ),
                    new State("26",
                        new SetAltTexture(1),
                        new TimedTransition(100, "27")
                        ),
                    new State("27",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 2"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 2"),
                        new EntityExistsTransition("Classic Ghost", 999, "Check 2")
                        ),
                    new State("Check 2",
                        new EntitiesNotExistsTransition(100, "28", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("28",
                        new SetAltTexture(1),
                        new TimedTransition(0, "29")
                        ),
                    new State("29",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "30")
                        ),
                    new State("30",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "31")
                        ),
                    new State("31",
                        new SetAltTexture(12),
                        new TimedTransition(500, "32")
                        ),
                    new State("32",
                        new SetAltTexture(13),
                        new TimedTransition(500, "33")
                        ),
                    new State("33",
                        new SetAltTexture(12),
                        new TimedTransition(500, "34")
                        ),
                    new State("34",
                        new SetAltTexture(13),
                        new TimedTransition(500, "35")
                        ),
                    new State("35",
                        new SetAltTexture(12),
                        new TimedTransition(500, "36")
                        ),
                    new State("36",
                        new SetAltTexture(13),
                        new TimedTransition(500, "37")
                        ),
                    new State("37",
                        new SetAltTexture(12),
                        new TimedTransition(500, "38")
                        ),
                    new State("38",
                        new SetAltTexture(13),
                        new TimedTransition(500, "39")
                        ),
                    new State("39",
                        new SetAltTexture(12),
                        new TimedTransition(500, "40")
                        ),
                    new State("40",
                        new SetAltTexture(13),
                        new TimedTransition(500, "41")
                        ),
                    new State("41",
                        new SetAltTexture(12),
                        new TimedTransition(500, "42")
                        ),
                    new State("42",
                        new SetAltTexture(13),
                        new TimedTransition(500, "43")
                        ),
                    new State("43",
                        new SetAltTexture(12),
                        new TimedTransition(500, "44")
                        ),
                    new State("44",
                        new SetAltTexture(13),
                        new TimedTransition(100, "45")
                        ),
                    new State("45",
                        new SetAltTexture(0),
                        new TimedTransition(100, "46")
                        ),
                    new State("46",
                        new SetAltTexture(1),
                        new TimedTransition(100, "47")
                        ),
                    new State("47",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 3"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 3"),
                        new EntityExistsTransition("Classic Ghost", 999, "Check 3")
                        ),
                    new State("Check 3",
                        new EntitiesNotExistsTransition(100, "48", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("48",
                        new SetAltTexture(1),
                        new TimedTransition(0, "49")
                        ),
                    new State("49",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "50")
                        ),
                    new State("50",
                        new Taunt(true, "The next wave will appear in 3 seconds. Prepare yourself!", "l hope you're prepared because the next wave is in 3 seconds.", "The next onslaught will begin in 3 seconds!", "You have 3 seconds until your next challenge!", "3 seconds until the next attack!"),
                        new TimedTransition(0, "51")
                        ),
                    new State("51",
                        new SetAltTexture(12),
                        new TimedTransition(500, "52")
                        ),
                    new State("52",
                        new SetAltTexture(13),
                        new TimedTransition(500, "53")
                        ),
                    new State("53",
                        new SetAltTexture(12),
                        new TimedTransition(500, "54")
                        ),
                    new State("54",
                        new SetAltTexture(13),
                        new TimedTransition(500, "55")
                        ),
                    new State("55",
                        new SetAltTexture(12),
                        new TimedTransition(500, "56")
                        ),
                    new State("56",
                        new SetAltTexture(13),
                        new TimedTransition(500, "57")
                        ),
                    new State("57",
                        new SetAltTexture(12),
                        new TimedTransition(500, "58")
                        ),
                    new State("58",
                        new SetAltTexture(13),
                        new TimedTransition(500, "59")
                        ),
                    new State("59",
                        new SetAltTexture(12),
                        new TimedTransition(500, "60")
                        ),
                    new State("60",
                        new SetAltTexture(13),
                        new TimedTransition(500, "61")
                        ),
                    new State("61",
                        new SetAltTexture(12),
                        new TimedTransition(500, "62")
                        ),
                    new State("62",
                        new SetAltTexture(13),
                        new TimedTransition(500, "63")
                        ),
                    new State("63",
                        new SetAltTexture(12),
                        new TimedTransition(500, "64")
                        ),
                    new State("64",
                        new SetAltTexture(13),
                        new TimedTransition(500, "65")
                        ),
                    new State("65",
                        new SetAltTexture(0),
                        new TimedTransition(100, "66")
                        ),
                    new State("66",
                        new SetAltTexture(1),
                        new TimedTransition(100, "67")
                        ),
                    new State("67",
                        new SetAltTexture(11),
                        new OrderOnce(100, "Arena Up Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Lf Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Dn Spawner", "Stage 4"),
                        new OrderOnce(100, "Arena Rt Spawner", "Stage 4"),
                        new EntityExistsTransition("Classic Ghost", 999, "Check 4")
                        ),
                    new State("Check 4",
                        new EntitiesNotExistsTransition(100, "68", "Werewolf", "Mini Werewolf", "Zombie Hulk", "Classic Ghost")
                        ),
                    new State("68",
                        new SetAltTexture(1),
                        new TimedTransition(0, "69")
                        ),
                    new State("69",
                        new SetAltTexture(0),
                        new TimedTransition(2000, "70")
                        ),
                    new State("70",
                        new Taunt(true, "Congratulations on your victory, warrior. Your reward shall be..."),
                        new TimedTransition(0, "71")
                        ),
                    new State("71",
                        new SetAltTexture(12),
                        new TimedTransition(500, "72")
                        ),
                    new State("72",
                        new SetAltTexture(13),
                        new TimedTransition(500, "73")
                        ),
                    new State("73",
                        new SetAltTexture(12),
                        new TimedTransition(500, "74")
                        ),
                    new State("74",
                        new SetAltTexture(13),
                        new TimedTransition(500, "75")
                        ),
                    new State("75",
                        new SetAltTexture(12),
                        new TimedTransition(500, "76")
                        ),
                    new State("76",
                        new SetAltTexture(13),
                        new TimedTransition(500, "77")
                        ),
                    new State("77",
                        new SetAltTexture(12),
                        new TimedTransition(500, "78")
                        ),
                    new State("78",
                        new SetAltTexture(13),
                        new TimedTransition(500, "79")
                        ),
                    new State("79",
                        new SetAltTexture(12),
                        new TimedTransition(500, "80")
                        ),
                    new State("80",
                        new SetAltTexture(13),
                        new TimedTransition(500, "81")
                        ),
                    new State("81",
                        new SetAltTexture(12),
                        new TimedTransition(500, "82")
                        ),
                    new State("82",
                        new SetAltTexture(13),
                        new TimedTransition(500, "83")
                        ),
                    new State("83",
                        new SetAltTexture(12),
                        new TimedTransition(500, "84")
                        ),
                    new State("84",
                        new SetAltTexture(13),
                        new TimedTransition(500, "85")
                        ),
                    new State("85",
                        new Taunt("Your death will be SWIFT!!!"),
                        new SetAltTexture(0),
                        new TimedTransition(500, "86")
                        ),
                    new State("86",
                        new SetAltTexture(2),
                        new TimedTransition(500, "87")
                        ),
                    new State("87",
                        new SetAltTexture(3),
                        new TimedTransition(500, "88")
                        ),
                    new State("88",
                        new SetAltTexture(2),
                        new TimedTransition(500, "89")
                        ),
                    new State("89",
                        new SetAltTexture(3),
                        new TimedTransition(500, "90")
                        ),
                    new State("90",
                        new SetAltTexture(2),
                        new Shoot(10, 36, projectileIndex: 1, fixedAngle: 0, angleOffset: fixedAngle_RingAttack2),
                        new TimedTransition(0, "91")
                        ),
                    new State("91",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(0, "92")
                        ),
                    new State("92",
                        new SetAltTexture(0),
                        new OrderOnce(100, "Halloween Zombie Spawner", "1"),
                        new Shoot(20, 4, shootAngle: 13, projectileIndex: 0, predictive: 0.5, coolDown: 500),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                        new HpLessTransition(0.5, "93")
                        ),
                    new State("93",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Spawn("Flying Flame Skull", maxChildren: 2),
                        new Shoot(20, 4, shootAngle: 13, projectileIndex: 0, predictive: 0.5, coolDown: 500),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                        new TimedTransition(5000, "94")
                        ),
                    new State("94",
                        new Shoot(20, 4, shootAngle: 13, projectileIndex: 0, predictive: 0.5, coolDown: 500),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 0, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 90, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 180, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 270, coolDown: 5000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 45, coolDown: 2000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 135, coolDown: 2000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 225, coolDown: 2000),
                        new Shoot(100, 1, projectileIndex: 2, fixedAngle: 0, angleOffset: 315, coolDown: 2000)
                        )
                ),
                new Threshold(0.0001,
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(5, ItemType.Ring, 0.0625),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Skuld", 1),
                    new ItemLoot("Potion of Vitality", 1),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Plague Poison", 0.006, damagebased: true),
                    new ItemLoot("Resurrected Warrior's Armor", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Etherite Dagger", 0.004, damagebased: true),
                    new ItemLoot("Spectral Ring of Horrors", 0.006, damagebased: true),
                    new ItemLoot("Ghastly Drape", 0.006, damagebased: true),
                    new ItemLoot("Mantle of Skuld", 0.006, damagebased: true),
                    new ItemLoot("Resurrected Archer's Armor", 0.001, damagebased: true, threshold: 0.01)
                )
            )
            .Init("Halloween Zombie Spawner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Leech"),
                    new State("1",
                        new Spawn("Zombie Rise", maxChildren: 1),
                        new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                )
            )
            .Init("Zombie Rise",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Blue Zombie"),
                    new State("1",
                        new SetAltTexture(1),
                        new TimedTransition(750, "2")
                    ),
                    new State("2",
                        new SetAltTexture(2),
                        new TimedTransition(750, "3")
                    ),
                    new State("3",
                        new SetAltTexture(3),
                        new TimedTransition(750, "4")
                    ),
                    new State("4",
                        new SetAltTexture(4),
                        new TimedTransition(750, "5")
                    ),
                    new State("5",
                        new SetAltTexture(5),
                        new TimedTransition(750, "6")
                    ),
                    new State("6",
                        new SetAltTexture(6),
                        new TimedTransition(750, "7")
                    ),
                    new State("7",
                        new SetAltTexture(7),
                        new TimedTransition(750, "8")
                    ),
                    new State("8",
                        new SetAltTexture(8),
                        new TimedTransition(750, "9")
                    ),
                    new State("9",
                        new SetAltTexture(9),
                        new TimedTransition(750, "10")
                    ),
                    new State("10",
                        new SetAltTexture(10),
                        new TimedTransition(750, "11")
                    ),
                    new State("11",
                        new SetAltTexture(11),
                        new TimedTransition(750, "12")
                    ),
                    new State("12",
                        new SetAltTexture(12),
                        new TimedTransition(750, "13")
                    ),
                    new State("13",
                        new SetAltTexture(13),
                        new TimedTransition(750, "14")
                    ),
                    new State("14",
                        new Suicide()
                    )
                )
            )
            .Init("Blue Zombie",
                new State(
                    new Follow(0.03, 100, 1),
                    new State("1",
                        new Shoot(10, 1, projectileIndex: 0, coolDown: 1000),
                        new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                )
            )
            .Init("Flying Flame Skull",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Orbit(1, 5, 10, target: "Ghost of Skuld"),
                    new State("1",
                        new Shoot(100, 10, shootAngle: 36, projectileIndex: 0, coolDown: 1000),
                        new EntityNotExistsTransition("Ghost of Skuld", 100, "2")
                    ),
                    new State("2",
                        new Suicide()
                    )
                )
            )
            ;
    }
}