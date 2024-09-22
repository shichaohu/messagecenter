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
    // WxTemplate数据模型
    /// </summary>
    public partial class MWxTemplate: MBaseModel
    {
        
        /// <summary>
        // 主键
        /// </summary>
        private string _id;
        
        /// <summary>
        // 模板名称
        /// </summary>
        private string _temp_name;
        
        /// <summary>
        // 模板内容 邮件内容模板通过{key}来进行模板参数占位
        /// </summary>
        private string _temp_body;
        
        /// <summary>
        // 模板邮件标题 邮件标题模板通过{key}来进行模板参数占位
        /// </summary>
        private string _temp_title;
        
        /// <summary>
        // 邮件发送信息配置id
        /// </summary>
        private string _mail_configuer_id;
        
        /// <summary>
        // 状态 1：启用
        //2：禁用
        //默认为1
        /// </summary>
        private int? _state;
        
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
        /// 主键
        /// </summary>
        [FieldAttribute("主键")]
        [ExcelAttribute("主键", 20, 10)]
        public string id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [FieldAttribute("模板名称")]
        [ExcelAttribute("模板名称", 20, 10)]
        public string temp_name
        {
            get
            {
                return this._temp_name;
            }
            set
            {
                this._temp_name = value;
            }
        }
        
        /// <summary>
        /// 模板内容 邮件内容模板通过{key}来进行模板参数占位
        /// </summary>
        [FieldAttribute("模板内容 邮件内容模板通过{key}来进行模板参数占位")]
        [ExcelAttribute("模板内容 邮件内容模板通过{key}来进行模板参数占位", 20, 10)]
        public string temp_body
        {
            get
            {
                return this._temp_body;
            }
            set
            {
                this._temp_body = value;
            }
        }
        
        /// <summary>
        /// 模板邮件标题 邮件标题模板通过{key}来进行模板参数占位
        /// </summary>
        [FieldAttribute("模板邮件标题 邮件标题模板通过{key}来进行模板参数占位")]
        [ExcelAttribute("模板邮件标题 邮件标题模板通过{key}来进行模板参数占位", 20, 10)]
        public string temp_title
        {
            get
            {
                return this._temp_title;
            }
            set
            {
                this._temp_title = value;
            }
        }
        
        /// <summary>
        /// 邮件发送信息配置id
        /// </summary>
        [FieldAttribute("邮件发送信息配置id")]
        [ExcelAttribute("邮件发送信息配置id", 20, 10)]
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
        /// 状态 1：启用
        ///2：禁用
        ///默认为1
        /// </summary>
        [FieldAttribute("状态 1：启用 2：禁用 默认为1")]
        [ExcelAttribute("状态 1：启用 2：禁用 默认为1", 20, 10)]
        public int? state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
        
        /// <summary>
        /// 创建人Id
        /// </summary>
        [FieldAttribute("创建人Id")]
        [ExcelAttribute("创建人Id", 20, 10)]
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
        [ExcelAttribute("创建人姓名", 20, 10)]
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
        [ExcelAttribute("创建时间", 20, 10)]
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
        [ExcelAttribute("更新人Id", 20, 10)]
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
        [ExcelAttribute("更新人姓名", 20, 10)]
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
        [ExcelAttribute("更新时间", 20, 10)]
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