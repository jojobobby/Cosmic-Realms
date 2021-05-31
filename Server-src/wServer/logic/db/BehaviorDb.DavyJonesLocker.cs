using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
//by GhostMaree
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ DavyJonesLocker = () => Behav()
            .Init("Davy Jones",
                new State(
                    new ScaleHP2(40,3,15),
                    new DropPortalOnDeath("Glowing Realm Portal"),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new State("Floating",
                            new ChangeSize(100, 100),
                            new SetAltTexture(3),
                            new Wander(.6),
                            new StayCloseToSpawn(0.2, 8),
                            new Shoot(10, 5, 10, 0, coolDown: 1000),
                            new Shoot(10, 1, 10, 1, coolDown: 2000),
                            new EntityExistsTransition("Ghost Lanturn On", 99, "CheckOffLanterns")
                            ),
                        new State("CheckOffLanterns",
                            new SetAltTexture(2),
                            new ReturnToSpawn(1),
                            new Shoot(10, 5, 10, 0, coolDown: 1000, predictive: 0.5),
                            new Shoot(10, 1, 10, 1, coolDown: 500),
                            new EntityNotExistsTransition("Ghost Lanturn Off", 99, "Vunerable")
                            )
                        ),
                    new State("Vunerable",
                        new OrderOnce(99, "Ghost Lanturn On", "Stay Here"),
                        new SetAltTexture(5, 6, 500, true),
                        new TimedTransition(2500, "deactivate 1")
                        ),
                    new State("deactivate 1",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new SetAltTexture(4),
                        new OrderOnce(99, "Ghost Lanturn On", "Shoot"),
                        new TimedTransition(3500, "Idle")
                        )
                    ),
                new Threshold(0.00001,
                    new ItemLoot("Potion of Wisdom", 0.80, 3),
                    new ItemLoot("Potion of Wisdom", 0.80, 3),
                    new ItemLoot("Potion of Wisdom", 1),
                    new ItemLoot("Ghost Pirate Rum", 0.4),
                    new ItemLoot("Ghost Pirate Rum", 0.3),
                    new ItemLoot("Ghost Pirate Rum", 0.2),
                    new ItemLoot("50 Credits", 0.01),
                    new TierLoot(8, ItemType.Weapon, 0.25),
                    new TierLoot(9, ItemType.Weapon, 0.125),
                    new TierLoot(8, ItemType.Armor, 0.25),
                    new TierLoot(9, ItemType.Armor, 0.125),
                    new TierLoot(3, ItemType.Ability, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.125),
                    new TierLoot(4, ItemType.Ring, 0.125),
                    new TierLoot(10, ItemType.Armor, 0.125),
                    new TierLoot(5, ItemType.Ability, 0.0625),
                    new ItemLoot("Captain's Ring", 0.01),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Spectral Cloth Armor", 0.01), 
                    new ItemLoot("Swashbuckler's Sickle", 0.004, damagebased: true),
                    new ItemLoot("Ancient Dusted Katana", 0.004, damagebased: true),
                    new ItemLoot("Spirit Dagger", 0.004, damagebased: true),
                    new ItemLoot("Ghostly Prism", 0.006, damagebased: true),
                    new ItemLoot("Bow of Morel Fungus", 0.006, damagebased: true),
                    new ItemLoot("Mark of Davy Jones", 1)
                    )
            )
            .Init("Ghost Lanturn Off",
                new State(
                    new TransformOnDeath("Ghost Lanturn On")
                    )
            )
            .Init("Ghost Lanturn On",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new State("idle",
                        new TimedTransition(8000, "gone")
                        ),
                    new State("Stay Here"),
                    new State("Shoot",
                        new Shoot(10, 6, coolDown: 9000001, coolDownOffset: 100),
                        new TimedTransition(250, "gone")
                        ),
                    new State("gone",
                        new Transform("Ghost Lanturn Off")
                        )
                    )
            )
            ;
    }
}