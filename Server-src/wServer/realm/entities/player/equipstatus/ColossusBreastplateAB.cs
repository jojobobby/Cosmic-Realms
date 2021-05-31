using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class ColossusBreastplateAB : IEquipStatus
    {
        private bool yes;
        public EquippedStatus Status => EquippedStatus.ColossusBreastplateAB;
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
                    Color = new ARGB(0x581845),
                    Message = "Void's Glory (4s)"
                }, PacketPriority.Low);
                yes = true;
                player.Owner.Timers.Add(new WorldTimer(6500, (world, t) =>
                {
                    yes = false;
                }));
                player.ApplyConditionEffect(ConditionEffectIndex.Strength, 4500);
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
