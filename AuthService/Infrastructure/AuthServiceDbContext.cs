using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AuthServiceDbContext : DbContext
    {
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthServiceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
