using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    class ClaimDailyRewardHandler : PacketHandlerBase<ClaimDailyRewardMessage>
    {
        public override PacketId ID => PacketId.CLAIM_LOGIN_REWARD_MSG;

        protected override void HandlePacket(Client client, ClaimDailyRewardMessage packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));//
            Handle(client, packet);
        }

        private void Handle(Client client, ClaimDailyRewardMessage packet)
        {
            DailyCalendar CalendarDb = new DailyCalendar(client.Account);
            var Calendar = MonthCalendarUtils.MonthCalendarList;

            int ClaimKey = (Convert.ToInt32(packet.ClaimKey) - 1);
            string Type = packet.Type; // TODO Consecutive Calendar

            if (MonthCalendarUtils.DISABLE_CALENDAR || DateTime.UtcNow >= MonthCalendarUtils.EndDate || CalendarDb.IsNull ||
                CalendarDb.ClaimedDays.ToCommaSepString().Contains(packet.ClaimKey + 1)
                || CalendarDb.UnlockableDays < ClaimKey || Calendar.Count < ClaimKey)
                return;

            client.SendPacket(new ClaimDailyRewardResponse
            {
                ItemId = Calendar[ClaimKey].Item,
                Quantity = Calendar[ClaimKey].Quantity,
                Gold = Calendar[ClaimKey].Gold
            });

            if (Calendar[ClaimKey].Gold > 0)
                client.Account.Credits += Calendar[ClaimKey].Gold;
            else if (Calendar[ClaimKey].Item != -1 && Calendar[ClaimKey].Quantity > 1)
            {
                List<ushort> items = new List<ushort>();

                for (int i = 0; i < Calendar[ClaimKey].Quantity; i++)
                    items.Add((ushort)Calendar[ClaimKey].Item);
                client.Manager.Database.AddGifts(client.Account, items);
            }
            else if (Calendar[ClaimKey].Item != -1)
                client.Manager.Database.AddGift(client.Account, (ushort)Calendar[ClaimKey].Item);

            var ClaimedDays = CalendarDb.ClaimedDays.ToList();

            ClaimedDays.Add(ClaimKey + 1);
            CalendarDb.ClaimedDays = ClaimedDays.ToArray();
            CalendarDb.FlushAsync();

            client.Account.FlushAsync();//
            client.Account.Reload();//
        }
    }
}
