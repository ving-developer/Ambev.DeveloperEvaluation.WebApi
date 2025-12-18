using Ambev.DeveloperEvaluation.Application.Commands.Auth.AuthenticateUser;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Auth;

/// <summary>
/// Provides methods for generating test data for AuthenticateUserHandler.
/// Centralizes creation of valid and invalid AuthenticateUserCommand instances.
/// </summary>
public static class AuthenticateUserHandlerTestData
{
    private static readonly Faker<AuthenticateUserCommand> authenticateUserFaker = new Faker<AuthenticateUserCommand>()
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Password, f => $"Test@{f.Random.Number(100, 999)}");

    /// <summary>
    /// Generates a valid AuthenticateUserCommand with random email and password.
    /// </summary>
    /// <returns>A valid AuthenticateUserCommand instance.</returns>
    public static AuthenticateUserCommand GenerateValidCommand()
    {
        return authenticateUserFaker.Generate();
    }
}
