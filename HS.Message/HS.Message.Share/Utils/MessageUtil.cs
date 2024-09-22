using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Utils
{
    /// <summary>
    /// 消息工具
    /// </summary>
    public static class MessageUtil
    {
        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="input">输入的文字内容</param>
        /// <param name="template">模板中的内容</param>
        /// <param name="dynamicParameter">输入的动态参数</param>
        /// <returns></returns>
        public static string LoadMessageTemplate(string input, string template, Dictionary<string, string> dynamicParameter)
        {
            string result = input ?? template;
            if (dynamicParameter != null && dynamicParameter.Keys?.Count > 0)
            {
                result = template;
                foreach (var param in dynamicParameter)
                {
                    result = result.Replace($"{{{param.Key}}}", param.Value);
                }
            }

            return result;
        }
    }
}
