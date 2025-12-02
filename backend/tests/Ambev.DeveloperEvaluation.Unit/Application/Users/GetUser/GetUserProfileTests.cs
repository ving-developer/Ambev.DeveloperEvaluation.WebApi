using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users.GetUser
{
    /// <summary>
    /// Contains unit tests for the <see cref="GetUserProfile"/> AutoMapper profile.
    /// </summary>
    public class GetUserProfileTests
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes AutoMapper with the <see cref="GetUserProfile"/> for testing.
        /// </summary>
        public GetUserProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetUserProfile>();
            });

            // Validates that the profile configuration is correct
            configuration.AssertConfigurationIsValid();

            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// Tests that mapping from <see cref="User"/> to <see cref="UserResult"/> works correctly.
        /// </summary>
        [Fact(DisplayName = "User to GetUserResult mapping should map properties correctly")]
        public void User_To_GetUserResult_Mapping_ShouldMapPropertiesCorrectly()
        {
            // Given
            var user = UserTestData.GenerateValidUser();

            // When
            var result = _mapper.Map<UserResult>(user);

            // Then
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Username);
            result.Id.Should().Be(user.Id);
            result.Email.Should().Be(user.Email);
            result.Phone.Should().Be(user.Phone);
            result.Status.Should().Be(user.Status);
            result.Role.Should().Be(user.Role);
        }
    }
}
