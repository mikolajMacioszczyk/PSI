using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> CreateAsync(Basket basket);
    }
}
