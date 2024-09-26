using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using HS.Message.Share.BaseModel;
using HS.Message.Share.MessageEmitter.Params;
using Microsoft.Extensions.Logging;

namespace HS.Message.Share.MessageEmitter
{
    /// <summary>
    /// 阿里云短信
    /// </summary>
    public class AliyunSMSEmitter
    {
        private readonly SMSParameter _smsParameter;
        private readonly ILogger<AliyunSMSEmitter> _logger;

        public AliyunSMSEmitter(SMSParameter smsParameter,
            ILogger<AliyunSMSEmitter> logger)
        {
            _smsParameter = smsParameter;
            _logger = logger;
        }
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="request"></param>
        /// <param name="request.SignName">签名名称</param>
        /// <param name="request.PhoneNumbers">接收短信的手机号码</param>
        /// <param name="request.TemplateCode">模板id</param>
        /// <param name="request.TemplateParam">模板参数</param>
        /// <param name="request.OutId">外部流水号</param>
        public async Task<BaseResponse<SendSmsResponse>> MsmSend(SendSmsRequest request)
        {
            BaseResponse<SendSmsResponse> result = new() { Code = ResponseCode.InternalError, Message = "操作失败！" };

            try
            {
                var client = CreateClient(_smsParameter.AliyunSmsAccessKeyId, _smsParameter.AliyunSmsAccessKeySecret);

                SendSmsRequest addSmsSignRequest = new()
                {
                    // 签名名称 如果没有传递就取默认的签名
                    SignName = string.IsNullOrEmpty(request.SignName) ? _smsParameter.AliyunSmsSignName : request.SignName,
                    // 接收短信的手机号码
                    PhoneNumbers = request.PhoneNumbers,
                    // 模板id 如果没有传递就取默认的模板id
                    TemplateCode = string.IsNullOrEmpty(request.TemplateCode) ? _smsParameter.AliyunSmsTemplateCode : request.TemplateCode,
                    // 模板参数，主要数把验证码以json方式传递
                    TemplateParam = request.TemplateParam,
                    // 外部流水号 OutId
                    OutId = request.OutId
                };

                // 复制代码运行请自行打印 API 的返回值
                SendSmsResponse response = await client.SendSmsAsync(addSmsSignRequest);

                // 发送提交成功
                if (response.Body.Code.ToUpper() == "OK")
                {
                    result.Code = ResponseCode.Success;
                    result.Message = "发送成功！";
                }

                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.InternalError;
                result.Message = ex.Message;

                _logger.LogError($"阿里云短信发送异常:{ex.Message},SendInfo:{request}");
            }


            return result;
        }

        /// <summary>
        /// 查询短信发送结果
        /// </summary>
        /// <param name="smsInfo"></param>
        /// <returns></returns>
        public async Task<BaseResponse<QuerySendDetailsResponse>> QuerySendDetails(QuerySendDetailsRequest request)
        {
            BaseResponse<QuerySendDetailsResponse> result = new() { Code = ResponseCode.InternalError, Message = "操作失败！" };

            var client = CreateClient(_smsParameter.AliyunSmsAccessKeyId, _smsParameter.AliyunSmsAccessKeySecret);

            //QuerySendDetailsRequest request = new QuerySendDetailsRequest()
            //{
            //    BizId = smsInfo.biz_id,
            //    CurrentPage = 1,
            //    PageSize = 100,
            //    PhoneNumber = smsInfo.phone_numbers,
            //    SendDate = smsInfo.submit_time.ToString("yyyyMMdd")
            //};
            QuerySendDetailsResponse response = client.QuerySendDetails(request);

            // 发送提交成功
            if (response?.Body?.Code?.ToUpper() == "OK")
            {
                result.Code = ResponseCode.Success;
                result.Message = "查询成功！";
            }
            else
            {
                result.Code = ResponseCode.InternalError;
                result.Message = response?.Body?.Message;
            }

            result.Data = response;

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
