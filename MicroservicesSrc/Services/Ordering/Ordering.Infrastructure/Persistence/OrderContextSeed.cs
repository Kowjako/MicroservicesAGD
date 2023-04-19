using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Orders database was seeded with sample data!");
            }
        }

        private static IList<Order> GetOrders()
        {
            return new List<Order>()
            {
                new Order
                {
                    UserName = "admin",
                    FirstName = "Wlodzmierz",
                    LastName = "Kowjako",
                    EmailAddress = "kowyako@sample-mail.com",
                    AddressLine = "Wroclaw",
                    Country = "Poland",
                    TotalPrice = 350
                }
            };
        }
    }
}
