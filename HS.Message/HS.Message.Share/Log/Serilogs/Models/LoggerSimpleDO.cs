using Serilog.Events;
using System.Text.Json.Serialization;

namespace HS.Message.Share.Log.Serilogs.Models
{
    /// <summary>
    /// 日志数据库模型(简洁版-用于查询)
    /// </summary>
    public class LoggerSimpleDO
    {
        public int Id { get; set; }
        /// <summary>
        /// 主机地址
        /// </summary>
        public string HttpHost { get; set; }
        /// <summary>
        /// Http RemoteAddress
        /// </summary>
        public string HttpRemoteAddress { get; set; }
        /// <summary>
        /// Http X-Forwarded-For
        /// </summary>
        public string HttpXForwardedFor { get; set; }
        /// <summary>
        /// http path
        /// </summary>
        public string HttpPath { get; set; }
        /// <summary>
        /// http 请求id
        /// </summary>
        public string HttpRequestId { get; set; }
        /// <summary>
        /// 日志触发写入的上下文
        /// </summary>
        public string SourceContext { get; set; }
        /// <summary>
        /// 日志写入时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        [JsonIgnore]
        public LogEventLevel LevelEnum
        {
            get
            {
                if (Enum.TryParse(Level, out LogEventLevel level))
                {
                    return level;
                }
                else
                {
                    return LogEventLevel.Verbose;
                }
            }
        }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Exception { get; set; }
    }
}
