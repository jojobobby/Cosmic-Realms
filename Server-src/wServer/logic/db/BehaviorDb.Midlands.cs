using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
//by creepylava, nr, ghostmaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Midlands = () => Behav()
            //Mid Plains
            .Init("Orc King",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 0.1),
                    new Shoot(3),
                    new Spawn("Orc Queen", maxChildren: 2, coolDown: 60000, givesNoXp: false),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Follow(0.6, range: 1, duration: 3000, coolDown: 3000),
                        new Wander(0.6)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.18),
                    new TierLoot(4, ItemType.Weapon, 0.18),
                    new TierLoot(5, ItemType.Weapon, 0.05),
                    new TierLoot(4, ItemType.Armor, 0.21),
                    new TierLoot(5, ItemType.Armor, 0.21),
                    new ItemLoot("Health Potion", 0.4),
                    new TierLoot(2, ItemType.Ring, 0.07),
                    new TierLoot(2, ItemType.Ability, 0.17)
                )
            )
            .Init("Orc Queen",
                new State(
                    new Spawn("Orc Mage", maxChildren: 2, coolDown: 8000, givesNoXp: false),
                    new Spawn("Orc Warrior", maxChildren: 3, coolDown: 8000, givesNoXp: false),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Protect(0.8, "Orc King", acquireRange: 11, protectionRange: 7, reprotectRange: 5.4),
                        new Wander(0.8)
                    ),
                    new HealGroup(10, "OrcKings", 300)
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.03)
                )
            )
            .Init("Orc Warrior",
                new State(
                    new Shoot(3, predictive: 1, coolDown: 500),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Orbit(1.35, 2.5, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1,
                            radiusVariance: 0.1)
                        )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.03)
                )
            )
            .Init("Orc Mage",
                new State(
                    new State("circle_player",
                        new Shoot(8, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(1.4, 60),
                            new Protect(0.7, "Orc Queen", acquireRange: 11, protectionRange: 10, reprotectRange: 3),
                            new Orbit(0.7, 3.5, acquireRange: 11)
                        ),
                        new TimedTransition(3500, "circle_queen")
                    ),
                    new State("circle_queen",
                        new Shoot(8, count: 3, predictive: 0.3, shootAngle: 120, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new StayAbove(1.4, 60),
                            new Orbit(1.2, 2.5, target: "Orc Queen", acquireRange: 12, speedVariance: 0.1,
                                radiusVariance: 0.1)
                            ),
                        new TimedTransition(3500, "circle_player")
                        )
                ),
                new Threshold(0.01,
                    new ItemLoot("Magic Potion", 0.03)
                )
            )
            .Init("Wasp Queen",
                new State(
                    new Spawn("Worker Wasp", maxChildren: 5, coolDown: 3400, givesNoXp: false),
                    new Spawn("Warrior Wasp", maxChildren: 2, coolDown: 4400, givesNoXp: false),
                    new State("idle",
                        new StayAbove(0.4, 60),
                        new Wander(0.55),
                        new PlayerWithinTransition(10, "froth")
                    ),
                    new State("froth",
                        new Shoot(8, predictive: 0.1, coolDown: 1600),
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Wander(0.55)
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.14),
                    new TierLoot(4, ItemType.Weapon, 0.14),
                    new TierLoot(5, ItemType.Weapon, 0.14),
                    new TierLoot(3, ItemType.Armor, 0.19),
                    new TierLoot(4, ItemType.Armor, 0.19),
                    new TierLoot(5, ItemType.Armor, 0.19),
                    new TierLoot(6, ItemType.Armor, 0.02),
                    new TierLoot(2, ItemType.Ring, 0.07),
                    new TierLoot(3, ItemType.Ability, 0.01),
                    new ItemLoot("Health Potion", 0.15),
                    new ItemLoot("Magic Potion", 0.07)
                )
            )
            .Init("Worker Wasp",
                new State(
                    new Shoot(8, coolDown: 4000),
                    new Prioritize(
                        new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0.5),
                        new Wander(0.75)
                    )
                )
            )
            .Init("Warrior Wasp",
                new State(
                    new Shoot(8, predictive: 200, coolDown: 1000),
                    new State("protecting",
                        new Prioritize(
                            new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(0.75)
                        ),
                        new TimedTransition(3000, "attacking")
                    ),
                    new State("attacking",
                        new Prioritize(
                            new Follow(0.8, acquireRange: 9, range: 3.4),
                            new Orbit(1, 2, target: "Wasp Queen", radiusVariance: 0),
                            new Wander(0.75)
                        ),
                        new TimedTransition(2200, "protecting")
                    )
                )
            )
            .Init("Earth Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new Shoot(8, count: 2, shootAngle: 12, coolDown: 600),
                        new State("first_satellites",
                            new Spawn("Green Satellite", maxChildren: 1, coolDown: 200),
                            new Spawn("Gray Satellite 3", maxChildren: 1, coolDown: 200),
                            new TimedTransition(300, "next_satellite")
                        ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 3", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                        ),
                        new State("follow",
                            new Prioritize(
                                new StayAbove(1.4, 65),
                                new Follow(1.4, range: 3),
                                new Wander(0.8)
                            ),
                            new TimedTransition(2000, "wander1")
                        ),
                        new State("wander1",
                            new Prioritize(
                                new StayAbove(1.55, 65),
                                new Wander(0.55)
                            ),
                            new TimedTransition(4000, "circle")
                        ),
                        new State("circle",
                            new Prioritize(
                                new StayAbove(1.2, 65),
                                new Orbit(1.2, 5.4, acquireRange: 11)
                            ),
                            new TimedTransition(4000, "wander2")
                        ),
                        new State("wander2",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new Wander(0.55)
                            ),
                            new TimedTransition(3000, "back_and_forth")
                        ),
                        new State("back_and_forth",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new BackAndForth(0.8)
                            ),
                            new TimedTransition(3000, "first_satellites")
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Green Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.1, 2, target: "Darkness Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(1.1, 2, target: "Earth Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Gray Satellite 3",
                new State(
                    new Shoot(7, count: 5, shootAngle: 72, coolDown: 3200, coolDownOffset: 600),
                    new Shoot(7, count: 4, shootAngle: 90, coolDown: 3200, coolDownOffset: 1400),
                    new Shoot(7, count: 5, shootAngle: 72, defaultAngle: 36, coolDown: 3200, coolDownOffset: 2200),
                    new Shoot(7, count: 4, shootAngle: 90, defaultAngle: 45, coolDown: 3200, coolDownOffset: 3000),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Red Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Green Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Paper Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new Spawn("Blue Satellite", maxChildren: 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", maxChildren: 1, coolDown: 200),
                        new Shoot(10, predictive: 0.5, coolDown: 700),
                        new Prioritize(
                            new StayAbove(1.4, 65),
                            new Follow(1, range: 3, duration: 3000, coolDown: 3000),
                            new Wander(0.4)
                        ),
                        new TimedTransition(12000, "idle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Blue Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.1, 2, target: "Clockwork Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(1.1, 2, target: "Paper Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Gray Satellite 1",
                new State(
                    new Shoot(6, count: 3, shootAngle: 34, predictive: 0.3, coolDown: 850),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Red Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Blue Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Fire Sprite",
                new State(
                    new Shoot(10, count: 2, shootAngle: 7, coolDown: 300),
                    new Prioritize(
                        new StayAbove(1.4, 55),
                        new Wander(1.4)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Ice Sprite",
                new State(
                    new Shoot(10, count: 3, shootAngle: 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Ability, 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Magic Sprite",
                new State(
                    new Reproduce(densityMax: 2),
                    new Shoot(10, count: 4, shootAngle: 7),
                    new Prioritize(
                        new StayAbove(1.4, 60),
                        new Wander(1.4)
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Shambling Sludge",
                new State(
                    new State("idle",
                        new StayAbove(0.5, 55),
                        new PlayerWithinTransition(10, "toss_sludge")
                    ),
                    new State("toss_sludge",
                        new Prioritize(
                            new StayAbove(0.5, 55),
                            new Wander(0.5)
                        ),
                        new Shoot(8, coolDown: 1200),
                        new TossObject("Sludget", range: 3, angle: 20, coolDown: 100000, throwEffect: true),
                        new TossObject("Sludget", range: 3, angle: 92, coolDown: 100000, throwEffect: true),
                        new TossObject("Sludget", range: 3, angle: 164, coolDown: 100000, throwEffect: true),
                        new TossObject("Sludget", range: 3, angle: 236, coolDown: 100000, throwEffect: true),
                        new TossObject("Sludget", range: 3, angle: 308, coolDown: 100000, throwEffect: true),
                        new TimedTransition(8000, "pause")
                    ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(0.5, 55),
                            new Wander(0.5)
                            ),
                        new TimedTransition(1000, "idle")
                        )
                ),
                new Threshold(0.01,
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new TierLoot(2, ItemType.Ability, 0.4),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Sludget",
                new State(
                    new State("idle",
                        new Shoot(8, predictive: 0.5, coolDown: 600),
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(0.5)
                        ),
                        new TimedTransition(1400, "wander")
                    ),
                    new State("wander",
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Wander(0.5)
                        ),
                        new TimedTransition(5400, "jump")
                    ),
                    new State("jump",
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Follow(7, acquireRange: 6, range: 1),
                            new Wander(0.5)
                        ),
                        new TimedTransition(200, "attack")
                    ),
                    new State("attack",
                        new Shoot(8, predictive: 0.5, coolDown: 600, coolDownOffset: 300),
                        new Prioritize(
                            new Protect(0.5, "Shambling Sludge", 11, 7.5, 7.4),
                            new Follow(0.5, acquireRange: 6, range: 1),
                            new Wander(0.5)
                        ),
                        new TimedTransition(4000, "idle")
                    ),
                    new Decay(9000)
                )
            )
            .Init("Big Green Slime",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(9),
                    new Wander(0.4),
                    new TransformOnDeath("Little Green Slime", 4, 4)
                )
            )
            .Init("Little Green Slime",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(6),
                    new Wander(0.4),
                    new Protect(0.4, "Big Green Slime")
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.3),
                    new ItemLoot("Magic Potion", 0.3)
                )
            )
            .Init("Gray Blob",
                new State(
                    new State("searching",
                        new StayAbove(0.2, 50),
                        new Prioritize(
                            new Charge(2),
                            new Wander(0.4)
                        ),
                        new PlayerWithinTransition(2, "creeping")
                    ),
                    new State("creeping",
                        new Shoot(0, count: 10, shootAngle: 36, fixedAngle: 0),
                        new Decay(0)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.3),
                    new ItemLoot("Magic Potion", 0.3),
                    new ItemLoot("Magic Mushroom", 0.01)
                )
            )
            .Init("Pink Blob",
                new State(
                    new StayAbove(0.4, 50),
                    new Shoot(6, count: 3, shootAngle: 7),
                    new Prioritize(
                        new Follow(0.8, acquireRange: 15, range: 5),
                        new Wander(0.4)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.3),
                    new ItemLoot("Magic Potion", 0.3)
                )
            )
            .Init("Swarm",
                new State(
                    new State("circle",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new Follow(4, acquireRange: 11, range: 3.5, duration: 1000, coolDown: 5000),
                            new Orbit(1.9, 3.5, acquireRange: 12),
                            new Wander(0.4)
                        ),
                        new Shoot(4, predictive: 0.1, coolDown: 500),
                        new TimedTransition(3000, "dart_away")
                    ),
                    new State("dart_away",
                        new Prioritize(
                            new StayAbove(0.4, 60),
                            new StayBack(2, distance: 5),
                            new Wander(0.4)
                        ),
                        new Shoot(8, count: 5, shootAngle: 72, fixedAngle: 20, coolDown: 100000, coolDownOffset: 800),
                        new Shoot(8, count: 5, shootAngle: 72, fixedAngle: 56, coolDown: 100000, coolDownOffset: 1400),
                        new TimedTransition(1600, "circle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.4),
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(3, ItemType.Armor, 0.4),
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(1, ItemType.Ring, 0.4),
                    new TierLoot(1, ItemType.Ability, 0.4),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Candy Gnome",
                new State(
                    new State("Ini",
                        new Wander(speed: 0.4),
                        new PlayerWithinTransition(dist: 14, targetState: "Main", seeInvis: true)
                    ),
                    new State("Main",
                        new Follow(speed: 1.4, acquireRange: 17, range: 6),
                        new TimedTransition(time: 1600, targetState: "Flee")
                    ),
                    new State("Flee",
                        new PlayerWithinTransition(dist: 11, targetState: "RunAwayMed", seeInvis: true),
                        new PlayerWithinTransition(dist: 8, targetState: "RunAwayMedFast", seeInvis: true),
                        new PlayerWithinTransition(dist: 5, targetState: "RunAwayFast", seeInvis: true),
                        new PlayerWithinTransition(dist: 16, targetState: "RunAwaySlow", seeInvis: true)
                    ),
                    new State("RunAwayFast",
                        new StayBack(speed: 1.9, distance: 30, entity: null),
                        new TimedRandomTransition(1000, false, "RunAwayMedFast", "RunAwayMed", "RunAwaySlow")
                    ),
                    new State("RunAwayMedFast",
                        new StayBack(speed: 1.45, distance: 30, entity: null),
                        new TimedRandomTransition(1000, false, "RunAwayMed", "RunAwaySlow")
                    ),
                    new State("RunAwayMed",
                        new StayBack(speed: 1.1, distance: 30, entity: null),
                        new TimedRandomTransition(1000, false, "RunAwayMedFast", "RunAwaySlow")
                    ),
                    new State("RunAwaySlow",
                        new StayBack(speed: 0.8, distance: 30, entity: null),
                        new TimedRandomTransition(1000, false, "RunAwayMedFast", "RunAwayMed")
                    ),
                    new DropPortalOnDeath(target: "Candyland Portal", probability: 1, timeout: 30)
                ),
                new Threshold(0.01,
                    new ItemLoot(item: "Rock Candy", probability: 0.4),
                    new ItemLoot(item: "Red Gumball", probability: 0.4),
                    new ItemLoot(item: "Purple Gumball", probability: 0.4),
                    new ItemLoot(item: "Blue Gumball", probability: 0.4),
                    new ItemLoot(item: "Green Gumball", probability: 0.4),
                    new ItemLoot(item: "Yellow Gumball", probability: 0.4)
                )
            )
            //Mid Forest
            .Init("Dwarf King",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 0.20),
                    new SpawnGroup("Dwarves", maxChildren: 10, coolDown: 8000),
                    new Shoot(4, coolDown: 2000),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, acquireRange: 11),
                            new Wander(0.4)
                        ),
                        new TimedTransition(3400, "Engaging")
                    ),
                    new State("Engaging",
                        new Taunt(0.2, "You'll taste my axe!"),
                        new Prioritize(
                            new Follow(1.0, acquireRange: 15, range: 1),
                            new Wander(0.4)
                        ),
                        new TimedTransition(2600, "Circling")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.4),
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(2, ItemType.Armor, 0.4),
                    new TierLoot(3, ItemType.Armor, 0.4),
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(1, ItemType.Ring, 0.4),
                    new TierLoot(1, ItemType.Ability, 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Dwarf Axebearer",
                new State(
                    new Shoot(3.4),
                    new State("Default",
                        new Wander(0.4)
                    ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, acquireRange: 11),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.4)
                        ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, acquireRange: 15, range: 1),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.4)
                        ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Dwarf Mage",
                new State(
                    new State("Default",
                        new Prioritize(
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.6)
                        ),
                        new State("fire1_def",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_def")
                        ),
                        new State("fire2_def",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_def")
                        )
                    ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, acquireRange: 11),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.6)
                        ),
                        new State("fire1_cir",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_cir")
                        ),
                        new State("fire2_cir",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_cir")
                        ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, acquireRange: 15, range: 1),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.4)
                        ),
                        new State("fire1_eng",
                            new Shoot(10, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire2_eng")
                        ),
                        new State("fire2_eng",
                            new Shoot(5, predictive: 0.2, coolDown: 100000),
                            new TimedTransition(1500, "fire1_eng")
                        ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Dwarf Veteran",
                new State(
                    new Shoot(4),
                    new State("Default",
                        new Prioritize(
                            new Follow(1.0, acquireRange: 9, range: 2, duration: 3000, coolDown: 1000),
                            new Wander(0.4)
                            )
                    ),
                    new State("Circling",
                        new Prioritize(
                            new Orbit(0.4, 2.7, acquireRange: 11),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.4)
                        ),
                        new TimedTransition(3300, "Default"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    ),
                    new State("Engaging",
                        new Prioritize(
                            new Follow(1.0, acquireRange: 15, range: 1),
                            new Protect(1.2, "Dwarf King", acquireRange: 15, protectionRange: 6, reprotectRange: 3),
                            new Wander(0.4)
                        ),
                        new TimedTransition(2500, "Circling"),
                        new EntityNotExistsTransition("Dwarf King", 8, "Default")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Werelion",
                new State(
                    new DropPortalOnDeath("Spider Den Portal", 0.1),
                    new Spawn("Weretiger", maxChildren: 1, coolDown: 23000, givesNoXp: false),
                    new Spawn("Wereleopard", maxChildren: 2, coolDown: 9000, givesNoXp: false),
                    new Spawn("Werepanther", maxChildren: 3, coolDown: 15000, givesNoXp: false),
                    new Shoot(4, coolDown: 2000),
                    new State("idle",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                        ),
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new State("normal_attack",
                            new Shoot(10, count: 3, shootAngle: 15, predictive: 1, coolDown: 10000),
                            new TimedTransition(900, "if_cloaked")
                        ),
                        new State("if_cloaked",
                            new Shoot(10, count: 8, shootAngle: 45, defaultAngle: 20, coolDown: 1600, coolDownOffset: 400),
                            new Shoot(10, count: 8, shootAngle: 45, defaultAngle: 42, coolDown: 1600, coolDownOffset: 1200),
                            new PlayerWithinTransition(10, "normal_attack")
                        ),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(0.4, acquireRange: 7, range: 3),
                            new Wander(0.6)
                        ),
                        new TimedTransition(30000, "idle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new TierLoot(2, ItemType.Ability, 0.4),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Weretiger",
                new State(
                    new Shoot(8, predictive: 0.3, coolDown: 1000),
                    new Prioritize(
                        new StayAbove(0.6, 60),
                        new Protect(1.1, "Werelion", acquireRange: 12, protectionRange: 10, reprotectRange: 5),
                        new Follow(0.8, range: 6.3),
                        new Wander(0.6)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Wereleopard",
                new State(
                    new Shoot(4.5, predictive: 0.4, coolDown: 900),
                    new Prioritize(
                        new Protect(1.1, "Werelion", acquireRange: 12, protectionRange: 10, reprotectRange: 5),
                        new Follow(1.1, range: 3),
                        new Wander(1)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Werepanther",
                new State(
                    new State("idle",
                        new Protect(0.65, "Werelion", acquireRange: 11, protectionRange: 7.5, reprotectRange: 7.4),
                        new PlayerWithinTransition(9.5, "wander")
                    ),
                    new State("wander",
                        new Prioritize(
                            new Protect(0.65, "Werelion", acquireRange: 11, protectionRange: 7.5, reprotectRange: 7.4),
                            new Follow(0.65, range: 5, acquireRange: 10),
                            new Wander(0.65)
                        ),
                        new PlayerWithinTransition(4, "jump")
                    ),
                    new State("jump",
                        new Prioritize(
                            new Protect(0.65, "Werelion", acquireRange: 11, protectionRange: 7.5, reprotectRange: 7.4),
                            new Follow(7, range: 1, acquireRange: 6),
                            new Wander(0.55)
                        ),
                        new TimedTransition(200, "attack")
                    ),
                    new State("attack",
                        new Prioritize(
                            new Protect(0.65, "Werelion", acquireRange: 11, protectionRange: 7.5, reprotectRange: 7.4),
                            new Follow(0.5, range: 1, acquireRange: 6),
                            new Wander(0.5)
                        ),
                        new Shoot(4, predictive: 0.5, coolDown: 800, coolDownOffset: 300),
                        new TimedTransition(4000, "idle")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Metal Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new Shoot(8, count: 1, coolDown: 1000),
                        new State("first_satellites",
                            new Spawn("Red Satellite", maxChildren: 1, coolDown: 200),
                            new Spawn("Gray Satellite 1", maxChildren: 1, coolDown: 200),
                            new TimedTransition(300, "next_satellite")
                        ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 3", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                        ),
                        new State("follow",
                            new Prioritize(
                                new StayAbove(1.4, 65),
                                new Follow(1.4, range: 3),
                                new Wander(0.8)
                            ),
                            new TimedTransition(2000, "wander1")
                        ),
                        new State("wander1",
                            new Prioritize(
                                new StayAbove(1.55, 65),
                                new Wander(0.55)
                            ),
                            new TimedTransition(4000, "circle")
                        ),
                        new State("circle",
                            new Prioritize(
                                new StayAbove(1.2, 65),
                                new Orbit(1.2, 5.4, acquireRange: 11)
                            ),
                            new TimedTransition(4000, "wander2")
                        ),
                        new State("wander2",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new Wander(0.55)
                            ),
                            new TimedTransition(3000, "back_and_forth")
                        ),
                        new State("back_and_forth",
                            new Prioritize(
                                new StayAbove(0.55, 65),
                                new BackAndForth(0.8)
                            ),
                            new TimedTransition(3000, "first_satellites")
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Red Satellite",
                new State(
                    new Prioritize(
                        new Orbit(1.7, 2, target: "Fire Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(1.7, 2, target: "Metal Golem", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Clockwork Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new Spawn("Blue Satellite", maxChildren: 1, coolDown: 200),
                        new Spawn("Gray Satellite 2", maxChildren: 1, coolDown: 200),
                        new Shoot(10, coolDown: 1500),
                        new Prioritize(
                            new StayAbove(1.4, 65),
                            new Follow(1, range: 3, duration: 3000, coolDown: 3000),
                            new Wander(0.4)
                        ),
                        new TimedTransition(12000, "idle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Gray Satellite 2",
                new State(
                    new Shoot(7, predictive: 0.3, coolDown: 600),
                    new Prioritize(
                        new Orbit(2.2, 0.75, target: "Green Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0),
                        new Orbit(2.2, 0.75, target: "Blue Satellite", acquireRange: 15, speedVariance: 0, radiusVariance: 0)
                    ),
                    new Decay(16000)
                )
            )
            .Init("Horned Drake",
                new State(
                    new Spawn("Drake Baby", maxChildren: 1, initialSpawn: 1, coolDown: 50000, givesNoXp: false),
                    new State("idle",
                        new StayAbove(0.8, 60),
                        new PlayerWithinTransition(10, "get_player")
                    ),
                    new State("get_player",
                        new Prioritize(
                            new StayAbove(0.8, 60),
                            new Follow(0.8, range: 2.7, acquireRange: 10, duration: 5000, coolDown: 1800),
                            new Wander(0.8)
                        ),
                        new State("one_shot",
                            new Shoot(8, predictive: 0.1, coolDown: 800),
                            new TimedTransition(900, "three_shot")
                        ),
                        new State("three_shot",
                            new Shoot(8, count: 3, shootAngle: 40, predictive: 0.1, coolDown: 100000, coolDownOffset: 800),
                            new TimedTransition(2000, "one_shot")
                        )
                    ),
                    new State("protect_me",
                        new Protect(0.8, "Drake Baby", acquireRange: 12, protectionRange: 2.5, reprotectRange: 1.5),
                        new State("one_shot",
                            new Shoot(8, predictive: 0.1, coolDown: 700),
                            new TimedTransition(800, "three_shot")
                        ),
                        new State("three_shot",
                            new Shoot(8, count: 3, shootAngle: 40, predictive: 0.1, coolDown: 100000,
                                coolDownOffset: 700),
                            new TimedTransition(1800, "one_shot")
                        ),
                        new EntityNotExistsTransition("Drake Baby", 8, "idle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(6, ItemType.Weapon, 0.4),
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new TierLoot(3, ItemType.Ring, 0.25),
                    new TierLoot(2, ItemType.Ability, 0.4),
                    new TierLoot(3, ItemType.Ability, 0.25),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Drake Baby",
                new State(
                    new DropPortalOnDeath("Forest Maze Portal", 0.20),
                    new State("unharmed",
                        new Shoot(8, coolDown: 1500),
                        new State("wander",
                            new Prioritize(
                                new StayAbove(0.8, 60),
                                new Wander(0.8)
                            ),
                            new TimedTransition(2000, "find_mama")
                        ),
                        new State("find_mama",
                            new Prioritize(
                                new StayAbove(0.8, 60),
                                new Protect(1.4, "Horned Drake", acquireRange: 15, protectionRange: 4, reprotectRange: 4)
                            ),
                            new TimedTransition(2000, "wander")
                        ),
                        new HpLessTransition(0.65, "call_mama")
                    ),
                    new State("call_mama",
                        new Flash(0xff484848, 0.6, 5000),
                        new State("get_close_to_mama",
                            new Taunt("Awwwk! Awwwk!"),
                            new Protect(1.4, "Horned Drake", acquireRange: 15, protectionRange: 1, reprotectRange: 1),
                            new TimedTransition(1500, "cry_for_mama")
                        ),
                        new State("cry_for_mama",
                            new StayBack(0.65, 8),
                            new Order(8, "Horned Drake", "protect_me")
                        )
                    )
               )
            )
            .Init("Red Spider",
                new State(
                    new Wander(0.8),
                    new Shoot(9)
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Black Bat",
                new State(
                    new Prioritize(
                        new Charge(),
                        new Wander(0.4)
                    ),
                    new Shoot(1)
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            //Mid Sand
            .Init("Desert Werewolf",
                new State(
                    new SpawnGroup("Wargs", maxChildren: 8, coolDown: 8000),
                    new State("unharmed",
                        new Shoot(8, projectileIndex: 0, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Prioritize(
                            new Follow(0.5, acquireRange: 10.5, range: 2.5),
                            new Wander(0.5)
                        ),
                        new HpLessTransition(0.75, "enraged")
                    ),
                    new State("enraged",
                        new Shoot(8, projectileIndex: 0, predictive: 0.3, coolDown: 1000, coolDownOffset: 500),
                        new Taunt(0.7, "GRRRRAAGH!"),
                        new ChangeSize(20, 170),
                        new Flash(0xffff0000, 0.4, 5000),
                        new Prioritize(
                            new Follow(0.65, acquireRange: 9, range: 2),
                            new Wander(0.65)
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(3, ItemType.Weapon, 0.2),
                    new TierLoot(4, ItemType.Weapon, 0.12),
                    new TierLoot(3, ItemType.Armor, 0.2),
                    new TierLoot(4, ItemType.Armor, 0.15),
                    new TierLoot(5, ItemType.Armor, 0.02),
                    new TierLoot(1, ItemType.Ring, 0.11),
                    new TierLoot(1, ItemType.Ability, 0.38),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Tawny Warg",
                new State(
                    new Shoot(3.4),
                    new Prioritize(
                        new Protect(1.2, "Desert Werewolf", acquireRange: 14, protectionRange: 8, reprotectRange: 5),
                        new Follow(0.7, acquireRange: 9, range: 2),
                        new Wander(0.8)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Demon Warg",
                new State(
                    new Shoot(4.5),
                    new Prioritize(
                        new Protect(1.2, "Desert Werewolf", acquireRange: 14, protectionRange: 8, reprotectRange: 5),
                        new Wander(0.8)
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Health Potion", 0.04)
                )
            )
            .Init("Fire Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new Prioritize(
                            new StayAbove(1.4, 65),
                            new Follow(1, range: 3, duration: 3000, coolDown: 3000),
                            new Wander(0.4)
                        ),
                        new Spawn("Red Satellite", maxChildren: 1, coolDown: 200),
                        new Spawn("Gray Satellite 1", maxChildren: 1, coolDown: 200),
                        new State("slowshot",
                            new Shoot(10, projectileIndex: 0, predictive: 0.5, coolDown: 300, coolDownOffset: 600),
                            new TimedTransition(5000, "megashot")
                        ),
                        new State("megashot",
                            new Flash(0xffffffff, 0.2, 5),
                            new Shoot(10, projectileIndex: 1, predictive: 0.2, coolDown: 90, coolDownOffset: 1000),
                            new TimedTransition(1200, "slowshot")
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(4, ItemType.Armor, 0.4),
                    new ItemLoot("Health Potion", 0.4)
                )
            )
            .Init("Darkness Golem",
                new State(
                    new State("idle",
                        new PlayerWithinTransition(11, "player_nearby")
                    ),
                    new State("player_nearby",
                        new State("first_satellites",
                            new Spawn("Green Satellite", maxChildren: 1, coolDown: 200),
                            new Spawn("Gray Satellite 2", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "next_satellite")
                        ),
                        new State("next_satellite",
                            new Spawn("Gray Satellite 2", maxChildren: 1, coolDown: 200),
                            new TimedTransition(200, "follow")
                        ),
                        new State("follow",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(1.2, 65),
                                new Follow(1.2, range: 1),
                                new Wander(0.5)
                            ),
                            new TimedTransition(3000, "wander1")
                        ),
                        new State("wander1",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(0.65, 65),
                                new Wander(0.65)
                            ),
                            new TimedTransition(3800, "back_up")
                        ),
                        new State("back_up",
                            new Flash(0xffffffff, 0.2, 25),
                            new Shoot(9, projectileIndex: 1, coolDown: 1400, coolDownOffset: 1000),
                            new Prioritize(
                                new StayAbove(0.4, 65),
                                new StayBack(0.4, 4),
                                new Wander(0.4)
                            ),
                            new TimedTransition(5400, "wander2")
                        ),
                        new State("wander2",
                            new Shoot(6, projectileIndex: 0, coolDown: 200),
                            new Prioritize(
                                new StayAbove(0.65, 65),
                                new Wander(0.65)
                            ),
                            new TimedTransition(3800, "first_satellites")
                        )
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Sand Phantom",
                new State(
                    new Prioritize(
                        new StayAbove(0.85, 60),
                        new Follow(0.85, acquireRange: 10.5, range: 1),
                        new Wander(0.85)
                    ),
                    new Shoot(8, predictive: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("follow_player",
                        new PlayerWithinTransition(4.4, "sneak_away_from_player")
                    ),
                    new State("sneak_away_from_player",
                        new Transform("Sand Phantom Wisp")
                    )
                )
            )
            .Init("Sand Phantom Wisp",
                new State(
                    new Shoot(8, predictive: 0.4, coolDown: 400, coolDownOffset: 600),
                    new State("move_away_from_player",
                        new State("keep_back",
                            new Prioritize(
                                new StayBack(0.6, distance: 5),
                                new Wander(0.9)
                            ),
                            new TimedTransition(800, "wander")
                        ),
                        new State("wander",
                            new Wander(0.9),
                            new TimedTransition(800, "keep_back")
                        ),
                        new TimedTransition(6500, "wisp_finished")
                    ),
                    new State("wisp_finished",
                        new Transform("Sand Phantom")
                    )
                )
            )
            .Init("Great Lizard",
                new State(
                    new State("idle",
                        new StayAbove(0.6, 60),
                        new Wander(0.6),
                        new PlayerWithinTransition(10, "charge")
                    ),
                    new State("charge",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(6, acquireRange: 11, range: 1.5)
                        ),
                        new TimedTransition(200, "spit")
                    ),
                    new State("spit",
                        new Shoot(8, projectileIndex: 0, count: 1, coolDown: 100000, coolDownOffset: 1000),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 16, coolDown: 100000, coolDownOffset: 1200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.2, coolDown: 100000, coolDownOffset: 1600),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 24, coolDown: 100000, coolDownOffset: 2200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.2, coolDown: 100000, coolDownOffset: 2800),
                        new Shoot(8, projectileIndex: 0, count: 2, shootAngle: 16, coolDown: 100000, coolDownOffset: 3200),
                        new Shoot(8, projectileIndex: 0, count: 1, predictive: 0.1, coolDown: 100000, coolDownOffset: 3800),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                        ),
                        new TimedTransition(5000, "flame_ring")
                    ),
                    new State("flame_ring",
                        new Shoot(7, projectileIndex: 1, count: 30, shootAngle: 12, coolDown: 400, coolDownOffset: 600),
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Follow(0.6, acquireRange: 9, range: 1),
                            new Wander(0.6)
                        ),
                        new TimedTransition(3500, "pause")
                    ),
                    new State("pause",
                        new Prioritize(
                            new StayAbove(0.6, 60),
                            new Wander(0.6)
                        ),
                        new TimedTransition(1000, "idle")
                    )
                ),
                new Threshold(0.01,
                    new TierLoot(4, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Weapon, 0.4),
                    new TierLoot(5, ItemType.Armor, 0.4),
                    new TierLoot(6, ItemType.Armor, 0.4),
                    new TierLoot(2, ItemType.Ring, 0.4),
                    new TierLoot(2, ItemType.Ability, 0.4),
                    new ItemLoot("Health Potion", 0.4),
                    new ItemLoot("Magic Potion", 0.4)
                )
            )
            .Init("Nomadic Shaman",
                new State(
                    new Prioritize(
                        new StayAbove(0.8, 55),
                        new Wander(0.7)
                    ),
                    new State("fire1",
                        new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 11, coolDown: 500, coolDownOffset: 500),
                        new TimedTransition(3100, "fire2")
                    ),
                    new State("fire2",
                        new Shoot(10, projectileIndex: 1, coolDown: 700, coolDownOffset: 700),
                        new TimedTransition(2200, "fire1")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Magic Potion", 0.4)
                )
            );
    }
}