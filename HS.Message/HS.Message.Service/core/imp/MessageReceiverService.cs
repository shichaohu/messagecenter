using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 消息接收人服务
    /// </summary>
    public class MessageReceiverService : BaseService<MMessageReceiver, MMessageReceiverCondtion>, IMessageReceiverService
    {
        private readonly IMessageReceiverRepository<MMessageReceiver, MMessageReceiverCondtion> _messageReceiverRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 字典相关服务
        /// </summary>
        /// <param name="messageReceiverRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MessageReceiverService(
            IMessageReceiverRepository<MMessageReceiver, MMessageReceiverCondtion> messageReceiverRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(messageReceiverRepository, injectedObjects, "MessageReceiver")
        {
            _messageReceiverRepository = messageReceiverRepository;
            _cache = cache;
        }


    }
}