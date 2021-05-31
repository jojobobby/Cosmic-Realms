using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class CrystalArmor : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CrystalArmor;
        private bool actuive;
        public void OnEquip(Player player)
        {
            
        }
        

        public void OnHit(Player player, int dmg)
        {
            
            if (dmg >= 25)
            {
                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xd3d3d3),
                        Message = "Crystal Thorns"
                    }, PacketPriority.Low);

                    player.Stats.Boost.ActivateBoost[2].Push(8, false);
                    player.Stats.ReCalculateValues();
                    player.Stats.Boost.ActivateBoost[5].Push(8, false);
                    player.Stats.ReCalculateValues();
                    player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                    {
                        player.Stats.Boost.ActivateBoost[2].Pop(8, false);
                        player.Stats.ReCalculateValues();
                        player.Stats.Boost.ActivateBoost[5].Pop(8, false);
                        player.Stats.ReCalculateValues();
                        
                    }));
                
             
            }
        }

        public void OnTick(Player player, RealmTime time)
        {
           
        }

        public void Unequip(Player player)
        {
          
        }
    }
}
