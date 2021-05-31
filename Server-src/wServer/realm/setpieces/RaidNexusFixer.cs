using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class RaidNexusFixer : ISetPiece
    {
        public int Size { get { return 19; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["RaidNexusFixer"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
