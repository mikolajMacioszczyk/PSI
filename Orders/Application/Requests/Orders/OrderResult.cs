using Application.Requests.Shipments;
using Domain.Enums;

namespace Application.Requests.Orders;

public class OrderResult
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ClientId { get; set; }

    public Guid? PurchaseId { get; set; }

    public required ShipmentResult Shipment { get; set; }

    public required string OrderNumber { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime SubmitionTimestamp { get; set; }
    public bool ConsentGranted { get; set; }
    public decimal OrderPrice { get; set; }
}
