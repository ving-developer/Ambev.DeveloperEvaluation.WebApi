using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItemFromCart;

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