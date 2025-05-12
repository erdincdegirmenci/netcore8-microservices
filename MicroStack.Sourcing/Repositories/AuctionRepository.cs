using MicroStack.Sourcing.Data.Interfaces;
using MicroStack.Sourcing.Entitites;
using MicroStack.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace MicroStack.Sourcing.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ISourcingContext _sourcingContext;
        public AuctionRepository(ISourcingContext sourcingContext)
        {
            _sourcingContext = sourcingContext;
        }
        public async Task Create(Auction auction)
        {
            await _sourcingContext.Auctions.InsertOneAsync(auction);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Auction>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _sourcingContext.Auctions.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Auction> GetAuction(string id)
        {
            return await _sourcingContext.Auctions.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Auction> GetAuctionByName(string name)
        {
            var filter = Builders<Auction>.Filter.Eq(p => p.Name, name);
            return await _sourcingContext.Auctions.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            return await _sourcingContext.Auctions.Find(p => true).ToListAsync();
        }

        public async Task<bool> Update(Auction auction)
        {
            var updateResult = await _sourcingContext.Auctions.ReplaceOneAsync(filter: g => g.Id == auction.Id, replacement: auction);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
