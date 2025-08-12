using Dapper;
using FC_Application.Models;
using System.Data.SqlClient;

namespace FC_Application.Repository
{
    public class FinanceRepository
    {
        private readonly string _connectionString;

        public FinanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<bool> AddFinanceAsync(Finance finance)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            INSERT INTO Finance (
                SONumber, Client, Customer, DateReceived, DateDue, DateSubmitted, 
                ExpirationDate, POCName, POCEmail, POCPhone, Status, ServiceType, 
                UnitQuantity, ProposalTotal
            )
            VALUES (
                @SONumber, @Client, @Customer, @DateReceived, @DateDue, @DateSubmitted, 
                @ExpirationDate, @POCName, @POCEmail, @POCPhone, @Status, @ServiceType, 
                @UnitQuantity, @ProposalTotal
            );";

                var rows = await connection.ExecuteAsync(sql, finance);
                return rows > 0;
            }
        }

        public async Task<IEnumerable<Finance>> GetPagedFinancesAsync(string search, int page, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT * FROM Finance
            WHERE (@Search = '' OR Client LIKE '%' + @Search + '%' OR SONumber LIKE '%' + @Search + '%')
            ORDER BY SrNo
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";

                return await connection.QueryAsync<Finance>(sql, new
                {
                    Search = search ?? "",
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });
            }
        }

        public async Task<int> GetTotalCountAsync(string search)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT COUNT(*) FROM Finance
            WHERE (@Search = '' OR Client LIKE '%' + @Search + '%' OR SONumber LIKE '%' + @Search + '%');
            ";

                return await connection.ExecuteScalarAsync<int>(sql, new { Search = search ?? "" });
            }
        }

        public async Task<Finance?> GetFinanceByIdAsync(int srNo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Finance WHERE SrNo = @SrNo";
                return await connection.QueryFirstOrDefaultAsync<Finance>(sql, new { SrNo = srNo });
            }
        }

        public async Task<bool> UpdateFinanceAsync(Finance finance)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            UPDATE Finance SET
                SONumber = @SONumber,
                Client = @Client,
                Customer = @Customer,
                DateReceived = @DateReceived,
                DateDue = @DateDue,
                DateSubmitted = @DateSubmitted,
                ExpirationDate = @ExpirationDate,
                POCName = @POCName,
                POCEmail = @POCEmail,
                POCPhone = @POCPhone,
                Status = @Status,
                ServiceType = @ServiceType,
                UnitQuantity = @UnitQuantity,
                ProposalTotal = @ProposalTotal
            WHERE SrNo = @SrNo;
            ";

                var rows = await connection.ExecuteAsync(sql, finance);
                return rows > 0;
            }
        }

        public async Task<bool> DeleteFinanceAsync(int srNo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Finance WHERE SrNo = @SrNo";
                var rows = await connection.ExecuteAsync(sql, new { SrNo = srNo });
                return rows > 0;
            }
        }
        public async Task<int> GetTotalFinanceCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Finance";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
        public async Task<int> GetPendingFinanceCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Finance WHERE Status = 'Pending'";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
        public async Task<IEnumerable<Finance>> GetPendingFinanceAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @" SELECT  * FROM Finance WHERE Status = 'Pending'";

                return await connection.QueryAsync<Finance>(sql);
            }
        }
    }
}
