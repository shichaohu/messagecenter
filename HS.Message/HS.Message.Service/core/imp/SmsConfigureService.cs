using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 短信配置服务
    /// </summary>
    public class SmsConfigureService : BaseService<MSmsConfigure, MSmsConfigureCondition>, ISmsConfigureService
    {
        private readonly ISmsConfigureRepository<MSmsConfigure, MSmsConfigureCondition> _smsConfigureRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsConfigureRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public SmsConfigureService(
            ISmsConfigureRepository<MSmsConfigure, MSmsConfigureCondition> smsConfigureRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(smsConfigureRepository, injectedObjects, "SmsConfigure")
        {
            _smsConfigureRepository = smsConfigureRepository;
            _cache = cache;
        }

    }
}