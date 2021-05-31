using common;

namespace wServer.networking.packets.outgoing
{
    public class AchievementUnlocked : OutgoingMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int IconId { get; set; }

        public override PacketId ID => PacketId.ACHIEVEMENT_UNLOCKED;
        public override Packet CreateInstance() { return new AchievementUnlocked(); }

        protected override void Read(NReader rdr)
        {
            Title = rdr.ReadUTF();
            Description = rdr.ReadUTF();
            IconId = rdr.ReadInt32();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Title);
            wtr.WriteUTF(Description);
            wtr.Write(IconId);
        }
    }
}
