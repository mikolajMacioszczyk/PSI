using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
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
            var (catalogProducts, totalCount) = await _unitOfWork.CatalogProductRepository
                .GetPaged((int)request.PageNumber, (int)request.PageSize, p => p.InCatalogToTimestamp == null || p.InCatalogToTimestamp < DateTime.UtcNow);
            var items = _mapper.Map<IEnumerable<CatalogProductResult>>(catalogProducts);
            return new PagedResultBase<CatalogProductResult>(items.ToList(), (uint)totalCount, request.PageSize, request.PageNumber);
        }
    }
}
