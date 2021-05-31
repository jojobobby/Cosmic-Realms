using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EggPets = () => Behav()


           .Init("Void Dragon Pet",
                new State(
                    new State("Nothing"
                        ),
                     new State("Open",
                         new Taunt("The Void is my Brother!"),
                         new Suicide()
                )
            )
            )











            ;
    }
}