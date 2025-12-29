using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Auth;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Auth;

/// <summary>
/// Contains integration tests for the <see cref="AuthController"/> class.
/// These tests validate the authentication flow using the real Web API pipeline,
/// dependency injection, and initial seeded user credentials.
/// </summary>
public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(IntegrationTestFactory factory)
        : base(factory) { }

    [Fact(DisplayName = "AuthenticateUser should return token when credentials are valid")]
    public async Task AuthenticateUser_ShouldReturnToken()
    {
        // Guiven
        var controller = CreateController<AuthController>();
        var request = AuthTestData.GetValidSeededUserRequest();

        // When
        var result = await controller.AuthenticateUser(request, default);

        // Then
        var response = result.Should()
            .BeOfType<OkObjectResult>().Which.Value
            .Should().BeAssignableTo<ApiResponseWithData<AuthenticateUserResponse>>()
            .Which;

        response.Success.Should().BeTrue();
        response.Data!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = "AuthenticateUser should return BadRequest when request is invalid")]
    public async Task AuthenticateUser_ShouldReturnBadRequest()
    {
        // Guiven
        var controller = CreateController<AuthController>();
        var request = AuthTestData.GetInvalidRequest();

        // When
        var result = await controller.AuthenticateUser(request, default);

        // Then
        var badRequest = result.Should()
            .BeOfType<BadRequestObjectResult>()
            .Which;

        badRequest.Value.Should().BeAssignableTo<IEnumerable<object>>()
            .Which.Should().NotBeEmpty();
    }
}
