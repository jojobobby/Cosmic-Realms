using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class WildShadow : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.WildShadow;
        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
        }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.MP > 200)
            {
                player.Stats.Boost.ActivateBoost[12].Push(25, true);
                player.Stats.Boost.ActivateBoost[4].Push(10, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[12].Pop(25, true);
                    player.Stats.Boost.ActivateBoost[4].Pop(10, true);
                    player.Stats.ReCalculateValues();
                }));
            }
        }

        public void Unequip(Player player)
        {
          
        }
    }
}
