using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Redis;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core.imp
{
    /// <summary>
    /// 邮件消息仓储
    /// </summary>
    public class MailMessageRepository : BizRepositoryAdapter<MMailMessage, MMailMessageCondtion>, IMailMessageRepository<MMailMessage, MMailMessageCondtion>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MailMessageRepository(IRepositoryInjectedObjects injectedObjects) : base(injectedObjects, "mail_message")
        {
        }


    }
}
