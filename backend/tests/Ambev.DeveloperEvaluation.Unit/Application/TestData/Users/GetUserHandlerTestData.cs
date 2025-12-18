using Ambev.DeveloperEvaluation.Application.Queries.Users.GetUserById;

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
    public static GetUserByIdQuery GenerateValidCommand()
    {
        return new GetUserByIdQuery(Guid.NewGuid());
    }

    /// <summary>
    /// Generates an invalid GetUserCommand (e.g., empty Guid) for testing validation failures.
    /// </summary>
    /// <returns>An invalid GetUserCommand instance.</returns>
    public static GetUserByIdQuery GenerateInvalidCommand()
    {
        return new GetUserByIdQuery(Guid.Empty);
    }
}
