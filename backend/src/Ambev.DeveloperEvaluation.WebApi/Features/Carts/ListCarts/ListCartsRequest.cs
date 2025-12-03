namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

public record ListCartsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}