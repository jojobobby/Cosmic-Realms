using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class CorruptAB : IEquipStatus
    {
        private bool yes;
        public EquippedStatus Status => EquippedStatus.Corrupt;

        public void OnEquip(Player player)
        {
            yes = false;
        }

        public void OnHit(Player player, int dmg)
        {
            if (yes == false)
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFF0000),
                    Message = "Diminished (3s)"
                }, PacketPriority.Low);
                yes = true;
                player.Owner.Timers.Add(new WorldTimer(3000, (world, t) =>
                {
                    yes = false;
                }));
                player.ApplyConditionEffect(ConditionEffectIndex.Diminished, 3000);
            }
            //using wServer.networking.packets.outgoing;
      
        }

        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
        }
    }
}
