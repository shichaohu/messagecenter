using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Enrichers
{
    public class HttpHostEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string DefaultPropertyName = "HttpHost";

        private readonly string _enricherName;

        public HttpHostEnricher(IHttpContextAccessor httpContextAccessor)
            : this(DefaultPropertyName, httpContextAccessor)
        {
        }

        public HttpHostEnricher(string _enricherName, IHttpContextAccessor httpContextAccessor)
        {
            this._enricherName = _enricherName;
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

        private bool TryGetCurrentHttpHost(out string httpHost)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                httpHost = _httpContextAccessor.HttpContext.Request.Host.ToUriComponent() ?? "";
            }
            else
            {
                httpHost = "127.0.0.1";
            }

            return true;
        }
    }
}
