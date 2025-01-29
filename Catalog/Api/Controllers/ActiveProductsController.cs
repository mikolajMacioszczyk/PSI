using Application.Models;
using Application.Requests.Products;
using Application.Requests.Products.GetActiveProductById;
using Application.Requests.Products.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ActiveProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActiveProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<PagedResultBase<CatalogProductResult>>> GetActiveProducts([FromQuery] GetActiveProductsQuery query) =>
            Ok(await _mediator.Send(query));

        [HttpGet("{Id}")]
        public async Task<ActionResult<CatalogProductResult>> GetActiveProductById([FromRoute] GetActiveProductByIdQuery query)
        {
            var product = await _mediator.Send(query);
            if (product is null)
            {
                return NotFound(query.Id);
            }
            return Ok(product);
        }
    }
}
