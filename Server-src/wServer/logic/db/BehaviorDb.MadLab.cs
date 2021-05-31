using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ MadLab = () => Behav()
        .Init("Dr Terrible",
            new State(
               new ScaleHP2(40,1,15),
                new HpLessTransition(.2, "rage"),
                new State("idle",
                    new PlayerWithinTransition(12, "GP", true)
                    ),
                new State("rage",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 1500, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(12000, "rage TA")
                    ),
                new State("rage TA",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Turret Attack", coolDown: 1500, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(10000, "rage")
                    ),
                new State("GP",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new Taunt(0.5, "For Science"),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(12000, "TA")
                    ),
                new State("TA",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Turret Attack", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(10000, "hide")
                    ),
                new State("hide",
                    new OrderOnce(100, "Monster Cage", "spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "nohide")
                    ),
                new State("nohide",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(12000, "TA2")
                    ),
                new State("TA2",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(10000, "hide2")
                    ),
                new State("hide2",
                    new OrderOnce(100, "Monster Cage", "spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "nohide2")
                    ),
                new State("nohide2",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(12000, "TA3")
                    ),
                new State("TA3",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "ON"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(10000, "hide3")
                    ),
                new State("hide3",
                    new OrderOnce(100, "Monster Cage", "spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "nohide3")
                    ),
                new State("nohide3",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(12000, "TA4")
                    ),
                new State("TA4",
                    new OrderOnce(100, "Monster Cage", "no spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "nothing change"),
                    new OrderOnce(100, "Red Gas Spawner UL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner UR", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LL", "ON"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new Wander(0.5),
                    new SetAltTexture(0),
                    new TossObject("Green Potion", coolDown: 2000, coolDownOffset: 0, throwEffect: true),
                    new TimedTransition(10000, "hide4")
                    ),
                new State("hide4",
                    new OrderOnce(100, "Monster Cage", "spawn"),
                    new OrderOnce(100, "Dr Terrible Bubble", "Bubble time"),
                    new OrderOnce(100, "Red Gas Spawner UL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner UR", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LL", "OFF"),
                    new OrderOnce(100, "Red Gas Spawner LR", "OFF"),
                    new ReturnToSpawn(speed: 1),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new SetAltTexture(1),
                    new TimedTransition(15000, "idle")
                    )
                ),
                new Threshold(0.0001,
                    new ItemLoot("Conducting Wand", 0.004, damagebased: true),
                    new ItemLoot("Scepter of Fulmination", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Robe of the Mad Scientist", 0.006, damagebased: true),
                    new ItemLoot("Experimental Ring", 0.006, damagebased: true),//Power Battery
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Mark of Dr Terrible", 1),
                    new ItemLoot("Power Battery", 0.0045, damagebased: true, threshold: 0.01),
                    new ItemLoot("Potion of Wisdom", 0.3, 3),
                    new ItemLoot("Potion of Wisdom", 0.3, 3),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(5, ItemType.Ability, 0.0625)
                    )
            )
        .Init("Dr Terrible Mini Bot",
            new State(
                 new Wander(0.5),
                 new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 1000)
                 )
            )
        .Init("Dr Terrible Rampage Cyborg",
            new State(
                new State("idle",
                    new PlayerWithinTransition(10, "Hp_Check"),
                new State("Hp_Check",
                    new HpLessTransition(.2, "blink"),
                    new State("normal",
                        new Wander(0.5),
                        new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                        new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                        coolDown: 800, coolDownOffset: 0),
                        new TimedTransition(10000, "rage blink")
                        ),
                    new State("rage blink",
                        new Wander(0.5),
                        new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                        new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                        new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                        coolDown: 800, coolDownOffset: 0),
                        new TimedTransition(3000, "rage")
                        ),
                    new State("rage",
                         new Wander(0.5),
                        new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                        new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                        new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                        coolDown: 800, coolDownOffset: 0)
                        )
                    ),
                new State("blink",
                    new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(10, count: 8, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                    new Suicide()
                )
                    )
            )
            )
        .Init("Dr Terrible Escaped Experiment",
              new State(
                  new Wander(0.5),
                  new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                  coolDown: 800, coolDownOffset: 0)
                  )
               )
            .Init("Mini Bot",
            new State(
                 new Wander(0.5),
                 new Shoot(10, 2, 20, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 1000)
                 )
            )
         .Init("Rampage Cyborg",
            new State(
                new State("idle",
                    new PlayerWithinTransition(10, "normal"),
                new State("normal",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(10000, "rage blink")
                    ),
                new State("rage blink",
                    new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink"),
                    new TimedTransition(3000, "rage")
                    ),
                new State("rage",
                    new Wander(0.5),
                    new Flash(0xf0e68c, flashRepeats: 5, flashPeriod: 0.1),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 1, predictive: 1,
                    coolDown: 800, coolDownOffset: 0),
                    new HpLessTransition(.2, "blink")
                    ),
                new State("blink",
                    new Wander(0.5),
                    new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                    new Flash(0xfFF0000, flashRepeats: 10000, flashPeriod: 0.1),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                    new Flash(0xfFF0000, 1, 9000001),
                    new Shoot(10, count: 8, projectileIndex: 2, fixedAngle: fixedAngle_RingAttack2),
                    new Suicide()
                )
                    )
            )
                )
        .Init("Escaped Experiment",
              new State(
                  new Wander(0.5),
                  new Shoot(10, 1, 0, defaultAngle: 0, angleOffset: 0, projectileIndex: 0, predictive: 1,
                  coolDown: 800, coolDownOffset: 0)
                  )
            )
        .Init("West Automated Defense Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(32, fixedAngle: 0, coolDown: new Cooldown(3000, 1000))
                    )
            )
        .Init("East Automated Defense Turret",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Shoot(32, fixedAngle: 180, coolDown: new Cooldown(3000, 1000))
                    )
            )
        .Init("Crusher Abomination",
            new State(
                new State("1 step",
                    new Wander(0.5),
                    new Shoot(10, 3, 20, angleOffset: 0 / 3, projectileIndex: 0, coolDown: 1000),
                    new HpLessTransition(.75, "2 step")
                    ),
                new State("2 step",
                    new Wander(0.5),
                    new ChangeSize(11, 150),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 1, coolDown: 1000),
                    new HpLessTransition(.5, "3 step")
                    ),
                new State("3 step",
                     new Wander(0.5),
                    new ChangeSize(11, 175),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 2, coolDown: 1000),
                    new HpLessTransition(.25, "4 step")
                    ),
                new State("4 step",
                    new Wander(0.5),
                    new ChangeSize(11, 200),
                    new Shoot(10, 2, 20, angleOffset: 0 / 3, projectileIndex: 3, coolDown: 1000)
                    )
                )
            )
        .Init("Enforcer Bot 3000",
            new State(
                new Wander(0.5),
                new Shoot(10, 3, 20, angleOffset: 0 / 3, projectileIndex: 0, coolDown: 1000),
                new Shoot(10, 4, 20, angleOffset: 0 / 4, projectileIndex: 1, coolDown: 1000),
                new TransformOnDeath("Mini Bot", 0, 3)

                )
            )
        .Init("Green Potion",
            new State(
                new State("Idle",
                    new Flash(0x001DFF, .1, 15),
                    new TimedTransition(2000, "explode")
                    ),
                new State("explode",
                      new Shoot(10, count: 6, projectileIndex: 0, fixedAngle: fixedAngle_RingAttack2),
                      new Suicide()
                    )
                )
            )
        .Init("Red Gas Spawner UL",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF"),
                new State("ON",
                    new Shoot(10, count: 18, projectileIndex: 0, fixedAngle: 0, angleOffset: 0)
                )
                )
        )
        .Init("Red Gas Spawner UR",
           new State(
               new ConditionalEffect(ConditionEffectIndex.Invincible),
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF"),
                new State("ON",
                    new Shoot(10, count: 18, projectileIndex: 0, fixedAngle: 0, angleOffset: 0)
                )
                )
        )
        .Init("Red Gas Spawner LL",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF"),
                new State("ON",
                    new Shoot(10, count: 18, projectileIndex: 0, fixedAngle: 0, angleOffset: 0)
                )
                )
        )
        .Init("Red Gas Spawner LR",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new EntityNotExistsTransition("Dr Terrible", 50, "OFF"),
                new State("OFF"),
                new State("ON",
                    new Shoot(10, count: 18, projectileIndex: 0, fixedAngle: 0, angleOffset: 0)
                )
                )
        )
        .Init("Turret Attack",
            new State(
                new Shoot(10, 2, 35, angleOffset: 0 / 2, projectileIndex: 0, coolDown: 2000)
            )
        )
        .Init("Mad Scientist Summoner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new DropPortalOnDeath("Glowing Realm Portal"),
                new State("idle",
                     new EntitiesNotExistsTransition(300, "Death", "Dr Terrible")
                    ),
                new State("Death",
                    new Suicide()
                    )
                )
            )
        .Init("Dr Terrible Bubble",
            new State(
                new State("nothing change",
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    //new SetAltTexture(0)
                    ),
                new State("Bubble time",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    //new SetAltTexture(1),
                    new TimedTransition(1000, "Bubble time2")
                    ),
                new State("Bubble time2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    //new SetAltTexture(2),
                    new TimedTransition(1000, "Bubble time")
                )
            )
            )
        .Init("Mad Gas Controller", //don't need xD
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true)
               )
           )
        .Init("Monster Cage",
            new State(
                new State("no spawn"),
                new State("spawn",
                    // new SetAltTexture(2),
                    new Spawn("Dr Terrible Rampage Cyborg", maxChildren: 1, initialSpawn: 0),
                    new Spawn("Dr Terrible Mini Bot", maxChildren: 1, initialSpawn: 0),
                    new Spawn("Dr Terrible Escaped Experiment", maxChildren: 1, initialSpawn: 0)
                    )
                )
            )
        .Init("Mad Lab Open Wall",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible),
                new State("1",
                    new EntityNotExistsTransition("Dr Terrible", 99, "2")
                ),
                new State("2",
                    new RemoveTileObject(0x18c1, 5)
                )
            )
        )
        .Init("Horrific Creation Summoner",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("1",
                    new EntityNotExistsTransition("Dr Terrible", 99, "2")
                    ),
                new State("2",
                    new Transform("Horrific Creation")
                    )
               )
           )
        .Init("Horrific Creation",
            new State(
                new ScaleHP2(40,3,15),
                new StayCloseToSpawn(0.5, 9),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt(true, "WHO KILL MASTER? RAHHHH!"),
                    new EntitiesNotExistsTransition(9999, "2", "Tesla Coil")
                    ),
                new State("2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt(true, "My door is open. Come let me crush you!"),
                    new TimedTransition(0, "3")
                    ),
                new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityExistsTransition("Hexxer", 1, "10"),
                    new State("4",
                        new SetAltTexture(0),
                        new Wander(0.1),
                        new Follow(0.4, 10, 6),
                        new Shoot(10, 3, 30, 0, coolDown: 1000),
                        new TimedTransition(6000, "5")
                        ),
                    new State("5",
                        new Flash(0xFFFFFF, .1, 32),
                        new Taunt(true, "What, you scared???"),
                        new TimedTransition(4000, "6")
                        ),
                    new State("6",//16 shoots
                        new SetAltTexture(1),
                        new Shoot(12, 1, projectileIndex: 5, coolDown: 250),
                        new TimedTransition(4000, "7")
                        ),
                    new State("7",
                        new SetAltTexture(0),
                        new Taunt(true, "Me can do this all day!"),
                        new Charge(2, 14),
                        new Shoot(7, 8, 45, 4, angleOffset: 45, coolDown: 750),
                        new TimedTransition(3000, "8")
                        ),
                    new State("8",
                        new Taunt(true, "Attacks no hurt me!!!!"),
                        new Flash(0xFFFFFF, .1, 16),
                        new TimedTransition(1000, "9")
                        ),
                    new State("9",
                        new Shoot(999, 8, 45, 3, 0, 0, coolDown: 999999),
                        new TimedTransition(2500, "3")
                        )
                    ),
                new State("10",
                    new TimedRandomTransition(100, false, "a12", "a13", "a14", "a15", "a16", "a17")
                    ),
                new State("a12",
                    new SetAltTexture(2),
                    new BackAndForth(0.8, 1),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("a13",
                    new SetAltTexture(3),
                    new BackAndForth(0.8, 2),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("a14",
                    new SetAltTexture(4),
                    new BackAndForth(0.8, 2),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("a15",
                    new SetAltTexture(5),
                    new BackAndForth(0.8, 2),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("a16",
                    new SetAltTexture(6),
                    new BackAndForth(0.8, 2),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("a17",
                    new SetAltTexture(7),
                    new BackAndForth(0.8, 2),
                    new TimedTransition(5000, "BackToBlue")
                    ),
                new State("BackToBlue",
                    new MoveLine(2, 0, 14),
                    new EntityExistsTransition("DeHexxer", 2, "3"),
                    new TimedTransition(3000, "3")
                    )
               ),
                new Threshold(0.01,
                    new ItemLoot("Conducting Wand", 0.008, damagebased: true, threshold: 0.01),
                    new ItemLoot("Experimental Ring", 0.04, damagebased: true, threshold: 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Wisdom", .30),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Grotesque Scepter", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("Garment of the Beast", 0.006, damagebased: true, threshold: 0.01),
                    new ItemLoot("Power Battery", 0.008, damagebased: true, threshold: 0.01),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.25),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(11, ItemType.Armor, 0.125),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new TierLoot(5, ItemType.Ring, 0.0625)
                    )
           )
            .Init("Hexxer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(effects: new[] {ConditionEffectIndex.Stunned, ConditionEffectIndex.Speedy, ConditionEffectIndex.Hexed}, 2, -1)
                    )
            )
            .Init("DeHexxer",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new ConditionEffectRegion(effects: new[] {ConditionEffectIndex.Stunned, ConditionEffectIndex.Speedy, ConditionEffectIndex.Hexed}, 2, 0)
                    )
            )
        ;

    }
}