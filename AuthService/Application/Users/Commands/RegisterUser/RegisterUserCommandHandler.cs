using Application.Contracts;
using Application.Extensions;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterUserCommandHandler> logger, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.UserRepository;

            if (await userRepository.ExistsByEmail(request.Email))
            {
                _logger.LogWarning("Registration failed: Email {Email} is already in use.", request.Email);
                throw new Exception("This email is already in use");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            userRepository.Add(user);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                _logger.LogError("User creation failed for {Email}. No changes were saved to the database.", request.Email);
                throw new InvalidOperationException("Failed to create user. No changes were saved to the database.");
            }

            _logger.LogInformation("User {Email} successfully registered with ID {UserId}.", request.Email, user.Id);

            return "User successfully created.";

        }
    }
}
