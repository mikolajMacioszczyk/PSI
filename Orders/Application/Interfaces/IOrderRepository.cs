using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderRepository
{
    
    Task<Order?> GetByIdWithShipment(Guid orderId);
    Task<Order> CreateAsync(Order order);
    Task<Order?> GetNewestOrder();
}
