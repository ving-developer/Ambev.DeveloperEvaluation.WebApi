using Ambev.DeveloperEvaluation.Application.Queries.Product.SearchProducts;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products
{
    /// <summary>
    /// Provides methods for generating test data for the ListProductsHandler.
    /// Centralizes creation of valid ListProductsCommand instances.
    /// </summary>
    public static class ListProductsHandlerTestData
    {
        private static readonly Faker Faker = new Faker();

        /// <summary>
        /// Generates a valid ListProductsCommand with randomized pagination and ordering.
        /// </summary>
        /// <returns>A valid ListProductsCommand instance.</returns>
        public static SearchProductsQuery GenerateValidCommand()
        {
            return new SearchProductsQuery
            {
                Page = Faker.Random.Int(1, 5),
                PageSize = Faker.Random.Int(1, 50),
                OrderBy = Faker.Random.Bool() ? "Title" : "Price"
            };
        }

        /// <summary>
        /// Generates a ListProductsCommand with empty pagination values (defaults).
        /// Useful for testing defaults and empty input scenarios.
        /// </summary>
        /// <returns>A ListProductsCommand instance with default values.</returns>
        public static SearchProductsQuery GenerateDefaultCommand()
        {
            return new SearchProductsQuery
            {
                Page = 1,
                PageSize = 10,
                OrderBy = "Title"
            };
        }
    }
}
