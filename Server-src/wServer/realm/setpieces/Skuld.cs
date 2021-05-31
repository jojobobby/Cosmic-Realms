using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class Skuld : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Skuld"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
