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
    // WxTemplate 条件查询扩展模型
    /// </summary>
    public class MWxTemplateCondtion:MWxTemplate
    {
        
        /// <summary>
        // 创建时间
        /// </summary>
        private System.DateTime _created_time_start;
        
        /// <summary>
        // 创建时间
        /// </summary>
        private System.DateTime _created_time_end;
        
        /// <summary>
        // 更新时间
        /// </summary>
        private System.DateTime _updated_time_start;
        
        /// <summary>
        // 更新时间
        /// </summary>
        private System.DateTime _updated_time_end;
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("创建时间")]
        public System.DateTime created_time_start
        {
            get
            {
                return this._created_time_start;
            }
            set
            {
                this._created_time_start = value;
            }
        }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("创建时间")]
        public System.DateTime created_time_end
        {
            get
            {
                return this._created_time_end;
            }
            set
            {
                this._created_time_end = value;
            }
        }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [FieldAttribute("更新时间")]
        public System.DateTime updated_time_start
        {
            get
            {
                return this._updated_time_start;
            }
            set
            {
                this._updated_time_start = value;
            }
        }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [FieldAttribute("更新时间")]
        public System.DateTime updated_time_end
        {
            get
            {
                return this._updated_time_end;
            }
            set
            {
                this._updated_time_end = value;
            }
        }
    }
}
