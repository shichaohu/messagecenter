using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 短息服务
    /// </summary>
    public class SmsMessageService : BaseService<MSmsMessage, MSmsMessageCondition>, ISmsMessageService
    {
        private readonly ISmsMessageRepository<MSmsMessage, MSmsMessageCondition> _smsMessageRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsMessageRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public SmsMessageService(
            ISmsMessageRepository<MSmsMessage, MSmsMessageCondition> smsMessageRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(smsMessageRepository, injectedObjects, "SmsMessage")
        {
            _smsMessageRepository = smsMessageRepository;
            _cache = cache;
        }

    }
}