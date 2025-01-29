using MediatR;

namespace Application.Requests.Products.GetActiveProductById;

public record GetActiveProductByIdQuery(Guid Id) : IRequest<CatalogProductResult?>;