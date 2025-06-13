using Domain.Entities;

namespace Domain.Contracts
{
    public interface IProductRepository
    {
        IQueryable<Product> Get();
        void Add(Product product);
    }
}
