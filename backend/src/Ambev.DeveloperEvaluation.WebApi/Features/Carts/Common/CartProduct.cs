namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public record CartProduct
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
