using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class IMagicAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.IMagicAB;

        private int x = 1;
        private int y = 1;
        public void OnEquip(Player player)
        {
            x = 1;
            y = 1;
        }

        public void OnHit(Player player, int dmg)
        {
        }

        public void OnTick(Player player, RealmTime time)
        {


            x = (player.Stats[1] - 500) / 15;

            if (x < 0)
                x = 0;

            var g = x * 2;

            player.Stats.Boost.ActivateBoost[7].Push(g, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[7].Pop(g, true);
                player.Stats.ReCalculateValues();
            }));
            
            if (y != g)
            {
                y = g;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x25BEBA),
                    Message = "Frozen Solid (" + y + ")"
                }, PacketPriority.Low);
            }


        }

        public void Unequip(Player player)
        {
            
        }
    }
}
