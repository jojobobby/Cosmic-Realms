using common.resources;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities.player.equipstatus
{
    class SilentRobe : IEquipStatus
    {

        private bool Boosted;
        public EquippedStatus Status => EquippedStatus.SilentRobe;

        public void OnEquip(Player player)
        {
            Boosted = false;
        }

        public void OnHit(Player player, int dmg) { }
        public void OnTick(Player player, RealmTime time)
        {

            if (player.HasConditionEffect(ConditionEffects.Bleeding))
            {
                Boosted = true;

                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Bleeding,
                    DurationMS = 0
                });
                player.Client.SendPacket(new Notification()
                {
                    ObjectId = player.Id,
                    Color = new ARGB(0xCC1100),
                    Message = "{\"key\": \"Blood Pact\"}"
                }, PacketPriority.Low);
            }
            if (Boosted)
            {

                player.Stats.Boost.ActivateBoost[5].Push(5, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(5000, (world, t) =>
                {
                    Boosted = false;
                }));
            }
            else
            {
                player.Stats.Boost.ActivateBoost[5].Pop(5, true);
                player.Stats.ReCalculateValues();
            }

        }
        public void Unequip(Player player)
        {
            Boosted = false;
            player.Stats.Boost.ActivateBoost[5].Pop(5, true);
            player.Stats.ReCalculateValues();
        }
    }
}
