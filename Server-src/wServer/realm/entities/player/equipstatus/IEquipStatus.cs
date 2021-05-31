using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public interface IEquipStatus
    {
        EquippedStatus Status { get; }

        void OnEquip(Player player);
        void Unequip(Player player);

        void OnTick(Player player, RealmTime time);
        void OnHit(Player player, int dmg);
    }
}
