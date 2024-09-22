using HS.Message.Share.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Extensions
{
    public class JsonConverterExtendsion : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType.Name.ToUpper() == "DATETIME")
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                DateTime dateTime = Convert.ToDateTime(value);
                if (dateTime.IsEffectiveDateTime())
                {
                    writer.WriteValue(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    writer.WriteValue(string.Empty);
                }
            }
            catch (Exception)
            {
                writer.WriteValue(string.Empty);
            }
        }
    }
}
