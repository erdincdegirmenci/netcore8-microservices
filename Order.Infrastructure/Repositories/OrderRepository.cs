using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : Repository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {

        }
        public async Task<IEnumerable<OrderEntity>> GetOrdersBySellerUserName(string sellerUserName)
        {
            var orderList = await _orderContext.Orders.Where(o => o.SellerUserName == sellerUserName).ToListAsync();
            return orderList;
        }
    }
}
