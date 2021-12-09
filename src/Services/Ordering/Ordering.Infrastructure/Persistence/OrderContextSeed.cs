using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            var isOrderAvailable = await orderContext.Orders.AnyAsync();

            if (!isOrderAvailable)
            {
                await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbcontextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    Username = "sharthak123",
                    FirstName = "sharthak",
                    LastName = "mallik",
                    EmailAddress="sharthakmallik@gmail.com",
                    AddressLine="Siliguri, Westbengal-734003",
                    TotalPrice = 350
                }
            };
        }
    }
}
