using Application.Requests.Baskets;
using Application.Requests.Baskets.AddProductToBasket;
using Application.Requests.Baskets.CreateEmptyBasket;
using Application.Requests.Baskets.GetBasketById;
using Application.Requests.Baskets.PopulateMockBasket;
using Application.Requests.Baskets.SubstractProductFromBasket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BasketController : BaseApiController
    {
        public BasketController(IMediator mediator) : base(mediator)
        {}

        // TODO: Authorized
        [HttpGet("{Id}")]
        public Task<ActionResult<BasketResult>> GetBasketById([FromRoute] GetBasketByIdQuery query) =>
            HandleRequest(query);

        // TODO: Authorized
        [HttpPost()]
        public async Task<ActionResult<BasketResult>> CreateEmptyBasket([FromBody] CreateEmptyBasketCommand command) =>
            Ok(await _mediator.Send(command));

        // TODO: Admin
        [HttpPost("populate")]
        public async Task<ActionResult<BasketResult>> PopulateMockBasket([FromQuery] PopulateMockBasketCommand command) =>
            Ok(await _mediator.Send(command));

        // TODO: Authorized
        [HttpPut("{BasketId}/product/{ProductInCatalogId}/add")]
        public Task<ActionResult<BasketResult>> AddProductToBasket([FromRoute] AddProductToBasketCommand command) =>
            HandleRequest(command);

        // TODO: Authorized
        [HttpPut("{BasketId}/product/{ProductInCatalogId}/substract")]
        public Task<ActionResult<BasketResult>> AddProductToBasket([FromRoute] SubstractProductFromBasketCommand command) =>
            HandleRequest(command);
    }
}
