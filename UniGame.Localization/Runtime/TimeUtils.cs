namespace UniGame.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UniGame.Utils.Runtime;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine.Localization.SmartFormat.Utilities;

    public static class TimeUtils
    {
        public const TimeSpanFormatOptions DefaultOptions = TimeSpanFormatOptions.Abbreviate|TimeSpanFormatOptions.LessThanOff|
                                                            TimeSpanFormatOptions.TruncateFill|TimeSpanFormatOptions.RangeSeconds|
                                                            TimeSpanFormatOptions.RangeHours;
        public const TimeSpanFormatOptions DefaultOptionsWithoutSeconds = TimeSpanFormatOptions.Abbreviate|
                                                                          TimeSpanFormatOptions.LessThanOff|TimeSpanFormatOptions.TruncateFill|
                                                                          TimeSpanFormatOptions.RangeMinutes|TimeSpanFormatOptions.RangeHours;
        
        private static Regex _tokenRegex = new Regex(@"\{[^\{\}]+\}");

        private static TimeZoneInfo easterStandardTimezone = 
            TimeZoneInfo.CreateCustomTimeZone(
                "America/NewYork",
                TimeSpan.FromHours(-5), 
                "America/NewYork", 
                "America/NewYork");
        
        public static readonly DateTime StartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime EstStartTime => TimeZoneInfo.ConvertTimeFromUtc(StartTime, easterStandardTimezone);

        public static long GetUtcNow()
        {
            return (long)(DateTime.UtcNow - StartTime).TotalMilliseconds;
        }

        public static long UtcToEst(long utcMs)
        {
            var utc = StartTime.AddMilliseconds(utcMs);
            return (long)(TimeZoneInfo.ConvertTimeFromUtc(utc, easterStandardTimezone) - EstStartTime).TotalMilliseconds;
        }

        public static long EstToUtc(long estMs)
        {
            var est = EstStartTime.AddMilliseconds(estMs);
            return (long) (TimeZoneInfo.ConvertTimeToUtc(est, easterStandardTimezone) - StartTime).TotalMilliseconds;
        }

        public static long GetEstNow()
        {
            var utc = GetUtcNow();
            return UtcToEst(utc);
        }

        public static DateTime UtcDateTimeFromMilliseconds(long ms)
        {
            return StartTime.AddMilliseconds(ms);
        }
        
        public static DateTime EstDateTimeFromMilliseconds(long ms)
        {
            return EstStartTime.AddMilliseconds(ms);
        }

        public static string Format(TimeSpan time, TimeSpanFormatOptions options = DefaultOptions)
        {
            var str = time.ToTimeString(options, LocalizationHelper.LocalizedTimeTextInfo);
            return str;
        }

        public static string Format(string format, TimeSpan timeSpan)
        {
            var units  = new List<int> {timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds};

            var tokens = _tokenRegex.Matches(format).Count;
            if (tokens > units.Count)
            {
                GameLog.LogError($"Input string requires more parameters. Format: {format}, submitted parameters count: {units.Count}");
                return timeSpan.ToString(@"hh\:mm\:ss");
            }

            var firstValIndex = units.FindIndex(u => u != 0);
            units = firstValIndex == -1 ? Enumerable.Repeat(0, tokens).ToList() : units.GetRange(Math.Min(firstValIndex, units.Count - tokens), tokens);
            
            return tokens switch
            {
                1 => string.Format(format, units[0]),
                2 => string.Format(format, units[0], units[1]),
                3 => string.Format(format, units[0], units[1], units[2]),
                4 => string.Format(format, units[0], units[1], units[2], units[3]),
                _ => format
            };
        }
    }
}
