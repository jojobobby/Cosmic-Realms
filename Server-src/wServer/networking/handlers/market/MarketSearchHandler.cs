using System;
using System.Collections.Generic;
using System.Linq;
using common;
using wServer.networking.packets;
using wServer.networking.packets.incoming.market;
using wServer.networking.packets.outgoing.market;

namespace wServer.networking.handlers.market
{
    public enum SortByType : int
    {
        All,
        Weapon,
        Ability,
        Armor,
        Ring,
        Misc
    }
    public enum SortByRarity : int
    {
        All,
        Tier,
        UT, 
        LG,
        MY
    }

    class MarketSearchHandler : PacketHandlerBase<MarketSearch>
    {
        private Dictionary<SortByType, int[]> _sortByTypes = new Dictionary<SortByType, int[]>()
        {
            { SortByType.Weapon, new int[] { 1, 3, 17, 8, 2, 24 } },
            { SortByType.Ability, new int[] { 13, 15, 11, 4, 16, 5, 12, 18, 19, 20, 21, 22, 23, 25 } },
            { SortByType.Armor, new int[] { 6, 14, 7, 6 } },
            { SortByType.Ring, new int[] { 9 } }
        };

        private List<DbMarketData> GetSortedOffers(Client client, SortByType type, SortByRarity rarity)
        {
            var sortedByTypeOffers = new List<DbMarketData>();
            var sortedOffers = new List<DbMarketData>();
            var allOffers = DbMarketData.GetAllMarketData(client.Manager.Database.Conn);

            switch(type)
            {
                case SortByType.All: sortedByTypeOffers = allOffers.ToList(); break;
                case SortByType.Misc:
                    {
                        foreach (var i in allOffers)
                        {
                            var isValid = true;
                            foreach (var t in _sortByTypes.Values)
                                if (t.Contains(i.SlotType))
                                {
                                    isValid = false;
                                    break;
                                }

                            if (isValid)
                                sortedByTypeOffers.Add(i);
                        }
                    } break;
                default:
                    {
                        foreach (var i in allOffers)
                            if (_sortByTypes[type].Contains(i.SlotType))
                                sortedByTypeOffers.Add(i);
                    } break;
            }

            foreach(var offer in sortedByTypeOffers)
            {
                var item = client.Manager.Resources.GameData.Items[offer.ItemType];

                switch(rarity)
                {
                    case SortByRarity.All: sortedOffers = sortedByTypeOffers; break;
                    case SortByRarity.Tier: { if (item.Tier != -1) sortedOffers.Add(offer); } break;
                    case SortByRarity.UT: { if (item.BagType == 8) sortedOffers.Add(offer); } break;
                    case SortByRarity.LG: { if (item.LG || item.MLG || item.MY) sortedOffers.Add(offer); } break;
                    case SortByRarity.MY: { if (item.Mythical) sortedOffers.Add(offer); } break;
                }
            }

            return sortedOffers;
        }


        public override PacketId ID => PacketId.MARKET_SEARCH;

        protected override void HandlePacket(Client client, MarketSearch packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
            {
                var player = client.Player;
                if (player == null || IsTest(client))
                    return;
                
                var result = new List<MarketData>();

                Console.WriteLine($"SORT:{packet.SortByType} {packet.SortByRarity}");

                var query = GetSortedOffers(client, (SortByType)packet.SortByType, (SortByRarity)packet.SortByRarity);

                foreach (var q in query)
                {
                    if (q.SellerId == client.Account.AccountId)
                        continue;

                    result.Add(new MarketData()
                    {
                        Id = q.Id,
                        ItemType = q.ItemType,
                        SellerName = q.SellerName,
                        SellerId = q.SellerId,
                        Price = q.Price,
                        Currency = (int)q.Currency
                    });
                }

                Console.WriteLine(result.Count);

                client.SendPacket(new MarketSearchResult
                {
                    Results = result.ToArray(), 
                    Description = string.Empty
                });
            });
        }
    }
}
