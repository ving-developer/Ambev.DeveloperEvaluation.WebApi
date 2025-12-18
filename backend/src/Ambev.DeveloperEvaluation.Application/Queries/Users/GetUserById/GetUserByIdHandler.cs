using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Queries.Users.GetUserById;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserByIdHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetUserByIdHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserByIdHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetUserCommand request
    /// </summary>
    /// <param name="request">The GetUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    public async Task<UserResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[INF] Starting GetUserHandler for UserId={UserId}", request.Id);

        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("[WRN] User not found. UserId={UserId}", request.Id);
            throw new EntityNotFoundException(nameof(User), request.Id);
        }

        _logger.LogInformation("[INF] User retrieved successfully. UserId={UserId}", request.Id);

        return _mapper.Map<UserResult>(user);
    }
}
