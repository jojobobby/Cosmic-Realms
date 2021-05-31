using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace wServer.realm.entities.player.equipstatus
{
    public sealed class EquippedStatusManager
    {
        private Dictionary<EquippedStatus, Type> _statusCollection;

        public EquippedStatusManager()
        {
            _statusCollection = new Dictionary<EquippedStatus, Type>();

            foreach(var assembly in Assembly.GetAssembly(typeof(IEquipStatus)).GetTypes().Where(_ => 
                typeof(IEquipStatus).IsAssignableFrom(_) && !_.IsInterface))
            {
                var c = (IEquipStatus)Activator.CreateInstance(assembly);
                _statusCollection.Add(c.Status, assembly);
            }
        }

        public bool Contians(EquippedStatus status) => _statusCollection.ContainsKey(status);
        public IEquipStatus CreateInstance(EquippedStatus status) => (IEquipStatus)Activator.CreateInstance(_statusCollection[status]);

        public void Dispose()
        {
            _statusCollection.Clear();
            _statusCollection = null;
        }
    }
}
