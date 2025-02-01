using Application.Models;

namespace Application.Interfaces;

public interface IOrderPriceService
{
    Task<decimal> GetOrderPrice(Basket basket);
}
