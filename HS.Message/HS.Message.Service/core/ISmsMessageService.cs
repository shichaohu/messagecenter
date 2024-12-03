using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 短息服务
    /// </summary>
    public interface ISmsMessageService : IBaseService<MSmsMessage, MSmsMessageCondition>, IDependency
    {
    }
}
