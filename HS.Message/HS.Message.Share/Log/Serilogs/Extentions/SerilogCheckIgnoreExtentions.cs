using HS.Message.Share.Log.Serilogs.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Log.Serilogs.Extentions
{
    internal static class SerilogCheckIgnoreExtentions
    {
        public static List<string> IgnoreLogMethodList { get; set; }
        static SerilogCheckIgnoreExtentions()
        {
            IgnoreLogMethodList = new List<string>();
        }
        public static void UseSerilogIgnore(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appName = env.ApplicationName;
            InitSerilogIgnoreObjects(appName);
        }
        public static bool CheckMethodIncludInIgnore(string controllerName, string actionName)
        {
            if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(actionName))
            {
                if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                    controllerName += "controller";
                string fullActionName = $"{controllerName}.{actionName}";
                return IgnoreLogMethodList.Any(m =>
                m.Contains(controllerName, StringComparison.OrdinalIgnoreCase)
                || m.Contains(fullActionName, StringComparison.OrdinalIgnoreCase)
                );
            }
            else return false;
        }
        private static void InitSerilogIgnoreObjects(string appName)
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{appName}.dll")
                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
                .Where(a => a.FullName.StartsWith($"{appName}"));
            var types = assemblies.Select(x => x.GetTypes());

            foreach (var type in types)
            {
                var controllerList = type.Where(x =>
                    x.FullName.StartsWith($"{appName}.Controllers", StringComparison.OrdinalIgnoreCase)
                    && x.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                ).ToList();

                foreach (var controller in controllerList)
                {
                    string controllerFullName = controller.FullName;
                    if (controller.GetCustomAttribute<SerilogIgnoreAttribute>() != null)
                    {
                        IgnoreLogMethodList.Add(controllerFullName);
                    }
                    var methods = controller.GetMethods()?.Where(x => x.GetCustomAttribute<SerilogIgnoreAttribute>() != null);

                    if (methods?.Count() > 0)
                    {
                        var actionNames = methods.Select(x => $"{controllerFullName}.{x.Name}").ToList();
                        if (actionNames?.Count > 0)
                            IgnoreLogMethodList.AddRange(actionNames);
                    }

                }
            }
        }
    }
}
