using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;

/// <summary>
/// Provides methods for generating test data for the UpdateUserHandler.
/// This centralizes creation of valid and invalid UpdateUserCommand instances.
/// </summary>
public static class UpdateUserHandlerTestData
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Generates a valid UpdateUserCommand with realistic Bogus data.
    /// </summary>
    public static UpdateUserCommand GenerateValidCommand()
    {
        return new UpdateUserCommand(
            id: Guid.NewGuid(),
            username: _faker.Internet.UserName(),
            phone: _faker.Phone.PhoneNumber("+55 ## #####-####"),
            email: _faker.Internet.Email(),
            status: _faker.PickRandom<UserStatus>(),
            role: _faker.PickRandom<UserRole>()
        );
    }

    /// <summary>
    /// Generates an invalid UpdateUserCommand with purposefully incorrect data.
    /// </summary>
    public static UpdateUserCommand GenerateInvalidCommand()
    {
        return new UpdateUserCommand(
            id: Guid.Empty,
            username: "", 
            phone: "invalid-phone",
            email: "not-an-email",
            status: 0,
            role: 0
        );
    }
}
