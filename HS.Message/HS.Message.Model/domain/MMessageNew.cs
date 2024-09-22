
using HS.Message.Share.Attributes;
using System;
namespace HS.Message.Model
{


    /// <summary>
    // Dmessage数据模型
    /// </summary>
    public partial class MMessageNew : MBaseModel
    {

        /// <summary>
        /// 乐观锁
        /// </summary>
        [FieldAttribute("乐观锁")]
        public int Revision { get; set; }

        /// <summary>
        /// id
        /// </summary>
        [FieldAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// 逻辑id
        /// </summary>
        [FieldAttribute("逻辑id")]
        public string LogicalId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [FieldAttribute("三字码")]
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
        public string Sendchannel { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [FieldAttribute("创建人ID")]
        public string CreatedById { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [FieldAttribute("创建人姓名")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        [FieldAttribute("更新人ID")]
        public string UpdatedById { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        [FieldAttribute("更新人姓名")]
        public string UpdatedByName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [FieldAttribute("更新时间")]
        public DateTime UpdatedTime { get; set; }
    }
}
