using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.SearchCarts;

public record SearchCartsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public CartStatus? Status { get; set; }
}