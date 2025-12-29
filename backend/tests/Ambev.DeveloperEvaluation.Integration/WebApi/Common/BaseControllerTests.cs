using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Common;

/// <summary>
/// Integration tests for <see cref="BaseController"/> helper methods.
/// </summary>
public class BaseControllerTests : IntegrationTestBase
{
    public BaseControllerTests(IntegrationTestFactory factory)
        : base(factory) { }

    private static TestController CreateTestControllerWithUser(int userId = 123, string email = "test@example.com")
    {
        var controller = new TestController();
        var claims = new List<Claim>
    {
        new (ClaimTypes.NameIdentifier, userId.ToString()),
        new (ClaimTypes.Email, email)
    };
        var identity = new ClaimsIdentity(claims);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };
        return controller;
    }

    #region GetCurrentUserId / GetCurrentUserEmail
    [Fact(DisplayName = "GetCurrentUserId should return user ID from claims")]
    public void GetCurrentUserId_ShouldReturnUserIdFromClaims()
    {
        // Given
        var controller = CreateTestControllerWithUser(42);

        // When
        var userId = controller.GetUserIdPublic();

        // Then
        userId.Should().Be(42);
    }

    [Fact(DisplayName = "GetCurrentUserEmail should return email from claims")]
    public void GetCurrentUserEmail_ShouldReturnEmailFromClaims()
    {
        // Given
        var controller = CreateTestControllerWithUser(email: "test@example.com");

        // When
        var email = controller.GetUserEmailPublic();

        // Then
        email.Should().Be("test@example.com");
    }

    [Fact(DisplayName = "GetCurrentUserId should throw when claim missing")]
    public void GetCurrentUserId_ShouldThrow_WhenClaimMissing()
    {
        // Given
        var controller = new TestController();

        // When / Then
        FluentActions.Invoking(() => controller.GetUserIdPublic())
            .Should()
            .Throw<NullReferenceException>();
    }

    [Fact(DisplayName = "GetCurrentUserEmail should throw when claim missing")]
    public void GetCurrentUserEmail_ShouldThrow_WhenClaimMissing()
    {
        // Given
        var controller = new TestController();

        // When / Then
        FluentActions.Invoking(() => controller.GetUserEmailPublic())
            .Should()
            .Throw<NullReferenceException>();
    }
    #endregion

    #region Ok / Created / BadRequest / NotFound
    [Fact(DisplayName = "Ok should wrap data in ApiResponseWithData")]
    public void Ok_ShouldWrapDataInApiResponseWithData()
    {
        // Given
        var controller = new TestController();

        // When
        var result = controller.OkPublic("hello", "msg");

        // Then
        var okResult = result.Should().BeOfType<OkObjectResult>().Which;
        var wrapper = okResult.Value.Should().BeAssignableTo<ApiResponseWithData<string>>().Which;
        wrapper.Data.Should().Be("hello");
        wrapper.Message.Should().Be("msg");
        wrapper.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "Created should return CreatedAtRouteResult with ApiResponseWithData")]
    public void Created_ShouldWrapDataInApiResponseWithData()
    {
        // Given
        var controller = new TestController();
        var data = "hello";

        // When
        var result = controller.CreatedPublic("RouteName", new { id = 1 }, data);

        // Then
        var createdResult = result.Should().BeOfType<CreatedAtRouteResult>().Which;
        var wrapper = createdResult.Value.Should().BeAssignableTo<ApiResponseWithData<string>>().Which;
        wrapper.Data.Should().Be("hello");
        wrapper.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "BadRequest should return BadRequestObjectResult")]
    public void BadRequest_ShouldReturnApiResponseWithSuccessFalse()
    {
        // Given
        var controller = new TestController();

        // When
        var result = controller.BadRequestPublic("error");

        // Then
        var badResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
        var apiResponse = badResult.Value.Should().BeAssignableTo<ApiResponse>().Which;
        apiResponse.Success.Should().BeFalse();
        apiResponse.Message.Should().Be("error");
    }

    [Fact(DisplayName = "NotFound should return NotFoundObjectResult")]
    public void NotFound_ShouldReturnApiResponseWithSuccessFalse()
    {
        // Given
        var controller = new TestController();

        // When
        var result = controller.NotFoundPublic("not found");

        // Then
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Which;
        var apiResponse = notFoundResult.Value.Should().BeAssignableTo<ApiResponse>().Which;
        apiResponse.Success.Should().BeFalse();
        apiResponse.Message.Should().Be("not found");
    }

    [Fact(DisplayName = "OkPaginated should return ApiResponseWithData containing PaginatedResponse")]
    public void OkPaginated_ShouldReturnPaginatedResponse()
    {
        // Given
        var controller = new TestController();
        var items = new List<string> { "A", "B", "C" };
        var paginatedList = new PaginatedResponse<string>(items, 1, 1, 3);

        // When
        var result = controller.OkPaginatedPublic(paginatedList);

        // Then
        var okResult = result.Should().BeOfType<OkObjectResult>().Which;
        var response = okResult.Value.Should().BeAssignableTo<PaginatedResponse<string>>().Which;

        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(items);
        response.CurrentPage.Should().Be(1);
        response.TotalPages.Should().Be(1);
        response.TotalCount.Should().Be(3);
    }
    #endregion

    #region PRIVATE TEST CONTROLLER
    /// <summary>
    /// Controller used solely for testing protected BaseController methods.
    /// </summary>
    private class TestController : BaseController
    {
        public int GetUserIdPublic() => GetCurrentUserId();
        public string GetUserEmailPublic() => GetCurrentUserEmail();
        public IActionResult OkPublic<T>(T data, string message = "") => base.Ok(data, message);
        public IActionResult CreatedPublic<T>(string routeName, object routeValues, T data) => base.Created(routeName, routeValues, data);
        public IActionResult BadRequestPublic(string message) => base.BadRequest(message);
        public IActionResult NotFoundPublic(string message = "Resource not found") => base.NotFound(message);
        public IActionResult OkPaginatedPublic<T>(PaginatedResponse<T> pagedList) => base.OkPaginated(pagedList);
    }
    #endregion
}
