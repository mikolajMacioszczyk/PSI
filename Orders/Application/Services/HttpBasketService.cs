using Application.Interfaces;
using Application.Models;
using System.Net.Http;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Application.Services;

public class HttpBasketService : IBasketService
{
    private readonly HttpClient _httpClient;

    public HttpBasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Basket?> GetBasketById(string basketId)
    {
        var productResponse = await _httpClient.GetAsync($"Basket/{basketId}");

        if (productResponse.IsSuccessStatusCode)
        {
            var responseContent = await productResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Basket>(responseContent, SerializerOptions.Value);
        }
        else if (productResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Request failed with status code {productResponse.StatusCode}");
    }

    // TODO: Base HttpClient
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
