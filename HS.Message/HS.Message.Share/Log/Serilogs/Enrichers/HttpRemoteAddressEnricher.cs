using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace HS.Message.Share.Log.Serilogs.Enrichers
{
    public class HttpRemoteAddressEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string DefaultPropertyName = "HttpRemoteAddress";

        private readonly string _enricherName;

        public HttpRemoteAddressEnricher(IHttpContextAccessor httpContextAccessor)
            : this(DefaultPropertyName, httpContextAccessor)
        {
        }

        public HttpRemoteAddressEnricher(string _enricherName, IHttpContextAccessor httpContextAccessor)
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

        private bool TryGetCurrentHttpHost(out string remoteIpAddress)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            }
            else
            {
                remoteIpAddress = "127.0.0.1";
            }

            return true;
        }
    }
}
