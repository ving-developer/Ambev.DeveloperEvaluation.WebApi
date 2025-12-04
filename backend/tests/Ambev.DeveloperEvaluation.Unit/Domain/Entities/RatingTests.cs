using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Rating entity.
/// </summary>
public class RatingTests
{
    [Fact(DisplayName = "Given valid rate and count, when creating Rating, then it should initialize correctly")]
    public void Given_ValidRateAndCount_When_CreatingRating_Then_ShouldInitialize()
    {
        // Given
        decimal rate = 4.5m;
        int count = 10;

        // When
        var rating = new Rating(rate, count);

        // Then
        rating.Rate.Should().Be(rate);
        rating.Count.Should().Be(count);
    }

    [Theory(DisplayName = "Given different rates and counts, when creating Rating, then properties should match")]
    [InlineData(0.0, 0)]
    [InlineData(5.0, 1)]
    [InlineData(3.7, 15)]
    public void Given_VariousRateAndCount_When_CreatingRating_Then_ShouldMatchProperties(decimal rate, int count)
    {
        // Given & When
        var rating = new Rating(rate, count);

        // Then
        rating.Rate.Should().Be(rate);
        rating.Count.Should().Be(count);
    }

    [Theory(DisplayName = "Given invalid rate, when creating Rating, then it should throw")]
    [InlineData(-1)]
    [InlineData(6)]
    public void Given_InvalidRate_When_CreatingRating_Then_ShouldThrow(decimal invalidRate)
    {
        // Given
        int count = 1;

        // When
        Action act = () => new Rating(invalidRate, count);

        // Then
        act.Should().Throw<DomainException>();
    }

    [Theory(DisplayName = "Given invalid count, when creating Rating, then it should throw")]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Given_InvalidCount_When_CreatingRating_Then_ShouldThrow(int invalidCount)
    {
        // Given
        decimal rate = 4.0m;

        // When
        Action act = () => new Rating(rate, invalidCount);

        // Then
        act.Should().Throw<DomainException>();
    }
}
