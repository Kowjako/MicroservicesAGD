using Npgsql;

namespace Discount.API.Extensions
{
    public static class PgSqlMigrator
    {
        public async static Task<WebApplication> MigratePgDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Start Pg database migration");
                using var connection = new NpgsqlConnection(config["DatabaseSettings:ConnectionString"]);

                await connection.OpenAsync();

                using var cmd = new NpgsqlCommand { Connection = connection };
                cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY,
                                                         ProductName VARCHAR(24) NOT NULL,
                                                         Description TEXT,
                                                         Amount INT)";
                await cmd.ExecuteNonQueryAsync();
                logger.LogInformation("Database migrated");

                cmd.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) 
                                    VALUES ('IPhone X', 'IPhone discount', 150),
                                           ('Samsung 10', 'Samsung discount', 100)";
                await cmd.ExecuteNonQueryAsync();
            }
            catch (NpgsqlException ex)
            {
                logger.LogError("Error during migration", ex);
            }

            return app;
        }
    }
}
