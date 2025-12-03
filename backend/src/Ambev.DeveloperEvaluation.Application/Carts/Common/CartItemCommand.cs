namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public record CartItemCommand
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
