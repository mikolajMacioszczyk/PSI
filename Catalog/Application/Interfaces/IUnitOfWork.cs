namespace Application.Interfaces;

public interface IUnitOfWork
{
    ICatalogProductRepository CatalogProductRepository { get; }

    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
