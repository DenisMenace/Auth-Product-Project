using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : IRequest<string>;
}
