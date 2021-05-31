using common.resources;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Keyper  = () => Behav()

          .Init("The Keyper",
          new State(
               new DropPortalOnDeath("Mountain Temple Portal", 100),
                new ScaleHP2(67, 3, 15),
                new StayCloseToSpawn(0.2, 5),
              new State("Idle",
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new PlayerWithinTransition(12, "Uh oh")
                  ),
              new State("Uh oh",
                  new Taunt("Oryx has sent me to check on you realmers, I shall finish you off if you choose not to fight back!"),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedTransition(3000, "Fire Stream")
                  ),
              new State("Fire Stream",
                  new Wander(0.15),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 9, fixedAngle: 5 * 9, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 10, fixedAngle: 5 * 10, shootAngle: 60),
                  new Shoot(30, projectileIndex: 0, count: 3, coolDown: 300, shootAngle: 16),
                  new HpLessTransition(0.80, "Fire Rings")
                  ),
              new State("Fire Rings",
                  new Wander(0.15),
                  new Taunt("You're in for a little treat if you're able to defeat me!"),
                  new Shoot(30, projectileIndex: 0, count: 3, coolDown: 100, shootAngle: 16),
                  new Shoot(30, projectileIndex: 2, count: 7, coolDown: 1000, shootAngle: 360 / 7),
                  new Shoot(30, projectileIndex: 3, count: 7, coolDown: 1000, shootAngle: 360 / 7),
                  new HpLessTransition(0.60, "Fire Tentacles")
                  ),
              new State("Fire Tentacles",
                  new Wander(0.15),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 0, fixedAngle: 0, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 1, fixedAngle: 5 * 1, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 2, fixedAngle: 5 * 2, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 3, fixedAngle: 5 * 3, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 4, fixedAngle: 5 * 4, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 5, fixedAngle: 5 * 5, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 6, fixedAngle: 5 * 6, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 7, fixedAngle: 5 * 7, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 8, fixedAngle: 5 * 8, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 9, fixedAngle: 5 * 9, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 10, fixedAngle: 5 * 10, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 11, fixedAngle: 5 * 11, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 12, fixedAngle: 5 * 12, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 13, fixedAngle: 5 * 13, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 14, fixedAngle: 5 * 14, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 15, fixedAngle: 5 * 15, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 16, fixedAngle: 5 * 16, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 17, fixedAngle: 5 * 17, shootAngle: 60),
                  new Shoot(50, projectileIndex: 1, count: 6, coolDown: 200 * 20, coolDownOffset: 200 * 18, fixedAngle: 5 * 18, shootAngle: 60),
                  new HpLessTransition(0.40, "Fire Waves")
                  ),
              new State("Fire Waves",
                  new Wander(0.15),
                  new Taunt("Enough playing around, lets get serious!"),
                  new Shoot(30, projectileIndex: 0, count: 1, coolDown: 200, shootAngle: 16),
                  new Shoot(30, projectileIndex: 1, count: 1, coolDown: 1000),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, fixedAngle: 0, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 0, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 400, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 800, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1200, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 1600, fixedAngle: 180, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, fixedAngle: 90, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 5, coolDown: 4000, coolDownOffset: 2000, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 2400, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 2800, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3200, fixedAngle: 270, shootAngle: 9),
                  new Shoot(50, projectileIndex: 4, count: 9, coolDown: 4000, coolDownOffset: 3600, fixedAngle: 270, shootAngle: 9),
                  new HpLessTransition(0.20, "Pre Rage")
                  ),
              new State("Pre Rage",
                  new Taunt("hmm..."),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new TimedTransition(6000, "Rage")
                  ),
              new State("Rage",
                  new Flash(0xff0000, 2, 999999),
                  new Wander(0.15),
                  new Shoot(30, projectileIndex: 0, count: 3, coolDown: 100, shootAngle: 16),
                  new Shoot(30, projectileIndex: 4, count: 1, coolDown: 250),
                  new HpLessTransition(0.05, "Ded")
                  ),
              new State("Ded",
                  new Taunt("I will be back!"),
                  new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                  new Flash(0x000000, 4, 1),
                  new TimedTransition(2000, "Oof")
                  ),
              new State("Oof",
                  new Suicide()
                  )
                    ),
              new Threshold(0.00001,
                  new ItemLoot("Potion of Life", 0.5),
                  new ItemLoot("Potion of Mana", 0.5),
                  new ItemLoot("Greater Potion of Vitality", 1),
                  new ItemLoot("Greater Potion of Vitality", 1),
                  new ItemLoot("Potion of Attack", 1),

                  //  new ItemLoot("Mastery Level Gem", 0.025),
                  // new ItemLoot("Mastery Points Gem", 0.05),

                  new ItemLoot("Fragment of the Earth", 0.01),
                  new ItemLoot("Chancellor's Claw Pendant", 0.006, damagebased: true),
                  new ItemLoot("Diplomatic Robe", 0.001, threshold: 0.01, damagebased: true),
                  new ItemLoot("Genesis Spell", 0.004, damagebased: true),
                  new ItemLoot("Superior", 0.001, threshold: 0.01, damagebased: true),

                  new TierLoot(10, ItemType.Weapon, 0.4),
                  new TierLoot(11, ItemType.Weapon, 0.4),

                  new TierLoot(11, ItemType.Armor, 0.4),
                  new TierLoot(12, ItemType.Armor, 0.4),

                  new TierLoot(5, ItemType.Ability, 0.4),

                  new TierLoot(5, ItemType.Ring, 0.45)
              )
          )












      ;


    }
}