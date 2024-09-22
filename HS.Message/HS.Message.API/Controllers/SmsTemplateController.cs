
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 短息模板
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class SmsTemplateController : CommonController<MSmsTemplate, MSmsTemplateCondtion>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        ISmsTemplateService _smsTemplateService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="smsTemplateService"></param>
        public SmsTemplateController(ISmsTemplateService smsTemplateService) : base(smsTemplateService)
        {
            this._smsTemplateService = smsTemplateService;

        }
        
    }
}