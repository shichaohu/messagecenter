using HS.Message.Share.Attributes;
using HS.Message.Share.Extensions;

namespace HS.Message.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public MBaseModel()
        {
            Revision = DateTime.Now.Second;
        }
        /// <summary>
        /// 乐观锁
        /// </summary>
        [Field("乐观锁")]
        public int Revision { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Field("主键")]
        [Excel("主键", 20, 1)]
        public int Id { get; set; }

        /// <summary>
        /// 逻辑id
        /// </summary>
        [Field("逻辑id")]
        [Excel("逻辑id", 20, 10)]
        public string LogicalId { get; set; }


        /// <summary>
        /// 创建人ID
        /// </summary>
        [Field("创建人ID")]
        public string CreatedById { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [Field("创建人姓名")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Field("创建时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        [Field("更新人ID")]
        public string UpdatedById { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        [Field("更新人姓名")]
        public string UpdatedByName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Field("更新时间")]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 创建时间开始
        /// </summary>
        [Field("创建时间")]
        public System.DateTime CreatedTimeStart { get; set; }

        /// <summary>
        /// 创建时间结束
        /// </summary>
        [Field("创建时间")]
        public System.DateTime CreatedTimeEnd { get; set; }

        /// <summary>
        /// 更新时间开始
        /// </summary>
        [Field("更新时间")]
        public System.DateTime UpdatedTimeStart { get; set; }

        /// <summary>
        /// 更新时间结束
        /// </summary>
        [Field("更新时间")]
        public System.DateTime UpdatedTimeEnd { get; set; }

        public string QueryFields { get; set; }

        public string Orderby { get; set; }

        public int IsNotLike { get; set; }

        public string FuzzySearchKeyWord { get; set; }

        public string FuzzySearchFields { get; set; }

        public int IsLanguageHandle { get; set; }

        public string LanguageHandleEcludeField { get; set; }

        public int LimitNum { get; set; }

        public string ExcludeId { get; set; }
    }
}
