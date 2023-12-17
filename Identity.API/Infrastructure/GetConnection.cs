﻿using System.Data;
using System.Data.SqlClient;

namespace Identity.API.Infrastructure
{
    public static class GetConnection
    {
        public static IDbConnection GetConnectionDB(string ConnectionString)
        {
            IDbConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
