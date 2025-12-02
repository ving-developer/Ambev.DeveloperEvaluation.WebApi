using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler for processing UpdateUserCommand requests
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UpdateUserHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="command">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated user details</returns>
    public async Task<UserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting UpdateUserHandler execution for UserId={UserId}", command.Id);

        var existingUser = await _userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (existingUser == null)
        {
            _logger.LogWarning("[WRN] User with UserId={UserId} not found. Aborting UpdateUser.", command.Id);
            throw new KeyNotFoundException($"User with ID {command.Id} not found");
        }

        if (existingUser.Email != command.Email)
        {
            var userWithSameEmail = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (userWithSameEmail != null)
            {
                _logger.LogWarning("[WRN] Email={Email} already in use by another user. Aborting UpdateUser.", command.Email);
                throw new InvalidOperationException($"Email {command.Email} is already in use by another user");
            }
        }

        _logger.LogInformation("[INF] Updating user properties for UserId={UserId}", command.Id);

        _mapper.Map(command, existingUser);

        var updatedUser = await _userRepository.UpdateAsync(existingUser, cancellationToken);

        _logger.LogInformation("[INF] User (UserId={UserId}) updated successfully", command.Id);

        return _mapper.Map<UserResult>(updatedUser);
    }
}