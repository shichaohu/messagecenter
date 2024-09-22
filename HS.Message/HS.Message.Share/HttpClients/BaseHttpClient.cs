using HS.Message.Share.BaseModel;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace HS.Message.Share.HttpClients
{
    public class BaseHttpClient : IHttpClient
    {
        public HttpClient Client { get; private set; }
        public BaseHttpClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
        public async Task<string> GetAsync(string url)
        {
            var responseStr = await Client.GetStringAsync(url);
            return responseStr;
        }
        public async Task<T> GetAsync<T>(string url)
        {
            var responseStr = await GetAsync(url);
            if (string.IsNullOrWhiteSpace(responseStr))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(responseStr);
        }
        public async Task<BaseResponse<T>> PostAsync<T>(string url, object parameters, Dictionary<string, string> headers = null)
        {
            var result = new BaseResponse<T>();
            var httpResponseMessage = await PostAsync(url, parameters, headers);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
                result.Code = ResponseCode.Success;
                if (typeof(T) != typeof(string))
                {
                    result.Data = JsonConvert.DeserializeObject<T>(responseStr);
                }
                else
                {
                    result.Message = responseStr;
                }
            }
            else
            {
                result.Code = ResponseCode.InternalError;
                result.Message = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            return result;

        }
        private async Task<HttpResponseMessage> PostAsync(string url, object parameters, Dictionary<string, string> headers = null)
        {
            string postJsonData = JsonConvert.SerializeObject(parameters);

            Encoding encoding = Encoding.UTF8;
            string mediaType = "application/json";
            string charset = "utf-8";
            var httpContent = new StringContent(postJsonData, encoding, mediaType);
            httpContent.Headers.ContentType.CharSet = charset;
            httpContent.Headers.TryAddWithoutValidation(HeaderNames.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            httpContent.Headers.TryAddWithoutValidation(HeaderNames.Accept, "application/json");

            if (headers != null)
            {
                foreach (var head in headers)
                {
                    httpContent.Headers.TryAddWithoutValidation(head.Key, head.Value);
                }
            }

            var httpResponseMessage = await Client.PostAsync(url, httpContent);
            return httpResponseMessage;

        }
    }
}
