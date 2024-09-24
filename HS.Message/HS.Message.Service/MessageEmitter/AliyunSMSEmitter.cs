using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using HS.Message.Service.core;
using HS.Message.Share.BaseModel;
using HS.Message.Share.Utils;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HS.Message.Service.MessageEmitter
{
    /// <summary>
    /// 阿里云短信
    /// </summary>
    public class AliyunSMSEmitter
    {
        private readonly SMSParameter _sMSParameter;
        private readonly ISmsMessageService _smsMessageService;
        private readonly ISmsMessageDetailsService _smsMessageDetailsService;
        private readonly ILogger<AliyunSMSEmitter> _logger;

        public AliyunSMSEmitter(SMSParameter sMSParameter,
            ISmsMessageService smsMessageService,
            ISmsMessageDetailsService smsMessageDetailsService,
            ILogger<AliyunSMSEmitter> logger)
        {
            _sMSParameter = sMSParameter;
            _smsMessageService = smsMessageService;
            _smsMessageDetailsService = smsMessageDetailsService;
            _logger = logger;
        }
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="smsInfo"></param>
        public async Task<BaseResponse> MsmSend(MSmsMessage smsInfo)
        {
            BaseResponse result = new() { Code = ResponseCode.Success, Message = "操作成功！" };

            try
            {
                var client = CreateClient(_sMSParameter.AliyunSmsAccessKeyId, _sMSParameter.AliyunSmsAccessKeySecret);

                SendSmsRequest addSmsSignRequest = new()
                {
                    // 签名名称 如果没有传递就取默认的签名
                    SignName = string.IsNullOrEmpty(smsInfo.sign_name) ? _sMSParameter.AliyunSmsSignName : smsInfo.sign_name,
                    // 接收短信的手机号码
                    PhoneNumbers = smsInfo.phone_numbers,
                    // 模板id 如果没有传递就取默认的模板id
                    TemplateCode = string.IsNullOrEmpty(smsInfo.template_code) ? _sMSParameter.AliyunSmsTemplateCode : smsInfo.template_code,
                    // 模板参数，主要数把验证码以json方式传递
                    TemplateParam = smsInfo.template_param,
                    // 外部流水号 OutId
                    OutId = smsInfo.logical_id
                };

                // 复制代码运行请自行打印 API 的返回值
                SendSmsResponse response = await client.SendSmsAsync(addSmsSignRequest);

                // 发送提交成功
                if (response.Body.Code.ToUpper() == "OK")
                {
                    smsInfo.send_state = 2;
                }
                else
                {
                    smsInfo.send_state = 3;
                }

                smsInfo.submit_time = PublicTools.GetSysDateTimeNow();
                smsInfo.biz_id = response.Body.BizId;
                smsInfo.request_id = response.Body.RequestId;
                smsInfo.send_remark = response.Body.Message;

                // 更新发送结果
                result = await _smsMessageService.UpdateByIdAsync(smsInfo);
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.ParameterError;
                result.Message = ex.Message;

                _logger.LogError($"阿里云短信发送异常:{ex.Message},SendInfo:{smsInfo}");
            }


            return result;
        }

        /// <summary>
        /// 查询短信发送结果
        /// </summary>
        /// <param name="smsInfo"></param>
        /// <returns></returns>
        public async Task<BaseResponse> QuerySendDetails(MSmsMessage smsInfo)
        {
            BaseResponse result = new() { Code = ResponseCode.Success, Message = "操作成功！" };

            var client = CreateClient(_sMSParameter.AliyunSmsAccessKeyId, _sMSParameter.AliyunSmsAccessKeySecret);

            QuerySendDetailsRequest request = new QuerySendDetailsRequest()
            {
                BizId = smsInfo.biz_id,
                CurrentPage = 1,
                PageSize = 100,
                PhoneNumber = smsInfo.phone_numbers,
                SendDate = smsInfo.submit_time.ToString("yyyyMMdd")
            };
            QuerySendDetailsResponse response = client.QuerySendDetails(request);

            List<MSmsMessageDetails> smsMessageDetailsList = new List<MSmsMessageDetails>();

            // 发送提交成功
            if (response.Body.Code.ToUpper() == "OK")
            {
                if (response.Body.SmsSendDetailDTOs != null &&
                    response.Body.SmsSendDetailDTOs.SmsSendDetailDTO != null)
                {
                    foreach (var item in response.Body.SmsSendDetailDTOs.SmsSendDetailDTO)
                    {
                        if (item.SendStatus == 1)
                        {
                            continue;
                        }
                        MSmsMessageDetails smsMessageDetailsModel = new()
                        {
                            logical_id = Guid.NewGuid().ToString().Replace("-", ""),
                            sms_message_id = smsInfo.logical_id,
                            channel_code = 1,
                            channel_name = "阿里云",
                            content = item.Content,
                            phone_number = item.PhoneNum,
                            request_id = response.Body.RequestId,
                            biz_id = smsInfo.biz_id,
                            send_state = 3,
                            //main_id = item.OutId,
                            last_send_time = Convert.ToDateTime(item.SendDate),
                            receive_time = Convert.ToDateTime(item.ReceiveDate),
                            updated_time = PublicTools.GetSysDateTimeNow()
                        };

                        smsMessageDetailsModel.send_state = item.SendStatus == 2 ? 4 : 3;

                        smsMessageDetailsList.Add(smsMessageDetailsModel);
                    }
                }
            }

            // 更新发送结果
            await _smsMessageDetailsService.BactchAddAsync(smsMessageDetailsList);

            return result;
        }

        private static Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            Config config = new Config
            {
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new Client(config);
        }
    }
}
