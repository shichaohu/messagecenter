using System.Collections;
using System.Reflection;

namespace HS.Message.Share.Utils
{
    /// <summary>
    /// 拷贝帮助类
    /// 有异常直接抛出
    /// </summary>
    public static class CopyHelper
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static TDestination Copy<TSource, TDestination>(this TSource sourceObj)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var destinationObj = new DeepCopyByReflect().DoCopy(sourceObj, typeof(TDestination)) as TDestination;
            return destinationObj;
        }
        /// <summary>
        /// 复制list
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static List<TDestination> CopyList<TSource, TDestination>(this List<TSource> sourceObj)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var destinationObj = new DeepCopyByReflect().DoCopy(sourceObj, typeof(List<TDestination>)) as List<TDestination>;
            return destinationObj;
        }
        /// <summary>
        /// CopyAs函数，Copy函数的简化版。可简写源类型，但同时带来入侵风险
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObj"></param>
        /// <returns></returns>
        public static TDestination CopyAs<TDestination>(this object sourceObj)
            where TDestination : class, new()
        {
            if (sourceObj is Task)
            {
                throw new Exception("copy:type error");
            }
            var destinationObj = new DeepCopyByReflect().DoCopy(sourceObj, typeof(TDestination)) as TDestination;
            return destinationObj;
        }
        /// <summary>
        /// CopyAs函数，Copy函数的简化版。可简写源类型，但同时带来入侵风险
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObj"></param>
        /// <param name="sourceType">目前只为占位用</param>
        /// <param name="copyEvent">copy中的事件</param>
        /// <returns></returns>
        public static TDestination CopyAs<TDestination>(this object sourceObj, Type sourceType, CopyEvent copyEvent)
            where TDestination : class, new()
        {
            if (sourceObj is Task)
            {
                throw new Exception("copy:type error");
            }
            var destinationObj = new DeepCopyByReflect().DoCopy(sourceObj, sourceType, copyEvent) as TDestination;
            return destinationObj;
        }

        /// <summary>
        /// CopyAs函数，Copy函数的简化版。可简写源类型，但同时带来入侵风险
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="sourceObj"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static TDestination CopyAs<TDestination>(this object sourceObj, List<(Type sourceType, Type targetType)> mapper)
            where TDestination : class, new()
        {
            if (sourceObj is Task)
            {
                throw new Exception("copy:type error");
            }
            if (mapper == null || mapper.Count == 0)
            {
                return sourceObj.CopyAs<TDestination>();
            }
            else
            {
                var copeEvent = new CopyEvent()
                {
                    //当获取类型时，在mapper中查找
                    OnGetDestinationType = (sourceObjType, destinationObjType, souObj) =>
                    {
                        var mapTargetTpye = mapper?.FirstOrDefault(x => x.sourceType.Name == souObj.GetType().Name);
                        if (mapTargetTpye != null && mapTargetTpye != (null, null))
                        {
                            return mapTargetTpye.Value.targetType;
                        }
                        return null;
                    },
                    //当获取属性类型时，在mapper中查找
                    OnGetPropDestinationType = (souProObjType, desProObjType, souObj, sourParentObj) =>
                    {
                        Type newPropTargetType = null;
                        var proMmapTargetTpye = mapper?.FirstOrDefault(x =>
                        x.targetType.Name == desProObjType.Name ||
                        x.targetType.IsAssignableTo(desProObjType)
                        );

                        if (proMmapTargetTpye != null && proMmapTargetTpye != (null, null))
                        {
                            newPropTargetType = proMmapTargetTpye.Value.targetType;
                        }
                        return newPropTargetType;
                    }
                };
                return new DeepCopyByReflect().DoCopy(sourceObj, typeof(TDestination), copeEvent) as TDestination;
            }
        }

        /// <summary>
        /// 深拷贝帮助类，注意循环引用问题
        /// </summary>
        public class DeepCopyByReflect
        {
            //存储已经拷贝过的类 
            Dictionary<object, object> dict = new Dictionary<object, object>();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sourceObj">源值</param>
            /// <param name="destinationType">目标值类型</param>
            /// <param name="copyEvent">copy事件</param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public object DoCopy(
                object sourceObj,
                Type destinationType,
                CopyEvent copyEvent = null
                )
            {
                if (sourceObj == null)
                {
                    return null;
                }

                var sourceObjType = sourceObj.GetType();//获取源对象的类型
                var destinationObjType = destinationType;//获取目的对象的类型
                //用于递归时，修改创建实例的类型
                var newDestinationObjType = copyEvent?.OnGetDestinationType?.Invoke(sourceObjType, destinationObjType, sourceObj);
                if (newDestinationObjType != null)
                {
                    destinationObjType = newDestinationObjType;
                }

                if (sourceObj is string || sourceObjType.IsValueType)
                {
                    return GetValueofDestinationType(sourceObj, sourceObjType, destinationObjType);
                }
                //Include List and Dictionary and......
                else if (typeof(IEnumerable).IsAssignableFrom(sourceObjType))
                {
                    //当源对象为字典时，单独处理
                    if (sourceObjType.IsGenericType && typeof(IDictionary).IsAssignableFrom(sourceObjType))
                    {
                        if (typeof(IDictionary).IsAssignableFrom(destinationObjType))
                        {
                            throw new Exception("copy:not dictionary");
                        }
                        //todo 处理IDictionary相关对象拷贝
                        throw new Exception("copy:unsupported dictionary");
                    }
                    else
                    {
                        var sourceEnumerable = sourceObj as IEnumerable;
                        //Array Type
                        if (destinationObjType.IsArray)
                        {
                            //获取源集合长度
                            var length = GetIEnumerableLength(sourceEnumerable);
                            //获取源类型
                            Type elementType = destinationObjType.GetElementType();
                            //创建数组实例
                            var destinationArray = Array.CreateInstance(elementType, length);
                            //拷贝 
                            int i = 0;
                            foreach (var item in sourceEnumerable)
                            {
                                var copyValue = DoCopy(item, elementType, copyEvent);
                                destinationArray.SetValue(copyValue, i++);
                            }
                            return destinationArray;
                        }
                        //Common IList type
                        else
                        {
                            //获取源类型
                            Type elementType = destinationObjType.GenericTypeArguments[0];
                            //创建IList实例
                            var destinationEnumerable = CreateInstance(destinationObjType) as IList;
                            //拷贝
                            foreach (var item in sourceEnumerable)
                            {
                                var copyValue = DoCopy(item, elementType, copyEvent);
                                destinationEnumerable.Add(copyValue);
                            }
                            return destinationEnumerable;
                        }
                    }
                }
                else
                {
                    //引用类型 
                    //存在字典中的键直接返回当前 Type 的所有公共字段。 
                    if (dict.ContainsKey(sourceObj))
                    {
                        return dict[sourceObj];
                    }
                    object retval = CreateInstance(destinationObjType);
                    dict.Add(sourceObj, retval);

                    //下面是对象属性拷贝
                    PropertyInfo[] sourcePropertys = sourceObjType.GetProperties();//获取源对象的属性
                    PropertyInfo[] destinationPropertys = destinationObjType.GetProperties();//获取目的对象的属性
                    Type souProType, desProType; // 源属性类型，目的属性类型
                    foreach (var desPro in destinationPropertys)
                    {
                        //如果目标属性无set权限，放弃对他的操作
                        if (desPro.CanWrite == false)
                        {
                            continue;
                        }
                        foreach (var souPro in sourcePropertys)
                        {
                            //属性名相同时，进行操作
                            if (souPro.Name == desPro.Name)
                            {
                                souProType = souPro.PropertyType;//得到变量的类型
                                desProType = desPro.PropertyType;//得到目的变量的类型
                                //当获取属性类型时可在外部控制
                                Type newPropTargetType = copyEvent?.OnGetPropDestinationType?.Invoke(souProType, desProType, souPro.GetValue(sourceObj), sourceObj);
                                if (newPropTargetType != null)
                                {
                                    desProType = newPropTargetType;//得到目的变量的类型
                                }

                                try
                                {
                                    object value = souPro.GetValue(sourceObj);
                                    value = DoCopy(value, desProType, copyEvent);
                                    desPro.SetValue(retval, value, null);
                                }
                                catch (Exception ex)
                                {
                                    throw;//不处理异常，交给外部进行处理
                                }
                                //找到了马上跳出
                                break;
                            }
                        }
                    }

                    return retval;
                }
            }
            /// <summary>
            /// 获取到模板属性类型的值（类型不一致时强转类型）
            /// </summary>
            /// <param name="sourceValue"></param>
            /// <param name="sourceType"></param>
            /// <param name="destinationType"></param>
            /// <returns></returns>
            private object GetValueofDestinationType(object sourceValue, Type sourceType, Type destinationType)
            {
                object value = sourceValue;
                if (destinationType == sourceType)
                {
                    return value;
                }
                //当目标类型为可null类型时，需要获取到对应的基础类型
                if (destinationType.IsGenericType
                        && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>)
                        && destinationType.GenericTypeArguments.Length > 0) //如果要转类型，判断是否是可null类型
                {
                    destinationType = destinationType.GenericTypeArguments[0]; // 如果目的为可null类型必须转换为强类型才能进行赋值
                }
                //枚举不进行强制转类型
                if (sourceType.IsEnum || destinationType.IsEnum)
                {
                    //如果类型相等，不做处理 不为null的枚举可拷贝给为null的枚举
                    if (sourceType != destinationType)
                    {
                        if (sourceType.IsEnum)
                        {
                            value = (int)value;
                        }
                        if (destinationType.IsEnum)
                        {
                            string enumName = Enum.GetName(destinationType, value);
                            if (enumName != null)
                            {
                                value = Enum.Parse(destinationType, enumName);
                            }
                            else
                            {
                                value = Enum.Parse(destinationType, value.ToString());
                            }
                        }
                    }
                }
                else
                {
                    value = Convert.ChangeType(value, destinationType);
                }
                return value;
            }
            /// <summary>
            /// 获取GetIEnumerableLength的长度
            /// </summary>
            /// <param name="enumerable"></param>
            /// <returns></returns>
            public int GetIEnumerableLength(IEnumerable enumerable)
            {
                if (enumerable.GetType().IsArray)
                {
                    return (enumerable as Array).Length;
                }
                else if (enumerable is IList)
                {
                    return (enumerable as IList).Count;
                }
                else
                {
                    int length = 0;
                    foreach (var item in enumerable)
                    {
                        length++;
                    }
                    return length;
                }
            }
            /// <summary>
            /// 创建实例
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private object CreateInstance(Type type)
            {
                if (type.FullName == "System.Type")
                {
                    return type;
                }
                else
                {
                    var obj = Activator.CreateInstance(type);
                    return obj;
                }
            }
        }
        /// <summary>
        /// 获取目标类型时
        /// </summary>
        /// <param name="sourceObjType"></param>
        /// <param name="destinationObjType"></param>
        /// <param name="sourceObj">原对象值</param>
        /// <returns></returns>
        public delegate Type GetTypeDelegate(Type sourceObjType, Type destinationObjType, object sourceObj);
        /// <summary>
        /// 获取目标属性类型
        /// </summary>
        /// <param name="sourceObjType"></param>
        /// <param name="destinationObjType"></param>
        /// <param name="sourceObj"></param>
        /// <param name="sourceParentObj"></param>
        /// <returns></returns>
        public delegate Type GetPropTypeDelegate(Type sourceObjType, Type destinationObjType, object sourceObj, object sourceParentObj);
        /// <summary>
        /// 拷贝时事件
        /// </summary>
        public class CopyEvent
        {
            /// <summary>
            /// 当获取目标类型type时
            /// </summary>
            public GetTypeDelegate OnGetDestinationType { get; set; }
            /// <summary>
            /// 当获取属性的目标类型时
            /// </summary>
            public GetPropTypeDelegate OnGetPropDestinationType { get; set; }
        }
    }

}
