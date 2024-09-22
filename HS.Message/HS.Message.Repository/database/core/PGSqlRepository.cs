using HS.Message.Repository.database.tool;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;

namespace HS.Message.Repository.database.core
{
    public abstract class PGSqlRepository<T> : BaseRepository<T>
    {
        public PGSqlRepository(IConfiguration configuration)
        {
            DapperTool = new PostgresDapperTool<T>(configuration);
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
                return $"\"{tableName}\" "; ;
            }
        }

        /// <summary>
        /// 使用Dapper时，PostgreSQL IN关键字不支持把数组作为参数，可以使用any关键字进行此项操作
        /// </summary>
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
                //var inSql = $" {field} = any('{string.Join("','", fieldValue)}') ";
                var inSql = $" {field} in ('{string.Join("','", fieldValue)}') ";
                return inSql;
            }
        }
    }
}
