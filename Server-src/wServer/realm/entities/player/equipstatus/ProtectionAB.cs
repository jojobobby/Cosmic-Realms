using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class ProtectionAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Protection;

        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
            player.ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Barrier, DurationMS = 1000 });
        }

        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
            player.ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Barrier, DurationMS = 0 });
        }
    }
}
