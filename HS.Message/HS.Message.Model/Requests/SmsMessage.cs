using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Model.Requests
{
    /// <summary>
    /// 短息模板
    /// </summary>
    public class SmsMessage:MBaseModel
    {

        /// <summary>
        /// 乐观锁
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 消息接收人id
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// 短息渠道编号;1：阿里云
        /// </summary>
        public int ChannelCode { get; set; }

        /// <summary>
        /// 短息渠道名称;1：阿里云
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 短息签名名称
        /// </summary>
        public string SignName { get; set; }

        /// <summary>
        /// 短息模板id
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短息模板变量值;短信平台模板参数json字符串
        /// </summary>
        public string TemplateParam { get; set; }

        /// <summary>
        /// 短息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送方式;1：单发，2：群发
        /// </summary>
        public int SendType { get; set; }

        /// <summary>
        /// 接收手机号码;不同的手机号码直接通过,连接
        /// </summary>
        public string PhoneNumbers { get; set; }

        /// <summary>
        /// 发送状态;1：待发送，2：已发送，3：发送失败
        /// </summary>
        public int SendState { get; set; }

        /// <summary>
        /// 提交时间;我方提交短信平台时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 发送回执ID
        /// </summary>
        public string BizId { get; set; }

        /// <summary>
        /// 请求id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 发送结果备注
        /// </summary>
        public string SendRemark { get; set; }

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
