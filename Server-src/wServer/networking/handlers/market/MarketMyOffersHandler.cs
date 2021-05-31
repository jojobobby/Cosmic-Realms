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
    class MarketMyOffersHandler : PacketHandlerBase<MarketMyOffers>
    {
        public override PacketId ID => PacketId.MARKET_MY_OFFERS;

        protected override void HandlePacket(Client client, MarketMyOffers packet)
        {
            client.Manager.Logic.AddPendingAction(t => 
            {
                var player = client.Player;
                if (player == null || IsTest(client))
                {
                    return;
                }
                
                List<MarketData> myOffers = new List<MarketData>();
                for (var i = 0; i < client.Account.MarketOffers.Length; i++)
                {
                    DbMarketData result = player.Manager.Database.GetMarketData(client.Account.MarketOffers[i]);
                    if (result == null) /* This will only happend if someone bought our item */
                    {
                        continue;
                    }

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
