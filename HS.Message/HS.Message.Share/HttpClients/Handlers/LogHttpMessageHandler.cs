using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace HS.Message.Share.HttpClients.Handlers
{
    /// <summary>
    /// 请求的日志处理
    /// </summary>
    public class LogHttpMessageHandler<T> : DelegatingHandler where T : IHttpClient
    {
        private IServiceProvider _provider;
        private readonly ILogger _logger;

        public LogHttpMessageHandler(IServiceProvider provider)
        {
            _provider = provider;
            _provider = provider;
            _logger = provider.GetRequiredService<ILogger<T>>();
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = string.Empty;
            if (request.Method == HttpMethod.Post)
            {
                using var ms = new MemoryStream();
                try
                {
                    await request.Content.CopyToAsync(ms, cancellationToken);
                    var param = ms.ToArray();
                    requestBody = Encoding.UTF8.GetString(param);
                }
                catch (Exception ex)
                {
                    requestBody = $"读取request的body失败，Message：{ex.Message}";
                }
            }
            else if (request.Method == HttpMethod.Get)
            {
                requestBody = request.RequestUri?.Query;
            };
            string requestMessage = $"{typeof(T).Name}.Request.Body:{requestBody},Request Detail:{JsonConvert.SerializeObject(request)}";
            _logger.LogInformation(requestMessage);
            var response = await base.SendAsync(request, cancellationToken);
            string responseBody = await response.Content.ReadAsStringAsync();
            string responseMessage = $"{typeof(T).Name}.Response.Body:{responseBody}";
            _logger.LogInformation(responseMessage);
            return response;
        }
    }
}
