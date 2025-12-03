using Ambev.DeveloperEvaluation.Application.Carts.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public record CreateCartCommand : IRequest<CartResult>
{
    public Guid CustomerId { get; init; }
    public Guid BranchId { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public List<CartItemCommand> Items { get; init; } = new();
}