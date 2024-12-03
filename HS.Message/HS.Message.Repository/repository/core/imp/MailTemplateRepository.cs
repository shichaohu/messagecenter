using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 邮件模板仓储
    /// </summary>
    public class MailTemplateRepository : BizRepositoryAdapter<MMailTemplate, MMailTemplateCondition>, IMailTemplateRepository<MMailTemplate, MMailTemplateCondition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailTemplateRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "mail_template")
        {
        }


    }
}
