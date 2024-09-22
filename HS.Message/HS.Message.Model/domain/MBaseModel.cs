using HS.Message.Share.Attributes;

namespace HS.Message.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MBaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field("主键")]
        [Excel("主键", 20, 1)]
        public int id { get; set; }

        /// <summary>
        /// 逻辑id
        /// </summary>
        [Field("逻辑id")]
        [Excel("逻辑id", 20, 10)]
        public string logical_id { get; set; }


        public string queryFields { get; set; }

        public string orderby { get; set; }

        public int isNotLike { get; set; }

        public string fuzzySearchKeyWord { get; set; }

        public string fuzzySearchFields { get; set; }

        public int isLanguageHandle { get; set; }

        public string languageHandleEcludeField { get; set; }

        public int limitNum { get; set; }

        public string excludeId { get; set; }
    }
}
