using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CrystalCove = () => Behav()
            .Init("Forbidden Crystal Prisoner",
                new State(
                    new ScaleHP2(60,3,15),
                    new DropPortalOnDeath("Glowing Realm Portal", 100),
                    new State("BStart",
                        new Taunt("BBreakk... Thee... Coreess...", "Pleasee... Freee.... Mee.."),
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Crystal Core", 100, "Start"),
                        new TimedTransition(5500, "BStart2")
                        ),
                      new State("BStart2",
                        new Taunt("FFreee..  Meee....", "Craackk.. Thhee.... Crysstaal.... Corresss..."),
                        new SetAltTexture(1),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntityNotExistsTransition("Crystal Core", 100, "Start"),
                        new TimedTransition(5500, "BStart")
                        ),
                    new State("Start",
                        new SetAltTexture(0),
                        new StayCloseToSpawn(.1, 3),
                        new Taunt("*Cracks Ice*"),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new PlayerWithinTransition(8, "Start2")
                        ),
                    new State("Start2",
                        new StayCloseToSpawn(.1, 0),
                        new Taunt("Hahaa... Come to think you're that stupid."),
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(5300, "Start3")
                        ),
                    new State("Start3",
                        new StayCloseToSpawn(.1, 0),
                        new Taunt("It's been 10 years sense i've been able to use this power!"),
                     new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(5000, "Start4")
                        ),
                    new State("Start4",
                        new Taunt("You will suffer from your mistake!"),
                        new Flash(0xFFFFFF, 2, 4),
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2500, "Attack")
                             ),
                    new State("Attack",
                        new Flash(0xFFFFFF, 2, 4),
                        new Shoot(10, count: 3, projectileIndex: 2, coolDown: 500),
                        new Shoot(8.4, count: 20, projectileIndex: 3, coolDown: 4000),
                        new HpLessTransition(0.50, "NAttack"),
                        new TimedTransition(5000, "Attack2")
                        ),
                    new State("Attack2",
                        new Flash(0xFFFFFF, 2, 4),
                        new Grenade(6, 150, 10),
                       new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 100, coolDownOffset: 1600, shootAngle: 90),
                             new Shoot(10, projectileIndex: 2, predictive: 0.5, coolDown: 1000),
                        new HpLessTransition(0.50, "NAttack"),
                        new TimedTransition(9000, "Attack3")
                        ),
                     new State("Attack3",
                        new Flash(0xFFFFFF, 2, 4),
                        new Shoot(18, 2, 7, projectileIndex: 1, coolDown: 500),
                        new Shoot(10, 1, 7, projectileIndex: 2, predictive: 0.2, coolDown: 250),
                        new Grenade(5, 60, 7),
                        new HpLessTransition(0.50, "NAttack"),
                        new TimedTransition(5000, "Attack")
                         ),
                               
                    new State("NAttack",
                        new Taunt("ENOUGH, this will be the death of you."),
                        new Flash(0xFF0000, 2, 4),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2700, "NAttack2")
                                 ),
                     new State("NAttack2",
                        new Flash(0xFF0000, 2, 4),
                        new Shoot(10, 3, fixedAngle: 0, coolDownOffset: 0, coolDown: 1000),
                        new Shoot(10, 3, fixedAngle: 24, coolDownOffset: 200, coolDown: 1000),
                        new Shoot(10, 3, fixedAngle: 48, coolDownOffset: 400, coolDown: 1000),
                        new Shoot(10, 3, fixedAngle: 72, coolDownOffset: 600, coolDown: 1000),
                        new Shoot(10, 3, fixedAngle: 96, coolDownOffset: 800, coolDown: 1000),
                        new Grenade(6, 190, 7),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new HpLessTransition(0.05, "Dead"),
                        new TimedTransition(5000, "NAttack3")
                         ),
                           
                     new State("NAttack3",
                        new Flash(0xFF0000, 2, 4),
                        new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 1400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 1600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 1800, shootAngle: 90),
                            new Shoot(1, 8, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 2000, shootAngle: 45),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 180, coolDownOffset: 0, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 170, coolDownOffset: 200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 160, coolDownOffset: 400, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 150, coolDownOffset: 600, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 140, coolDownOffset: 800, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 130, coolDownOffset: 1000, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 120, coolDownOffset: 1200, shootAngle: 90),
                            new Shoot(1, 4, projectileIndex: 3, coolDown: 4575, fixedAngle: 110, coolDownOffset: 1400, shootAngle: 90),
                        new Shoot(10, 5, projectileIndex: 3, coolDownOffset: 500, angleOffset: 22.5, coolDown: 1000),
                        new Shoot(8, 1, shootAngle: 20, projectileIndex: 2, coolDown: 2000),
                        new Shoot(10, 1, 7, 1, predictive: 0.5, coolDown: 600),
                        new Grenade(6, 90, 7),
                        new HpLessTransition(0.05, "Dead"),
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new TimedTransition(5000, "NAttack2")
                         ),
                        new State("Dead",
                        new Taunt("Even at my full power, I still wasn't enough...."),
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TimedTransition(2700, "Dead2")
                         ),
                        new State("Dead2",
                        new Shoot(8.4, count: 40, projectileIndex: 1, coolDown: 1000),
                        new Suicide(),
                        new TimedTransition(2000, "Dead2")
                         )
                    ),
                    new Threshold(.0001,
                        new TierLoot(4, ItemType.Ring, 0.2),
                        new TierLoot(7, ItemType.Armor, 0.2),
                        new TierLoot(8, ItemType.Weapon, 0.2),
                        new TierLoot(4, ItemType.Ability, 0.1),
                        new TierLoot(8, ItemType.Armor, 0.1),
                        new TierLoot(5, ItemType.Ring, 0.05),
                        new TierLoot(9, ItemType.Armor, 0.03),
                        new TierLoot(5, ItemType.Ability, 0.03),
                        new TierLoot(9, ItemType.Weapon, 0.03),
                        new TierLoot(10, ItemType.Armor, 0.02),
                        new TierLoot(10, ItemType.Weapon, 0.02),
                        new TierLoot(11, ItemType.Armor, 0.01),
                        new TierLoot(12, ItemType.Weapon, 0.01),
                        new TierLoot(6, ItemType.Ring, 0.01),
                        new ItemLoot("Greater Potion of Critical Damage", 0.80, 3),
                        new ItemLoot("50 Credits", 0.01),//Mark of the Crystal Entity
                        new ItemLoot("Mark of the Crystal Entity", 1),
                        new ItemLoot("Crystal Cave Key", 0.01),
                        new ItemLoot("Radiant Heart", 0.004, damagebased: true),
                        new ItemLoot("Luminous Armor", 0.004, damagebased: true),
                        new ItemLoot("Crystalline Kunai", 0.004, damagebased: true),
                        new ItemLoot("Quartz Cutter", 0.004, damagebased: true),
                        new ItemLoot("Magical Soul Hide", 0.006, damagebased: true),
                        new ItemLoot("Book of Crystal Protection", 0.004, damagebased: true)
                    )
            )
        .Init("Guardian of the Crystal",
                new State(
                    new State("Start",
                       new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                       new PlayerWithinTransition(8, "Attack")
                    ),
                    new State("Attack",
                       new SetAltTexture(1),
                       new Shoot(2, count: 2, projectileIndex: 1, coolDown: 300),
                       new Shoot(6, count: 6, projectileIndex: 0, coolDown: 2000),
                        new Follow(0.55, 8, 1),
                            new Wander(0.1),
                       new NoPlayerWithinTransition(8, "Start")
                )
            )
            )
        .Init("Crystal Core",
                new State(
                    new State("Noob",
                        new PlayerWithinTransition(4, "Default")
                        ),
                    new State("Default",
                        new ConditionalEffect(ConditionEffectIndex.ArmorBroken),
                        new Shoot(8.4, count: 10, projectileIndex: 0,  coolDown: 2500)
                        )
                )
            )
           .Init("Crystal Serpent",
                new State(
                    new State("Default",
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 100),
                        new Prioritize(
                            new Follow(0.55, 8, 1),
                            new Wander(0.1)
                            )
                        )
                )
            )
        .Init("Crystal Giant",
                new State(
                    new State("Default",
                        new ChangeSize(20, 130),
                        new Shoot(10, count: 3, projectileIndex: 0, coolDown: 550),
                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 280),
                        new Prioritize(
                            new Follow(0.25, 8, 1),
                            new Wander(0.1)
                            )
                        )
                )
            
          
            );
    }
}