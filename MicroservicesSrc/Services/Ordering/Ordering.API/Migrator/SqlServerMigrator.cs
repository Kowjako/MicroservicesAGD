using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API.Migrator
{
    public static class SqlServerMigrator
    {
        public async static Task<WebApplication> MigrateMcrDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContextSeed>>();

            try
            {
                await db.Database.MigrateAsync();
                await OrderContextSeed.SeedAsync(db, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Something went wrong during sql server migration");
            }

            return app;
        }
    }
}
