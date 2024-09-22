using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HS.Message.Repository.database.tool
{
    public class MySqlDapperTool<T> : BaseDapperTool<T>
    {
        private readonly IConfiguration _configuration;

        public MySqlDapperTool(IConfiguration configuration)
            : base(DBType.MySql)
        {
            _configuration = configuration;
        }

        public override IDbConnection GetDbConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }

        public override string GetConnectionString()
        {
            return _configuration["ConnectionStrings:MySqlConnection"];

        }
    }
}
