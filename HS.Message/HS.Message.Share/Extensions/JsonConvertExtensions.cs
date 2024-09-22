using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Extensions
{
    public static class JsonConvertExtensions
    {

        public static T DeserializeObject<T>(string result)
        {
            return JsonConvert.DeserializeObject<T>(result, new JsonConverter[1]
            {
            new JsonConverterExtendsion()
            });
        }

        public static string SerializeObject<T>(T result)
        {
            return JsonConvert.SerializeObject(result, new JsonConverterExtendsion());
        }

        public static string SerializeObject(object result)
        {
            return JsonConvert.SerializeObject(result, new JsonConverterExtendsion());
        }
    }
}
