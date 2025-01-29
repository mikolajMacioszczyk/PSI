using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Baskets;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<Basket, BasketResult>();
    }
}
