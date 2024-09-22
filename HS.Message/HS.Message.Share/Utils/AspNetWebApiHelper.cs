using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using HS.Message.Share.BaseModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace HS.Message.Share.Utils
{
    public static class AspNetWebApiHelper
    {
        /// <summary>
        /// 加载所有的api接口信息
        /// </summary>
        /// <param name="appName">程序名</param>
        /// <param name="appXmlFilePath">程序的xml文件路径</param>
        /// <returns></returns>
        public static List<ApiBaseInfo> LoadAllApiInfo(string appName, string appXmlFilePath)
        {
            List<ApiBaseInfo> apiInfoList = new();

            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{appName}.dll")
                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
                .Where(a => a.FullName.StartsWith($"{appName}"));
            var types = assemblies.Select(x => x.GetTypes());

            foreach (var type in types)
            {
                var controllerList = type.Where(x => x.IsAssignableTo(typeof(ControllerBase))).ToList();

                foreach (var controller in controllerList)
                {
                    string controllerName = controller.FullName;//?.Replace("Controller", "");

                    var methods = controller.GetMethods()?.Where(
                        x => x.IsPublic && x.GetCustomAttribute<HttpMethodAttribute>() != null
                        ).ToList();
                    if (methods?.Count() > 0)
                    {
                        foreach (var method in methods)
                        {
                            string methodName = method.Name?.Replace("Async", "");
                            var methodRoute = method.GetCustomAttribute<RouteAttribute>();
                            string methodRouteTemplate = string.Empty;
                            if (methodRoute != null)
                            {
                                methodRouteTemplate = methodRoute.Template;
                            }
                            apiInfoList.Add(new ApiBaseInfo
                            {
                                Namespace = controller.Namespace,
                                Controller = controller.Name,
                                Action = method.Name,
                                ActionRouteTemplate = methodRouteTemplate ?? methodName
                            });
                        }
                    }

                }
            }

            // 拼接 XML 文件路径
            string filePath = $".{appXmlFilePath}/{appName}.xml";

            try
            {
                // 创建 XmlReaderSettings 对象并禁用 DTD 处理（可选）
                var readerSettings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore
                };

                // 使用 XmlReader 加载 XML 文件
                using (XmlReader xmlReader = XmlReader.Create(filePath, readerSettings))
                {
                    // 创建 XmlDocument 对象并加载 XML 数据
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);
                    //获取所有的注释信息
                    var xmlItem = xmlDoc.ChildNodes.Item(1).ChildNodes.Item(1).ChildNodes;
                    //循环所有接口
                    foreach (var api in apiInfoList)
                    {
                        Dictionary<string, object> data = new();
                        string actionFullName = $"{api.Namespace}.{api.Controller}.{api.Action}";
                        string controllerFullName = $"{api.Namespace}.{api.Controller}";

                        XmlNode ApiNode = null;
                        string actionSummary = "";//接口注释
                        string controllerSummary = "";//控制器注释
                        foreach (XmlNode xmlNode in xmlItem)
                        {
                            //拿到XML 中 匹配字段的属性
                            string tempName = string.Empty;
                            if (xmlNode?.Attributes?.Count > 0)
                            {
                                tempName = xmlNode?.Attributes[0]?.InnerText.TrimStart("M:".ToCharArray()).TrimStart("P:".ToCharArray()).TrimStart("T:".ToCharArray());
                            }

                            //去除末尾括号以及括号中的内容
                            tempName = Regex.Replace(tempName, @"\([^()]*\)$", "");

                            if (actionFullName == tempName)
                            {
                                //匹配成功后，获取对应的注释信息
                                actionSummary = xmlNode.ChildNodes.Item(0).InnerText.Trim();
                                ApiNode = xmlNode;
                                break;
                            }
                        }
                        //获取控制器的注释
                        foreach (XmlNode xmlNode in xmlItem)
                        {
                            //拿到XML 中 匹配字段的属性
                            string tempName = string.Empty;
                            if (xmlNode?.Attributes?.Count > 0)
                            {
                                tempName = xmlNode.Attributes[0].InnerText.TrimStart("M:".ToCharArray()).TrimStart("P:".ToCharArray()).TrimStart("T:".ToCharArray());
                            }

                            //去除末尾括号以及括号中的内容
                            tempName = Regex.Replace(tempName, @"\([^()]*\)$", "");
                            //与控制器的fullName 做匹配
                            if (controllerFullName == tempName)
                            {
                                //匹配成功后，获取对应的注释信息
                                controllerSummary = xmlNode.ChildNodes.Item(0).InnerText.Trim();
                                break;
                            }
                        }
                        //成功匹配则加入列表并返回
                        api.ActionSummary = actionSummary;
                        api.ControllerSummary = controllerSummary;

                        api.Controller = api.Controller.Replace("Controller", "");
                        if (string.IsNullOrEmpty(api.ControllerSummary))
                        {
                            api.ControllerSummary = api.Controller;
                        }
                        if (string.IsNullOrEmpty(api.ActionSummary))
                        {
                            api.ActionSummary = api.Action;
                        }
                        if (string.IsNullOrEmpty(api.ActionRouteTemplate))
                        {
                            api.ActionRouteTemplate = api.Action;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML 加载错误：{ex.Message}");
            }

            return apiInfoList;
        }
    }
}
