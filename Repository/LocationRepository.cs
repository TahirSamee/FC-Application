using Dapper;
using FC_Application.Models;
using System.Data.SqlClient;

namespace FC_Application.Repository
{
    public class LocationRepository
    {
        private readonly string _connectionString;

        public LocationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> AddLocationAsync(Location location)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                INSERT INTO Location (
                    SurveyorName, LocationID, SalesOrderID, ClientLocationIdentifier, Status,
                    ServiceDueDate, ServiceDate, Client, Customer, BrandName, LocationNumber,
                    LocationNickname, Service, Address, City, State, Zip, PhoneNumber, Email,
                    ManagerName, L1ManagerName, L1ManagerEmail, L1ManagerPhone,
                    L2ManagerName, L2ManagerEmail, L2ManagerPhone,
                    AssetsVerified, AssetCount, SqFt, Value, Notes, Verifier, DateVerified
                )
                VALUES (
                    @SurveyorName, @LocationID, @SalesOrderID, @ClientLocationIdentifier, @Status,
                    @ServiceDueDate, @ServiceDate, @Client, @Customer, @BrandName, @LocationNumber,
                    @LocationNickname, @Service, @Address, @City, @State, @Zip, @PhoneNumber, @Email,
                    @ManagerName, @L1ManagerName, @L1ManagerEmail, @L1ManagerPhone,
                    @L2ManagerName, @L2ManagerEmail, @L2ManagerPhone,
                    @AssetsVerified, @AssetCount, @SqFt, @Value, @Notes, @Verifier, @DateVerified
                );
            ";

                var rowsAffected = await connection.ExecuteAsync(sql, location);
                return rowsAffected > 0;
            }
        }


        public async Task<IEnumerable<Location>> GetPagedLocationsAsync(string search, int page, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT * FROM Location
            WHERE (@Search = '' OR Client LIKE '%' + @Search + '%' OR LocationID LIKE '%' + @Search + '%')
            ORDER BY SrNo
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

                return await connection.QueryAsync<Location>(sql, new
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
            SELECT COUNT(*) FROM Location
            WHERE (@Search = '' OR Client LIKE '%' + @Search + '%' OR LocationID LIKE '%' + @Search + '%');
        ";

                return await connection.ExecuteScalarAsync<int>(sql, new { Search = search ?? "" });
            }
        }

        public async Task<Location?> GetLocationByIdAsync(int SrNo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Location WHERE SrNo = @SrNo";
                return await connection.QueryFirstOrDefaultAsync<Location>(sql, new { SrNo = SrNo });
            }
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
        UPDATE Location SET
            SurveyorName = @SurveyorName,
            SalesOrderID = @SalesOrderID,
            ClientLocationIdentifier = @ClientLocationIdentifier,
            Status = @Status,
            ServiceDueDate = @ServiceDueDate,
            ServiceDate = @ServiceDate,
            Client = @Client,
            Customer = @Customer,
            BrandName = @BrandName,
            LocationNumber = @LocationNumber,
            LocationNickname = @LocationNickname,
            Service = @Service,
            Address = @Address,
            City = @City,
            State = @State,
            Zip = @Zip,
            PhoneNumber = @PhoneNumber,
            Email = @Email,
            ManagerName = @ManagerName,
            L1ManagerName = @L1ManagerName,
            L1ManagerEmail = @L1ManagerEmail,
            L1ManagerPhone = @L1ManagerPhone,
            L2ManagerName = @L2ManagerName,
            L2ManagerEmail = @L2ManagerEmail,
            L2ManagerPhone = @L2ManagerPhone,
            AssetsVerified = @AssetsVerified,
            AssetCount = @AssetCount,
            SqFt = @SqFt,
            Value = @Value,
            Notes = @Notes,
            Verifier = @Verifier,
            DateVerified = @DateVerified
        WHERE SrNo = @SrNo;
        ";

                var rows = await connection.ExecuteAsync(sql, location);
                return rows > 0;
            }
        }

        public async Task<bool> DeleteLocationAsync(int SrNo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Location WHERE SrNo = @SrNo";
                var rows = await connection.ExecuteAsync(sql, new { SrNo = SrNo });
                return rows > 0;
            }
        }
        public async Task<int> GetTotalLocationCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Location";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
        public async Task<int> GetPendingLocationCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Location WHERE Status = 'Pending Schedule'";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<IEnumerable<Location>> GetPendingLocationsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @" SELECT  * FROM Location WHERE Status = 'Pending Schedule'";

                return await connection.QueryAsync<Location>(sql);
            }
        }
    }
}
