using Application.Models;

namespace Application.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<InventoryProduct>> GetInventoryProductsBySKUs(IEnumerable<string> skus);
}
