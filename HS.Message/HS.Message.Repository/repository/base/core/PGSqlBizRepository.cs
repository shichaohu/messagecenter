using HS.Message.Share.BaseModel;
using HS.Message.Share.Utils;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HS.Message.Repository.repository.@base.core
{
    public class PGSqlBizRepository<TModel, TCondition> : database.core.PGSqlRepository<TModel>, IBizRepository<TModel, TCondition> where TModel : MBaseModel, new() where TCondition : MBaseModel, new()
    {
        public string TableName { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PGSqlBizRepository(IConfiguration configuration, string tableName) : base(configuration)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        public new int AddOne(TModel model)
        {
            // 初始化主键
            if (string.IsNullOrEmpty(model.LogicalId))
            {
                model.LogicalId = CreatKey(1);

            }

            // 执行插入操作
            return base.AddOne(model);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns>处理结果</returns>
        public async new Task<int> AddOneAsync(TModel model)
        {
            // 初始化主键
            if (string.IsNullOrEmpty(model.LogicalId))
            {
                model.LogicalId = CreatKey(1);

            }

            // 执行插入操作
            return await base.AddOneAsync(model);
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        public int BactchAdd(List<TModel> modelList)
        {
            List<TModel> addList = new List<TModel>();
            foreach (var model in modelList)
            {
                // 初始化主键
                if (string.IsNullOrEmpty(model.LogicalId))
                {
                    model.LogicalId = CreatKey(1);

                    // 对上生成的唯一主键做一个判重，如果重复，那么重新生成，尝试重新生成3次
                    int num = 0;
                    while (num < 3 && addList.Exists(x => x.LogicalId == model.LogicalId))
                    {
                        Thread.Sleep(200);//停200秒 
                        model.LogicalId = CreatKey(1);
                        num++;
                    }
                }

                addList.Add(model);
            }

            // 执行插入操作
            return BactchAddData(addList);
        }

        /// <summary>
        /// 批量新增数据记录
        /// </summary>
        /// <param name="modelList">数据模型集合</param>
        /// <returns>影响的行数</returns>
        public async Task<int> BactchAddAsync(List<TModel> modelList)
        {
            List<TModel> addList = new List<TModel>();
            foreach (var model in modelList)
            {
                // 初始化主键
                if (string.IsNullOrEmpty(model.LogicalId))
                {
                    model.LogicalId = CreatKey(1);

                    // 对上生成的唯一主键做一个判重，如果重复，那么重新生成，尝试重新生成3次
                    int num = 0;
                    while (num < 3 && addList.Exists(x => x.LogicalId == model.LogicalId))
                    {
                        Thread.Sleep(200);//停200秒 
                        model.LogicalId = CreatKey(1);
                        num++;
                    }
                }

                addList.Add(model);
            }

            // 执行插入操作
            return await BactchAddDataAsync(addList);
        }
        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public int DeleteById(string logical_id)
        {
            // 执行删除操作
            return DapperTool.ExecuteNonQuery($" delete from  \"{TableName}\" where logical_id=@logical_id", new TModel() { LogicalId = logical_id });
        }
        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>

        public async Task<int> DeleteByIdAsync(string logical_id)
        {
            // 执行删除操作
            return await DapperTool.ExecuteNonQueryAsync($" delete from  \"{TableName}\" where logical_id=@logical_id", new TModel() { LogicalId = logical_id });
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public int BactchDeleteByIdList(List<string> idList)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", idList);
            sqlWehre.CheckSQLInjection();

            // 执行删除操作
            return DapperTool.ExecuteNonQuery($" delete from  \"{TableName}\" where logical_id logical_id ('" + sqlWehre + "')");
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchDeleteByIdListAsync(List<string> idList)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", idList);
            sqlWehre.CheckSQLInjection();

            // 执行删除操作
            return await DapperTool.ExecuteNonQueryAsync($" delete from  \"{TableName}\" where logical_id logical_id ('" + sqlWehre + "')");
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public int LogicDeleteById(string logical_id)
        {
            // 执行删除操作
            return DapperTool.ExecuteNonQuery($" update \"{TableName}\" set \"\"=1 where logical_id=@logical_id", new TModel() { LogicalId = logical_id });
        }

        /// <summary>
        /// 根据logical_id删除数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <returns>影响的行数</returns>
        public async Task<int> LogicDeleteByIdAsync(string logical_id)
        {
            // 执行删除操作
            return await DapperTool.ExecuteNonQueryAsync($" update \"{TableName}\" set \"\"=1 where logical_id=@logical_id", new TModel() { LogicalId = logical_id });
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public int BactchLogicDeleteByIdList(List<string> idList)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", idList);
            sqlWehre.CheckSQLInjection();

            // 执行删除操作
            return DapperTool.ExecuteNonQuery($" update \"{TableName}\" set \"\"=1  where logical_id in ('{sqlWehre}')");
        }

        /// <summary>
        /// 根据logical_id集合批量删除数据
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <returns>处理结果</returns>
        public async Task<int> BactchLogicDeleteByIdListAsync(List<string> idList)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", idList);
            sqlWehre.CheckSQLInjection();

            // 执行删除操作
            return await DapperTool.ExecuteNonQueryAsync($" update \"{TableName}\" set \"\"=1  where logical_id in ('{sqlWehre}')");
        }

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public int UpdateById(TModel model)
        {
            return DapperTool.ExecuteNonQuery(GetUpdateSql(), model);
        }

        /// <summary>
        /// 根据logical_id更新数据实体
        /// </summary>
        /// <param name="model">实体模型</param>
        /// <returns>更新结果</returns>
        public async Task<int> UpdateByIdAsync(TModel model)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetUpdateSql(), model);
        }

        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public int BactchUpdateById(List<TModel> modelList)
        {
            var result = 0;
            using (IDbConnection dbConnection = DapperTool.GetDbConnection())
            {
                // 执行删除操作
                dbConnection.Open();
                using (IDbTransaction transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var model in modelList)
                        {
                            result = dbConnection.Execute(GetUpdateSql(), model);
                            if (result <= 0)
                            {
                                transaction.Rollback();
                                break;
                            }
                        }

                        if (result > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw exception;
                    }
                }

                dbConnection.Close();
                dbConnection.Dispose();
            }
            return result;
        }
        /// <summary>
        /// 根据logical_id更新数据实体(批量更新)
        /// </summary>
        /// <param name="modelList">实体模型集合</param>
        /// <returns>更新结果</returns>
        public async Task<int> BactchUpdateByIdAsync(List<TModel> modelList)
        {
            var result = 0;
            using (IDbConnection dbConnection = DapperTool.GetDbConnection())
            {
                // 执行删除操作
                dbConnection.Open();
                using (IDbTransaction transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var model in modelList)
                        {
                            result = await dbConnection.ExecuteAsync(GetUpdateSql(), model);
                            if (result <= 0)
                            {
                                transaction.Rollback();
                                break;
                            }
                        }

                        if (result > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw exception;
                    }
                }

                dbConnection.Close();
                dbConnection.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        public int BactchUpdateSpecifyFieldsById(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", bactchUpdateSpecifyFields.idList);
            sqlWehre.CheckSQLInjection();

            StringBuilder updateSql = new StringBuilder($"update \"{TableName}\" set ");
            StringBuilder fieldValue = new StringBuilder();
            foreach (var item in bactchUpdateSpecifyFields.updateFieldsValue)
            {
                updateSql.Append($"\"{item.Key}\"='{item.Value}',");
                fieldValue.Append($"{item.Key}{item.Value}");
            }
            fieldValue.ToString().CheckSQLInjection();

            return DapperTool.ExecuteNonQuery($"{updateSql.ToString().Trim(',')} where logical_id in ('{sqlWehre}') ");
        }
        /// <summary>
        /// 批量更新指定字段的值(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="updateFieldsValue">跟新字段键值对</param>
        /// <returns>所有数据集合</returns>
        public async Task<int> BactchUpdateSpecifyFieldsByIdAsync(MBactchUpdateSpecifyFields<string> bactchUpdateSpecifyFields)
        {
            // 校验SQL注入
            string sqlWehre = string.Join("','", bactchUpdateSpecifyFields.idList);
            sqlWehre.CheckSQLInjection();

            StringBuilder updateSql = new StringBuilder($"update \"{TableName}\" set ");
            StringBuilder fieldValue = new StringBuilder();
            foreach (var item in bactchUpdateSpecifyFields.updateFieldsValue)
            {
                updateSql.Append($"\"{item.Key}\"='{item.Value}',");
                fieldValue.Append($"{item.Key}{item.Value}");
            }
            fieldValue.ToString().CheckSQLInjection();

            return await DapperTool.ExecuteNonQueryAsync($"{updateSql.ToString().Trim(',')} where logical_id in ('{sqlWehre}') ");
        }

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public long GetTotalCount(TCondition condition)
        {
            return DapperTool.GetTotal($"\"{TableName}\"", GetSqlWhereByModel(condition), condition);
        }

        /// <summary>
        /// 根据条件获取总条数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<long> GetTotalCountAsync(TCondition condition)
        {
            return await DapperTool.GetTotalAsync($"\"{TableName}\"", GetSqlWhereByModel(condition), condition);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public TModel GetModelById(string logical_id, string queryFields = "")
        {

            return GetModelById<TModel>(logical_id, queryFields);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        public async Task<TModel> GetModelByIdAsync(string logical_id, string queryFields = "")
        {

            return await GetModelByIdAsync<TModel>(logical_id, queryFields);
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
		public T GetModelById<T>(string logical_id, string queryFields = "")
        {
            T model = DapperTool.GetModel<TModel, T>(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields,
                $"\"{TableName}\"",
                $" 1=1  and logical_id=@logical_id ",
                new TModel() { LogicalId = logical_id }
                );
            ModelUtil.AutoFill(model, autoFillFieldsList);
            return model;
        }

        /// <summary>
        /// 根据logical_id获取一个模型数据
        /// </summary>
        /// <param name="logical_id">logical_id</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>模型数据</returns>
        public async Task<T> GetModelByIdAsync<T>(string logical_id, string queryFields = "")
        {
            T model = await DapperTool.GetModelAsync<TModel, T>(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields,
                $"\"{TableName}\"",
                $" 1=1  and logical_id=@logical_id ",
                new TModel() { LogicalId = logical_id }
                );
            ModelUtil.AutoFill(model, autoFillFieldsList);
            return model;
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        public TModel GetOneModel(TCondition condition)
        {
            var sqlSB = GetSqlWhereByModel(condition);

            TModel model = DapperTool.GetModel<TModel>(condition == null || string.IsNullOrEmpty(condition.QueryFields) ? GetAllField() : condition.QueryFields,
                    $"\"{TableName}\"",
                    sqlSB,
                    condition as TModel
                    );

            ModelUtil.AutoFill(model, autoFillFieldsList);
            return model;
        }

        /// <summary>
        /// 根据条件获取一个模型数据
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns>模型数据</returns>
        public async Task<TModel> GetOneModelAsync(TCondition condition)
        {
            var sqlSB = GetSqlWhereByModel(condition);

            TModel model = await DapperTool.GetModelAsync<TModel>(condition == null || string.IsNullOrEmpty(condition.QueryFields) ? GetAllField() : condition.QueryFields,
                    $"\"{TableName}\"",
                    sqlSB,
                    condition as TModel
                    );

            ModelUtil.AutoFill(model, autoFillFieldsList);
            return model;
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<TModel> GetAllListByIdList(List<string> idList, string queryFields = "")
        {
            // 校验SQL注入
            string sqlWehre = $"{string.Join("','", idList)}";
            sqlWehre.CheckSQLInjection();

            // 构建查询语句
            string sql = $"SELECT {(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields)} FROM \"{TableName}\" where logical_id in ('" + sqlWehre + "') ;";
            var dataList = DapperTool.Query<TModel>(sql).ToList();
            ModelUtil.AutoFill(dataList, autoFillFieldsList);
            return dataList;
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<TModel>> GetAllListByIdListAsync(List<string> idList, string queryFields = "")
        {
            // 校验SQL注入
            string sqlWehre = $"{string.Join("','", idList)}";
            sqlWehre.CheckSQLInjection();

            // 构建查询语句
            string sql = $"SELECT {(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields)} FROM \"{TableName}\" where logical_id in ('" + sqlWehre + "') ;";
            var dataList = (await DapperTool.QueryAsync<TModel>(sql)).ToList();
            ModelUtil.AutoFill(dataList, autoFillFieldsList);
            return dataList;
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public List<T> GetAllListByIdList<T>(List<string> idList, string queryFields = "")
        {
            // 校验SQL注入
            string sqlWehre = $"{string.Join("','", idList)}";
            sqlWehre.CheckSQLInjection();

            // 构建查询语句
            string sql = $"SELECT {(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields)} FROM \"{TableName}\" where logical_id in ('" + sqlWehre + "') ;";
            var dataList = DapperTool.Query<T>(sql).ToList();
            ModelUtil.AutoFill(dataList, autoFillFieldsList);
            return dataList;
        }

        /// <summary>
        /// 获取所有数据集合(根据主键集合)
        /// </summary>
        /// <param name="idList">logical_id集合</param>
        /// <param name="queryFields">需要查询的字段，空代表获取全部，默认为空</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<T>> GetAllListByIdListAsync<T>(List<string> idList, string queryFields = "")
        {
            // 校验SQL注入
            string sqlWehre = $"{string.Join("','", idList)}";
            sqlWehre.CheckSQLInjection();

            // 构建查询语句
            string sql = $"SELECT {(string.IsNullOrEmpty(queryFields) ? GetAllField() : queryFields)} FROM \"{TableName}\" where logical_id in ('" + sqlWehre + "') ;";
            var dataList = (await DapperTool.QueryAsync<T>(sql)).ToList();
            ModelUtil.AutoFill(dataList, autoFillFieldsList);
            return dataList;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        public List<TModel> GetPageList(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            #region 初始化分页条件

            var condition = pageQueryCondition.condition;

            MPageInfo pageInfor = new MPageInfo()
            {
                orderby = string.IsNullOrEmpty(pageQueryCondition.orderby) ? " logical_id desc "
                : pageQueryCondition.orderby,
                pageIndex = pageQueryCondition.pageIndex < 1 ? 1 : pageQueryCondition.pageIndex,
                pageSize = pageQueryCondition.pageSize < 1 ? 20 : pageQueryCondition.pageSize
            };

            #endregion

            // 获取查询条件
            var sql = GetSqlWhereByModel(condition);

            // 调用通用分页查询接口
            List<TModel> listModel = DapperTool.GetPageList<TCondition, TModel>(string.IsNullOrEmpty(condition.QueryFields) ? GetAllField() : condition.QueryFields,
                 $"\"{TableName}\"", sql, pageInfor, condition);
            pageQueryCondition.pageIndex = pageInfor.pageIndex;
            pageQueryCondition.pageSize = pageInfor.pageSize;
            pageQueryCondition.total = pageInfor.total;

            ModelUtil.AutoFill(listModel, autoFillFieldsList);
            return listModel;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageQueryCondition">查询条件</param>
        /// <returns>符合要求的分页数据集合</returns>
        public async Task<List<TModel>> GetPageListAsync(MPageQueryCondition<TCondition> pageQueryCondition)
        {
            #region 初始化分页条件

            var condition = pageQueryCondition.condition;

            MPageInfo pageInfor = new MPageInfo()
            {
                orderby = string.IsNullOrEmpty(pageQueryCondition.orderby) ? " logical_id desc "
                : pageQueryCondition.orderby,
                pageIndex = pageQueryCondition.pageIndex < 1 ? 1 : pageQueryCondition.pageIndex,
                pageSize = pageQueryCondition.pageSize < 1 ? 20 : pageQueryCondition.pageSize
            };

            #endregion

            // 获取查询条件
            var sql = GetSqlWhereByModel(condition);

            // 调用通用分页查询接口
            List<TModel> listModel = await DapperTool.GetPageListAsync<TCondition, TModel>(string.IsNullOrEmpty(condition.QueryFields) ? GetAllField() : condition.QueryFields,
                 $"\"{TableName}\"", sql, pageInfor, condition);
            pageQueryCondition.pageIndex = pageInfor.pageIndex;
            pageQueryCondition.pageSize = pageInfor.pageSize;
            pageQueryCondition.total = pageInfor.total;

            ModelUtil.AutoFill(listModel, autoFillFieldsList);
            return listModel;
        }

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        public List<TModel> GetAllList(TCondition model, int limitNum = 0)
        {
            // 构建查询语句
            string sql = $"SELECT {(model == null || string.IsNullOrEmpty(model.QueryFields) ? GetAllField() : model.QueryFields)} FROM \"{TableName}\" where 1=1 and {GetSqlWhereByModel(model)}  ORDER BY {(model == null || string.IsNullOrEmpty(model.Orderby) ? " logical_id desc " : model.Orderby)} {(limitNum > 0 ? " limit " + limitNum : "")};";
            var dataList = DapperTool.Query<TCondition, TModel>(sql, model).ToList();

            ModelUtil.AutoFill(dataList, autoFillFieldsList);

            return dataList;
        }

        /// <summary>
        /// 获取所有数据集合(根据条件)
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="limitNum">限制查询条数 0 代表查询全部数据</param>
        /// <returns>所有数据集合</returns>
        public async Task<List<TModel>> GetAllListAsync(TCondition model, int limitNum = 0)
        {
            // 构建查询语句
            string sql = $"SELECT {(model == null || string.IsNullOrEmpty(model.QueryFields) ? GetAllField() : model.QueryFields)} FROM \"{TableName}\" where 1=1 and {GetSqlWhereByModel(model)}  ORDER BY {(model == null || string.IsNullOrEmpty(model.Orderby) ? " logical_id desc " : model.Orderby)} {(limitNum > 0 ? " limit " + limitNum : "")};";
            var dataList = (await DapperTool.QueryAsync<TCondition, TModel>(sql, model)).ToList();

            ModelUtil.AutoFill(dataList, autoFillFieldsList);

            return dataList;
        }

        /// <summary>
        /// 根据model构建查询条件
        /// </summary>
        /// <param name="model">实体model</param>
        /// <returns>条件集合</returns>
        public string GetSqlWhereByModel(TCondition model)
        {
            string sqlWhere = GetSqlWhereByModelFunc(model, "")?.ToString()?.Replace('`', '\"');
            return sqlWhere;
        }

        /// <summary>
        /// 根据model构建查询条件
        /// </summary>
        /// <param name="model">实体model</param>
        /// <returns>条件集合</returns>
        public Func<TCondition, string, StringBuilder> GetSqlWhereByModelFunc { get; set; }

        /// <summary>
        /// 获取表全部字段
        /// </summary>
        /// <returns></returns>
        public string GetAllField(string tableAlias = "")
        {
            tableAlias = string.IsNullOrEmpty(tableAlias) ? "" : tableAlias + ".";
            string allFields = GetAllFieldFunc(tableAlias)?.Replace('`', '\"');
            return $"{tableAlias}\"id\",{allFields}";
        }

        /// <summary>
        /// 获取表全部字段(不包含id)
        /// </summary>
        /// <returns></returns>
        public Func<string, string> GetAllFieldFunc { get; set; }

        /// <summary>
        /// 获取表\的插入语句 
        /// </summary>
        /// <returns>插入语句</returns>
        public override string GetInsertSql()
        {
            // 构建插入语句
            string insertSql = GetInsertSqlFunc()?.Replace('`', '\"');
            return insertSql;
        }

        /// <summary>
        /// 获取表的插入语句 
        /// </summary>
        /// <returns>插入语句</returns>
        public Func<string> GetInsertSqlFunc { get; set; }
        /// <summary>
        /// 获取表的更新语句 
        /// </summary>
        /// <returns>更新语句</returns>
        public string GetUpdateSql()
        {
            // 构建更新语句
            string updateSql = GetUpdateSqlFunc()?.Replace('`', '\"');
            return updateSql;
        }

        /// <summary>
        /// 获取更新sql
        /// </summary>
        /// <returns></returns>
        public Func<string> GetUpdateSqlFunc { get; set; }
    }
}
