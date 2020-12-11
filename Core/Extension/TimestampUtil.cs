using System;

namespace Core.Extension
{
    public class TimestampUtil
    {
        private static readonly DateTime UtcOffset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long UnixTimestamp()
        {
            return (long)(DateTime.UtcNow - UtcOffset).TotalSeconds;
        }

        public static long UnixTimestampMillis()
        {
            return (long)(DateTime.UtcNow - UtcOffset).TotalMilliseconds;
        }
    }
}