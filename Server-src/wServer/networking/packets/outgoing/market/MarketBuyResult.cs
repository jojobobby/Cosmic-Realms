using common;

namespace wServer.networking.packets.outgoing.market
{
    public class MarketBuyResult : OutgoingMessage
    {
        public override Packet CreateInstance() => new MarketBuyResult();

        public override PacketId ID => PacketId.MARKET_BUY_RESULT;

        public const int NOT_VALID_ID = 0;
        public const int CANT_AFFORD = 1;
        public const int ITEM_DOESNT_EXIST = 2;
        public const int COULDNT_BUY_ITEM = 3;
        public const int MY_ITEM = 4;

        public int Code;
        public string Description;
        public int OfferId;

        protected override void Read(NReader rdr)
        {
            Code = rdr.ReadInt32();
            Description = rdr.ReadUTF();
            OfferId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(Code);
            wtr.WriteUTF(Description);
            wtr.Write(OfferId);
        }
    }
}
