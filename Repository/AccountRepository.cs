using Dapper;
using FC_Application.Models;
using System.Data.SqlClient;

namespace FC_Application.Repository
{
    public class AccountRepository 
    {
        private readonly string _connectionString;
        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
            SELECT TOP(1) UserID, UserName, Password
            FROM Users
            WHERE UserName = @UserName";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserName = userName });
        }
    }
}
