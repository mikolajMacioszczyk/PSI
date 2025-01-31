using Domain.Enums;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public Guid ClientId { get; set; }

        public Guid? PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }

        public Guid ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;

        public IEnumerable<OrderStatusChangedNotification> StatusChangedNotifications { get; set; } = [];

        public required string OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime SubmitionTimestamp { get; set; }
        public bool ConsentGranted { get; set; }
        public decimal OrderPrice { get; set; }
    }
}
