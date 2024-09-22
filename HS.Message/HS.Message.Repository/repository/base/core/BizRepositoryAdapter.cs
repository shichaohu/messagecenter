using HS.Message.Repository.database.tool;
using HS.Message.Share.Attributes;
using HS.Message.Share.BaseModel;
using HS.Message.Share.CommonObject;
using HS.Message.Share.Utils;
using System.Reflection;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.@base.core
{
    public abstract class BizRepositoryAdapter<TModel, TCondition> where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        protected string TableName { get; }
        private IBizRepository<TModel, TCondition> _sqlRepository;
        protected BaseDapperTool<TModel> _dapperTool;
        protected DBType DbType { get; }
        protected string[] _globalIgnoreField;

        protected readonly IRepositoryInjectedObjects _injectedObjects;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizRepositoryAdapter(IRepositoryInjectedObjects injectedObjects, string tableName)
        {
            _injectedObjects = injectedObjects;
            TableName = tableName;
            if (!Enum.TryParse(_injectedObjects.Configuration["ConnectionStrings:DatabaseType"], true, out DBType dbType))
            {
                dbType = DBType.MySql;
            }
            DbType = dbType;
            switch (dbType)
            {
                case DBType.MySql:
                    _sqlRepository = new MySqlBizRepository<TModel, TCondition>(_injectedObjects.Configuration, tableName);
                    break;
                case DBType.PGSql:
                    _sqlRepository = new PGSqlBizRepository<TModel, TCondition>(_injectedObjects.Configuration, tableName);
                    break;
                case DBType.Oracle:
                default:
                    _sqlRepository = new MySqlBizRepository<TModel, TCondition>(_injectedObjects.Configuration, tableName);
                    break;
            }
            _dapperTool = _sqlRepository.DapperTool;
            _sqlRepository.GetSqlWhereByModelFunc = GetSqlWhereByModel;
            _sqlRepository.GetAllFieldFunc = GetAllFieldForSql;
            _sqlRepository.GetInsertSqlFunc = GetInsertSql;
            _sqlRepository.GetUpdateSqlFunc = GetUpdateSql;


            _globalIgnoreField = new string[] { "queryFields", "orderby", "isNotLike", "fuzzySearchKeyWord", "fuzzySearchFields", "isLanguageHandle", "languageHandleEcludeField", "limitNum" };
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
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public int DeleteById(string logical_id)
        {
            return _sqlRepository.DeleteById(logical_id);
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public async Task<int> DeleteByIdAsync(string logical_id)
        {
            return await _sqlRepository.DeleteByIdAsync(logical_id);
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public int BactchDeleteByIdList(List<string> idList)
        {
            return _sqlRepository.BactchDeleteByIdList(idList);
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchDeleteByIdListAsync(List<string> idList)
        {
            return await _sqlRepository.BactchDeleteByIdListAsync(idList);
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public int LogicDeleteById(string logical_id)
        {
            return _sqlRepository.LogicDeleteById(logical_id);
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public async Task<int> LogicDeleteByIdAsync(string logical_id)
        {
            return await _sqlRepository.LogicDeleteByIdAsync(logical_id);
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public int BactchLogicDeleteByIdList(List<string> idList)
        {
            return _sqlRepository.BactchLogicDeleteByIdList(idList);
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchLogicDeleteByIdListAsync(List<string> idList)
        {
            return await _sqlRepository.BactchLogicDeleteByIdListAsync(idList);
        }

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public int UpdateById(TModel model)
        {
            return _sqlRepository.UpdateById(model);
        }

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public async Task<int> UpdateByIdAsync(TModel model)
        {
            return await _sqlRepository.UpdateByIdAsync(model);
        }

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public int BactchUpdateById(List<TModel> modelList)
        {
            return _sqlRepository.BactchUpdateById(modelList);
        }

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
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
        /// <param name="idList">logical_id集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        public int BactchUpdateSpecifyFieldsById(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            return _sqlRepository.BactchUpdateSpecifyFieldsById(bactchUpdateSpecifyFields);
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
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
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public TModel GetModelById(string logical_id, string queryFields = "")
        {
            return _sqlRepository.GetModelById(logical_id, queryFields);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public async Task<TModel> GetModelByIdAsync(string logical_id, string queryFields = "")
        {
            return await _sqlRepository.GetModelByIdAsync(logical_id, queryFields);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public T GetModelById<T>(string logical_id, string queryFields = "")
        {
            return _sqlRepository.GetModelById<T>(logical_id, queryFields);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public async Task<T> GetModelByIdAsync<T>(string logical_id, string queryFields = "")
        {
            return await _sqlRepository.GetModelByIdAsync<T>(logical_id, queryFields);
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
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<TModel> GetAllListByIdList(List<string> idList, string queryFields = "")
        {
            return _sqlRepository.GetAllListByIdList(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<TModel>> GetAllListByIdListAsync(List<string> idList, string queryFields = "")
        {
            return await _sqlRepository.GetAllListByIdListAsync(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<T> GetAllListByIdList<T>(List<string> idList, string queryFields = "")
        {
            return _sqlRepository.GetAllListByIdList<T>(idList, queryFields);
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
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
            var fieldsDic = GetAllFields();
            // 构建查查询条件sql
            StringBuilder sqlSB = new();
            if (fieldsDic?.Keys.Count > 0)
            {
                sqlSB.Append(" 1=1 ");
            }

            // 如果没有给查询条件，那么直接获取全部数据
            if (model == null)
            {
                return sqlSB;
            }

            // sql注入校验
            SQLInjectionUtil.CheckSQLInjection(model.orderby);

            bool islike = model.isNotLike < 1;
            var ignoreLikeFields = SqlWhereIgnoreLikeFields();

            table ??= TableName;
            string tableAlias = string.IsNullOrEmpty(table) ? "" : $"`{table}`" + ".";
            foreach (var field in fieldsDic)
            {
                Type modelType = model.GetType();
                var value = modelType.GetProperty(field.Key).GetValue(model);


                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    if (field.Key == "belong_airport_three" && string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        value = _injectedObjects.HttpContextInfo.GetBelongAirportThree();
                    }

                    if (field.Value == typeof(string))
                    {
                        if (islike && (ignoreLikeFields == null || !ignoreLikeFields.Contains(field.Key)))
                        {
                            sqlSB.Append($"and {tableAlias}`{field.Key}` like CONCAT(CONCAT('%',@{field.Key}),'%') ");
                        }
                        else
                        {
                            sqlSB.Append($"and {tableAlias}`{field.Key}`=@{field.Key} ");
                        }
                    }
                    else if (field.Value == typeof(DateTime))
                    {
                        if (DateTime.TryParse(value.ToString(), out DateTime val) && PublicTools.IsEffectiveDateTime(val))
                        {
                            // 时间查询
                            if (field.Key.EndsWith("_start"))
                            {
                                sqlSB.Append($"and {tableAlias}`{field.Key}`>=@{field.Key} ");
                            }
                            else if (field.Key.EndsWith("_end"))
                            {
                                sqlSB.Append($"and {tableAlias}`{field.Key}`<=@{field.Key} ");
                            }
                            else
                            {
                                sqlSB.Append($"and {tableAlias}`{field.Key}`=@{field.Key} ");
                            }
                        }
                    }
                    else if (field.Value.IsValueType)
                    {
                        if (decimal.TryParse(value.ToString(), out decimal val) && val > 0)
                        {
                            //数值查询
                            //只有当数值大于0时才做条件处理
                            sqlSB.Append($"and {tableAlias}`{field.Key}`=@{field.Key} ");
                        }
                    }
                }
            }
            // 主键 排除查询条件
            if (!string.IsNullOrEmpty(model.excludeId))
            {
                sqlSB.Append($"and {tableAlias}`logical_id`<>@excludeId ");
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
        /// <returns></returns>
        public virtual Dictionary<string, Type> GetAllFields(string[] ignoreField = null)
        {
            Type mType = typeof(TModel);
            Dictionary<string, Type> fieldMap = new();
            ignoreField = ignoreField ?? Array.Empty<string>();
            foreach (var propInfo in mType.GetProperties())
            {
                var attr = propInfo.GetCustomAttribute<FieldAttribute>();
                if (attr != null)
                {
                    if (propInfo.PropertyType.IsPublic
                        && !_globalIgnoreField.Contains(propInfo.Name)
                        && !ignoreField.Contains(propInfo.Name))
                    {
                        fieldMap.TryAdd(propInfo.Name, propInfo.PropertyType);
                    }
                }
            }

            return fieldMap;
        }
        /// <summary>
        /// 获取表全部字段(不包含id)
        /// </summary>
        /// <returns></returns>
        public virtual string GetAllFieldForSql(string tableAlias = "")
        {
            Type mType = typeof(TModel);
            tableAlias = string.IsNullOrEmpty(tableAlias) ? "" : (tableAlias.Contains('.') ? tableAlias : tableAlias + ".");
            StringBuilder stringBuilder = new();
            string[] ignoreField = new string[] { "id" };
            Dictionary<string, Type> fields = GetAllFields(ignoreField);
            foreach (var field in fields)
            {
                stringBuilder.Append($",{tableAlias}`{field.Key}`");

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
            string[] ignoreField = new string[] { "id" };
            Dictionary<string, Type> fields = GetAllFields(ignoreField);
            foreach (var field in fields)
            {
                stringBuilder.Append($",@{field.Key}");

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
            string sql = $@"insert into `{TableName}` ({GetAllFieldForSql()}) values({GetAllFieldForParams()})";
            if (sql.Contains("@created_time"))
            {
                sql = sql.Replace("@created_time", $"'{PublicTools.GetSysDateTimeNowStringYMDHMS()}'");
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
            string[] ignoreField = new string[] { "id", "logical_id", "created_id", "created_name" };
            Dictionary<string, Type> fields = GetAllFields(ignoreField);
            foreach (var field in fields)
            {
                stringBuilder.Append($",`{field.Key}`=@{field.Key}");

            }
            string sql = $"update `{TableName}` set {stringBuilder.ToString().TrimStart(',')}  where logical_id=@logical_id";
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

        /// <summary>
        /// 构建最基本的查询条件
        /// 包含机场三字码、是否删除=2
        /// </summary>
        /// <param name="table">表的别名</param>
        /// <param name="hasOtherConditionInFront">前面是否有其他查询条件</param>
        /// <returns></returns>
        public string GetBaseSqlWhere(string table = "", bool hasOtherConditionInFront = true)
        {
            StringBuilder sqlSB = new();
            table ??= TableName;
            string tableAlias = string.IsNullOrEmpty(table) ? "" : $"`{table}`" + ".";
            string belong_airport_three = _injectedObjects.HttpContextInfo.GetBelongAirportThree();
            if (!string.IsNullOrEmpty(belong_airport_three))
            {
                sqlSB.Append($" {(hasOtherConditionInFront ? "and " : "")}{tableAlias}`belong_airport_three`='{belong_airport_three}' ");
            }
            sqlSB.Append($" and {tableAlias}`is_delete`=2 ");

            return sqlSB.ToString();
        }
        /// <summary>
        /// 构建机场三字码的查询条件（只包含三字码）
        /// </summary>
        /// <param name="table">表的别名</param>
        /// <param name="hasOtherConditionInFront">前面是否有其他查询条件</param>
        /// <returns></returns>
        public string GetBelongAirportThreeSqlWhere(string table = "", bool hasOtherConditionInFront = false)
        {
            StringBuilder sqlSB = new();
            table ??= TableName;
            string tableAlias = string.IsNullOrEmpty(table) ? "" : $"`{table}`" + ".";
            string belong_airport_three = _injectedObjects.HttpContextInfo.GetBelongAirportThree();
            if (!string.IsNullOrEmpty(belong_airport_three))
            {
                sqlSB.Append($" {(hasOtherConditionInFront ? "and " : "")}{tableAlias}`belong_airport_three`='{belong_airport_three}' ");
            }

            return sqlSB.ToString();
        }

        /// <summary>
        /// 获取机场三字码
        /// </summary>
        /// <param name="table"></param>
        /// <param name="hasOtherConditionInFront"></param>
        /// <returns></returns>
        public string GetBelongAirportThree()
        {
            string belong_airport_three = _injectedObjects.HttpContextInfo.GetBelongAirportThree();

            return belong_airport_three;
        }
        #endregion

    }
}
