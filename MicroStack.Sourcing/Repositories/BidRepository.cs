using MicroStack.Sourcing.Data.Interfaces;
using MicroStack.Sourcing.Entitites;
using MicroStack.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;
using Polly;

namespace MicroStack.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _sourcingContext;
        public BidRepository(ISourcingContext sourcingContext)
        {
            _sourcingContext = sourcingContext;
        }
        public async Task<List<Bid>> GetBidByAuctionId(string auctionId)
        {
            var filter = Builders<Bid>.Filter.Eq(q => q.AuctionId, auctionId);
            List<Bid> bids = await _sourcingContext.Bids.Find(filter).ToListAsync();
            bids = bids.OrderByDescending(a => a.CreatedAt).GroupBy(a => a.SellerUserName).Select(a => new Bid
            {
                AuctionId = a.FirstOrDefault().AuctionId,
                Price = a.FirstOrDefault().Price,
                CreatedAt = a.FirstOrDefault().CreatedAt,
                SellerUserName = a.FirstOrDefault().SellerUserName,
                ProductId = a.FirstOrDefault().ProductId,
                Id = a.FirstOrDefault().Id
            }).ToList();

            return bids;
        }

        public async Task<List<Bid>> GetAllBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(p => p.AuctionId, id);

            List<Bid> bids = await _sourcingContext
                          .Bids
                          .Find(filter)
                          .ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedAt)
                                   .Select(a => new Bid
                                   {
                                       AuctionId = a.AuctionId,
                                       Price = a.Price,
                                       CreatedAt = a.CreatedAt,
                                       SellerUserName = a.SellerUserName,
                                       ProductId = a.ProductId,
                                       Id = a.Id
                                   })
                                   .ToList();

            return bids;
        }
        public async Task<Bid> GetWinnerBid(string auctionId)
        {
            List<Bid> bids = await GetBidByAuctionId(auctionId);
            return bids.OrderByDescending(a => a.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _sourcingContext.Bids.InsertOneAsync(bid);
        }
    }
}
