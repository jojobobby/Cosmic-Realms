using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class RobeLG : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.ROBEYESLG;

        private int _statVal;

        public void OnEquip(Player player) 
        {
            
        }
        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            _statVal = System.Convert.ToInt32(player.Stats[2] * 0.20);
            if (player.HP > 500)
            {
                player.Stats.Boost.ActivateBoost[2].Push(_statVal, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[2].Pop(_statVal, true);
                    player.Stats.ReCalculateValues();
                }));
            }
        }
        public void Unequip(Player player)
        {
            
        }
    }
}
