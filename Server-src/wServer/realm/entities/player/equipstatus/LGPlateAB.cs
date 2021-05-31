using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class LGPlateAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGPLATEYESAB;

        private int _statVal;
        private int test;
        private bool ppp;
        public void OnEquip(Player player)
        {
            test = 1;
        }

        public void OnHit(Player player, int dmg)
        {
        }

        public void OnTick(Player player, RealmTime time)
        {
            if (ppp == false && test == 1)
            {
                if (player.HP < player.Stats[0] / 2)
                {
                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0x102F3F),
                        Message = "Plated Guardian"
                    }, PacketPriority.Low);
                    test = 2;
                }
            }

            _statVal = player.Stats.Base[3] / 5;

            if (player.HP < player.Stats[0] / 2)
            {
                ppp = false;
                player.Stats.Boost.ActivateBoost[3].Push(_statVal, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[12].Pop(_statVal, true);
                    player.Stats.ReCalculateValues();
                }));
            }
            else
            {
                ppp = true;
                test = 1;
            }
        }

        public void Unequip(Player player)
        {
         
        }
    }
}
