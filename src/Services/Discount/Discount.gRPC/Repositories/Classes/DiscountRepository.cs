using Dapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.gRPC.Repositories.Classes
{
    public class DiscountRepository : IDiscountRepository, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscountRepository> _logger;
        private readonly NpgsqlConnection npgsqlConnection;

        public DiscountRepository(IConfiguration configuration, ILogger<DiscountRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            try
            {
                npgsqlConnection = new NpgsqlConnection(
                    _configuration.GetSection("DiscountDb").GetValue<string>("ConnectionString")
                );
                _logger.LogInformation("Connection established to DiscountDb");
            }
            catch (Exception e) {
                _logger.LogError("Connection failed to DiscountDb");
            }
        }

        // Fetches a discouunt coupon
        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await npgsqlConnection.QueryFirstOrDefaultAsync<Coupon>(
               "SELECT * FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName}
            );

            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No discount available" };

            return coupon;
        }

        // Creates a new discount coupon
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await npgsqlConnection.ExecuteAsync(
                "INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount}
            );

            if (affected == 0)
                return false;

            return true;
        }

        // Update an existing discount coupon
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await npgsqlConnection.ExecuteAsync(
                "UPDATE Coupons SET ProductName = @ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id }
            );

            if (affected == 0)
                return false;

            return true;
        }

        // Delete an existing coupon
        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await npgsqlConnection.ExecuteAsync(
               "DELETE FROM Coupons WHERE ProductName=@ProductName",
               new { ProductName = productName }
            );

            if (affected == 0)
                return false;

            return true;
        }

        // Object disposal function
        public void Dispose()
        {
            try
            {
                npgsqlConnection.Close();
                _logger.LogInformation("Connection to DiscountDb revoked");
            }
            catch (Exception e)
            {
                _logger.LogTrace("Unable to close connection", e);
            }

        }

    }
}
