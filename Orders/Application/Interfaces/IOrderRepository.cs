using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
}
