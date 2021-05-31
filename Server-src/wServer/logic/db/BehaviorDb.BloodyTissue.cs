using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
using common.resources;
// by Classic White
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ BloodyTissue = () => Behav()
            .Init("Bloody Tissue",
              new State(
                 new ScaleHP2(85, 2, 15),
                new State("1",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new TimedTransition(3000, "2")
                ),
                new State("2",
                    new RemoveConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Wander(0.1),
                    new Shoot(20, 10, 15, 0, 90, coolDown: 6000, coolDownOffset: 3000),
                    new Shoot(20, 10, 15, 0, 270, coolDown: 6000, coolDownOffset: 3000),
                    new Shoot(20, 10, 15, 0, 360, coolDown: 6000, coolDownOffset: 6000),
                    new Shoot(20, 10, 15, 0, 180, coolDown: 6000, coolDownOffset: 6000),
                    new Shoot(20, 1, projectileIndex: 3, predictive: 0.1, coolDown: 5000),
                    new HpLessTransition(.8, "3")
                ),
                new State("3",
                    new Wander(0.15),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                    new Shoot(22, 12, 30, 4, 360, coolDown: 2500),
                    new Shoot(20, 3, 25, 1, predictive: 0.1, coolDown: 1000, coolDownOffset: 500),
                    new Shoot(20, 2, 15, 1, predictive: 0.1, coolDown: 1000, coolDownOffset: 1000),
                    new HpLessTransition(.5, "4")
                ),
                new State("4",
                    new Wander(0.2),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 3000),
                    new Shoot(20, 20, 13.5, 5, 30, coolDown: 12000, coolDownOffset: 1000),
                    new Shoot(20, 20, 13.5, 5, 60, coolDown: 12000, coolDownOffset: 2000),
                    new Shoot(20, 20, 13.5, 5, 90, coolDown: 12000, coolDownOffset: 3000),
                    new Shoot(20, 20, 13.5, 5, 120, coolDown: 12000, coolDownOffset: 4000),
                    new Shoot(20, 20, 13.5, 5, 150, coolDown: 12000, coolDownOffset: 5000),
                    new Shoot(20, 20, 13.5, 5, 180, coolDown: 12000, coolDownOffset: 6000),
                    new Shoot(20, 20, 13.5, 5, 210, coolDown: 12000, coolDownOffset: 7000),
                    new Shoot(20, 20, 13.5, 5, 300, coolDown: 12000, coolDownOffset: 10000),
                    new Shoot(20, 20, 13.5, 5, 330, coolDown: 12000, coolDownOffset: 11000),
                    new Shoot(20, 20, 13.5, 5, 360, coolDown: 12000, coolDownOffset: 12000),
                    new Shoot(20, 3, 10, 2, predictive: 0.1, coolDown: 700),
                    new HpLessTransition(.2, "5")
                ),
                new State("5",
                    new Wander(0.25),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, duration: 2000),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 0, coolDown: 1800, coolDownOffset: 800),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 270, coolDown: 1800, coolDownOffset: 800),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 45, coolDown: 1800, coolDownOffset: 1800),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 135, coolDown: 1800, coolDownOffset: 1800),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 225, coolDown: 1800, coolDownOffset: 1800),
                    new Shoot(20, projectileIndex: 5, count: 5, shootAngle: 12, fixedAngle: 315, coolDown: 1800, coolDownOffset: 1800)
                )
              ),
              new Threshold(0.0001,
                  new ItemLoot("Dirk of Bleeding Purity", 0.001, damagebased: true, threshold: 0.01),
                  new ItemLoot("Blood Bath", 0.004, damagebased: true),

                  new ItemLoot("Flawed panacea", 0.003, damagebased: true),
                  new ItemLoot("Blood-drenched hope", 0.003, damagebased: true),
                  new ItemLoot("Death certificate", 0.003, damagebased: true),
                  new ItemLoot("Tendon rupture", 0.003, damagebased: true),

                  new ItemLoot("Awoken Blood Bath", 0.001, damagebased: true, threshold: 0.01),
                  new ItemLoot("Red Ichor", 0.004, damagebased: true, threshold: 0.01),
                  new ItemLoot("Potion of Vitality", 0.30),
                  new ItemLoot("Potion of Vitality", 0.80),
                  new ItemLoot("Potion of Wisdom", 1)
              )
            )
        ;
    }
}