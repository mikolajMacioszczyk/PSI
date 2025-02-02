using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Purchases;

public class PurchaseProfile : Profile
{
    public PurchaseProfile()
    {
        CreateMap<Purchase, PurchaseResult>();
    }
}
