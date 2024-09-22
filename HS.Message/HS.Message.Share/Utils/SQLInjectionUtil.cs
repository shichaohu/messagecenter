using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HS.Message.Share.Utils
{
    public static class SQLInjectionUtil
    {
        private static string _regexString;

        private static string regexString
        {
            get
            {
                if (string.IsNullOrEmpty(_regexString))
                {
                    _regexString = GetRegexString();
                }

                return _regexString;
            }
        }

        private static string GetRegexString()
        {
            string[] array = new string[22]
            {
            "and ", "<script>", "exec ", "insert ", "select ", "delete ", "update ", "from ", "drop ", "char ",
            " or ", "%", "\"", "master ", "truncate ", "declare ", "SiteName", "net user", "xp_cmdshell", "/add",
            "exec master.dbo.xp_cmdshell", "net localgroup administrators"
            };
            string text = ".*(";
            for (int i = 0; i < array.Length - 1; i++)
            {
                text = text + array[i] + "|";
            }

            return text + array[^1] + ").*";
        }

        public static void CheckSQLInjection(this string value)
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, regexString))
            {
                return;
            }

            throw new Exception("非法请求");
        }
    }
}
