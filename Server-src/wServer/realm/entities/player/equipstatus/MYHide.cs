using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class MYHide : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.MYHide;
        private int _canTpCooldownTime2;

        public void OnEquip(Player player)
        {
        }
        public void OnHit(Player player, int dmg) {

         
        }
        public void OnTick(Player player, RealmTime time) {

            if (player.HP <= player.Stats[0] * 0.4 && _canTpCooldownTime2 == 0)
            {
                _canTpCooldownTime2 = 15 * 1000;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x8A2BE2),
                    Message = "{\"key\": \"Silence\"}"
                }, PacketPriority.Low);
                player.ApplyConditionEffect(ConditionEffectIndex.Invisible, 5000);
                player.ApplyConditionEffect(ConditionEffectIndex.Invincible, 500);
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
