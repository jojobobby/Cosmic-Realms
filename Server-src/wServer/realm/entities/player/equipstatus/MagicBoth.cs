using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class CMagicAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CMagicAB;

        private int x = 1;
        private int y = 1;
        private int g = 1;
        public void OnEquip(Player player)
        {
            x = 1;
            y = 1;
               g = 1; 
        }

        public void OnHit(Player player, int dmg)
        {
        }

        public void OnTick(Player player, RealmTime time)
        {


            x = (player.Stats[1] - 500) / 30;

            if (x < 0)
                x = 0;
            g = x + x;
            player.Stats.Boost.ActivateBoost[2].Push(x, true);
            player.Stats.Boost.ActivateBoost[5].Push(x, true);
            player.Stats.Boost.ActivateBoost[7].Push(g, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(1250, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[2].Pop(x, true);
                player.Stats.Boost.ActivateBoost[5].Pop(x, true);
                player.Stats.Boost.ActivateBoost[7].Pop(g, true);
                player.Stats.ReCalculateValues();
            }));

            if (y != x)
            {
                y = x;
                var duh = x + x;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x9F77A4),
                    Message = "Universal [" + y + ", " + duh + "]"
                }, PacketPriority.Low);
            }


        }

        public void Unequip(Player player)
        {

        }
    }
}
