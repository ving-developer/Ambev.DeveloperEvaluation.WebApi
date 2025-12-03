using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public record CreateCartRequest
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public List<CartProduct> Items { get; set; } = new();
}