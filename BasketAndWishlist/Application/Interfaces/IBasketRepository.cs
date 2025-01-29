using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> CreateAsync(Basket basket);
        Task<Basket?> GetByIdWithProducts(Guid id);
        Task<Basket?> GetByUserIdWithProducts(Guid userId);
    }
}
