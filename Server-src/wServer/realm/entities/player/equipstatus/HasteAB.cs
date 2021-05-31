using common.resources;
using wServer.networking.packets.outgoing;
namespace wServer.realm.entities.player.equipstatus
{
    class HasteAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Haste;

        public void OnEquip(Player player)
        {
        }
        private bool active;
        public void OnHit(Player player, int dmg)
        {
            if (active == false)
            {
                player.ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Haste, DurationMS = 3000 });
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFFFF00),
                    Message = "{\"key\": \"Haste(3s)\"}"
                }, PacketPriority.Low);
                active = true;
                player.Owner.Timers.Add(new WorldTimer(3000, (world, t) =>
                {
                    active = false;
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
