using HS.Message.Model;
using HS.Message.Repository.repository.core;
using HS.Message.Repository.repository.core.imp;
using HS.Message.Share.BaseModel;
using HS.Message.Share.MessageEmitter;
using HS.Message.Share.MessageEmitter.Params;
using HS.Rabbitmq.Core;
using HS.Rabbitmq.Model;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MqMessageConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<ConsumerResponse>> RunAsync(QueueMessage message)
        {
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true
                }
            };
            switch (message.MessageType)
            {
                case QueueMessageType.Email:
                    return await ConsumerMqMessageByEmail(message);
                case QueueMessageType.SMS:
                    return await ConsumerMqMessageBySMS(message);
                default:
                    break;
            }


            return result;
        }

        private async Task<BaseResponse<ConsumerResponse>> ConsumerMqMessageByEmail(QueueMessage message)
        {
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true
                }
            };
            try
            {
                var repository = _serviceProvider.GetService<IMqMessageConsumerRepository>();
                var mailMessage = await repository.GetMailMessageByIdAsync(message.MessageContent);
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
                    //save log
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
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var response1 = await repository.UpdateMailMessageByIdAsync(mailMessage);
                        var response2 = await repository.AddOneMailSendLogsAsync(mailSendLogs);
                        scope.Complete();
                        successed = response1 > 0 && response2 > 0;
                    }

                    if (successed)
                    {
                        result.Data = new ConsumerResponse
                        {
                            Successed = true
                        };
                    }
                    else
                    {
                        result.Code = ResponseCode.DataError;
                        result.Data = new ConsumerResponse
                        {
                            Successed = false
                        };
                        result.Message = $"add MailSendLogs failure,id: {message.MessageContent}";
                    }
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
            catch (Exception ex)
            {
                result.Code = ResponseCode.InternalError;
                result.Data = new ConsumerResponse
                {
                    Successed = false
                };
                result.Message = $"消费Email队列的数据出错,message: {ex}";
            }

            return result;
        }
        private async Task<BaseResponse<ConsumerResponse>> ConsumerMqMessageBySMS(QueueMessage message)
        {
            var result = new BaseResponse<ConsumerResponse>
            {
                Code = ResponseCode.Success,
                Data = new ConsumerResponse
                {
                    Successed = true
                }
            };
            try
            {
                var repository = _serviceProvider.GetService<IMqMessageConsumerRepository>();
                var smsMessage = await repository.GetSmsMessageByIdAsync(message.MessageContent);
                if (smsMessage != null)
                {
                    //to do send sms
                    Thread.Sleep(5000);

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
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var response1 = await repository.UpdateSmsMessageByIdAsync(smsMessage);
                        var response2 = await repository.AddOneSmsMessageDetailsAsync(smsSendLogs);
                        scope.Complete();
                        successed = response1 > 0 && response2 > 0;
                    }

                    if (successed)
                    {
                        result.Data = new ConsumerResponse
                        {
                            Successed = true
                        };
                    }
                    else
                    {
                        result.Code = ResponseCode.DataError;
                        result.Data = new ConsumerResponse
                        {
                            Successed = false
                        };
                        result.Message = $"add SmsMessageDetails failure,id: {message.MessageContent}";
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
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.InternalError;
                result.Data = new ConsumerResponse
                {
                    Successed = false
                };
                result.Message = $"消费SMS队列的数据出错,message: {ex}";
            }
            return result;
        }
    }

}