using Domain.Enums;

namespace Application.Requests.Purchases;

public class PurchaseResult
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PurchaseTimestamp { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
