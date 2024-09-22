
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 邮件
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MailMessageController : CommonController<MMailMessage, MMailMessageCondtion>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        IMailMessageService _mailMessageService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="mailMessageService"></param>
        public MailMessageController(IMailMessageService mailMessageService) : base(mailMessageService)
        {
            this._mailMessageService = mailMessageService;

        }
        
    }
}