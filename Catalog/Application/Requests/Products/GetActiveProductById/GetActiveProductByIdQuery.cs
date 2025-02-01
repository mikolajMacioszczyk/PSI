using Common.Application.Models;
using MediatR;

namespace Application.Requests.Products.GetActiveProductById;

public record GetActiveProductByIdQuery(Guid Id) : IRequest<Result<CatalogProductResult>>;