using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItemToCart;

public record AddItemToCartCommand : IRequest<CartResult>
{
    public Guid CartId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}