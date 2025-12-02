using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Users;

/// <summary>
/// Integration tests for <see cref="UsersController"/> using the real DI pipeline
/// and real mediator handlers.
/// </summary>
public class UsersControllerTests : IntegrationTestBase
{
    public UsersControllerTests(IntegrationTestFactory factory)
        : base(factory) { }

    #region /POST CREATE USER
    [Fact(DisplayName = "CreateUser should return Created when request is valid")]
    public async Task CreateUser_ShouldReturnCreated()
    {
        // Guiven
        var controller = CreateController<UsersController>();
        var request = CreateUserRequestTestData.GetValidCreateUserRequest();

        // When
        var result = await controller.CreateUser(request, default);

        // Then
        var createdUser = ExtractCreatedUserResponse(result);
        createdUser.Id.Should().NotBeEmpty();
    }
    #endregion

    #region /GET/{id} GET USER
    [Fact(DisplayName = "GetUser should return user when user exists")]
    public async Task GetUser_ShouldReturnUser()
    {
        // Guiven
        var controller = CreateController<UsersController>();

        // When
        var result = await controller.GetUser(IntegrationTestConstants.InitialUserId, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        var response = ok.Value.Should()
            .BeAssignableTo<ApiResponseWithData<UserResponse>>()
            .Which;

        response.Data!.Id.Should().Be(IntegrationTestConstants.InitialUserId);
    }

    [Fact(DisplayName = "GetUser should throw KeyNotFoundException when user does not exist")]
    public async Task GetUser_ShouldThrowKeyNotFoundException()
    {
        // Given
        var controller = CreateController<UsersController>();
        var nonExistingUserId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.GetUser(nonExistingUserId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {nonExistingUserId} not found");
    }
    #endregion

    #region /DELETE/{id} DELETE USER
    [Fact(DisplayName = "DeleteUser should return Ok when user exists")]
    public async Task DeleteUser_ShouldReturnOk()
    {
        // Guiven
        var controller = CreateController<UsersController>();
        var createRequest = CreateUserRequestTestData.GetValidCreateUserRequest();

        var createdResult = await controller.CreateUser(createRequest, default);
        var createdResponse = ExtractCreatedUserResponse(createdResult);

        // When
        var result = await controller.DeleteUser(createdResponse.Id, default);

        // Then
        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        ok.Value.Should()
            .BeAssignableTo<ApiResponse>()
            .Which.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "DeleteUser should throw KeyNotFoundException when user does not exist")]
    public async Task DeleteUser_ShouldThrowKeyNotFoundException()
    {
        // Given
        var controller = CreateController<UsersController>();
        var nonExistentUserId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.DeleteUser(nonExistentUserId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {nonExistentUserId} not found");
    }
    #endregion

    #region PRIVATE METHODS
    private static UserResponse ExtractCreatedUserResponse(IActionResult result)
    {
        var createdObject = result as CreatedAtRouteResult;

        var responseWrapper = createdObject!.Value as ApiResponseWithData<UserResponse>;

        return responseWrapper!.Data!;
    }
    #endregion
}
