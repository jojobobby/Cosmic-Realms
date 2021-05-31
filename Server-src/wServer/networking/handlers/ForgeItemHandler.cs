using wServer.realm;
using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using common.resources;
using System.Collections.Generic;
using wServer.realm.worlds.logic;
using StackExchange.Redis;
using System.Linq;
using System;

namespace wServer.networking.handlers
{
    class ForgeItemHandler : PacketHandlerBase<ForgeItem>
    {
        public override PacketId ID => PacketId.FORGE_ITEM;

        protected override void HandlePacket(Client client, ForgeItem packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, t, packet));
        }

        void Handle(Player player, RealmTime time, ForgeItem packet)
        {
            if (player?.Owner == null)
                return;

            if (packet.isPresent)
            {
                //creating list for slots
                var slots = new List<byte>();
                bool tryCraft = false;
                //adding craft shits from inv to list
                if (packet.InputItemSlotId < player.Inventory.Length && packet.InputItemSlotId >= 0)
                    if (player.Inventory[packet.InputItemSlotId] != null)
                    {
                        slots.Add(packet.InputItemSlotId);
                        tryCraft = true;
                    }
                if (packet.Modifiers.Length > 0)
                {
                    for (int i = 0; i < packet.Modifiers.Length; i++)
                        if (packet.Modifiers[i] < player.Inventory.Length && packet.Modifiers[i] >= 0)
                            if (player.Inventory[packet.Modifiers[i]] != null)
                            {
                                slots.Add(packet.Modifiers[i]);
                                tryCraft = true;
                            }
                }
                if (!tryCraft) return;
                //create list for items and remove duplicate slots
                var items = new List<Item>();
                var noDupeSlots = slots.Distinct();
                //add items from slots
                foreach (int slot in noDupeSlots)
                    if (player.Inventory[slot] != null)
                        items.Add(player.Inventory[slot]);
                Item result;
                if ((result = player.Manager.GetCraftResult(items)) != null)
                {
                    //fee prepaid
                    //var pay = TryDeduct(CurrencyType.Fame, player, 10);
                    //if (!pay) return;
                    //clear items used in craft
                    foreach (int slot in noDupeSlots)
                        player.Inventory[slot] = null;
                    //give reward
                    var availableSlot = player.Inventory.GetAvailableInventorySlot(result);
                    if (availableSlot != -1)
                    {
                        player.Inventory[availableSlot] = result;
                        player.SendInfo("Successfully forged " + result.ObjectId + "!");
                        return;
                    }
                    else
                    {
                        //so yea xD
                        player.SendError("Not enough space in inventory!");
                        return;
                    }
                }
                else
                {
                    player.SendError("Failed forge!");
                    return;
                }
            }
        }

        private bool TryDeduct(CurrencyType currency, Player player, int price)
        {
            if (player.Owner is Test)
                return false;

            var acc = player.Client.Account;
            var db = player.Manager.Database;
            if (acc.Guest)
            {
                // reload acc just in case user registered in game
                acc.FlushAsync();
                acc.Reload();
                if (acc.Guest) return false;
            }

            if (currency == CurrencyType.Fame)
            {
                if (acc.Fame < price)
                {
                    player.SendError("{\"key\":\"server.not_enough_fame\"}");
                    return false;
                }
                var trans = player.Manager.Database.Conn.CreateTransaction();
                player.Manager.Database.UpdateCurrency(acc, -price, CurrencyType.Fame, trans)
                    .ContinueWith(t =>
                    {
                        player.CurrentFame = acc.Fame;
                    });
                trans.Execute(CommandFlags.FireAndForget);
                return true;
            }
            if (currency == CurrencyType.Gold)
            {
                if (acc.Credits < price)
                {
                    player.SendError("{\"key\":\"server.not_enough_gold\"}");
                    return false;
                }
                db.UpdateCredit(acc, -price);
                player.Credits = acc.Credits;
                return true;
            }

            return false;
        }
    }
}