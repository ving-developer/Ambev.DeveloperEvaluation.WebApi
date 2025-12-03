using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedNever();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("Name");

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Code");

        builder.HasIndex(b => b.Code)
            .IsUnique()
            .HasDatabaseName("IX_Branches_Code");

        builder.Property(b => b.City)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("City");

        builder.Property(b => b.State)
            .IsRequired()
            .HasMaxLength(2)
            .HasColumnName("State")
            .IsFixedLength();

        builder.HasIndex(b => b.State)
            .HasDatabaseName("IX_Branches_State");

        builder.HasIndex(b => b.City)
            .HasDatabaseName("IX_Branches_City");

        builder.HasIndex(b => new { b.State, b.City })
            .HasDatabaseName("IX_Branches_State_City");

        builder.HasMany(b => b.Sales)
            .WithOne(c => c.Branch)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}