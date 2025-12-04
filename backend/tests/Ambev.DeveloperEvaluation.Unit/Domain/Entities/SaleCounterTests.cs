using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the SaleCounter entity.
/// </summary>
public class SaleCounterTests
{
    #region Constructor Tests

    [Fact(DisplayName = "Given valid branchId, when creating SaleCounter, then it should initialize correctly")]
    public void Given_ValidBranchId_When_CreatingSaleCounter_Then_ShouldInitialize()
    {
        // Given
        var branchId = Guid.NewGuid();

        // When
        var counter = new SaleCounter(branchId);

        // Then
        counter.BranchId.Should().Be(branchId);
        counter.LastNumber.Should().Be(1);
    }

    [Fact(DisplayName = "Given empty branchId, when creating SaleCounter, then it should throw ArgumentException")]
    public void Given_EmptyBranchId_When_CreatingSaleCounter_Then_ShouldThrow()
    {
        // Given
        var emptyBranchId = Guid.Empty;

        // When
        Action act = () => new SaleCounter(emptyBranchId);

        // Then
        act.Should().Throw<ArgumentException>()
            .WithMessage("Branch ID cannot be empty.*");
    }

    #endregion

    #region GetNextSaleNumber Tests

    [Fact(DisplayName = "Given a SaleCounter, when getting next sale number, then it should increment LastNumber")]
    public void Given_SaleCounter_When_GetNextSaleNumber_Then_LastNumberShouldIncrement()
    {
        // Given
        var branchId = Guid.NewGuid();
        var counter = new SaleCounter(branchId);
        var initialNumber = counter.LastNumber;

        // When
        var nextNumber = counter.GetNextSaleNumber();

        // Then
        nextNumber.Should().Be(initialNumber + 1);
        counter.LastNumber.Should().Be(initialNumber + 1);
    }

    [Fact(DisplayName = "Given multiple calls to GetNextSaleNumber, then it should increment sequentially")]
    public void Given_SaleCounter_When_GetNextSaleNumberMultipleTimes_Then_ShouldIncrementSequentially()
    {
        // Given
        var branchId = Guid.NewGuid();
        var counter = new SaleCounter(branchId);

        // When
        var first = counter.GetNextSaleNumber();
        var second = counter.GetNextSaleNumber();
        var third = counter.GetNextSaleNumber();

        // Then
        first.Should().Be(2);
        second.Should().Be(3);
        third.Should().Be(4);
        counter.LastNumber.Should().Be(4);
    }

    #endregion
}
