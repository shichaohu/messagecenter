using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 邮件模板服务
    /// </summary>
    public class MailTemplateService : BaseService<MMailTemplate, MMailTemplateCondtion>, IMailTemplateService
    {
        private readonly IMailTemplateRepository<MMailTemplate, MMailTemplateCondtion> _mailTemplateRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 字典相关服务
        /// </summary>
        /// <param name="mailTemplateRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MailTemplateService(
            IMailTemplateRepository<MMailTemplate, MMailTemplateCondtion> mailTemplateRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(mailTemplateRepository, injectedObjects, "MailTemplate")
        {
            _mailTemplateRepository = mailTemplateRepository;
            _cache = cache;
        }

    }
}