namespace HS.Message.Share.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtensions
    {

        /// <summary>
        /// TimeSpan WithMilliseconds
        /// </summary>
        /// <returns></returns>
        public static long ToTimeSpanWithMilliseconds(this DateTime timeValue)
        {

            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeUTC = timeValue.ToUniversalTime();
            TimeSpan ts = (timeUTC - dd);
            return (long)ts.TotalMilliseconds;//精确到毫秒
        }

        /// <summary>
        /// 本时区日期时间转Utc时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>Utc时间戳(精确到秒)</returns>
        public static long ToTimestamp(this DateTime datetime)
        {
            DateTime dd = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime timeUTC = datetime.ToUniversalTime();
            TimeSpan ts = (timeUTC - dd);
            return (long)ts.TotalSeconds;//精确到秒
        }

        /// <summary>
        /// Utc时间戳转本时区时间
        /// </summary>
        /// <param name="timeStamp">Utc时间戳(精确到秒)</param>
        /// <returns>本地时间</returns>
        public static DateTime TimestampToDateTime(long timeStamp)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var utcTtime = startTime.AddSeconds(timeStamp);

            var time = TimeZoneInfo.ConvertTimeFromUtc(utcTtime, TimeZoneInfo.Local);
            return time;
        }
    }
}
