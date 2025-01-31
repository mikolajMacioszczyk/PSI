using Application.Models;

namespace Application.Interfaces;

public interface ICatalogService
{
    Task<IEnumerable<CatalogProduct>> GetCatalogProductsByIds(IEnumerable<Guid> ids);
}
