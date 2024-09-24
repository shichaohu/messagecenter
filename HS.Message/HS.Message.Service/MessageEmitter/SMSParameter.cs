using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HS.Message.Service.MessageEmitter
{
    /// <summary>
    /// SMS公共参数
    /// </summary>
    public class SMSParameter
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public SMSParameter(IConfiguration configuration)

        {
            _configuration = configuration;
            AliyunSmsAccessKeyId = _configuration["SMS:Aliyun:AccessKeyId"];
            AliyunSmsAccessKeySecret = _configuration["SMS:Aliyun:AccessKeySecret"];
            AliyunSmsSignName = _configuration["SMS:Aliyun:SignName"];
            AliyunSmsTemplateCode = _configuration["SMS:Aliyun:TemplateCode"];
        }

        /// <summary>
        /// 阿里云短信配置 AccessKey
        /// </summary>
        public string AliyunSmsAccessKeyId { get; }

        /// <summary>
        /// 阿里云短信配置 AccessKey Secret	
        /// </summary>
        public string AliyunSmsAccessKeySecret { get; }

        /// <summary>
        /// 阿里云短信签名名称
        /// </summary>
        public string AliyunSmsSignName { get; }

        /// <summary>
        /// 阿里云短信模板id
        /// </summary>
        public string AliyunSmsTemplateCode { get; }
    }
}
