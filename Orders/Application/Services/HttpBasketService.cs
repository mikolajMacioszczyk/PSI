using Application.Interfaces;
using Application.Models;
using Common.Application.Services;

namespace Application.Services;

public class HttpBasketService : HttpServiceBase, IBasketService
{
    public HttpBasketService(HttpClient httpClient) : base(httpClient)
    {}

    public Task<Basket?> GetBasketById(Guid basketId)
        => Get<Basket>($"Basket/{basketId}");
}
