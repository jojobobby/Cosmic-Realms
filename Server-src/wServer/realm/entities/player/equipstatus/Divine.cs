using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class Divine : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Divine;

        public void OnEquip(Player player)
        {
            _canTpCooldownTime15 = 10 * 1000;
        }

        public void OnHit(Player player, int dmg) { }

        private int _canTpCooldownTime15;

        public void OnTick(Player player, RealmTime time)
        {
            if (_canTpCooldownTime15 > 0)
            {
                _canTpCooldownTime15 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime15 < 0)
                    _canTpCooldownTime15 = 0;
            }

            if (_canTpCooldownTime15 == 0)
            {
                _canTpCooldownTime15 = 10 * 1000; // 10 seconds
                player.Rn4g = RandomUtil.RandInt(1, 10);

                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xffff4d),
                    Message = "{\"key\": \"Divine Assistance\"}"
                }, PacketPriority.Low);
            }
        }

        public void Unequip(Player player) { }
    }
}
