using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class Primaeval : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Primaeval"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
