using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.SaleNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("SaleNumber");

        builder.HasIndex(c => c.SaleNumber)
            .IsUnique()
            .HasDatabaseName("IX_Carts_SaleNumber");

        builder.Property(c => c.SaleDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(c => c.CustomerId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(c => c.BranchId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(c => c.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(c => c.Status)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(20);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(c => c.CanceledAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(c => c.CancellationReason)
            .HasMaxLength(500)
            .HasColumnName("CancellationReason");

        builder.HasOne(c => c.Branch)
            .WithMany(b => b.Sales)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(c => c.Items)
            .WithOne(i => i.Cart)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Ignore(c => c.Subtotal);
        builder.Ignore(c => c.TotalDiscount);
        builder.Ignore(c => c.TotalItemCount);
        builder.Ignore(c => c.UniqueProductIds);

        builder.Metadata.FindNavigation(nameof(Cart.Items))!
            .SetField("_items");

        builder.HasIndex(c => c.CustomerId)
            .HasDatabaseName("IX_Carts_CustomerId");

        builder.HasIndex(c => c.BranchId)
            .HasDatabaseName("IX_Carts_BranchId");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_Carts_Status");

        builder.HasIndex(c => c.SaleDate)
            .HasDatabaseName("IX_Carts_SaleDate");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Carts_CreatedAt");

        builder.HasIndex(c => c.CanceledAt)
            .HasDatabaseName("IX_Carts_CanceledAt");

        builder.HasIndex(c => new { c.BranchId, c.Status })
            .HasDatabaseName("IX_Carts_BranchId_Status");

        builder.HasIndex(c => new { c.CustomerId, c.CreatedAt })
            .HasDatabaseName("IX_Carts_CustomerId_CreatedAt");

        builder.HasIndex(c => new { c.Status, c.CreatedAt })
            .HasDatabaseName("IX_Carts_Status_CreatedAt");

        builder.HasIndex(c => new { c.BranchId, c.SaleDate })
            .HasDatabaseName("IX_Carts_BranchId_SaleDate");
    }
}