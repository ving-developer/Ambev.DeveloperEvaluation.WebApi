namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateItemQuantity;

public record UpdateItemQuantityRequest
{
    public int Quantity { get; set; }
}