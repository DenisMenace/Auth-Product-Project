using Application.Products.Commands.InsertProduct;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

public class InsertProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<InsertProductCommandHandler>> _loggerMock;
    private readonly InsertProductCommandHandler _handler;

    public InsertProductCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<InsertProductCommandHandler>>();

        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);

        _handler = new InsertProductCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccessMessage()
    {
        // Arrange
        var command = new InsertProductCommand("Test Product","Red");
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Product successfully created.", result);
        _productRepositoryMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null, "Red")]
    [InlineData("Test Product", null)]
    [InlineData("", "Red")]
    [InlineData("Test Product", "")]
    public async Task Handle_InvalidInput_ThrowsArgumentException(string name, string colour)
    {
        // Arrange
        var command = new InsertProductCommand( name, colour );

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Product name or colour must not be null or empty", ex.Message);
        _productRepositoryMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesReturnsZero_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new InsertProductCommand("Test Product", "Red" );
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Failed to create product. No changes were saved to the database.", ex.Message);
        _productRepositoryMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
