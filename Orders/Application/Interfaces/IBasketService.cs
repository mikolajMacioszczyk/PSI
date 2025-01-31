using Application.Models;

namespace Application.Interfaces;

public interface IBasketService
{
    Task<Basket?> GetBasketById(Guid basketId);
    Task SetBasketInactive(Guid basketId);
}
