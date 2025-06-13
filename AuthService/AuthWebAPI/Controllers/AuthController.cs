using Application.Users.Commands.RegisterUser;
using Application.Users.Queries.LoginUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController(IMediator mediator, ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid registration request: {@Request}", request);
                return ValidationProblem(ModelState);
            }

            var command = new RegisterUserCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            try
            {
                var result = await mediator.Send(command, cancellationToken);
                logger.LogInformation("User successfully registered: {Email}", request.Email);
                return CreatedAtAction(nameof(Register), new { email = request.Email }, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Registration failed for {Email}", request.Email);
                return Problem("An error occurred during registration.");
            }
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid login request: {@Request}", request);
                return ValidationProblem(ModelState);
            }

            var query = new LoginUserQuery(request.Email, request.Password);

            try
            {
                var token = await mediator.Send(query, cancellationToken);
                logger.LogInformation("User successfully logged in: {Email}", request.Email);
                return Ok(token);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Login failed for {Email}", request.Email);
                return Unauthorized("Invalid credentials.");
            }
        }
    }
}
