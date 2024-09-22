using Newtonsoft.Json;

namespace HS.Message.Share.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ValueToObjectUtils
    {
        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        /// <summary>
        /// 将字符串序列化为json
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToJson(this object str)
        {
            if (str is string) return str.ToString();
            return JsonConvert.SerializeObject(str);
        }
    }

}
