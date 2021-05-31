using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class OmnipotenceAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.OmnipotenceAB;

        public void OnEquip(Player player)
        {
        }

        public void OnHit(Player player, int dmg)
        {
            if (RandomUtil.RandInt(1, 5) == 1)
            {
                player.Stats.Boost.ActivateBoost[0].Push(120, true);
                player.Stats.Boost.ActivateBoost[1].Push(120, true);
                player.Stats.Boost.ActivateBoost[2].Push(8, true);
                player.Stats.Boost.ActivateBoost[3].Push(8, true);
                player.Stats.Boost.ActivateBoost[4].Push(8, true);
                player.Stats.Boost.ActivateBoost[5].Push(8, true);
                player.Stats.Boost.ActivateBoost[6].Push(8, true);
                player.Stats.Boost.ActivateBoost[7].Push(8, true);
                player.Stats.Boost.ActivateBoost[11].Push(15, true);
                player.Stats.Boost.ActivateBoost[12].Push(5, true);
                player.Stats.ReCalculateValues();

                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[0].Pop(120, true);
                    player.Stats.Boost.ActivateBoost[1].Pop(120, true);
                    player.Stats.Boost.ActivateBoost[2].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[3].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[4].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[5].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[6].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[7].Pop(8, true);
                    player.Stats.Boost.ActivateBoost[11].Pop(15, true);
                    player.Stats.Boost.ActivateBoost[12].Pop(5, true);
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
