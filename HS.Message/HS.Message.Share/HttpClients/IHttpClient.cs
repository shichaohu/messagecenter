using HS.Message.Share.BaseModel;

namespace HS.Message.Share.HttpClients
{
    public interface IHttpClient
    {
        Task<T> GetAsync<T>(string method);
        Task<string> GetAsync(string method);
        Task<BaseResponse<T>> PostAsync<T>(string method, object parameters, Dictionary<string, string> headers = null);
    }
}
