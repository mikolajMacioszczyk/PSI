using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Requests.Products.GetActiveProductById;

public class GetActiveProductByIdQueryHandler : IRequestHandler<GetActiveProductByIdQuery, CatalogProductResult?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CatalogProductResult?> Handle(GetActiveProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.CatalogProductRepository.GetById(request.Id);

        if (product is null || !product.IsActive())
        {
            return null;
        }

        return _mapper.Map<CatalogProductResult>(product);
    }
}
