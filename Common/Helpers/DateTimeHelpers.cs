using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class DateTimeHelpers
    {

        public static string GetTimeStamp(long ms)
        {
            return TimeSpan.FromMilliseconds(ms).ToString(@"hh\:mm\:ss");
        }

        public static bool TimesAreEqual(DateTime x, DateTime y)
        {
            return x.ToString("HH:mm:ss") == y.ToString("HH:mm:ss");
        }

        public static string GetCurrentTimeZone()
        {
            //TODO handle daylight savings
            var localZone = TimeZone.CurrentTimeZone;
            return localZone.StandardName;
        }

        public static DateTime ConvertFromUtc(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
        }
    }
}
