namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public record CartItemCommand
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
