using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public record CreateCartRequest
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<CartItem> Items { get; set; } = [];
}