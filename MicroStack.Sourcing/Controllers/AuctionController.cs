using AutoMapper;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using MicroStack.Sourcing.Entitites;
using MicroStack.Sourcing.Enums;
using MicroStack.Sourcing.Repositories.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly ILogger<AuctionController> _logger;
    private readonly IMapper _mapper;
    private readonly EventBusRabbitMQProducer _eventBusRabbitMQProducer;


    public AuctionController(EventBusRabbitMQProducer eventBusRabbitMQProducer, IMapper mapper, IAuctionRepository auctionRepository, IBidRepository bidRepository, ILogger<AuctionController> logger)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
        _logger = logger;
        _mapper = mapper;
        _eventBusRabbitMQProducer = eventBusRabbitMQProducer;
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
    [HttpPost("CompleteAuction")]
    public async Task<ActionResult> CompleteAuction([FromBody] string id)
    {
        Auction auction = await _auctionRepository.GetAuction(id);
        if (auction == null)
            return NotFound();

        if (auction.Status != (int)Status.Active)
        {
            _logger.LogError("Auction can not be completed");
            return BadRequest();
        }

        Bid bid = await _bidRepository.GetWinnerBid(id);
        if (bid == null)
            return NotFound();

        OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
        eventMessage.Quantity = auction.Quantity;
        auction.Status = (int)Status.Closed;
        bool updateResponse = await _auctionRepository.Update(auction);
        if (!updateResponse)
        {
            _logger.LogError("Auction can not updated");
            return BadRequest();

        }
        try
        {
            _eventBusRabbitMQProducer.Publish(EventBusConstants.OrderCreateQueue, eventMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Sourcing");
            throw;
        }
        return Accepted();
    }
    [HttpPost("TestEvent")]
    public ActionResult<OrderCreateEvent> TestEvent()
    {
        OrderCreateEvent eventMesssage = new OrderCreateEvent();
        eventMesssage.AuctionId = "dummyAuction1";
        eventMesssage.ProductId = "dummyProduct";
        eventMesssage.Price = 10;
        eventMesssage.Quantity = 100;
        eventMesssage.SellerUserName = "test@test.com";
        try
        {
            _eventBusRabbitMQProducer.Publish(EventBusConstants.OrderCreateQueue, eventMesssage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing integration event: {EventId} from {AppName}", eventMesssage.Id, "Sourcing");
            throw;

        }
        return Accepted(eventMesssage);
    }
}
