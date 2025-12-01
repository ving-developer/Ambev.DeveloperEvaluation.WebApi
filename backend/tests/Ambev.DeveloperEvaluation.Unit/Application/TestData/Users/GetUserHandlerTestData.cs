using Ambev.DeveloperEvaluation.Application.Users.GetUser;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;

/// <summary>
/// Provides methods for generating test data for the GetUserHandler.
/// This centralizes creation of valid and invalid GetUserCommand instances.
/// </summary>
public static class GetUserHandlerTestData
{
    /// <summary>
    /// Generates a valid GetUserCommand with a randomized user ID.
    /// </summary>
    /// <returns>A valid GetUserCommand instance.</returns>
    public static GetUserCommand GenerateValidCommand()
    {
        return new GetUserCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates an invalid GetUserCommand (e.g., empty Guid) for testing validation failures.
    /// </summary>
    /// <returns>An invalid GetUserCommand instance.</returns>
    public static GetUserCommand GenerateInvalidCommand()
    {
        return new GetUserCommand(Guid.Empty);
    }
}
