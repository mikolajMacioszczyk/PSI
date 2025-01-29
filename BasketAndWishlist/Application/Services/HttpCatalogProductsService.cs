using Application.Interfaces;
using Application.Models;
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
        var productsResponse = await _httpClient.GetAsync($"products?pageSize={pageSize}&pageNumber={pageNumber}");
        productsResponse.EnsureSuccessStatusCode();

        var responseContent = await productsResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ICollection<CatalogProduct>>(responseContent, SerializerOptions.Value) ?? [];
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
