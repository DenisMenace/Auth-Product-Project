using Domain.Contracts;

namespace Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductServiceDbContext _dbContext;
        public UnitOfWork(IProductRepository productRepository, ProductServiceDbContext dbContext)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }
        public IProductRepository ProductRepository => _productRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
