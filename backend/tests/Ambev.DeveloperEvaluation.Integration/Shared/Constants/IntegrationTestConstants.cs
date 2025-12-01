namespace Ambev.DeveloperEvaluation.Integration.Shared.Constants;

/// <summary>
/// Provides constant values used across integration tests,
/// ensuring consistency and avoiding duplication of test data.
/// </summary>
public class IntegrationTestConstants
{
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
}
