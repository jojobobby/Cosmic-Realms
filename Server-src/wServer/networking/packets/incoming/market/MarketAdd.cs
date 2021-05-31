using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketAdd : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketAdd();

        public override PacketId ID => PacketId.MARKET_ADD;

        public byte Slot;
        public int Price;

        protected override void Read(NReader rdr)
        {
            Slot = rdr.ReadByte();
            Price = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr) { }
    }
}
