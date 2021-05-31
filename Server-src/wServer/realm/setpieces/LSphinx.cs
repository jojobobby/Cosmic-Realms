using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class LCubeGod : ISetPiece
    {
        public int Size { get { return 64; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["LCubeGod"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
