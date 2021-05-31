using common.resources;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Omen = () => Behav()
         .Init("The Haunted Omen",//new TransformOnDeath("OM Loot Chest", 1, 1, 1),new ScaleHP(500),
            new State(
                new ScaleHP2(40,3,15),
                       new State("default",
                           new ConditionalEffect(ConditionEffectIndex.Invincible),
                           new PlayerWithinTransition(8, "setthethingies")
                           ),
                         new State("setthethingies",
                              new ConditionalEffect(ConditionEffectIndex.Invincible),
                              new Flash(0xFF00FF, 2, 2),
                           new TimedTransition(1000, "PurpleShotgunPhase")
                           ),
                       new State("PurpleShotgunPhase",
                           new Shoot(13, count: 6, shootAngle: 8, projectileIndex: 0, coolDown: 250),
                           new Shoot(13, count: 7, projectileIndex: 1, coolDown: 250),
                           new Shoot(13, count: 3, projectileIndex: 2, coolDown: 1000),
                           new TimedTransition(6500, "SpawnNsandRingShotgun")
                           ),
                       new State("SpawnNsandRingShotgun",
                           new Shoot(10, count: 8, projectileIndex: 3, coolDown: 1570),
                           new Shoot(13, count: 2, shootAngle: 2, projectileIndex: 5, coolDown: 750),
                           new TimedTransition(6000, "WarnThePlayer")
                           ),
                       new State("WarnThePlayer",
                           new Taunt(1.00, "You will be Haunted!!", "Hahaha...", "Fear me"),
                           new Flash(0xFF00FF, 2, 2),
                           new ConditionalEffect(ConditionEffectIndex.Armored),
                           new TimedTransition(3000, "BlastTheMelees")
                           ),
                       new State("BlastTheMelees",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Shoot(10, count: 8, projectileIndex: 3, coolDown: 1570),
                           new Shoot(13, count: 2, shootAngle: 2, projectileIndex: 2, coolDown: 750),
                           new TimedTransition(3000, "MoShotguns")
                           ),
                       new State("MoShotguns",
                           new Shoot(10, count: 20, projectileIndex: 0, coolDown: 300),
                           new Shoot(13, count: 1, projectileIndex: 4, coolDown: 200),
                           new Shoot(13, count: 1, projectileIndex: 3, predictive: 5, coolDown: 1000),
                           new TimedTransition(5000, "PurpleShotgunPhase")
                           ),
                       new State("RemovePowerandDie1",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Taunt("I'm immortal! My body will be reborn here!"),
                           new TimedTransition(5000, "RemovePowerandDie")
                           ),
                         new State("RemovePowerandDie",
                           new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                           new Shoot(10, count: 10, projectileIndex: 6, coolDown: 400),
                           new Suicide()
                     )
                ),
                   new Threshold(.00001,
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot(item: "Potion of Vitality", probability: 0.3),
                    new ItemLoot(item: "Potion of Wisdom", probability: 0.3),
                    new ItemLoot("Potion of Defense", 0.5),
                    new ItemLoot("50 Credits", 0.01),
                    new ItemLoot("Haunted Omen's Key", 0.01),
                    new ItemLoot("Robe of the Cadet", 0.006, damagebased: true),
                    new ItemLoot("Blood Vial Necklace", 0.001, damagebased: true, threshold: 0.01),
                    new ItemLoot("Haunted Shield", 0.004, damagebased: true)
                     )


            )

        
            
            

            .Init("OM Loot Chest",
            new State(
          new State("timed",
          new ConditionalEffect(ConditionEffectIndex.Invincible),
          new TimedTransition(5000, "loot")
              ),
          new State("loot"
              )),
                 new Threshold(.01,
                    new ItemLoot("Potion of Defense", 1),
                    new ItemLoot("Potion of Defense", 0.5)
                     ),
                 new Threshold(.01,
                    new ItemLoot("Haunted Omen's Key", 0.02)
                     ),
                 new Threshold(.01,
                     new ItemLoot("Blood Vial Necklace", 0.0065, damagebased: true, threshold: 0.01),
                     new ItemLoot("Haunted Shield", 0.006, damagebased: true, threshold: 0.01)

            )

        )
            ;
    }
}