using MicroStack.Products.Entities;
using MongoDB.Driver;

namespace MicroStack.Products.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
