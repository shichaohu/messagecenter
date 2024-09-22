using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Model.Requests
{
    /// <summary>
    /// 邮件消息
    /// </summary>
    public class MailMessage:MBaseModel
    {

        /// <summary>
        /// 乐观锁
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 逻辑id
        /// </summary>
        public string LogicalId { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 消息接收人id
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// 邮件模板id
        /// </summary>
        public string MailTemplateId { get; set; }

        /// <summary>
        /// 邮件发送信息配置id
        /// </summary>
        public string MailConfiguerId { get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string MailTitle { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailBody { get; set; }

        /// <summary>
        /// 收件邮箱;支持多个邮件一起发送，不同的接受邮件地址间通过“,”连接起来
        /// </summary>
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// 抄送邮箱
        /// </summary>
        public string ReceiverCcEmail { get; set; }

        /// <summary>
        /// 总共发送次数;默认为1一次
        /// </summary>
        public int TotalSendNum { get; set; }

        /// <summary>
        /// 发送频率间隔(单位秒);单位为秒，只有要求发送多次的才有效
        /// </summary>
        public int SendFrequency { get; set; }

        /// <summary>
        /// 开始发送时间
        /// </summary>
        public DateTime StartSendTime { get; set; }

        /// <summary>
        /// 已经发送次数
        /// </summary>
        public int HasSendNum { get; set; }

        /// <summary>
        /// 上一次发送时间
        /// </summary>
        public DateTime LastSendTime { get; set; }

        /// <summary>
        /// 下一次发送时间
        /// </summary>
        public DateTime NextSendTime { get; set; }

        /// <summary>
        /// 发送状态;1：待发送，2：发送中（针对发送多次的，还未发送完都是这个状态），3：已发送，默认为1
        /// </summary>
        public int SendState { get; set; }

        /// <summary>
        /// 邮件Smtp服务器
        /// </summary>
        public string SmtpService { get; set; }

        /// <summary>
        /// 发送邮件地址
        /// </summary>
        public string SendEmail { get; set; }

        /// <summary>
        /// 邮件发送密码
        /// </summary>
        public string SendPwd { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatedById { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新人Id
        /// </summary>
        public string UpdatedById { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        public string UpdatedByName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; }


    }
}
