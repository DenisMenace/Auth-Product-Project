using MediatR;

namespace Application.Users.Queries.LoginUserQuery
{
    public sealed record LoginUserQuery(string Email, string Password) : IRequest<string>;
}
