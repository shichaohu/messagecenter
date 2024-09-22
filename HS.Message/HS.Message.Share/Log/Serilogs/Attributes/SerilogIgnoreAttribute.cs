using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Log.Serilogs.Attributes
{
    /// <summary>
    /// 忽略日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SerilogIgnoreAttribute : Attribute
    {
        public SerilogIgnoreAttribute()
        {
        }

    }
}
