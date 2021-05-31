using common;


namespace wServer.networking.packets.outgoing
{
    public class SwordDmg : OutgoingMessage
    {
        public float DmgMultiplied { get; set; }
        public bool ActivatedSword { get; set; }

        public override PacketId ID => PacketId.CRITICALDAMAGE;

        public override Packet CreateInstance()
        {
            return new SwordDmg();
        }

        protected override void Read(NReader rdr)
        {
            ActivatedSword = rdr.ReadBoolean();
            DmgMultiplied = rdr.ReadSingle();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.Write(ActivatedSword);
            wtr.Write(DmgMultiplied);
        }
    }
}
