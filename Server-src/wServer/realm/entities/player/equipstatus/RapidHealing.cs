using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class RapidHealing : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.HealAB;

        public void OnEquip(Player player) { }
        public void Unequip(Player player) { }
        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.HP < 300 && time.TotalElapsedMs % 5 == 0)
                player.HP = player.HP + 5;
        }
    }
}
