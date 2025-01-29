using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CatalogProduct, CatalogProductResult>();
    }
}
