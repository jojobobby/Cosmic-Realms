using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class ArmorAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.ArmorAB;

        public void OnEquip(Player player) { }

        public void OnHit(Player player, int dmg)
        {
            
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xccccff),
                    Message = "{\"key\": \"Magical Counter\"}"
                }, PacketPriority.Low);
                var entity = Entity.Resolve(player.Manager, 0x70aa);
                entity.Move(player.X, player.Y);
                entity.SetPlayerOwner(player);
                player.Owner.EnterWorld(entity);
            
        }

        public void OnTick(Player player, RealmTime time) { }
        public void Unequip(Player player) { }
    }
}
