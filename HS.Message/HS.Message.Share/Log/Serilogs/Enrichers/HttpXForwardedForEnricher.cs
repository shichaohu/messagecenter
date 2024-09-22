using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Enrichers
{
    public class HttpXForwardedForEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string DefaultPropertyName = "HttpXForwardedFor";

        private readonly string _enricherName;

        public HttpXForwardedForEnricher(IHttpContextAccessor httpContextAccessor)
            : this(DefaultPropertyName, httpContextAccessor)
        {
        }

        public HttpXForwardedForEnricher(string _enricherName, IHttpContextAccessor httpContextAccessor)
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

        private bool TryGetCurrentHttpHost(out string httpXForwardedFor)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                httpXForwardedFor = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "";
            }
            else
            {
                httpXForwardedFor = "";
            }

            return true;
        }
    }
}
