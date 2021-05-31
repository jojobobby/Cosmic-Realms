using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class VargoRobe : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.VargoRobe;
        private int _canProcAbility;
        public void OnEquip(Player player)
        {
            _canProcAbility = 10 * 1000;
        }
        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time)
        {
            if (_canProcAbility > 0)
                _canProcAbility -= time.ElaspedMsDelta;

            if (player.freeAbilityUse == false && _canProcAbility <= 0)
            {
                player.freeAbilityUse = true;
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0x35546B),
                    Message = "{\"key\": \"Knowledge Rune\"}"
                }, PacketPriority.Low);
                _canProcAbility = 10 * 1000;
            }
        }
        public void Unequip(Player player)
        {
            player.freeAbilityUse = false;
        }
    }
}