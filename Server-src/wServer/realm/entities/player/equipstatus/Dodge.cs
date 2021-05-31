using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class Dodge : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Dodge;

        public void OnEquip(Player player) { }

        public void OnHit(Player player, int dmg) {

            if (RandomUtil.RandInt(5, 100) <= 5 && !player.UltraInstinct)
            {
                player.UltraInstinct = true;

                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFFFFFF),
                    Message = "{\"key\": \"Ultra Instinct\"}"
                }, PacketPriority.Low);

                player.ApplyConditionEffect(new ConditionEffect { Effect = ConditionEffectIndex.Invincible, DurationMS = 5000 });

                player.Client.SendPacket(new SwitchMusic()
                {
                    Music = "https://github.com/GhostRealm/GhostRealm.github.io/raw/master/music/UltraInstinct.mp3",
                    isMusic = true
                });

                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    player.UltraInstinct = false;

                    player.Client.SendPacket(new SwitchMusic()
                    {
                        Music = player.Owner.Music,
                        isMusic = true
                    });
                }));
            }
        }

        public void OnTick(Player player, RealmTime time)
        {
          
        }

        public void Unequip(Player player) { }
    }
}
