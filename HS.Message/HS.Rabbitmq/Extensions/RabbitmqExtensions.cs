using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Http;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using HS.Rabbitmq.Core;
using HS.Rabbitmq.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HS.Rabbitmq.Extensions;

/// <summary>
/// 
/// </summary>
public static class RabbitmqExtensions
{
    /// <summary>
    /// Adds Asp.Net base services to the specified IServiceCollection
    /// </summary>
    /// <param name="services"></param>
    public static void AddRabbitmq(this IServiceCollection services, string cnsumerCallBackProjectName)
    {
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{cnsumerCallBackProjectName}.dll")
            .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.Where(t => t.IsAssignableTo(typeof(IConsumerCallBack))))
                .AsImplementedInterfaces(x =>
                {
                    return x.IsAssignableTo(typeof(IConsumerCallBack));
                })
        .WithSingletonLifetime());

        services.AddSingleton(typeof(RabbitmqTopicConsumer));
        services.AddScoped(typeof(RabbitmqTopicProducer));
    }
    public static IApplicationBuilder UseRabbitmq(this IApplicationBuilder app)
    {
        var consumer = app.ApplicationServices.GetService<RabbitmqTopicConsumer>();
        consumer.InitMq();
        return app;
    }

}

