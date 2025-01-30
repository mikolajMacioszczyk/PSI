namespace Application.Interfaces;

public interface IOrderNumberService
{
    Task<string> GetNext();
}
