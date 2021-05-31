using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class PermafrostLord : ISetPiece
    {
        public int Size { get { return 21; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Permafrost"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
