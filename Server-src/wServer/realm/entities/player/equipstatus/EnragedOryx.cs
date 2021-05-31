using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class EnragedOryx : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.enrageoryx;

        public void OnEquip(Player player)
        {
           
        }

        public void OnHit(Player player, int dmg)
        {
            //using wServer.networking.packets.outgoing;
          

            if (RandomUtil.RandInt(1, 5) == 1)
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x25BE54),
                    Message = "{\"key\": \"Enraged Plate\"}"
                }, PacketPriority.Low);

                player.Stats.Boost.ActivateBoost[5].Push(10, true);
                player.Stats.Boost.ActivateBoost[2].Push(10, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[5].Pop(10, true);
                    player.Stats.Boost.ActivateBoost[2].Pop(10, true);
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
