using Application.Interfaces;
using Application.Models;
using Common.Application.Services;
using System.Text;

namespace Application.Services;

public class HttpBasketService : HttpServiceBase, IBasketService
{
    public HttpBasketService(HttpClient httpClient) : base(httpClient)
    {}

    public Task<Basket?> GetBasketById(Guid basketId)
        => Get<Basket>($"Basket/{basketId}");

    public async Task SetBasketInactive(Guid basketId)
    {
        var response = await _httpClient.PutAsync($"Basket/{basketId}/inactive", new StringContent(string.Empty, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}
