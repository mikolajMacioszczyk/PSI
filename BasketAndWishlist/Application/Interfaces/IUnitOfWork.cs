namespace Application.Interfaces;

public interface IUnitOfWork
{
    IBasketRepository BasketRepository { get; }
    IProductInBasketRepository ProductInBasketRepository { get; }

    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
