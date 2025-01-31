using Application.Interfaces;
using Application.Requests.Baskets.GetBasketById;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace UnitTests.Requests.Handlers;

public class GetBasketByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetBasketByIdQueryHandler _handler;

    public GetBasketByIdQueryHandlerTests()
    {
        _unitOfWorkMock.Setup(m => m.BasketRepository).Returns(_basketRepositoryMock.Object);
        _handler = new GetBasketByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task BasketNotFound_ReturnsNotFound()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var command = new GetBasketByIdQuery(basketId);
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
    public async Task BasketFound_ReturnsSuccess()
    {
        // Arrange
        var basketId = Guid.NewGuid();
        var command = new GetBasketByIdQuery(basketId);

        var basketFromRepo = new Basket { Id = basketId, ProductsInBaskets = [], IsActive = true };
        _basketRepositoryMock.Setup(m => m.GetByIdWithProducts(basketId))
            .ReturnsAsync(basketFromRepo)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _basketRepositoryMock.Verify();
    }
}
