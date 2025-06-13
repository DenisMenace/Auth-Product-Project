using Domain.Entities;

namespace Application.Contracts
{
    public interface ITokenProvider
    {
        string Create(User user);
    }
}
