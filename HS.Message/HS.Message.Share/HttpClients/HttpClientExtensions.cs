using HS.Message.Share.HttpClients.Clients;
using HS.Message.Share.HttpClients.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HS.Message.HttpClients
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds customer HttpClient to the specified IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCustomerHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<MessageConsumeHttpClient>(
                httpClient =>
                {
                    httpClient.BaseAddress = new Uri(configuration["ExternalApiUrl:MessageConsume:Url"]);
                })
                .AddHttpMessageHandler(provider =>
                {
                    return new LogHttpMessageHandler<MessageConsumeHttpClient>(provider);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));
        }

    }
}
