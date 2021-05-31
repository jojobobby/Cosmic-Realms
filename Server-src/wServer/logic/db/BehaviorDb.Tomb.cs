using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TomboftheAncients = () => Behav()
            .Init("Tomb Defender",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("idle",
                        new Taunt(true, "THIS WILL NOW BE YOUR TOMB!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored, true),
                        new Prioritize(
                            new Orbit(.3, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                            new Wander(0.3)
                        ),
                        new HpLessTransition(.989, "weakning")
                    ),
                    new State("weakning",
                        new Prioritize(
                            new Orbit(.3, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt(true, "Impudence! I am an Immortal, I needn't waste time on you!"),
                        new Shoot(12, 20, projectileIndex: 3, coolDown: 10000),
                        new Shoot(12, 20, projectileIndex: 3, coolDown: 10000, coolDownOffset: 1000),
                        new HpLessTransition(.979, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 12000),
                        new Prioritize(
                            new Orbit(.3, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new Shoot(3.8, 8, 45, 2, 0, 0, coolDown: 1200),
                        new Shoot(12, 3, 100, 1, 0, 0, coolDown: 5400, predictive: 0.6),
                        new Shoot(0, 6, 60, 0, 0, 0, coolDown: 5000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new HpLessTransition(.7, "boomerang")
                    ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.325, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.325)
                        ),
                        new Shoot(0, 6, shootAngle: 60, projectileIndex: 0, coolDown: 8000),
                        new Shoot(12, 1, projectileIndex: 0, coolDown: 3000, predictive: 0.6),
                        new Shoot(3.8, 8, projectileIndex: 2, coolDown: 1200),
                        new Shoot(12, 4, 60, 1, coolDown: 6000, predictive: 0.6),
                        new Shoot(12, 3, 10, 1, coolDown: 6000, predictive: 0.6),
                        new HpLessTransition(.55, "double shot")
                    ),
                    new State("double shot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 4000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new Prioritize(
                            new Orbit(.35, 4, target: "Tomb Boss Anchor"),
                            new Wander(0.35)
                        ),
                        new Shoot(3.8, 9, shootAngle: 40, projectileIndex: 2, coolDown: 1000),
                        new Shoot(12, 2, 5, 0, coolDown: 3200, predictive: 0.6),
                        new Shoot(12, 4, 30, 1, coolDown: 6000, predictive: 0.6),
                        new Shoot(12, 2, 10, 1, coolDown: 6000, predictive: 0.6),
                        new HpLessTransition(.4, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 10000),
                        new Prioritize(
                            new Orbit(.375, 6, target: "Tomb Boss Anchor"),
                            new Wander(0.375)
                        ),
                        new Shoot(3.8, 10, shootAngle: 36, projectileIndex: 2, coolDown: 1000),
                        new Shoot(12, 2, 5, 0, coolDown: 3000, predictive: 0.6),
                        new Shoot(12, 6, 24, 1, coolDown: 6000, predictive: 0.6),
                        new Shoot(12, 3, 10, 1, coolDown: 6000, predictive: 0.6),
                        new Spawn("Pyramid Artifact 1", 3, 0),
                        new Spawn("Pyramid Artifact 2", 2, 0),
                        new Spawn("Pyramid Artifact 3", 1, 0),
                        new HpLessTransition(.25, "artifacts 2")
                    ),
                    new State("artifacts 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken, duration: 10000),
                        new Taunt(true, "My artifacts shall prove my wall of defense is impenetrable!"),
                        new Prioritize(
                            new Orbit(.4, 6, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(4, 10, shootAngle: 36, projectileIndex: 2, coolDown: 900),
                        new Shoot(12, 3, 15, 0, coolDown: 2800, predictive: 0.6),
                        new Shoot(12, 7, 24, 1, coolDown: 6000, predictive: 0.6),
                        new Shoot(12, 3, 10, 1, coolDown: 6000, predictive: 0.6),
                        new Spawn("Pyramid Artifact 1", 3, 0),
                        new Spawn("Pyramid Artifact 2", 2, 0),
                        new Spawn("Pyramid Artifact 3", 1, 0),
                        new HpLessTransition(.06, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 5400),
                        new Taunt(true, "The end of your path is here!"),
                        new Prioritize(
                            new StayCloseToSpawn(0.8, 7),
                            new Follow(0.5, 10, 2.4)
                        ),
                        new Spawn("Pyramid Artifact 1", 3, 0),
                        new Spawn("Pyramid Artifact 2", 2, 0),
                        new Spawn("Pyramid Artifact 3", 2, 0),
                        new Flash(0xFF0000, 10, 6000),
                        new Shoot(0, 6, 60, 0, coolDown: 8000),
                        new Shoot(20, 1, 60, 0, coolDown: 1400, predictive: 0.6),
                        new Shoot(20, 12, 6, 4, coolDown: 800),
                        new Shoot(20, 7, 24, 1, coolDown: 5500, predictive: 0.6),
                        new Shoot(20, 3, 10, 1, coolDown: 5500, predictive: 0.6),
                        new Shoot(0, 5, 5, 4, 0, 15, coolDown: 400)
                    )
                ),
                new Threshold(0.0001,
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Golden Ankh", 0.05),
                       new ItemLoot("Light Armor Schematic", 0.006, damagebased: true),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Ring of the Pyramid", 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot("Tome of Holy Protection", 0.004, damagebased: true),
                    new ItemLoot("Sword of Royal Majesty", 0.006, damagebased: true),
                    new ItemLoot("Ring of Ancient Slaves", 0.006, damagebased: true),
                    new ItemLoot("Seal of the Royal Priest", 0.006, damagebased: true),
                    new ItemLoot("Unbound Temple Armor", 0.006, damagebased: true)
                )
            )
            .Init("Tomb Support",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("idle",
                        new Taunt(true, "ENOUGH OF YOUR VANDALISM!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Orbit(.3, 4.8, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new HpLessTransition(.989, "weakning")
                    ),
                    new State("weakning",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new ConditionalEffect(ConditionEffectIndex.Armored, duration: 12000),
                        new Shoot(12, 20, projectileIndex: 7, coolDown: 10000),
                        new Shoot(12, 20, projectileIndex: 7, coolDown: 10000, coolDownOffset: 1000),
                        new Taunt("Impudence! I am an immortal, I needn't take your seriously."),
                        new Prioritize(
                            new Orbit(.3, 4.8, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new HpLessTransition(.979, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4, 4.8, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(12, 6, projectileIndex: 6, coolDown: 6000, predictive: 0.6),
                        new Shoot(10, 3, 120, 1, 0, coolDown: 6000, coolDownOffset: 4000),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 5000, coolDownOffset: 6000),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 4000, coolDownOffset: 8000),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 8000, coolDownOffset: 10000),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 800),
                        new HpLessTransition(.9, "boomerang")
                    ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4, 4.2, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new HealEntity(10, "Tomb Defender", 100, coolDown: 500),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 5000, coolDownOffset: 4200),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 4000, coolDownOffset: 3400),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 8000, coolDownOffset: 2800),
                        new HpLessTransition(.7, "paralyze")
                    ),
                    new State("paralyze",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.45, 4.2, target: "Tomb Boss Anchor"),
                            new Wander(0.45)
                        ),
                        new Shoot(12, 1, projectileIndex: 6, coolDown: 5600, predictive: 0.6),
                        new Shoot(10, 3, 120, 1, 0, coolDown: 5600, coolDownOffset: 2000),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 4700, coolDownOffset: 3000),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 3800, coolDownOffset: 4000),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 7800, coolDownOffset: 5000),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 800),
                        new HpLessTransition(.5, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.5, 4.2, target: "Tomb Boss Anchor"),
                            new Wander(0.5)
                        ),
                        new Shoot(12, 1, projectileIndex: 6, coolDown: 5300, predictive: 0.6),
                        new Shoot(10, 3, 120, 1, 0, coolDown: 5300),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 4700),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 3600),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 7200),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 700),
                        new HealEntity(10, "Tomb Attacker", 40, coolDown: 500),
                        new Spawn("Sphinx Artifact 1", 3, 0),
                        new Spawn("Sphinx Artifact 2", 3, 0),
                        new Spawn("Sphinx Artifact 3", 3, 0),
                        new HpLessTransition(.3, "double shoot")
                    ),
                    new State("double shoot",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4, 4.2, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(12, 1, projectileIndex: 6, coolDown: 5000, predictive: 0.6),
                        new Shoot(10, 3, 120, 1, 0, coolDown: 5000),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 4800),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 3400),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 6400),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 600),
                        new Spawn("Sphinx Artifact 1", 3, 0),
                        new Spawn("Sphinx Artifact 2", 3, 0),
                        new Spawn("Sphinx Artifact 3", 3, 0),
                        new HpLessTransition(.06, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Flash(0xFF0000, 10, 6000),
                        new Taunt(true, "This cannot be! You shall not succeed!"),
                        new Prioritize(
                            new StayCloseToSpawn(0.8, 10),
                            new Follow(0.65, 10, 2.4, duration: 30000, coolDown: 6000)
                        ),
                        new Spawn("Sphinx Artifact 1", 3, 0),
                        new Spawn("Sphinx Artifact 2", 3, 0),
                        new Spawn("Sphinx Artifact 3", 3, 0),
                        new Shoot(12, 2, shootAngle: 10, projectileIndex: 6, coolDown: 4000, predictive: 0.6),
                        new Shoot(12, 2, shootAngle: 15, projectileIndex: 0, coolDown: 700, predictive: 0.6),
                        new Shoot(10, 3, 120, 1, 0, coolDown: 5000),
                        new Shoot(10, 4, 90, 2, 0, coolDown: 3600),
                        new Shoot(10, 5, 72, 3, 0, coolDown: 2800),
                        new Shoot(10, 6, 60, 4, 0, coolDown: 6000),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 600)
                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Eye of Osiris", 0.05),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Robe Schematic", 0.006, damagebased: true),
                    new ItemLoot("50 Credits", 0.01),//Quiver of The Ancient Archer
                    new ItemLoot("Quiver of The Ancient Archer", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Ring of the Sphinx", 0.004, damagebased: true),
                    new ItemLoot("Death's Sepulcher", 0.006, damagebased: true),
                    new ItemLoot("Swift Death's Sepulcher", 0.002, damagebased: true, threshold: 0.01)
                )
            )
            .Init("Tomb Attacker",
                new State(
                    new ScaleHP2(40,3,15),
                    new State("idle",
                        new Taunt(true, "YOU HAVE AWAKENED US!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Prioritize(
                            new Orbit(.3, 5.8, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new HpLessTransition(.989, "weakning")
                    ),
                    new State("weakning",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.3, 5.8, target: "Tomb Boss Anchor"),
                            new Wander(0.3)
                        ),
                        new Shoot(12, 20, projectileIndex: 3, coolDown: 10000),
                        new Shoot(12, 20, projectileIndex: 3, coolDown: 10000, coolDownOffset: 1000),
                        new HpLessTransition(.979, "active")
                    ),
                    new State("active",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Prioritize(
                            new Orbit(.4, 5.8, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(12, 2, 5, 2, coolDown: 600, predictive: 0.6),
                        new Grenade(3, 120, 10, coolDown: 3500),
                        new Grenade(4, 120, 10, coolDown: 6000),
                        new Shoot(0, 6, shootAngle: 60, projectileIndex: 0, coolDown: 5000),
                        new HpLessTransition(.72, "lets dance")
                    ),
                    new State("lets dance",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4, 5.8, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(12, 2, 40, 0, coolDown: 2000, predictive: 0.6),
                        new Shoot(12, 8, 45, 1, 0, coolDown: 5000),
                        new Shoot(12, 4, 80, 2, coolDown: 1000, predictive: 0.6),
                        new Shoot(12, 1, 40, 0, coolDown: 1400),
                        new Shoot(12, 1, 80, 2, coolDown: 400, predictive: 0.6),
                        new Grenade(3, 100, 10, coolDown: 4000),
                        new Grenade(4, 120, 10, coolDown: 6000),
                        new Spawn("Scarab", 4, 0),
                        new HpLessTransition(.6, "more muthafucka")
                    ),
                    new State("more muthafucka",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.4, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.4)
                        ),
                        new Shoot(12, 2, 40, 0, coolDown: 2000, predictive: 0.6),
                        new Shoot(12, 10, 36, 1, 0, coolDown: 5000),
                        new Shoot(12, 5, 60, 2, coolDown: 1200, predictive: 0.6),
                        new Shoot(12, 1, 40, 0, coolDown: 1200),
                        new Shoot(12, 1, 80, 2, coolDown: 600, predictive: 0.6),
                        new Grenade(3, 100, 10, coolDown: 3750),
                        new Grenade(4, 120, 10, coolDown: 5750),
                        new Spawn("Scarab", 4, 0),
                        new HpLessTransition(.4, "artifacts")
                    ),
                    new State("artifacts",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.5, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.5)
                        ),
                        new Shoot(12, 4, 35, 0, coolDown: 2000, predictive: 0.6),
                        new Shoot(12, 10, 36, 1, 0, coolDown: 15000),
                        new Shoot(12, 5, 60, 2, coolDown: 1200, predictive: 0.6),
                        new Shoot(12, 1, 40, 0, coolDown: 1400),
                        new Shoot(12, 2, 15, 2, coolDown: 600, predictive: 0.6),
                        new Grenade(3, 100, 10, coolDown: 3500),
                        new Grenade(4, 120, 10, coolDown: 5500),
                        new Grenade(6, 40, 10, coolDown: 2000),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0),
                        new Spawn("Nile Artifact 2", 2, 0),
                        new Spawn("Nile Artifact 3", 2, 0),
                        new HpLessTransition(.2, "artifacts 2")
                    ),
                    new State("artifacts 2",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6000),
                        new Prioritize(
                            new Orbit(.55, 5, target: "Tomb Boss Anchor"),
                            new Wander(0.55)
                        ),
                        new Shoot(12, 4, 35, 0, coolDown: 1800, predictive: 0.6),
                        new Shoot(12, 10, 36, 1, 0, coolDown: 14000),
                        new Shoot(12, 5, 60, 2, coolDown: 1200, predictive: 0.6),
                        new Shoot(12, 1, 40, 0, coolDown: 1400),
                        new Shoot(12, 1, 15, 2, coolDown: 600, predictive: 0.6),
                        new Grenade(3, 100, 10, coolDown: 3000),
                        new Grenade(4, 120, 10, coolDown: 5000),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0),
                        new Spawn("Nile Artifact 2", 2, 0),
                        new Spawn("Nile Artifact 3", 2, 0),
                        new HpLessTransition(.06, "rage")
                    ),
                    new State("rage",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt(true, "Argh! You shall pay for your crimes!"),
                        new Flash(0xFF0000, 10, 6000),
                        new Prioritize(
                            new StayCloseToSpawn(2, 7),
                            new StayBack(0.9, 1.8, null)
                        ),
                        new Shoot(12, 2, 35, 0, coolDown: 1000, predictive: 0.6),
                        new Shoot(12, 10, 36, 1, 0, coolDown: 4800),
                        new Shoot(12, 5, 42, 2, coolDown: 1200, predictive: 0.6),
                        new Shoot(12, 2, 5, 0, coolDown: 1400),
                        new Shoot(12, 1, 15, 2, coolDown: 400, predictive: 0.6),
                        new Grenade(3, 120, 12, coolDown: 3500),
                        new Grenade(4, 150, 12, coolDown: 5500),
                        new Spawn("Scarab", 4, 0),
                        new Spawn("Nile Artifact 1", 2, 0),
                        new Spawn("Nile Artifact 2", 2, 0),
                        new Spawn("Nile Artifact 3", 2, 0),
                        new Shoot(0, 1, 0, projectileIndex: 5, 0, 15, coolDown: 400)
                    )
                ),
                new Threshold(0.00001,
                    new ItemLoot("Heavy Armor Schematic", 0.006, damagebased: true),
                    new ItemLoot("Pharaoh's Mask", 0.05),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("50 Credits", 0.01),//Quiver of The Ancient Archer
                    new ItemLoot("Mark of Geb", 0, 1),
                    new ItemLoot("Ring of the Nile", 0.004, damagebased: true),
                    new ItemLoot("Book of Geb", 0.006, damagebased: true),
                    new ItemLoot("Scepter of Geb", 0.006, damagebased: true),
                    new ItemLoot("Shendyt of Geb", 0.006, damagebased: true),
                    new ItemLoot("Geb's Ring of Wisdom", 0.006, damagebased: true)
                )
            )
            //Minions
            .Init("Pyramid Artifact 1",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Pyramid Artifact 2",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Pyramid Artifact 3",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, 3, 120, coolDown: 2500)
                    )
            )
        .Init("Sphinx Artifact 1",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Sphinx Artifact 2",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Sphinx Artifact 3",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Nile Artifact 1",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Nile Artifact 2",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Nile Artifact 3",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(12, 3, 120, coolDown: 2500)
                    )
            )
            .Init("Tomb Defender Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Defender")
                        )
                    ))
            .Init("Tomb Support Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Support")
                        )
                    ))
            .Init("Tomb Attacker Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Transform("Tomb Attacker")
                        )
                    )
            )
            .Init("Scarab",
                new State(
                    new NoPlayerWithinTransition(7, "Idle"),
                    new PlayerWithinTransition(7, "Chase"),
                    new State("Idle",
                        new Wander(.1)
                    ),
                    new State("Chase",
                        new Follow(1.5, 7, 0),
                        new Shoot(3, projectileIndex: 1, coolDown: 500)
                    )
                )
            )
            .Init("Tomb Boss Anchor",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                    new State("Idle",
                        new EntitiesNotExistsTransition(300, "Death", "Tomb Support", "Tomb Attacker", "Tomb Defender",
                            "Active Sarcophagus", "Tomb Defender Statue", "Tomb Support Statue", "Tomb Attacker Statue")
                    ),
                    new State("Death",
                        new Suicide()
                    )
                )
            );
    }
}