using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.@base.core
{
    public interface IRepository<TModel, TCondition> : IDependency where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        int AddOne(TModel model);

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        Task<int> AddOneAsync(TModel model);

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        int BactchAdd(List<TModel> modelList);

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        Task<int> BactchAddAsync(List<TModel> modelList);

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        int DeleteById(string logical_id);

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteByIdAsync(string logical_id);

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        int BactchDeleteByIdList(List<string> idList);

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        Task<int> BactchDeleteByIdListAsync(List<string> idList);

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        int LogicDeleteById(string logical_id);

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        Task<int> LogicDeleteByIdAsync(string logical_id);

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        int BactchLogicDeleteByIdList(List<string> idList);

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        Task<int> BactchLogicDeleteByIdListAsync(List<string> idList);

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        int UpdateById(TModel model);

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        Task<int> UpdateByIdAsync(TModel model);

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        int BactchUpdateById(List<TModel> modelList);

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        Task<int >BactchUpdateByIdAsync(List<TModel> modelList);

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        int BactchUpdateSpecifyFieldsById(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields);

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        Task<int >BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields);

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        long GetTotalCount(TCondition condition);

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<long >GetTotalCountAsync(TCondition condition);

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        TModel GetModelById(string logical_id, string queryFields = "");

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        Task<TModel >GetModelByIdAsync(string logical_id, string queryFields = "");

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        TModel GetOneModel(TCondition condition);

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        Task<TModel >GetOneModelAsync(TCondition condition);

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        List<TModel> GetAllListByIdList(List<string> idList, string queryFields = "");

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        Task<List<TModel>> GetAllListByIdListAsync(List<string> idList, string queryFields = "");

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        List<TModel> GetPageList(MPageQueryCondition<TCondition> pageQueryCondition);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        Task<List<TModel> >GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition);

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        List<TModel> GetAllList(TCondition model, int limitNum = 0);

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        Task<List<TModel> >GetAllListAsync(TCondition model, int limitNum = 0);

    }
}
