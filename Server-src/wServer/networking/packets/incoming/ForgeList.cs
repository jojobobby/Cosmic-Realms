using common;

namespace wServer.networking.packets.incoming
{
    public class ForgeList : IncomingMessage
    {
        public override PacketId ID => PacketId.FORGE_LIST;

        public int Category { get; set; }

        public override Packet CreateInstance() => new ForgeList();

        protected override void Read(NReader rdr)
        {
            Category = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
