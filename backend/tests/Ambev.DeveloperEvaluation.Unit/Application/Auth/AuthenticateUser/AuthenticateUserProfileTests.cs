using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth.AuthenticateUser
{
    /// <summary>
    /// Unit tests for <see cref="AuthenticateUserProfile"/> AutoMapper configuration.
    /// Ensures that mapping from <see cref="User"/> to <see cref="AuthenticateUserResult"/> works correctly.
    /// </summary>
    public class AuthenticateUserProfileTests
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes the mapper with the <see cref="AuthenticateUserProfile"/> profile.
        /// Also asserts that the AutoMapper configuration is valid.
        /// </summary>
        public AuthenticateUserProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AuthenticateUserProfile>();
            });

            config.AssertConfigurationIsValid();

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Tests that a User entity is correctly mapped to AuthenticateUserResult.
        /// Validates that Role is converted to string and Token is ignored.
        /// </summary>
        [Fact(DisplayName = "User to AuthenticateUserResult mapping should map properties correctly")]
        public void Map_UserToAuthenticateUserResult_ShouldMapPropertiesCorrectly()
        {
            // Given
            var user = UserTestData.GenerateValidUser();

            // When
            var result = _mapper.Map<AuthenticateUserResult>(user);

            // Then
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Username);
            result.Email.Should().Be(user.Email);
            result.Phone.Should().Be(user.Phone);
            result.Role.Should().Be(user.Role.ToString());
            result.Token.Should().Be(string.Empty);
        }
    }
}
