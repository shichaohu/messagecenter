using HS.Message.Share.BaseModel;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace HS.Message.Repository.database.tool
{
    public abstract class BaseDapperTool<T>
    {
        protected DBType dbtype;

        public BaseDapperTool(DBType dbtype = DBType.MySql)
        {
            this.dbtype = dbtype;
        }

        public abstract string GetConnectionString();

        public abstract IDbConnection GetDbConnection();

        public int ExecuteNonQuery(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Execute(SQLString);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteNonQueryAsync(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = await dbConnection.ExecuteAsync(SQLString);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteNonQuery(string SQLString, T tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Execute(SQLString, tmodel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteNonQueryAsync(string SQLString, T tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = await dbConnection.ExecuteAsync(SQLString, tmodel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteNonQuery<T2>(string SQLString, T2 tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            int result = dbConnection.Execute(SQLString, tmodel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteNonQueryAsync<T2>(string SQLString, T2 tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            int result = await dbConnection.ExecuteAsync(SQLString, tmodel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteInsert(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Query<int>(SQLString).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteInsertAsync(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = (await dbConnection.QueryAsync<int>(SQLString)).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteInsert(string SQLString, T tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Query<int>(SQLString, tmodel).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteInsertAsync(string SQLString, T tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = (await dbConnection.QueryAsync<int>(SQLString, tmodel)).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteInsert<T2>(string SQLString, T2 tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Query<int>(SQLString, tmodel).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteInsertAsync<T2>(string SQLString, T2 tmodel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = (await dbConnection.QueryAsync<int>(SQLString, tmodel)).FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public int ExecuteNonQuery(string SQLString, List<T> listModel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = dbConnection.Execute(SQLString, listModel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<int> ExecuteNonQueryAsync(string SQLString, List<T> listModel)
        {
            using IDbConnection dbConnection = GetDbConnection();
            int result = await dbConnection.ExecuteAsync(SQLString, listModel);
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public List<T> Query(string SQLString)
        {
            using IDbConnection cnn = GetDbConnection();
            return cnn.Query<T>(SQLString).ToList();
        }
        public async Task<List<T>> QueryAsync(string SQLString)
        {
            using IDbConnection cnn = GetDbConnection();
            return (await cnn.QueryAsync<T>(SQLString)).ToList();
        }

        public List<TRetrun> Query<TRetrun>(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            List<TRetrun> result = dbConnection.Query<TRetrun>(SQLString).ToList();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<List<TRetrun>> QueryAsync<TRetrun>(string SQLString)
        {
            using IDbConnection dbConnection = GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            List<TRetrun> result = (await dbConnection.QueryAsync<TRetrun>(SQLString)).ToList();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public List<T> Query<TCondtion>(string SQLString, TCondtion model)
        {
            return QueryBase<TCondtion, T>(SQLString, model);
        }
        public async Task<List<T>> QueryAsync<TCondtion>(string SQLString, TCondtion model)
        {
            return await QueryBaseAsync<TCondtion, T>(SQLString, model);
        }

        public List<TRetrun> Query<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            return QueryBase<TCondtion, TRetrun>(SQLString, model);
        }
        public async Task<List<TRetrun>> QueryAsync<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            return await QueryBaseAsync<TCondtion, TRetrun>(SQLString, model);
        }

        public List<T> Query(string SQLString, T model)
        {
            return QueryBase<T, T>(SQLString, model);
        }
        public async Task<List<T>> QueryAsync(string SQLString, T model)
        {
            return await QueryBaseAsync<T, T>(SQLString, model);
        }

        public List<TRetrun> QueryBase<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            using IDbConnection dbConnection = GetDbConnection();
            List<TRetrun> result = dbConnection.Query<TRetrun>(SQLString, model).ToList();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<List<TRetrun>> QueryBaseAsync<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            using IDbConnection dbConnection = GetDbConnection();
            var res = await dbConnection.QueryAsync<TRetrun>(SQLString, model);
            List<TRetrun> result = res?.ToList();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public T QueryOneModel(string SQLString, T model)
        {
            return QueryOneModelBase<T, T>(SQLString, model);
        }
        public async Task<T> QueryOneModelAsync(string SQLString, T model)
        {
            return await QueryOneModelBaseAsync<T, T>(SQLString, model);
        }

        public TRetrun QueryOneModel<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            return QueryOneModelBase<TCondtion, TRetrun>(SQLString, model);
        }
        public async Task<TRetrun> QueryOneModelAsync<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            return await QueryOneModelBaseAsync<TCondtion, TRetrun>(SQLString, model);
        }

        public TRetrun QueryOneModel<TRetrun>(string SQLString, T model)
        {
            return QueryOneModelBase<T, TRetrun>(SQLString, model);
        }
        public async Task<TRetrun> QueryOneModelAsync<TRetrun>(string SQLString, T model)
        {
            return await QueryOneModelBaseAsync<T, TRetrun>(SQLString, model);
        }

        public TRetrun QueryOneModelBase<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            using IDbConnection dbConnection = GetDbConnection();
            TRetrun result = dbConnection.Query<TRetrun>(SQLString, model).SingleOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public async Task<TRetrun> QueryOneModelBaseAsync<TCondtion, TRetrun>(string SQLString, TCondtion model)
        {
            using IDbConnection dbConnection = GetDbConnection();
            TRetrun result = (await dbConnection.QueryAsync<TRetrun>(SQLString, model)).SingleOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public long GetTotal(string sql)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                try
                {
                    using (SqlMapper.GridReader gridReader = dbConnection.QueryMultiple(sql))
                    {
                        result = gridReader.ReadFirst<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
        public async Task<long> GetTotalAsync(string sql)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                try
                {
                    using (SqlMapper.GridReader gridReader = await dbConnection.QueryMultipleAsync(sql))
                    {
                        result = await gridReader.ReadFirstAsync<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        public long GetTotal(string tableName, string where, T model)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = dbConnection.QueryMultiple(sql, model))
                    {
                        result = gridReader.ReadFirst<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
        public async Task<long> GetTotalAsync(string tableName, string where, T model)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = await dbConnection.QueryMultipleAsync(sql, model))
                    {
                        result = await gridReader.ReadFirstAsync<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        public long GetTotal<TCondtion>(string tableName, string where, TCondtion model)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = dbConnection.QueryMultiple(sql, model))
                    {
                        result = gridReader.ReadFirst<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
        public async Task<long> GetTotalAsync<TCondtion>(string tableName, string where, TCondtion model)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = await dbConnection.QueryMultipleAsync(sql, model))
                    {
                        result = await gridReader.ReadFirstAsync<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        public long GetTotal(string tableName, string where)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = dbConnection.QueryMultiple(sql))
                    {
                        result = gridReader.ReadFirst<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
        public async Task<long> GetTotalAsync(string tableName, string where)
        {
            long result = 0L;
            using (IDbConnection dbConnection = GetDbConnection())
            {
                string sql = $"select count(1) from {tableName} where {where} ";
                try
                {
                    using (SqlMapper.GridReader gridReader = await dbConnection.QueryMultipleAsync(sql))
                    {
                        result = await gridReader.ReadFirstAsync<long>();
                    }

                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
        
        public List<T> GetPageList(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return GetPageListBase<T, T>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }
        public async Task<List<T>> GetPageListAsync(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<T, T>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }

        public List<T> GetPageList(string files, string tableName, string where, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return GetPageListBase<T, T>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }
        public async Task<List<T>> GetPageListAsync(string files, string tableName, string where, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<T, T>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }

        public List<TRetrun> GetPageList<TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return GetPageListBase<T, TRetrun>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }
        public async Task<List<TRetrun>> GetPageListAsync<TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<T, TRetrun>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }

        public List<TRetrun> GetPageList<TRetrun>(string files, string tableName, string where, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return GetPageListBase<T, TRetrun>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }
        public async Task<List<TRetrun>> GetPageListAsync<TRetrun>(string files, string tableName, string where, MPageInfo mPageInfor, T model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<T, TRetrun>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }

        public List<T> GetPageList<TCondtion>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return GetPageListBase<TCondtion, T>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }
        public async Task<List<T>> GetPageListAsync<TCondtion>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<TCondtion, T>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }

        public List<T> GetPageList<TCondtion>(string files, string tableName, string where, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return GetPageListBase<TCondtion, T>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }
        public async Task<List<T>> GetPageListAsync<TCondtion>(string files, string tableName, string where, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<TCondtion, T>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }

        public List<TRetrun> GetPageList<TCondtion, TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return GetPageListBase<TCondtion, TRetrun>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }
        public async Task<List<TRetrun>> GetPageListAsync<TCondtion, TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<TCondtion, TRetrun>(files, tableName, where, count_files, mPageInfor, model, returnResult);
        }

        public List<TRetrun> GetPageList<TCondtion, TRetrun>(string files, string tableName, string where, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return GetPageListBase<TCondtion, TRetrun>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }
        public async Task<List<TRetrun>> GetPageListAsync<TCondtion, TRetrun>(string files, string tableName, string where, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            return await GetPageListBaseAsync<TCondtion, TRetrun>(files, tableName, where, string.Empty, mPageInfor, model, returnResult);
        }

        public List<TRetrun> GetPageListBase<TCondtion, TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            using IDbConnection dbConnection = GetDbConnection();
            try
            {
                int num = 1;
                if (mPageInfor.pageIndex > 0)
                {
                    num = (mPageInfor.pageIndex - 1) * mPageInfor.pageSize;
                }

                StringBuilder stringBuilder = new StringBuilder();
                count_files = !files.ToLower().Contains("distinct ") ? "1" : string.IsNullOrEmpty(count_files) ? files : "distinct " + count_files;
                string sql = "SELECT COUNT(" + count_files + ") FROM " + tableName + " where " + where;
                using (SqlMapper.GridReader gridReader = model == null ? dbConnection.QueryMultiple(sql) : dbConnection.QueryMultiple(sql, model))
                {
                    mPageInfor.total = gridReader.ReadFirst<int>();
                    if (mPageInfor.total < 1)
                    {
                        return null;
                    }
                }

                stringBuilder = new StringBuilder();
                if (dbtype == DBType.Oracle)
                {
                    if (string.IsNullOrEmpty(mPageInfor.orderby))
                    {
                        stringBuilder.AppendFormat(" select {6} from\r\n                                        (select rownum r,{0}\r\n                                            from {1} where  {2} and rownum <= {4} {3} ) t \r\n                                       where r >{5}", files, tableName, where, "", mPageInfor.pageIndex * mPageInfor.pageSize, (mPageInfor.pageIndex - 1) * mPageInfor.pageSize, files.Contains(".") ? "t.*" : files);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("select {6} from (select rownum r,{0}\r\n                                            from {1} where  {2}  ORDER BY {3} ) t where r <= {4} and r > {5} ", files, tableName, where, mPageInfor.orderby, mPageInfor.pageIndex * mPageInfor.pageSize, (mPageInfor.pageIndex - 1) * mPageInfor.pageSize, files.Contains(".") ? "t.*" : files, files.Contains(".") ? "t2.*" : files);
                    }
                }
                else if (dbtype == DBType.PGSql)
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("SELECT  {0} from {1}\r\n                                WHERE  {2}\r\n                                {3} LIMIT {4} OFFSET {5}", files, tableName, where, string.IsNullOrEmpty(mPageInfor.orderby) ? "" : "ORDER BY " + mPageInfor.orderby, mPageInfor.pageSize, num);
                }
                else
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("SELECT  {0} from {1}\r\n                                WHERE  {2}\r\n                                {3} limit {4}, {5}", files, tableName, where, string.IsNullOrEmpty(mPageInfor.orderby) ? "" : "ORDER BY " + mPageInfor.orderby, num, mPageInfor.pageSize);
                }

                using SqlMapper.GridReader gridReader2 = model == null ? dbConnection.QueryMultiple(stringBuilder.ToString()) : dbConnection.QueryMultiple(stringBuilder.ToString(), model);
                IEnumerable<TRetrun> enumerable = gridReader2.Read<TRetrun>();
                if (enumerable != null)
                {
                    return new List<TRetrun>(enumerable);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch
                {
                }
            }
        }
        public async Task<List<TRetrun>> GetPageListBaseAsync<TCondtion, TRetrun>(string files, string tableName, string where, string count_files, MPageInfo mPageInfor, TCondtion model, BaseResponse returnResult = null)
        {
            using IDbConnection dbConnection = GetDbConnection();
            try
            {
                int num = 1;
                if (mPageInfor.pageIndex > 0)
                {
                    num = (mPageInfor.pageIndex - 1) * mPageInfor.pageSize;
                }

                StringBuilder stringBuilder = new StringBuilder();
                count_files = !files.ToLower().Contains("distinct ") ? "1" : string.IsNullOrEmpty(count_files) ? files : "distinct " + count_files;
                string sql = "SELECT COUNT(" + count_files + ") FROM " + tableName + " where " + where;
                using (SqlMapper.GridReader gridReader = model == null ? await dbConnection.QueryMultipleAsync(sql) : await dbConnection.QueryMultipleAsync(sql, model))
                {
                    mPageInfor.total = await gridReader.ReadFirstAsync<int>();
                    if (mPageInfor.total < 1)
                    {
                        return null;
                    }
                }

                stringBuilder = new StringBuilder();
                if (dbtype == DBType.Oracle)
                {
                    if (string.IsNullOrEmpty(mPageInfor.orderby))
                    {
                        stringBuilder.AppendFormat(" select {6} from\r\n                                        (select rownum r,{0}\r\n                                            from {1} where  {2} and rownum <= {4} {3} ) t \r\n                                       where r >{5}", files, tableName, where, "", mPageInfor.pageIndex * mPageInfor.pageSize, (mPageInfor.pageIndex - 1) * mPageInfor.pageSize, files.Contains(".") ? "t.*" : files);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("select {6} from (select rownum r,{0}\r\n                                            from {1} where  {2}  ORDER BY {3} ) t where r <= {4} and r > {5} ", files, tableName, where, mPageInfor.orderby, mPageInfor.pageIndex * mPageInfor.pageSize, (mPageInfor.pageIndex - 1) * mPageInfor.pageSize, files.Contains(".") ? "t.*" : files, files.Contains(".") ? "t2.*" : files);
                    }
                }
                else if (dbtype == DBType.PGSql)
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("SELECT  {0} from {1}\r\n                                WHERE  {2}\r\n                                {3} LIMIT {4} OFFSET {5}", files, tableName, where, string.IsNullOrEmpty(mPageInfor.orderby) ? "" : "ORDER BY " + mPageInfor.orderby, mPageInfor.pageSize, num);
                }
                else
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("SELECT  {0} from {1}\r\n                                WHERE  {2}\r\n                                {3} limit {4}, {5}", files, tableName, where, string.IsNullOrEmpty(mPageInfor.orderby) ? "" : "ORDER BY " + mPageInfor.orderby, num, mPageInfor.pageSize);
                }

                using SqlMapper.GridReader gridReader2 = model == null ? await dbConnection.QueryMultipleAsync(stringBuilder.ToString()) : await dbConnection.QueryMultipleAsync(stringBuilder.ToString(), model);
                IEnumerable<TRetrun> enumerable = await gridReader2.ReadAsync<TRetrun>();
                if (enumerable != null)
                {
                    return new List<TRetrun>(enumerable);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                    if (dbtype == DBType.Oracle)
                    {
                        OracleConnection.ClearPool((OracleConnection)dbConnection);
                    }
                }
                catch
                {
                }
            }
        }

        public T GetModel(string files, string tableName, string where, T model, string orderBy = "")
        {
            return GetModelBase<T, T>(files, tableName, where, model, orderBy);
        }
        public async Task<T> GetModelAsync(string files, string tableName, string where, T model, string orderBy = "")
        {
            return await GetModelBaseAsync<T, T>(files, tableName, where, model, orderBy);
        }

        public TRetrun GetModel<TRetrun>(string files, string tableName, string where, T model, string orderBy = "")
        {
            return GetModelBase<T, TRetrun>(files, tableName, where, model, orderBy);
        }
        public async Task<TRetrun> GetModelAsync<TRetrun>(string files, string tableName, string where, T model, string orderBy = "")
        {
            return await GetModelBaseAsync<T, TRetrun>(files, tableName, where, model, orderBy);
        }

        public TRetrun GetModel<TCondtion, TRetrun>(string files, string tableName, string where, TCondtion model, string orderBy = "")
        {
            return GetModelBase<TCondtion, TRetrun>(files, tableName, where, model, orderBy);
        }
        public async Task<TRetrun> GetModelAsync<TCondtion, TRetrun>(string files, string tableName, string where, TCondtion model, string orderBy = "")
        {
            return await GetModelBaseAsync<TCondtion, TRetrun>(files, tableName, where, model, orderBy);
        }

        private TRetrun GetModelBase<TCondtion, TRetrun>(string files, string tableName, string where, TCondtion model, string orderBy = "")
        {
            using IDbConnection dbConnection = GetDbConnection();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT  {0} from {1} \r\n                                WHERE  {2} {3}\r\n                               ", files, tableName, where, string.IsNullOrEmpty(orderBy) ? string.Empty : "order by " + orderBy);

            string sql = stringBuilder.ToString();
            TRetrun result = dbConnection.Query<TRetrun>(sql, model).ToList().FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch
            {
            }

            return result;
        }

        private async Task<TRetrun> GetModelBaseAsync<TCondtion, TRetrun>(string files, string tableName, string where, TCondtion model, string orderBy = "")
        {
            using IDbConnection dbConnection = GetDbConnection();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT  {0} from {1} \r\n                                WHERE  {2} {3}\r\n                               ", files, tableName, where, string.IsNullOrEmpty(orderBy) ? string.Empty : "order by " + orderBy);

            string sql = stringBuilder.ToString();
            var res = await dbConnection.QueryAsync<TRetrun>(sql, model);
            TRetrun result = res.ToList().FirstOrDefault();
            try
            {
                dbConnection.Close();
                dbConnection.Dispose();
                if (dbtype == DBType.Oracle)
                {
                    OracleConnection.ClearPool((OracleConnection)dbConnection);
                }
            }
            catch
            {
            }

            return result;
        }
    }

}
