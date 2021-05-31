using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm;
using wServer.realm.entities;
using common.resources;

namespace wServer.networking.handlers
{
    class AoeAckHandler : PacketHandlerBase<AoeAck>
    {
        public override PacketId ID => PacketId.AOEACK;

        protected override void HandlePacket(Client client, AoeAck packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet.Damage, packet.ObjectName, packet.Effect, packet.Duration));
        }

        private void Handle(Player player, RealmTime time, int dmg, string objectName, ConditionEffectIndex effect, float duration)
        {
            if (player?.Owner == null)
                return;
            if (!player.HasConditionEffect(ConditionEffects.Invincible) && !player.HasConditionEffect(ConditionEffects.Stasis) && effect != 0)
                player.ApplyConditionEffect(effect, (int)duration);
            player?.GrenadeDamage(dmg, objectName);
        }
    }
}
