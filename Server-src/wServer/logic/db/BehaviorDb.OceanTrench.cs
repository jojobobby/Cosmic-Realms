using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        _ OceanTrench = () => Behav()
            .Init("Coral Gift",  //credits to GhostMaree, ???
                new State(
                   new ScaleHP2(85, 2, 15),
                    new State("Texture1",
                        new SetAltTexture(1),
                        new TimedTransition(500, "Texture2")
                    ),
                    new State("Texture2",
                        new SetAltTexture(2),
                        new TimedTransition(500, "Texture0")
                    ),
                    new State("Texture0",
                        new SetAltTexture(0),
                        new TimedTransition(500, "Texture1")
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Coral Juice", 0.4),
                    new ItemLoot("Coral Juice", 0.3),
                    new ItemLoot("Coral Juice", 0.2),
                    new ItemLoot("Coral Juice", 0.1),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Potion of Mana", 0.3),
                    new ItemLoot("Coral Bow", 0.006, damagebased: true),
                    new ItemLoot("Coral Venom Trap", 0.006, damagebased: true),
                    new ItemLoot("Coral Silk Armor", 0.002, damagebased: true),
                    new ItemLoot("Coral Ring", 0.04, damagebased: true),
                    new ItemLoot("Pineapple Ring", 0.009, damagebased: true),
                     new ItemLoot("Pineapple Armor", 0.009, damagebased: true)//Pineapple Ring
                    )
            )
            .Init("Coral Bomb Big",
                new State(
                    new State("Spawning",
                        new TossObject("Coral Bomb Small", 1, angle: 30, coolDown: 500, throwEffect: true),
                        new TossObject("Coral Bomb Small", 1, angle: 90, coolDown: 500, throwEffect: true),
                        new TossObject("Coral Bomb Small", 1, angle: 150, coolDown: 500, throwEffect: true),
                        new TossObject("Coral Bomb Small", 1, angle: 210, coolDown: 500, throwEffect: true),
                        new TossObject("Coral Bomb Small", 1, angle: 270, coolDown: 500, throwEffect: true),
                        new TossObject("Coral Bomb Small", 1, angle: 330, coolDown: 500, throwEffect: true),
                        new TimedTransition(500, "Attack")
                    ),
                    new State("Attack",
                        new Shoot(4.4, count: 5, fixedAngle: 0, shootAngle: 70),
                        new Suicide()
                    )
                )
            )
            .Init("Coral Bomb Small",
                new State(
                    new Shoot(3.8, count: 5, fixedAngle: 0, shootAngle: 70),
                    new Suicide()
                )
             )
            .Init("Deep Sea Beast",
                new State(
                    new ChangeSize(11, 100),
                    new Prioritize(
                        new StayCloseToSpawn(0.2, 2),
                        new Follow(0.2, acquireRange: 4, range: 1)
                    ),
                    new Shoot(1.8, count: 1, projectileIndex: 0, coolDown: 1000),
                    new Shoot(2.5, count: 1, projectileIndex: 1, coolDown: 1000),
                    new Shoot(3.3, count: 1, projectileIndex: 2, coolDown: 1000),
                    new Shoot(4.2, count: 1, projectileIndex: 3, coolDown: 1000)
                 )
            )
            .Init("Thessal the Mermaid Goddess",
                new State(
                    new ScaleHP2(65, 2, 15),
                    new DropPortalOnDeath(target: "Deep Sea Portal"),
                    new TransformOnDeath("Thessal the Mermaid Goddess Wounded", probability: 0.1),
                    new TransformOnDeath("Thessal Dropper"),
                    new StayCloseToSpawn(1, 2),
                    new State("Start",
                        new Prioritize(
                            new Wander(0.3),
                            new Follow(0.3, acquireRange: 10, range: 2)
                        ),
                        new HpLessTransition(0.999, "Attack1")
                    ),
                    new State("Main",
                        new Prioritize(
                            new Wander(0.3),
                            new Follow(0.3, acquireRange: 10, range: 2)
                         ),
                        new TimedTransition(0, "Attack1")
                    ),
                    new State("Spawning Bomb",
                        new TossObject("Coral Bomb Big", angle: 45, throwEffect: true),
                        new TossObject("Coral Bomb Big", angle: 135, throwEffect: true),
                        new TossObject("Coral Bomb Big", angle: 225, throwEffect: true),
                        new TossObject("Coral Bomb Big", angle: 315, throwEffect: true),
                        new TimedTransition(1000, "Main")
                   ),
                   new State("Spawning Bomb Attack2",
                       new TossObject("Coral Bomb Big", angle: 45, throwEffect: true),
                       new TossObject("Coral Bomb Big", angle: 135, throwEffect: true),
                       new TossObject("Coral Bomb Big", angle: 225, throwEffect: true),
                       new TossObject("Coral Bomb Big", angle: 315, throwEffect: true),
                       new TimedTransition(1000, "Attack2")
                   ),
                   new State("Attack1",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                       new HpLessTransition(0.5, "Attack2"),
                       new Wander(0.3),
                       new TimedRandomTransition(2000, false, "Yellow Wall", "Super Trident", "Thunder Swirl", "Spawning Bomb")
                   ),
                   new State("Thunder Swirl",
                       new Shoot(8.8, count: 8, shootAngle: 45, projectileIndex: 0),
                       new TimedTransition(500, "Thunder Swirl 2")
                   ),
                   new State("Thunder Swirl 2",
                       new Shoot(8.8, count: 8, shootAngle: 45, angleOffset: 22.5, projectileIndex: 0),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Thunder Swirl 3")
                   ),
                   new State("Thunder Swirl 3",
                       new Shoot(8.8, count: 8, shootAngle: 45, projectileIndex: 0),
                       new TimedTransition(100, "Main")
                   ),
                   new State("Thunder Swirl Attack2",
                       new Shoot(8.8, count: 16, shootAngle: 45, angleOffset: 22.5, projectileIndex: 0),
                       new TimedTransition(500, "Thunder Swirl 2 Attack2")
                   ),
                   new State("Thunder Swirl 2 Attack2",
                       new Shoot(8.8, count: 16, shootAngle: 45, projectileIndex: 0),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Thunder Swirl 3 Attack2")
                   ),
                   new State("Thunder Swirl 3 Attack2",
                       new Shoot(8.8, count: 16, shootAngle: 45, angleOffset: 22.5, projectileIndex: 0),
                       new TimedTransition(100, "Attack2")
                   ),
                   new State("Super Trident",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Super Trident 2")
                   ),
                   new State("Super Trident 2",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Main")
                   ),
                   new State("Super Trident Attack2",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Super Trident 2 Attack2")
                   ),
                   new State("Super Trident 2 Attack2",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                       new TimedTransition(500, "Super Trident 3 Attack2")
                   ),
                   new State("Super Trident 3 Attack2",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 0),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 90),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 180),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 270),
                       new TossObject("Coral Bomb Big", throwEffect: true),
                       new TimedTransition(500, "Super Trident 4 Attack2")
                   ),
                   new State("Super Trident 4 Attack2",
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 45),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 135),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 225),
                       new Shoot(21, count: 2, shootAngle: 25, projectileIndex: 2, angleOffset: 315),
                       new TimedTransition(500, "Attack2")
                   ),
                   new State("Yellow Wall",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Yellow Wall 2")
                   ),
                   new State("Yellow Wall 2",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Yellow Wall 3")
                   ),
                   new State("Yellow Wall 3",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Main")
                   ),
                   new State("Yellow Wall Attack2",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Yellow Wall 2 Attack2")
                   ),
                   new State("Yellow Wall 2 Attack2",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Yellow Wall 3 Attack2")
                   ),
                   new State("Yellow Wall 3 Attack2",
                       new Flash(0xFFFF00, .1, 15),
                       new Shoot(18, count: 30, fixedAngle: 6, projectileIndex: 3, coolDown: 20000),
                       new TimedTransition(1000, "Attack2")
                   ),
                   new State("Attack2",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                       new Wander(0.3),
                       new TimedRandomTransition(2000, false, "Yellow Wall Attack2", "Super Trident Attack2", "Thunder Swirl Attack2", "Spawning Bomb Attack2")
                   )
                ),
                new Threshold(0.0001,
                    new ItemLoot("Potion of Mana", 0.3, 3),
                    new ItemLoot("Coral Juice", 0.4),
                    new ItemLoot("Coral Juice", 0.3),
                    new ItemLoot("Coral Juice", 0.2),
                    new ItemLoot("Coral Juice", 0.1),
                    new ItemLoot("Coral Bow", 0.004, damagebased: true),
                    new ItemLoot("Coral Venom Trap", 0.006, damagebased: true),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Coral Silk Armor", 0.004, damagebased: true),
                    new ItemLoot("Coral Ring", 0.05, damagebased: true),
                    new ItemLoot("Sea-Enchanted Sword", 0.004, damagebased: true),
                    new ItemLoot("50 Credits", 0.01),//Sea-Enchanted Sword
                    new ItemLoot("Mark of Thessal", 1),
                    new ItemLoot("Royal Queen Robe", 0.004, damagebased: true),
                     new ItemLoot("Pineapple Ring", 0.004, damagebased: true),
                     new ItemLoot("Pineapple Armor", 0.006, damagebased: true)
                )
            )
            .Init("Thessal Dropper",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new TransformOnDeath("Ocean Vent"),
                    new State("Idle",
                        new EntityNotExistsTransition("Thessal the Mermaid Goddess", 100, "Suicide")
                    ),
                    new State("Suicide",
                        new Suicide()
                    )
                 )
            )
            .Init("Thessal the Mermaid Goddess Wounded",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("Is King Alexander alive?"),
                    new State("Idle",
                        new TimedTransition(24000, "Fail"),
                        new SetAltTexture(0, 1, 500, true),
                        new PlayerTextTransition("Prize", "He lives and reigns and conquers the world", 24)
                    ),
                    new State("Prize",
                        new Taunt("Thank you kind sailor."),
                        new TossObject("Coral Gift", range: 5, angle: 45, throwEffect: true, coolDown: 10000),
                        new TossObject("Coral Gift", range: 5, angle: 135, throwEffect: true, coolDown: 10000),
                        new TossObject("Coral Gift", range: 5, angle: 235, throwEffect: true, coolDown: 10000),
                        new Decay(2000)
                    ),
                    new State("Fail",
                        new Taunt("You speak LIES!!"),
                        new Decay(2000)
                    )
                )
            );
    }
}