using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 短信详情仓储
    /// </summary>
    public class SmsMessageDetailsRepository : BizRepositoryAdapter<MSmsMessageDetails, MSmsMessageDetailsCondtion>, ISmsMessageDetailsRepository<MSmsMessageDetails, MSmsMessageDetailsCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsMessageDetailsRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "sms_message_details")
        {
        }


    }
}
