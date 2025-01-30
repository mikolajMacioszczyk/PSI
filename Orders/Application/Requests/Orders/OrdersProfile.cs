using Application.Requests.Orders.CreateOrder;
using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Orders;

public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<Order, OrderResult>();
        CreateMap<CreateOrderCommand, Shipment>()
            .ForMember(m => m.ProviderId, opt => opt.MapFrom(src => src.ShipmentProviderId))
            .ForMember(m => m.TrackingLink, opt => opt.MapFrom(_ => string.Empty));
    }
}
