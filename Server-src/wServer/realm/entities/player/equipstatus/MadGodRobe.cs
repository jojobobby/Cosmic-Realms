using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class MadGodRobe : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.MadGodRobe;

        public void OnEquip(Player player)
        {
          
        }

        public void OnHit(Player player, int dmg)
        {
            if (RandomUtil.RandInt(1, 10) == 1)
            {
                player.Stats.Boost.ActivateBoost[2].Push(10, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[2].Pop(10, true);
                    player.Stats.ReCalculateValues();
                }));
            } 
        }

        public void OnTick(Player player, RealmTime time)
        {
        }

        public void Unequip(Player player)
        {
           
        }
    }
}
