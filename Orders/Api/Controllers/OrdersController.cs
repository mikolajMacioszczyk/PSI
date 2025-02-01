using Application.Requests.Orders;
using Application.Requests.Orders.CreateOrder;
using Application.Requests.Orders.GetExpectedOrderCost;
using Application.Requests.Orders.GetOrderById;
using Application.Requests.Orders.GetOrderHistory;
using Common.Api.Controllers;
using Common.Application.Models;
using Common.Infrastructure.AuthenticationAdapters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class OrdersController : BaseApiController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {}

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public Task<ActionResult<OrderResult>> GetOrderById([FromRoute] GetOrderByIdQuery query) =>
            HandleRequest(query);

        [AllowAnonymous]
        [HttpGet("costs")]
        public Task<ActionResult<GetExpectedOrderCostQueryResult>> GetExpectedOrderCost([FromQuery] GetExpectedOrderCostQuery query) =>
            HandleRequest(query);

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet("history")]
        public Task<ActionResult<PagedResultBase<OrderResult>>> GetOrderHistory([FromQuery] GetOrderHistoryQuery query) =>
            HandleRequest(query);

        [AllowAnonymous]
        [HttpPost()]
        public Task<ActionResult<OrderResult>> CreateOrder([FromBody] CreateOrderCommand command) =>
            HandleRequest(command);
    }
}
