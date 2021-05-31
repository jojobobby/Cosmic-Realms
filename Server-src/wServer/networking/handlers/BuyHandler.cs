using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities.vendors;

namespace wServer.networking.handlers
{
    class BuyHandler : PacketHandlerBase<Buy>
    {
        public override PacketId ID => PacketId.BUY;

        protected override void HandlePacket(Client client, Buy packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet.ObjectId, packet.Quantity));
            //Handle(client.Player, packet.ObjectId);
        }

        void Handle(Player player, int objId, int quantity)
        {
            if (player?.Owner == null)
                return;

            var obj = player.Owner.GetEntity(objId) as SellableObject;
            for (var i = 0; i < quantity; i++)
                obj?.Buy(player);
        }
    }
}
