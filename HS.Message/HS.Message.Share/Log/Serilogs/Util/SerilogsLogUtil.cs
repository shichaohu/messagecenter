using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Log.Serilogs.Utils
{
    public class SerilogsLogUtils
    {
        /// <summary>
        /// 获取分表后的日志表名
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="tableNamePrefix">日志表前缀(即名称相同部分)</param>
        /// <returns></returns>
        public static string GetPracticalTableName(DateTime dateTime, string tableNamePrefix)
        {
            //创建公历日历对象
            GregorianCalendar gregorianCalendar = new GregorianCalendar();

            //获取指定日期是周数
            // CalendarWeekRule:第一周开始于该年的第一天
            // DayOfWeek:每周第一天是星期几
            int weekOfYear = gregorianCalendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return $"{tableNamePrefix}_week_{weekOfYear}";
        }
        /// <summary>
        /// 获取分表后的日志表名List
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="tableNamePrefix">日志表前缀(即名称相同部分)</param>
        /// <returns></returns>
        public static List<string> GetPracticalTableNameList(DateTime startTime, DateTime endTime, string tableNamePrefix)
        {
            List<string> tableNameList = new();
            //创建公历日历对象
            GregorianCalendar gregorianCalendar = new GregorianCalendar();

            //获取指定日期是周数
            // CalendarWeekRule:第一周开始于该年的第一天
            // DayOfWeek:每周第一天是星期几
            int startWeekOfYear = gregorianCalendar.GetWeekOfYear(startTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int endWeekOfYear = gregorianCalendar.GetWeekOfYear(endTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            for (int i = startWeekOfYear; i <= endWeekOfYear; i++)
            {
                tableNameList.Add($"{tableNamePrefix}_week_{i}");
            }

            return tableNameList;
        }
    }
}
