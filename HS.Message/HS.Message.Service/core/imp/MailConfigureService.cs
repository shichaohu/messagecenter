using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 邮件配置相关服务
    /// </summary>
    public class MailConfigureService : BaseService<MMailConfigure, MMailConfigureCondition>, IMailConfigureService
    {
        private readonly IMailConfigureRepository<MMailConfigure, MMailConfigureCondition> _mailConfigureRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 字典相关服务
        /// </summary>
        /// <param name="mailConfigureRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MailConfigureService(
            IMailConfigureRepository<MMailConfigure, MMailConfigureCondition> mailConfigureRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(mailConfigureRepository, injectedObjects, "MailConfigure")
        {
            _mailConfigureRepository = mailConfigureRepository;
            _cache = cache;
        }


    }
}