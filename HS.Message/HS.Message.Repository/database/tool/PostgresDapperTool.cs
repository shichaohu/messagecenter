using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HS.Message.Repository.database.tool
{
    public class PostgresDapperTool<T> : BaseDapperTool<T>
    {
        private readonly IConfiguration _configuration;

        public PostgresDapperTool(IConfiguration configuration)
            : base(DBType.PGSql)
        {
            _configuration = configuration;
        }

        public override IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(GetConnectionString());
        }

        public override string GetConnectionString()
        {
            return _configuration["ConnectionStrings:PostgreSQLConnection"];

        }
    }
}
