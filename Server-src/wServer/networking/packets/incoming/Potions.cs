using common;

namespace wServer.networking.packets.incoming
{
    public class Potions : IncomingMessage
    {
        public int Type { get; set; }
        public bool Max { get; set; }

        public override PacketId ID => PacketId.POTIONS;
        public override Packet CreateInstance() { return new Potions(); }

        protected override void Read(NReader rdr)
        {
            Type = rdr.ReadInt32();
            Max = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Type);
            wtr.Write(Max);
        }
    }
}