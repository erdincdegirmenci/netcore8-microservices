using MicroStack.Sourcing.Entitites;
using MongoDB.Driver;

namespace MicroStack.Sourcing.Data.Interfaces
{
    public interface ISourcingContext
    {
        IMongoCollection<Auction> Auctions { get; }
        IMongoCollection<Bid> Bids { get; }
    }
}
