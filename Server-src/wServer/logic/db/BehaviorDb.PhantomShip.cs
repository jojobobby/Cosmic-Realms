using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PhantomShip = () => Behav()
        
            .Init("Haunted Ghost Ship",
                new State(
                    new DropPortalOnDeath(target: "Glowing Realm Portal"),
                    new State("Begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(dist: 11, targetState: "Attack_One", seeInvis: true)
                    ),
                    new State("Attack_One",
                    new Swirl(0.2, 3, 10, false),
                    new Wander(0.5),
                    new Shoot(120, count: 3, shootAngle: 15, projectileIndex: 0, coolDown: 550),
                    new Shoot(120, count: 8, projectileIndex: 1, coolDown: 1000, coolDownOffset: 950),
                    new TimedTransition(3500, "Attack_Two"),
                    new HpLessTransition(0.50, "Phase_2")
                        ),
                    new State("Attack_Two",
                    new Follow(0.5, 10,0),
                    new Shoot(120, count: 1, projectileIndex: 2, coolDown: 450),
                    new Shoot(120, count: 3, shootAngle: 10, projectileIndex: 0, coolDown: 850),
                    new Shoot(120, count: 8, projectileIndex: 1, coolDown: 1500),
                    new TimedTransition(3500, "Attack_Three"),
                    new HpLessTransition(0.50, "Phase_2")
                      ),
                    new State("Attack_Three",
                    new StayBack(0.3, 5),
                    new Wander(0.1),
                    new HealSelf(500, 10000),
                    new Shoot(120, count: 3, projectileIndex: 1, coolDown: 500),
                    new Shoot(120, count: 6, shootAngle: 10, projectileIndex: 3, coolDown: 850),
                    new TimedTransition(3500, "Attack_One"),
                    new HpLessTransition(0.50, "Phase_2")
                    ),
                    new State("Phase_2",
                    new ReturnToSpawn(1),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TimedTransition(2500,"P2Attack_One")
                    ),
                    new State("P2Attack_One",
                    new Shoot(120, count: 1, shootAngle: 15, projectileIndex: 2, coolDown: 550),
                    new Shoot(120, count: 3, shootAngle: 15, projectileIndex: 0, coolDown: 950),
                    new Shoot(120, count: 8, projectileIndex: 3, coolDown: 1000, coolDownOffset: 1200),
                    new TimedTransition(3500, "P2Attack_Two")
                    ),
                    new State("P2Attack_Two",
                    new Grenade(5, 150, 3, fixedAngle: 0, coolDown: 9000),
                    new Grenade(5, 150, 3, fixedAngle: 90, coolDown: 9000),
                    new Grenade(5, 150, 3, fixedAngle: 180, coolDown: 9000),
                    new Grenade(5, 150, 3, fixedAngle: 270, coolDown: 9000),
                    new TimedTransition(2000, "P2Attack_Three")
                      ),
                    new State("P2Attack_Three",
                    new Grenade(5, 150, 6, fixedAngle: 0, coolDown: 9000),
                    new Grenade(5, 150, 6, fixedAngle: 90, coolDown: 9000),
                    new Grenade(5, 150, 6, fixedAngle: 180, coolDown: 9000),
                    new Grenade(5, 150, 6, fixedAngle: 270, coolDown: 9000),
                    new TimedTransition(2000, "P2Attack_Four")
                    ),
                    new State("P2Attack_Four",
                    new Grenade(5, 150, 9, fixedAngle: 0, coolDown: 9000),
                    new Grenade(5, 150, 9, fixedAngle: 90, coolDown: 9000),
                    new Grenade(5, 150, 9, fixedAngle: 180, coolDown: 9000),
                    new Grenade(5, 150, 9, fixedAngle: 270, coolDown: 9000),
                    new TimedTransition(2000, "P2Attack_Five")
                           ),
                    new State("P2Attack_Five",
                    new Wander(0.1),
                    new Shoot(120, count: 8, projectileIndex: 3, coolDown: 1000),
                    new Shoot(120, count: 6, projectileIndex: 2, fixedAngle:0, coolDown: 1250),
                    new Shoot(120, count: 6, projectileIndex: 0, fixedAngle:25, coolDown: 1500),
                    new Shoot(120, count: 1, shootAngle: 1, projectileIndex: 0, coolDown: 750),
                    new TimedTransition(2500, "P2Attack_One")
                )
            )
            )
            ;
    }
}