using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreConfiguredOrders());
                await orderContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<OrderEntity> GetPreConfiguredOrders()
        {

            return new List<OrderEntity>
            {
                new OrderEntity
                {
                    AuctionId =Guid.NewGuid().ToString(),
                    SellerUserName = "seller_one",
                    ProductId = Guid.NewGuid().ToString(),
                    UnitPrice = 199.99m,
                    TotalPrice = 1000,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new OrderEntity
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    SellerUserName = "seller_two",
                    ProductId = Guid.NewGuid().ToString(),
                    UnitPrice = 349.50m,
                    TotalPrice = 2000,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new OrderEntity
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    SellerUserName = "seller_three",
                    ProductId = Guid.NewGuid().ToString(),
                    UnitPrice = 89.90m,
                    TotalPrice = 3000,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
