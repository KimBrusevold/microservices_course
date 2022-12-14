using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.API.Entities;
using System.Threading.Tasks;
using Npgsql;
using Dapper;

namespace Discount.API.Repositories;
public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connString;
    private readonly ILogger<DiscountRepository> _logger;

    public DiscountRepository(IConfiguration configuration, ILogger<DiscountRepository> logger)
    {
        _configuration = configuration;
        _connString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        _logger = logger;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        _logger.LogInformation("Started getting coupon for {ProductName}", productName);
        await using var connection = new NpgsqlConnection(_connString);
        await connection.OpenAsync();

        var sql = "SELECT * FROM Coupon WHERE ProductName LIKE @ProductName";
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            sql,
            new { ProductName = productName });

        return coupon;
    }
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_connString);
        await connection.OpenAsync();

        var rowsAffected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
            new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount}
        );

        return rowsAffected >= 1;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {

        await using var connection = new NpgsqlConnection(_connString);
        await connection.OpenAsync();

        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName }
        );

        return rowsAffected >= 1;
    }


    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_connString);
        await connection.OpenAsync();
        
        var sql = "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id}
        );

        return rowsAffected >= 1;
    }
}
