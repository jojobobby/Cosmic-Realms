using wServer.logic.behaviors;
using wServer.logic.transitions;
using common.resources;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CourtofOryx = () => Behav()
            .Init("Craig, Intern of the Mad God",
              new State(
                  new ConditionalEffect(ConditionEffectIndex.Invincible),
                  new State("1",
                      new PlayerWithinTransition(6, "2")
                  ),
                  new State("2",
                      new TimedTransition(3000, "3")
                  ),
                  new State("3",
                      new Taunt("Oh. This is unexpected. We've never gotten anything like you before."),
                      new TimedTransition(8000, "4")
                  ),
                  new State("4",
                      new PlayerTextTransition("rage", "skip", 20),
                      new State("5",
                          new Taunt("So, you're probably here to see a member of the court. Problem is, I'm new here."),
                          new TimedTransition(9500, "6")
                      ),
                      new State("6",
                          new Taunt("This is the deal: I throw a chest, we hope for the best, and no one tells Oryx. Cool? Alright! Let me get that for you."),
                          new TimedTransition(9500, "7")
                      )
                  ),
                  new State("7",
                      new TossObject("Event Chest", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("Idle"),
                  new State("rage",
                      new Taunt("flut"),
                      new TimedRandomTransition(1000, false, "oryx", "thessal", "permafrost", "bes", "nut", "geb")
                  ),
                  new State("oryx",
                      new TossObject("Tidale, The Defender of the Ancients", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("thessal",
                      new TossObject("Thessal the Mermaid Goddess", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("permafrost",
                      new TossObject("Permafrost Lord", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("bes",
                      new TossObject("Tomb Defender", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("nut",
                      new TossObject("Tomb Support", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  ),
                  new State("geb",
                      new TossObject("Tomb Attacker", 6, 90, throwEffect: true),
                      new TimedTransition(500, "Idle")
                  )
             )
           )
        ;
    }
}