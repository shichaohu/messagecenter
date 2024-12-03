
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 短息消息详情
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class SmsMessageDetailsController : CommonController<MSmsMessageDetails, MSmsMessageDetailsCondition>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        ISmsMessageDetailsService _smsMessageDetailsService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="smsMessageDetailsService"></param>
        public SmsMessageDetailsController(ISmsMessageDetailsService smsMessageDetailsService) : base(smsMessageDetailsService)
        {
            this._smsMessageDetailsService = smsMessageDetailsService;

        }
        
    }
}