using Application.Models;

namespace Application.Requests.Orders.GetOrderById;

public class GetOrderByIdQueryResult : OrderResult
{
    public IEnumerable<CatalogProduct> Products { get; set; } = [];
}
