using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public static class UseRepositoryExtensions
    {
        public static async Task<bool> ExistsByEmail(this IUserRepository userRepository, string email)
        {
            return await userRepository.Get().AnyAsync(u => u.Email == email);
        }

        public static async Task<User?> GetByEmail(this IUserRepository userRepository, string email)
        {
            return await userRepository.Get().SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
