using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class VargoFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.VargoFull;

        public void OnEquip(Player player)
        {
            player.Stats.Boost.ActivateBoost[1].Push(150, true);
            player.Stats.Boost.ActivateBoost[7].Push(30, true);
            player.Stats.ReCalculateValues();
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }

        public void Unequip(Player player)
        {
            player.Stats.Boost.ActivateBoost[1].Pop(150, true);
            player.Stats.Boost.ActivateBoost[7].Pop(30, true);
            player.Stats.ReCalculateValues();
        }
    }
}
