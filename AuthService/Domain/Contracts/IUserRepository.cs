using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUserRepository
    {
        IQueryable<User> Get();
        void Add(User user);
    }
}
