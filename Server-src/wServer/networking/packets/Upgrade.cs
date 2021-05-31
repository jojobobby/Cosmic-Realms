using common;

namespace wServer.networking.packets.incoming
{
    public class Upgrade : IncomingMessage
    {
        public bool Lunar { get; set; }
        public bool Earth { get; set; }
        public bool Enhancer { get; set; }

        public override PacketId ID => PacketId.UPGRADE;
        public override Packet CreateInstance() { return new Upgrade(); }

        protected override void Read(NReader rdr)
        {
            Lunar = rdr.ReadBoolean();
            Earth = rdr.ReadBoolean();
            Enhancer = rdr.ReadBoolean();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Lunar);
            wtr.Write(Earth);
            wtr.Write(Enhancer);
        }
    }
}
