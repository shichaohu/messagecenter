using HS.Message.Model;
using HS.Message.Repository.repository.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.MessageEmitter;
using HS.Message.Share.MessageEmitter.Params;
using HS.Rabbitmq.Core;
using HS.Rabbitmq.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace HS.Message.Service.core.imp
{
    /// <summary>
    /// 消息队列消费服务
    /// </summary>
    public class MqMessageConsumerService : IConsumerCallBack
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MqMessageConsumerService> _logger;
        private string operatorTag = "【队列消费】";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public MqMessageConsumerService(IServiceProvider serviceProvider, ILogger<MqMessageConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<ConsumerResponse>> RunAsync(QueueMessage message)
        {
            switch (message.MessageType)
            {
                case QueueMessageType.Email:
                    return await ConsumerMqMessageByEmail(message);
                case QueueMessageType.SMS:
                    return await ConsumerMqMessageBySMS(message);
                default:
                    var result = new BaseResponse<ConsumerResponse>
                    {
                        Ret = ResponseCode.Success,
                        Data = new ConsumerResponse
                        {
                            Successed = true,
                            Message = message
                        }
                    };
                    return result;
            }
        }

        private async Task<BaseResponse<ConsumerResponse>> ConsumerMqMessageByEmail(QueueMessage message)
        {
            string logPrefix = $"{operatorTag}【{message.MessageType}:{message.MessageId}】";
            var result = new BaseResponse<ConsumerResponse>
            {
                Ret = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true,
                    Message = message
                }
            };
            try
            {
                var mailMessage = JsonConvert.DeserializeObject<MMailMessage>(message.MessageContent);
                if (mailMessage != null && !string.IsNullOrWhiteSpace(mailMessage.ReceiverEmail))
                {
                    //to do send email
                    var sender = _serviceProvider.GetService<SendMailEmitter>();
                    var sendMessage = new MailMParameter
                    {
                        SmtpService = mailMessage.SmtpService,
                        SendEmail = mailMessage.SendEmail,
                        SendPwd = mailMessage.SendPwd,
                        ReceiverEmails = string.IsNullOrWhiteSpace(mailMessage.ReceiverEmail) ? null : mailMessage.ReceiverEmail.Replace("，", ",").Split(','),
                        ReceiverCcEmails = string.IsNullOrWhiteSpace(mailMessage.ReceiverCcEmail) ? null : mailMessage.ReceiverCcEmail.Replace("，", ",").Split(','),
                        MailTitle = mailMessage.MailTitle,
                        MailBody = mailMessage.MailBody
                    };
                    var sendResponse = await sender.SendMailByMailKitMassed(sendMessage);
                    string sendSuccessMessage = $"{logPrefix}邮件发送成功 ";
                    if (sendMessage.ReceiverEmails != null)
                    {
                        sendSuccessMessage += $" 接收邮箱：{string.Join(",", sendMessage.ReceiverEmails)}";
                    }
                    if (sendMessage.ReceiverCcEmails != null)
                    {
                        sendSuccessMessage += $" 抄送邮箱：{string.Join(",", sendMessage.ReceiverCcEmails)}";
                    }
                    _logger.LogInformation(sendSuccessMessage);

                    //email send log
                    var mailSendLogs = new MMailSendLogs()
                    {
                        LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                        MailMessageId = mailMessage.LogicalId,
                        MailTitle = mailMessage.MailTitle,
                        MailBody = mailMessage.MailBody,
                        MailConfiguerId = mailMessage.MailConfiguerId,
                        ReceiverEmail = mailMessage.ReceiverEmail,
                        ReceiverCcEmail = mailMessage.ReceiverCcEmail,
                        SendTime = DateTime.Now,
                        SendState = sendResponse.Ret == ResponseCode.Success ? 1 : 2,
                        SendResult = JsonConvert.SerializeObject(sendResponse),
                        CreatedById = "message center",
                        CreatedByName = "message center",
                        CreatedTime = DateTime.Now,
                    };
                    mailMessage.SendState = sendResponse.Ret == ResponseCode.Success ? 2 : 3;
                    mailMessage.LastSendTime = DateTime.Now;
                    mailMessage.UpdatedTime = DateTime.Now;
                    mailMessage.UpdatedById = "message center";
                    mailMessage.UpdatedByName = "message center";
                    mailMessage.TotalSendNum++;

                    bool successed = false;
                    bool stepOne = false;//第一步执行结果
                    bool stepTwo = false;//第二步执行结果
                    bool hasContentWritedToDb = message.hasContentWritedToDb;//消息内容是否已经写入数据库
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var mqMsgConsumRepository = _serviceProvider.GetService<IMqMessageConsumerRepository>();
                        //更新邮件主表
                        if (hasContentWritedToDb)
                        {
                            var res = await mqMsgConsumRepository.UpdateMailMessageByIdAsync(mailMessage);
                            stepOne = res > 0;
                            _logger.LogInformation($"{logPrefix}邮件状态更新成功...");
                        }
                        else
                        {
                            var res = await mqMsgConsumRepository.AddOneMailMessageAsync(mailMessage);
                            stepOne = res > 0;
                            _logger.LogInformation($"{logPrefix}邮件信息落库成功...");
                        }
                        //写邮件发送日志
                        var res2 = await mqMsgConsumRepository.AddOneMailSendLogsAsync(mailSendLogs);
                        stepTwo = res2 > 0;
                        _logger.LogInformation($"{logPrefix}邮件发送日志落库完成...");

                        //更新消息接收表.邮件发送状态
                        var res3 = await mqMsgConsumRepository.UpdateMessageReceiverByIdAsync(new MMessageReceiver
                        {
                            LogicalId = mailMessage.ReceiverId,
                            EmailLastSendtime = DateTime.Now,
                            EmailSendState = sendResponse.Ret == ResponseCode.Success ? 2 : 3
                        });
                        stepTwo = stepTwo && res3 > 0;
                        //更新消息结束表邮件发送状态
                        var res4 = await mqMsgConsumRepository.UpdateMessageByIdAsync(new MMessage
                        {
                            LogicalId = mailMessage.MessageId
                        });
                        stepTwo = stepTwo && res4 > 0;

                        scope.Complete();
                    }
                    if (!message.hasContentWritedToDb && stepOne) message.hasContentWritedToDb = true;
                    successed = stepOne && stepTwo;

                    if (successed)
                    {
                        result.Data.Successed = true;
                    }
                    else
                    {
                        result.Ret = ResponseCode.DataError;
                        result.Data.Successed = false;

                        string errorMsg = $"add MailSendLogs failure,id: {JsonConvert.SerializeObject(message)}";
                        result.Msg = errorMsg;
                        _logger.LogInformation($"{logPrefix}{errorMsg}");
                    }
                }
                else
                {
                    result.Ret = ResponseCode.DataError;
                    result.Data.Successed = false;
                    string errorMsg = $"消息内容构造失败或者没有邮件接收邮箱，id: {JsonConvert.SerializeObject(message)}";
                    result.Msg = errorMsg;
                    _logger.LogInformation($"{logPrefix}{errorMsg}");
                }
            }
            catch (Exception ex)
            {
                result.Ret = ResponseCode.InternalError;
                result.Data.Successed = false;
                string errorMsg = $"消费Email队列的数据出错,message: {ex}";
                result.Msg = errorMsg;
                _logger.LogInformation($"{logPrefix}{errorMsg}");
            }

            return result;
        }
        private async Task<BaseResponse<ConsumerResponse>> ConsumerMqMessageBySMS(QueueMessage message)
        {
            string logPrefix = $"{operatorTag}【{message.MessageType}:{message.MessageId}】";
            var result = new BaseResponse<ConsumerResponse>
            {
                Ret = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true,
                    Message = message
                }
            };
            try
            {
                var smsMessage = JsonConvert.DeserializeObject<MSmsMessage>(message.MessageContent);
                if (smsMessage != null && !string.IsNullOrWhiteSpace(smsMessage.PhoneNumbers))
                {
                    //to do send sms
                    Thread.Sleep(5000);
                    _logger.LogInformation($"{logPrefix}短息发送成功：{smsMessage.PhoneNumbers}");

                    //save log
                    var smsSendLogs = new MSmsMessageDetails()
                    {
                        LogicalId = Guid.NewGuid().ToString().Replace("-", ""),
                        SmsMessageId = smsMessage.LogicalId,
                        ChannelCode = smsMessage.ChannelCode,
                        ChannelName = smsMessage.ChannelName,
                        Content = smsMessage.Content,
                        PhoneNumber = smsMessage.PhoneNumbers,
                        HasSendNum = 1,
                        SendState = 1,
                        //operator_state_code="",
                        //lock_time=DateTime.Now,
                        SubmitTime = DateTime.Now,
                        ReceiveTime = DateTime.Now,
                        LastSendTime = DateTime.Now,
                        //biz_id=smsMessage.Data.biz_id,
                        //request_id=smsMessage.Data.request_id,
                        CreatedById = "message center",
                        CreatedByName = "message center",
                        CreatedTime = DateTime.Now,
                    };
                    smsMessage.SendState = 3;
                    smsMessage.UpdatedTime = DateTime.Now;
                    smsMessage.UpdatedById = "message center";
                    smsMessage.UpdatedByName = "message center";

                    bool successed = false;
                    bool stepOne = false;//第一步执行结果
                    bool stepTwo = false;//第二步执行结果
                    bool hasContentWritedToDb = message.hasContentWritedToDb;//消息内容是否已经写入数据库
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var mqMsgConsumRepository = _serviceProvider.GetService<IMqMessageConsumerRepository>();

                        if (hasContentWritedToDb)
                        {
                            var res = await mqMsgConsumRepository.UpdateSmsMessageByIdAsync(smsMessage);
                            stepOne = res > 0;
                            _logger.LogInformation($"{logPrefix}短息状态更新成功...");
                        }
                        else
                        {
                            var res = await mqMsgConsumRepository.AddOneSmsMessageAsync(smsMessage);
                            stepOne = res > 0;
                            _logger.LogInformation($"{logPrefix}短息信息落库成功...");
                        }
                        var res2 = await mqMsgConsumRepository.AddOneSmsMessageDetailsAsync(smsSendLogs);
                        stepTwo = res2 > 0;
                        _logger.LogInformation($"{logPrefix}短息发送日志落库完成...");

                        scope.Complete();
                    }

                    if (!message.hasContentWritedToDb && stepOne) message.hasContentWritedToDb = true;
                    successed = stepOne && stepTwo;

                    if (successed)
                    {
                        result.Data.Successed = true;
                    }
                    else
                    {
                        result.Ret = ResponseCode.DataError;
                        result.Data.Successed = false;

                        string errorMsg = $"add SmsMessageDetails failure,id: {JsonConvert.SerializeObject(message)}";
                        result.Msg = errorMsg;
                        _logger.LogInformation($"{logPrefix}{errorMsg}");
                    }
                }
                else
                {
                    result.Ret = ResponseCode.DataError;
                    result.Data.Successed = false;

                    string errorMsg = $"消息内容构造失败或者没有短息接收号码，id: {JsonConvert.SerializeObject(message)}";
                    result.Msg = errorMsg;
                    _logger.LogInformation($"{logPrefix}{errorMsg}");
                }
            }
            catch (Exception ex)
            {
                result.Ret = ResponseCode.InternalError;
                result.Data.Successed = false;

                string errorMsg = $"消费SMS队列的数据出错,message: {ex}";
                result.Msg = errorMsg;
                _logger.LogInformation($"{logPrefix}{errorMsg}");
            }
            return result;
        }
    }

}