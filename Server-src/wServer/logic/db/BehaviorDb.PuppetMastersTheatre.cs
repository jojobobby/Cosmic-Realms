using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PuppetMastersTheatre = () => Behav()
            .Init("The Puppet Master",
                new State(
                    new ScaleHP2(40,3,15),
                    new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Puppet.mp3"),
                    new DropPortalOnDeath(target: "Puppet Encore Portal"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(dist: 4, targetState: "2")
                        ),
                    new State("2",
                        new Taunt(true, "Welcome to the Final Act, my friends. My puppets require life essence in order to continue performing..."),
                        new TimedTransition(time: 4000, targetState: "3")
                        ),
                    new State("3",
                        new MoveLine(speed: 0.5, direction: 90, distance: 7),
                        new TimedTransition(time: 4000, targetState: "4")
                        ),
                    new State("4",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Its not much, but your lives will have to do for now!"),
                        new Flash(color: 0xFFFFFF, flashPeriod: 0.3, flashRepeats: 12),
                        new TimedTransition(time: 4000, targetState: "5")
                        ),
                    new State("5",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new HpLessTransition(threshold: 0.75, targetState: "16"),
                        new State("7",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 55, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 35, coolDown: 999999),
                            new TimedTransition(300, "8")
                            ),
                        new State("8",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 65, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 25, coolDown: 999999),
                            new TimedTransition(300, "9")
                            ),
                        new State("9",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 75, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 15, coolDown: 999999),
                            new TimedTransition(300, "10")
                            ),
                        new State("10",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 85, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 5, coolDown: 999999),
                            new TimedTransition(300, "11")
                            ),
                        new State("11",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 85, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 5, coolDown: 999999),
                            new TimedTransition(300, "12")
                            ),
                        new State("12",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 75, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 15, coolDown: 999999),
                            new TimedTransition(300, "13")
                            ),
                        new State("13",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 65, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 25, coolDown: 999999),
                            new TimedTransition(300, "14")
                            ),
                        new State("14",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 55, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 35, coolDown: 999999),
                            new TimedTransition(300, "7")
                            ),
                        new TimedTransition(time: 5000, targetState: "15")
                        ),
                    new State("15",
                        new HpLessTransition(threshold: 0.75, targetState: "16"),
                        new Follow(0.6, 10, 5),
                        new Shoot(10, 1, projectileIndex: 2, coolDown: 500),
                        new TimedTransition(5000, "5")
                        ),
                    new State("16",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Watch them dance, hero, as they drain your life away!"),
                        new Spawn("Puppet Wizard 2", 1, 0),
                        new Spawn("Puppet Knight 2", 1, 0),
                        new Spawn("Puppet Priest 2", 1, 0),
                        new TimedTransition(4000, "17")
                        ),
                    new State("17",
                        new HpLessTransition(0.5, "38"),
                        new State("18",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 45, coolDown: 999999),
                            new TimedTransition(300, "19")
                            ),
                        new State("19",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 55, coolDown: 999999),
                            new TimedTransition(300, "20")
                            ),
                        new State("20",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 65, coolDown: 999999),
                            new TimedTransition(300, "21")
                            ),
                        new State("21",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 75, coolDown: 999999),
                            new TimedTransition(300, "22")
                            ),
                        new State("22",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 85, coolDown: 999999),
                            new TimedTransition(300, "23")
                            ),
                        new State("23",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 95, coolDown: 999999),
                            new TimedTransition(300, "24")
                            ),
                        new State("24",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 105, coolDown: 999999),
                            new TimedTransition(300, "25")
                            ),
                        new State("25",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 115, coolDown: 999999),
                            new TimedTransition(300, "26")
                            ),
                        new State("26",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 125, coolDown: 999999),
                            new TimedTransition(300, "27")
                            ),
                        new State("27",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 135, coolDown: 999999),
                            new TimedTransition(300, "28")
                            ),
                        new State("28",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 125, coolDown: 999999),
                            new TimedTransition(300, "29")
                            ),
                        new State("29",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 135, coolDown: 999999),
                            new TimedTransition(300, "30")
                            ),
                        new State("30",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 145, coolDown: 999999),
                            new TimedTransition(300, "31")
                            ),
                        new State("31",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 155, coolDown: 999999),
                            new TimedTransition(300, "32")
                            ),
                        new State("32",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 165, coolDown: 999999),
                            new TimedTransition(300, "33")
                            ),
                        new State("33",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 175, coolDown: 999999),
                            new TimedTransition(300, "34")
                            ),
                        new State("34",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 185, coolDown: 999999),
                            new TimedTransition(300, "35")
                            ),
                        new State("35",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 195, coolDown: 999999),
                            new TimedTransition(300, "36")
                            ),
                        new State("36",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 205, coolDown: 999999),
                            new TimedTransition(300, "37")
                            ),
                        new State("37",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 215, coolDown: 999999),
                            new TimedTransition(300, "15")
                            )
                        ),
                    new State("38",
                        new Wander(0.3),
                        new HpLessTransition(0.25, "43"),
                        new State("39",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new SetAltTexture(1),
                            new Spawn("False Puppet Master", 5, 0),
                            new Taunt("Find me if you can, hero, or die trying!"),
                            new TimedTransition(3000, "40")
                            ),
                        new State("40",
                            new SetAltTexture(0),
                            new Shoot(10, 6, 60, 0, coolDown: 2500),
                            new Shoot(10, 3, 10, 2, coolDown: 1000),
                            new State("41",
                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                new TimedTransition(4000, "42")
                                ),
                            new State("42",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(4000, "41")
                                )
                            )
                        ),
                    new State("43",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Lucky guess hero, but I've run out of time to play games with you. It is time that you died!"),
                        new ReturnToSpawn(speed: 0.8),
                        new Flash(0xFFFFFF, 0.3, 12),
                        new TimedTransition(5000, "44")
                        ),
                    new State("44",
                        new HpLessTransition(0.10, "52"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new OrderOnce(range: 99, children: "False Puppet Master", targetState: "Suicide"),
                        new Spawn("Puppet Wizard 2", 1, 0, 10000),
                        new Spawn("Puppet Knight 2", 1, 0, 10000),
                        new Spawn("Puppet Priest 2", 1, 0, 10000),
                        new State("45",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 65, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 25, coolDown: 999999),
                            new TimedTransition(300, "46")
                            ),
                        new State("46",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 75, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 15, coolDown: 999999),
                            new TimedTransition(300, "47")
                            ),
                        new State("47",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 85, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 5, coolDown: 999999),
                            new TimedTransition(300, "48")
                            ),
                        new State("48",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 85, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 5, coolDown: 999999),
                            new TimedTransition(300, "49")
                            ),
                        new State("49",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 75, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 15, coolDown: 999999),
                            new TimedTransition(300, "50")
                            ),
                        new State("50",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 65, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 25, coolDown: 999999),
                            new TimedTransition(300, "51")
                            ),
                        new State("51",
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 55, coolDown: 999999),
                            new Shoot(radius: 0, count: 4, shootAngle: 90, projectileIndex: 3, fixedAngle: 35, coolDown: 999999),
                            new TimedTransition(300, "45")
                            )
                        ),
                    new State("52",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Taunt("No!!! This can not be how my story ends!! I WILL HAVE MY ENCORE, HERO!"),
                        new TimedTransition(5000, "53")
                        ),
                    new State("53",
                        new Shoot(0, 10, 36, 1, 0, 0, coolDown: 999999),
                        new Suicide()
                        )
                    ),
                new Threshold(0.00001,
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(3, ItemType.Ability, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.25),
                    new TierLoot(10, ItemType.Weapon, 0.125),
                    new TierLoot(5, ItemType.Ring, 0.0625),//0.004
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of the Puppet Master", 0, 1),
                    new ItemLoot("Large Jester Argyle Cloth", 0.06),
                    new ItemLoot("Small Jester Argyle Cloth", 0.06),
                    new ItemLoot("50 Credits", 0.05),
                    new ItemLoot("Twindrill Singer Skin", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("Prism of Dancing Swords", 0.006, damagebased: true),
                    new ItemLoot("Harlequin Armor", 0.004, damagebased: true)
                )
            )
            .Init("Puppet Knight 2",
                new State(
                    new Follow(0.6, 10, 3),
                    new Shoot(9, count: 1, projectileIndex: 0, coolDown: 1000),
                    new Shoot(9, count: 1, projectileIndex: 1, coolDown: 2000)
                 )
            )
            .Init("Puppet Wizard 2",
                new State(
                    new Follow(0.6, 10, 3),
                    new Shoot(9, 15, 24, 0, coolDown: 2500)
                 )
            )
            .Init("Puppet Priest 2",
                new State(
                    new Orbit(0.37, 4, 20, "The Puppet Master"),
                    new HealEntity(99, "The Puppet Master", 75, 4500),
                    new SetAltTexture(minValue: 0, maxValue: 4, cooldown: 500, loop: true)
                 )
            )
            .Init("False Puppet Master",
                new State(
                    new TransformOnDeath("Rogue Puppet", 1, 1, 1),
                    new TransformOnDeath("Assassin Puppet", 1, 1, 1),
                    new State("Ini",
                        new Wander(0.4),
                        new Protect(1, "The Puppet Master", 10, 8, 3),
                        new Shoot(8.4, count: 7, projectileIndex: 0, coolDown: 2850),
                        new Shoot(8.4, count: 3, shootAngle: 30, projectileIndex: 2, coolDown: 1500),
                        new HpLessTransition(0.11, "dead")
                        ),
                     new State("dead",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt(1.00, "You may have killed me, but I am only a pretender. Get ready for the plot twist!"),
                        new TimedTransition(2500, "die")
                        ),
                     new State("die",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new Shoot(8.4, count: 8, projectileIndex: 1, coolDown: 2850),
                         new Suicide()
                        ),
                     new State("Suicide",
                         new Suicide()
                     )
                )
            )
            .Init("Assassin Puppet",
                new State(
                    new Follow(0.35, 8, 4),
                    new Wander(0.2),
                    new Grenade(3, 140, 4, coolDown: 3000),
                    new Shoot(8.4, count: 1, projectileIndex: 0, coolDown: 1500)
                )
            )
            .Init("Rogue Puppet",
                new State(
                    new State("1",
                        new SetAltTexture(1),
                        new Wander(0.6),
                        new TimedTransition(3000, "2")
                        ),
                    new State("2",
                        new SetAltTexture(0),
                        new Shoot(9, count: 1, projectileIndex: 0, coolDown: 1000),
                        new TimedTransition(3000, "1")
                        )
                )
            )
            ;
    }
}