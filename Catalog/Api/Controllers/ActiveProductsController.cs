using Application.Requests.Products;
using Application.Requests.Products.GetActiveProductById;
using Application.Requests.Products.GetAllProducts;
using Common.Api.Controllers;
using Common.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    public class ActiveProductsController : BaseApiController
    {
        public ActiveProductsController(IMediator mediator) : base(mediator)
        {}

        [HttpGet()]
        public async Task<ActionResult<PagedResultBase<CatalogProductResult>>> GetActiveProducts([FromQuery] GetActiveProductsQuery query) =>
            Ok(await _mediator.Send(query));

        [HttpGet("{Id}")]
        public Task<ActionResult<CatalogProductResult>> GetActiveProductById([FromRoute] GetActiveProductByIdQuery query)
            => HandleRequest(query);
    }
}
