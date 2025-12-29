using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.UpdateItemQuantity;

public record UpdateItemQuantityCommand : IRequest<CartResult>
{
    public Guid CartId { get; init; }
    public Guid ItemId { get; init; }
    public int Quantity { get; init; }

    public UpdateItemQuantityCommand(Guid cartId, Guid itemId, int quantity)
    {
        CartId = cartId;
        ItemId = itemId;
        Quantity = quantity;
    }
}