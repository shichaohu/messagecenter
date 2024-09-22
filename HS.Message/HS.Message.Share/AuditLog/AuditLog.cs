using HS.Message.Share.Attributes;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.AuditLog
{
    public class AuditLog<T>
    {
        public string GeteChangeAuditLog(T oldModel, T newModel)
        {
            if (oldModel == null)
            {
                return GeteChangeAuditLog(newModel);
            }

            StringBuilder stringBuilder = new StringBuilder();
            Type type = oldModel.GetType();
            newModel.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object valueOld = propertyInfo.GetValue(oldModel, null);
                string name = propertyInfo.Name;
                PropertyInfo property = typeof(T).GetProperty(name);
                object valueNew = property.GetValue(newModel, null);
                bool flag = CheckObjIsNull(valueOld);
                bool flag2 = CheckObjIsNull(valueNew);
                if (flag && flag2)
                {
                    continue;
                }

                if (!flag && !flag2)
                {
                    if (valueOld.GetType().IsValueType)
                    {
                        if (valueOld == valueNew || valueOld.ToString() == valueNew.ToString())
                        {
                            continue;
                        }
                    }
                    else if (JsonConvertExtensions.SerializeObject(valueOld) == JsonConvertExtensions.SerializeObject(valueNew))
                    {
                        continue;
                    }
                }

                if (valueOld == valueNew)
                {
                    continue;
                }

                object[] customAttributes = propertyInfo.GetCustomAttributes(inherit: true);
                for (int j = 0; j < customAttributes.Length; j++)
                {
                    Attribute attribute = (Attribute)customAttributes[j];
                    if (!(attribute.GetType() == typeof(FieldAttribute)))
                    {
                        continue;
                    }

                    FieldAttribute fieldAttribute = (FieldAttribute)attribute;
                    if (!string.IsNullOrEmpty(fieldAttribute.EnumRelation))
                    {
                        List<MKeyValue> list = JsonConvertExtensions.DeserializeObject<List<MKeyValue>>(fieldAttribute.EnumRelation);
                        MKeyValue mKeyValue = list.Find((MKeyValue x) => x.key == valueNew.ToString());
                        if (mKeyValue != null)
                        {
                            valueNew = mKeyValue.value;
                        }

                        mKeyValue = list.Find((MKeyValue x) => x.key == valueOld.ToString());
                        if (mKeyValue != null)
                        {
                            valueOld = mKeyValue.value;
                        }
                    }

                    stringBuilder.Append(string.Format("{0} Change from 【{1}】 To 【{2}】；", fieldAttribute.Name, flag ? "" : valueOld, flag2 ? "" : valueNew));
                }
            }

            return stringBuilder.ToString();
        }

        public string GeteChangeAuditLog(T model)
        {
            StringBuilder stringBuilder = new StringBuilder();
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(model, null);
                if (CheckObjIsNull(value))
                {
                    continue;
                }

                _ = propertyInfo.Name;
                object[] customAttributes = propertyInfo.GetCustomAttributes(inherit: true);
                for (int j = 0; j < customAttributes.Length; j++)
                {
                    Attribute attribute = (Attribute)customAttributes[j];
                    if (!(attribute.GetType() == typeof(FieldAttribute)))
                    {
                        continue;
                    }

                    FieldAttribute fieldAttribute = (FieldAttribute)attribute;
                    if (!string.IsNullOrEmpty(fieldAttribute.EnumRelation))
                    {
                        MKeyValue mKeyValue = JsonConvertExtensions.DeserializeObject<List<MKeyValue>>(fieldAttribute.EnumRelation).Find((MKeyValue x) => x.key == value.ToString());
                        if (mKeyValue != null)
                        {
                            value = mKeyValue.value;
                        }
                    }

                    stringBuilder.Append($"{((FieldAttribute)attribute).Name} :【{value}】；");
                }
            }

            return stringBuilder.ToString();
        }

        public string GeteChangeAuditLog<T_NUM>(T model, MBactchUpdateSpecifyFields<T_NUM> bactchUpdateSpecifyFields)
        {
            StringBuilder stringBuilder = new StringBuilder("The batch update field primary key is:" + JsonConvert.SerializeObject(bactchUpdateSpecifyFields.idList) + "。The details of batch modified data is ：");
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                string name = propertyInfo.Name;
                if (!bactchUpdateSpecifyFields.updateFieldsValue.ContainsKey(name))
                {
                    continue;
                }

                string arg = name;
                object[] customAttributes = propertyInfo.GetCustomAttributes(inherit: true);
                for (int j = 0; j < customAttributes.Length; j++)
                {
                    Attribute attribute = (Attribute)customAttributes[j];
                    if (attribute.GetType() == typeof(FieldAttribute))
                    {
                        arg = ((FieldAttribute)attribute).Name;
                    }
                }

                stringBuilder.Append($"{arg} change to：{bactchUpdateSpecifyFields.updateFieldsValue[name]}；");
            }

            return stringBuilder.ToString();
        }

        private bool CheckObjIsNull(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value.GetType() == typeof(string) && (string.IsNullOrEmpty(value.ToString()) || value.ToString() == "0001-01-01T00:00:00" || value.ToString() == "0001-01-01 00:00:00"))
            {
                return true;
            }

            if (value.GetType() == typeof(DateTime) && ((DateTime)value == DateTime.MaxValue || (DateTime)value == DateTime.MinValue))
            {
                return true;
            }

            return false;
        }
    }
}
