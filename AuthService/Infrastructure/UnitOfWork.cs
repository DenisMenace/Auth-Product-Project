using Domain.Contracts;

namespace Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthServiceDbContext _dbContext;
        public UnitOfWork(IUserRepository userRepository, AuthServiceDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }
        public IUserRepository UserRepository => _userRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
