using Application.Interfaces;
using Domain.Entities;

namespace Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _context;

    public OrderRepository(OrdersContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _context.AddAsync(order);
        return order;
    }
}
