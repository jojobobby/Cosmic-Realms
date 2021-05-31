using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class Hollyhock : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Hollyhock;

        public void OnEquip(Player player)
        {
         
        }

        private bool _hasCond;
        public void OnHit(Player player, int dmg)
        {

            if (RandomUtil.RandInt(1, 10) == 1 && _hasCond == false)
            {
                //using wServer.networking.packets.outgoing;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x25BE54),
                    Message = "{\"key\": \"Green Thumb\"}"
                }, PacketPriority.Low);
                player.Stats.Boost.ActivateBoost[5].Push(10, true);
                player.Stats.ReCalculateValues();
                _hasCond = true;
                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    _hasCond = false;
                    player.Stats.Boost.ActivateBoost[5].Pop(10, true);
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
