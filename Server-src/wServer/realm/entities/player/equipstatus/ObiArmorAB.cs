using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class ObiArmorAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.ObiArmorAB;

        private int x;
        private int y;
        private int test;
        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
        }

        public void OnTick(Player player, RealmTime time)
        {


            x = (player.Stats[0] - 1000);
            

            if (x < 0)
                x = 0;

            test = x / 5;


            player.Stats.Boost.ActivateBoost[3].Push(test, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[3].Pop(test, true);
                player.Stats.ReCalculateValues();
            }));
            
            if (y != test)
            {
                y = test;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x838E93),
                    Message = "Absolute Unit (" + y + ")"
                }, PacketPriority.Low);
            }


        }

        public void Unequip(Player player)
        {
            
        }
    }
}
