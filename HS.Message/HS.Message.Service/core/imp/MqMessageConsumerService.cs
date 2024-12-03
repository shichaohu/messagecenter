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
        private string operatorTag= "【队列消费】";

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
                        Code = ResponseCode.Success,
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
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true,
                    Message = message
                }
            };
            try
            {
                var mailMessage = JsonConvert.DeserializeObject<MMailMessage>(message.MessageContent);
                if (mailMessage != null)
                {
                    //to do send email
                    var sender = _serviceProvider.GetService<SendMailEmitter>();
                    var sendMessage = new MailMParameter
                    {
                        SmtpService = mailMessage.smtp_service,
                        SendEmail = mailMessage.send_email,
                        SendPwd = mailMessage.send_pwd,
                        ReceiverEmails = mailMessage.receiver_email.Split(','),
                        ReceiverCcEmails = mailMessage.receiver_cc_email.Split(','),
                        MailTitle = mailMessage.mail_title,
                        MailBody = mailMessage.mail_body
                    };
                    var sendResponse = await sender.SendMailByMailKitMassed(sendMessage);
                    _logger.LogInformation($"{logPrefix}邮件发送成功：{string.Join(",", sendMessage.ReceiverEmails)},抄送：{string.Join(",", sendMessage.ReceiverCcEmails)}");
                    //email send log
                    var mailSendLogs = new MMailSendLogs()
                    {
                        logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                        mail_message_id = mailMessage.logical_id,
                        mail_title = mailMessage.mail_title,
                        mail_body = mailMessage.mail_body,
                        mail_configuer_id = mailMessage.mail_configuer_id,
                        receiver_email = mailMessage.receiver_email,
                        receiver_cc_email = mailMessage.receiver_cc_email,
                        send_time = DateTime.Now,
                        send_state = sendResponse.Code == ResponseCode.Success ? 1 : 2,
                        send_result = JsonConvert.SerializeObject(sendResponse),
                        created_by_id = "message center",
                        created_by_name = "message center",
                        created_time = DateTime.Now,
                    };
                    mailMessage.send_state = 3;
                    mailMessage.last_send_time = DateTime.Now;
                    mailMessage.updated_time = DateTime.Now;
                    mailMessage.updated_by_id = "message center";
                    mailMessage.updated_by_name = "message center";
                    mailMessage.total_send_num++;

                    bool successed = false;
                    bool stepOne = false;//第一步执行结果
                    bool stepTwo = false;//第二步执行结果
                    bool hasContentWritedToDb = message.hasContentWritedToDb;//消息内容是否已经写入数据库
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var mqMsgConsumRepository = _serviceProvider.GetService<IMqMessageConsumerRepository>();

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
                        var res2 = await mqMsgConsumRepository.AddOneMailSendLogsAsync(mailSendLogs);
                        stepTwo = res2 > 0;
                        _logger.LogInformation($"{logPrefix}邮件发送日志落库完成...");

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
                        result.Code = ResponseCode.DataError;
                        result.Data.Successed = false;

                        string errorMsg = $"add MailSendLogs failure,id: {JsonConvert.SerializeObject(message)}";
                        result.Message = errorMsg;
                        _logger.LogInformation($"{logPrefix}{errorMsg}");
                    }
                }
                else
                {
                    result.Code = ResponseCode.DataError;
                    result.Data.Successed = false;
                    string errorMsg = $"消息内容构造失败,id: {JsonConvert.SerializeObject(message)}";
                    result.Message = errorMsg;
                    _logger.LogInformation($"{logPrefix}{errorMsg}");
                }
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.InternalError;
                result.Data.Successed = false;
                string errorMsg = $"消费Email队列的数据出错,message: {ex}";
                result.Message = errorMsg;
                _logger.LogInformation($"{logPrefix}{errorMsg}");
            }

            return result;
        }
        private async Task<BaseResponse<ConsumerResponse>> ConsumerMqMessageBySMS(QueueMessage message)
        {
            string logPrefix = $"{operatorTag}【{message.MessageType}:{message.MessageId}】";
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true,
                    Message = message
                }
            };
            try
            {
                var smsMessage = JsonConvert.DeserializeObject<MSmsMessage>(message.MessageContent);
                if (smsMessage != null)
                {
                    //to do send sms
                    Thread.Sleep(5000);
                    _logger.LogInformation($"{logPrefix}短息发送成功：{smsMessage.phone_numbers}");

                    //save log
                    var smsSendLogs = new MSmsMessageDetails()
                    {
                        logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                        sms_message_id = smsMessage.logical_id,
                        channel_code = smsMessage.channel_code,
                        channel_name = smsMessage.channel_name,
                        content = smsMessage.content,
                        phone_number = smsMessage.phone_numbers,
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
                    smsMessage.send_state = 3;
                    smsMessage.updated_time = DateTime.Now;
                    smsMessage.updated_by_id = "message center";
                    smsMessage.updated_by_name = "message center";

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
                        result.Code = ResponseCode.DataError;
                        result.Data.Successed = false;

                        string errorMsg = $"add SmsMessageDetails failure,id: {JsonConvert.SerializeObject(message)}";
                        result.Message = errorMsg;
                        _logger.LogInformation($"{logPrefix}{errorMsg}");
                    }
                }
                else
                {
                    result.Code = ResponseCode.DataError;
                    result.Data.Successed = false;

                    string errorMsg = $"消息内容构造失败,id: {JsonConvert.SerializeObject(message)}";
                    result.Message = errorMsg;
                    _logger.LogInformation($"{logPrefix}{errorMsg}");
                }
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.InternalError;
                result.Data.Successed = false;

                string errorMsg = $"消费SMS队列的数据出错,message: {ex}";
                result.Message = errorMsg;
                _logger.LogInformation($"{logPrefix}{errorMsg}");
            }
            return result;
        }
    }

}