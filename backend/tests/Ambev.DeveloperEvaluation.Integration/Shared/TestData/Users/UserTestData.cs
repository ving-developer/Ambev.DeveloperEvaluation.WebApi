using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes UserTestData test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class UserTestData
{
    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// </summary>
    public static User GetValidUser()
    {
        return new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("(11) 9####-####"))
            .RuleFor(u => u.Status, _ => UserStatus.Active)
            .RuleFor(u => u.Role, _ => UserRole.Admin)
            .Generate();
    }

    /// <summary>
    /// Generates an initial user with predefined constants for integration tests.
    /// This user has fixed email, password, and ID as specified in IntegrationTestConstants.
    /// Note: The password is NOT hashed - you need to hash it separately using your password hasher.
    /// </summary>
    /// <returns>A user entity with predefined test constants.</returns>
    public static User GetInitialUser()
    {
        var user = GetValidUser();

        user.SetEmail(IntegrationTestConstants.InitialUserEmail);
        user.SetPassword(IntegrationTestConstants.InitialUserPassword);
        user.Id = IntegrationTestConstants.InitialUserId;

        return user;
    }

    /// <summary>
    /// Generates an initial user with predefined constants and applies password hashing.
    /// </summary>
    /// <param name="hasher">Password hasher instance to hash the initial password.</param>
    /// <returns>A user entity with predefined test constants and hashed password.</returns>
    public static User GetInitialUser(IPasswordHasher hasher)
    {
        var user = GetInitialUser();
        user.SetPassword(hasher.HashPassword(IntegrationTestConstants.InitialUserPassword));
        return user;
    }
}
