using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class InfernoRing : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.infernoRing;

        public void OnEquip(Player player)
        {
            _hasCond = false;
        }


        private bool _hasCond;

        public void OnHit(Player player, int dmg)
        {
            if (dmg > 50 && _hasCond == false)
            {
                //using wServer.networking.packets.outgoing;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xBE5425),
                    Message = "{\"key\": \"Inferno's Protection\"}"
                }, PacketPriority.Low);
                _hasCond = true;
                player.Stats.Boost.ActivateBoost[0].Push(100, true);
                player.Stats.ReCalculateValues();
                
                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[0].Pop(100, true);
                    player.Stats.ReCalculateValues();
                    _hasCond = false;
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
