using System;
using common.resources;
using wServer.networking.packets;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing.market;

namespace wServer.networking.handlers.market
{
    class MarketAddHandler : PacketHandlerBase<MarketAdd>
    {
        public override PacketId ID => PacketId.MARKET_ADD;

        const int MAX_OFFERS = 50;
        const int MAX_PRICE = 500000;
        
        protected override void HandlePacket(Client client, MarketAdd packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                Console.WriteLine("MARKET ADD");

                var player = client.Player;
                if (player == null || IsTest(client))
                    return;

                if (client.Player.Rank >= 60)
                {
                    client.SendPacket(new MarketBuyResult
                    {
                        Code = MarketBuyResult.COULDNT_BUY_ITEM,
                        Description = "Invalid rank!"
                    });
                    return;
                }

                if (client.Account.MarketOffers.Length> MAX_OFFERS ||
                    client.Account.MarketOffers.Length + 1 > MAX_OFFERS) //ADJUST TO RANK
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.INVALID_UPTIME,
                        Description = "Max offers Reached."
                    });
                    return;
                }

                if (packet.Price > MAX_PRICE)
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.INVALID_PRICE,
                        Description = "Please offer a reasonable price."
                    });
                    return;
                }

                if (packet.Price <= 0) /* Client has this check, but check it incase it was modified */
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.INVALID_PRICE,
                        Description = "You cannot sell items for 0 or less."
                    });
                    return;
                }

                byte slotId = packet.Slot;

                if (player.Inventory[slotId] == null) /* Make sure they are selling valid items */
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.SLOT_IS_NULL,
                        Description = $"The slot {slotId} is empty or invalid."
                    });
                    return;
                }

                Item item = player.Inventory[slotId];
                if (Banned(item)) /* Client has this check, but check it incase it was modified */
                {
                    client.SendPacket(new MarketAddResult
                    {
                        Code = MarketAddResult.ITEM_IS_SOULBOUND,
                        Description = "You cannot sell non tradeable items."
                    });
                    return;
                }

                var slotType = player.Inventory[slotId].SlotType;
                player.Inventory[slotId] = null; /* Set the slot to null */
                player.Manager.Database.AddMarketData(client.Account, item.ObjectType, player.AccountId, player.Name, packet.Price, slotType);

                client.SendPacket(new MarketAddResult
                {
                    Code = -1,
                    Description = $"Successfully added {item.DisplayName} items to the market."
                });
            });
        }

        private static bool Banned(Item item) /* What you add here you must add client sided too */
        {
            return item.Soulbound;
        }
    }
}
