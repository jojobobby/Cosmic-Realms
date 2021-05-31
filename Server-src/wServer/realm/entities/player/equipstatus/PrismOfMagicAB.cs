using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class PrismOfMagicAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.PrismOfMagicAB;

        private bool _prism;

        public void OnEquip(Player player)
        {
            _prism = false;
        }

        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            var wisdom = player.Stats[7] * 3;
            var Magic = player.Stats.Boost[1] / 5;

            player.Stats.Boost.ActivateBoost[0].Push(Magic, true);
            player.Stats.ReCalculateValues();
            player.Stats.Boost.ActivateBoost[1].Push(wisdom, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[0].Pop(Magic, true);
                player.Stats.ReCalculateValues();
                player.Stats.Boost.ActivateBoost[1].Pop(wisdom, true);
                player.Stats.ReCalculateValues();
            }));






        }

        public void Unequip(Player player)
        {
           
        }
    }
}
