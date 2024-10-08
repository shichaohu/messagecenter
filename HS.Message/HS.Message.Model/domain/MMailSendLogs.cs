//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HS.Message.Share.Attributes;

namespace HS.Message.Model
{


    /// <summary>
    // MailSendLogs数据模型
    /// </summary>
    public partial class MMailSendLogs: MBaseModel
    {
        
        /// <summary>
        // 乐观锁
        /// </summary>
        private int? _REVISION;

        /// <summary>
        // 邮件消息Id
        /// </summary>
        private string _mail_message_id;
        
        /// <summary>
        // 邮件标题
        /// </summary>
        private string _mail_title;
        
        /// <summary>
        // 邮件内容
        /// </summary>
        private string _mail_body;
        
        /// <summary>
        // 邮件发送信息配置id
        /// </summary>
        private string _mail_configuer_id;
        
        /// <summary>
        // 收件邮箱;支持多个邮件一起发送，不同的接受邮件地址间通过“,”连接起来
        /// </summary>
        private string _receiver_email;
        
        /// <summary>
        // 抄送邮箱
        /// </summary>
        private string _receiver_cc_email;
        
        /// <summary>
        // 发送时间
        /// </summary>
        private System.DateTime _send_time;
        
        /// <summary>
        // 发送结果状态;1：发送成功，2：发送失败，默认为1
        /// </summary>
        private int? _send_state;
        
        /// <summary>
        // 发送结果
        /// </summary>
        private string _send_result;
        
        /// <summary>
        // 创建人Id
        /// </summary>
        private string _created_by_id;
        
        /// <summary>
        // 创建人姓名
        /// </summary>
        private string _created_by_name;
        
        /// <summary>
        // 创建时间
        /// </summary>
        private System.DateTime _created_time;
        
        /// <summary>
        // 更新人Id
        /// </summary>
        private string _updated_by_id;
        
        /// <summary>
        // 更新人姓名
        /// </summary>
        private string _updated_by_name;
        
        /// <summary>
        // 更新时间
        /// </summary>
        private System.DateTime _updated_time;
        
        /// <summary>
        /// 乐观锁
        /// </summary>
        [FieldAttribute("乐观锁")]
        public int? REVISION
        {
            get
            {
                return this._REVISION;
            }
            set
            {
                this._REVISION = value;
            }
        }

        /// <summary>
        /// 邮件消息Id
        /// </summary>
        [FieldAttribute("邮件消息Id")]
        public string mail_message_id
        {
            get
            {
                return this._mail_message_id;
            }
            set
            {
                this._mail_message_id = value;
            }
        }
        
        /// <summary>
        /// 邮件标题
        /// </summary>
        [FieldAttribute("邮件标题")]
        public string mail_title
        {
            get
            {
                return this._mail_title;
            }
            set
            {
                this._mail_title = value;
            }
        }
        
        /// <summary>
        /// 邮件内容
        /// </summary>
        [FieldAttribute("邮件内容")]
        public string mail_body
        {
            get
            {
                return this._mail_body;
            }
            set
            {
                this._mail_body = value;
            }
        }
        
        /// <summary>
        /// 邮件发送信息配置id
        /// </summary>
        [FieldAttribute("邮件发送信息配置id")]
        public string mail_configuer_id
        {
            get
            {
                return this._mail_configuer_id;
            }
            set
            {
                this._mail_configuer_id = value;
            }
        }
        
        /// <summary>
        /// 收件邮箱;支持多个邮件一起发送，不同的接受邮件地址间通过“,”连接起来
        /// </summary>
        [FieldAttribute("收件邮箱;支持多个邮件一起发送，不同的接受邮件地址间通过“,”连接起来")]
        public string receiver_email
        {
            get
            {
                return this._receiver_email;
            }
            set
            {
                this._receiver_email = value;
            }
        }
        
        /// <summary>
        /// 抄送邮箱
        /// </summary>
        [FieldAttribute("抄送邮箱")]
        public string receiver_cc_email
        {
            get
            {
                return this._receiver_cc_email;
            }
            set
            {
                this._receiver_cc_email = value;
            }
        }
        
        /// <summary>
        /// 发送时间
        /// </summary>
        [FieldAttribute("发送时间")]
        public System.DateTime send_time
        {
            get
            {
                return this._send_time;
            }
            set
            {
                this._send_time = value;
            }
        }
        
        /// <summary>
        /// 发送结果状态;1：发送成功，2：发送失败，默认为1
        /// </summary>
        [FieldAttribute("发送结果状态;1：发送成功，2：发送失败，默认为1")]
        public int? send_state
        {
            get
            {
                return this._send_state;
            }
            set
            {
                this._send_state = value;
            }
        }
        
        /// <summary>
        /// 发送结果
        /// </summary>
        [FieldAttribute("发送结果")]
        public string send_result
        {
            get
            {
                return this._send_result;
            }
            set
            {
                this._send_result = value;
            }
        }
        
        /// <summary>
        /// 创建人Id
        /// </summary>
        [FieldAttribute("创建人Id")]
        public string created_by_id
        {
            get
            {
                return this._created_by_id;
            }
            set
            {
                this._created_by_id = value;
            }
        }
        
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [FieldAttribute("创建人姓名")]
        public string created_by_name
        {
            get
            {
                return this._created_by_name;
            }
            set
            {
                this._created_by_name = value;
            }
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("创建时间")]
        public System.DateTime created_time
        {
            get
            {
                return this._created_time;
            }
            set
            {
                this._created_time = value;
            }
        }
        
        /// <summary>
        /// 更新人Id
        /// </summary>
        [FieldAttribute("更新人Id")]
        public string updated_by_id
        {
            get
            {
                return this._updated_by_id;
            }
            set
            {
                this._updated_by_id = value;
            }
        }
        
        /// <summary>
        /// 更新人姓名
        /// </summary>
        [FieldAttribute("更新人姓名")]
        public string updated_by_name
        {
            get
            {
                return this._updated_by_name;
            }
            set
            {
                this._updated_by_name = value;
            }
        }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [FieldAttribute("更新时间")]
        public System.DateTime updated_time
        {
            get
            {
                return this._updated_time;
            }
            set
            {
                this._updated_time = value;
            }
        }
    }
}
