using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Infrastructure.Data
{
    public class OrderContext: DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            
        }

        public DbSet<OrderEntity> Orders { get; set; }

    }
}
