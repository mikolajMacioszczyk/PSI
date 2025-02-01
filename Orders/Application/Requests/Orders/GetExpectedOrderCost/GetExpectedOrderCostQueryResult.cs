namespace Application.Requests.Orders.GetExpectedOrderCost;

public class GetExpectedOrderCostQueryResult
{
    public decimal ShipmentPrice { get; set; }
    public decimal OrderPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
