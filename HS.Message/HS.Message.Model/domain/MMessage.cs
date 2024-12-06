using System;
using System.Collections.Generic;
using HS.Message.Share.Attributes;

/*
 * @author : shichaohu
 * @date : 2024-12-6
 * @desc : 消息
 */
namespace HS.Message.Model
{
    /// <summary>
    /// 消息
    /// </summary>
    public partial class MMessage : MBaseModel
    {

        /// <summary>
        /// 主题
        /// </summary>
        [FieldAttribute("主题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [FieldAttribute("内容")]
        public string Content { get; set; }

        /// <summary>
        /// 动态参数;动态参数，用于替换消息模板里面的内容
        /// </summary>
        [FieldAttribute("动态参数")]
        public string DynamicParameter { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [FieldAttribute("链接地址")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 链接文本
        /// </summary>
        [FieldAttribute("链接文本")]
        public string LinkText { get; set; }

        /// <summary>
        /// 业务类型key
        /// </summary>
        [FieldAttribute("业务类型key")]
        public string BusinessTypeKey { get; set; }

        /// <summary>
        /// 业务类型value
        /// </summary>
        [FieldAttribute("业务类型value")]
        public string BusinessTypeValue { get; set; }

        /// <summary>
        /// 消息发送渠道;1:邮件，2:短信，3:企业微信，4:钉钉...... 多个用,隔开
        /// </summary>
        [FieldAttribute("消息发送渠道")]
        public string SendChannel { get; set; }

        /// <summary>
        /// 发送状态;1：待发送，2：发送中，3：全部成功，4.部分成功
        /// </summary>
        [FieldAttribute("发送状态")]
        public int SendState { get; set; }
    }
}
