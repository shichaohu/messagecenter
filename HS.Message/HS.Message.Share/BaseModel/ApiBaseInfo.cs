using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    /// <summary>
    /// 接口基础信息
    /// </summary>
    public class ApiBaseInfo
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
        public string Controller { get; set; }
        /// <summary>
        /// controller 注释
        /// </summary>
        public string ControllerSummary { get; set; }
        public string Action { get; set; }
        /// <summary>
        /// action route template
        /// </summary>
        public string ActionRouteTemplate { get; set; }
        /// <summary>
        /// action 注释
        /// </summary>
        public string ActionSummary { get; set; }
        
    }
}
