using common.resources;

namespace wServer.realm.entities.player.equipstatus
{
    class RadiantHeart : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.radiantheart;

        public void OnEquip(Player player)
        {
            _hasCond = false;
            _time = -1;
            _condStart = -1;
        }

        private bool _hasCond;
        private long _condStart;
        private long _time;
        public void OnHit(Player player, int dmg)
        {
            if (RandomUtil.RandInt(1, 5) == 1)
            {
                player.Stats.Boost.ActivateBoost[0].Push(100, true);
                player.Stats.Boost.ActivateBoost[2].Push(5, true);
                player.Stats.ReCalculateValues();
                _hasCond = true;
                _condStart = _time;
            } else if (_time - _condStart > 5000 && _hasCond)
            {
                player.Stats.Boost.ActivateBoost[0].Pop(100, true);
                player.Stats.Boost.ActivateBoost[2].Pop(5, true);
                player.Stats.ReCalculateValues();
                _hasCond = false;
            }
        }

        public void OnTick(Player player, RealmTime time)
        {
            _time = time.TotalElapsedMs;
        }

        public void Unequip(Player player)
        {
            if (_hasCond)
            {
                player.Stats.Boost.ActivateBoost[0].Pop(100, true);
                player.Stats.Boost.ActivateBoost[2].Pop(5, true);
                player.Stats.ReCalculateValues();
            }
        }
    }
}
