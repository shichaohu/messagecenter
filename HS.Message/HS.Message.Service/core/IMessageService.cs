using HS.Message.Model.Requests;
using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 消息服务
    /// </summary>
    public interface IMessageService : IBaseService<MMessage, MMessageCondition>, IDependency
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse> SendMessageAsync(List<MessageRequest> request);
    }
}
