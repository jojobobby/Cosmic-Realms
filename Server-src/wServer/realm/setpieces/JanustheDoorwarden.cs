using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class JanustheDoorwarden : ISetPiece
    {
        public int Size { get { return 23; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["JanustheDoorwarden"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
