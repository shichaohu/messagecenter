using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 邮件发送日志服务
    /// </summary>
    public interface IMailSendLogsService : IBaseService<MMailSendLogs, MMailSendLogsCondition>, IDependency
    {
    }
}
