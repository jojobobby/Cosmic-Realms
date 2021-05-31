using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    class ForgeListHandler : PacketHandlerBase<ForgeList>
    {
        private static int[] WEAPON_TYPE = new int[] { 1, 3, 17, 8, 2, 24 };
        private static int[] ABILITY_TYPE = new int[] { 13, 15, 11, 4, 16, 5, 12, 18, 19, 20, 21, 22, 23, 25 };
        private static int[] ARMOR_TYPE = new int[] { 6, 14, 7, 6 };
        private static int[] RING_TYPE = new int[] { 9 };
        public override PacketId ID => PacketId.FORGE_LIST;

        protected override void HandlePacket(Client client, ForgeList packet)
        {
            var recipes = client.Manager.Recipes;
            var gameData = client.Manager.Resources.GameData;

            client.SendPackets(recipes.Where(_ => correctCategory(packet.Category, _.Value.SlotType)).Select(
                _ =>
                {
                    var key = _.Key.Select(x =>
                    {
                        var item = gameData.Items[gameData.IdToObjectType[x]];

                        return item.ObjectId;
                    }).ToList();

                    return new ForgeListResult()
                    {
                        Result = _.Value.ObjectId,
                        Recipes = key
                    };
                }));
        }

        private bool correctCategory(int category, int slotType)
        {
            switch(category)
            {
                case 0: return WEAPON_TYPE.Contains(slotType);
                case 1: return ABILITY_TYPE.Contains(slotType);
                case 2: return ARMOR_TYPE.Contains(slotType);
                case 3: return RING_TYPE.Contains(slotType);
                case 4: return true;
            }
            return false;
        }
    }
}
