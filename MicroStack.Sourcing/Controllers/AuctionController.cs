using Microsoft.AspNetCore.Mvc;
using MicroStack.Sourcing.Entitites;
using MicroStack.Sourcing.Repositories.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ILogger<AuctionController> _logger;

    public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger)
    {
        _auctionRepository = auctionRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
    {
        var auctions = await _auctionRepository.GetAuctions();
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Auction>> GetAuction(string id)
    {
        var auction = await _auctionRepository.GetAuction(id);
        if (auction == null)
        {
            _logger.LogWarning("Auction with id: {Id} not found", id);
            return NotFound();
        }
        return Ok(auction);
    }

    [HttpGet("GetByName/{name}")]
    public async Task<ActionResult<Auction>> GetByName(string name)
    {
        var auction = await _auctionRepository.GetAuctionByName(name);
        if (auction == null)
        {
            _logger.LogWarning("Auction with name: {Name} not found", name);
            return NotFound();
        }
        return Ok(auction);
    }

    [HttpPost]
    public async Task<ActionResult<Auction>> Create([FromBody] Auction auction)
    {
        await _auctionRepository.Create(auction);
        return CreatedAtAction(nameof(GetAuction), new { id = auction.Id }, auction);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Auction auction)
    {
        var updated = await _auctionRepository.Update(auction);
        if (!updated)
        {
            _logger.LogWarning("Update failed. Auction with id: {Id} not found", auction.Id);
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _auctionRepository.Delete(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete failed. Auction with id: {Id} not found", id);
            return NotFound();
        }
        return Ok(deleted);
    }
}
