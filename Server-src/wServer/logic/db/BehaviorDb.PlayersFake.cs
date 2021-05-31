using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ PlayersFake = () => Behav()
          
            .Init("Mythical Knight",
                new State(
                    new State("Start",
                    new Follow(speed: 0.5, range: 12),
                    new Wander(speed: 0.2),
                    new Shoot(80, count: 1, projectileIndex: 0, shootAngle: 0, predictive: 0.6, coolDown: 150),
                    new Shoot(20, count: 5, projectileIndex: 1, shootAngle: 12, predictive: 0.4, coolDown: 1200)
                        )
                ),

                new Threshold(0.01,
                    new ItemLoot("Sword of Acclaim", 0.01),
                    new ItemLoot("Shield of Ogmur", 0.01),
                    new ItemLoot("Acropolis Armor", 0.01),
                    new ItemLoot("Bracer of the Guardian", 0.01)//Acropolis Armor
                
            ))







            ;
    }
}