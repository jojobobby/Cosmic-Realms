using wServer.realm;
using wServer.networking.packets.outgoing;

namespace wServer.logic.behaviors
{
    public class Sound : Behavior
    {
        private readonly int soundId;

        public Sound(int soundId = 0)
        {
            this.soundId = soundId;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            host.Owner.BroadcastPacketNearby(new PlaySound()
            {
                OwnerId = host.Id,
                SoundId = soundId
            }, host, null, PacketPriority.Low);
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}