using HS.Message.Share.BaseModel;
using HS.Message.Share.MessageEmitter.Params;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HS.Message.Share.MessageEmitter
{
    /// <summary>
    /// 发送邮件工具
    /// </summary>
    public class SendMailEmitter
    {
        private readonly ILogger<SendMailEmitter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public SendMailEmitter(
            ILogger<SendMailEmitter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 使用SmtpClient发送邮件，单发
        /// （部分平台弃用）
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse> SendMailBySmtpClient(MailMParameter sendInfo)
        {
            var result = new BaseResponse()
            {
                Ret = ResponseCode.Success
            };

            try
            {

                //确定smtp服务器地址 实例化一个Smtp客户端
                using (System.Net.Mail.SmtpClient smtpclient = new()
                {
                    Host = sendInfo.SmtpService
                })
                {

                    //确定发件地址与收件地址
                    MailAddress sendAddress = new MailAddress(sendInfo.SendEmail);

                    //如果有多个收件人，那么分别发送
                    string[] receiveAddressLList = sendInfo.ReceiverEmails;
                    foreach (var item in receiveAddressLList)
                    {
                        try
                        {
                            MailAddress receiveAddress = new(item);
                            //构造一个Email的Message对象 内容信息

                            MailMessage mailMessage = new(sendAddress, receiveAddress);
                            mailMessage.Subject = sendInfo.MailTitle;
                            mailMessage.SubjectEncoding = Encoding.UTF8;
                            mailMessage.Body = sendInfo.MailBody;
                            mailMessage.BodyEncoding = Encoding.UTF8;

                            //抄送人
                            string[] receivesCC = sendInfo.ReceiverCcEmails;
                            foreach (var cc in receivesCC)
                            {
                                mailMessage.CC.Add(cc);
                            }


                            //邮件发送方式  通过网络发送到smtp服务器
                            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                            //如果服务器支持安全连接，则将安全连接设为true
                            smtpclient.EnableSsl = true;

                            //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
                            smtpclient.UseDefaultCredentials = false;

                            //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
                            NetworkCredential networkCredential = new NetworkCredential(sendInfo.SendEmail, sendInfo.SendPwd);
                            smtpclient.Credentials = networkCredential;

                            //发送邮件
                            await smtpclient.SendMailAsync(mailMessage);
                            result.Ret = ResponseCode.Success;
                            result.Msg = "发送成功！";
                        }
                        catch (Exception ex)
                        {
                            result.Ret = ResponseCode.ParameterError;
                            result.Msg = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
                        }

                    }
                }

                result.Ret = ResponseCode.Success;
                result.Msg = "发送成功！";
            }
            catch (Exception ex)
            {
                result.Ret = ResponseCode.ParameterError;
                result.Msg = ex.Message;

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
        public async Task<BaseResponse> SendMailBySmtpClientMassed(MailMParameter sendInfo)
        {
            var result = new BaseResponse()
            {
                Ret = ResponseCode.Success
            };

            try
            {

                //确定smtp服务器地址 实例化一个Smtp客户端
                using (System.Net.Mail.SmtpClient smtpclient = new()
                {
                    Host = sendInfo.SmtpService
                })
                {
                    try
                    {
                        MailMessage mailMessage = new()
                        {
                            From = new MailAddress(sendInfo.SendEmail),
                            Subject = sendInfo.MailTitle,
                            SubjectEncoding = Encoding.UTF8,
                            Body = sendInfo.MailBody,
                            BodyEncoding = Encoding.UTF8
                        };

                        //收件人
                        string[] receives = sendInfo.ReceiverEmails;
                        foreach (var item in receives)
                        {
                            mailMessage.To.Add(item);
                        }
                        //抄送人
                        string[] receivesCC = sendInfo.ReceiverCcEmails;
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
                        NetworkCredential networkCredential = new NetworkCredential(sendInfo.SendEmail, sendInfo.SendPwd);
                        smtpclient.Credentials = networkCredential;

                        //发送邮件
                        await smtpclient.SendMailAsync(mailMessage);
                        result.Ret = ResponseCode.Success;
                        result.Msg = "发送成功！";
                    }
                    catch (Exception ex)
                    {
                        result.Ret = ResponseCode.ParameterError;
                        result.Msg = ex.Message;
                        _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
                    }

                }

                result.Ret = ResponseCode.Success;
                result.Msg = "发送成功！";
            }
            catch (Exception ex)
            {
                result.Ret = ResponseCode.ParameterError;
                result.Msg = ex.Message;

                _logger.LogError($"邮件发送异常:{ex.Message},SendInfo:{sendInfo}");
            }

            return result;
        }

        /// <summary>
        /// 使用MailKit发送邮件
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse> SendMailByMailKit(MailMParameter sendInfo)
        {
            // 处理结果
            BaseResponse result = new() { Ret = ResponseCode.Success };


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sendInfo.SendEmail, sendInfo.SendEmail));

            message.Subject = sendInfo.MailTitle;  //邮件标题
            var builder = new BodyBuilder
            {
                HtmlBody = sendInfo.MailBody//支持Html
            };

            //添加附件
            //builder.Attachments.Add($@"{Directory.GetCurrentDirectory()}\1.png");//包含图片附件，或者正文中有图片会被当成垃圾邮件退回，所以不建议放图片内容（跟Mail类库框架无关）
            message.Body = builder.ToMessageBody();

            var cc = sendInfo.ReceiverCcEmails.Select(x => new MailboxAddress(x, x)).ToList();
            if (cc?.Count > 0)
            {
                message.Cc.AddRange(cc);
            }

            //如果有多个收件人，那么分别发送
            string[] receiveAddressLList = sendInfo.ReceiverEmails;
            foreach (var item in receiveAddressLList)
            {
                message.To.Add(new MailboxAddress(item, item));
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        client.Connect(sendInfo.SmtpService, 465, true);//网易、QQ支持 25(未加密)，465和587(SSL加密）

                        client.Authenticate(sendInfo.SendEmail, sendInfo.SendPwd);

                        try
                        {
                            await client.SendAsync(message);//发送邮件
                            client.Disconnect(true);

                            result.Ret = ResponseCode.Success;
                            result.Msg = "发送成功！";
                        }
                        catch (SmtpCommandException ex)
                        {
                            result.Ret = ResponseCode.ParameterError;
                            result.Msg = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                        }
                        catch (Exception ex)
                        {
                            result.Ret = ResponseCode.ParameterError;
                            result.Msg = ex.Message;
                            _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                        }

                    }

                    result.Ret = ResponseCode.Success;
                    result.Msg = "发送成功！";
                }
                catch (Exception ex)
                {
                    result.Ret = ResponseCode.ParameterError;
                    result.Msg = ex.Message;

                    _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                }

                message.To.Clear();
            }
            return result;
        }

        /// <summary>
        /// 使用MailKit发送邮件,群发
        /// </summary>
        /// <param name="sendInfo">发送的邮件内容</param>
        /// <Codeurns>发送结果</Codeurns>
        public async Task<BaseResponse<string>> SendMailByMailKitMassed(MailMParameter sendInfo)
        {
            // 处理结果
            BaseResponse<string> result = new() { Ret = ResponseCode.Success };

            if (sendInfo == null || sendInfo.ReceiverEmails == null || sendInfo.ReceiverEmails.Length == 0)
            {
                result.Ret = ResponseCode.ParameterError;
                result.Msg = "邮件信息为空或者邮件的接收人为空";
                return result;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sendInfo.SendEmail, sendInfo.SendEmail));

            message.Subject = sendInfo.MailTitle;  //邮件标题
            var builder = new BodyBuilder
            {
                HtmlBody = sendInfo.MailBody//支持Html
            };

            //添加附件
            //builder.Attachments.Add($@"{Directory.GetCurrentDirectory()}\1.png");//包含图片附件，或者正文中有图片会被当成垃圾邮件退回，所以不建议放图片内容（跟Mail类库框架无关）
            message.Body = builder.ToMessageBody();


            if (sendInfo.ReceiverEmails?.Length > 0)
            {
                var receives = sendInfo.ReceiverEmails.Select(x => new MailboxAddress(x, x)).ToList();
                message.To.AddRange(receives);
            }

            if (sendInfo.ReceiverCcEmails?.Length > 0)
            {
                var cc = sendInfo.ReceiverCcEmails.Select(x => new MailboxAddress(x, x)).ToList();
                message.Cc.AddRange(cc);
            }
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(sendInfo.SmtpService, 465, true);//网易、QQ支持 25(未加密)，465和587(SSL加密）

                    client.Authenticate(sendInfo.SendEmail, sendInfo.SendPwd);

                    try
                    {
                        var sendResponse = await client.SendAsync(message);//发送邮件
                        client.Disconnect(true);

                        result.Ret = ResponseCode.Success;
                        result.Msg = "发送成功！";
                        result.Data = sendResponse;
                    }
                    catch (SmtpCommandException ex)
                    {
                        result.Ret = ResponseCode.ParameterError;
                        result.Msg = ex.Message;
                        _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                    }
                    catch (Exception ex)
                    {
                        result.Ret = ResponseCode.ParameterError;
                        result.Msg = ex.Message;
                        _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
                    }

                }

                result.Ret = ResponseCode.Success;
                result.Msg = "发送成功！";
            }
            catch (Exception ex)
            {
                result.Ret = ResponseCode.ParameterError;
                result.Msg = ex.Message;

                _logger.LogError($"邮件发送异常:{ex.Message},SendData:{sendInfo}");
            }
            return result;
        }
    }
}
