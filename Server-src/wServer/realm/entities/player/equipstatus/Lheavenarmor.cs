using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class Lheavenarmor : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Lheavenarmor;
        private bool actuive;
        public void OnEquip(Player player)
        {
            
        }
        

        public void OnHit(Player player, int dmg)
        {
            
            
                  
                
             
            
        }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.HP == player.Stats[0])
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xEE82EE),
                    Message = "Lunar Energy"
                }, PacketPriority.Low);

                player.Stats.Boost.ActivateBoost[0].Push(160, true);
                player.Stats.Boost.ActivateBoost[6].Push(40, true);
                player.Stats.Boost.ActivateBoost[3].Push(25, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[0].Pop(160, true);
                    player.Stats.Boost.ActivateBoost[6].Pop(40, true);
                    player.Stats.Boost.ActivateBoost[3].Pop(25, true);
                    player.Stats.ReCalculateValues();

                }));
            }
          
        }

        public void Unequip(Player player)
        {
          
        }
    }
}
