using Common.Application.Interfaces;

namespace Common.Application.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateOnly GetCurrentDate()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}
