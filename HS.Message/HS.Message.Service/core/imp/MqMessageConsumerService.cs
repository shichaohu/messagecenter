using HS.Message.Model;
using HS.Message.Model.Enums;
using HS.Message.Model.Requests;
using HS.Message.Repository.repository.core;
using HS.Message.Service.@base;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Redis;
using HS.Message.Share.Utils;
using HS.Rabbitmq.Core;
using HS.Rabbitmq.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
                    return ConsumerMqMessageByEmail(message);
                case QueueMessageType.SMS:
                    return ConsumerMqMessageBySMS(message);
                default:
                    break;
            }


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
            try
            {
                var repository = _serviceProvider.GetService<IMqMessageConsumerRepository>();
                var mailMessage = repository.GetMailMessageByIdAsync(message.MessageContent).Result;
                if (mailMessage != null)
                {
                    //to do send email
                    Thread.Sleep(5000);
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
                        send_state = 1,
                        created_by_id = "message center",
                        created_by_name = "message center",
                        created_time = DateTime.Now,
                    };
                    var response = repository.AddOneMailSendLogsAsync(mailSendLogs).Result;
                    if (response > 0)
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
            try
            {
                var repository = _serviceProvider.GetService<IMqMessageConsumerRepository>();
                var smsMessage = repository.GetSmsMessageByIdAsync(message.MessageContent).Result;
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
                    var response = repository.AddOneSmsMessageDetailsAsync(smsSendLogs).Result;
                    if (response > 0)
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