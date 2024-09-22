using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;

namespace HS.Message.Middlewares
{
    /// <summary>
    /// 路由适配
    /// </summary>
    public class RouteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RouteMiddleware> _logger;
        public RouteMiddleware( RequestDelegate next, IConfiguration configuration, ILogger<RouteMiddleware> logger)
        {
            this._next = next;
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                string requestPath = "/api/bus/";
                if (context.Request.Path.HasValue && context.Request.Path.Value.StartsWith(requestPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    context.Request.Path = context.Request.Path.Value.Replace("/bus/", "/");
                }
            }

            await _next.Invoke(context);
        }
    }
}
