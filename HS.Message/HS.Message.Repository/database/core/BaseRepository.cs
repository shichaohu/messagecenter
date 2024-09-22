using HS.Message.Repository.database.tool;
using HS.Message.Share.AutoFill;
using NodaTime;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HS.Message.Repository.database.core
{
    public abstract class BaseRepository<T>
    {
        public List<MAutoFillField> autoFillFieldsList = new List<MAutoFillField>();
        public BaseDapperTool<T> DapperTool { get; set; }

        public abstract string GetInsertSql();

        public int AddOneData(T model)
        {
            return DapperTool.ExecuteNonQuery(GetInsertSql(), model);
        }

        public async Task<int> AddOneDataAsync(T model)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetInsertSql(), model);
        }

        public int AddOne(T model)
        {
            return DapperTool.ExecuteNonQuery(GetInsertSql(), model);
        }
        public async Task<int> AddOneAsync(T model)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetInsertSql(), model);
        }

        public int AddOneData<T2>(T2 model)
        {
            return DapperTool.ExecuteNonQuery(GetInsertSql(), model);
        }
        public async Task<int> AddOneDataAsync<T2>(T2 model)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetInsertSql(), model);
        }

        public int AddOne<T2>(T2 model)
        {
            return DapperTool.ExecuteNonQuery(GetInsertSql(), model);
        }
        public async Task<int> AddOneAsync<T2>(T2 model)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetInsertSql(), model);
        }

        public abstract int AddOneDataRetrunId(T model);

        public abstract int AddOneDataRetrunId<T2>(T2 model);

        public int BactchAddData(List<T> modelList)
        {
            int num = 0;
            foreach (T model in modelList)
            {
                DapperTool.ExecuteNonQuery<T>(GetInsertSql(), model);
                num++;
            }

            return num;
        }
        public async Task<int> BactchAddDataAsync(List<T> modelList)
        {
            int num = 0;
            foreach (T model in modelList)
            {
                await DapperTool.ExecuteNonQueryAsync<T>(GetInsertSql(), model);
                num++;
            }

            return num;
        }

        public int BactchAddData<T2>(List<T2> modelList)
        {
            return DapperTool.ExecuteNonQuery(GetInsertSql(), modelList);
        }
        public async Task<int> BactchAddDataAsync<T2>(List<T2> modelList)
        {
            return await DapperTool.ExecuteNonQueryAsync(GetInsertSql(), modelList);
        }

        public virtual string CreatKey(int type = 1)
        {
            if (type == 1)
            {
                return GetDateRandomString();
            }

            return Guid.NewGuid().ToString();
        }

        private string GetDateRandomString()
        {
            return GetSysDateTimeNow().ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999);
        }

        private int GetRandomSeed()
        {
            byte[] array = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }

        private DateTime GetSysDateTimeNow()
        {
            Instant currentInstant = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone zone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return currentInstant.InZone(zone).ToDateTimeUnspecified();
        }

        public StringBuilder GetfuzzySearchWhere(MBaseModel model, string table = "")
        {
            if (!string.IsNullOrEmpty(model.fuzzySearchFields) && !string.IsNullOrEmpty(model.fuzzySearchKeyWord))
            {
                string[] array = model.fuzzySearchKeyWord.Split(' ');
                string[] array2 = model.fuzzySearchFields.Split(',');
                StringBuilder stringBuilder = new StringBuilder(" and (");
                bool flag = true;
                string[] array3 = array2;
                foreach (string text in array3)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    if (!flag)
                    {
                        stringBuilder.Append(" or(");
                    }
                    else
                    {
                        stringBuilder.Append(" ( ");
                    }

                    flag = false;
                    bool flag2 = true;
                    string[] array4 = array;
                    foreach (string text2 in array4)
                    {
                        if (!string.IsNullOrEmpty(text2))
                        {
                            if (!flag2)
                            {
                                stringBuilder.Append(" and ");
                            }

                            stringBuilder.Append((string.IsNullOrEmpty(table) ? "" : table + ".") + text + " like '%" + text2 + "%'  ");
                            flag2 = false;
                        }
                    }

                    stringBuilder.Append(") ");
                }

                stringBuilder.Append(") ");
                return stringBuilder;
            }

            return null;
        }

        /// <summary>
        /// 格式化数据库名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract string FormattingTableName(string tableName);

        /// <summary>
        /// 格式化in sql 语句
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public abstract string FormattingInSql<T2>(string field, List<T2> fieldValue);
    }
}
