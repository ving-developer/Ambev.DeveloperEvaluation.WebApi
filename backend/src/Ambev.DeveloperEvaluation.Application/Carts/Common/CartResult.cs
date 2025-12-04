namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public record CartResult
{
    public Guid Id { get; set; }

    public string SaleNumber { get; set; } = string.Empty;

    public DateTime SaleDate { get; set; }

    public Guid CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public bool IsCanceled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CanceledAt { get; set; }

    public string? CancellationReason { get; set; }

    public List<CartItemCommand> Items { get; set; } = new();
}
