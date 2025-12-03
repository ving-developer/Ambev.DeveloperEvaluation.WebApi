namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItemToCart;

public record AddItemToCartRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}