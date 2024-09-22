using HS.Message.Share.Log.Serilogs.Enrichers;
using HS.Message.Share.Log.Serilogs.LogEventSinks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Extentions
{
    internal static class SerilogExtentions
    {
        /// <summary>
        /// 日志内容包含HttpRequestId
        /// </summary>
        /// <param name="loggerEnrichmentConfiguration"></param>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static LoggerConfiguration WithHttpRequestId(this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            if (loggerEnrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(loggerEnrichmentConfiguration));

            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            return loggerEnrichmentConfiguration.With(new HttpRequestIdEnricher(httpContextAccessor));
        }

        internal static bool CheckCanWriteLog(this LogEvent logEvent, IHttpContextAccessor httpContextAccessor)
        {
            if (logEvent.Level == LogEventLevel.Error)//错误日志一定要写
            {
                return true;
            }

            if (httpContextAccessor?.HttpContext != null)
            {
                try
                {
                    bool isWriteLog = true;

                    if (logEvent.Properties.ContainsKey("SourceContext"))
                    {
                        string sourceContext = logEvent.Properties["SourceContext"]?.ToString();
                        if (!string.IsNullOrEmpty(sourceContext))
                        {
                            isWriteLog = sourceContext.Contains(".RequestLoggingMiddleware");
                            if (isWriteLog) return true;
                        }
                    }

                    isWriteLog = true;
                    object controllerName = null;
                    object actionName = null;
                    isWriteLog = isWriteLog && httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("controller", out controllerName);
                    isWriteLog = isWriteLog && httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("action", out actionName);
                    if (!isWriteLog)
                    {
                        return true;//非访问controller.action,直接返回true
                    }
                    else
                    {
                        isWriteLog = !SerilogCheckIgnoreExtentions.CheckMethodIncludInIgnore(controllerName?.ToString(), actionName?.ToString());

                        return isWriteLog;
                    }
                }
                catch
                {

                }
            }
            return true;
        }

        internal static void SetCustomEnrichDiagnosticContext(this RequestLoggingOptions logOptions)
        {
            logOptions.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var request = httpContext.Request;
                if (request != null)
                {
                    diagnosticContext.Set("HttpRequestId", httpContext.Connection.Id);
                    diagnosticContext.Set("Host", request.Host);
                    diagnosticContext.Set("Protocol", request.Protocol);
                    diagnosticContext.Set("Scheme", request.Scheme);

                    if (request.QueryString.HasValue)
                    {
                        diagnosticContext.Set("QueryString", request.QueryString.Value);
                    }

                    diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

                    var endpoint = httpContext.GetEndpoint();
                    if (endpoint is object)
                    {
                        diagnosticContext.Set("EndpointName", endpoint.DisplayName);
                    }
                }
            };

        }
        /// <summary>
        /// 日志写入MysSQL
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="restrictedToMinimumLevel"></param>
        /// <param name="storeTimestampInUtc"></param>
        /// <param name="batchSize"></param>
        /// <param name="levelSwitch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static LoggerConfiguration ToMySQL(this LoggerSinkConfiguration loggerConfiguration, string connectionString, string tableName = "Logs", LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, bool storeTimestampInUtc = false, uint batchSize = 100u, LoggingLevelSwitch levelSwitch = null)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException("loggerConfiguration");
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (batchSize < 1 || batchSize > 1000)
            {
                throw new ArgumentOutOfRangeException("[batchSize] argument must be between 1 and 1000 inclusive");
            }

            try
            {
                return loggerConfiguration.Sink(new MySqlSink(connectionString, tableName, storeTimestampInUtc, batchSize), restrictedToMinimumLevel, levelSwitch);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
                throw;
            }
        }

    }

}
