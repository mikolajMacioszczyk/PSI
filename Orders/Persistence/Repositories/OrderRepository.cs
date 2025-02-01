using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public Task<Order?> GetNewestOrder()
    {
        return _context.Orders.OrderByDescending(o => o.SubmitionTimestamp).FirstOrDefaultAsync();
    }

    public async Task<(ICollection<Order>, int totalCount)> GetPaged<TKey>(
        int pageNumber, 
        int pageSize, 
        Expression<Func<Order, bool>>? filter = null,
        Expression<Func<Order, TKey>>? orderBy = null,
        bool descending = false)
    {
        IQueryable<Order> query = _context.Orders;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        if (orderBy != null)
        {
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        var pagedCollection = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (pagedCollection, totalCount);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _context.AddAsync(order);
        return order;
    }
}
