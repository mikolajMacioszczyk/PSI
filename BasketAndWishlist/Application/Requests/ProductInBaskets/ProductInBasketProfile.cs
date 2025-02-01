using AutoMapper;
using Domain.Entities;

namespace Application.Requests.ProductInBaskets;

public class ProductInBasketProfile : Profile
{
    public ProductInBasketProfile()
    {
        CreateMap<ProductInBasket, ProductInBasketResult>();
    }
}
