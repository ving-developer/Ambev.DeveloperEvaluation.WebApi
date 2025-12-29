using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Common.Carts;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for CreateCartHandler using the Bogus library.
/// </summary>
public static class CreateCartHandlerTestData
{
    private static readonly Faker<CreateCartCommand> createCartFaker = new Faker<CreateCartCommand>()
        .RuleFor(c => c.CustomerId, f => f.Random.Guid())
        .RuleFor(c => c.BranchId, f => f.Random.Guid())
        .RuleFor(c => c.Items, f =>
        [
            new CartItemCommand
            {
                ProductId = f.Random.Guid(),
                Quantity = f.Random.Int(1, 5)
            }
        ]);

    /// <summary>
    /// Generates a valid CreateCartCommand with random but valid data.
    /// </summary>
    /// <returns>A valid CreateCartCommand instance.</returns>
    public static CreateCartCommand GenerateValidCommand()
    {
        return createCartFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateCartCommand with multiple items.
    /// Useful for testing carts with more than one product.
    /// </summary>
    /// <param name="itemCount">Number of items to generate.</param>
    /// <returns>A CreateCartCommand instance with multiple items.</returns>
    public static CreateCartCommand GenerateCommandWithMultipleItems(int itemCount = 3)
    {
        var faker = new Faker<CreateCartCommand>()
            .RuleFor(c => c.CustomerId, f => f.Random.Guid())
            .RuleFor(c => c.BranchId, f => f.Random.Guid())
            .RuleFor(c => c.Items, f =>
            {
                var items = new List<CartItemCommand>();
                for (int i = 0; i < itemCount; i++)
                {
                    items.Add(new CartItemCommand
                    {
                        ProductId = f.Random.Guid(),
                        Quantity = f.Random.Int(1, 5)
                    });
                }
                return items;
            });

        return faker.Generate();
    }
}
