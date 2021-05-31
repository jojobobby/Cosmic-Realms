using common;

namespace wServer.networking.packets.incoming
{
    public class ForgeItem : IncomingMessage
    {
        public string Name { get; set; }
        public byte TargetSlotId { get; set; }
        public byte InputItemSlotId { get; set; }
        public byte[] Modifiers { get; set; }
        public bool isPresent { get; set; }
        public string newDescription { get; set; }

        public override PacketId ID => PacketId.FORGE_ITEM;
        public override Packet CreateInstance() { return new ForgeItem(); }

        protected override void Read(NReader rdr)
        {
            Name = rdr.ReadUTF();
            TargetSlotId = rdr.ReadByte();
            InputItemSlotId = rdr.ReadByte();
            Modifiers = new byte[rdr.ReadByte()];
            for (int i = 0; i < Modifiers.Length; i++)
                Modifiers[i] = rdr.ReadByte();
            isPresent = rdr.ReadBoolean();
            newDescription = rdr.ReadUTF();
        }
        protected override void Write(NWriter wtr)
        {
            //its read-only if im right?
        }
    }
}