using Ambev.DeveloperEvaluation.Application.Branches.Common;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Integration.Shared.Fixtures;
using Ambev.DeveloperEvaluation.Integration.Shared.Helpers;
using Ambev.DeveloperEvaluation.Integration.Shared.TestData.Branches;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Branches;

/// <summary>
/// Integration tests for <see cref="BranchesController"/> using the real DI pipeline
/// and real mediator handlers.
/// </summary>
public class BranchesControllerTests : IntegrationTestBase
{
    public BranchesControllerTests(IntegrationTestFactory factory)
        : base(factory) { }

    #region /POST CREATE BRANCH
    [Fact(DisplayName = "CreateBranch should return Created when request is valid")]
    public async Task CreateBranch_ShouldReturnCreated()
    {
        // Given
        var controller = CreateController<BranchesController>();
        var request = CreateBranchRequestTestData.GetValidCreateBranchRequest();

        // When
        var result = await controller.CreateBranch(request, default);

        // Then
        var createdBranch = ExtractCreatedBranchResponse(result);
        createdBranch.Id.Should().NotBeEmpty();
        createdBranch.Name.Should().Be(request.Name);
    }
    #endregion

    #region /GET/{id} GET BRANCH
    [Fact(DisplayName = "GetBranch should return branch when branch exists")]
    public async Task GetBranch_ShouldReturnBranch()
    {
        // Given
        var controller = CreateController<BranchesController>();
        var createdBranch = await CreateSampleBranch(controller);

        // When
        var result = await controller.GetBranch(createdBranch.Id, default);

        // Then
        var ok = result.Should().BeOfType<OkObjectResult>().Which;
        var response = ok.Value.Should().BeAssignableTo<ApiResponseWithData<BranchResponse>>().Which;
        response.Data!.Id.Should().Be(createdBranch.Id);
    }

    [Fact(DisplayName = "GetBranch should throw KeyNotFoundException when branch does not exist")]
    public async Task GetBranch_ShouldThrowKeyNotFoundException()
    {
        // Given
        var controller = CreateController<BranchesController>();
        var nonExistingBranchId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.GetBranch(nonExistingBranchId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Branch with ID {nonExistingBranchId} not found");
    }
    #endregion

    #region /GET LIST BRANCHES
    [Fact(DisplayName = "ListBranches should return paginated branches")]
    public async Task ListBranches_ShouldReturnPaginatedBranches()
    {
        // Given
        var controller = CreateController<BranchesController>();
        await CreateSampleBranch(controller);
        var request = new ListBranchesRequest
        {
            Page = 1,
            Size = 10
        };

        // When
        var result = await controller.ListBranches(request, default);

        // Then
        var ok = result.Should().BeOfType<OkObjectResult>().Which;
        var response = ok.Value.Should().BeAssignableTo<PaginatedResponse<BranchResult>>().Which;
        response.Data.Should().NotBeEmpty();
    }
    #endregion

    #region /DELETE/{id} DELETE BRANCH
    [Fact(DisplayName = "DeleteBranch should return Ok when branch exists")]
    public async Task DeleteBranch_ShouldReturnOk()
    {
        // Given
        var controller = CreateController<BranchesController>();
        var createdBranch = await CreateSampleBranch(controller);

        // When
        var result = await controller.DeleteBranch(createdBranch.Id, default);

        // Then
        var ok = result.Should().BeOfType<OkObjectResult>().Which;
        var response = ok.Value.Should().BeAssignableTo<ApiResponse>().Which;
        response.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "DeleteBranch should throw KeyNotFoundException when branch does not exist")]
    public async Task DeleteBranch_ShouldThrowKeyNotFoundException()
    {
        // Given
        var controller = CreateController<BranchesController>();
        var nonExistingBranchId = Guid.NewGuid();

        // When & Then
        await FluentActions
            .Invoking(() => controller.DeleteBranch(nonExistingBranchId, default))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("Key to delete has not found.");
    }
    #endregion

    #region PRIVATE HELPERS
    private static BranchResponse ExtractCreatedBranchResponse(IActionResult result)
    {
        var createdObject = result as CreatedAtRouteResult;
        var responseWrapper = createdObject!.Value as ApiResponseWithData<BranchResponse>;
        return responseWrapper!.Data!;
    }

    private static async Task<BranchResponse> CreateSampleBranch(BranchesController controller)
    {
        var request = CreateBranchRequestTestData.GetValidCreateBranchRequest();
        var result = await controller.CreateBranch(request, default);
        return ExtractCreatedBranchResponse(result);
    }
    #endregion
}
