using Application.Interfaces;
using Application.Models;
using Application.Services;
using Moq;

namespace UnitTests.ServicesTests;

public class OrderPriceServiceTests
{
    private readonly Mock<ICatalogService> _catalogServiceMock = new();
    private readonly OrderPriceService _orderPriceService;

    public OrderPriceServiceTests()
    {
        _orderPriceService = new OrderPriceService(_catalogServiceMock.Object);
    }

    [Fact]
    public async Task GetOrderPrice_ShouldReturnCorrectTotalPrice_WhenProductsExistInCatalog()
    {
        // Arrange
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        var basket = new Basket
        {
            ProductsInBaskets =
            [
                new() { ProductInCatalogId = productId1, PieceCount = 2 },
                new() { ProductInCatalogId = productId2, PieceCount = 3 }
            ]
        };

        var catalogProducts = new List<CatalogProduct>
        {
            new() { Id = productId1, Price = 10m },
            new() { Id = productId2, Price = 20m }
        };

        _catalogServiceMock.Setup(s => s.GetCatalogProductsByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(catalogProducts);

        // Act
        var totalPrice = await _orderPriceService.GetOrderPrice(basket);

        // Assert
        Assert.Equal(2 * 10 + 3 * 20, totalPrice);
    }

    [Fact]
    public async Task GetOrderPrice_ShouldReturnZero_WhenBasketIsEmpty()
    {
        // Arrange
        var basket = new Basket { ProductsInBaskets = new List<ProductInBasket>() };
        _catalogServiceMock.Setup(s => s.GetCatalogProductsByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync([]);

        // Act
        var totalPrice = await _orderPriceService.GetOrderPrice(basket);

        // Assert
        Assert.Equal(0, totalPrice);
    }

    [Fact]
    public async Task GetOrderPrice_ShouldThrowException_WhenProductNotFoundInCatalog()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var basket = new Basket
        {
            ProductsInBaskets =
            [
                new() { ProductInCatalogId = productId, PieceCount = 2 }
            ]
        };

        var catalogProducts = new List<CatalogProduct>(); // Empty catalog response

        _catalogServiceMock.Setup(s => s.GetCatalogProductsByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(catalogProducts);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _orderPriceService.GetOrderPrice(basket));
    }
}
