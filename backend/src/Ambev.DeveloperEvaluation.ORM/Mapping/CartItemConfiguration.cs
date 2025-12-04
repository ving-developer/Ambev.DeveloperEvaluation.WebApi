using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(ci => ci.CartId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(ci => ci.ProductId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.DiscountPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0m);

        builder.Property(ci => ci.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(ci => ci.CartId)
            .HasDatabaseName("IX_CartItems_CartId");

        builder.HasIndex(ci => ci.ProductId)
            .HasDatabaseName("IX_CartItems_ProductId");

        builder.HasIndex(ci => ci.CreatedAt)
            .HasDatabaseName("IX_CartItems_CreatedAt");

        builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_CartItems_CartId_ProductId");

        builder.HasIndex(ci => new { ci.CartId, ci.CreatedAt })
            .HasDatabaseName("IX_CartItems_CartId_CreatedAt");

        builder.Ignore(ci => ci.Subtotal);
        builder.Ignore(ci => ci.DiscountAmount);
        builder.Ignore(ci => ci.TotalPrice);
        builder.Ignore(ci => ci.UnitPrice);
    }
}