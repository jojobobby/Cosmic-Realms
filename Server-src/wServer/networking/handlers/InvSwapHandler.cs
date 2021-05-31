using System;
using System.Collections.Generic;
using System.Linq;
using common.resources;
using Mono.Game;
using wServer.realm;
using wServer.realm.entities;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using System.Text.RegularExpressions;

namespace wServer.networking.handlers
{
    class InvSwapHandler : PacketHandlerBase<InvSwap>
    {
        private static readonly Random Rand = new Random();
        private const string tokenPrefix = " x";

        public override PacketId ID => PacketId.INVSWAP;

        protected override void HandlePacket(Client client, InvSwap packet)
        {
            client.Manager.Logic.AddPendingAction(t =>
                Handle(
                client.Player,
                client.Player.Owner.GetEntity(packet.SlotObj1.ObjectId),
                client.Player.Owner.GetEntity(packet.SlotObj2.ObjectId),
                packet.SlotObj1.SlotId, packet.SlotObj2.SlotId));
            
        }
        private void Handle(Player player, Entity a, Entity b, int slotA, int slotB) {
            if (player?.Owner == null)
                return;

            if (!ValidateEntities(player, a, b) || player.tradeTarget != null)
            {
                a.ForceUpdate(slotA);
                b.ForceUpdate(slotB);
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            var conA = (IContainer)a;
            var conB = (IContainer)b;

            // check if stacking operation
            if (b == player)
                foreach (var stack in player.Stacks)
                    if (stack.Slot == slotB)
                    {
                        var stackTrans = conA.Inventory.CreateTransaction();
                        var item = stack.Put(stackTrans[slotA]);
                        if (item == null) // success
                        {
                            // if a stackable item ends up in a gift chest it becomes infinite if not removed
                            if (a is GiftChest && stackTrans[slotA] != null)
                            {
                                var trans = player.Manager.Database.Conn.CreateTransaction();
                                player.Manager.Database.RemoveGift(player.Client.Account, stackTrans[slotA].ObjectType, trans);
                                trans.Execute();
                            }

                            stackTrans[slotA] = null;
                            Inventory.Execute(stackTrans);
                            player.Client.SendPacket(new InvResult() { Result = 0 });
                            return;
                        }
                    }

            // not stacking operation, continue on with normal swap

            // validate slot types
            if (!ValidateSlotSwap(player, conA, conB, slotA, slotB))
            {
                a.ForceUpdate(slotA);
                b.ForceUpdate(slotB);
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            // setup swap
            var queue = new Queue<Action>();
            var conATrans = conA.Inventory.CreateTransaction();
            var conBTrans = conB.Inventory.CreateTransaction();
            var itemA = conATrans[slotA];
            var itemB = conBTrans[slotB];
            conBTrans[slotB] = itemA;
            conATrans[slotA] = itemB;
            // validate that soulbound items are not placed in public bags (includes any swaped item from admins)
            if (!ValidateItemSwap(player, a, itemB))
            {
                queue.Enqueue(() => DropInSoulboundBag(player, itemB));
                conATrans[slotA] = null;
            }
            if (!ValidateItemSwap(player, b, itemA))
            {
                queue.Enqueue(() => DropInSoulboundBag(player, itemA));
                conBTrans[slotB] = null;
            }

            if (ValidateStackable(itemA, itemB, a, b, slotB, slotA))
            {
                var quantity = itemA.Quantity;

                for (var i = 1; i <= quantity; i++)
                {
                    var xmlData = player.Manager.Resources.GameData;
                    var itemBName = Regex.Replace(itemB.ObjectId, "\\d+", "") + (itemB.Quantity + 1);

                    if (xmlData.IdToObjectType.ContainsKey(itemBName))
                    {
                        var itemAName = Regex.Replace(itemA.ObjectId, "\\d+", "") + (itemA.Quantity - 1);

                        itemB = xmlData.Items[xmlData.IdToObjectType[itemBName]];
                        itemA = xmlData.IdToObjectType.ContainsKey(itemAName) ? xmlData.Items[xmlData.IdToObjectType[itemAName]] : null;

                        conBTrans[slotB] = itemB;
                        conATrans[slotA] = itemA;
                    }
                    else break;
                }
            }

            var gameData = player.Manager.Resources.GameData;

            if (!(a is GiftChest) &&
             !(b is GiftChest) &&
             itemA != null && itemB != null &&
             itemA.Doses != 0 && itemB.Doses != 0 &&
               (itemB.ObjectId?.Equals(itemA.ObjectId) ?? false) && // lazy hack so you can only stack items of the same kind
             (itemA.Doses != itemA.MaxDoses || itemB.Doses != itemB.MaxDoses))
            {
                int sum = itemA.Doses + itemB.Doses;
                int bCount = Math.Min(itemA.MaxDoses, sum);
                int aCount = sum >= itemA.MaxDoses ? sum - itemA.MaxDoses : 0;

                conATrans[slotA] = (from item in gameData.Items.Values
                                    where item.ObjectId.Equals(itemA.ObjectId) && item.Doses == aCount
                                    select item)?.FirstOrDefault() ?? null;

                conBTrans[slotB] = (from item in gameData.Items.Values
                                    where item.ObjectId.Equals(item.ObjectId) && item.Doses == bCount
                                    select item)?.FirstOrDefault() ?? null;

                if (Inventory.Execute(conATrans, conBTrans))
                {
                    while (queue.Count > 0)
                        queue.Dequeue()();

                    player.Client.SendPacket(new InvResult() { Result = 0 });
                    return;
                }

                a.ForceUpdate(slotA);
                b.ForceUpdate(slotB);
                player.Client.SendPacket(new InvResult() { Result = 1 });
                return;
            }

            // swap items
            if (Inventory.Execute(conATrans, conBTrans))
            {
                // remove gift if from gift chest
                var db = player.Manager.Database;
                var trans = db.Conn.CreateTransaction();
                if (a is GiftChest && itemA != null)
                    db.RemoveGift(player.Client.Account, itemA.ObjectType, trans);
                if (b is GiftChest && itemB != null)
                    db.RemoveGift(player.Client.Account, itemB.ObjectType, trans);
                if (trans.Execute())
                {
                    while (queue.Count > 0)
                        queue.Dequeue()();

                    player.Client.SendPacket(new InvResult() { Result = 0 });
                    return;
                }

                // if execute failed, undo inventory changes
                if (!Inventory.Revert(conATrans, conBTrans))
                    Log.Warn($"Failed to revert changes. {player.Name} has an extra {itemA?.ObjectId} or {itemB?.ObjectId}");
            }

            a.ForceUpdate(slotA);
            b.ForceUpdate(slotB);
            player.Client.SendPacket(new InvResult() { Result = 1 });
        }

        bool ValidateEntities(Player p, Entity a, Entity b)
        { // returns false if bad input
            if (a == null || b == null)
                return false;

            if ((a as IContainer) == null ||
                (b as IContainer) == null)
                return false;

            if (a is Player && a != p ||
                b is Player && b != p)
                return false;

            if (a is Container &&
                (a as Container).BagOwners.Length > 0 &&
                !(a as Container).BagOwners.Contains(p.AccountId))
                return false;

            if (b is Container &&
                (b as Container).BagOwners.Length > 0 &&
                !(b as Container).BagOwners.Contains(p.AccountId))
                return false;

            if (a is OneWayContainer && b != p ||
                b is OneWayContainer && a != p)
                return false;

            var aPos = new Vector2(a.X, a.Y);
            var bPos = new Vector2(b.X, b.Y);
            if (Vector2.DistanceSquared(aPos, bPos) > 1)
                return false;

            return true;
        }

        private bool ValidateStackable(Item itemA, Item itemB, Entity a, Entity b, int slotA, int slotB)
           => itemA != null && itemB != null
               && itemA.Quantity > 0 && itemB.Quantity > 0
               && a is Player && b is Player
               && slotA != slotB
               && SameItem(itemA, itemB);

        private bool SameItem(Item itemA, Item itemB)
        {
            var itemAObjId = itemA.ObjectId;
            var itemBObjId = itemB.ObjectId;

            if (itemAObjId.Contains(tokenPrefix) && itemBObjId.Contains(tokenPrefix))
            {
                var itemAPrefix = itemAObjId.IndexOf(tokenPrefix);
                var itemBPrefix = itemBObjId.IndexOf(tokenPrefix);
                var itemAName = itemAObjId.Substring(0, itemAPrefix);
                var itemBName = itemBObjId.Substring(0, itemBPrefix);

                return itemAName == itemBName;
            }
            else return false;
        }

        private bool ValidateSlotSwap(Player player, IContainer conA, IContainer conB, int slotA, int slotB)
        {
            return
                (slotA < 12 && slotB < 12 || player.HasBackpack) &&
                conB.AuditItem(conA.Inventory[slotA], slotB) &&
                conA.AuditItem(conB.Inventory[slotB], slotA);
        }

        private bool ValidateItemSwap(Player player, Entity c, Item item)
        {
            return c == player ||
                   item == null ||
                   !item.Soulbound && !player.Client.Account.Admin ||
                   IsSoleContainerOwner(player, c as IContainer);
        }

        private bool IsSoleContainerOwner(Player player, IContainer con)
        {
            int[] owners = null;
            var container = con as Container;
            if (container != null)
                owners = container.BagOwners;

            return owners != null && owners.Length == 1 && owners.Contains(player.AccountId);
        }

        private void DropInSoulboundBag(Player player, Item item)
        {
            var container = new Container(player.Manager, 0x0503, 1000 * 60, true)
            {
                BagOwners = new int[] { player.AccountId }
            };
            container.Inventory[0] = item;
            container.Move(player.X + (float)((Rand.NextDouble() * 2 - 1) * 0.5),
                           player.Y + (float)((Rand.NextDouble() * 2 - 1) * 0.5));
            container.SetDefaultSize(75);
            player.Owner.EnterWorld(container);
        }
    }
}
