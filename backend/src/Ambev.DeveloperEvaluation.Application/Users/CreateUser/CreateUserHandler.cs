using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        ILogger<CreateUserHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateUserCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<UserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting CreateUserHandler execution");

        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            _logger.LogWarning("[WRN] User with Email={Email} already exists. Aborting CreateUser.", command.Email);
            throw new DomainException($"User with email {command.Email} already exists");
        }

        command.Password = _passwordHasher.HashPassword(command.Password);
        var user = _mapper.Map<User>(command);

        _logger.LogInformation("[INF] Saving user into repository");

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        
        await _userRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("[INF] User (UserId={UserId}) created successfully", createdUser.Id);

        return _mapper.Map<UserResult>(createdUser);
    }
}
