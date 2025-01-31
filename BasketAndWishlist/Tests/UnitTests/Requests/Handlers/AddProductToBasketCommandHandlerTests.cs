using Application.Interfaces;
using Application.Models;
using Application.Requests.Baskets.AddProductToBasket;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace UnitTests.Requests.Handlers;

public class AddProductToBasketCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICatalogProductsService> _catalogProductsServiceMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IProductInBasketRepository> _productInBasketRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly AddProductToBasketCommandHandler _handler;

    public AddProductToBasketCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.BasketRepository).Returns(_basketRepositoryMock.Object);
        _unitOfWorkMock.Setup(m => m.ProductInBasketRepository).Returns(_productInBasketRepositoryMock.Object);
        _handler = new AddProductToBasketCommandHandler(_unitOfWorkMock.Object, _catalogProductsServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task BasketNotFound_ReturnsNotFound()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new AddProductToBasketCommand(basketId, productInCatalogId);
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
    public async Task InactiveBasket_ReturnsNotFound()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new AddProductToBasketCommand(basketId, productInCatalogId);
        var expectedErrorMesage = $"Basket with provided id {basketId} is not active";

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [], IsActive = false };

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
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    public async Task ProductExistsInBasket_IncrementsPieceCount(int existingPieceCount, int expectedPieceCount)
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new AddProductToBasketCommand(basketId, productInCatalogId);

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [], IsActive = true };
        
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
    public async Task ProductNotInCatalog_ReturnsFailure()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new AddProductToBasketCommand(basketId, productInCatalogId);
        var expectedErrorMesage = $"Catalog product with provided id {productInCatalogId} not exists";

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [], IsActive = true };
        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        _catalogProductsServiceMock.Setup(c => c.GetCatalogProductById(productInCatalogId))
            .ReturnsAsync((CatalogProduct?)null)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(expectedErrorMesage, result.ErrorMessage);
        _basketRepositoryMock.Verify();
        _catalogProductsServiceMock.Verify();
    }

    [Fact]
    public async Task Handle_NewProductInBasket_AddsNewEntry()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var productInCatalogId = Guid.NewGuid();
        var command = new AddProductToBasketCommand(basketId, productInCatalogId);

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [], IsActive = true };
        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        var catalogProduct = new CatalogProduct()
        {
            Id = productInCatalogId,
            ProductId = Guid.NewGuid(),
            Price = 10,
            PhotoUrl = "photo url",
            SKU = "sku",
            Description = "catalog product description"
        };

        _catalogProductsServiceMock.Setup(c => c.GetCatalogProductById(productInCatalogId))
            .ReturnsAsync(catalogProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _productInBasketRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<ProductInBasket>()), Times.Once);
        _basketRepositoryMock.Verify();
        _unitOfWorkMock.Verify(m => m.SaveChangesAsync(), Times.Once);

        var createdProductInBasket = (ProductInBasket) _productInBasketRepositoryMock.Invocations.Where(m => m.Method.Name == nameof(IProductInBasketRepository.CreateAsync)).First().Arguments[0];
        Assert.Equal(1, createdProductInBasket.PieceCount);
        Assert.Equal(productInCatalogId, createdProductInBasket.ProductInCatalogId);
        Assert.Equal(basketId, createdProductInBasket.BasketId);
    }
}
