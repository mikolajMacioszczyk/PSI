using Application.Interfaces;
using Application.Models;
using Common.Application.Models;
using Common.Application.Services;
using System.Text.Json;

namespace Application.Services;

public class HttpCatalogProductsService : HttpServiceBase, ICatalogProductsService
{
    public HttpCatalogProductsService(HttpClient httpClient) : base(httpClient)
    {}

    public async Task<ICollection<CatalogProduct>> GetActiveCatalogProducts(uint pageSize, uint pageNumber)
    {
        var productsResponse = await _httpClient.GetAsync($"ActiveProducts?pageSize={pageSize}&pageNumber={pageNumber}");
        productsResponse.EnsureSuccessStatusCode();

        var responseContent = await productsResponse.Content.ReadAsStringAsync();
        var pagedCollection = JsonSerializer.Deserialize<PagedResultBase<CatalogProduct>>(responseContent, SerializerOptions.Value);
        return pagedCollection?.Items ?? [];
    }

    public Task<CatalogProduct?> GetCatalogProductById(Guid id)
        => Get<CatalogProduct>($"ActiveProducts/{id}");
}
