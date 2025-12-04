using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for <see cref="Branch"/> using the Bogus library.
/// This class ensures consistent and maintainable test data generation across unit tests,
/// supporting both valid and invalid scenarios.
/// </summary>
public static class BranchTestData
{
    /// <summary>
    /// Faker configured to create valid <see cref="Branch"/> instances.
    /// All properties will be populated with acceptable and logically consistent values.
    /// </summary>
    private static readonly Faker<Branch> BranchFaker =
        new Faker<Branch>()
            .CustomInstantiator(f =>
                new Branch(
                    GenerateValidName(),
                    GenerateValidCode(),
                    GenerateValidCity(),
                    GenerateValidState()
                ));

    /// <summary>
    /// Generates a valid Branch entity using randomized data.
    /// </summary>
    /// <returns>A valid <see cref="Branch"/> instance.</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates a valid branch name.
    /// </summary>
    /// <returns>A valid branch name.</returns>
    public static string GenerateValidName()
    {
        return new Faker().Company.CompanyName();
    }

    /// <summary>
    /// Generates a valid alphanumeric branch code (3–10 characters).
    /// </summary>
    /// <returns>A valid branch code.</returns>
    public static string GenerateValidCode()
    {
        return new Faker().Random.AlphaNumeric(5).ToUpper();
    }

    /// <summary>
    /// Generates a valid city name.
    /// </summary>
    /// <returns>A valid city name.</returns>
    public static string GenerateValidCity()
    {
        return new Faker().Address.City();
    }

    /// <summary>
    /// Generates a valid Brazilian state abbreviation.
    /// </summary>
    /// <returns>A valid state abbreviation (e.g., SP, RJ, MG).</returns>
    public static string GenerateValidState()
    {
        return new Faker().PickRandom(new[]
        {
            "SP","RJ","MG","BA","RS","PR","SC","CE","GO","PE","AM","PA","MT","MS","ES"
        });
    }


    /// <summary>
    /// Generates an invalid empty name.
    /// </summary>
    public static string GenerateInvalidName() => string.Empty;

    /// <summary>
    /// Generates an invalid code (too short).
    /// </summary>
    public static string GenerateInvalidCode() =>
        new Faker().Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

    /// <summary>
    /// Generates a numeric-only invalid city.
    /// </summary>
    public static string GenerateInvalidCity() =>
        new Faker().Random.Number(10000, 99999).ToString();

    /// <summary>
    /// Generates an invalid state abbreviation (wrong length).
    /// </summary>
    public static string GenerateInvalidState() =>
        new Faker().Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

    /// <summary>
    /// Generates a list of valid Branch entities.
    /// </summary>
    /// <param name="count">Number of branches to generate.</param>
    /// <returns>A list of valid <see cref="Branch"/> instances.</returns>
    public static List<Branch> GenerateList(int count)
    {
        return BranchFaker.Generate(count);
    }
}
