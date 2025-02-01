using Common.Application.Models;
using MediatR;

namespace Application.Requests.Products.GetAllProducts;

public record GetActiveProductsQuery : PagedResultQueryBase, IRequest<PagedResultBase<CatalogProductResult>>;
