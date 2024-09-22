using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 消息仓储
    /// </summary>
    public class MessageRepository : BizRepositoryAdapter<MMessage, MMessageCondtion>, IMessageRepository<MMessage, MMessageCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "message")
        {
        }


    }
}
