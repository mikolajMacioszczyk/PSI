using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersContext _context;

    public OrderRepository(OrdersContext context)
    {
        _context = context;
    }

    public Task<Order?> GetByIdWithShipment(Guid orderId)
    {
        return _context.Orders
            .Include(o => o.Shipment)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _context.AddAsync(order);
        return order;
    }

    public Task<Order?> GetNewestOrder()
    {
        return _context.Orders.OrderByDescending(o => o.SubmitionTimestamp).FirstOrDefaultAsync();
    }
}
