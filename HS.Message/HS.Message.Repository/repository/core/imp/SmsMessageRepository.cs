using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 短信仓储
    /// </summary>
    public class SmsMessageRepository : BizRepositoryAdapter<MSmsMessage, MSmsMessageCondtion>, ISmsMessageRepository<MSmsMessage, MSmsMessageCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsMessageRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "sms_message")
        {
        }


    }
}
