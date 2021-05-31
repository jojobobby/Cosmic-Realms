using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class MYKatana : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.ZemithFull;
        private int _canTpCooldownTime2;

        public void OnEquip(Player player) {

            _canTpCooldownTime2 = 5 * 1000;
        }
        public void OnHit(Player player, int dmg) {


          
        }
        public void OnTick(Player player, RealmTime time) {

            if (_canTpCooldownTime2 == 0)
            {
                if (player.HP < (player.Stats[0] / 1.25))
                {
                    player.Client.SendPacket(new Notification()
                    {
                        ObjectId = player.Id,
                        Color = new ARGB(0xFF0000),
                        Message = "{\"key\": \"Eternal Rage [+100 HP]\"}"
                    }, PacketPriority.Low);
                    player.HP += 100;
                    _canTpCooldownTime2 = 5 * 1000;
                }
            }
            if (_canTpCooldownTime2 > 0)
            {
                _canTpCooldownTime2 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime2 < 0)
                    _canTpCooldownTime2 = 0;
            }

        }
        public void Unequip(Player player) { }
    }
}
