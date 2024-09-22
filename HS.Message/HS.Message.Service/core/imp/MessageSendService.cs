using HS.Message.Model;
using HS.Message.Model.Enums;
using HS.Message.Model.Requests;
using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 消息发送服务
    /// </summary>
    public class MessageSendService : IMessageSendService
    {
        private readonly IMessageRepository<MMessage, MMessageCondtion> _messageRepository;
        private readonly IMessageReceiverService _messageReceiverService;
        private readonly IMailConfigureService _mailConfigureService;
        private readonly IMailMessageService _mailMessageService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly ISmsConfigureService _smsConfigureService;
        private readonly ISmsMessageService _smsMessageService;
        private readonly ISmsTemplateService _smsTemplateService;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageRepository"></param>
        /// <param name="messageReceiverService"></param>
        /// <param name="mailConfigureService"></param>
        /// <param name="mailMessageService"></param>
        /// <param name="mailTemplateService"></param>
        /// <param name="smsConfigureService"></param>
        /// <param name="smsTemplateService"></param>
        /// <param name="smsMessageService"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        public MessageSendService(
            IMessageRepository<MMessage, MMessageCondtion> messageRepository,
            IMessageReceiverService messageReceiverService,

            IMailConfigureService mailConfigureService,
            IMailMessageService mailMessageService,
            IMailTemplateService mailTemplateService,
            ISmsConfigureService smsConfigureService,
            ISmsMessageService smsMessageService,
            ISmsTemplateService smsTemplateService,

            IInjectedObjects injectedObjects,
            IDistributedCache cache
            )
        {
            _messageRepository = messageRepository;
            _messageReceiverService = messageReceiverService;
            _mailConfigureService = mailConfigureService;
            _mailMessageService = mailMessageService;
            _mailTemplateService = mailTemplateService;
            _smsConfigureService = smsConfigureService;
            _smsMessageService = smsMessageService;
            _smsTemplateService = smsTemplateService;

            _cache = cache;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> RunAsync()
        {
            var result = new BaseResponse()
            {
                Code = ResponseCode.Success
            };

            result.Code = ResponseCode.Success;
            result.Message = "操作成功，消息已经写入发送队列！";

            return result;

        }
        public async Task<BaseResponse> SendEmailAsync()
        {
            var result = new BaseResponse()
            {
                Code = ResponseCode.Success
            };

            var waitToSendEamils = await _mailMessageService.GetPageListAsync(
                new MPageQueryCondition<MMailMessageCondtion>
                {
                    condition = new MMailMessageCondtion
                    {
                        send_state = 1
                    },
                    orderby = "created_time",
                    pageIndex = 1,
                    pageSize = 100
                });
            if (waitToSendEamils.Code == ResponseCode.Success && waitToSendEamils.Data?.Count > 0)
            {
                waitToSendEamils.Data.AsParallel().ForAll(x => {
                
                });
            }
            result.Code = ResponseCode.Success;
            result.Message = "操作成功，消息已经写入发送队列！";

            return result;
        }
    }

}