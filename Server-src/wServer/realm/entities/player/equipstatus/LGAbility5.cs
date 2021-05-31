using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class LGAbility5 : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGAbility5;

        public void OnEquip(Player player)
        {
            _canTpCooldownTime2 = 10 * 1000;
        }

        public void OnHit(Player player, int dmg) { }

        private int _canTpCooldownTime2;

        public void OnTick(Player player, RealmTime time)
        {
            if (_canTpCooldownTime2 > 0)
            {
                _canTpCooldownTime2 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime2 < 0)
                    _canTpCooldownTime2 = 0;
            }
            var x = player.Stats[0] / 2;
            if (player.HP <= x && _canTpCooldownTime2 == 0)
            {
                _canTpCooldownTime2 = 20 * 1000; // 20 seconds

                player.HP += player.Stats[6];

                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x000000),
                    Message = "{\"key\": \"Front Line\"}"
                }, PacketPriority.Low);
            }

            if (player.HP >= 600) 
                player.ApplyConditionEffect(ConditionEffectIndex.Armored, 1000);
            if (player.HP <= 300)
                player.ApplyConditionEffect(ConditionEffectIndex.Healing, 1000);
        }

        public void Unequip(Player player)
        {

        }
    }
}
