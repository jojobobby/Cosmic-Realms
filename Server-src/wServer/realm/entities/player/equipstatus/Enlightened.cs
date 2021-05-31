using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class Enlightened : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.Enlightened;

        public void OnEquip(Player player)
        {
            _canTpCooldownTime14 = 20 * 1000;
        }

        public void OnHit(Player player, int dmg) { }

        private int _canTpCooldownTime14;

        public void OnTick(Player player, RealmTime time)
        {
            if (_canTpCooldownTime14 > 0)
            {
                _canTpCooldownTime14 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime14 < 0)
                    _canTpCooldownTime14 = 0;
            }
            var amount = player.Stats[0] / 4;


            if (_canTpCooldownTime14 == 0 && player.HP <= amount && player.MP >= 150)
            {
                _canTpCooldownTime14 = 20 * 1000; // 10 seconds
                player.HP += amount;

                if (player.MP - 150 > 0)
                    player.MP -= 150;

                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xffff4d),
                    Message = "{\"key\": \"Enlightened\"}"
                }, PacketPriority.Low);
            }
        }

        public void Unequip(Player player) { }
    }
}
