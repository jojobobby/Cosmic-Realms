using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class CybShieldFull : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.CybShieldFull;

        public void OnEquip(Player player)
        {
            //using wServer.networking.packets.outgoing;
            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0xFFFF00),
                Message = "{\"key\": \"Dual Shield\"}"
            }, PacketPriority.Low);


       
        }

        public void OnHit(Player player, int dmg) { }

        public void OnTick(Player player, RealmTime time) {
            player.ApplyConditionEffect(ConditionEffectIndex.Armored);

            player.Stats.Boost.ActivateBoost[0].Push(60, true);
            player.Stats.ReCalculateValues();
            player.Owner.Timers.Add(new WorldTimer(2000, (world, t) =>
            {
                player.Stats.Boost.ActivateBoost[0].Pop(60, true);
                player.Stats.ReCalculateValues();
            }));
        }

        public void Unequip(Player player)
        {
            player.Stats.Boost.ActivateBoost[0].Pop(60, true);
            player.Stats.ReCalculateValues();
            player.ApplyConditionEffect(ConditionEffectIndex.Armored, 0);
        }
    }
}
