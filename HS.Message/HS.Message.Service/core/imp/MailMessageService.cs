using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 邮件消息相关服务
    /// </summary>
    public class MailMessageService : BaseService<MMailMessage, MMailMessageCondtion>, IMailMessageService
    {
        private readonly IMailMessageRepository<MMailMessage, MMailMessageCondtion> _mailMessageRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 字典相关服务
        /// </summary>
        /// <param name="mailMessageRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MailMessageService(
            IMailMessageRepository<MMailMessage, MMailMessageCondtion> mailMessageRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(mailMessageRepository, injectedObjects, "MailMessage")
        {
            _mailMessageRepository = mailMessageRepository;
            _cache = cache;
        }

    }
}