
using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class QuiverNia : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.QuiverNia;

        private int _statVal;
        private int _statVal2;
        private int _statVal3;

        public void OnEquip(Player player)
        {
          
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) {

            _statVal = player.Stats[1] / 100;
            _statVal2 = _statVal * 15;
            _statVal3 = _statVal2;
            player.Stats.Boost.ActivateBoost[2].Push(_statVal3, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[2].Pop(_statVal3, true);
                player.Stats.ReCalculateValues();
            }));



        }

        public void Unequip(Player player)
        {
           
        }
    }
}
