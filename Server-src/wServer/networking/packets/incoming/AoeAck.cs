using common;
using common.resources;

namespace wServer.networking.packets.incoming
{
    public class AoeAck : IncomingMessage
    {
        public int Time { get; set; }
        public Position Position { get; set; }
        public int Damage { get; set; }
        public string ObjectName { get; set; }
        public ConditionEffectIndex Effect { get; set; }
        public float Duration { get; set; }

        public override PacketId ID => PacketId.AOEACK;
        public override Packet CreateInstance() { return new AoeAck(); }

        protected override void Read(NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(rdr);
            Damage = rdr.ReadInt32();
            ObjectName = rdr.ReadUTF();
            Effect = (ConditionEffectIndex)rdr.ReadByte();
            Duration = rdr.ReadSingle();
        }
        protected override void Write(NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
            wtr.Write(Damage);
            wtr.WriteUTF(ObjectName);
            wtr.Write((byte)Effect);
            wtr.Write(Duration);
        }
    }
}
