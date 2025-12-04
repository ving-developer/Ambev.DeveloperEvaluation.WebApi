using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

public record CancelCartCommand : IRequest<Unit>
{
    public Guid CartId { get; init; }
    public string Reason { get; init; } = string.Empty;

    public CancelCartCommand(Guid cartId, string reason)
    {
        CartId = cartId;
        Reason = reason;
    }
}