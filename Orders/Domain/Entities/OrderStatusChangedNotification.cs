using Domain.Enums;

namespace Domain.Entities
{
    public class OrderStatusChangedNotification : Notification
    {
        public OrderStatus NewStatus { get; set; }
        public OrderStatus? PreviousStatus { get; set; }
        public Guid ClientId { get; set; }
        public Guid OrderId { get; set; }
        public required Order Order { get; set; }
    }
}
