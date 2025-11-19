using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeMvcAdo.Data
{
    public class DbHelper
    {
        private readonly string _connString;
        public DbHelper(IConfiguration config)
        {
            _connString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> ExecuteDataTableAsync(string sql, params SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            await conn.OpenAsync();
            da.Fill(dt);
            return dt;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object?> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }

        // Utility: Read via reader with projection
        public async Task<List<T>> ExecuteReaderAsync<T>(string sql, Func<SqlDataReader, T> projector, params SqlParameter[] parameters)
        {
            var list = new List<T>();
            using var conn = new SqlConnection(_connString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(projector(reader));
            }
            return list;
        }
    }
}
