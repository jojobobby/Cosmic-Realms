using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class LGAbility2 : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGAbility2;
        private int _canTpCooldownTime2;

        public void OnEquip(Player player)
        {
            _canTpCooldownTime2 = 10 * 1000;
        }

        public void OnHit(Player player, int dmg)
        {
            if (player.HP <= player.Stats[0] / 3 && _canTpCooldownTime2 == 0)
            {
                _canTpCooldownTime2 = 30 * 1000;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xFF0000),
                    Message = "{\"key\": \"Lava's Assistance\"}"
                }, PacketPriority.Low);
                player.ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 2000);
                player.ApplyConditionEffect(ConditionEffectIndex.Damaging, 2500);
                player.ApplyConditionEffect(ConditionEffectIndex.Haste, 2500);
            }
        }


        public void OnTick(Player player, RealmTime time)
        {
            if (_canTpCooldownTime2 > 0)
            {
                _canTpCooldownTime2 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime2 < 0)
                    _canTpCooldownTime2 = 0;
            }
           

        }

        public void Unequip(Player player)
        {
        }
    }
}
