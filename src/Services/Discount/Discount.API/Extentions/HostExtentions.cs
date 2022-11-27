using Npgsql;

namespace Discount.API.Extentions;

public static class WebAppExtentions
{
    public static void MigrateDatabase<TContext>(this WebApplication app, int? retry = 0)
    {
        var retryForAvailability = retry!.Value;

        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Migrating postgresql database");

            var connString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var connection = new NpgsqlConnection(connString);
            connection.Open();

            var sql = "DROP TABLE IF EXISTS Coupon";
            using var command = new NpgsqlCommand(sql, connection);
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(id SERIAL PRIMARY KEY,
                                                        ProductName VARCHAR(24) NOT NULL,
                                                        Description TEXT,
                                                        Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();

            logger.LogInformation("Migrated postresql database.");
        
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "An error occurred while migrating the postgresql database");

            if(retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(app, retryForAvailability);
            }
        }
    }
}