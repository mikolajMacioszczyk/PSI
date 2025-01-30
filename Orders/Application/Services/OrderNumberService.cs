using Application.Interfaces;
using Common.Application.Interfaces;

namespace Application.Services;

// TODO: Tests
public class OrderNumberService : IOrderNumberService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public OrderNumberService(IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GetNext()
    {
        var datePart = _dateTimeProvider.GetCurrentDate().ToString("yyyyMMdd");
        var nextNumber = await GetNextOrderNumber();
        return $"ZM_{datePart}_{nextNumber}";
    }

    private async Task<int> GetNextOrderNumber()
    {
        var newestOrder = await _unitOfWork.OrderRepository.GetNewestOrder();
        if (newestOrder is null)
        {
            return 1;
        }

        bool isFromTheSameDay = DateOnly.FromDateTime(newestOrder.SubmitionTimestamp) == _dateTimeProvider.GetCurrentDate();
        if (isFromTheSameDay)
        {
            var parts = newestOrder.OrderNumber.Split('_');
            if (parts.Length < 3)
            {
                return 1;
            }

            var currentHighestNumber = Convert.ToInt32(newestOrder.OrderNumber.Split('_')[2]);
            return currentHighestNumber + 1;
        }

        return 1;
    }
}
