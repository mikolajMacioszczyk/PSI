using Domain.Entities;

namespace Application.Interfaces;

public interface IPurchaseRepository
{
    Task<Purchase?> GetById(Guid purchaseId);

    Task<Purchase> CreateAsync(Purchase order);
}
