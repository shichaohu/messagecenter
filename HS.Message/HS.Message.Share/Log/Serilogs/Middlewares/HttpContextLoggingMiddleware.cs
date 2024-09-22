using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace HS.Message.Share.Log.Serilogs.Middlewares
{
    /// <summary>
    /// SerilogMiddleware
    /// </summary>
    public class HttpContextLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpContextLoggingMiddleware> _logger;

        public HttpContextLoggingMiddleware(RequestDelegate next, ILogger<HttpContextLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string traceId = Guid.NewGuid().ToString().Replace("-", "");//设置此次请求唯一ID，方便与返回结果关联
            context.Connection.Id = traceId;
            string bodyStr = string.Empty;

            string contentType = context.Request.Headers["Content-Type"];
            bool isWriteLog = !string.IsNullOrEmpty(contentType) && contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);

            if (isWriteLog)
            {
                try
                {
                    context.Request.EnableBuffering();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    using (var ms = new MemoryStream())
                    {
                        context.Request.Body.CopyTo(ms);
                        var param = ms.ToArray();
                        bodyStr = Encoding.UTF8.GetString(param);
                    };
                    context.Request.EnableBuffering();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);

                    _logger.LogInformation(@$"Request.Body：{bodyStr}");

                }
                catch { }
            }

            if (isWriteLog)
            {
                var originalResponseBody = context.Response.Body;
                try
                {
                    using var ms = new MemoryStream();
                    context.Response.Body = ms;
                    await _next(context);

                    context.Response.Body.Position = 0;

                    var responseReader = new StreamReader(context.Response.Body);
                    var responseContent = responseReader.ReadToEnd();

                    if (context.Response.Body.Length <= 1024 * 200)//200kb
                    {
                        string space = "\n";
                        responseContent = responseContent.Replace(",", $",{space}").Replace("{", "{" + space).Replace("}", space + "}");
                        _logger.LogInformation(@$"Response.Body：{responseContent}");
                    }

                    context.Response.Body.Position = 0;

                    await ms.CopyToAsync(originalResponseBody);

                }
                catch { }
                finally
                {
                    context.Response.Body = originalResponseBody;
                }
            }
            else
            {
                await _next(context);
            }
        }

    }
}
