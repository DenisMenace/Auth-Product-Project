using Domain.Contracts;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthServiceDbContext _dbContext;
        public UserRepository(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(User user)
        {
            _dbContext.Set<User>().Add(user);
        }

        public IQueryable<User> Get()
        {
            return _dbContext.Users;
        }
    }
}
