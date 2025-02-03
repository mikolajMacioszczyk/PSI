using Application.Interfaces;
using Application.Models;
using Common.Application.Services;

namespace Application.Services;

public class HttpCatalogService : HttpServiceBase, ICatalogService
{
    public HttpCatalogService(HttpClient httpClient) : base(httpClient)
    {}

    public async Task<IEnumerable<CatalogProduct>> GetCatalogProductsByIds(IEnumerable<Guid> ids)
    {
        if (ids.Any())
        {
            ids = ids.Distinct();
            return (await Get<IEnumerable<CatalogProduct>>($"ActiveProducts/search?Ids={string.Join("&Ids=", ids)}"))!;
        }
        return [];
    }
}
