using HS.Message.Repository.repository.core;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Utils;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Transactions;

namespace HS.Message.Service.MessageEmitter
{
    /// <summary>
    /// 发送邮件工具
    /// </summary>
    public class SendingMailEmitter
    {
        private readonly IMessageRepository<MMessage, MMessageCondtion> _messageRepository;
        private readonly IMailMessageService _mailMessageService;
        private readonly IMailSendLogsService _mailSendLogsService;
        private readonly ILogger<SendingMailEmitter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageRepository"></param>
        /// <param name="mailMessageService"></param>
        /// <param name="mailSendLogsService"></param>
        /// <param name="logger"></param>
        public SendingMailEmitter(

            IMessageRepository<MMessage, MMessageCondtion> messageRepository,
            IMailMessageService mailMessageService,
            IMailSendLogsService mailSendLogsService,
            ILogger<SendingMailEmitter> logger)
        {
            _messageRepository = messageRepository;
            _mailMessageService = mailMessageService;
            _mailSendLogsService = mailSendLogsService;
            _logger = logger;
        }
        /// <summary>
        /// 使用SmtpClient发送邮件，单发
        /// （部分平台弃用）
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse> SendMailBySmtpClient(MMailMessage sendInfo)
        {
            var result = new BaseResponse()
            {
                Code = ResponseCode.Success
            };

            try
            {

                //确定smtp服务器地址 实例化一个Smtp客户端
                using (System.Net.Mail.SmtpClient smtpclient = new()
                {
                    Host = sendInfo.smtp_service
                })
                {

                    //确定发件地址与收件地址
                    MailAddress sendAddress = new MailAddress(sendInfo.send_email);

                    //如果有多个收件人，那么分别发送
                    string[] receiveAddressLList = sendInfo.receiver_email.Split(',');
                    foreach (var item in receiveAddressLList)
                    {
                        try
                        {
                            MailAddress receiveAddress = new(item);
                            //构造一个Email的Message对象 内容信息

                            MailMessage mailMessage = new(sendAddress, receiveAddress);
                            mailMessage.Subject = sendInfo.mail_title;
                            mailMessage.SubjectEncoding = Encoding.UTF8;
                            mailMessage.Body = sendInfo.mail_body;
                            mailMessage.BodyEncoding = Encoding.UTF8;

                            //邮件发送方式  通过网络发送到smtp服务器
                            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                            //如果服务器支持安全连接，则将安全连接设为true
                            smtpclient.EnableSsl = true;

                            //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
                            smtpclient.UseDefaultCredentials = false;

                            //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
                            NetworkCredential networkCredential = new NetworkCredential(sendInfo.send_email, sendInfo.send_pwd);
                            smtpclient.Credentials = networkCredential;

                            //发送邮件
                            await smtpclient.SendMailAsync(mailMessage);
                            result.Code = ResponseCode.Success;
                            result.Message = "发送成功！";
                        }
                        catch (Exception ex)
                        {
                            result.Code = ResponseCode.ParameterError;
                            result.Message = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
                        }

                        // 处理发送结果
                        DoSendMailResult(sendInfo, item, "", result);
                    }
                }

                result.Code = ResponseCode.Success;
                result.Message = "发送成功！";
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.ParameterError;
                result.Message = ex.Message;

                _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
            }

            return result;
        }

        /// <summary>
        /// 使用SmtpClient发送邮件，群发
        /// （部分平台弃用）
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse> SendMailBySmtpClientMassed(MMailMessage sendInfo)
        {
            var result = new BaseResponse()
            {
                Code = ResponseCode.Success
            };

            try
            {

                //确定smtp服务器地址 实例化一个Smtp客户端
                using (System.Net.Mail.SmtpClient smtpclient = new()
                {
                    Host = sendInfo.smtp_service
                })
                {
                    try
                    {
                        MailMessage mailMessage = new();
                        mailMessage.From = new MailAddress(sendInfo.send_email);
                        mailMessage.Subject = sendInfo.mail_title;
                        mailMessage.SubjectEncoding = Encoding.UTF8;
                        mailMessage.Body = sendInfo.mail_body;
                        mailMessage.BodyEncoding = Encoding.UTF8;

                        //收件人
                        string[] receives = sendInfo.receiver_email.Split(',');
                        foreach (var item in receives)
                        {
                            mailMessage.To.Add(item);
                        }
                        //抄送人
                        string[] receivesCC = sendInfo.receiver_cc_email.Split(',');
                        foreach (var item in receivesCC)
                        {
                            mailMessage.CC.Add(item);
                        }

                        //邮件发送方式  通过网络发送到smtp服务器
                        smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                        //如果服务器支持安全连接，则将安全连接设为true
                        smtpclient.EnableSsl = true;

                        //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
                        smtpclient.UseDefaultCredentials = false;

                        //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
                        NetworkCredential networkCredential = new NetworkCredential(sendInfo.send_email, sendInfo.send_pwd);
                        smtpclient.Credentials = networkCredential;

                        //发送邮件
                        await smtpclient.SendMailAsync(mailMessage);
                        result.Code = ResponseCode.Success;
                        result.Message = "发送成功！";
                    }
                    catch (Exception ex)
                    {
                        result.Code = ResponseCode.ParameterError;
                        result.Message = ex.Message;
                        _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
                    }

                    // 处理发送结果
                    DoSendMailResult(sendInfo, sendInfo.receiver_email, sendInfo.receiver_cc_email, result);

                }

                result.Code = ResponseCode.Success;
                result.Message = "发送成功！";
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.ParameterError;
                result.Message = ex.Message;

                _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
            }

            return result;
        }

        /// <summary>
        /// 使用MailKit发送邮件
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse> SendMailByMailKit(MMailMessage sendInfo)
        {
            // 处理结果
            BaseResponse result = new() { Code = ResponseCode.Success };


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sendInfo.send_email, sendInfo.send_email));

            message.Subject = sendInfo.mail_title;  //邮件标题
            var builder = new BodyBuilder
            {
                HtmlBody = sendInfo.mail_body//支持Html
            };

            //添加附件
            //builder.Attachments.Add($@"{Directory.GetCurrentDirectory()}\1.png");//包含图片附件，或者正文中有图片会被当成垃圾邮件退回，所以不建议放图片内容（跟Mail类库框架无关）
            message.Body = builder.ToMessageBody();

            //如果有多个收件人，那么分别发送
            string[] receiveAddressLList = sendInfo.receiver_email.Split(',');
            foreach (var item in receiveAddressLList)
            {
                message.To.Add(new MailboxAddress(item, item));
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(sendInfo.smtp_service, 465, true);//网易、QQ支持 25(未加密)，465和587(SSL加密）

                        client.Authenticate(sendInfo.send_email, sendInfo.send_pwd);

                        try
                        {
                            await client.SendAsync(message);//发送邮件
                            client.Disconnect(true);

                            result.Code = ResponseCode.Success;
                            result.Message = "发送成功！";
                        }
                        catch (SmtpCommandException ex)
                        {
                            result.Code = ResponseCode.ParameterError;
                            result.Message = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                        }
                        catch (Exception ex)
                        {
                            result.Code = ResponseCode.ParameterError;
                            result.Message = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                        }

                        // 处理发送结果
                        DoSendMailResult(sendInfo, item, "", result);
                    }

                    result.Code = ResponseCode.Success;
                    result.Message = "发送成功！";
                }
                catch (Exception ex)
                {
                    result.Code = ResponseCode.ParameterError;
                    result.Message = ex.Message;

                    _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                }
            }
            return result;
        }

        /// <summary>
        /// 邮件发送成功后，需要更新对应的消息表数据+落地一条发送日志信息
        /// </summary>
        /// <param name="sendInfo">发送邮件主体信息</param>
        /// <param name="receiveAddress">消息接受者地址</param>
        /// <param name="receiveCCAddress">抄送地址</param>
        /// <param name="result">发送结果</param>
        private async void DoSendMailResult(MMailMessage sendInfo, string receiveAddress, string receiveCCAddress, BaseResponse result)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // 更新发送结果信息
                sendInfo.last_send_time = PublicTools.GetSysDateTimeNow();
                sendInfo.has_send_num = sendInfo.has_send_num + 1;
                sendInfo.updated_time = sendInfo.last_send_time;
                sendInfo.send_state = sendInfo.has_send_num >= sendInfo.total_send_num ? 3 : 2;

                await _mailMessageService.UpdateByIdAsync(sendInfo);

                // 落地一条发送结果日志记录
                MMailSendLogs mailSendLogs = new()
                {
                    logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                    mail_message_id = sendInfo.logical_id,
                    mail_title = sendInfo.mail_title,
                    mail_body = sendInfo.mail_body,
                    mail_configuer_id = sendInfo.mail_configuer_id,
                    receiver_email = receiveAddress,
                    receiver_cc_email = receiveCCAddress,
                    send_result = result.Message,
                    send_state = result.Code == ResponseCode.Success ? 1 : 2,
                    send_time = PublicTools.GetSysDateTimeNow()
                };

                await _mailSendLogsService.AddOneAsync(mailSendLogs);

                scope.Complete();
            }
        }
    }
}
