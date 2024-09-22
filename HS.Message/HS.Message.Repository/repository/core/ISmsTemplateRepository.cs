using HS.Message.Repository.repository.@base.core;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.core
{
    public interface ISmsTemplateRepository<TModel, TCondition> : IRepository<TModel, TCondition>, IDependency where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
    }
}
