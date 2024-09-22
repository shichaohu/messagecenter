
using HS.Message.Controllers.Base;
using HS.Message.Model.Requests;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;

namespace HS.Message.Controllers
{
    /// <summary>
    /// 消息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiGroup(ApiGroupNames.Message)]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        /// <summary>
        /// 通过构造函数依赖注入
        /// </summary>
        /// <param name="messageService"></param>
        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }
        #region 自定义接口
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("sendmessage")]
        [HttpPost]
        public async Task<BaseResponse> SendMessageAsync(List<MessageRequest> request)
        {
            return await _messageService.SendMessageAsync(request);
        }

        #endregion
    }
}