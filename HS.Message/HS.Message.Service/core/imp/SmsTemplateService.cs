using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 短息模板服务
    /// </summary>
    public class SmsTemplateService : BaseService<MSmsTemplate, MSmsTemplateCondition>, ISmsTemplateService
    {
        private readonly ISmsTemplateRepository<MSmsTemplate, MSmsTemplateCondition> _smsTemplateRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsTemplateRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public SmsTemplateService(
            ISmsTemplateRepository<MSmsTemplate, MSmsTemplateCondition> smsTemplateRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(smsTemplateRepository, injectedObjects, "SmsTemplate")
        {
            _smsTemplateRepository = smsTemplateRepository;
            _cache = cache;
        }

    }
}