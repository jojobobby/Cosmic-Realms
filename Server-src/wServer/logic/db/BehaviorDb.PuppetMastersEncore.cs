using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PuppetMastersEncore = () => Behav()
            .Init("Puppet Master v2",
                new State(
                   new ScaleHP2(40,3,15),
                   new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Puppet.mp3"),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new MoveLine(1, 135, 0.5),
                        new PlayerWithinTransition(dist: 4, targetState: "2")
                    ),
                    new State("2",
                        new Taunt("Hello, Hero. Did you truly think that I was gone forever?"),
                        new TimedTransition(time: 3000, targetState: "3")
                    ),
                    new State("3",
                        new Taunt("No! I cannot allow you to end my story in such a way."),
                        new TimedTransition(time: 3000, targetState: "4")
                    ),
                    new State("4",
                        new Taunt("You fail to appreciate my puppets, hero… You do not understand my art. If you cannot be sensible of their beauty then you must die."),
                        new TimedTransition(time: 3000, targetState: "5")
                    ),
                    new State("5",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("You cannot return from whence you came. Face me now, hero and prepare to become the finest of my puppets when I am done with you!"),
                        new TimedTransition(time: 3000, targetState: "6")
                    ),
                    new State("6",
                        new Shoot(0, 12, 30, 1, 0, 15, coolDown: 750),
                        new State("7",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "8")
                        ),
                        new State("8",
                            new EntityNotExistsTransition("HomingFire", 20, "7")
                        ),
                        new HpLessTransition(0.8, "9")
                    ),
                    new State("9",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Awaken, my dear puppet. Hunt them down!"),
                        new OrderOnce(15, "Encore Huntress Statue", "Decay"),
                        new Shoot(0, 6, 60, 3, 0, -15, coolDown: 400),
                        new State("10",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "11")
                        ),
                        new State("11",
                            new EntityNotExistsTransition("HomingFire", 20, "10")
                        )
                    ),
                    new State("12",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                        new Taunt("How dare you destroy my prized puppet! You shall pay for this with your life!"),
                        new Shoot(0, 12, 30, 1, 0, 15, coolDown: 750),
                        new Shoot(12, 3, 10, 0, coolDown: 1000),
                        new State("13",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "14")
                        ),
                        new State("14",
                            new EntityNotExistsTransition("HomingFire", 20, "13")
                        ),
                        new HpLessTransition(0.6, "15")
                    ),
                    new State("15",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Awaken, my dear puppet. Fear its blades!"),
                        new OrderOnce(15, "Encore Trickster Statue", "Decay"),
                        new Shoot(0, 6, 60, 3, 0, -15, coolDown: 400),
                        new State("16",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "17")
                        ),
                        new State("17",
                            new EntityNotExistsTransition("HomingFire", 20, "16")
                        )
                    ),
                    new State("18",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("No! You will suffer for destroying my precious puppets!"),
                        new Shoot(0, 7, 10, 0, 90, coolDown: 3000, coolDownOffset: 500),
                        new Shoot(0, 15, 10, 0, 90, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(0, 23, 10, 0, 90, coolDown: 3000, coolDownOffset: 1500),
                        new Shoot(0, 7, 10, 0, 270, coolDown: 3000, coolDownOffset: 2000),
                        new Shoot(0, 15, 10, 0, 270, coolDown: 3000, coolDownOffset: 2500),
                        new Shoot(0, 23, 10, 0, 270, coolDown: 3000, coolDownOffset: 3000),
                        new State("19",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "20")
                        ),
                        new State("20",
                            new EntityNotExistsTransition("HomingFire", 20, "19")
                        ),
                        new HpLessTransition(0.4, "21")
                    ),
                    new State("21",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("Awaken, my puppets, and avenge your fallen brethren. Destroy these so-called heroes, leave them to rot here and become one with the dust."),
                        new OrderOnce(15, "Confuse Puppet Statue", "Decay"),
                        new OrderOnce(15, "Bleed Puppet Statue", "Decay"),
                        new Shoot(0, 6, 60, 3, 0, -15, coolDown: 400),
                        new State("22",
                            new Spawn("HomingFire", 1, 0),
                            new EntityExistsTransition("HomingFire", 20, "23")
                        ),
                        new State("23",
                            new EntityNotExistsTransition("HomingFire", 20, "22")
                        ),
                        new State("24", //Confuse Puppet
                            new EntityNotExistsTransition("Bleed Puppet", 20, "26"),
                            new EntityExistsTransition("Bleed Puppet", 20, "22")
                        ),
                        new State("25", //Bleed Puppet
                            new EntityNotExistsTransition("Confuse Puppet", 20, "26"),
                            new EntityExistsTransition("Confuse Puppet", 20, "22")
                        )
                    ),
                    new State("26",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                        new Taunt("I will rend your souls from your bodies and turn you into my puppet slaves when this is over!"),
                        new Shoot(0, 12, 30, 1, 0, 15, coolDown: 750),
                        new Shoot(12, coolDown: 500),
                        new Grenade(4, 180, 14, coolDown: 4000),
                        new Shoot(400, 11, 5, 2, 0, 90, coolDown: 400),
                        new HpLessTransition(0.1, "27")
                    ),
                    new State("27",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Taunt("I don’t understand, it wasn’t supposed to end this way! Noooo!!!"),
                        new Shoot(0, 16, 22, 0, 0, coolDownOffset: 4000),
                        new Decay(4000)
                    )
                ),
                new Threshold(0.0001,
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(12, ItemType.Weapon, 0.125),
                    new TierLoot(10, ItemType.Weapon, 0.0625),
                    new TierLoot(11, ItemType.Weapon, 0.0625),
                    new TierLoot(5, ItemType.Ring, 0.0625),
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Attack", 0.3, 3),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("Potion of Defense", 0.3, 3),
                    new ItemLoot("Potion of Mana", 0.3),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Critical Chance", 0.02),
                    new ItemLoot("Potion of Critical Damage", 0.02),
                    new ItemLoot("Prism of Dire Instability", 0.006, damagebased: true),
                    new ItemLoot("Fate's Twisted Card Deck", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Magician's Scepter", 0.006, damagebased: true),
                    new ItemLoot("Hat of the Magician", 0.006, damagebased: true),
                    new ItemLoot("Thousand Shot", 0.004, damagebased: true)
                )
            )
            .Init("HomingFire",
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
                        new Decay(0)
                    )
                )
            )
            .Init("Encore Huntress Statue",
                new State(
                    new TransformOnDeath("Encore Huntress"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Idle"),
                    new State("Decay",
                        new Decay(1000)
                    )
                 )
            )
            .Init("Encore Huntress",
                new State(
                    new OrderOnDeath(30, "Puppet Master v2", "12"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveLine(0.4, 315, 1),
                        new TimedTransition(2000, "2")
                    ),
                    new State("2",
                        new Orbit(0.5, 3, 7),
                        new Shoot(14, 2, 10, 0, coolDown: 400),
                        new TossObject("Huntress Trap", 8, coolDown: 4000, throwEffect: true),
                        new HpLessTransition(0.3, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF000000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Follow(0.1, range: 1),
                        new Shoot(14, 2, 10, 0, coolDown: 400),
                        new Shoot(14, 3, 45, 1, coolDown: 1000),
                        new TossObject("Huntress Trap", 8, coolDown: 4000, throwEffect: true)
                    )
               )
            )
            .Init("Huntress Trap",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(2, "2")
                    ),
                    new State("2",
                        new Shoot(0, 8, 45, 0, 0),
                        new Decay(0)
                    )
               )
            )
            .Init("Encore Trickster Statue",
                new State(
                    new TransformOnDeath("Encore Trickster"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Idle"),
                    new State("Decay",
                        new Decay(1000)
                    )
                 )
            )
            .Init("Encore Trickster",
                new State(
                    new OrderOnDeath(30, "Puppet Master v2", "18"),
                    new OrderOnDeath(30, "Puppet Clone", "Decay"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveLine(0.4, 225, 1),
                        new TimedTransition(2000, "2")
                    ),
                    new State("2",
                        new Prioritize(
                            new StayBack(1, 7),
                            new Follow(1, 10, 1)
                        ),
                        new Spawn("Puppet Clone", 3, 0),
                        new Shoot(14, 1, coolDown: 500),
                        new Shoot(14, 4, 90, 1, coolDown: 2000),
                        new Grenade(3, 180, 14, coolDown: 4000),
                        new HpLessTransition(0.3, "3")
                    ),
                    new State("3",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFF000000, 1, 1),
                        new TimedTransition(2000, "4")
                    ),
                    new State("4",
                        new Prioritize(
                            new Follow(0.3, 20, 6),
                            new Wander(0.2)
                        ),
                        new Spawn("Puppet Clone", 3, 0),
                        new State("5",
                            new Shoot(0, 3, 10, 1, 0, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 90, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 180, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 270, coolDown: 500),
                            new TimedTransition(1500, "6")
                        ),
                        new State("6",
                            new Shoot(0, 3, 10, 1, 45, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 135, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 225, coolDown: 500),
                            new Shoot(0, 3, 10, 1, 315, coolDown: 500),
                            new TimedTransition(1500, "5")
                        ),
                        new Grenade(3, 180, 14, coolDown: 4000)
                    )
               )
            )
            .Init("Puppet Clone",
               new State(
                   new State("Idle",
                       new Follow(0.1, 10, 1),
                       new Shoot(14, 3, 45, coolDown: 1500)
                   ),
                   new State("Decay",
                       new Decay(0)
                   )
               )
            )
            .Init("Confuse Puppet Statue",
                new State(
                    new TransformOnDeath("Confuse Puppet"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Idle"),
                    new State("Decay",
                        new Decay(1000)
                    )
                 )
            )
            .Init("Confuse Puppet",
                new State(
                    new OrderOnDeath(30, "Puppet Master v2", "24"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveLine(0.4, 125, 1),
                        new TimedTransition(2000, "2")
                    ),
                    new State("2",
                        new Prioritize(
                            new Orbit(0.1, 6, 20, "Puppet Master v2"),
                            new Wander(0.3)
                        ),
                        new Shoot(14, coolDown: 500),
                        new Shoot(14, 3, 90, 2, coolDown: 1000),
                        new HpLessTransition(0.7, "3")
                    ),
                    new State("3",
                        new Prioritize(
                            new Orbit(0.3, 8, 20, "Puppet Master v2", orbitClockwise: true),
                            new Wander(0.3)
                        ),
                        new Shoot(14, coolDown: 500),
                        new Shoot(14, 3, 90, 2, coolDown: 1000),
                        new HpLessTransition(0.4, "4")
                    ),
                    new State("4",
                        new Prioritize(
                            new Orbit(0.5, 9, 20, "Puppet Master v2"),
                            new Wander(0.3)
                        ),
                        new Shoot(14, 2, 80, 1, angleOffset: 90, coolDown: 500),
                        new Shoot(14, 2, 80, 1, angleOffset: -90, coolDown: 500),
                        new Shoot(14, 3, 90, 2, coolDown: 1000)
                    )
               )
            )
            .Init("Bleed Puppet Statue",
                new State(
                    new TransformOnDeath("Bleed Puppet"),
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new State("Idle"),
                    new State("Decay",
                        new Decay(1000)
                    )
                 )
            )
            .Init("Bleed Puppet",
                new State(
                    new OrderOnDeath(30, "Puppet Master v2", "25"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new MoveLine(0.4, 45, 1),
                        new TimedTransition(2000, "2")
                    ),
                    new State("2",
                        new Prioritize(
                            new Orbit(0.1, 5, 20, "Puppet Master v2", orbitClockwise: true),
                            new Wander(0.3)
                        ),
                        new Shoot(0, 4, 90, 0, 0, 15, coolDown: 1000),
                        new HpLessTransition(0.7, "3")
                    ),
                    new State("3",
                        new Prioritize(
                            new Orbit(0.3, 4, 20, "Puppet Master v2"),
                            new Wander(0.3)
                        ),
                        new Shoot(0, 4, 90, 0, 0, -15, coolDown: 900),
                        new HpLessTransition(0.4, "4")
                    ),
                    new State("4",
                        new Prioritize(
                            new Orbit(0.5, 3, 20, "Puppet Master v2", orbitClockwise: true),
                            new Wander(0.3)
                        ),
                        new Shoot(0, 4, 90, 0, 0, 15, coolDown: 800)
                    )
               )
            )
            ;
    }
}