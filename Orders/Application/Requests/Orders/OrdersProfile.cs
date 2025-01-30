using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Orders;

public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<Order, OrderResult>();
    }
}
