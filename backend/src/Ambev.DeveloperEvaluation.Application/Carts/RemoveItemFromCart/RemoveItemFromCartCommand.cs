using Ambev.DeveloperEvaluation.Application.Carts.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItemFromCart;

public record RemoveItemFromCartCommand : IRequest<CartResult>
{
    public Guid CartId { get; init; }
    public Guid ItemId { get; init; }

    public RemoveItemFromCartCommand(Guid cartId, Guid itemId)
    {
        CartId = cartId;
        ItemId = itemId;
    }
}