using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 消息接收人服务
    /// </summary>
    public interface IMessageReceiverService : IBaseService<MMessageReceiver, MMessageReceiverCondtion>, IDependency
    {
    }
}
