using HS.Message.Service.@base;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Service.core
{
    /// <summary>
    /// 短息模板服务
    /// </summary>
    public interface ISmsTemplateService : IBaseService<MSmsTemplate, MSmsTemplateCondition>, IDependency
    {
    }
}
