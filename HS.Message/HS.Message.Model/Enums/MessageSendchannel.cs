using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Model.Enums
{
    /// <summary>
    /// 消息发送渠道
    /// </summary>
    public enum MessageSendchannel
    {
        /// <summary>
        /// 邮件
        /// </summary>
        Email = 1,
        /// <summary>
        /// 短信
        /// </summary>
        SMS,
        /// <summary>
        /// 企业微信
        /// </summary>
        EnterpriseWechat,
        /// <summary>
        /// 钉钉
        /// </summary>
        Dingtalk
    }
}
