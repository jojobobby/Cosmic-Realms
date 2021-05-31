using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class VoidFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.VoidFull;

        public void OnEquip(Player player)
        {
            player.StatBoostsTF = true;
            player.Stats.Boost.ActivateBoost[2].Push(10, true);
            player.Stats.Boost.ActivateBoost[5].Push(10, true);
            player.Stats.ReCalculateValues();
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }

        public void Unequip(Player player)
        {
            player.StatBoostsTF = false;
            player.Stats.Boost.ActivateBoost[2].Pop(10, true);
            player.Stats.Boost.ActivateBoost[5].Pop(10, true);
            player.Stats.ReCalculateValues();
        }
    }
}
