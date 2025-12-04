namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public record CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
