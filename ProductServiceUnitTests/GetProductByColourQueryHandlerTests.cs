using Application.Products.Queries.GetProductByColourQuery;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class GetProductByColourQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<GetProductByColourQueryHandler>> _loggerMock;
    private readonly FakeDbContext _dbContext;
    private readonly GetProductByColourQueryHandler _handler;

    public GetProductByColourQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetProductByColourQueryHandler>>();

        var options = new DbContextOptionsBuilder<FakeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new FakeDbContext(options);
        var fakeRepo = new FakeProductRepository(_dbContext);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.ProductRepository).Returns(fakeRepo);

        _handler = new GetProductByColourQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidColour_ReturnsMatchingProducts()
    {
        // Arrange
        _dbContext.Products.AddRange(
            new Product { Id = Guid.NewGuid(), Name = "Product A", Colour = "Red" },
            new Product { Id = Guid.NewGuid(), Name = "Product B", Colour = "Red" },
            new Product { Id = Guid.NewGuid(), Name = "Product C", Colour = "Blue" }
        );
        await _dbContext.SaveChangesAsync();

        var query = new GetProductByColourQuery( "Red" );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Equal("Red", p.Colour));
    }

    [Fact]
    public async Task Handle_WithValidColourButNoMatches_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetProductByColourQuery( "Green" );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task Handle_WithInvalidColour_ThrowsArgumentException(string colour)
    {
        // Arrange
        var query = new GetProductByColourQuery( colour );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(query, CancellationToken.None));
    }
}

public class FakeDbContext : DbContext
{
    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}

public class FakeProductRepository : IProductRepository
{
    private readonly FakeDbContext _context;

    public FakeProductRepository(FakeDbContext context)
    {
        _context = context;
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
    }

    public IQueryable<Product> Get()
    {
        return _context.Products;//.AsQueryable();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

