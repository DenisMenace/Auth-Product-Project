using Domain.Contracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContext _dbContext;
        public ProductRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public IQueryable<Product> Get()
        {
            return _dbContext.Products;
        }
    }
}