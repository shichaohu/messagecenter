using HS.Message.Share.BaseModel;
using System.Threading.Tasks;

namespace HS.Message.Service.@base
{
    /// <summary>
    /// 业务服务base接口
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    public interface IBaseService<TModel, TCondition> where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<long> GetTotalCountAsync(TCondition condition);

        /// <summary>
        /// 新增一条Test记录
        /// </summary>
        /// <param name="model">Test数据模型</param>
        /// <returns>操作结果</returns>
        Task<BaseResponse> AddOneAsync(TModel modelBase);

        /// <summary>
        /// 批量新增Test数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>操作结果</returns>
        Task<BaseResponse> BactchAddAsync(List<TModel> modelList);

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>影响的行数</returns>
        Task<BaseResponse> DeleteByIdAsync(string id);

        /// <summary>
        /// 根据Id集合批量删除数据
        /// </summary>
        /// <param name="idList">Id集合</param>
        /// <returns>处理结果</returns>
        Task<BaseResponse> BactchDeleteByIdListAsync(List<string> idList);

        /// <summary>
        /// 根据id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        Task<BaseResponse> UpdateByIdAsync(TModel model);

        /// <summary>
        /// 根据id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        Task<BaseResponse> BactchUpdateByIdAsync(List<TModel> modelList);

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息
        /// </summary>
        /// <param name="modelJobj">需要更新的 test 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        Task<BaseResponse> UpdateDynamicAsync(Dictionary<string, object> modelJobj);

        /// <summary>
        /// 动态更新，根据实际需要，传递什么字段就更新什么字段信息（批量更新）
        /// </summary>
        /// <param name="modelJobjList">需要更新的 auth 数据Jobject实例</param>
        /// <returns>处理结果</returns>
        Task<BaseResponse> BactchUpdateDynamicAsync(List<Dictionary<string, object>> modelJobjList);

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="bactchUpdateSpecifyFields">id集合</param>
        /// <returns>所有数据集合</returns>
        Task<BaseResponse> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields);

        /// <summary>
        /// 根据id获取一个模型数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        Task<BaseResponse<TModel>> GetModelByIdAsync(string id, string queryFields = "");

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        Task<BaseResponse<TModel>> GetOneModelAsync(TCondition condition);

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        Task<BaseResponse<List<TModel>>> GetAllListByIdListAsync(List<string> idList, string queryFields = "");

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition"></param>
        /// <returns>符合要求的分页数据集合</returns>
        Task<MPageQueryResponse<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition);

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合要求的数据集合</returns>
        Task<MPageQueryResponse<TModel>> GetAllListAsync(TCondition condition);

    }
}
