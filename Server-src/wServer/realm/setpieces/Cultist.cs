using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class Cultist : ISetPiece
    {
        public int Size { get { return 32; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Cultist"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
