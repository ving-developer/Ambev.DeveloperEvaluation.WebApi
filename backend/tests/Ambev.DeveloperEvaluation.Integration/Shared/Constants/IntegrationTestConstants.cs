using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Integration.Shared.Constants;

/// <summary>
/// Provides constant values used across integration tests,
/// ensuring consistency and avoiding duplication of test data.
/// </summary>
public class IntegrationTestConstants
{
    /// <summary>
    /// The default ID of the initial seeded user created specifically
    /// for authentication-related integration tests.
    /// </summary>
    public static readonly Guid InitialUserId = Guid.NewGuid();

    /// <summary>
    /// The default password of the initial seeded user created specifically
    /// for authentication-related integration tests.
    /// </summary>
    public const string InitialUserPassword = "Pass@word";

    /// <summary>
    /// The default email address of the initial seeded user created specifically
    /// for authentication-related integration tests.
    /// </summary>
    public const string InitialUserEmail = "admin@test.com";

    /// <summary>
    /// The default ID of the initial seeded branch created specifically
    /// for branch-related integration tests.
    /// </summary>
    public static readonly Guid InitialBranchId = Guid.NewGuid();

    /// <summary>
    /// The default name of the initial seeded branch created specifically
    /// for branch-related integration tests.
    /// </summary>
    public const string InitialBranchName = "Filial Centro";

    /// <summary>
    /// The default code of the initial seeded branch created specifically
    /// for branch-related integration tests.
    /// </summary>
    public const string InitialBranchCode = "SP-42";

    /// <summary>
    /// The default city of the initial seeded branch created specifically
    /// for branch-related integration tests.
    /// </summary>
    public const string InitialBranchCity = "São Paulo";

    /// <summary>
    /// The default state (UF) of the initial seeded branch created specifically
    /// for branch-related integration tests.
    /// </summary>
    public const string InitialBranchState = "SP";

    /// <summary>
    /// The default ID of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public static readonly Guid InitialProductId = Guid.NewGuid();

    /// <summary>
    /// The default title of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const string InitialProductTitle = "Cerveja Skol 350ml";

    /// <summary>
    /// The default price of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const decimal InitialProductPrice = 4.50m;

    /// <summary>
    /// The default description of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const string InitialProductDescription = "Cerveja Skol Pilsen 350ml lata";

    /// <summary>
    /// The default category of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const ProductCategory InitialProductCategory = ProductCategory.Clothing;

    /// <summary>
    /// The default image URL of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const string InitialProductImage = "https://example.com/images/skol-350ml.jpg";

    /// <summary>
    /// The default rating value of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const decimal InitialProductRating = 4.5m;

    /// <summary>
    /// The default rating count of the initial seeded product created specifically
    /// for product-related integration tests.
    /// </summary>
    public const int InitialProductRatingCount = 125;
}