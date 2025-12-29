using Ambev.DeveloperEvaluation.Application.Common.Carts;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;

public record CreateCartCommand : IRequest<CartResult>
{
    public Guid CustomerId { get; init; }
    public Guid BranchId { get; init; }
    public List<CartItemCommand> Items { get; init; } = [];
}