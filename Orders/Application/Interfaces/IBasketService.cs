using Application.Models;

namespace Application.Interfaces;

public interface IBasketService
{
    Task<Basket?> GetBasketById(string basketId);
}
