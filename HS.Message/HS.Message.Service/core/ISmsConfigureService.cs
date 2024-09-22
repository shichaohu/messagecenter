using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 短息配置服务
    /// </summary>
    public interface ISmsConfigureService : IBaseService<MSmsConfigure, MSmsConfigureCondtion>, IDependency
    {
    }
}
