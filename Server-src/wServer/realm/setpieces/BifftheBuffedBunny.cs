using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class BifftheBuffedBunny : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["BifftheBuffedBunny"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
