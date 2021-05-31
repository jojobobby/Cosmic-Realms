using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class CybShieldAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CybShieldAB;

        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
        }

        private bool _armored;
        private bool _solid;
        private bool _barrier;

        public void OnTick(Player player, RealmTime time)
        {
            if (player.HasConditionEffect(ConditionEffects.Armored))
            {
                player.Stats.Boost.ActivateBoost[12].Push(10, true);
                player.Stats.ReCalculateValues();
            }//lalatheislandgal
            if (player.HasConditionEffect(ConditionEffects.Solid))
            {
                player.Stats.Boost.ActivateBoost[12].Push(10, true);
                player.Stats.ReCalculateValues();
            } 
            if (player.HasConditionEffect(ConditionEffects.Barrier))
            {
                player.Stats.Boost.ActivateBoost[12].Push(10, true);
                player.Stats.ReCalculateValues();
            } 

            player.Owner.Timers.Add(new WorldTimer(8000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[12].Pop(10, true);
                player.Stats.ReCalculateValues();
                player.Stats.Boost.ActivateBoost[12].Pop(10, true);
                player.Stats.ReCalculateValues();
                player.Stats.Boost.ActivateBoost[12].Pop(10, true);
                player.Stats.ReCalculateValues();

            }));
        }

        public void Unequip(Player player)
        {

          
        }
    }
}
