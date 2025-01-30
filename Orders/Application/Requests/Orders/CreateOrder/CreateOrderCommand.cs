using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.CreateOrder;

public record CreateOrderCommand(
    Guid BasketId,
    bool ConsentGranted,
    Guid ShipmentProviderId,
    string FirstName,
    string LastName,
    string Email,
    string Country,
    string City,
    string Street,
    string PostalCode,
    int HomeNumber,
    string PhoneNumber,
    string AreaCode
    ) 
    : IRequest<Result<OrderResult>>;
