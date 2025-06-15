using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _dbContext.Products.Add(product);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<List<Product>> GetByColourAsync(string colour)
        {
            if (string.IsNullOrWhiteSpace(colour))
            {
                throw new ArgumentException("Colour must be provided", nameof(colour));
            }

            return await _dbContext.Products.Where(p => p.Colour == colour).ToListAsync();
        }
    }
}