﻿using Application.Requests.Baskets;
using Application.Requests.Baskets.GetBasketById;
using Application.Requests.Baskets.PopulateMockBasket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // TODO: Authorized
        [HttpGet("{Id}")]
        public Task<BasketResult?> GetBasketById([FromRoute] GetBasketByIdQuery query) =>
            _mediator.Send(query);

        // TODO: Admin
        [HttpPost("populate")]
        public Task<BasketResult> PopulateMockBasket([FromQuery] PopulateMockBasketCommand command) =>
            _mediator.Send(command);
    }
}
