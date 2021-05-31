using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class OryxFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.OryxFull;

        public void OnEquip(Player player)
        {
            player.Stats.Boost.ActivateBoost[3].Push(15, true);
            player.Stats.Boost.ActivateBoost[6].Push(20, true);
            player.Stats.ReCalculateValues();
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }

        public void Unequip(Player player)
        {
            player.Stats.Boost.ActivateBoost[3].Pop(15, true);
            player.Stats.Boost.ActivateBoost[6].Pop(20, true);
            player.Stats.ReCalculateValues();
        }
    }
}
