namespace Common.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime GetCurrentTime();
    DateOnly GetCurrentDate();
}
