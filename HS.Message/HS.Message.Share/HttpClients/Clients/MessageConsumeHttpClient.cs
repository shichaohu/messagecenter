using HS.Message.Share.BaseModel;

namespace HS.Message.Share.HttpClients.Clients
{
    /// <summary>
    /// 消费消息的HttpClient
    /// </summary>
    public class MessageConsumeHttpClient : BaseHttpClient, IHttpClient
    {
        public MessageConsumeHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResponse<T>> SendAsync<T>(string url, object parameters)
        {
            var header = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            var result = await PostAsync<T>(url, parameters, header);
            return result;
        }
    }

}
