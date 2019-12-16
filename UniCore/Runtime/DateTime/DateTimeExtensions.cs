namespace UniGreenModules.UniCore.Runtime.DateTime
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime UnitTime = new DateTime(1970, 1, 1);
        
        public static int ToUnixTimestamp(this DateTime date)
        {
            var unixTimestamp = (int)(date.ToUniversalTime().
                Subtract(UnitTime)).TotalSeconds;
            return unixTimestamp;
        }

    }
}
