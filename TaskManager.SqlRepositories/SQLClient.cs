﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.SqlRepositories
{
    public class SQLClient
    {
        private string _connectionString;

        public SQLClient(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<T> GetSingle<T>(string storedProcedureName, object parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                return await conn.QuerySingleAsync<T>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure, param: parameters);
            }
        }

        public async Task<IEnumerable<TReturn>> GetComplex<T1, T2, TReturn>(string storedProcedureName, Func<T1, T2, TReturn> map)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var data = conn.Query<T1, T2, TReturn>(storedProcedureName, map);

                return data;

            }
        }
    }
}
