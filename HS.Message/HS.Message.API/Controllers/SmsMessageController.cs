
using HS.Message.Controllers.Base;
using HS.Message.Model.Requests;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 短息消息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class SmsMessageController : CommonController<MSmsMessage, MSmsMessageCondtion>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        ISmsMessageService _smsMessageService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="smsMessageService"></param>
        public SmsMessageController(ISmsMessageService smsMessageService) : base(smsMessageService)
        {
            this._smsMessageService = smsMessageService;

        }
        
    }
}