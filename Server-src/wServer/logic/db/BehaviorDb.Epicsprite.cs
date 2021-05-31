using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Epicspr = () => Behav()//by  ppmaks
            .Init("Epic Limon The Sprite God",
                new State(
                     new State("0",
                        new ScaleHP2(30, 3, 15),
                        new ChangeMusic("https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/Sprite.mp3"),
                         new DropPortalOnDeath(target: "Glowing Realm Portal", probability: 1),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(10, "1")
                    ),
                    new State("1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                         new Flash(color: 0x00FF00, flashPeriod: 0.25, flashRepeats: 8),
                        new Taunt("!"),
                        new TimedTransition(4000, "2")
                    ),
                    new State("2",
                        new Charge(7,14,coolDown:1000),
                        new Follow(5,10,duration:3000),
                        new Shoot(999,4,shootAngle:45,coolDown:1000),
                        new HpLessTransition(0.30, "3")
                          ),
                    new State("3",
                         new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 6500),
                         new Grenade(4, 15, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Slowed, effectDuration: 1000),
                         new Taunt("Minions help me!"),
                         new Spawn("Native Sprite God", 6, 0),
                          new Grenade(5, 120, coolDown: 2000),
                          new Shoot(999,5,45,predictive:0.7),
                           new Shoot(999, 5, 85,projectileIndex:2,coolDownOffset:1, predictive: 0.7),
                        new HpLessTransition(0.40, "4")
                         ),
                    new State("4",
                         new Grenade(4, 150, range: 8, coolDown: 1500, effect: ConditionEffectIndex.Weak, effectDuration: 1500),
                          new Charge(7, 14, coolDown: 5000),
                           new Shoot(10,6,45,0),
                          new Shoot(5, 2, 0, predictive: 0.1),
                        new HpLessTransition(0.60, "5")
                          ),
                    new State("5",
                        new Taunt("Minions help your master"),
                         new Spawn("Native Sprite God",4,0),
                         new Grenade(5, 90, coolDown: 2000),
                        new Shoot(5, 5, 25,projectileIndex:1, predictive: 5),
                         new Shoot(5, 4, 25, predictive: 2),
                        new TimedTransition(696969, "2")
                    )//                         new Grenade(5,90,coolDown:2000),
                ),
                new Threshold(0.00001,
                    new ItemLoot(item: "Dagger of the Endless Magic", probability: 0.0008, damagebased: true),
                    new ItemLoot(item: "Wand of Mythical Fusion", probability: 0.0008, damagebased: true),



                    new ItemLoot(item: "Sprite Wand", probability: 0.004, damagebased: true),
                    new ItemLoot(item: "Wine Cellar Incantation", probability: 0.05),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot(item: "Sprite Essence", probability: 0.0045, threshold: 0.01, damagebased: true),
                    new ItemLoot(item: "Spriteful Dirk", probability: 0.008, damagebased: true),
                    new ItemLoot(item: "Awoken Spriteful Dirk", probability: 0.004, damagebased: true, threshold: 0.01),
                    new ItemLoot(item: "Staff of Extreme Prejudice", probability: 0.015, damagebased: true),//Wand of Mythical Fusion
                    new ItemLoot(item: "Cloak of the Planewalker", probability: 0.015, damagebased: true),//Dagger of the Endless Magic
                    new ItemLoot("Greater potion of dexterity", 1),
                    new ItemLoot("Standard Chest", 0.7),
                    new ItemLoot("Weirdly Pulsating Armor", 0.007),
                    new ItemLoot("Epic quest chest", 0.3),
                    new ItemLoot("Potion of Dexterity",1),
                    new ItemLoot("Items Crate", 0.05)
                )
            );
    }
}