using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 短息详情
    /// </summary>
    public interface ISmsMessageDetailsService : IBaseService<MSmsMessageDetails, MSmsMessageDetailsCondtion>, IDependency
    {
    }
}
