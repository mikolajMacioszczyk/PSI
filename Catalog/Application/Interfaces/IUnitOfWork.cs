namespace Application.Interfaces;

public interface IUnitOfWork
{
    event EventHandler? BeforeSaveChanges;

    Task<bool> SaveChangesAsync();
}
