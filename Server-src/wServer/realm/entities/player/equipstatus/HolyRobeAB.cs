using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    public class HolyRobeAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.HolyRobeAB;

        public void OnEquip(Player player)
        {
            //using wServer.networking.packets.outgoing;
           
        }

        public void OnHit(Player player, int dmg)
        {
            if (player.HP >= player.Stats[0] / 3)
            {
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xACBE25),
                    Message = "{\"key\": \"Holy Touch\"}"
                }, PacketPriority.Low);
            }

        }

        public void OnTick(Player player, RealmTime time)
        {
            if (player.Stats[0] >= player.Stats[0] / 3)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Sick, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Bleeding, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Weak, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Paralyzed, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Stunned, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Slowed, 0);
            }
        }

        public void Unequip(Player player)
        {
        }
    }
}
