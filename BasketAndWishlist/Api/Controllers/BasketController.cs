using Application.Requests.Baskets;
using Application.Requests.Baskets.AddProductToBasket;
using Application.Requests.Baskets.CreateOrGetBasket;
using Application.Requests.Baskets.GetBasketById;
using Application.Requests.Baskets.PopulateMockBasket;
using Application.Requests.Baskets.SubstractProductFromBasket;
using Common.Api.Controllers;
using Common.Infrastructure.AuthenticationAdapters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BasketController : BaseApiController
    {
        public BasketController(IMediator mediator) : base(mediator)
        {}

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public Task<ActionResult<BasketResult>> GetBasketById([FromRoute] GetBasketByIdQuery query) =>
            HandleRequest(query);

        [AllowAnonymous]
        [HttpPost("guest")]
        public async Task<ActionResult<BasketResult>> CreateEmptyBasket([FromBody] CreateOrGetBasketCommand command) =>
            Ok(await _mediator.Send(command));

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<BasketResult>> CreateOrGetBasket([FromBody] CreateOrGetBasketCommand command) =>
            Ok(await _mediator.Send(command));

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("populate")]
        public async Task<ActionResult<BasketResult>> PopulateMockBasket([FromQuery] PopulateMockBasketCommand command) =>
            Ok(await _mediator.Send(command));

        [AllowAnonymous]
        [HttpPut("{BasketId}/product/{ProductInCatalogId}/add")]
        public Task<ActionResult<BasketResult>> AddProductToBasket([FromRoute] AddProductToBasketCommand command) =>
            HandleRequest(command);

        [AllowAnonymous]
        [HttpPut("{BasketId}/product/{ProductInCatalogId}/substract")]
        public Task<ActionResult<BasketResult>> AddProductToBasket([FromRoute] SubstractProductFromBasketCommand command) =>
            HandleRequest(command);
    }
}
