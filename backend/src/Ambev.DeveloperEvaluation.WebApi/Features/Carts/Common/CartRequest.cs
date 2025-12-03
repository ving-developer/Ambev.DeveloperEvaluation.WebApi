namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public record CartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new();
}