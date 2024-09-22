using HS.Message.Share.AutoFill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Utils
{
    public class ModelUtil
    {
        public static object GetPropertyValue(object target, string propertyName)
        {
            return target.GetType().InvokeMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, Type.DefaultBinder, target, null);
        }

        public static T GetPropertyValue<T>(object target, string propertyName)
        {
            object obj = target.GetType().InvokeMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, Type.DefaultBinder, target, null);
            if (obj == null)
            {
                return default(T);
            }

            return (T)obj;
        }

        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            target.GetType().InvokeMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder, target, new object[1] { value });
        }

        public static void AutoFill<T>(T dtos, List<MAutoFillField> autoFillFieldsList = null)
        {
            AutoFill(new List<T> { dtos }, AllAutoFillFieldDataSource.AutoFillFieldDataList, autoFillFieldsList);
        }

        public static void AutoFill<T>(T dtos, List<MAutoFillFieldDataSource> autoFillFieldDataList, List<MAutoFillField> autoFillFieldsList = null)
        {
            AutoFill(new List<T> { dtos }, autoFillFieldDataList, autoFillFieldsList);
        }

        public static void AutoFill<T>(List<T> dtos, List<MAutoFillField> autoFillFieldsList = null)
        {
            AutoFill(dtos, AllAutoFillFieldDataSource.AutoFillFieldDataList, autoFillFieldsList);
        }

        public static void AutoFill<T>(List<T> dtos, List<MAutoFillFieldDataSource> autoFillFieldDataList = null, List<MAutoFillField> autoFillFieldsList = null)
        {
            if (dtos == null || dtos.Count == 0 || autoFillFieldsList == null || autoFillFieldsList.Count <= 0 || autoFillFieldDataList == null || autoFillFieldDataList.Count <= 0)
            {
                return;
            }

            foreach (T dto in dtos)
            {
                if (dto == null)
                {
                    continue;
                }

                foreach (MAutoFillField item in autoFillFieldsList)
                {
                    int key = GetPropertyValue<int>(dto, item.KeyFieldName);
                    MAutoFillFieldDataSource mAutoFillFieldDataSource = autoFillFieldDataList.Find((MAutoFillFieldDataSource x) => x.DataType == item.DataType && x.Key == key);
                    if (mAutoFillFieldDataSource != null && !string.IsNullOrEmpty(mAutoFillFieldDataSource.Value))
                    {
                        SetPropertyValue(dto, item.ValueFieldName, mAutoFillFieldDataSource.Value);
                    }
                    else
                    {
                        SetPropertyValue(dto, item.ValueFieldName, "unknown");
                    }
                }
            }
        }

        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type, nonPublic: true);
        }

        public static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), nonPublic: true);
        }

        public static bool ExistProperty(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties();
            if (properties.Length != 0)
            {
                return properties.Where((PropertyInfo f) => f.Name == propertyName).Any();
            }

            return false;
        }

        public static void Map(object from, object to, IEnumerable<string> excludeNames = null, Dictionary<string, string> mapNames = null)
        {
            IEnumerable<string> enumerable2;
            if (excludeNames == null)
            {
                IEnumerable<string> enumerable = new List<string>();
                enumerable2 = enumerable;
            }
            else
            {
                enumerable2 = excludeNames;
            }

            excludeNames = enumerable2;
            mapNames = ((mapNames != null) ? mapNames : new Dictionary<string, string>());
            PropertyInfo[] properties = from.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!excludeNames.Contains(propertyInfo.Name))
                {
                    if (ExistProperty(to, propertyInfo.Name))
                    {
                        SetPropertyValue(to, propertyInfo.Name, propertyInfo.GetValue(from));
                    }
                    else if (mapNames.ContainsKey(propertyInfo.Name) && ExistProperty(to, mapNames[propertyInfo.Name]))
                    {
                        SetPropertyValue(to, mapNames[propertyInfo.Name], propertyInfo.GetValue(from));
                    }
                }
            }
        }

        public static List<string> GetPropertyNames<T>()
        {
            return (from p in typeof(T).GetProperties()
                    select p.Name).ToList();
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos<TDestination>()
        {
            return typeof(TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
