using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICatalogProductRepository
    {
        Task<ICollection<CatalogProduct>> GetAll();
    }
}
