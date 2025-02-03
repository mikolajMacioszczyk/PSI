using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class HttpInventoryService : IInventoryService
{
    public Task<IEnumerable<InventoryProduct>> GetInventoryProductsBySKUs(IEnumerable<string> skus)
    {
        // TODO
        throw new NotImplementedException();
    }
}
