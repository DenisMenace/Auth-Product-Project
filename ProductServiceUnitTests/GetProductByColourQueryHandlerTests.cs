using Application.Products.Queries.GetProductByColourQuery;
using Domain.Contracts;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

public class GetProductByColourQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<GetProductByColourQueryHandler>> _loggerMock;
    private readonly GetProductByColourQueryHandler _handler;

    public GetProductByColourQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetProductByColourQueryHandler>>();

        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);

        _handler = new GetProductByColourQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidColour_ReturnsMatchingProducts()
    {
        // Arrange
        var query = new GetProductByColourQuery("Red");
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product A", Colour = "Red" },
            new Product { Id = Guid.NewGuid(), Name = "Product B", Colour = "Red" }
        };

        _productRepositoryMock
            .Setup(repo => repo.GetByColourAsync("Red"))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.Colour == "Red");
    }

    [Fact]
    public async Task Handle_WithValidColourButNoMatches_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetProductByColourQuery("Green");

        _productRepositoryMock
            .Setup(repo => repo.GetByColourAsync("Green"))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
