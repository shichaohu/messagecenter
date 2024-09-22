using Serilog.Events;
using System.Text.Json.Serialization;

namespace HS.Message.Share.Log.Serilogs.Models
{
    /// <summary>
    /// 日志数据库模型
    /// </summary>
    public class LoggerDO : LoggerSimpleDO
    {
        /// <summary>
        /// 日志模版
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Exception { get; set; }
    }
}
