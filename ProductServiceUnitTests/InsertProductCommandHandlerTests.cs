using Application.Products.Commands.InsertProduct;
using Domain.Contracts;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class InsertProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<InsertProductCommandHandler>> _loggerMock;
    private readonly InsertProductCommandHandler _handler;
    private readonly InsertProductCommandValidator _validator;

    public InsertProductCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<InsertProductCommandHandler>>();
        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(_productRepositoryMock.Object);

        _handler = new InsertProductCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        _validator = new InsertProductCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccessMessage()
    {
        // Arrange
        var command = new InsertProductCommand("Test Product", "Red");
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Product successfully created.");
        _productRepositoryMock.Verify(r => r.Add(It.Is<Product>(
            p => p.Name == "Test Product" && p.Colour == "Red")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesReturnsZero_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new InsertProductCommand("Test Product", "Red");
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to create product. No changes were saved to the database.");

        _productRepositoryMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
