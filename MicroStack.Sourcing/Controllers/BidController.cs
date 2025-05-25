using Microsoft.AspNetCore.Mvc;
using MicroStack.Sourcing.Entitites;
using MicroStack.Sourcing.Repositories.Interfaces;
using System.Net;

namespace MicroStack.Sourcing.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidRepository bidRepository, ILogger<BidController> logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendBid([FromBody] Bid bid)
        {
            try
            {
                await _bidRepository.SendBid(bid);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending bid for auctionId: {AuctionId}", bid.AuctionId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetBidByAuctionId/{auctionId}")]
        public async Task<ActionResult<List<Bid>>> GetBidByAuctionId(string auctionId)
        {
            var bids = await _bidRepository.GetBidByAuctionId(auctionId);
            if (bids == null || !bids.Any())
            {
                _logger.LogWarning("No bids found for auctionId: {AuctionId}", auctionId);
                return NotFound();
            }
            return Ok(bids);
        }
        [HttpGet("GetAllBidsByAuctionId")]
        [ProducesResponseType(typeof(List<Bid>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetAllBidsByAuctionId(string id)
        {
            IEnumerable<Bid> bids = await _bidRepository.GetAllBidsByAuctionId(id);

            return Ok(bids);
        }

        [HttpGet("GetWinnerBid/{auctionId}")]
        public async Task<ActionResult<Bid>> GetWinnerBid(string auctionId)
        {
            var bid = await _bidRepository.GetWinnerBid(auctionId);
            if (bid == null)
            {
                _logger.LogWarning("No winning bid found for auctionId: {AuctionId}", auctionId);
                return NotFound();
            }
            return Ok(bid);
        }
    }
}
