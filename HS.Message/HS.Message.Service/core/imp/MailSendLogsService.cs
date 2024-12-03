using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using System.Threading.Tasks;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 邮件发送日志服务
    /// </summary>
    public class MailSendLogsService : BaseService<MMailSendLogs, MMailSendLogsCondition>, IMailSendLogsService
    {
        private readonly IMailSendLogsRepository<MMailSendLogs, MMailSendLogsCondition> _mailSendLogsRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 字典相关服务
        /// </summary>
        /// <param name="mailSendLogsRepository"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MailSendLogsService(
            IMailSendLogsRepository<MMailSendLogs, MMailSendLogsCondition> mailSendLogsRepository,
            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
            : base(mailSendLogsRepository, injectedObjects, "MailSendLogs")
        {
            _mailSendLogsRepository = mailSendLogsRepository;
            _cache = cache;
        }

    }
}