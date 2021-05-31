using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class OryxItem : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.OryxItem;

        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
            if (RandomUtil.RandInt(1, 4) == 1)
                player.ApplyConditionEffect(ConditionEffectIndex.Bloodlust, 3000);
        }

        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
        }
    }
}
