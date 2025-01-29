using Application.Models;
using Application.Requests.Products;
using Application.Requests.Products.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public Task<PagedResultBase<CatalogProductResult>> GetActiveProducts([FromQuery] GetActiveProductsQuery query) =>
            _mediator.Send(query);
    }
}
