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

        public async Task<int> AddOneMailMessageAsync(MMailMessage data)
        {
            var repository = new BizTransientRepositoryAdapter<MMailMessage, MMailMessageCondition>(_configuration, "mail_message");
            return await repository.AddOneAsync(data);
        }
        public async Task<MMailMessage> GetMailMessageByIdAsync(string id)
        {
            var repository = new BizTransientRepositoryAdapter<MMailMessage, MMailMessageCondition>(_configuration, "mail_message");
            return await repository.GetModelByIdAsync(id);
        }
        public async Task<int> AddOneMailSendLogsAsync(MMailSendLogs data)
        {
            var repository = new BizTransientRepositoryAdapter<MMailSendLogs, MMailSendLogsCondition>(_configuration, "mail_send_logs");
            return await repository.AddOneAsync(data);
        }
        public async Task<int> UpdateMailMessageByIdAsync(MMailMessage message)
        {
            var repository = new BizTransientRepositoryAdapter<MMailMessage, MMailMessageCondition>(_configuration, "mail_message");
            return await repository.UpdateByIdAsync(message);
        }


        public async Task<int> AddOneSmsMessageAsync(MSmsMessage data)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessage, MSmsMessageCondition>(_configuration, "sms_message");
            return await repository.AddOneAsync(data);
        }
        public async Task<MSmsMessage> GetSmsMessageByIdAsync(string id)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessage, MSmsMessageCondition>(_configuration, "sms_message");
            return await repository.GetModelByIdAsync(id);
        }
        public async Task<int> AddOneSmsMessageDetailsAsync(MSmsMessageDetails data)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessageDetails, MSmsMessageDetailsCondition>(_configuration, "sms_message_details");
            return await repository.AddOneAsync(data);
        }
        public async Task<int> UpdateSmsMessageByIdAsync(MSmsMessage message)
        {
            var repository = new BizTransientRepositoryAdapter<MSmsMessage, MSmsMessageCondition>(_configuration, "sms_message");
            return await repository.UpdateByIdAsync(message);
        }


        public async Task<int> UpdateMessageByIdAsync(MMessage message)
        {

            var repository = new BizTransientRepositoryAdapter<MMessage, MMessageCondition>(_configuration, "message");

            string sql = $@"
UPDATE message m
INNER JOIN(
	SELECT IF(totalstate<100,2,IF(totalstate=500,3,4)) finalstate,message_id
	FROM(
		SELECT message_id
		,sms_send_state,wechat_send_state,dingtalk_send_state,other_channel_send_state
		,if(IFNULL(email_send_state,2)=2,100,email_send_state)+if(IFNULL(sms_send_state,2)=2,100,sms_send_state)+if(IFNULL(wechat_send_state,2)=2,100,wechat_send_state)+if(IFNULL(dingtalk_send_state,2)=2,100,dingtalk_send_state)+if(IFNULL(other_channel_send_state,2)=2,100,other_channel_send_state) totalstate
		from message_receiver WHERE message_id=@message_id
	) t
)b on b.message_id=m.logical_id
SET m.send_state=b.finalstate";
            return await repository._sqlRepository.DapperTool.ExecuteNonQueryAsync(sql, new { message_id = message.LogicalId });
        }

        public async Task<int> UpdateMessageReceiverByIdAsync(MMessageReceiver message)
        {
            var repository = new BizTransientRepositoryAdapter<MMessageReceiver, MMessageReceiverCondition>(_configuration, "message_receiver");

            string sql = $@"UPDATE message_receiver SET 
email_send_state=@email_send_state,email_last_sendtime=@email_last_sendtime,
sms_send_state=@sms_send_state,sms_last_sendtime=@sms_last_sendtime,
wechat_send_state=@wechat_send_state,wechat_last_sendtime=@wechat_last_sendtime,
dingtalk_send_state=@dingtalk_send_state,dingtalk_last_sendtime=@dingtalk_last_sendtime,
other_channel_send_state=@other_channel_send_state,other_channel_last_sendtime=@other_channel_last_sendtime
WHERE logical_id=@logicalId";
            var param = new
            {
                logicalId = message.LogicalId,
                email_send_state = message.EmailSendState,
                email_last_sendtime = message.EmailLastSendtime,
                sms_send_state = message.SmsSendState,
                sms_last_sendtime = message.SmsLastSendtime,
                wechat_send_state = message.WechatSendState,
                wechat_last_sendtime = message.WechatLastSendtime,
                dingtalk_send_state = message.DingtalkSendState,
                dingtalk_last_sendtime = message.SmsLastSendtime,
                other_channel_send_state = message.OtherChannelSendState,
                other_channel_last_sendtime = message.OtherChannelLastSendtime
            };
            return await repository._sqlRepository.DapperTool.ExecuteNonQueryAsync(sql, param);
        }
    }
}
