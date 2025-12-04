using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Product entity.
/// </summary>
public class ProductTests
{
    [Fact(DisplayName = "Given a new Product, when instantiated, then properties should be initialized correctly")]
    public void Given_NewProduct_When_Instantiated_Then_PropertiesShouldBeInitialized()
    {
        // Given & When
        var product = new Product();

        // Then
        product.Title.Should().BeEmpty();
        product.Description.Should().BeEmpty();
        product.Image.Should().BeEmpty();
        product.Price.Should().Be(0);
        product.Category.Should().Be(default(ProductCategory));
        product.Rating.Should().BeNull();
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        product.UpdatedAt.Should().BeNull();
    }

    [Fact(DisplayName = "Given a Product, when setting Rating, then it should assign correctly")]
    public void Given_Product_When_SettingRating_Then_RatingShouldBeAssigned()
    {
        // Given
        var product = new Product();
        var rating = new Rating(4.5m, 10);

        // When
        var ratingProperty = typeof(Product).GetProperty("Rating");
        ratingProperty!.SetValue(product, rating);

        // Then
        product.Rating.Should().NotBeNull();
        product.Rating.Should().Be(rating);
    }

    [Theory(DisplayName = "Given various titles and descriptions, when Product is instantiated, then strings should match")]
    [InlineData("Product 1", "Description 1")]
    [InlineData("Product 2", "Description 2")]
    [InlineData("", "")]
    public void Given_TitlesAndDescriptions_When_SettingProperties_Then_ShouldMatch(string title, string description)
    {
        // Given
        var product = new Product();

        // When
        var titleProp = typeof(Product).GetProperty("Title");
        var descProp = typeof(Product).GetProperty("Description");
        titleProp!.SetValue(product, title);
        descProp!.SetValue(product, description);

        // Then
        product.Title.Should().Be(title);
        product.Description.Should().Be(description);
    }
}
