using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class CybRingAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CybRingAB;

        int _critDmg;

      
        public void OnTick(Player player, RealmTime time)
        {

            
            player.Stats.Boost.ActivateBoost[12].Push(_critDmg, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[12].Pop(_critDmg, true);
                player.Stats.ReCalculateValues();
            }));

            if (_critDmg != player.Stats.Boost[2])
            {
                _critDmg = player.Stats.Boost[2];
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFF0000),
                    Message = "Demonic Horns (" + _critDmg + ")"
                }, PacketPriority.Low);
            }

        }
        public void OnEquip(Player player)
        {


        }
        public void Unequip(Player player)
        {
          
        }

        public void OnHit(Player player, int dmg) { }
       
    }
}
