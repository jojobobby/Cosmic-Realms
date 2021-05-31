using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Beroth = () => Behav()
          
       
           .Init("Earth Chunk",
                new State(
                    new ScaleHP2(20, 3, 15),
                    new State("Start",
                        new TimedTransition(1550,"true")   
                        ),
                    new State("true",
                    new Shoot(radius: 60, count: 6, fixedAngle: 10, coolDown: 1500),
                    new Orbit(0.5,8,10, "Beroth, the Earth Dragon"),
                    new HpLessTransition(0.75,"2")
                           ),
                    new State("2",
                    new SetAltTexture(1),
                    new Shoot(radius: 60, count: 6, fixedAngle: 10, coolDown: 1400),
                    new Orbit(0.5, 8, 10, "Beroth, the Earth Dragon"),
                    new HpLessTransition(0.50, "3")
                           ),
                    new State("3",
                    new SetAltTexture(2),
                    new Shoot(radius: 60, count: 6, fixedAngle: 10, coolDown: 1300),
                    new Orbit(0.5, 8, 10, "Beroth, the Earth Dragon"),
                    new HpLessTransition(0.25, "4")
                           ),
                    new State("4",
                    new SetAltTexture(3),
                    new Shoot(radius: 60, count: 6, fixedAngle: 10, coolDown: 1200),
                    new Orbit(0.5, 8, 10, "Beroth, the Earth Dragon")

                )
                    )
            )
               


            .Init("Beroth, the Earth Dragon",
                new State(
                    new ScaleHP2(50, 3, 15),
                    new State("Start",
                    new SetAltTexture(1),
                    new PlayerWithinTransition(7, "2")
                    ),
                    new State("2",
                    new SetAltTexture(2),
                    new TossObject("Earth Chunk", 4, 90, 99999999, 0, true),
                    new TossObject("Earth Chunk", 4, 180, 99999999, 0, true),
                    new TossObject("Earth Chunk", 4, 270, 99999999, 0, true),
                    new TossObject("Earth Chunk", 4, 360, 99999999, 0, true),
                    new Taunt("You're not suppose to be here."),
                    new TimedTransition(1550, "3")
                    ),
                    new State("3",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new SetAltTexture(0),
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 10, coolDown: 300),
                    new EntityNotExistsTransition("Earth Chunk", 10, "4")
                    ),
                    new State("4",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You dare test me?"),
                    new TimedTransition(1500,"5")
                    ),
                    new State("5",
                    new Shoot(radius: 160, count: 4, fixedAngle: 0, rotateAngle: 5, coolDown: 100),
                    new Shoot(radius: 160, projectileIndex: 1, count: 4, predictive: 0.4, rotateAngle: -25, fixedAngle: 0, coolDown: 800),
                    new HpLessTransition(0.05, "6")
                    ),
                    new State("6",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Taunt("I was created from the earth! I will be back."),
                    new TimedTransition(1250,"7")
                    ),
                    new State("7",
                       new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Suicide()
                )
                    ),
                new Threshold(0.0001,
                    new ItemLoot("Potion of Speed", 1),
                    new ItemLoot("Earth Dragon Blade", 0.02),
                    new TierLoot(tier: 6, type: ItemType.Armor, probability: 0.4),
                    new TierLoot(tier: 7, type: ItemType.Weapon, probability: 0.25),
                    new TierLoot(tier: 9, type: ItemType.Weapon, probability: 0.125),
                    new TierLoot(tier: 3, type: ItemType.Ability, probability: 0.25),
                    new TierLoot(tier: 4, type: ItemType.Ability, probability: 0.125),
                    new TierLoot(tier: 7, type: ItemType.Armor, probability: 0.4),
                    new TierLoot(tier: 9, type: ItemType.Armor, probability: 0.25),
                    new TierLoot(tier: 3, type: ItemType.Ring, probability: 0.25),
                    new TierLoot(tier: 4, type: ItemType.Ring, probability: 0.125),
                    new TierLoot(tier: 10, type: ItemType.Weapon, probability: 0.0625),
                    new TierLoot(tier: 10, type: ItemType.Armor, probability: 0.125)
                )
            )
            ;
    }
}