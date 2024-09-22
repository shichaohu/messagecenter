using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    public class MBactchUpdateSpecifyFields<T>
    {
        public List<T> idList { get; set; }

        public Dictionary<string, object> updateFieldsValue { get; set; }
    }
}
