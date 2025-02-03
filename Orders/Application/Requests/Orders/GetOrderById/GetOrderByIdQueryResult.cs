using Application.Models;
using Application.Requests.Purchases;

namespace Application.Requests.Orders.GetOrderById;

public class GetOrderByIdQueryResult : OrderResult
{
    public IEnumerable<CatalogProduct> Products { get; set; } = [];
    public PurchaseResult? Purchase { get; set; }
}
