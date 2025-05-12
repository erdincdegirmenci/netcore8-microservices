using MicroStack.Sourcing.Entitites;

namespace MicroStack.Sourcing.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task SendBid(Bid bid);
        Task<List<Bid>> GetBidByAuctionId(string auctionId);
        Task<Bid> GetWinnerBid(string auctionId);
    }
}
