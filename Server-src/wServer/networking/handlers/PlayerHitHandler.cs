using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm;
using wServer.realm.entities;
using common.resources;

namespace wServer.networking.handlers
{
    class PlayerHitHandler : PacketHandlerBase<PlayerHit>
    {
        public override PacketId ID => PacketId.PLAYERHIT;

        protected override void HandlePacket(Client client, PlayerHit packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet.ObjectId, packet.BulletId));
        }

        private void Handle(Player player, RealmTime time, int objectId, byte bulletId)
        {
            if (player?.Owner == null)
                return;

            var entity = player.Owner.GetEntity(objectId);
            var prj = entity != null ?
                ((IProjectileOwner) entity).Projectiles[bulletId] :
                player.Owner.Projectiles
                    .Where(p => p.Value.ProjectileOwner.Self.Id == objectId)
                    .SingleOrDefault(p => p.Value.ProjectileId == bulletId).Value;
            if (prj != null)
            {
                foreach (ConditionEffect effect in prj.ProjDesc.Effects)
                {
                    player?.ApplyConditionEffect(effect);
                }
            }
            prj?.ForceHit(player, time);
        }
    }
}
