using Domain.Enums;

namespace Domain.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public required Order Order { get; set; }

        public decimal Amount { get; set; }
        public DateTime PurchaseTimestamp { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
