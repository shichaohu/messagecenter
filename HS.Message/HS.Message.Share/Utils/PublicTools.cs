using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Converters;
using NodaTime;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HS.Message.Share.Utils
{
    public static class PublicTools
    {
        public static int? _timeZoneDif;

        public static IsoDateTimeConverter GetDateTimeConverter()
        {
            return new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
        }

        public static long GetTimeDifferenceMilliseconds(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                return 0L;
            }

            return (endTime - startTime).Milliseconds;
        }

        public static DateTime StampToDateTime(string timestampStr)
        {
            if (string.IsNullOrEmpty(timestampStr))
            {
                return default(DateTime);
            }

            return StampToDateTime(Convert.ToInt64(timestampStr));
        }

        public static DateTime StampToDateTime(long timestamp)
        {
            return Convert.ToDateTime(new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static string StampToString(string timestampStr)
        {
            if (string.IsNullOrEmpty(timestampStr))
            {
                return string.Empty;
            }

            return StampToString(Convert.ToInt64(timestampStr));
        }

        public static string StampToString(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime MillisecondToDateTime(string timestampStr, int addHour = 0)
        {
            if (string.IsNullOrEmpty(timestampStr))
            {
                return default(DateTime);
            }

            return MillisecondToDateTime(Convert.ToInt64(timestampStr), addHour);
        }

        public static DateTime MillisecondToDateTime(long timestamp, int addHour = 0)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timestamp).ToLocalTime();
            if (addHour != 0)
            {
                dateTime = dateTime.AddHours(addHour);
            }

            return Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static string MillisecondToString(string timestampStr, int addHour = 0)
        {
            if (string.IsNullOrEmpty(timestampStr))
            {
                return string.Empty;
            }

            return MillisecondToString(Convert.ToInt64(timestampStr), addHour);
        }

        public static string MillisecondToString(long timestamp, int addHour = 0)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local).AddMilliseconds(timestamp).ToLocalTime();
            if (addHour > 0)
            {
                dateTime = dateTime.AddHours(addHour);
            }

            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime GMT2Local(string gmt)
        {
            DateTime result = DateTime.MinValue;
            try
            {
                string text = "";
                if (gmt.IndexOf("+0") != -1)
                {
                    gmt = gmt.Replace("GMT", "");
                    text = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                }

                if (gmt.ToUpper().IndexOf("GMT") != -1)
                {
                    text = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
                }

                if (text != "")
                {
                    result = DateTime.ParseExact(gmt, text, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                    result = result.ToLocalTime();
                }
                else
                {
                    result = Convert.ToDateTime(gmt);
                }
            }
            catch
            {
            }

            return result;
        }

        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1 || m == 2)
            {
                m += 12;
                y--;
            }

            int num = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
            string result = "";
            switch (num)
            {
                case 0:
                    result = "周一";
                    break;
                case 1:
                    result = "周二";
                    break;
                case 2:
                    result = "周三";
                    break;
                case 3:
                    result = "周四";
                    break;
                case 4:
                    result = "周五";
                    break;
                case 5:
                    result = "周六";
                    break;
                case 6:
                    result = "周日";
                    break;
            }

            return result;
        }

        public static string CaculateWeekDay(this DateTime dateTime)
        {
            return CaculateWeekDay(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static DateTime GetResultDateTime(DateTime planDateTime, DateTime estimatedDateTime, DateTime actualDateTime)
        {
            if (actualDateTime.IsEffectiveDateTime())
            {
                return actualDateTime;
            }

            if (estimatedDateTime.IsEffectiveDateTime())
            {
                return estimatedDateTime;
            }

            return planDateTime;
        }

        public static DateTime GetResultDateTime(DateTime planDateTime, DateTime estimatedDateTime, DateTime actualDateTime, DateTime scheduleDate)
        {
            if (scheduleDate.IsEffectiveDateTime())
            {
                return scheduleDate;
            }

            if (actualDateTime.IsEffectiveDateTime())
            {
                return actualDateTime;
            }

            if (estimatedDateTime.IsEffectiveDateTime())
            {
                return estimatedDateTime;
            }

            return planDateTime;
        }

        public static bool IsEffectiveDateTime(this DateTime dateTime)
        {
            if (DateTime.MinValue < dateTime && dateTime < DateTime.MaxValue)
            {
                return true;
            }

            return false;
        }

        public static long GetTimeStamp()
        {
            return Convert.ToInt64((GetSysDateTimeNow() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        public static long GetTimeStamp(DateTime dateTime)
        {
            return Convert.ToInt64((dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        public static string ConvertDeteTimeToString(DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
            {
                return null;
            }

            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static int TimeZoneDif()
        {
            try
            {
                if (_timeZoneDif.HasValue)
                {
                    return _timeZoneDif.Value;
                }

                string value = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build()
                    .GetSection("TimeZoneDif")
                    .Value;
                if (string.IsNullOrEmpty(value))
                {
                    _timeZoneDif = 0;
                }
                else
                {
                    _timeZoneDif = Convert.ToInt32(value);
                }
            }
            catch (Exception)
            {
                _timeZoneDif = 0;
            }

            return _timeZoneDif.Value;
        }

        public static DateTime GetSysDateTimeNow()
        {
            Instant currentInstant = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone zone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            DateTime result = currentInstant.InZone(zone).ToDateTimeUnspecified();
            if (TimeZoneDif() != 0)
            {
                return result.AddHours(TimeZoneDif());
            }

            return result;
        }

        public static string GetSysDateTimeNowStringYMDHMS()
        {
            return GetSysDateTimeNow().ToFormatStringYMDHMS();
        }

        public static string GetSysDateTimeNowStringYMD()
        {
            return GetSysDateTimeNow().ToFormatStringYMD();
        }

        public static string ToFormatStringYMDHMS(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToFormatStringYMD(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        public static string GetDateRandomString()
        {
            return GetSysDateTimeNow().ToString("yyyyMMddHHmmssfff") + new Random().Next(100000, 999999);
        }

        public static string DecodeUnicode(string value)
        {
            short result;
            return new Regex("\\\\u([0-9a-fA-F]{4})", RegexOptions.Compiled).Replace(value, (Match m) => short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result) ? (((char)result).ToString() ?? "") : m.Value);
        }

        public static string UTF8Encode(this string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8);
        }

        public static string UTF8Decode(this string value)
        {
            return HttpUtility.UrlDecode(value, Encoding.UTF8);
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, "^[+-]?\\d*[.]?\\d*$");
        }

        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, "^[+-]?\\d*$");
        }

        public static bool IsVersion(string value)
        {
            return Regex.IsMatch(value, "^[0-9]{1,2}\\.[0-9]{1,2}$");
        }

        public static List<string> SplitStringToLtterAndNum(string orgStr)
        {
            if (string.IsNullOrEmpty(orgStr))
            {
                return null;
            }

            string text = string.Empty;
            List<string> list = new List<string>();
            int num = 0;
            bool flag = false;
            char[] array = orgStr.ToCharArray();
            foreach (char c in array)
            {
                if (!flag)
                {
                    if ('0' <= c && c <= '9')
                    {
                        switch (num)
                        {
                            case 0:
                                num = 1;
                                break;
                            default:
                                num = 1;
                                flag = true;
                                list.Add(text);
                                text = string.Empty;
                                break;
                            case 1:
                                break;
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0:
                                num = 2;
                                break;
                            default:
                                num = 2;
                                flag = true;
                                list.Add(text);
                                text = string.Empty;
                                break;
                            case 2:
                                break;
                        }
                    }
                }

                text = $"{text}{c}";
            }

            if (!string.IsNullOrEmpty(text))
            {
                list.Add(text);
            }

            return list;
        }

        public static bool CheckLetterEnd(string orgStr)
        {
            if (string.IsNullOrEmpty(orgStr))
            {
                return false;
            }

            char[] array = orgStr.ToCharArray();
            if ('0' <= array[^1] && array[^1] <= '9')
            {
                return false;
            }

            return true;
        }

        public static FileExtensionContentTypeProvider SetSupportFile()
        {
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            fileExtensionContentTypeProvider.Mappings[".xls"] = "application/vnd.ms-excel";
            fileExtensionContentTypeProvider.Mappings[".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return fileExtensionContentTypeProvider;
        }

        public static string GetFirstUpperStr(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 1)
                {
                    return char.ToUpper(value[0]) + value.Substring(1);
                }

                return char.ToUpper(value[0]).ToString();
            }

            return null;
        }

        public static string GetFirstLowerStr(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 1)
                {
                    return char.ToLower(value[0]) + value.Substring(1);
                }

                return char.ToLower(value[0]).ToString();
            }

            return null;
        }

        public static string GetClassNameByTableName(this string tableName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(tableName))
            {
                string[] array = tableName.Split('_');
                foreach (string value in array)
                {
                    stringBuilder.Append(value.GetFirstUpperStr());
                }
            }

            return stringBuilder.ToString();
        }

        public static Type GetTypeByDateType(this string dataType, bool isnullable)
        {
            switch (dataType)
            {
                case "int":
                    if (!isnullable)
                    {
                        return typeof(int);
                    }

                    return typeof(int?);
                case "bigint":
                    if (!isnullable)
                    {
                        return typeof(long);
                    }

                    return typeof(long?);
                case "smallint":
                    if (!isnullable)
                    {
                        return typeof(short);
                    }

                    return typeof(short?);
                case "float":
                    if (!isnullable)
                    {
                        return typeof(float);
                    }

                    return typeof(float?);
                case "decimal":
                    if (!isnullable)
                    {
                        return typeof(decimal);
                    }

                    return typeof(decimal?);
                case "double":
                    if (!isnullable)
                    {
                        return typeof(double);
                    }

                    return typeof(double?);
                case "datetime":
                case "timestamp":
                    return typeof(DateTime);
                default:
                    return typeof(string);
            }
        }

        public static string ExportFile(string value, string fullFileName)
        {
            string text = "wwwroot/fileUpload";
            text = Directory.GetCurrentDirectory() + "/" + text;
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }

            new FileInfo(Path.Combine(text, fullFileName)).Delete();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            using (StreamWriter streamWriter = new StreamWriter(text + "/" + fullFileName, append: false, encoding))
            {
                streamWriter.Write(value);
                streamWriter.Flush();
            }

            return text + "/" + fullFileName;
        }

        public static string GetRandomCode(int lenght)
        {
            string text = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int num = random.Next();
                text += (char)(48 + (ushort)(num % 10));
            }

            return text;
        }

        public static bool CheckContain(this List<string> obj, List<string> compare)
        {
            if (obj == null || obj.Count < 1 || compare == null || compare.Count < 1)
            {
                return false;
            }

            foreach (string item in obj)
            {
                if (compare.Exists((string x) => x == item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
