
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 邮件发送日志
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MailSendLogsController : CommonController<MMailSendLogs, MMailSendLogsCondition>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        IMailSendLogsService _mailSendLogsService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="mailSendLogsService"></param>
        public MailSendLogsController(IMailSendLogsService mailSendLogsService) : base(mailSendLogsService)
        {
            this._mailSendLogsService = mailSendLogsService;

        }
        
    }
}