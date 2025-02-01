using Common.Application.Models;
using MediatR;

namespace Application.Requests.Products.GetActiveProductsByIds;

public record GetActiveProductsByIdsQuery(IEnumerable<Guid> Ids) : IRequest<Result<IEnumerable<CatalogProductResult>>>;
