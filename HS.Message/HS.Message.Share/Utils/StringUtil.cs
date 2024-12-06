using System;
using System.Text;
using System.Text.RegularExpressions;

namespace HS.Message.Share.Utils
{
    /// <summary>
    /// 字符串工具
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                   !objTwoDotPattern.IsMatch(strNumber) &&
                   !objTwoMinusPattern.IsMatch(strNumber) &&
                   objNumberPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>
        /// 是否为int类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        /// <summary>
        /// 是否为无符号数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }
        /// <summary>
        /// 数组转换成字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ArrayToString(string[] values)
        {
            if (null == values) return string.Empty;

            StringBuilder buffer = new StringBuilder(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                buffer.Append(values[i]).Append(",");
            }
            if (buffer.Length > 0)
            {
                return buffer.ToString().Substring(0, buffer.Length - 1);
            }
            return string.Empty;
        }
        /// <summary>
        /// object转string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ObjectToString(object obj, string defaultValue = "")
        {
            string result;
            if (obj == null)
            {
                result = defaultValue;
            }
            else
            {
                string name = obj.GetType().Name;
                switch (name)
                {
                    case "Int32":
                    case "Float":
                    case "Double":
                    case "Decimal":
                        result = obj.ToString();
                        return result;
                    case "DateTime":
                        result = (obj is DateTime ? (DateTime)obj : default).ToString("yyyy-mm-dd HH:MI:ss");
                        return result;
                    case "String":
                        result = string.Concat(obj).Length > 0 ? obj.ToString() : defaultValue;
                        return result;
                }
                result = obj as string;
            }
            return result;
        }

        /// <summary>
        /// 转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(string str)
        {
            return ToInt32(str, 0);
        }

        /// <summary>
        /// 转int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(string str, int defaultValue)
        {
            var value = 0;
            return int.TryParse(str, out value) ? value : defaultValue;
        }
        /// <summary>
        /// String 转Base64 string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBase64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Base64 string 转 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static string Base64ToString(string base64String)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        }
        /// <summary>
        /// 大驼峰转下划线连接
        /// </summary>
        /// <param name="pascalCase"></param>
        /// <returns></returns>
        public static string PascalToSnakeCase(string pascalCase)
        {
            if (string.IsNullOrEmpty(pascalCase))
            {
                return pascalCase;
            }

            return Regex.Replace(
                pascalCase,
                @"([a-z])([A-Z])",
                match => match.Groups[1].Value + "_" + match.Groups[2].Value).ToLower();
        }
    }
}
