using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes CreateUserRequestTestData test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
internal class CreateUserRequestTestData
{
    /// <summary>
    /// Generates a valid CreateUserRequest entity with randomized data.
    /// </summary>
    public static CreateUserRequest GetValidCreateUserRequest()
    {
        return new Faker<CreateUserRequest>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
            .RuleFor(u => u.Phone, f => f.Random.ReplaceNumbers("119########"))
            .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
            .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
            .Generate();
    }

    /// <summary>
    /// Generates invalid CreateUserRequest with empty fields.
    /// </summary>
    public static CreateUserRequest GetInvalidCreateUserRequest()
    {
        return new CreateUserRequest
        {
            Username = "",
            Email = "",
            Password = "",
            Phone = ""
        };
    }
}
