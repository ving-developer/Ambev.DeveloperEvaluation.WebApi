using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Users
{
    /// <summary>
    /// Provides methods for generating <see cref="CreateUserResult"/> test data.
    /// This centralizes creation of result objects from a given <see cref="User"/> entity,
    /// ensuring consistency across tests.
    /// </summary>
    public static class CreateUserResultTestData
    {
        /// <summary>
        /// Generates a <see cref="CreateUserResult"/> from a given <see cref="User"/>.
        /// All relevant fields are mapped from the user entity.
        /// </summary>
        /// <param name="user">The user entity to generate the result from.</param>
        /// <returns>A <see cref="CreateUserResult"/> populated with data from the user.</returns>
        public static CreateUserResult GenerateFromUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return new CreateUserResult
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                Role = user.Role
            };
        }
    }
}
