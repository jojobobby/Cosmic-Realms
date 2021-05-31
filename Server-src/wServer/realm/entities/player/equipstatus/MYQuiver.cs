using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class MYQuiver : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.MYQuiver;

        public void OnEquip(Player player) { }
        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
        public void Unequip(Player player) { }
    }
}
