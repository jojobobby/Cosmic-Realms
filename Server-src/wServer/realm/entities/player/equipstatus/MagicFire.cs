using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class FMagicAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.FMagicAB;

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


            x = (player.Stats[1] - 400) / 30;

            if (x < 0)
                x = 0;

            player.Stats.Boost.ActivateBoost[2].Push(x, true);
            player.Stats.Boost.ActivateBoost[5].Push(x, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(1250, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[2].Pop(x, true);
                player.Stats.Boost.ActivateBoost[5].Pop(x, true);
                player.Stats.ReCalculateValues();
            }));

            if (y != x)
            {
                y = x;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xF21F1F),
                    Message = "Set Ablaze (" + y + ")"
                }, PacketPriority.Low);
            }


        }

        public void Unequip(Player player)
        {

        }
    }
}
