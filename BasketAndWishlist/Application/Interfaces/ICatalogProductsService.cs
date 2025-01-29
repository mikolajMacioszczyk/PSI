using Application.Models;

namespace Application.Interfaces;

public interface ICatalogProductsService
{
    Task<ICollection<CatalogProduct>> GetActiveCatalogProducts(uint pageSize, uint pageNumber); 
}
