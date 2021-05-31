using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class Ciritical : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Critical;

        public void OnEquip(Player player) =>
            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0x800080),
                Message = "Spirit Sword"
            }, PacketPriority.Low);

        public void Unequip(Player player) { }
        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
    }
}
