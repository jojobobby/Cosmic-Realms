using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class CandyMan : ISetPiece
    {
        public int Size { get { return 40; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["CandyMan"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
