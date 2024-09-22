using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Model.Requests
{
    /// <summary>
    /// 消息请求模型
    /// </summary>
    public class MessageRequest
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 动态参数;动态参数，用于替换消息模板里面的内容
        /// </summary>
        public Dictionary<string, string> DynamicParameter { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 链接文本
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// 业务类型key
        /// </summary>
        public string BusinessTypeKey { get; set; }

        /// <summary>
        /// 业务类型value
        /// </summary>
        public string BusinessTypeValue { get; set; }

        /// <summary>
        /// 消息发送渠道;1:邮件，2:短信，3:企业微信，4:钉钉...... 多个用,隔开
        /// </summary>
        public string Sendchannel { get; set; }
        /// <summary>
        /// 消息发送人
        /// </summary>
        public MessageSender Sender { get; set; }
        /// <summary>
        /// 消息接收人
        /// </summary>
        public List<MessageReceiver> Receiver { get; set; }
    }

    /// <summary>
    /// 消息接收人
    /// </summary>
    public class MessageReceiver
    {

        /// <summary>
        /// 接收人用户id
        /// </summary>
        public string ReceiverUserid { get; set; }

        /// <summary>
        /// 接收人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 抄送邮件
        /// </summary>
        public string CcEmail { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 企业微信
        /// </summary>
        public string EnterpriseWechat { get; set; }

        /// <summary>
        /// 钉钉
        /// </summary>
        public string Dingtalk { get; set; }

        /// <summary>
        /// 其他消息接收渠道;消息的其他接收渠道，json字符串
        /// </summary>
        public string OtherReceiveChannel { get; set; }
    }
    /// <summary>
    /// 消息发送人
    /// </summary>
    public class MessageSender
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
    }
}
