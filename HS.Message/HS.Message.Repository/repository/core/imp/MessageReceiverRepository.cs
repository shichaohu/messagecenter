using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 消息接收人仓储
    /// </summary>
    public class MessageReceiverRepository : BizRepositoryAdapter<MMessageReceiver, MMessageReceiverCondtion>, IMessageReceiverRepository<MMessageReceiver, MMessageReceiverCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageReceiverRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "message_receiver")
        {
        }


    }
}
