using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;

/// <summary>
/// Provides methods for generating test data for the DeleteUserHandler.
/// This centralizes creation of valid and invalid DeleteUserCommand instances.
/// </summary>
public static class DeleteUserHandlerTestData
{

    /// <summary>
    /// Generates a valid DeleteUserCommand with a randomized user ID.
    /// </summary>
    /// <returns>A valid DeleteUserCommand instance.</returns>
    public static DeleteUserCommand GenerateValidCommand()
    {
        return new DeleteUserCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates an invalid DeleteUserCommand (e.g., empty Guid) for testing validation failures.
    /// </summary>
    /// <returns>An invalid DeleteUserCommand instance.</returns>
    public static DeleteUserCommand GenerateInvalidCommand()
    {
        return new DeleteUserCommand(Guid.Empty);
    }
}
