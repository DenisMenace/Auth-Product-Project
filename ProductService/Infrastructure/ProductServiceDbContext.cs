using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ProductServiceDbContext : DbContext, IDbContext
    {
        public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
