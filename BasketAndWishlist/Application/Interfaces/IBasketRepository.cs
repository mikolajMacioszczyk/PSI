using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> CreateAsync(Basket basket);
        Task<Basket?> GetById(Guid id);
        Task<Basket?> GetByIdWithProducts(Guid id);
        Task<Basket?> GetActiveByUserIdWithProducts(Guid userId);
        void Update(Basket basket);
    }
}
