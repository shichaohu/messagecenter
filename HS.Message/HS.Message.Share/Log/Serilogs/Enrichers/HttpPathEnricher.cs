using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Enrichers
{
    public class HttpPathEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string DefaultPropertyName = "HttpPath";

        private readonly string _enricherName;

        public HttpPathEnricher(IHttpContextAccessor httpContextAccessor)
            : this(DefaultPropertyName, httpContextAccessor)
        {
        }

        public HttpPathEnricher(string enricherName, IHttpContextAccessor httpContextAccessor)
        {
            _enricherName = enricherName;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent != null)
            {
                if (TryGetCurrentHttpHost(out var httpHost))
                {
                    LogEventProperty property = new(_enricherName, new ScalarValue(httpHost));
                    logEvent.AddPropertyIfAbsent(property);
                }
            }

        }

        private bool TryGetCurrentHttpHost(out string httpPath)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                httpPath = _httpContextAccessor.HttpContext.Request.Path.Value;
            }
            else
            {
                httpPath = "/";
            }

            return true;
        }
    }
}
