using Application.Interfaces;
using Application.Requests.Baskets.SubstractProductFromBasket;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace UnitTests.Requests.Handlers;

public class SubstractProductFromBasketCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IProductInBasketRepository> _productInBasketRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly SubstractProductFromBasketCommandHandler _handler;

    public SubstractProductFromBasketCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.BasketRepository).Returns(_basketRepositoryMock.Object);
        _unitOfWorkMock.Setup(m => m.ProductInBasketRepository).Returns(_productInBasketRepositoryMock.Object);
        _handler = new SubstractProductFromBasketCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task BasketNotFound_ReturnsNotFound()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new SubstractProductFromBasketCommand(basketId, productInCatalogId);
        var expectedErrorMesage = $"Basket with provided id {basketId} does not exists";

        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync((Basket?)null)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal(expectedErrorMesage, result.ErrorMessage);
        _basketRepositoryMock.Verify();
    }

    [Fact]
    public async Task ProductNotExistsInBasket_ShouldReturnFailure()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new SubstractProductFromBasketCommand(basketId, productInCatalogId);
        var expectedErrorMesage = $"Product with provided id {productInCatalogId} is not part of the busket";

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [] };

        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(expectedErrorMesage, result.ErrorMessage);
        _basketRepositoryMock.Verify();
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    public async Task ProductExistsInBasket_MoreThanOnePiece_SubsctractsPieceCount(int existingPieceCount, int expectedPieceCount)
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new SubstractProductFromBasketCommand(basketId, productInCatalogId);

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [] };

        var existingProduct = new ProductInBasket
        {
            Id = Guid.NewGuid(),
            ProductInCatalogId = productInCatalogId,
            PieceCount = existingPieceCount,
            Basket = basketFromRepo,
            BasketId = basketId,
        };
        basketFromRepo.ProductsInBaskets.Add(existingProduct);

        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPieceCount, existingProduct.PieceCount);
        _basketRepositoryMock.Verify();
        _productInBasketRepositoryMock.Verify(m => m.Update(existingProduct), Times.Once);
        _unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ProductExistsInBasket_OnePiece_ShouldRemoveIt()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new SubstractProductFromBasketCommand(basketId, productInCatalogId);

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [] };

        var existingProduct = new ProductInBasket
        {
            Id = Guid.NewGuid(),
            ProductInCatalogId = productInCatalogId,
            PieceCount = 1,
            Basket = basketFromRepo,
            BasketId = basketId,
        };
        basketFromRepo.ProductsInBaskets.Add(existingProduct);

        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _basketRepositoryMock.Verify();
        _productInBasketRepositoryMock.Verify(m => m.Remove(existingProduct), Times.Once);
        _unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);
    }
}
