using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuraiton)
            => _configuration = configuraiton;

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);

            var sql = "INSERT INTO Coupon (ProductName, Description, Amount) " +
                      "VALUES (@ProductName, @Description, @Amount)";

            return await connection.ExecuteAsync(sql, coupon) > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);

            var sql = "DELETE FROM Coupon WHERE ProductName = @ProductName";

            return await connection.ExecuteAsync(sql, productName) > 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);

            var sql = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, productName);
            if (coupon == null)
            {
                return new Coupon()
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);

            var sql = "UPDATE Coupon SET ProductName = @ProductName, " +
                      "Description = @Description, Amount = @Amount " +
                      "WHERE Id = @Id";

            return await connection.ExecuteAsync(sql, coupon) > 0;
        }
    }
}
