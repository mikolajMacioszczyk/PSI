using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Products.GetActiveProductsByIds;

public class GetActiveProductsByIdsQueryHandler : IRequestHandler<GetActiveProductsByIdsQuery, Result<IEnumerable<CatalogProductResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveProductsByIdsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CatalogProductResult>>> Handle(GetActiveProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        var products = (await _unitOfWork.CatalogProductRepository.GetByIds(request.Ids))
            .Where(p => p.IsActive());

        if (products.Count() != request.Ids.Count())
        {
            var missingIds = request.Ids.Where(i => !products.Any(p => p.Id != i));
            var missingIdsMessage = string.Join(",", missingIds);
            return new NotFound(string.Join(",", missingIdsMessage), $"catalog products with ids {missingIdsMessage} not exists");
        }

        return _mapper.Map<IEnumerable<CatalogProductResult>>(products).ToList();
    }
}
