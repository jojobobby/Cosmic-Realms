using common.resources;
using System;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class LGAbility4 : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.LGAbility4;

        private int _canTpCooldownTime2;

        public void OnEquip(Player player)
        {
            _canTpCooldownTime2 = 10 * 1000;


        }

        public void OnHit(Player player, int dmg)
        {
        }

    

        public void OnTick(Player player, RealmTime time)
        {
            if (_canTpCooldownTime2 > 0)
            {
                _canTpCooldownTime2 -= time.ElaspedMsDelta;
                if (_canTpCooldownTime2 < 0)
                    _canTpCooldownTime2 = 0;
            }
            if (player.MP < 150 && _canTpCooldownTime2 == 0)
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x228B22),
                    Message = "{\"key\": \"Leaf's Assistance\"}"
                }, PacketPriority.Low);

                _canTpCooldownTime2 = 20 * 1000;
                player.MP += 100 + (2 * player.Stats[7]);
            }
        }

        public void Unequip(Player player)
        {
        }
    }
}
