using MicroStack.Sourcing.Entitites;

namespace MicroStack.Sourcing.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task SendBid(Bid bid);
        Task<List<Bid>> GetBidByAuctionId(string auctionId);
        Task<List<Bid>> GetAllBidsByAuctionId(string id);
        Task<Bid> GetWinnerBid(string auctionId);
    }
}
