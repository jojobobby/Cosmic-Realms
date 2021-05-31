using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class LGClothesAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGCLOTHYESAB;

        private int _statVal;
        private bool _hasStat;
        private bool Said;
        public void OnEquip(Player player)
        {
           
        }

        public void OnHit(Player player, int dmg)
        {
           
            if (player.HP <= 500 && Said == false)
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xC91E32),
                    Message = "{\"key\": \"Assassin's Pride\"}"
                }, PacketPriority.Low);
                Said = true;
                if (player.HP > 500)
                {
                    Said = false;
                }
               
            }
        }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.HP <= 500)
            {
                _statVal = (player.Stats.Base[12] * 2);
                // using wServer.networking.packets.outgoing;
               

                player.Stats.Boost.ActivateBoost[12].Push(_statVal, true);
                player.Stats.ReCalculateValues();
                _hasStat = true;
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[12].Pop(_statVal, true);
                    player.Stats.ReCalculateValues();
                }));

            }
            if (player.HP > 500 && _hasStat == true)
            {
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[12].Pop(_statVal, true);
                    player.Stats.ReCalculateValues();
                    _hasStat = false;

                }));
             
            }
          
        }

        public void Unequip(Player player)
        {
        }
    }
}
