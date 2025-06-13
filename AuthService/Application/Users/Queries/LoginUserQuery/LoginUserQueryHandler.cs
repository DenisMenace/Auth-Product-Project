using Application.Contracts;
using Application.Extensions;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.LoginUserQuery
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<LoginUserQueryHandler> _logger;
        public LoginUserQueryHandler(
            IUnitOfWork unitOfWork,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher,
            ILogger<LoginUserQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _unitOfWork.UserRepository.GetByEmail(request.Email);            

            if (user is null)
            {
                _logger.LogWarning("Login failed: User with email {Email} not found.", request.Email);
                throw new Exception("The user was not found");
            }

            bool verified = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                throw new Exception("The password is incorrect");
            }

            string token = _tokenProvider.Create(user);

            _logger.LogInformation("User email {.Email} successfully logged in.", request.Email);

            return token;
        }
    }
}
