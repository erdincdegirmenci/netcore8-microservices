using Order.Domain.Entities;
using Order.Domain.Repositories.Base;

namespace Order.Domain.Repositories
{
    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<IEnumerable<OrderEntity>> GetOrdersBySellerUserName(string sellerUserName);
    }
}
