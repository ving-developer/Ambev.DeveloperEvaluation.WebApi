using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData.Users;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
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
}
