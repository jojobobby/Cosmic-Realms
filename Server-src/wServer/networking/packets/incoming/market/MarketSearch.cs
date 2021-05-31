using common;

namespace wServer.networking.packets.incoming.market
{
    public class MarketSearch : IncomingMessage
    {
        public override Packet CreateInstance() => new MarketSearch();

        public override PacketId ID => PacketId.MARKET_SEARCH;
        
        public int SortByType { get; set; }
        public int SortByRarity { get; set; }

        protected override void Read(NReader rdr)
        {
            SortByType = rdr.ReadInt32();
            SortByRarity = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
        }
    }
}
