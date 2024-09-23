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
    // SmsMessageDetails数据模型
    /// </summary>
    public partial class MSmsMessageDetails: MBaseModel
    {
        
        /// <summary>
        // 乐观锁
        /// </summary>
        private int? _REVISION;
        
        /// <summary>
        // 短信消息主id
        /// </summary>
        private string _sms_message_id;
        
        /// <summary>
        // 短息渠道编号;1：阿里云
        /// </summary>
        private int? _channel_code;
        
        /// <summary>
        // 短息渠道名称;1：阿里云
        /// </summary>
        private string _channel_name;
        
        /// <summary>
        // 短息内容
        /// </summary>
        private string _content;
        
        /// <summary>
        // 接收手机号码;不同的手机号码直接通过,连接
        /// </summary>
        private string _phone_number;
        
        /// <summary>
        // 已发送次数
        /// </summary>
        private int? _has_send_num;
        
        /// <summary>
        // 发送状态;1：发送锁定（代表正在发送），2：已发送 （代表已经将短信发送到运营商），3：发送成功（运营商返回成功），4：发送失败（运营商返回失败）
        /// </summary>
        private int _send_state;
        
        /// <summary>
        // 运营商状态编码
        /// </summary>
        private string _operator_state_code;
        
        /// <summary>
        // 锁定时间
        /// </summary>
        private System.DateTime _lock_time;
        
        /// <summary>
        // 提交时间;我方提交短信平台时间
        /// </summary>
        private System.DateTime _submit_time;
        
        /// <summary>
        // 接收时间;用户查看时间
        /// </summary>
        private System.DateTime _receive_time;
        
        /// <summary>
        // 上一次发送时间
        /// </summary>
        private System.DateTime _last_send_time;
        
        /// <summary>
        // 发送回执ID
        /// </summary>
        private string _biz_id;
        
        /// <summary>
        // 请求id
        /// </summary>
        private string _request_id;
        
        /// <summary>
        // 查询锁定状态;1：锁定，2：未锁定
        /// </summary>
        private int? _query_lock_state;
        
        /// <summary>
        // 查询锁定时间
        /// </summary>
        private System.DateTime _query_lock_time;
        
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
        /// 短信消息主id
        /// </summary>
        [FieldAttribute("短信消息主id")]
        public string sms_message_id
        {
            get
            {
                return this._sms_message_id;
            }
            set
            {
                this._sms_message_id = value;
            }
        }
        
        /// <summary>
        /// 短息渠道编号;1：阿里云
        /// </summary>
        [FieldAttribute("短息渠道编号;1：阿里云")]
        public int? channel_code
        {
            get
            {
                return this._channel_code;
            }
            set
            {
                this._channel_code = value;
            }
        }
        
        /// <summary>
        /// 短息渠道名称;1：阿里云
        /// </summary>
        [FieldAttribute("短息渠道名称;1：阿里云")]
        public string channel_name
        {
            get
            {
                return this._channel_name;
            }
            set
            {
                this._channel_name = value;
            }
        }
        
        /// <summary>
        /// 短息内容
        /// </summary>
        [FieldAttribute("短息内容")]
        public string content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }
        
        /// <summary>
        /// 接收手机号码;不同的手机号码直接通过,连接
        /// </summary>
        [FieldAttribute("接收手机号码;不同的手机号码直接通过,连接")]
        public string phone_number
        {
            get
            {
                return this._phone_number;
            }
            set
            {
                this._phone_number = value;
            }
        }
        
        /// <summary>
        /// 已发送次数
        /// </summary>
        [FieldAttribute("已发送次数")]
        public int? has_send_num
        {
            get
            {
                return this._has_send_num;
            }
            set
            {
                this._has_send_num = value;
            }
        }
        
        /// <summary>
        /// 发送状态;1：发送锁定（代表正在发送），2：已发送 （代表已经将短信发送到运营商），3：发送成功（运营商返回成功），4：发送失败（运营商返回失败）
        /// </summary>
        [FieldAttribute("发送状态;1：发送锁定（代表正在发送），2：已发送 （代表已经将短信发送到运营商），3：发送成功（运营商返回成功），4：发送失败（运营商返回失败）")]
        public int send_state
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
        /// 运营商状态编码
        /// </summary>
        [FieldAttribute("运营商状态编码")]
        public string operator_state_code
        {
            get
            {
                return this._operator_state_code;
            }
            set
            {
                this._operator_state_code = value;
            }
        }
        
        /// <summary>
        /// 锁定时间
        /// </summary>
        [FieldAttribute("锁定时间")]
        public System.DateTime lock_time
        {
            get
            {
                return this._lock_time;
            }
            set
            {
                this._lock_time = value;
            }
        }
        
        /// <summary>
        /// 提交时间;我方提交短信平台时间
        /// </summary>
        [FieldAttribute("提交时间;我方提交短信平台时间")]
        public System.DateTime submit_time
        {
            get
            {
                return this._submit_time;
            }
            set
            {
                this._submit_time = value;
            }
        }
        
        /// <summary>
        /// 接收时间;用户查看时间
        /// </summary>
        [FieldAttribute("接收时间;用户查看时间")]
        public System.DateTime receive_time
        {
            get
            {
                return this._receive_time;
            }
            set
            {
                this._receive_time = value;
            }
        }
        
        /// <summary>
        /// 上一次发送时间
        /// </summary>
        [FieldAttribute("上一次发送时间")]
        public System.DateTime last_send_time
        {
            get
            {
                return this._last_send_time;
            }
            set
            {
                this._last_send_time = value;
            }
        }
        
        /// <summary>
        /// 发送回执ID
        /// </summary>
        [FieldAttribute("发送回执ID")]
        public string biz_id
        {
            get
            {
                return this._biz_id;
            }
            set
            {
                this._biz_id = value;
            }
        }
        
        /// <summary>
        /// 请求id
        /// </summary>
        [FieldAttribute("请求id")]
        public string request_id
        {
            get
            {
                return this._request_id;
            }
            set
            {
                this._request_id = value;
            }
        }
        
        /// <summary>
        /// 查询锁定状态;1：锁定，2：未锁定
        /// </summary>
        [FieldAttribute("查询锁定状态;1：锁定，2：未锁定")]
        public int? query_lock_state
        {
            get
            {
                return this._query_lock_state;
            }
            set
            {
                this._query_lock_state = value;
            }
        }
        
        /// <summary>
        /// 查询锁定时间
        /// </summary>
        [FieldAttribute("查询锁定时间")]
        public System.DateTime query_lock_time
        {
            get
            {
                return this._query_lock_time;
            }
            set
            {
                this._query_lock_time = value;
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
