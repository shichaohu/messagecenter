
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 短息配置
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class SmsConfigureController : CommonController<MSmsConfigure, MSmsConfigureCondtion>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        ISmsConfigureService _smsConfigureService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="smsConfigureService"></param>
        public SmsConfigureController(ISmsConfigureService smsConfigureService) : base(smsConfigureService)
        {
            this._smsConfigureService = smsConfigureService;

        }
        
    }
}