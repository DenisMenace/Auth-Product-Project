using System.ComponentModel.DataAnnotations;

namespace Application.Users.Commands.RegisterUser
{
    public record RegisterUserRequest(
    [Required, EmailAddress] string Email,
    [Required] string FirstName,
    [Required] string LastName,
    [Required, MinLength(6)] string Password);
}
