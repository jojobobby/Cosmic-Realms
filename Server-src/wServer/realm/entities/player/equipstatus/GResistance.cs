using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    public class GResistance : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.GResistance;

        public void OnEquip(Player player)
        {
           

        }

        public void Unequip(Player player)
        {
        
        }

        public void OnHit(Player player, int dmg) {
           
        }
        public void OnTick(Player player, RealmTime time) {
            if (player.HP >= player.Stats[0] / 2)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.ArmorBreakImmune, 1000);
            }
        }
    }
}
