using Application.Interfaces;
using Application.Requests.Baskets.CreateOrGetBasket;
using Application.Requests.Baskets;
using AutoMapper;
using Domain.Entities;
using Moq;
using Common.Application.Interfaces;

namespace UnitTests.Requests.Handlers;
public class CreateOrGetBasketCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IIdentityService> _identityServiceMock = new();
    private readonly CreateOrGetBasketCommandHandler _handler;

    public CreateOrGetBasketCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.BasketRepository).Returns(_basketRepositoryMock.Object);
        _handler = new CreateOrGetBasketCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _identityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_UserHasExistingActiveBasket_ReturnsMappedBasket()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateOrGetBasketCommand();
        var existingBasket = new Basket { Id = Guid.NewGuid(), UserId = userId, ProductsInBaskets = [], IsActive = true };
        var expectedBasketResult = new BasketResult();

        _basketRepositoryMock.Setup(m => m.GetActiveByUserIdWithProducts(userId))
            .ReturnsAsync(existingBasket);

        _identityServiceMock.Setup(m => m.TryGetUserId())
            .Returns(userId)
            .Verifiable();

        _mapperMock.Setup(m => m.Map<BasketResult>(existingBasket))
            .Returns(expectedBasketResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedBasketResult, result);
        _basketRepositoryMock.Verify(m => m.GetActiveByUserIdWithProducts(userId), Times.Once);
        _identityServiceMock.Verify();
    }

    [Fact]
    public async Task Handle_UserHasNoExistingBasket_CreatesNewBasket()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateOrGetBasketCommand();
        Basket? createdBasket = null;
        var expectedBasketResult = new BasketResult();

        _basketRepositoryMock.Setup(m => m.GetActiveByUserIdWithProducts(userId))
            .ReturnsAsync((Basket?)null);
        
        _basketRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<Basket>()))
            .Callback<Basket>(b => createdBasket = b)!.ReturnsAsync(createdBasket);

        _identityServiceMock.Setup(m => m.TryGetUserId())
            .Returns(userId)
            .Verifiable();

        _unitOfWorkMock.Setup(m => m.SaveChangesAsync());

        _mapperMock.Setup(m => m.Map<BasketResult>(It.IsAny<Basket>())).Returns(expectedBasketResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(createdBasket);
        Assert.Equal(userId, createdBasket.UserId);
        Assert.Empty(createdBasket.ProductsInBaskets);
        Assert.Equal(expectedBasketResult, result);

        _basketRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Basket>()), Times.Once);
        _unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);
        _identityServiceMock.Verify();
    }

    [Fact]
    public async Task Handle_NoUserId_CreatesNewBasket()
    {
        // Arrange
        var command = new CreateOrGetBasketCommand();
        Basket? createdBasket = null;
        var expectedBasketResult = new BasketResult();

        _basketRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<Basket>()))
            .Callback<Basket>(b => createdBasket = b)!.ReturnsAsync(createdBasket);

        _identityServiceMock.Setup(m => m.TryGetUserId())
            .Returns((Guid?)null)
            .Verifiable();

        _unitOfWorkMock.Setup(m => m.SaveChangesAsync());

        _mapperMock.Setup(m => m.Map<BasketResult>(It.IsAny<Basket>())).Returns(expectedBasketResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(createdBasket);
        Assert.Null(createdBasket.UserId);
        Assert.Empty(createdBasket.ProductsInBaskets);
        Assert.True(createdBasket.IsActive);
        Assert.Equal(expectedBasketResult, result);

        _basketRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Basket>()), Times.Once);
        _unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);
        _identityServiceMock.Verify();
    }
}
