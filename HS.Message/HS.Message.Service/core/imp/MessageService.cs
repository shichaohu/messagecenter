using HS.Message.Model;
using HS.Message.Model.Enums;
using HS.Message.Model.Requests;
using HS.Message.Repository.repository.core;
using HS.Message.Repository.repository.core.imp;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using HS.Rabbitmq.Core;
using HS.Rabbitmq.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 消息服务
    /// </summary>
    public class MessageService : BaseService<MMessage, MMessageCondition>,IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IMessageRepository<MMessage, MMessageCondition> _messageRepository;
        private readonly IMessageReceiverService _messageReceiverService;
        private readonly IMailConfigureService _mailConfigureService;
        private readonly IMailMessageService _mailMessageService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly ISmsConfigureService _smsConfigureService;
        private readonly ISmsMessageService _smsMessageService;
        private readonly ISmsTemplateService _smsTemplateService;
        private readonly IMailSendLogsService _mailSendLogsService;
        private readonly ISmsMessageDetailsService _mailMessageDetailsService;
        private readonly IInjectedObjects _injectedObjects;
        private readonly IDistributedCache _cache;
        private readonly RabbitmqTopicProducer _rabbitmqTopicProducer;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="messageRepository"></param>
        /// <param name="messageReceiverService"></param>
        /// <param name="mailConfigureService"></param>
        /// <param name="mailMessageService"></param>
        /// <param name="mailTemplateService"></param>
        /// <param name="smsConfigureService"></param>
        /// <param name="smsTemplateService"></param>
        /// <param name="mailSendLogsService"></param>
        /// <param name="mailMessageDetailsService"></param>
        /// <param name="smsMessageService"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        /// <param name="rabbitmqTopicProducer"></param>
        /// <param name="configuration"></param>
        public MessageService(
            ILogger<MessageService> logger,
            IMessageRepository<MMessage, MMessageCondition> messageRepository,
            IMessageReceiverService messageReceiverService,

            IMailConfigureService mailConfigureService,
            IMailMessageService mailMessageService,
            IMailTemplateService mailTemplateService,
            ISmsConfigureService smsConfigureService,
            ISmsMessageService smsMessageService,
            ISmsTemplateService smsTemplateService,
            IMailSendLogsService mailSendLogsService,
            ISmsMessageDetailsService mailMessageDetailsService,

            IInjectedObjects injectedObjects,
            IDistributedCache cache,
            RabbitmqTopicProducer rabbitmqTopicProducer,
        IConfiguration configuration
            ) 
            : base(messageRepository, injectedObjects, "Message")
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _messageReceiverService = messageReceiverService;
            _mailConfigureService = mailConfigureService;
            _mailMessageService = mailMessageService;
            _mailTemplateService = mailTemplateService;
            _smsConfigureService = smsConfigureService;
            _smsMessageService = smsMessageService;
            _smsTemplateService = smsTemplateService;
            _mailSendLogsService = mailSendLogsService;
            _mailMessageDetailsService = mailMessageDetailsService;
            _injectedObjects = injectedObjects;
            _cache = cache;
            _rabbitmqTopicProducer = rabbitmqTopicProducer;
            _configuration = configuration;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendMessageAsync(List<MessageRequest> request)
        {

            string logPrefix = $"【消息中心】";
            _logger.LogInformation($"{logPrefix}开始处理消息:{JsonConvert.SerializeObject(request)}");
            var result = new BaseResponse()
            {
                Code = ResponseCode.ParameterError
            };
            if (request == null)
            {
                result.Code = ResponseCode.ParameterError;
                result.Message = $"消息不能为空！";
                return result;
            }
            List<MMessage> messageList = new();
            List<MMessageReceiver> messageReceiveList = new();
            List<MMailMessage> mailMessageList = new();
            List<MSmsMessage> smsMessageList = new();
            foreach (var item in request)
            {
                if (!(item.Receiver?.Count > 0) || item.Sender == null)
                {
                    result.Code = ResponseCode.ParameterError;
                    result.Message = $"消息发送人和接收人不能为空！";
                    return result;
                }
                if (string.IsNullOrEmpty(item.Title))
                {
                    result.Message = $"消息标题不能为空！";
                    return result;
                }
                if (string.IsNullOrEmpty(item.Content))
                {
                    result.Message = $"消息内容不能为空！";
                    return result;
                }
                if (string.IsNullOrEmpty(item.BusinessTypeKey) || string.IsNullOrEmpty(item.BusinessTypeValue))
                {
                    result.Message = $"消息类型不能为空！";
                    return result;
                }
                if (string.IsNullOrEmpty(item.Sendchannel))
                {
                    result.Message = $"消息发送渠道不能为空！";
                    return result;
                }

                //1、分解消息
                _logger.LogInformation($"{logPrefix}分解消息...");
                var sendChannels = item.Sendchannel.Split(',').Select(x => Enum.Parse<MessageSendchannel>(x)).ToList();
                bool needEmail = sendChannels.Any(x => x == MessageSendchannel.Email);//是否需要发邮件
                bool needSMS = sendChannels.Any(x => x == MessageSendchannel.SMS);//是否需要发短信
                _logger.LogInformation($"{logPrefix}是否需要发邮件：{needEmail},");
                _logger.LogInformation($"{logPrefix}是否需要发短信：{needSMS},");

                var message = new MMessage()
                {
                    LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                    Title = item.Title,
                    Content = item.Content,
                    DynamicParameter = JsonConvert.SerializeObject(item.DynamicParameter, Formatting.Indented),
                    LinkUrl = item.LinkUrl,
                    LinkText = item.LinkText,
                    BusinessTypeKey = item.BusinessTypeKey,
                    BusinessTypeValue = item.BusinessTypeValue,
                    SendChannel = item.Sendchannel,
                    SendState = 1,
                    CreatedById = item.Sender.UserId,
                    CreatedByName = item.Sender.UserName,
                    CreatedTime = DateTime.Now,
                };
                messageList.Add(message);
                var messageReceives = item.Receiver.Select(x => new MMessageReceiver
                {
                    LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                    MessageId = message.LogicalId,
                    ReceiverUserid = x.ReceiverUserid,
                    ReceiverName = x.ReceiverName,
                    Email = x.Email,
                    CcEmail = x.CcEmail,
                    EmailSendState = needEmail ? 1 : null,
                    Phone = x.Phone,
                    EnterpriseWechat = x.EnterpriseWechat,
                    SmsSendState = needSMS ? 1 : null,
                    Dingtalk = x.Dingtalk,
                    OtherReceiveChannel = x.OtherReceiveChannel,
                    CreatedById = item.Sender.UserId,
                    CreatedByName = item.Sender.UserName,
                    CreatedTime = DateTime.Now,
                }).ToList();
                messageReceiveList.AddRange(messageReceives);
                if (needEmail)
                {
                    var mailTemplate = await _mailTemplateService.GetOneModelAsync(new MMailTemplateCondition
                    {
                        BusinessTypeKey = message.BusinessTypeKey,
                        State = 1
                    });
                    var mailMessages = messageReceives.Select(x => new MMailMessage
                    {
                        LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                        MessageId = message.LogicalId,
                        SendEmail = _configuration["EmailConfig:SendEmail"],//"502242999@qq.com",
                        SendPwd = _configuration["EmailConfig:SendPassowrd"],//vkspnryltkotbjhc
                        SmtpService = _configuration["EmailConfig:SmtpService"],
                        ReceiverId = x.LogicalId,
                        MailTemplateId = mailTemplate?.Data?.LogicalId,
                        MailConfiguerId = "",
                        MailTitle = MessageUtil.LoadMessageTemplate(message.Title, mailTemplate?.Data?.TempTitle, item.DynamicParameter),
                        MailBody = MessageUtil.LoadMessageTemplate(message.Content, mailTemplate?.Data?.TempBody, item.DynamicParameter),
                        ReceiverEmail = x.Email,
                        ReceiverCcEmail = x.CcEmail,
                        TotalSendNum = 1,
                        StartSendTime = DateTime.Now,
                        HasSendNum = 0,
                        SendState = 1,
                        LastSendTime = DateTime.Now,
                        CreatedById = x.CreatedById,
                        CreatedByName = x.CreatedByName,
                        CreatedTime = DateTime.Now
                    }).ToList();
                    mailMessageList.AddRange(mailMessages);

                };
                if (needSMS)
                {
                    var smsTemplate = await _smsTemplateService.GetOneModelAsync(new MSmsTemplateCondition
                    {
                        BusinessTypeKey = message.BusinessTypeKey,
                        State = 1
                    });
                    var smsMessages = messageReceives.Select(x => new MSmsMessage
                    {
                        LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                        MessageId = message.LogicalId,
                        ReceiverId = x.LogicalId,
                        TemplateCode = smsTemplate?.Data?.TempCode,
                        TemplateParam = message.DynamicParameter,
                        Content = MessageUtil.LoadMessageTemplate(message.Content, smsTemplate?.Data?.TempBody, item.DynamicParameter),
                        SendType = 1,
                        PhoneNumbers = x.Phone,
                        SendState = 1,
                        CreatedById = x.CreatedById,
                        CreatedByName = x.CreatedByName,
                        CreatedTime = DateTime.Now
                    }).ToList();

                    smsMessageList.AddRange(smsMessages);
                };
            }

            if (mailMessageList?.Count > 0)
            {
                List<QueueMessage> queueMessageList = mailMessageList.Select(x => new QueueMessage
                {
                    MessageType = QueueMessageType.Email,
                    MessageContent = JsonConvert.SerializeObject(x),
                    MessageId = x.LogicalId,
                    hasContentWritedToDb = false
                }).ToList();

                _logger.LogInformation($"{logPrefix}邮件消息写入队列：{JsonConvert.SerializeObject(queueMessageList)},");
                var mqresponse = _rabbitmqTopicProducer.BatchProducer(queueMessageList);
                if (!mqresponse.Successed)
                {
                    result.Code = ResponseCode.InternalError;
                    result.Message = mqresponse.Message;
                    return result;
                }
            }
            if (smsMessageList?.Count > 0)
            {
                List<QueueMessage> queueMessageList = smsMessageList.Select(x => new QueueMessage
                {
                    MessageType = QueueMessageType.SMS,
                    MessageContent = JsonConvert.SerializeObject(x),
                    MessageId = x.LogicalId,
                    hasContentWritedToDb = false
                }).ToList();

                _logger.LogInformation($"{logPrefix}短息消息写入队列：{JsonConvert.SerializeObject(queueMessageList)},");
                var mqresponse = _rabbitmqTopicProducer.BatchProducer(queueMessageList);
                if (!mqresponse.Successed)
                {
                    result.Code = ResponseCode.InternalError;
                    result.Message = mqresponse.Message;
                    return result;
                }
            }
            int insertCount = 0;
            List<string> errorMessage = new List<string>();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                insertCount = await _messageRepository.BactchAddAsync(messageList);
                var res2 = await _messageReceiverService.BactchAddAsync(messageReceiveList);
                if (res2?.Code != ResponseCode.Success)
                {
                    errorMessage.Add(res2?.Message);
                }

                scope.Complete();
            }
            if (insertCount == 0)
            {
                result.Code = ResponseCode.InternalError;
                result.Message = "消息写入数据库时，事物提交失败！";
                return result;
            }

            result.Code = ResponseCode.Success;
            result.Message = "操作成功，消息已经写入发送队列！";

            return result;

        }
    }

}