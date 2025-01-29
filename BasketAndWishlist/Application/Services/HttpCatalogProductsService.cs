using Application.Interfaces;
using Application.Models;
using Common.Application.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Services;

public class HttpCatalogProductsService : ICatalogProductsService
{
    private readonly HttpClient _httpClient;

    public HttpCatalogProductsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<CatalogProduct>> GetActiveCatalogProducts(uint pageSize, uint pageNumber)
    {
        var productsResponse = await _httpClient.GetAsync($"ActiveProducts?pageSize={pageSize}&pageNumber={pageNumber}");
        productsResponse.EnsureSuccessStatusCode();

        var responseContent = await productsResponse.Content.ReadAsStringAsync();
        var pagedCollection = JsonSerializer.Deserialize<PagedResultBase<CatalogProduct>>(responseContent, SerializerOptions.Value);
        return pagedCollection?.Items ?? [];
    }

    public async Task<CatalogProduct?> GetCatalogProductById(Guid id)
    {
        var productResponse = await _httpClient.GetAsync($"ActiveProducts/{id}");

        if (productResponse.IsSuccessStatusCode)
        {
            var responseContent = await productResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CatalogProduct>(responseContent, SerializerOptions.Value);
        }
        else if (productResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Request failed with status code {productResponse.StatusCode}");
    }

    private static readonly Lazy<JsonSerializerOptions> SerializerOptions = new(InitializeSerializerOptions());

    private static JsonSerializerOptions InitializeSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
        };
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
