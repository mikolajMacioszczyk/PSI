using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Requests.Products.GetAllProducts
{
    public class GetActiveProductsQueryHandler : IRequestHandler<GetActiveProductsQuery, IEnumerable<CatalogProductResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CatalogProductResult>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
        {
            var catalogProducts = await _unitOfWork.CatalogProductRepository.GetAll();
            return _mapper.Map<IEnumerable<CatalogProductResult>>(catalogProducts);
        }
    }
}
