using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class WrathFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.WrathFull;

        public void OnEquip(Player player)
        {
            player.ApplyConditionEffect(ConditionEffectIndex.StunImmune);
            player.Stats.Boost.ActivateBoost[7].Push(15, true);
            player.Stats.ReCalculateValues();
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }

        public void Unequip(Player player)
        {
            player.ApplyConditionEffect(ConditionEffectIndex.StunImmune, 0);
            player.Stats.Boost.ActivateBoost[7].Pop(15, true);
            player.Stats.ReCalculateValues();
        }
    }
}
