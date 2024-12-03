using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core
{
    public interface IMqMessageConsumerRepository : ITransientDependency
    {
        Task<int> AddOneMailMessageAsync(MMailMessage data);
        Task<MMailMessage> GetMailMessageByIdAsync(string id);
        Task<int> AddOneMailSendLogsAsync(MMailSendLogs data);
        Task<int> UpdateMailMessageByIdAsync(MMailMessage message);

        Task<int> AddOneSmsMessageAsync(MSmsMessage data);
        Task<MSmsMessage> GetSmsMessageByIdAsync(string id);
        Task<int> AddOneSmsMessageDetailsAsync(MSmsMessageDetails data);
        Task<int> UpdateSmsMessageByIdAsync(MSmsMessage message);
    }
}
