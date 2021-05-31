using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class Magic : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.MagicAB;

        public void OnEquip(Player player) { }
        public void Unequip(Player player) { }
        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.MP < 300)
                player.MP = player.MP + 5;
        }
    }
}
