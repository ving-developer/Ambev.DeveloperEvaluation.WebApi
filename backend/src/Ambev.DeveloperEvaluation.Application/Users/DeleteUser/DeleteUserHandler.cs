using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="logger">The logger instance</param>
    public DeleteUserHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the DeleteUserCommand request
    /// </summary>
    /// <param name="request">The DeleteUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting DeleteUserHandler for UserId={UserId}", request.Id);

        await _userRepository.DeleteAsync(request.Id, cancellationToken);

        var success = await _userRepository.SaveChangesAsync(cancellationToken);

        if (!success)
        {
            _logger.LogWarning("[WRN] Delete failed. UserId={UserId} not found", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} not found");
        }

        _logger.LogInformation("[INF] User deleted successfully. UserId={UserId}", request.Id);

        return true;
    }
}
