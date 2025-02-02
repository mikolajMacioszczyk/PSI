using Application.Requests.Purchases.CreatePurchaseWithCash;
using Application.Requests.Purchases;
using AutoMapper;
using Common.Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Moq;
using Application.Interfaces;

namespace UnitTests.RequestsTests.Handlers;

public class CreatePurchaseWithCashCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPurchaseRepository> _purchaseRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CreatePurchaseWithCashCommandHandler _handler;

    public CreatePurchaseWithCashCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.OrderRepository).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.Setup(m => m.PurchaseRepository).Returns(_purchaseRepositoryMock.Object);
        _handler = new CreatePurchaseWithCashCommandHandler(
            _unitOfWorkMock.Object,
            _dateTimeProviderMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var command = new CreatePurchaseWithCashCommand(Guid.NewGuid());
        _orderRepositoryMock.Setup(u => u.GetByIdWithShipment(command.OrderId))
            .ReturnsAsync((Order?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"order with provided id {command.OrderId} not exists", result.ErrorMessage);
    }

    [Theory]
    [InlineData(OrderStatus.Canceled)]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Paid)]
    [InlineData(OrderStatus.Sent)]
    public async Task Handle_ShouldReturnFailure_WhenOrderStatusIsNotSubmitted(OrderStatus orderStatus)
    {
        // Arrange
        var order = new Order { Id = Guid.NewGuid(), OrderStatus = orderStatus, OrderNumber = "order number" };
        var command = new CreatePurchaseWithCashCommand(order.Id);

        _orderRepositoryMock.Setup(u => u.GetByIdWithShipment(command.OrderId))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Cannot create checkout session for order with status {orderStatus}", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPurchaseIsCreatedSuccessfully()
    {
        // Arrange
        var shipmentPrice = 9m;
        var orderPrice = 100m;
        var totalPrice = shipmentPrice + orderPrice;
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            LastName = "LastName",
            FirstName = "FirstName",
            Email = "Email",
            Country = "Country",
            City = "City",
            AreaCode = "AreaCode",
            PhoneNumber = "PhoneNumber",
            PostalCode = "PostalCode",
            Street = "Street",
            TrackingLink = "Street",
            HomeNumber = 1,
            ShipmentPrice = shipmentPrice,
        };
        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderStatus = OrderStatus.Submitted,
            OrderPrice = orderPrice,
            Shipment = shipment,
            OrderNumber = "order number",
        };
        var command = new CreatePurchaseWithCashCommand(order.Id);
        var purchase = new Purchase
        {
            Id = Guid.NewGuid(),
            Order = order,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            Amount = totalPrice,
            PurchaseTimestamp = DateTime.UtcNow
        };
        var purchaseResult = new PurchaseResult();

        _orderRepositoryMock.Setup(u => u.GetByIdWithShipment(command.OrderId))
            .ReturnsAsync(order);
        var now = DateTime.UtcNow;
        _dateTimeProviderMock.Setup(d => d.GetCurrentTime()).Returns(now);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        _purchaseRepositoryMock.Verify(u => u.CreateAsync(It.IsAny<Purchase>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        Purchase createdPurchase = (Purchase)_purchaseRepositoryMock.Invocations.First(i => i.Method.Name == nameof(IPurchaseRepository.CreateAsync)).Arguments[0];
        Assert.Equal(now, createdPurchase.PurchaseTimestamp);
        Assert.Equal(totalPrice, createdPurchase.Amount);
        Assert.Equal(PaymentMethod.CashOnDelivery, createdPurchase.PaymentMethod);
    }
}
