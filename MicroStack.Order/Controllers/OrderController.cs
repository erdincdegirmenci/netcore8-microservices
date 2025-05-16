using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands.OrderCreate;
using Order.Application.Queries;
using Order.Application.Responses;

namespace MicroStack.Order.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetOrdersByUserName/{userName}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersBySellerUsernameQuery(userName);
            var orders = await _mediator.Send(query);
            if (orders.Count() == decimal.Zero)
                return NotFound();
            return Ok(orders);
        }

        [HttpPost("OrderCreate")]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
