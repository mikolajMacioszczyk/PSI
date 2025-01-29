using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Requests.Products.GetAllProducts
{
    public class GetActiveProductsQueryHandler : IRequestHandler<GetActiveProductsQuery, PagedResultBase<CatalogProductResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultBase<CatalogProductResult>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
        {
            var (catalogProducts, totalCount) = await _unitOfWork.CatalogProductRepository.GetPaged((int)request.PageNumber, (int)request.PageSize);
            var items = _mapper.Map<IEnumerable<CatalogProductResult>>(catalogProducts);
            return new PagedResultBase<CatalogProductResult>(items.ToList(), (uint)totalCount, request.PageSize, request.PageNumber);
        }
    }
}
