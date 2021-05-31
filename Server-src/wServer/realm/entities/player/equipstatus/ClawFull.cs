using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class ClawFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CLAWFull;

        private int test;
        private bool ppp;
        public void OnEquip(Player player)
        {
            test = 1;
        }

        public void OnHit(Player player, int dmg) {

        }
        public void OnTick(Player player, RealmTime time)
        {
            if (ppp == false && test == 1)
            {
                if (player.HasConditionEffect(ConditionEffects.Invisible))
                {
                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xFFCCCB),
                        Message = "Super Saiyan 2"
                    }, PacketPriority.Low);
                    test = 2;
                }
            }

            if (player.HasConditionEffect(ConditionEffects.Invisible))
            {
                ppp = false;
                player.Stats.Boost.ActivateBoost[11].Push(75, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[11].Pop(75, true);
                    player.Stats.ReCalculateValues();
                }));
            }  else
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
