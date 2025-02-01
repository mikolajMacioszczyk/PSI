using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IOrderRepository
{
    
    Task<Order?> GetByIdWithShipment(Guid orderId);
    Task<Order?> GetNewestOrder();
    Task<(ICollection<Order>, int totalCount)> GetPaged<TKey>(
        int pageNumber, 
        int pageSize, 
        Expression<Func<Order, bool>>? filter = null,
        Expression<Func<Order, TKey>>? orderBy = null,
        bool descending = false);

    Task<Order> CreateAsync(Order order);
}
