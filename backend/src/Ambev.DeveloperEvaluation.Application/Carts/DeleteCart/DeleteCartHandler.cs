using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteCartHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="logger">The logger instance</param>
    public DeleteCartHandler(
        IUserRepository userRepository,
        ILogger<DeleteCartHandler> logger)
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
    public async Task<DeleteCartResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting DeleteUserHandler for UserId={UserId}", request.Id);

        var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);

        if (!success)
        {
            _logger.LogWarning("[WRN] Delete failed. UserId={UserId} not found", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} not found");
        }

        _logger.LogInformation("[INF] User deleted successfully. UserId={UserId}", request.Id);

        return new DeleteCartResponse { Success = true };
    }
}
