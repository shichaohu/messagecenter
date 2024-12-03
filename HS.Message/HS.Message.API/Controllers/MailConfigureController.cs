
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 邮件配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MailConfigureController : CommonController<MMailConfigure, MMailConfigureCondition>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        IMailConfigureService _mailConfigureService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="mailConfigureService"></param>
        public MailConfigureController(IMailConfigureService mailConfigureService) : base(mailConfigureService)
        {
            this._mailConfigureService = mailConfigureService;

        }
        
    }
}