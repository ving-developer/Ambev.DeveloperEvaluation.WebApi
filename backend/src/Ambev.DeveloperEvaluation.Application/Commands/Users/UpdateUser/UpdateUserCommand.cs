using Ambev.DeveloperEvaluation.Application.Common.Users;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Users.UpdateUser;

/// <summary>
/// Command for updating an existing user.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for updating a user,
/// including username, phone number, email, status, and role.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UserResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="UpdateUserValidator"/> which extends 
/// populated and follow the required rules.
/// 
/// Note: Password update should be handled separately in a dedicated command
/// for security reasons.
/// </remarks>
public record UpdateUserCommand : IRequest<UserResult>
{
    /// <summary>
    /// The unique identifier of the user to update
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// The phone number for the user
    /// </summary>
    public string Phone { get; init; } = string.Empty;

    /// <summary>
    /// The email address for the user (must be unique)
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// The status of the user
    /// </summary>
    public UserStatus Status { get; init; }

    /// <summary>
    /// The role of the user
    /// </summary>
    public UserRole Role { get; init; }

    /// <summary>
    /// Initializes a new instance of the UpdateUserCommand
    /// </summary>
    public UpdateUserCommand(
        Guid id,
        string username,
        string phone,
        string email,
        UserStatus status,
        UserRole role)
    {
        Id = id;
        Username = username;
        Phone = phone;
        Email = email;
        Status = status;
        Role = role;
    }
}