using HS.Message.Share.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HS.Message.Share.MessageEmitter.Params
{
    /// <summary>
    /// mail公共参数
    /// </summary>
    public class MailMParameter
    {
        /// <summary>
        /// 邮件Smtp服务器
        /// </summary>
        /// <summary>
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
        /// 收件邮箱;支持多个邮件一起发送
        /// </summary>
        public string[] ReceiverEmails { get; set; }
        /// <summary>
        /// 抄送邮箱
        /// </summary>
        public string[] ReceiverCcEmails { get; set; }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string MailTitle { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailBody { get; set; }

    }
}
