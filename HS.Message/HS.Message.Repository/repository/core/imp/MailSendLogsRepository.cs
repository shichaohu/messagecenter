using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 邮件发送日志仓储
    /// </summary>
    public class MailSendLogsRepository : BizRepositoryAdapter<MMailSendLogs, MMailSendLogsCondition>, IMailSendLogsRepository<MMailSendLogs, MMailSendLogsCondition>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailSendLogsRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "mail_send_logs")
        {
        }


    }
}
