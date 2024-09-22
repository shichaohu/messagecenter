using HS.Message.Repository.database.tool;
using Microsoft.Extensions.Configuration;

namespace HS.Message.Repository.database.core
{
    public abstract class MysqlRepository<T> : BaseRepository<T>
    {
        public MysqlRepository(IConfiguration configuration)
        {
            DapperTool = new MySqlDapperTool<T>(configuration);
        }

        public override int AddOneDataRetrunId(T model)
        {
            string text = GetInsertSql();
            if (!text.ToUpper().Contains("LAST_INSERT_ID()"))
            {
                text += ";SELECT LAST_INSERT_ID();";
            }

            return DapperTool.ExecuteInsert(text, model);
        }

        public override int AddOneDataRetrunId<T2>(T2 model)
        {
            string text = GetInsertSql();
            if (!text.ToUpper().Contains("LAST_INSERT_ID()"))
            {
                text += ";SELECT LAST_INSERT_ID();";
            }

            return DapperTool.ExecuteInsert(text, model);
        }

        /// <summary>
        /// 格式化数据库名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override string FormattingTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) return tableName;
            else
            {
                return $"`{tableName}`"; ;
            }
        }
        /// <summary>
        /// 格式化in sql 语句
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public override string FormattingInSql<T2>(string field, List<T2> fieldValue)
        {
            if (string.IsNullOrWhiteSpace(field) || fieldValue?.Count == 0)
            {
                return null;
            }
            else
            {
                var inSql = $" {field} in ('{string.Join("','", fieldValue)}') ";
                return inSql;
            }
        }
    }
}
