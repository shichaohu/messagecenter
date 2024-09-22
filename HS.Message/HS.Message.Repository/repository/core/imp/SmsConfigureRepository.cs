using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 短信配置仓储
    /// </summary>
    public class SmsConfigureRepository : BizRepositoryAdapter<MSmsConfigure, MSmsConfigureCondtion>, ISmsConfigureRepository<MSmsConfigure, MSmsConfigureCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsConfigureRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "sms_configure")
        {
        }


    }
}
