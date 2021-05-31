using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class KazeArmor : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.KazeArmor;

        public void OnEquip(Player player) { }
        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
        public void Unequip(Player player) { }
    }
}
