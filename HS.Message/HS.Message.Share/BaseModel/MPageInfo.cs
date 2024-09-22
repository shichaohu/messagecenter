using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.BaseModel
{
    public class MPageInfo
    {
        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public int total { get; set; }

        public string orderby { get; set; }
    }
}
