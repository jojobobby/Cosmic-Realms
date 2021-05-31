using System;
using System.Collections.Specialized;
using System.Xml.Linq;
using Anna.Request;
using common;
using common.resources;

namespace server.dailyLogin
{
    class fetchCalendar : RequestHandler
    {
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            DbAccount acc;
            var status = Database.Verify(query["guid"], query["password"], out acc);
            if (status != LoginStatus.OK || status == LoginStatus.AccountNotExists)
            {
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
                return;
            }
            if (MonthCalendarUtils.DISABLE_CALENDAR || DateTime.UtcNow >= MonthCalendarUtils.EndDate)
            {
                Write(context, "<Error>This feature is disabled.</Error>");
                return;
            }
            var CalendarDb = new DailyCalendar(acc);
            if (CalendarDb.IsNull || MonthCalendarUtils.IsNextCalendar(CalendarDb.LastTime))
            {
                CalendarDb = new DailyCalendar(acc)
                {
                    ClaimedDays = new int[] { },
                    ConsecutiveDays = 1,
                    NonConsecutiveDays = 1,
                    UnlockableDays = 1,
                    LastTime = DateTime.Today
                };
            }
            if (MonthCalendarUtils.IsNextDay(CalendarDb.LastTime))
            {
                CalendarDb.NonConsecutiveDays++;
                CalendarDb.UnlockableDays++;
                CalendarDb.LastTime = DateTime.Today;
            }
            CalendarDb.FlushAsync();
            Write(context, GetXML(CalendarDb));
        }

        internal string GetXML(DailyCalendar calendar)
        {
            var LoginRewards = new XElement("LoginRewards");
            LoginRewards.Add(new XAttribute("serverTime", DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            LoginRewards.Add(new XAttribute("conCurDay", calendar.ConsecutiveDays));
            LoginRewards.Add(new XAttribute("nonconCurDay", calendar.NonConsecutiveDays));

            var nonConsecutive = new XElement("NonConsecutive");
            nonConsecutive.Add(new XAttribute("days", calendar.NonConsecutiveDays));

            for (int day = 1; day < MonthCalendarUtils.MonthCalendarList.Count + 1; day++)
            {
                var dayCalendar = MonthCalendarUtils.MonthCalendarList[day - 1];

                var Login = new XElement("Login");
                Login.Add(new XElement("Days", day));

                var Item = new XElement("ItemId", dayCalendar.Item);
                Item.Add(new XAttribute("quantity", dayCalendar.Quantity));
                Login.Add(Item);

                var Gold = new XElement("Gold", dayCalendar.Gold);
                Login.Add(Gold);

                if (calendar.ClaimedDays.ToCommaSepString().Contains(day.ToString()))
                {
                    Login.Add(new XElement("Claimed"));
                }
                else if (calendar.UnlockableDays >= day)
                {
                    Login.Add(new XElement("key", day));
                }
                nonConsecutive.Add(Login);
            }

            LoginRewards.Add(nonConsecutive);

            var Consecutive = new XElement("Consecutive");
            Consecutive.Add(new XAttribute("days", calendar.ConsecutiveDays));
            LoginRewards.Add(Consecutive);

            var UnlockableDays = new XElement("Unlockable");
            UnlockableDays.Add(new XAttribute("days", calendar.UnlockableDays));
            LoginRewards.Add(UnlockableDays);

            return LoginRewards.ToString();
        }
    }
}