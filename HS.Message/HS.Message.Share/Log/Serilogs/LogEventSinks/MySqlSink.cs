using HS.Message.Share.Log.Serilogs.Utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System.Text;

namespace HS.Message.Share.Log.Serilogs.LogEventSinks
{
    internal class MySqlSink : BatchProvider, ILogEventSink
    {
        private readonly string _connectionString;

        private readonly bool _storeTimestampInUtc;

        private readonly string _tableName;

        private static string lastPracticalTableName = string.Empty;

        public MySqlSink(string connectionString, string tableName = "Logs", bool storeTimestampInUtc = false, uint batchSize = 100u)
            : base((int)batchSize)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _storeTimestampInUtc = storeTimestampInUtc;
            MySqlConnection sqlConnection = GetSqlConnection();

            CreateTable(sqlConnection, _tableName);
        }

        public void Emit(LogEvent logEvent)
        {
            PushEvent(logEvent);
        }

        private MySqlConnection GetSqlConnection()
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
                return null;
            }
        }

        private MySqlCommand GetInsertCommand(MySqlConnection sqlConnection)
        {
            string tableName = SerilogsLogUtils.GetPracticalTableName(DateTime.Now, _tableName);
            if (tableName != lastPracticalTableName)
            {
                CreateTable(sqlConnection, tableName);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO  " + tableName + " (");
            stringBuilder.Append("HttpHost, HttpRemoteAddress,HttpXForwardedFor,HttpPath, HttpRequestId, SourceContext, Timestamp, Level, Template, Message, Exception, Properties) ");
            stringBuilder.Append("VALUES (@httpHost,@httpRemoteAddress,@httpXForwardedFor,@httpPath,@httpRequestId,@sourceContext,@ts, @level,@template, @msg, @ex, @prop)");
            MySqlCommand mySqlCommand = sqlConnection.CreateCommand();
            mySqlCommand.CommandText = stringBuilder.ToString();

            #region 自定义属性
            mySqlCommand.Parameters.Add(new MySqlParameter("@httpHost", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@httpRemoteAddress", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@httpXForwardedFor", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@httpPath", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@httpRequestId", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@sourceContext", MySqlDbType.VarChar));
            #endregion

            mySqlCommand.Parameters.Add(new MySqlParameter("@ts", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@level", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@template", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@msg", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@ex", MySqlDbType.VarChar));
            mySqlCommand.Parameters.Add(new MySqlParameter("@prop", MySqlDbType.VarChar));
            return mySqlCommand;
        }
        private void CreateTable(MySqlConnection sqlConnection, string tableName)
        {
            try
            {
                tableName = SerilogsLogUtils.GetPracticalTableName(DateTime.Now, tableName);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("CREATE TABLE IF NOT EXISTS " + tableName + " (");
                stringBuilder.Append("id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,");

                #region 自定义属性
                stringBuilder.Append("HttpHost VARCHAR(100),");
                stringBuilder.Append("HttpRemoteAddress VARCHAR(100),");
                stringBuilder.Append("HttpXForwardedFor VARCHAR(200),");
                stringBuilder.Append("HttpPath VARCHAR(100),");
                stringBuilder.Append("HttpRequestId VARCHAR(50),");
                stringBuilder.Append("SourceContext VARCHAR(100),");
                #endregion

                stringBuilder.Append("Timestamp DATETIME,");
                stringBuilder.Append("Level VARCHAR(15),");
                stringBuilder.Append("Template TEXT,");
                stringBuilder.Append("Message TEXT,");
                stringBuilder.Append("Exception TEXT,");
                stringBuilder.Append("Properties TEXT,");
                stringBuilder.Append("_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP)");
                MySqlCommand mySqlCommand = sqlConnection.CreateCommand();
                mySqlCommand.CommandText = stringBuilder.ToString();
                mySqlCommand.ExecuteNonQuery();

                lastPracticalTableName = tableName;
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
            }
        }

        protected override async Task<bool> WriteLogEventAsync(ICollection<LogEvent> logEventsBatch)
        {
            try
            {
                return await WriteLogAsync(logEventsBatch);
            }
            catch
            {
                try
                {
                    MySqlConnection sqlConnection = GetSqlConnection();
                    CreateTable(sqlConnection, _tableName);
                    return await WriteLogAsync(logEventsBatch);
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        private async Task<bool> WriteLogAsync(ICollection<LogEvent> logEventsBatch)
        {
            using MySqlConnection sqlCon = GetSqlConnection();
            using MySqlTransaction tr = await sqlCon.BeginTransactionAsync().ConfigureAwait(continueOnCapturedContext: false);
            MySqlCommand insertCommand = GetInsertCommand(sqlCon);
            insertCommand.Transaction = tr;

            foreach (LogEvent item in logEventsBatch)
            {
                string GetItemPropertiesString(string name)
                {
                    string propString = item.Properties[name]?.ToString()?.TrimStart('"')?.TrimEnd('"');
                    return propString;
                };
                StringWriter stringWriter = new StringWriter(new StringBuilder());
                item.RenderMessage(stringWriter);

                #region 自定义属性 @host,@httpRequestId,@sourceContext
                insertCommand.Parameters["@httpHost"].Value = GetItemPropertiesString("HttpHost");
                insertCommand.Parameters["@httpRemoteAddress"].Value = GetItemPropertiesString("HttpRemoteAddress");
                insertCommand.Parameters["@httpXForwardedFor"].Value = GetItemPropertiesString("HttpXForwardedFor");
                insertCommand.Parameters["@httpPath"].Value = GetItemPropertiesString("HttpPath");
                insertCommand.Parameters["@httpRequestId"].Value = GetItemPropertiesString("HttpRequestId");
                insertCommand.Parameters["@sourceContext"].Value = GetItemPropertiesString("SourceContext");
                #endregion

                insertCommand.Parameters["@ts"].Value = (_storeTimestampInUtc ? item.Timestamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fffzzz") : item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
                insertCommand.Parameters["@level"].Value = item.Level.ToString();
                insertCommand.Parameters["@template"].Value = item.MessageTemplate.ToString();
                insertCommand.Parameters["@msg"].Value = stringWriter;
                insertCommand.Parameters["@ex"].Value = item.Exception?.ToString();
                insertCommand.Parameters["@prop"].Value = ((item.Properties.Count > 0) ? JsonConvert.SerializeObject(item.Properties) : string.Empty);
                int count = await insertCommand.ExecuteNonQueryAsync().ConfigureAwait(continueOnCapturedContext: false);

            }

            tr.Commit();
            return true;
        }
    }
}
