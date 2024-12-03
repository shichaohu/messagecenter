using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 短信模板仓储
    /// </summary>
    public class SmsTemplateRepository : BizRepositoryAdapter<MSmsTemplate, MSmsTemplateCondition>, ISmsTemplateRepository<MSmsTemplate, MSmsTemplateCondition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmsTemplateRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "sms_template")
        {
        }


    }
}
