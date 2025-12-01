using Ambev.DeveloperEvaluation.Integration.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Auth;

/// <summary>
/// Contains integration tests for the <see cref="AuthController"/> class.
/// These tests validate the authentication flow using the real Web API pipeline,
/// dependency injection, and initial seeded user credentials.
/// </summary>
public class AuthControllerTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IServiceProvider _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthControllerTests"/> class.
    /// Sets up the Web API integration test environment using <see cref="IntegrationTestFactory"/>,
    /// which provides access to the application's configured services.
    /// </summary>
    /// <param name="factory">The test application factory that configures the API host for integration tests.</param>
    public AuthControllerTests(IntegrationTestFactory factory)
    {
        _services = factory.Services;
    }

    /// <summary>
    /// Tests that authentication succeeds and returns a valid token when valid credentials are supplied.
    /// This test calls the controller method directly, leveraging dependency injection
    /// to create the <see cref="AuthController"/> instance with real services.
    /// </summary>
    [Fact(DisplayName = "AuthenticateUser should return token when credentials are valid")]
    public async Task AuthenticateUser_DirectMethod_ShouldReturnToken()
    {
        // Guiven
        using var scope = _services.CreateScope();

        var controller = ActivatorUtilities.CreateInstance<AuthController>(scope.ServiceProvider);

        var request = new AuthenticateUserRequest
        {
            Email = IntegrationTestConstants.InitialUserEmail,
            Password = IntegrationTestConstants.InitialUserPassword
        };

        // When
        var result = await controller.AuthenticateUser(request, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        var response = ok.Value.Should()
            .BeAssignableTo<ApiResponseWithData<AuthenticateUserResponse>>()
            .Which;

        response.Success.Should().BeTrue();
        response.Data!.Token.Should().NotBeNullOrWhiteSpace();
    }
}
