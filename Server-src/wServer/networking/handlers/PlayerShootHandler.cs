using common.resources;
using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm;
using log4net;

namespace wServer.networking.handlers
{
    class PlayerShootHandler : PacketHandlerBase<PlayerShoot>
    {
        public override PacketId ID => PacketId.PLAYERSHOOT;
        private static readonly ILog CheatLog = LogManager.GetLogger("CheatLog");

        protected override void HandlePacket(Client client, PlayerShoot packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet, t));
            //Handle(client.Player, packet);
        }

        void Handle(Player player, PlayerShoot packet, RealmTime time)
        {
            if (player?.Owner == null) return;

            Item item;

            if (!player.Manager.Resources.GameData.Items.TryGetValue(packet.ContainerType, out item)) 
                return;

            // if not shooting main weapon do nothing (ability shoot is handled with useItem)
            if (player.Inventory[0] != item)
                return;

            if (!player.ValidatePlayerShoot(packet.Time, item))
            {
                player.Client.Disconnect("Attack speed modifaction detected!");
                return;
            }

            // create projectile and show other players
            var prjDesc = item.Projectiles[0]; //Assume only one
            Projectile prj = player.PlayerShootProjectile(
                packet.BulletId, prjDesc, item.ObjectType,
                packet.Time, packet.StartingPos, packet.Angle);
            player.Owner.EnterWorld(prj);
            if (!player.Manager.Resources.Settings.DisableAlly)
                player.Owner.BroadcastPacketNearby(new AllyShoot()
                {
                    OwnerId = player.Id,
                    Angle = packet.Angle,
                    ContainerType = packet.ContainerType,
                    BulletId = packet.BulletId
                }, player, player, PacketPriority.Low);
            player.FameCounter.Shoot(prj);
        }
    }
}