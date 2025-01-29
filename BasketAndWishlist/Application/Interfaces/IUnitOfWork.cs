namespace Application.Interfaces;

public interface IUnitOfWork
{
    IBasketRepository BasketRepository { get; }

    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
