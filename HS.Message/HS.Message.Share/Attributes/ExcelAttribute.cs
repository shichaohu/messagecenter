using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Attributes
{
    public class ExcelAttribute : Attribute
    {
        public string title { get; set; }

        public int width { get; set; }

        public int sort { get; set; }

        public string attributeName { get; set; }

        public string joinValue { get; set; }

        public List<string> attributeNameList { get; set; }

        public string enumRelation { get; set; }

        public string dataType { get; set; }

        public string format { get; set; }

        public ExcelAttribute()
        {
        }

        public ExcelAttribute(string title)
        {
            width = 20;
            sort = 10;
            this.title = title;
        }

        public ExcelAttribute(string title, string enumRelation)
        {
            width = 20;
            sort = 10;
            this.title = title;
            this.enumRelation = enumRelation;
        }

        public ExcelAttribute(string title, int width, int sort)
        {
            this.title = title;
            this.width = width;
            this.sort = sort;
        }

        public ExcelAttribute(string title, int width, int sort, string enumRelation)
        {
            this.title = title;
            this.width = width;
            this.sort = sort;
            this.enumRelation = enumRelation;
        }
    }
}
