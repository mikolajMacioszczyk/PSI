using MediatR;

namespace Application.Requests.Products.GetAllProducts;

public record GetActiveProductsQuery : IRequest<IEnumerable<CatalogProductResult>>;
