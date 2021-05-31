using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class BloodVialAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.BloodVialAB;

        public void OnEquip(Player player) { }

        public void OnHit(Player player, int dmg)
        {

            var x = (dmg / 3);
            if (dmg > 100)
            {
                player.HP = player.HP + x;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFFCCCB),
                    Message = "" + x + " HP"
                }, PacketPriority.Low);
            }
        }

        public void OnTick(Player player, RealmTime time) { }
        public void Unequip(Player player) { }
    }
}
