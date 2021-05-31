using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class KazeFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.KazeFull;

        public void OnEquip(Player player)
        {
            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0xBE3425),
                Message = "{\"key\": \"Kaze Blade\"}"
            }, PacketPriority.Low);

        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) {
        

            player.Stats.Boost.ActivateBoost[5].Push(7, true);
            player.Stats.Boost.ActivateBoost[3].Push(17, true);
            player.Stats.Boost.ActivateBoost[0].Push(60, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[5].Pop(7, true);
                player.Stats.Boost.ActivateBoost[3].Pop(17, true);
                player.Stats.Boost.ActivateBoost[0].Pop(60, true);
                player.Stats.ReCalculateValues();
            }));

        }

        public void Unequip(Player player)
        {
        }
    }
}
