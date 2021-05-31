using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class AwokenAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Awoken;
        private bool yes;
        public void OnEquip(Player player) {
            yes = false;
        }

        public void OnHit(Player player, int dmg)
        {
            if (yes == false)
            {
                if (RandomUtil.RandInt(1, 4) == 1)
                    player.ApplyConditionEffect(ConditionEffectIndex.Awoken, 3000);
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xc16262),
                    Message = "{\"key\": \"Awoken\"}"
                }, PacketPriority.Low);
                yes = true;
                player.Owner.Timers.Add(new WorldTimer(3000, (world, t) =>
                {
                    yes = false;
                }));
            }


        }
        public void OnTick(Player player, RealmTime time) {



        }

        public void Unequip(Player player)
        {
        }
    }
}
