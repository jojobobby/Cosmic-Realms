using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class MagicRobe : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.MagicRobe;

        public void OnEquip(Player player)
        {
            player.ApplyConditionEffect(ConditionEffectIndex.ParalyzeImmune, -1);
            player.ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, -1);
        }

        public void Unequip(Player player)
        {
            player.ApplyConditionEffect(ConditionEffectIndex.ParalyzeImmune, 0);
            player.ApplyConditionEffect(ConditionEffectIndex.SlowedImmune, 0);
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
    }
}
