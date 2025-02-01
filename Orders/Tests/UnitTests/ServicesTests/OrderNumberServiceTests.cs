using Application.Interfaces;
using Application.Services;
using Common.Application.Interfaces;
using Domain.Entities;
using Moq;

namespace UnitTests.ServicesTests;

public class OrderNumberServiceTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly OrderNumberService _orderNumberService;

    public OrderNumberServiceTests()
    {
        _unitOfWorkMock.Setup(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);

        _orderNumberService = new OrderNumberService(_dateTimeProviderMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetNext_ShouldReturnFirstOrderNumber_WhenNoPreviousOrderExists()
    {
        // Arrange
        var currentDate = new DateOnly(2024, 1, 31);
        _dateTimeProviderMock.Setup(d => d.GetCurrentDate()).Returns(currentDate);
        _orderRepositoryMock.Setup(r => r.GetNewestOrder()).ReturnsAsync((Order?)null);

        // Act
        var result = await _orderNumberService.GetNext();

        // Assert
        Assert.Equal("ZM_20240131_1", result);
    }

    [Fact]
    public async Task GetNext_ShouldReturnIncrementedOrderNumber_WhenPreviousOrderExistsFromSameDay()
    {
        // Arrange
        var currentDate = new DateOnly(2024, 1, 31);
        _dateTimeProviderMock.Setup(d => d.GetCurrentDate()).Returns(currentDate);

        var previousOrder = new Order
        {
            OrderNumber = "ZM_20240131_5",
            SubmitionTimestamp = new DateTime(2024, 1, 31, 10, 0, 0),
        };
        _orderRepositoryMock.Setup(r => r.GetNewestOrder()).ReturnsAsync(previousOrder);

        // Act
        var result = await _orderNumberService.GetNext();

        // Assert
        Assert.Equal("ZM_20240131_6", result);
    }

    [Fact]
    public async Task GetNext_ShouldReturnFirstOrderNumber_WhenPreviousOrderIsFromDifferentDay()
    {
        // Arrange
        var currentDate = new DateOnly(2024, 2, 1);
        _dateTimeProviderMock.Setup(d => d.GetCurrentDate()).Returns(currentDate);

        var previousOrder = new Order
        {
            OrderNumber = "ZM_20240131_10",
            SubmitionTimestamp = new DateTime(2024, 1, 31, 23, 59, 59)
        };
        _orderRepositoryMock.Setup(r => r.GetNewestOrder()).ReturnsAsync(previousOrder);

        // Act
        var result = await _orderNumberService.GetNext();

        // Assert
        Assert.Equal("ZM_20240201_1", result);
    }
}
