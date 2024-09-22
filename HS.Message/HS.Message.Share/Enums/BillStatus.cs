using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Enums
{
    /// <summary>
    /// 单据状态（流转）
    /// </summary>
    public enum BillStatus
    {
        /// <summary>
        /// 未提交
        /// </summary>
        [Description("未提交")]
        uncommitted=10,
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        unaudited =20,
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        approved =30,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        rejected =40,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        completed =100
    }
}
