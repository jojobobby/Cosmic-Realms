using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class CultistEvent : ISetPiece
    {
        public int Size { get { return 64; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["CultistEvent"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
