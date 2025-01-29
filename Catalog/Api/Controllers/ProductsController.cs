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
        public Task<IEnumerable<CatalogProductResult>> GetActiveProducts() =>
            _mediator.Send(new GetActiveProductsQuery());
    }
}
