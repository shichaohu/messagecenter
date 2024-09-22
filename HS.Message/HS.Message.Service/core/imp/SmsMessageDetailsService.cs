using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 短信消息详情服务
    /// </summary>
    public class SmsMessageDetailsService : BaseService<MSmsMessageDetails, MSmsMessageDetailsCondtion>, ISmsMessageDetailsService
    {
        private readonly ISmsMessageDetailsRepository<MSmsMessageDetails, MSmsMessageDetailsCondtion> _smsMessageDetailsRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsMessageDetailsRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public SmsMessageDetailsService(
            ISmsMessageDetailsRepository<MSmsMessageDetails, MSmsMessageDetailsCondtion> smsMessageDetailsRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(smsMessageDetailsRepository, injectedObjects, "SmsMessageDetails")
        {
            _smsMessageDetailsRepository = smsMessageDetailsRepository;
            _cache = cache;
        }

    }
}