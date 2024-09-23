using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Http;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HS.Message.Share.Extensions;

/// <summary>
/// 
/// </summary>
public static class AspDotNetExtensions
{
    /// <summary>
    /// Adds Asp.Net base services to the specified IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    public static void AddAspDotNetBasic(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();
        services.AddSingleton<LocalCache>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped(typeof(HttpContextInfo));
        services.AddTransient<IDistributedCache, RedisCache>();

        services.AddCors(policy =>
        {
            policy.AddPolicy("CorsPolicy", opt => opt
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
            //.WithExposedHeaders("X-Pagination"));
        });
    }

    /// <summary>
    /// �Զ�ע��projectName��ʵ��IDependency�Ľӿ�
    /// </summary>
    /// <param name="services"></param>
    /// <param name="projectName">��Ŀ����ǰ׺��һ����������ע��㣬��service</param>
    public static void AddAutoDependency(this IServiceCollection services, string projectName)
    {
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
            .Where(a => a.FullName.StartsWith(projectName));

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.IsAssignableTo(typeof(IDependency))))
                .AsImplementedInterfaces(x =>
                {
                    return x.IsAssignableTo(typeof(IDependency));
                })
        .WithScopedLifetime());

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.IsAssignableTo(typeof(ISingletonDependency))))
                .AsImplementedInterfaces(x =>
                {
                    return x.IsAssignableTo(typeof(ISingletonDependency));
                })
        .WithSingletonLifetime());

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.IsAssignableTo(typeof(ITransientDependency))))
                .AsImplementedInterfaces(x =>
                {
                    return x.IsAssignableTo(typeof(ITransientDependency));
                })
        .WithTransientLifetime());
    }


    /// <summary>
    /// Adds the model state vrify service to the specified IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    public static void AddModelStateVrify(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                //��ȡ��֤ʧ�ܵ�ģ���ֶ� 
                var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => e.Value.Errors.First().ErrorMessage)
                .ToList();
                var str = string.Join("|", errors);
                //���÷�������
                var result = new BaseResponse
                {
                    Code = ResponseCode.ParameterError,
                    Message = str
                };
                var res = new BadRequestObjectResult(result)
                {
                    StatusCode = 200
                };
                return res;
            };
        });
    }

    /// <summary>
    /// �����ļ��ϴ���С 4G
    /// </summary>
    /// <param name="services"></param>
    /// <param name="webHostBuilder"></param>
    public static void UseFileUpload(this IServiceCollection services, IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureKestrel((context, options) =>
        {
            //�������4G��1073741824*4=4,294,967,296  byte��
            options.Limits.MaxRequestBodySize = 4_294_967_296;
        });
        services.Configure<FormOptions>(option =>
        {
            //�������4G��1073741824*4=4,294,967,296  byte��
            option.MultipartBodyLengthLimit = 4_294_967_296;
        });
    }
}

