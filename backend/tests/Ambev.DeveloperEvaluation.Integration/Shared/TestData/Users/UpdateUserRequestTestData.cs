using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users
{
    /// <summary>
    /// Provides methods for generating test data for UpdateUserRequest
    /// </summary>
    internal class UpdateUserRequestTestData
    {
        /// <summary>
        /// Generates a valid UpdateUserRequest entity with randomized data.
        /// </summary>
        public static UpdateUserRequest GetValidUpdateUserRequest()
        {
            return new Faker<UpdateUserRequest>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Phone, f => f.Random.ReplaceNumbers("119########"))
                .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
                .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin))
                .Generate();
        }

        /// <summary>
        /// Generates invalid UpdateUserRequest with empty fields.
        /// </summary>
        public static UpdateUserRequest GetInvalidUpdateUserRequest()
        {
            return new UpdateUserRequest
            {
                Username = "",
                Email = "",
                Phone = ""
            };
        }
    }
}
