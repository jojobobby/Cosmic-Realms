using System;
using wServer.realm;
using wServer.realm.setpieces;

namespace wServer.logic.behaviors
{
    public class ApplySetpiece : Behavior
    {
        private readonly string name;

        public ApplySetpiece(string name)
        {
            this.name = name;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            try
            {
                ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType("wServer.realm.setpieces." + name, true, true));
                piece.RenderSetPiece(host.Owner, new IntPoint((int)host.X, (int)host.Y));
            }
            catch { }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }

    public class AddSetpiece : Behavior
    {
        private readonly string name;

        public AddSetpiece(string name)
        {
            this.name = name;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            var proto = host.Owner.Manager.Resources.Worlds[name];
            SetPieces.RenderFromProto(host.Owner, new IntPoint((int)host.X, (int)host.Y), proto);
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}
