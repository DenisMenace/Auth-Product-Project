using Domain.Entities;

namespace Domain.Contracts
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetByColourAsync(string colour);

        void Add(Product product);
    }
}
