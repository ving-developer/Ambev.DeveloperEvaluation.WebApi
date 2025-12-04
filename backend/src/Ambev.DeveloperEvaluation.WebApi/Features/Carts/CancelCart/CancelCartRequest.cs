namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CancelCart;

public record CancelCartRequest
{
    public string Reason { get; set; } = string.Empty;
}