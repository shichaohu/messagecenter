using HS.Message.Model;
using HS.Message.Model.Enums;
using HS.Message.Model.Requests;
using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using HS.Rabbitmq;
using HS.Rabbitmq.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 消息服务
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository<MMessage, MMessageCondtion> _messageRepository;
        private readonly IMessageReceiverService _messageReceiverService;
        private readonly IMailConfigureService _mailConfigureService;
        private readonly IMailMessageService _mailMessageService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly ISmsConfigureService _smsConfigureService;
        private readonly ISmsMessageService _smsMessageService;
        private readonly ISmsTemplateService _smsTemplateService;
        private readonly IMailSendLogsService _mailSendLogsService;
        private readonly ISmsMessageDetailsService _mailMessageDetailsService;
        private readonly IDistributedCache _cache;
        private readonly RabbitmqTopic _rabbitmq;

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
        /// <param name="mailSendLogsService"></param>
        /// <param name="mailMessageDetailsService"></param>
        /// <param name="smsMessageService"></param>
        /// <param name="injectedObjects"></param>
        /// <param name="cache"></param>
        /// <param name="rabbitmq"></param>
        public MessageService(
            IMessageRepository<MMessage, MMessageCondtion> messageRepository,
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
            RabbitmqTopic rabbitmq
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
            _mailSendLogsService = mailSendLogsService;
            _mailMessageDetailsService = mailMessageDetailsService;
            _cache = cache;
            _rabbitmq = rabbitmq;
            _rabbitmq.ConsumerFunc = ConsumerMqMessageStrategy;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendMessageAsync(List<MessageRequest> request)
        {
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
                var sendChannels = item.Sendchannel.Split(',').Select(x => Enum.Parse<MessageSendchannel>(x)).ToList();
                bool needEmail = sendChannels.Any(x => x == MessageSendchannel.Email);//是否需要发邮件
                bool needShortMessage = sendChannels.Any(x => x == MessageSendchannel.SMS);//是否需要发短信

                var message = new MMessage()
                {
                    logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                    title = item.Title,
                    content = item.Content,
                    dynamic_parameter = JsonConvert.SerializeObject(item.DynamicParameter, Formatting.Indented),
                    link_url = item.LinkUrl,
                    link_text = item.LinkText,
                    business_type_key = item.BusinessTypeKey,
                    business_type_value = item.BusinessTypeValue,
                    sendchannel = item.Sendchannel,
                    created_by_id = item.Sender.UserId,
                    created_by_name = item.Sender.UserName,
                    created_time = DateTime.Now,
                };
                messageList.Add(message);
                var messageReceives = item.Receiver.Select(x => new MMessageReceiver
                {
                    logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                    message_id = message.logical_id,
                    receiver_userid = x.ReceiverUserid,
                    receiver_name = x.ReceiverName,
                    email = x.Email,
                    cc_email = x.CcEmail,
                    phone = x.Phone,
                    enterprise_wechat = x.EnterpriseWechat,
                    dingtalk = x.Dingtalk,
                    other_receive_channel = x.OtherReceiveChannel,
                    created_by_id = item.Sender.UserId,
                    created_by_name = item.Sender.UserName,
                    created_time = DateTime.Now,
                }).ToList();
                messageReceiveList.AddRange(messageReceives);
                if (needEmail)
                {
                    var mailTemplate = await _mailTemplateService.GetOneModelAsync(new MMailTemplateCondtion
                    {
                        business_type_key = message.business_type_key,
                        state = 1
                    });
                    var mailMessages = messageReceives.Select(x => new MMailMessage
                    {
                        logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                        message_id = message.logical_id,
                        receiver_id = x.logical_id,
                        mail_template_id = mailTemplate?.Data?.logical_id,
                        mail_configuer_id = "",
                        mail_title = MessageUtil.LoadMessageTemplate(message.title, mailTemplate?.Data?.temp_title, item.DynamicParameter),
                        mail_body = MessageUtil.LoadMessageTemplate(message.content, mailTemplate?.Data?.temp_body, item.DynamicParameter),
                        receiver_email = x.email,
                        receiver_cc_email = x.cc_email,
                        total_send_num = 1,
                        start_send_time = DateTime.Now,
                        has_send_num = 0,
                        send_state = 1,
                        created_by_id = x.created_by_id,
                        created_by_name = x.created_by_name,
                        created_time = DateTime.Now
                    }).ToList();
                    mailMessageList.AddRange(mailMessages);

                };
                if (needShortMessage)
                {
                    var smsTemplate = await _smsTemplateService.GetOneModelAsync(new MSmsTemplateCondtion
                    {
                        business_type_key = message.business_type_key,
                        state = 1
                    });
                    var smsMessages = messageReceives.Select(x => new MSmsMessage
                    {
                        logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                        message_id = message.logical_id,
                        receiver_id = x.logical_id,
                        template_code = smsTemplate?.Data?.temp_code,
                        template_param = message.dynamic_parameter,
                        content = MessageUtil.LoadMessageTemplate(message.content, smsTemplate?.Data?.temp_body, item.DynamicParameter),
                        send_type = 1,
                        phone_numbers = x.phone,
                        send_state = 1,
                        created_by_id = x.created_by_id,
                        created_by_name = x.created_by_name,
                        created_time = DateTime.Now
                    }).ToList();

                    smsMessageList.AddRange(smsMessages);
                };
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var res1 = await _messageRepository.BactchAddAsync(messageList);
                var res2 = await _messageReceiverService.BactchAddAsync(messageReceiveList);

                if (mailMessageList?.Count > 0)
                {
                    List<QueueMessage> queueMessageList = mailMessageList.Select(x => new QueueMessage
                    {
                        MessageType = QueueMessageType.Email,
                        MessageContent = x.logical_id,
                        NameSpace = ""
                    }).ToList();

                    _rabbitmq.BatchProducer(queueMessageList);
                    var res3 = await _mailMessageService.BactchAddAsync(mailMessageList);
                }
                if (smsMessageList?.Count > 0)
                {
                    List<QueueMessage> queueMessageList = smsMessageList.Select(x => new QueueMessage
                    {
                        MessageType = QueueMessageType.SMS,
                        MessageContent = x.logical_id,
                        NameSpace = ""
                    }).ToList();

                    _rabbitmq.BatchProducer(queueMessageList);
                    var res4 = await _smsMessageService.BactchAddAsync(smsMessageList);
                }

                scope.Complete();
            }

            result.Code = ResponseCode.Success;
            result.Message = "操作成功，消息已经写入发送队列！";

            return result;

        }
        /// <summary>
        /// 消费MQ的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public BaseResponse<ConsumerResponse> ConsumerMqMessageStrategy(QueueMessage message)
        {
            switch (message.MessageType)
            {
                case QueueMessageType.Email:
                    return ConsumerMqMessageByEmail(message);
                case QueueMessageType.SMS:
                    return ConsumerMqMessageBySMS(message);
                default:
                    break;
            }
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success
            };


            return result;
        }
        private BaseResponse<ConsumerResponse> ConsumerMqMessageByEmail(QueueMessage message)
        {
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true
                }
            };
            var mailMessage = _mailMessageService.GetModelByIdAsync(message.MessageContent).Result;
            if (mailMessage != null && mailMessage.Code == ResponseCode.Success)
            {
                //to do send email

                //save log
                var mailSendLogs = new MMailSendLogs()
                {
                    logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                    mail_message_id = mailMessage.Data.logical_id,
                    mail_title = mailMessage.Data.mail_title,
                    mail_body = mailMessage.Data.mail_body,
                    mail_configuer_id = mailMessage.Data.mail_configuer_id,
                    receiver_email = mailMessage.Data.receiver_email,
                    receiver_cc_email = mailMessage.Data.receiver_cc_email,
                    send_time = DateTime.Now,
                    send_state = 1,
                    created_by_id = "message center",
                    created_by_name = "message center",
                    created_time = DateTime.Now,
                };
                var response = _mailSendLogsService.AddOneAsync(mailSendLogs).Result;
                if (response?.Code != ResponseCode.Success)
                {
                    result.Data = new ConsumerResponse
                    {
                        Successed = false
                    };
                }
                else
                {
                    result.Code = ResponseCode.DataError;
                    result.Data = new ConsumerResponse
                    {
                        Successed = false
                    };
                    result.Message = $"未找到Email队列对应的数据,id: {message.MessageContent}";
                }
            }
            return result;
        }
        private BaseResponse<ConsumerResponse> ConsumerMqMessageBySMS(QueueMessage message)
        {
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true
                }
            };
            var smsMessage = _smsMessageService.GetModelByIdAsync(message.MessageContent).Result;
            if (smsMessage != null && smsMessage.Code == ResponseCode.Success)
            {
                //to do send sms

                //save log
                var smsSendLogs = new MSmsMessageDetails()
                {
                    logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                    sms_message_id = smsMessage.Data.logical_id,
                    channel_code = smsMessage.Data.channel_code,
                    channel_name = smsMessage.Data.channel_name,
                    content = smsMessage.Data.content,
                    phone_number = smsMessage.Data.phone_numbers,
                    has_send_num = 1,
                    send_state = 1,
                    //operator_state_code="",
                    //lock_time=DateTime.Now,
                    submit_time = DateTime.Now,
                    receive_time = DateTime.Now,
                    last_send_time = DateTime.Now,
                    //biz_id=smsMessage.Data.biz_id,
                    //request_id=smsMessage.Data.request_id,
                    created_by_id = "message center",
                    created_by_name = "message center",
                    created_time = DateTime.Now,
                };
                var response = _mailMessageDetailsService.AddOneAsync(smsSendLogs).Result;
                if (response?.Code != ResponseCode.Success)
                {
                    result.Data = new ConsumerResponse
                    {
                        Successed = false
                    };
                }
            }
            else
            {
                result.Code = ResponseCode.DataError;
                result.Data = new ConsumerResponse
                {
                    Successed = false
                };
                result.Message = $"未找到SMS队列对应的数据,id: {message.MessageContent}";
            }
            return result;
        }
    }

}