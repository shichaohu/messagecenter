
using HS.Message.Controllers.Base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 消息接收人
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MessageReceiverController : CommonController<MMessageReceiver, MMessageReceiverCondition>
    {
        /// <summary>
        /// 操作逻辑
        /// </summary>
        IMessageReceiverService _messageReceiverService = null;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="messageReceiverService"></param>
        public MessageReceiverController(IMessageReceiverService messageReceiverService) : base(messageReceiverService)
        {
            this._messageReceiverService = messageReceiverService;

        }
        
    }
}