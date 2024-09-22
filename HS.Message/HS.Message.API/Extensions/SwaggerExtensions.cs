using HS.Message.Extensions.Swagger;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Reflection;

namespace HS.Message.Extensions
{
    /// <summary>
    /// SwaggerUI集成扩展
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        ///Adds the SwaggerDoc service to the specified IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSwaggerDoc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "Authorization header. \r\n示例: 'bearer f234324fsdfds'",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                    },
                    Scheme = "oauth2",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                };
                options.AddSecurityDefinition("Authorization", scheme);
                var requirement = new OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                };
                options.AddSecurityRequirement(requirement);

                options.CustomSchemaIds(type => type.FullName);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HS.Message.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HS.Message.Service.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HS.Message.Model.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HS.Message.Repository.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HS.Message.Share.xml"), true);
                #region 分组
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    var openApiInfo = new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    };
                    options.SwaggerDoc(f.Name, openApiInfo);
                });

                //判断接口归于哪个分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (!apiDescription.TryGetMethodInfo(out MethodInfo method)) return false;

                    if (docName == nameof(ApiGroupNames.All)) return true;
                    //反射拿到控制器分组特性下的值
                    var actionlist = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is ApiGroupAttribute);
                    //得到尚未分组的接口
                    if (docName == nameof(ApiGroupNames.NoGroup)) return actionlist == null;
                    //加载对应已经分好组的接口
                    if (actionlist is ApiGroupAttribute actionfilter)
                    {
                        return actionfilter.GroupName.Any(x => x.ToString().Trim() == docName);
                    }
                    return false;
                });
                #endregion
                options.EnableAnnotations();
            });
        }
        /// <summary>
        /// Adds the SwaggerUi middleware to IApplicationBuilder
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public static void UseSwaggerUi(this IApplicationBuilder app, IConfiguration configuration,  IWebHostEnvironment env)
        {
            var isDevelopment = env.IsDevelopment();
            var isEnableOpenSwagger = Convert.ToBoolean(configuration["SwaggerDocOptions:Enabled"] ?? "false");
            if (isDevelopment || isEnableOpenSwagger)
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/api-docs/{documentName}/swagger.json";
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        if (!string.IsNullOrWhiteSpace(configuration["SwaggerDocOptions:BaseUrl"]))
                        {
                            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = configuration["SwaggerDocOptions:BaseUrl"] } };
                        }
                    });
                });
                //启用SwaggerUI,访问地址：http://ip:port/swagger/index.html
                app.UseSwaggerUI(options =>
                {
                    typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                    {
                        var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                        options.SwaggerEndpoint($"/api-docs/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                    });

                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
                //启用Knife4UI,访问地址：http://ip:port/index.html
                app.UseKnife4UI(options =>
                {
                    options.RoutePrefix = string.Empty;

                    typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                    {
                        var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                        options.SwaggerEndpoint($"/api-docs/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                    });

                });
            }
        }
    }
}
