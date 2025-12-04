using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Products
{
    /// <summary>
    /// Provides methods for generating test data for <see cref="CreateProductRequest"/>.
    /// This class centralizes test data generation for consistency across tests.
    /// </summary>
    internal class CreateProductRequestTestData
    {
        /// <summary>
        /// Generates a valid CreateProductRequest entity with randomized data.
        /// </summary>
        public static CreateProductRequest GetValidCreateProductRequest()
        {
            var faker = new Faker<CreateProductRequest>()
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 1000)))
                .RuleFor(p => p.Category, f => f.PickRandom<ProductCategory>())
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Rating, f => new ProductRatingResponse
                {
                    Rate = f.Random.Decimal(0, 5),
                    Count = f.Random.Int(0, 1000)
                });

            return faker.Generate();
        }

        /// <summary>
        /// Generates invalid CreateProductRequest with empty or invalid fields.
        /// </summary>
        public static CreateProductRequest GetInvalidCreateProductRequest()
        {
            return new CreateProductRequest
            {
                Title = "",
                Description = "",
                Price = -10,
                Category = 0,
                Image = "",
                Rating = new ProductRatingResponse
                {
                    Rate = -1,
                    Count = -5
                }
            };
        }
    }
}
