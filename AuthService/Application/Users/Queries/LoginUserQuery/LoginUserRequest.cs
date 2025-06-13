using System.ComponentModel.DataAnnotations;

namespace Application.Users.Queries.LoginUserQuery
{
    public record LoginUserRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);
}
