using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Enrichers
{
    public class HttpRequestIdEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string DefaultPropertyName = "HttpRequestId";

        private readonly string _enricherName;

        public HttpRequestIdEnricher(IHttpContextAccessor httpContextAccessor)
            : this(DefaultPropertyName, httpContextAccessor)
        {
        }

        public HttpRequestIdEnricher(string enricherName, IHttpContextAccessor httpContextAccessor)
        {
            _enricherName = enricherName;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent != null)
            {
                if (TryGetCurrentHttpRequestId(out var requestId))
                {
                    LogEventProperty property = new(_enricherName, new ScalarValue(requestId));
                    logEvent.AddPropertyIfAbsent(property);
                }
            }

        }

        private bool TryGetCurrentHttpRequestId(out string requestId)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                requestId = _httpContextAccessor.HttpContext.Connection.Id;
            }
            else
            {
                requestId = Guid.Empty.ToString().Replace("-", "");
            }

            return true;
        }
    }
}
