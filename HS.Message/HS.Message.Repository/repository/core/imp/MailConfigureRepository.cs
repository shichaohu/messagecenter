using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 邮件配置仓储
    /// </summary>
    public class MailConfigureRepository : BizRepositoryAdapter<MMailConfigure, MMailConfigureCondition>, IMailConfigureRepository<MMailConfigure, MMailConfigureCondition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailConfigureRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "mail_configure")
        {
        }


    }
}
