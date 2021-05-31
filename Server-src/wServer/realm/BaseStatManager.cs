using common;
using common.resources;

namespace wServer.realm
{
    class BaseStatManager
    {
        private readonly StatsManager _parent;
        private readonly int[] _base;

        public BaseStatManager(StatsManager parent)
        {
            _parent = parent;
            _base = Utils.ResizeArray(
                parent.Owner.Client.Character.Stats, 
                StatsManager.NumStatTypes);
            
            ReCalculateValues();
        }

        public int[] GetStats()
        {
            return (int[]) _base.Clone();
        }

        public int this[int index]
        {
            get { return _base[index]; }
            set
            {
                _base[index] = value;
                _parent.StatChanged(index);
            }
        }

        protected internal void ReCalculateValues(InventoryChangedEventArgs e = null)
        {
            SetWeaponDamage(e);
        }

        private void SetWeaponDamage(InventoryChangedEventArgs e)
        {
            if (_parent.Owner == null)
                return;

            ProjectileDesc[] proj = null;
            var inv = _parent.Owner.Inventory;
            var min = 0;
            var max = 0;

            if (inv != null && inv.HasItems && inv[0] != null)
                proj = inv[0].Projectiles;

            if (proj != null && proj.Length > 0)
            {
                min = proj[0].MinDamage;
                max = proj[0].MaxDamage;
            }

            _base[8] = min;
            _base[9] = max;
        }
    }
}
