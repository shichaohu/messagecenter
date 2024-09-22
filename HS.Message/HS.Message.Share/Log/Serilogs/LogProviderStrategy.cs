using HS.Message.Share.Log.Serilogs.Enrichers;
using HS.Message.Share.Log.Serilogs.Extentions;
using HS.Message.Share.Log.Serilogs.Middlewares;
using HS.Message.Share.Log.Serilogs.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Server.Kestrel.Core;



namespace HS.Message.Share.Log.Serilogs
{
    /// <summary>
    /// Register log strategy
    /// </summary>
    public static class LogProviderStrategy
    {
        /// <summary>
        /// Adds a log strategy service to the specified IHostBuilder.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="loggingBuilder"></param>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        public static void AddLogStrategy(this IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder, IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            loggingBuilder.ClearProviders();
            hostBuilder.UseSerilog((context, serviceProvider, logger) =>
            {
                var httpContext = serviceProvider.GetService<IHttpContextAccessor>();
                var filterLogger = logger
                // Filter out Microsoft&System infrastructre logs that are Error below
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Filter.ByIncludingOnly(x => x.CheckCanWriteLog(httpContext))
                .Enrich.With(new HttpHostEnricher(httpContext))
                .Enrich.With(new HttpRemoteAddressEnricher(httpContext))
                .Enrich.With(new HttpXForwardedForEnricher(httpContext))
                .Enrich.With(new HttpPathEnricher(httpContext))
                .Enrich.With(new HttpRequestIdEnricher(httpContext));

                filterLogger.ReadFrom.Configuration(context.Configuration);
                filterLogger.Enrich.FromLogContext();
                filterLogger.WriteTo.Console();

                LogStorageType logStorageType = configuration.GetSection("Log:LogStorageType").Get<LogStorageType>();
#if DEBUG
                logStorageType = configuration.GetSection("Log:LogStorageTypeWhenDebug").Get<LogStorageType>(); ;
#endif
                switch (logStorageType)
                {
                    case LogStorageType.LogFile:
                        WriteLogToFile(context, filterLogger, configuration);
                        break;
                    case LogStorageType.MySql:
                        WriteLogToMySql(context, filterLogger, configuration);
                        break;
                    default:
                        break;
                }


            });

            loggingBuilder.SetMinimumLevel(LogLevel.Information);
        }
        /// <summary>
        /// Adds the log middleware to IApplicationBuilder
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void UseLog(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogIgnore(env);
            app.UseMiddleware<HttpContextLoggingMiddleware>();
            app.UseSerilogRequestLogging(opts =>
            {
                opts.SetCustomEnrichDiagnosticContext();
            }
            );
        }

        private static void WriteLogToFile(HostBuilderContext context, LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            string outputTemp = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] Url:{HttpHost}{HttpPath}
RequestId:{HttpRequestId}
RemoteAddress:{HttpRemoteAddress}
X-Forwarded-For:{HttpXForwardedFor}
SourceContext:{SourceContext}
{Message:lj}
{Exception}{NewLine}";
            loggerConfiguration
                .WriteTo.Logger(configure => configure
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        $"logs/error/.log",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 10_000_000,
                        retainedFileCountLimit: 200,
                        retainedFileTimeLimit: TimeSpan.FromDays(7),
                        outputTemplate: outputTemp))

                .WriteTo.Logger(configure => configure
                    .WriteTo.File(
                    $"logs/.log",//如20230812.log,20230812_001.log
                    rollingInterval: RollingInterval.Day,// 设置日志输出到文件中，文件名按天滚动，文件夹名称为日期加小时
                    rollOnFileSizeLimit: true,// 设置为 true，表示启用日志文件大小限制，当日志文件达到设定的大小后，会自动滚动到新的文件中。
                    fileSizeLimitBytes: 10_000_000, //设置每个日志文件的最大大小，单位是字节。这里的值是 10MB，即 10_000_000 字节。
                    retainedFileCountLimit: 200,//设置保留的日志文件数量上限，这里是 200，即最多保留最新的 200 个日志文件。
                    retainedFileTimeLimit: TimeSpan.FromDays(7),//设置日志文件的最长保留时间，这里是 7 天。
                    shared: true, // 多进程共享文件
                    outputTemplate: outputTemp,
                    restrictedToMinimumLevel: LogEventLevel.Debug
                ));
        }
        private static void WriteLogToMySql(HostBuilderContext context, LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            string mySqlDBConnectionString = configuration["Log:MySql:DbConnectionString"];
            string logTableName = configuration["Log:MySql:TableName"];
            loggerConfiguration.WriteTo.ToMySQL(mySqlDBConnectionString, $"{logTableName}");
        }


    }

}

