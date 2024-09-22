using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    public class MPageQueryCondition<T> : MPageInfo
    {
        public T condition { get; set; }
    }
}
