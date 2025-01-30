using Application.Requests.Orders;
using Application.Requests.Orders.CreateOrder;
using Common.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class OrdersController : BaseApiController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {}

        // TODO: Get order by id
        // TODO: Get my orders 

        // TODO: Payments

        [AllowAnonymous]
        [HttpPost()]
        public Task<ActionResult<OrderResult>> CreateOrder([FromBody] CreateOrderCommand command) =>
            HandleRequest(command);
    }
}
