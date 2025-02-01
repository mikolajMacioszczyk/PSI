using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Products.GetActiveProductById;

public class GetActiveProductByIdQueryHandler : IRequestHandler<GetActiveProductByIdQuery, Result<CatalogProductResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CatalogProductResult>> Handle(GetActiveProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.CatalogProductRepository.GetById(request.Id);

        if (product is null || !product.IsActive())
        {
            return new NotFound(request.Id, $"Catalog product with provided id {request.Id} does not exists");
        }

        return _mapper.Map<CatalogProductResult>(product);
    }
}
