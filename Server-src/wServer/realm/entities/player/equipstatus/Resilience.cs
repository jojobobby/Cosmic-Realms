using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class Resilience : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Resilience;

        public void OnEquip(Player player)
        {
           

        }

        public void Unequip(Player player)
        {
        
        }

        public void OnHit(Player player, int dmg) {
           if (player.HP <= player.Stats[0] / 3)
           {
                player.HPRegenADD += 999;
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.HPRegenADD -= 999;
                }));
            }
        }
        public void OnTick(Player player, RealmTime time) { 
        
        
        }
    }
}
