using HS.Message.Model;
using HS.Message.Model.Requests;
using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 消息消费仓储
    /// </summary>
    public class MqMessageConsumerRepository : IMqMessageConsumerRepository
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MqMessageConsumerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<MMailMessage> GetMailMessageByIdAsync(string id)
        {
            var repository = new BizTransientRepositoryAdapter<MMailMessage, MMailMessageCondtion>(_configuration, "mail_message");
            return await repository.GetModelByIdAsync(id);
        }
        public async Task<int> AddOneMailSendLogsAsync(MMailSendLogs data)
        {
            var repository = new BizTransientRepositoryAdapter<MMailSendLogs, MMailSendLogsCondtion>(_configuration, "mail_send_logs");
            return await repository.AddOneAsync(data);
        }

        public async Task<MSmsMessage> GetSmsMessageByIdAsync(string id)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessage, MSmsMessageCondtion>(_configuration, "sms_message");
            return await repository.GetModelByIdAsync(id);
        }
        public async Task<int> AddOneSmsMessageDetailsAsync(MSmsMessageDetails data)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessageDetails, MSmsMessageDetailsCondtion>(_configuration, "sms_message_details");
            return await repository.AddOneAsync(data);
        }

    }
}
