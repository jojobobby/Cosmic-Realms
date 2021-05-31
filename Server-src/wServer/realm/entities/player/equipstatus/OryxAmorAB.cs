using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class OryxAmorAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.OryxArmorAB;

        public void OnEquip(Player player) => player.ApplyConditionEffect(ConditionEffectIndex.Healing, -1);
        public void Unequip(Player player) => player.ApplyConditionEffect(ConditionEffectIndex.Healing, 0);

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time) { }
    }
}
