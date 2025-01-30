using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Shipments;

public class ShipmentProfile : Profile
{
    public ShipmentProfile()
    {
        CreateMap<Shipment, ShipmentResult>();
    }
}
