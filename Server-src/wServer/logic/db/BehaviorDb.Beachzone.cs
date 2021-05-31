using wServer.logic.behaviors;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Beachzone = () => Behav()
            .Init("Masked Party God",
                new State(
                    new SetAltTexture(1, 3, 500, true),
                    new Taunt(true, 2500,
                        "Lets have a fun-time in the sun-shine!",
                        "Oh no, Mixcoatl is my brother, I prefer partying to fighting.",
                        "Nothing like relaxin' on the beach.",
                        "Chillin' is the name of the game!",
                        "I hope you're having a good time!",
                        "How do you like my shades?",
                        "EVERYBODY BOOGEY!",
                        "What a beautiful day!",
                        "Whoa there!",
                        "Oh SNAP!",
                        "Ho!"),
                    new HealSelf(5000, 30000)
                ),
                new Threshold(0.01,
                    new ItemLoot("Blue Paradise", 0.4),
                    new ItemLoot("Pink Passion Breeze", 0.4),
                    new ItemLoot("Bahama Sunrise", 0.4),
                    new ItemLoot("Lime Jungle Bay", 0.4),
                    new ItemLoot("Staff of the Rising Sun", 0.01),
                    new ItemLoot("Thousand Suns Spell", 0.01),
                    new ItemLoot("Robe of the Summer Solstice", 0.01),
                    new ItemLoot("Ring of the Burning Sun", 0.01)
                )
            );
    }
}
