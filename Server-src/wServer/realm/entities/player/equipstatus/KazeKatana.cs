using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class KazeKatana : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.KazeKatana;

        public void OnEquip(Player player) { }
        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
        public void Unequip(Player player) { }
    }
}
