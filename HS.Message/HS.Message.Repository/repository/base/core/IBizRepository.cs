using HS.Message.Repository.database.tool;
using HS.Message.Share.BaseModel;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.@base.core
{
    public interface IBizRepository<TModel, TCondition>
    {
        public string TableName { get; }
        public BaseDapperTool<TModel> DapperTool { get; }
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
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        int DeleteById(string logicalId);

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        Task<int> DeleteByIdAsync(string logicalId);

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        int BactchDeleteByIdList(List<string> idList);

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        Task<int> BactchDeleteByIdListAsync(List<string> idList);

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        int LogicDeleteById(string logicalId);

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        Task<int> LogicDeleteByIdAsync(string logicalId);

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        int BactchLogicDeleteByIdList(List<string> idList);

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        Task<int> BactchLogicDeleteByIdListAsync(List<string> idList);

        /// <summary>
        /// 根据logicalId更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        int UpdateById(TModel model);

        /// <summary>
        /// 根据logicalId更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        Task<int> UpdateByIdAsync(TModel model);

        /// <summary>
        /// 根据logicalId更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        int BactchUpdateById(List<TModel> modelList);

        /// <summary>
        /// 根据logicalId更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        Task<int> BactchUpdateByIdAsync(List<TModel> modelList);

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        int BactchUpdateSpecifyFieldsById(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields);

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        Task<int> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields);

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
        Task<long> GetTotalCountAsync(TCondition condition);

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        TModel GetModelById(string logicalId, string queryFields = "");

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        Task<TModel> GetModelByIdAsync(string logicalId, string queryFields = "");

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        T GetModelById<T>(string logicalId, string queryFields = "");

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        Task<T> GetModelByIdAsync<T>(string logicalId, string queryFields = "");

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
        Task<TModel> GetOneModelAsync(TCondition condition);

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        List<TModel> GetAllListByIdList(List<string> idList, string queryFields = "");

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        Task<List<TModel>> GetAllListByIdListAsync(List<string> idList, string queryFields = "");

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        List<T> GetAllListByIdList<T>(List<string> idList, string queryFields = "");

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        Task<List<T>> GetAllListByIdListAsync<T>(List<string> idList, string queryFields = "");

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
        Task<List<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition);

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
        Task<List<TModel>> GetAllListAsync(TCondition model, int limitNum = 0);

        /// <summary>
        /// 根据model构建查询条件
        /// </summary>
        /// <param name="model">实体model</param>
        /// <returns>条件集合</returns>
        string GetSqlWhereByModel(TCondition model);

        /// <summary>
        /// 根据model构建查询条件
        /// </summary>
        /// <param name="model">实体model</param>
        /// <returns>条件集合</returns>
        Func<TCondition, string, StringBuilder> GetSqlWhereByModelFunc { get; set; }

        /// <summary>
        /// 获取表全部字段
        /// </summary>
        /// <returns></returns>
        string GetAllSelectField(string tableAlias = "");

        /// <summary>
        /// 获取表全部字段(不包含id)
        /// </summary>
        /// <returns></returns>
        Func<string,bool, string> GetAllFieldFunc { get; set; }

        /// <summary>
        /// 获取表\的插入语句 
        /// </summary>
        /// <returns>插入语句</returns>
        string GetInsertSql();

        /// <summary>
        /// 获取表的插入语句 
        /// </summary>
        /// <returns>插入语句</returns>
        Func<string> GetInsertSqlFunc { get; set; }

        /// <summary>
        /// 获取表的更新语句 
        /// </summary>
        /// <returns>插入语句</returns>
        string GetUpdateSql();

        /// <summary>
        /// 获取更新sql
        /// </summary>
        /// <returns></returns>
        Func<string> GetUpdateSqlFunc { get; set; }

        StringBuilder GetfuzzySearchWhere(MBaseModel model, string table = "");


        /// <summary>
        /// 格式化数据库名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string FormattingTableName(string tableName);

        /// <summary>
        /// 拼接in sql语句
        /// </summary>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        string FormattingInSql<T2>(string field, List<T2> fieldValue);
    }
}
