using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class MEternal : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["MEternal"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
