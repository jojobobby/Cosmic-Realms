using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class AlienCores : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.AlienCores;

        int[] _stats;
        float _divider;
        public void OnEquip(Player player)
        {
            _stats = player.Client.Character.Stats;
            _divider = (player.HP > _stats[0] / 2) ? 0.05f : 0.1f;

            foreach (var s in _stats)
                player.SendInfo(s.ToString());

            player.Stats.Boost.ActivateBoost[2].Push((int)(_stats[2] * _divider));
            player.Stats.Boost.ActivateBoost[3].Push((int)(_stats[3] * _divider));
            player.Stats.Boost.ActivateBoost[4].Push((int)(_stats[4] * _divider));
            player.Stats.Boost.ActivateBoost[5].Push((int)(_stats[5] * _divider));
            player.Stats.Boost.ActivateBoost[6].Push((int)(_stats[6] * _divider));
            player.Stats.Boost.ActivateBoost[7].Push((int)(_stats[7] * _divider));
            player.Stats.Boost.ActivateBoost[11].Push((int)(_stats[11] * _divider));
            player.Stats.Boost.ActivateBoost[12].Push((int)(_stats[12] * _divider));

            player.Stats.ReCalculateValues();
        }

        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time)
        {
            float divider;
            if (player.HP >= player.Stats[0])
                divider = 0.2f;
            else
                divider = (player.HP > _stats[0] / 2) ? 0.1f : 0.05f;

            if (divider != _divider)
            {
                player.Stats.Boost.ActivateBoost[2].Pop((int)(_stats[2] * _divider));
                player.Stats.Boost.ActivateBoost[3].Pop((int)(_stats[3] * _divider));
                player.Stats.Boost.ActivateBoost[4].Pop((int)(_stats[4] * _divider));
                player.Stats.Boost.ActivateBoost[5].Pop((int)(_stats[5] * _divider));
                player.Stats.Boost.ActivateBoost[6].Pop((int)(_stats[6] * _divider));
                player.Stats.Boost.ActivateBoost[7].Pop((int)(_stats[7] * _divider));
                player.Stats.Boost.ActivateBoost[11].Pop((int)(_stats[11] * _divider));
                player.Stats.Boost.ActivateBoost[12].Pop((int)(_stats[12] * _divider));

                player.Stats.Boost.ActivateBoost[2].Push((int)(_stats[2] * divider));
                player.Stats.Boost.ActivateBoost[3].Push((int)(_stats[3] * divider));
                player.Stats.Boost.ActivateBoost[4].Push((int)(_stats[4] * divider));
                player.Stats.Boost.ActivateBoost[5].Push((int)(_stats[5] * divider));
                player.Stats.Boost.ActivateBoost[6].Push((int)(_stats[6] * divider));
                player.Stats.Boost.ActivateBoost[7].Push((int)(_stats[7] * divider));
                player.Stats.Boost.ActivateBoost[11].Push((int)(_stats[11] * divider));
                player.Stats.Boost.ActivateBoost[12].Push((int)(_stats[12] * divider));

                player.Stats.ReCalculateValues();
                _divider = divider;

            }
        }
        public void Unequip(Player player)
        {
            player.Stats.Boost.ActivateBoost[2].Pop((int)(_stats[2] * _divider));
            player.Stats.Boost.ActivateBoost[3].Pop((int)(_stats[3] * _divider));
            player.Stats.Boost.ActivateBoost[4].Pop((int)(_stats[4] * _divider));
            player.Stats.Boost.ActivateBoost[5].Pop((int)(_stats[5] * _divider));
            player.Stats.Boost.ActivateBoost[6].Pop((int)(_stats[6] * _divider));
            player.Stats.Boost.ActivateBoost[7].Pop((int)(_stats[7] * _divider));
            player.Stats.Boost.ActivateBoost[11].Pop((int)(_stats[11] * _divider));
            player.Stats.Boost.ActivateBoost[12].Pop((int)(_stats[12] * _divider));
            player.Stats.ReCalculateValues();
        }
    }
}
