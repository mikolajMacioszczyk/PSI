using Application.Requests.Orders;
using Application.Requests.Orders.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // TODO: Get order by id
        // TODO: Get my orders 

        // TODO: Payments

        // TODO: Authorize
        [HttpPost()]
        public async Task<ActionResult<OrderResult>> CreateOrder([FromRoute] CreateOrderCommand command)
        {
            // TODO: Handle
            return Ok(await _mediator.Send(command));
        }
    }
}
