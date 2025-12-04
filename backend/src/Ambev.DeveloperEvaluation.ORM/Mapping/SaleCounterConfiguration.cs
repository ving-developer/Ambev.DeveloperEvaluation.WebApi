using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleCounterConfiguration : IEntityTypeConfiguration<SaleCounter>
{
    public void Configure(EntityTypeBuilder<SaleCounter> builder)
    {
        builder.ToTable("SaleCounters");

        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(sc => sc.BranchId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(sc => sc.LastNumber)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(sc => sc.Branch)
            .WithOne()
            .HasForeignKey<SaleCounter>(sc => sc.BranchId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(sc => sc.BranchId)
            .IsUnique()
            .HasDatabaseName("IX_SaleCounters_BranchId_Unique");

        builder.HasIndex(sc => sc.LastNumber)
            .HasDatabaseName("IX_SaleCounters_LastNumber");
    }
}