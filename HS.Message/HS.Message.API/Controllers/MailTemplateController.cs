
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 邮件模板
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MailTemplateController : CommonController<MMailTemplate, MMailTemplateCondtion>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        IMailTemplateService _mailTemplateService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="mailTemplateService"></param>
        public MailTemplateController(IMailTemplateService mailTemplateService) : base(mailTemplateService)
        {
            this._mailTemplateService = mailTemplateService;

        }
        
    }
}