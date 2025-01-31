using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class HttpCatalogService : ICatalogService
{
    // TODO
    public Task<IEnumerable<CatalogProduct>> GetCatalogProductsByIds(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }
}
