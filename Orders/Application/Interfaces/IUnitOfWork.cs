namespace Application.Interfaces;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { get; }
    IPurchaseRepository PurchaseRepository { get; }

    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
