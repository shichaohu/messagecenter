using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Enums
{
    /// <summary>
    /// 单据操作
    /// </summary>
    public enum BillOperation
    {
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        commit = 10,
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
        done =100
    }
}
