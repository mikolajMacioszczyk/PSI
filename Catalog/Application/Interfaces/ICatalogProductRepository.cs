using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICatalogProductRepository
    {
        Task<ICollection<CatalogProduct>> GetAll();
        Task<(ICollection<CatalogProduct>, int totalCount)> GetPaged(int pageNumber, int pageSize);
    }
}
