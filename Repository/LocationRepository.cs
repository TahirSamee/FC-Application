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
    }
}
