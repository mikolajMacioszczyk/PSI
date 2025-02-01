using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface ICatalogProductRepository
    {
        Task<(ICollection<CatalogProduct>, int totalCount)> GetPaged(int pageNumber, int pageSize, Expression<Func<CatalogProduct, bool>>? filter = null);
        Task<CatalogProduct?> GetById(Guid id);
        Task<IEnumerable<CatalogProduct>> GetByIds(IEnumerable<Guid> ids);
    }
}
