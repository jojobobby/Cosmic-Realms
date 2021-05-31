using common.resources;

using wServer.networking.packets.outgoing;


namespace wServer.realm.entities.player.equipstatus
{
    class CloackAB : IEquipStatus
    {
        public EquippedStatus Status => EquippedStatus.cloakabc;

        public void OnEquip(Player player)
        {
            player.Client.SendPacket(new Notification()
            {
                ObjectId = player.Id,
                Color = new ARGB(0x808000),
                Message = "Assassin"
            }, PacketPriority.Low);
        }

        public void OnHit(Player player, int dmg)
        {
        }
        
        public void OnTick(Player player, RealmTime time)
        {
            if (player.HasConditionEffect(ConditionEffects.Invisible))
            {
                player.Stats.Boost.ActivateBoost[5].Push(12, true);
                player.Stats.ReCalculateValues();
                player.Owner.Timers.Add(new WorldTimer(1000, (world, t) =>
                {
                    player.Stats.Boost.ActivateBoost[5].Pop(12, true);
                    player.Stats.ReCalculateValues();
                }));
            }
        }

        public void Unequip(Player player)
        {
         
        }
    }
}
