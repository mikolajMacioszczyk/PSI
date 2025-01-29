namespace Application.Interfaces;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { get; }

    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
