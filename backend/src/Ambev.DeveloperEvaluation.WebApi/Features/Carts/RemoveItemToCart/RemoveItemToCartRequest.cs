namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.RemoveItemToCart;

public record RemoveItemToCartRequest
{
    public Guid ProductId { get; set; }
    public Guid CartId { get; set; }
}