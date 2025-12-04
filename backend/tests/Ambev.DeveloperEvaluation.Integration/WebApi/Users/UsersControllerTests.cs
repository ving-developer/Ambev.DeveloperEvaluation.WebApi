using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Integration.Shared.Constants;
using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Users;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
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
        var controller = CreateController<UsersController>();
        var request = CreateUserRequestTestData.GetValidCreateUserRequest();

        var result = await controller.CreateUser(request, default);

        var createdUser = ExtractCreatedUserResponse(result);
        createdUser.Id.Should().NotBeEmpty();
    }
    #endregion

    #region /GET/{id} GET USER
    [Fact(DisplayName = "GetUser should return user when user exists")]
    public async Task GetUser_ShouldReturnUser()
    {
        var controller = CreateController<UsersController>();

        var result = await controller.GetUser(IntegrationTestConstants.InitialUserId, default);

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
        var controller = CreateController<UsersController>();
        var nonExistingUserId = Guid.NewGuid();

        await FluentActions
            .Invoking(() => controller.GetUser(nonExistingUserId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {nonExistingUserId} not found");
    }
    #endregion

    #region /PUT/{id} UPDATE USER
    [Fact(DisplayName = "UpdateUser should return updated user when request is valid")]
    public async Task UpdateUser_ShouldReturnUpdatedUser()
    {
        var controller = CreateController<UsersController>();
        var createRequest = CreateUserRequestTestData.GetValidCreateUserRequest();
        var createdResult = await controller.CreateUser(createRequest, default);
        var createdUser = ExtractCreatedUserResponse(createdResult);

        var updateRequest = new UpdateUserRequest
        {
            Username = "Updated_" + createdUser.Name,
            Email = "updated_" + createdUser.Email,
            Phone = "11900000000",
            Status = createdUser.Status,
            Role = createdUser.Role
        };

        var result = await controller.UpdateUser(createdUser.Id, updateRequest, default);

        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;
        var responseWrapper = ok.Value.Should()
            .BeAssignableTo<ApiResponseWithData<UserResponse>>()
            .Which;
        var updatedUser = responseWrapper.Data!;

        updatedUser.Id.Should().Be(createdUser.Id);
        updatedUser.Name.Should().StartWith("Updated_");
    }

    [Fact(DisplayName = "UpdateUser should throw KeyNotFoundException when user does not exist")]
    public async Task UpdateUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var controller = CreateController<UsersController>();
        var nonExistingUserId = Guid.NewGuid();
        var updateRequest = UpdateUserRequestTestData.GetValidUpdateUserRequest();

        // Act & Assert
        await FluentActions
            .Invoking(() => controller.UpdateUser(nonExistingUserId, updateRequest, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>();
    }
    #endregion

    #region /GET LIST USERS
    [Fact(DisplayName = "ListUsers should return paginated list of users")]
    public async Task ListUsers_ShouldReturnPaginatedUsers()
    {
        var controller = CreateController<UsersController>();
        var request = new ListUsersRequest
        {
            Page = 1,
            Size = 10
        };

        var result = await controller.ListUsers(request, default);

        var ok = result.Should()
            .BeOfType<OkObjectResult>()
            .Which;

        var paginated = ok.Value.Should()
            .BeAssignableTo<PaginatedResponse<UserResult>>()
            .Which;

        paginated.Data.Should().NotBeNull();
        paginated.CurrentPage.Should().Be(1);
        paginated.Data.Count.Should().BeLessOrEqualTo(10);
    }
    #endregion

    #region /DELETE/{id} DELETE USER
    [Fact(DisplayName = "DeleteUser should return Ok when user exists")]
    public async Task DeleteUser_ShouldReturnOk()
    {
        var controller = CreateController<UsersController>();
        var createRequest = CreateUserRequestTestData.GetValidCreateUserRequest();
        var createdResult = await controller.CreateUser(createRequest, default);
        var createdUser = ExtractCreatedUserResponse(createdResult);

        var result = await controller.DeleteUser(createdUser.Id, default);

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
        var controller = CreateController<UsersController>();
        var nonExistentUserId = Guid.NewGuid();

        await FluentActions
            .Invoking(() => controller.DeleteUser(nonExistentUserId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Key to delete has not found.");
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
