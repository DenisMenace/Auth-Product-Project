using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Contracts
{
    public interface IDbContext : IDisposable
    {
        DbSet<Product> Products { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
