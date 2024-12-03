using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HS.Rabbitmq.Model
{
    /// <summary>
    /// 队列消息模型
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// 命名（系统名称）
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public QueueMessageType MessageType { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }
        /// <summary>
        /// 消息内容是否写入数据库
        /// </summary>
        public bool hasContentWritedToDb { get; set; }
        /// <summary>
        /// The number of times currently consumed
        /// </summary>
        public int NumberOfConsumed { get; set; }
    }
    public enum QueueMessageType
    {
        Email,
        SMS,
        EnterpriseWechat,
        Dingtalk,
        ExternalApi
    }
}
