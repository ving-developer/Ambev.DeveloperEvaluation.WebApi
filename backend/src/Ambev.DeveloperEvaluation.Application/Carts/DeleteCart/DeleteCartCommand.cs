using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Command for deleting a user
/// </summary>
public record DeleteCartCommand : IRequest<DeleteCartResponse>
{
    /// <summary>
    /// The unique identifier of the user to delete
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of DeleteUserCommand
    /// </summary>
    /// <param name="id">The ID of the user to delete</param>
    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }
}
