using System;
using System.Collections.Generic;
using System.Linq;
using common;
using common.resources;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing;
using wServer.networking.packets.outgoing.market;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.networking.handlers.market
{
    class MarketRemoveHandler : PacketHandlerBase<MarketRemove>
    {
        public override PacketId ID => PacketId.MARKET_REMOVE;

        protected override void HandlePacket(Client client, MarketRemove packet)
        {
            client.Manager.Logic.AddPendingAction(t => 
            {
                var player = client.Player;
                if (player == null || IsTest(client))
                {
                    return;
                }


                DbMarketData data = player.Manager.Database.GetMarketData(packet.Id);
                if (data == null) /* Incase the item was removed by MarketSweeper or someone bought it */
                {
                    client.SendPacket(new MarketRemoveResult
                    {
                        Code = MarketRemoveResult.ITEM_DOESNT_EXIST,
                        Description = "This item was already bought or removed."
                    });
                    return;
                }

                if (data.SellerId != player.AccountId) /* Incase someone tries to remove someone else' item */
                {
                    client.SendPacket(new MarketRemoveResult
                    {
                        Code = MarketRemoveResult.NOT_YOUR_ITEM,
                        Description = "You cannot remove an item that isnt yours."
                    });
                    return;
                }

                /* Remove it from the market */
                player.Manager.Database.RemoveMarketData(client.Account, data.Id);
                player.Manager.Database.AddGift(client.Account, data.ItemType);

                List<MarketData> myOffers = new List<MarketData>();
                for (var i = 0; i < client.Account.MarketOffers.Length; i++)
                {
                    DbMarketData result = player.Manager.Database.GetMarketData(client.Account.MarketOffers[i]);
                    myOffers.Add(new MarketData
                    {
                        Id = result.Id,
                        ItemType = result.ItemType,
                        SellerName = result.SellerName,
                        SellerId = result.SellerId,
                        Currency = (int)result.Currency,
                        Price = result.Price
                    });
                }

                client.SendPacket(new MarketMyOffersResult
                {
                    Results = myOffers.ToArray()
                });
            });
        }
    }
}
