using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Commands.Auth.AuthenticateUser;

/// <summary>
/// Handler responsible for processing <see cref="AuthenticateUserCommand"/> requests.
/// Performs user authentication, password validation, active user verification,
/// and JWT token generation.
/// </summary>
public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthenticateUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="AuthenticateUserHandler"/>.
    /// </summary>
    /// <param name="userRepository">Repository used to retrieve user information.</param>
    /// <param name="passwordHasher">Service responsible for verifying password hashes.</param>
    /// <param name="jwtTokenGenerator">Service responsible for generating JWT access tokens.</param>
    /// <param name="logger">Logger instance for tracing authentication flow.</param>
    public AuthenticateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthenticateUserHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the user authentication workflow.
    /// </summary>
    /// <param name="request">The <see cref="AuthenticateUserCommand"/> containing login credentials.</param>
    /// <param name="cancellationToken">Propagation token for request cancellation.</param>
    /// <returns>
    /// Returns an <see cref="AuthenticateUserResult"/> containing the JWT token and basic user data
    /// when the credentials are valid.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the email is not found, the password is invalid,
    /// or the user does not satisfy the required specifications to authenticate.
    /// </exception>
    public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting authentication for Email={Email}", request.Email);

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            _logger.LogWarning("[WRN] Authentication failed. User not found. Email={Email}", request.Email);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var activeUserSpec = new ActiveUserSpecification();
        if (!activeUserSpec.IsSatisfiedBy(user))
        {
            _logger.LogWarning("[WRN] Authentication blocked. User is not active. Email={Email}", request.Email);
            throw new UnauthorizedAccessException("User is not active");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation("[INF] Authentication successful for Email={Email}", request.Email);

        return new AuthenticateUserResult
        {
            Token = token,
            Email = user.Email,
            Name = user.Username,
            Role = user.Role.ToString()
        };
    }
}
