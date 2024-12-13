using HS.Message.Repository.database.tool;
using HS.Message.Share.Attributes;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Utils;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.@base.core
{
    public class BizTransientRepositoryAdapter<TModel, TCondition> where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        protected string TableName { get; }
        public IBizRepository<TModel, TCondition> _sqlRepository;
        protected BaseDapperTool<TModel> _dapperTool;
        protected DBType DbType { get; }
        protected string[] _globalIgnoreField;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizTransientRepositoryAdapter(IConfiguration Configuration, string tableName)
        {
            _configuration = Configuration;
            TableName = tableName;
            if (!Enum.TryParse(_configuration["ConnectionStrings:DatabaseType"], true, out DBType dbType))
            {
                dbType = DBType.MySql;
            }
            DbType = dbType;
            switch (dbType)
            {
                case DBType.MySql:
                    _sqlRepository = new MySqlBizRepository<TModel, TCondition>(_configuration, tableName);
                    break;
                case DBType.PGSql:
                    _sqlRepository = new PGSqlBizRepository<TModel, TCondition>(_configuration, tableName);
                    break;
                case DBType.Oracle:
                default:
                    _sqlRepository = new MySqlBizRepository<TModel, TCondition>(_configuration, tableName);
                    break;
            }
            _dapperTool = _sqlRepository.DapperTool;
            _sqlRepository.GetSqlWhereByModelFunc = GetSqlWhereByModel;
            _sqlRepository.GetAllFieldFunc = GetAllFieldForSql;
            _sqlRepository.GetInsertSqlFunc = GetInsertSql;
            _sqlRepository.GetUpdateSqlFunc = GetUpdateSql;


            _globalIgnoreField = new string[] { "QueryFields", "Orderby", "IsNotLike", "FuzzySearchKeyWord", "FuzzySearchFields", "IsLanguageHandle", "LanguageHandleEcludeField", "LimitNum" };
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        public int AddOne(TModel model)
        {
            return _sqlRepository.AddOne(model);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        public async Task<int> AddOneAsync(TModel model)
        {
            return await _sqlRepository.AddOneAsync(model);
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        public int BactchAdd(List<TModel> modelList)
        {
            return _sqlRepository.BactchAdd(modelList);
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        public async Task<int> BactchAddAsync(List<TModel> modelList)
        {
            return await _sqlRepository.BactchAddAsync(modelList);
        }

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        public int DeleteById(string logicalId)
        {
            return _sqlRepository.DeleteById(logicalId);
        }

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        public async Task<int> DeleteByIdAsync(string logicalId)
        {
            return await _sqlRepository.DeleteByIdAsync(logicalId);
        }

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        public int BactchDeleteByIdList(List<string> idList)
        {
            return _sqlRepository.BactchDeleteByIdList(idList);
        }

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchDeleteByIdListAsync(List<string> idList)
        {
            return await _sqlRepository.BactchDeleteByIdListAsync(idList);
        }

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        public int LogicDeleteById(string logicalId)
        {
            return _sqlRepository.LogicDeleteById(logicalId);
        }

        /// <summary>
        /// 根据logicalId删除数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <returns>影响的行数</returns>
        public async Task<int> LogicDeleteByIdAsync(string logicalId)
        {
            return await _sqlRepository.LogicDeleteByIdAsync(logicalId);
        }

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        public int BactchLogicDeleteByIdList(List<string> idList)
        {
            return _sqlRepository.BactchLogicDeleteByIdList(idList);
        }

        /// <summary>
        /// 根据logicalId集合批量删除数据
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchLogicDeleteByIdListAsync(List<string> idList)
        {
            return await _sqlRepository.BactchLogicDeleteByIdListAsync(idList);
        }

        /// <summary>
        /// 根据logicalId更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public int UpdateById(TModel model)
        {
            return _sqlRepository.UpdateById(model);
        }

        /// <summary>
        /// 根据logicalId更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public async Task<int> UpdateByIdAsync(TModel model)
        {
            return await _sqlRepository.UpdateByIdAsync(model);
        }

        /// <summary>
        /// 根据logicalId更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public int BactchUpdateById(List<TModel> modelList)
        {
            return _sqlRepository.BactchUpdateById(modelList);
        }

        /// <summary>
        /// 根据logicalId更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public async Task<int> BactchUpdateByIdAsync(List<TModel> modelList)
        {
            return await _sqlRepository.BactchUpdateByIdAsync(modelList);
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        public int BactchUpdateSpecifyFieldsById(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            return _sqlRepository.BactchUpdateSpecifyFieldsById(bactchUpdateSpecifyFields);
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        public async Task<int> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            return await _sqlRepository.BactchUpdateSpecifyFieldsByIdAsync(bactchUpdateSpecifyFields);
        }

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public long GetTotalCount(TCondition condition)
        {
            return _sqlRepository.GetTotalCount(condition);
        }

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<long> GetTotalCountAsync(TCondition condition)
        {
            return await _sqlRepository.GetTotalCountAsync(condition);
        }

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public TModel GetModelById(string logicalId, string queryFields = "")
        {
            return _sqlRepository.GetModelById(logicalId, queryFields);
        }

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public async Task<TModel> GetModelByIdAsync(string logicalId, string queryFields = "")
        {
            return await _sqlRepository.GetModelByIdAsync(logicalId, queryFields);
        }

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public T GetModelById<T>(string logicalId, string queryFields = "")
        {
            return _sqlRepository.GetModelById<T>(logicalId, queryFields);
        }

        /// <summary>
        /// 根据logicalId获取一个模型数据
        /// </summary>
        /// <param name="logicalId">logicalId</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public async Task<T> GetModelByIdAsync<T>(string logicalId, string queryFields = "")
        {
            return await _sqlRepository.GetModelByIdAsync<T>(logicalId, queryFields);
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        public TModel GetOneModel(TCondition condition)
        {
            return _sqlRepository.GetOneModel(condition);
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        public async Task<TModel> GetOneModelAsync(TCondition condition)
        {
            return await _sqlRepository.GetOneModelAsync(condition);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<TModel> GetAllListByIdList(List<string> idList, string queryFields = "")
        {
            return _sqlRepository.GetAllListByIdList(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<TModel>> GetAllListByIdListAsync(List<string> idList, string queryFields = "")
        {
            return await _sqlRepository.GetAllListByIdListAsync(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<T> GetAllListByIdList<T>(List<string> idList, string queryFields = "")
        {
            return _sqlRepository.GetAllListByIdList<T>(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logicalId集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<T>> GetAllListByIdListAsync<T>(List<string> idList, string queryFields = "")
        {
            return await _sqlRepository.GetAllListByIdListAsync<T>(idList, queryFields);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        public List<TModel> GetPageList(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            return _sqlRepository.GetPageList(pageQueryCondition);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        public async Task<List<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            return await _sqlRepository.GetPageListAsync(pageQueryCondition);
        }

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        public List<TModel> GetAllList(TCondition model, int limitNum = 0)
        {
            return _sqlRepository.GetAllList(model, limitNum);
        }

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<TModel>> GetAllListAsync(TCondition model, int limitNum = 0)
        {
            return await _sqlRepository.GetAllListAsync(model, limitNum);
        }

        #region 需派生类实现的方法
        /// <summary>
        /// 构建查询条件时，忽略使用like的字段
        /// </summary>
        /// <returns></returns>
        public virtual string[] SqlWhereIgnoreLikeFields()
        {
            return null;
        }
        /// <summary>
        /// 根据model构建查询条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="table">表名</param>
        /// <returns></returns>
        public virtual StringBuilder GetSqlWhereByModel(TCondition model, string table = "")
        {
            #region 根据model构建查询条件
            var fieldsDic = GetAllFields(null, true);
            // 构建查查询条件sql
            StringBuilder sqlSB = new();
            if (fieldsDic?.Count > 0)
            {
                sqlSB.Append(" 1=1 ");
            }

            // 如果没有给查询条件，那么直接获取全部数据
            if (model == null)
            {
                return sqlSB;
            }

            // sql注入校验
            SQLInjectionUtil.CheckSQLInjection(model.Orderby);

            bool islike = model.IsNotLike < 1;
            var ignoreLikeFields = SqlWhereIgnoreLikeFields();

            table ??= TableName;
            string tableAlias = string.IsNullOrEmpty(table) ? "" : $"`{table}`" + ".";
            foreach ((string propName, string dbFieldName, Type propType) in fieldsDic)
            {
                Type modelType = model.GetType();
                var value = modelType.GetProperty(propName).GetValue(model);


                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    if (propType == typeof(string))
                    {
                        if (islike && (ignoreLikeFields == null || !ignoreLikeFields.Contains(dbFieldName)))
                        {
                            sqlSB.Append($"and {tableAlias}`{dbFieldName}` like CONCAT(CONCAT('%',@{propName}),'%') ");
                        }
                        else
                        {
                            sqlSB.Append($"and {tableAlias}`{propName}`=@{propName} ");
                        }
                    }
                    else if (propType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(value.ToString(), out DateTime val) && PublicTools.IsEffectiveDateTime(val))
                        {
                            // 时间查询
                            if (dbFieldName.EndsWith("_start"))
                            {
                                sqlSB.Append($"and {tableAlias}`{dbFieldName}`>=@{propName} ");
                            }
                            else if (dbFieldName.EndsWith("_end"))
                            {
                                sqlSB.Append($"and {tableAlias}`{dbFieldName}`<=@{propName} ");
                            }
                            else
                            {
                                sqlSB.Append($"and {tableAlias}`{dbFieldName}`=@{propName} ");
                            }
                        }
                    }
                    else if (propType.IsValueType)
                    {
                        if (decimal.TryParse(value.ToString(), out decimal val) && val > 0)
                        {
                            //数值查询
                            //只有当数值大于0时才做条件处理
                            sqlSB.Append($"and {tableAlias}`{dbFieldName}`=@{propName} ");
                        }
                    }
                }
            }
            // 主键 排除查询条件
            if (!string.IsNullOrEmpty(model.ExcludeId))
            {
                sqlSB.Append($"and {tableAlias}`logicalId`<>@excludeId ");
            }

            // 获取模糊查询条件
            StringBuilder sqlFuzzySearch = GetfuzzySearchWhere(model, table);
            if (sqlFuzzySearch != null && sqlFuzzySearch.Length > 0)
            {
                sqlSB.Append(sqlFuzzySearch);
            }

            return sqlSB;

            #endregion
        }

        /// <summary>
        /// 获取表全部字段
        /// </summary>
        /// <param name="ignoreField">忽略的字段</param>
        /// <param name="isCondition">是否是条件模板</param>
        /// <returns>(类字段，表字段，类字段类型)</returns>
        public virtual List<(string, string, Type)> GetAllFields(string[] ignoreField = null, bool isCondition = false)
        {
            Type mType = typeof(TModel);
            if (isCondition)
            {
                mType = typeof(TCondition);
            }
            List<(string, string, Type)> fieldMap = new();
            ignoreField = ignoreField ?? Array.Empty<string>();
            foreach (var propInfo in mType.GetProperties())
            {
                var attr = propInfo.GetCustomAttribute<FieldAttribute>();
                if (attr != null)
                {
                    string dbFieldName = StringUtil.PascalToSnakeCase(propInfo.Name);
                    if (propInfo.PropertyType.IsPublic
                        && !_globalIgnoreField.Contains(dbFieldName)
                        && !ignoreField.Contains(dbFieldName))
                    {
                        fieldMap.Add((propInfo.Name, dbFieldName, propInfo.PropertyType));
                    }
                }
            }

            return fieldMap;
        }
        /// <summary>
        /// 获取表全部字段(不包含id)
        /// </summary>
        /// <returns></returns>
        public virtual string GetAllFieldForSql(string tableAlias = "", bool needColumnAlias = true)
        {
            Type mType = typeof(TModel);
            tableAlias = string.IsNullOrEmpty(tableAlias) ? "" : (tableAlias.Contains('.') ? tableAlias : tableAlias + ".");
            StringBuilder stringBuilder = new();
            string[] ignoreField = new string[] { "id", "created_time_start", "created_time_end", "updated_time_start", "updated_time_end" };
            var fields = GetAllFields(ignoreField);
            foreach ((string propName, string dbFieldName, Type propType) in fields)
            {
                stringBuilder.Append($",{tableAlias}`{dbFieldName}` {(needColumnAlias ? propName : "")}");

            }

            string sql = stringBuilder.ToString().TrimStart(',');
            return sql;
        }
        /// <summary>
        /// 获取表全部字段，并标记为@参数(不包含id)
        /// </summary>
        /// <returns></returns>
        public virtual string GetAllFieldForParams()
        {
            Type mType = typeof(TModel);
            StringBuilder stringBuilder = new();
            string[] ignoreField = new string[] { "id", "created_time_start", "created_time_end", "updated_time_start", "updated_time_end" };
            var fields = GetAllFields(ignoreField);
            foreach ((string propName, string dbFieldName, Type propType) in fields)
            {
                stringBuilder.Append($",@{propName}");

            }

            string sql = stringBuilder.ToString().TrimStart(',');
            return sql;
        }

        /// <summary>
        /// 获取表的插入语句 
        /// </summary>
        /// <returns>插入语句</returns>
        public virtual string GetInsertSql()
        {
            // 构建插入语句
            string sql = $@"insert into `{TableName}` ({GetAllFieldForSql("",false)}) values({GetAllFieldForParams()})";
            if (sql.Contains("@CreatedTime"))
            {
                sql = sql.Replace("@CreatedTime", $"'{PublicTools.GetSysDateTimeNowStringYMDHMS()}'");
            }
            return sql;
        }

        /// <summary>
        /// 获取更新sql
        /// </summary>
        /// <returns></returns>
        public virtual string GetUpdateSql()
        {
            Type mType = typeof(TModel);
            StringBuilder stringBuilder = new();
            string[] ignoreField = new string[] { "id", "logical_id", "created_id", "created_name", "created_time_start", "created_time_end", "updated_time_start", "updated_time_end" };
            var fields = GetAllFields(ignoreField);
            foreach ((string propName, string dbFieldName, Type propType) in fields)
            {
                stringBuilder.Append($",`{dbFieldName}`=@{propName}");

            }
            string sql = $"update `{TableName}` set {stringBuilder.ToString().TrimStart(',')}  where logical_id=@logicalId";
            if (sql.Contains("@updated_name"))
            {
                sql = sql.Replace("@updated_name", $"'{PublicTools.GetSysDateTimeNowStringYMDHMS()}'");
            }
            return sql;
        }
        #endregion

        public StringBuilder GetfuzzySearchWhere(MBaseModel model, string table = "")
        {
            return _sqlRepository.GetfuzzySearchWhere(model, table);
        }
        #region sql格式化  

        /// <summary>
        /// 格式化数据库名称
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected string FormattingTableName(string tableName)
        {
            return _sqlRepository.FormattingTableName(tableName);
        }

        /// <summary>
        /// 根据数据库类型格式化sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected string FormattingSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) return sql;
            else
            {
                switch (this.DbType)
                {
                    case DBType.MySql:
                        break;
                    case DBType.PGSql:
                        sql = sql.Replace("`", "\"");
                        break;
                    case DBType.Oracle:
                    default:
                        break;

                }
                return sql;
            }
        }

        public string FormattingInSql<T2>(string field, List<T2> fieldValue)
        {
            return _sqlRepository.FormattingInSql(field, fieldValue);
        }
        #endregion
        #region 常用的公共方法

        #endregion

    }
}
