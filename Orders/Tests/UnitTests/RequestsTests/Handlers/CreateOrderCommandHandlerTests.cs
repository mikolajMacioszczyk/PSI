using Application.Interfaces;
using Application.Models;
using Application.Requests.Orders.CreateOrder;
using AutoMapper;
using Common.Application.Interfaces;
using Domain.Entities;
using Moq;

namespace UnitTests.RequestsTests.Handlers;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IBasketService> _basketServiceMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IOrderPriceService> _orderPriceServiceMock = new();
    private readonly Mock<IOrderNumberService> _orderNumberServiceMock = new();
    private readonly Mock<IShipmentService> _shipmentServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.OrderRepository).Returns(_orderRepositoryMock.Object);
        _handler = new CreateOrderCommandHandler(
            _unitOfWorkMock.Object,
            _basketServiceMock.Object,
            _dateTimeProviderMock.Object,
            _orderPriceServiceMock.Object,
            _orderNumberServiceMock.Object,
            _shipmentServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBasketDoesNotExist()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var command = new CreateOrderCommand(basketId, true, Guid.NewGuid(), "first name", "last name", 
            "email", "country", "city", "street", "postal code", 1, "phone number", "area code");
        _basketServiceMock.Setup(s => s.GetBasketById(command.BasketId)).ReturnsAsync((Basket?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Basket with provided id {command.BasketId} not exists", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBasketIsNotActive()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket { Id = basketId, IsActive = false };
        var command = new CreateOrderCommand(basketId, true, Guid.NewGuid(), "first name", "last name",
            "email", "country", "city", "street", "postal code", 1, "phone number", "area code");
        _basketServiceMock.Setup(s => s.GetBasketById(command.BasketId)).ReturnsAsync(basket);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Basket with provided id {command.BasketId} is not active", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenShipmentProviderDoesNotExist()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket { Id = basketId, IsActive = true };
        var command = new CreateOrderCommand(basketId, true, Guid.NewGuid(), "first name", "last name",
            "email", "country", "city", "street", "postal code", 1, "phone number", "area code");

        _basketServiceMock.Setup(s => s.GetBasketById(command.BasketId)).ReturnsAsync(basket);
        _shipmentServiceMock.Setup(s => s.ValidateShipmentProviderExists(command.ShipmentProviderId)).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Shipment provider with provided id {command.ShipmentProviderId} not exists", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrderIsCreatedSuccessfully()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var basket = new Basket { Id = basketId, IsActive = true, UserId = Guid.NewGuid() };
        var command = new CreateOrderCommand(basketId, true, Guid.NewGuid(), "first name", "last name",
            "email", "country", "city", "street", "postal code", 1, "phone number", "area code");

        _basketServiceMock.Setup(s => s.GetBasketById(command.BasketId)).ReturnsAsync(basket);

        _shipmentServiceMock.Setup(s => s.ValidateShipmentProviderExists(command.ShipmentProviderId)).Returns(true);

        decimal shipmentPrice = 10m;
        _shipmentServiceMock.Setup(s => s.GetShipmentPrice(basket)).ReturnsAsync(shipmentPrice);

        var mappedShipment = new Shipment
        {
            Id = Guid.NewGuid(),
            LastName = command.LastName,
            FirstName = command.FirstName,
            Email = command.Email,
            Country = command.Country,
            City = command.City,
            AreaCode = command.AreaCode,
            PhoneNumber = command.PhoneNumber,
            PostalCode = command.PostalCode,
            Street = command.Street,
            TrackingLink = command.Street,
            HomeNumber = command.HomeNumber,
        };
        _mapperMock.Setup(m => m.Map<Shipment>(command)).Returns(mappedShipment);

        var order = new Order { Id = Guid.NewGuid(), OrderNumber = "", OrderPrice = 100m };

        var generatedOrderNumber = "ZM_20240131_001";
        _orderNumberServiceMock.Setup(s => s.GetNext()).ReturnsAsync(generatedOrderNumber);

        var generatedOrderPrice = 100m;
        _orderPriceServiceMock.Setup(s => s.GetOrderPrice(basket)).ReturnsAsync(generatedOrderPrice);

        var now = DateTime.UtcNow;
        _dateTimeProviderMock.Setup(m => m.GetCurrentTime())
            .Returns(now);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(shipmentPrice, mappedShipment.ShipmentPrice);
        Assert.Equal(shipmentPrice, mappedShipment.ShipmentPrice);
        _orderNumberServiceMock.Verify();
        _unitOfWorkMock.Verify(u => u.OrderRepository.CreateAsync(It.IsAny<Order>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _basketServiceMock.Verify(s => s.SetBasketInactive(command.BasketId), Times.Once);

        Order createdOrder = (Order) _orderRepositoryMock.Invocations.First(i => i.Method.Name == nameof(IOrderRepository.CreateAsync)).Arguments[0];
        Assert.Equal(mappedShipment, createdOrder.Shipment);
        Assert.Equal(basketId, createdOrder.BasketId);
        Assert.Equal(basket.UserId, createdOrder.ClientId);
        Assert.Equal(generatedOrderNumber, createdOrder.OrderNumber);
        Assert.Equal(generatedOrderPrice, createdOrder.OrderPrice);
        Assert.Equal(now, createdOrder.SubmitionTimestamp);
        Assert.True(createdOrder.ConsentGranted);
    }
}
