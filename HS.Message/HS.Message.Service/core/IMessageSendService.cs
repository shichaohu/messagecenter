using HS.Message.Model.Requests;
using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 消息发送服务
    /// </summary>
    public interface IMessageSendService : IDependency
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse> RunAsync();
    }
}
