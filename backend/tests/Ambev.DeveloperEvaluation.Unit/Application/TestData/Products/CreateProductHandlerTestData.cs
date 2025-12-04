using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products
{
    /// <summary>
    /// Provides methods for generating test data for the CreateProductHandler.
    /// </summary>
    public static class CreateProductHandlerTestData
    {
        /// <summary>
        /// Generates a valid CreateProductCommand using Bogus.
        /// </summary>
        /// <returns>A valid CreateProductCommand instance.</returns>
        public static CreateProductCommand GenerateValidCommand()
        {
            var faker = new Faker();

            return new CreateProductCommand
            {
                Title = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Category = faker.PickRandom<ProductCategory>(),
                Price = decimal.Parse(faker.Commerce.Price(1, 1000)),
                Image = faker.Image.PicsumUrl(),
                Rating = new ProductRating
                {
                    Rate = faker.Random.Decimal(0, 5),
                    Count = faker.Random.Int(0, 1000)
                }
            };
        }
    }
}
