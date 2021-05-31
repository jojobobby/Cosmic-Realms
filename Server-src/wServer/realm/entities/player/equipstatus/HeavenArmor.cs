using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class HeavenArmor : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Heavenarmor;

        public void OnEquip(Player player) { }

        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            //using wServer.networking.packets.outgoing;
          
            if (player.HP <= player.Stats[0] / 2)
            {
                player.Stats.Boost.ActivateBoost[6].Push(30, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[6].Pop(30, true);
                    player.Stats.ReCalculateValues();
                }));
            }
            
        }

        public void Unequip(Player player)
        {
            
        }
    }
}
