using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class ArmorNilAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.ArmorNilAB;
        public void OnEquip(Player player)
        {
            _statBoost = 0;
        }

        private int _statBoost;
        public void OnHit(Player player, int dmg)
        { 
            if (dmg > 50)
            {
                player.Stats.Boost.ActivateBoost[3].Push(_statBoost = 28, true);
                player.Stats.ReCalculateValues();
            }
            else
            {
                player.Stats.Boost.ActivateBoost[3].Push(_statBoost = 14, true);
                player.Stats.ReCalculateValues();
            }

            player.Owner.Timers.Add(new WorldTimer(250, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[3].Pop(_statBoost, true);
                player.Stats.ReCalculateValues();
            }));
        }

        public void OnTick(Player player, RealmTime time)
        {
          
        }

        public void Unequip(Player player)
        {
        }
    }
}
