using System;
using System.Data;
using Discount.API.entities;
using Npgsql;

namespace Discount.API.Repositories
{
	public class DiscountRepository: IDiscountRepository
    {
        private readonly IConfiguration _configuration;
		public DiscountRepository(IConfiguration configuration)
		{
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var dataSource = NpgsqlDataSource.Create(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            await using (var cmd = dataSource.CreateCommand(@"INSERT INTO ""Coupon"" (ProductName, Description, Amount) VALUES($1,$2,$3)"))
            {
                cmd.Parameters.AddWithValue(coupon.ProductName);
                cmd.Parameters.AddWithValue(coupon.Description);
                cmd.Parameters.AddWithValue(coupon.Amount);
                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0;

            };
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var dataSource = NpgsqlDataSource.Create(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            await using (var cmd = dataSource.CreateCommand(@"
               DELETE FROM ""Coupon"" WHERE productname = $1"))
            {
                cmd.Parameters.AddWithValue(productName);
                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 1;

            };
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var dataSource = NpgsqlDataSource.Create(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            await using (var cmd = dataSource.CreateCommand("SELECT * FROM public.\"Coupon\" WHERE ProductName = @productName")) {
                cmd.Parameters.AddWithValue("productName",productName);
                await using var reader = await cmd.ExecuteReaderAsync();
                var hasRow = await reader.ReadAsync();
               
                if (!hasRow)
                {
                    return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
                }

                return new Coupon()
                {
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                    Description = reader.GetString(reader.GetOrdinal("Description"))
                };

            };
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var dataSource = NpgsqlDataSource.Create(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            await using (var cmd = dataSource.CreateCommand(@"
               UPDATE ""Coupon"" SET ProductName=$1,Description=$2,Amount=$3 WHERE id = $4"))
            {
                cmd.Parameters.AddWithValue(coupon.ProductName);
                cmd.Parameters.AddWithValue(coupon.Description);
                cmd.Parameters.AddWithValue(coupon.Amount);
                cmd.Parameters.AddWithValue(coupon.Id);
                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 1;

            };
        }
    }
}

